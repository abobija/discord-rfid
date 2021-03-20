using Discord;
using Discord.Net;
using Discord.Rest;
using Discord.WebSocket;
using DiscordRfid.Com;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DiscordRfid
{
    public class Bot
    {
        protected static Bot Singletone;
        private static DiscordSocketClient Client;

        public string Name => Client?.CurrentUser?.Username;
        public SocketGuild Guild => Client.Guilds.ElementAt(0);
        public IRole MasterRole;
        public IRole SlaveRole;
        public ITextChannel Channel;

        public event Func<string, Exception, string> AuthenticationError;
        public event Action<Exception> EnvironmentCreationError;

        public event Action Connected;
        public event Action Ready;
        public event Action<Package> NewPackage;

        public Func<bool> RolesCreationPrompter;
        public Func<bool> ChannelCreationPrompter;

        protected Bot()
        {
            Log.Verbose("Bot constructor");

            Database.Instance.Init();

            Client = new DiscordSocketClient();

            Client.Log += DiscordLog;
            Client.Rest.Log += DiscordLog;

            Client.Connected += () =>
            {
                Connected?.Invoke();
                return Extensions.NoopTask();
            };

            Client.GuildAvailable += async g =>
            {
                Log.Debug($"Guild available");

                MasterRole = Guild.Roles.FirstOrDefault(r => r.Name == Configuration.MasterRoleName);
                SlaveRole = Guild.Roles.FirstOrDefault(r => r.Name == Configuration.SlaveRoleName);
                Channel = Guild.TextChannels.FirstOrDefault(c => c.Name == Configuration.ChannelName);

                try
                {
                    await CheckRolesAsync();
                    await CheckChannelAsync();
                    Ready?.Invoke();
                } catch(Exception ex)
                {
                    Log.Error(ex, "Fail to create environment");
                    EnvironmentCreationError?.Invoke(ex);
                }
            };

            Client.MessageReceived += OnMessageReceivedAsync;
        }

        private async Task OnMessageReceivedAsync(SocketMessage msg)
        {
            var pckg = await ParseMessageAsync(msg);
                
            if(pckg != null)
            {
                NewPackage?.Invoke(pckg);
            }
        }

        private async Task<Package> ParseMessageAsync(IMessage msg, ICollection<RestGuildUser> cache = null)
        {
            Package pckg = null;
            Exception exception = null;

            if(msg == null)
            {
                goto _return;
            }

            if(msg.Channel.Id != Channel.Id || msg.Type != MessageType.Default // listen only for default msgs on rfid channel
                || msg.Author.Id == Client.CurrentUser.Id) // ignore self messages
            {
                goto _return;
            }

#if !DEBUG
            if(! msg.Author.IsBot)
            {
                goto _return;
            }
#endif

            var member = cache?.FirstOrDefault(u => u.Id == msg.Author.Id);
                
            if(member == null)
            {
                Log.Debug("Fetching member for roles checking");
                member = await Client.Rest.GetGuildUserAsync(Guild.Id, msg.Author.Id);
                cache?.Add(member);
            }

            if (member.RoleIds.FirstOrDefault(id => id == SlaveRole.Id) == default)
            {
                Log.Warning("Message is not from slave");
                goto _return;
            }

            try
            {
                pckg = Package.FromDiscordMessage(msg);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            
            _return:
            if(pckg == null)
            {
                if (msg != null)
                {
                    Log.Debug($"Message has not parsed (Author={msg.Author}, IsBot={msg.Author.IsBot}, Content={msg.Content})");
                }

                if(exception != null)
                {
                    Log.Warning(exception, $"Fail to parse discord message into package");
                }
            }
            return pckg;
        }

        public async Task<List<Package>> LoadRecentPackagesAsync()
        {
            var pckgs = new List<Package>();
            var cache = new List<RestGuildUser>();

            foreach (var m in await Channel.GetMessagesAsync(Configuration.RecentPackagesLoadLimit).FlattenAsync())
            {
                var pckg = await ParseMessageAsync(m, cache);

                if(pckg != null)
                {
                    pckgs.Add(pckg);
                }
            }

            cache.Clear();
            return pckgs;
        }

        private Task DiscordLog(LogMessage lmsg)
        {
            Log.Debug($"[Discord] {lmsg}");
            return Extensions.NoopTask();
        }

        public async Task ConnectAsync()
        {
            var config = Configuration.Instance;
            string token = config.Token;
            bool check = true;

            while (check)
            {
                Log.Debug("Checking token");

                if (token == null)
                {
                    Log.Debug("Null token");
                    token = AuthenticationError?.Invoke(null, new Exception("Token not set"));
                }

                if(token != null)
                {
                    try
                    {
                        Log.Debug("Logging in");
                        await Client.LoginAsync(TokenType.Bot, token);
                        Log.Debug("Logged in. Starting");
                        await Client.StartAsync();
                        config.Token = token;
                        check = false;
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "Fail to login");

                        if (ex is HttpException && (ex as HttpException).HttpCode == HttpStatusCode.Unauthorized)
                        {
                            config.Token = null;
                            token = AuthenticationError?.Invoke(token, ex);
                        }
                        else throw (ex);
                    }
                }

                if (token == null)
                {
                    Log.Error("Token not provided");
                    throw new Exception("Token not provided");
                }
            }
        }

        protected async Task CheckRolesAsync()
        {
            Log.Verbose("Checking roles");

            if (MasterRole == null || SlaveRole == null)
            {
                Log.Warning("Roles are not set. Calling creation prompter");

                if (! RolesCreationPrompter())
                {
                    throw new Exception("Roles are required");
                }

                await CreateRolesAsync();
            }

            await SelfAssignMasterRoleAsync();
        }

        protected async Task CheckChannelAsync()
        {
            var bot = Instance;

            if (bot.Channel == null)
            {
                Log.Warning("Channel has not set. Calling creation prompter");

                if (! ChannelCreationPrompter())
                {
                    throw new Exception($"Textual channel \"{Configuration.ChannelName}\" is required");
                }

                bot.Channel = await bot.Guild.CreateTextChannelAsync(Configuration.ChannelName);
            }

            await bot.SetChannelPermissionsAsync();
        }

        protected async Task CreateRolesAsync()
        {
            var perms = new GuildPermissions(
                viewChannel: true,
                readMessageHistory: true,
                sendMessages: true,
                addReactions: true,
                attachFiles: true
                );

            if (MasterRole == null)
            {
                Log.Debug("Creating master role");
                MasterRole = await Guild.CreateRoleAsync(Configuration.MasterRoleName, perms, null, false, false, null);
            }

            if (SlaveRole == null)
            {
                Log.Debug("Creating slave role");
                SlaveRole = await Guild.CreateRoleAsync(Configuration.SlaveRoleName, perms, null, false, false, null);
            }
        }

        protected async Task SelfAssignMasterRoleAsync()
        {
            var self = Guild.GetUser(Client.CurrentUser.Id);
            var masterRoleAssigned = self.Roles.FirstOrDefault(r => r.Id == MasterRole.Id) != null;

            if (!masterRoleAssigned)
            {
                Log.Debug("Self-assing master role");
                await self.AddRoleAsync(MasterRole);
            }
        }

        protected async Task SetChannelPermissionsAsync()
        {
            var everyoneOw = Channel.PermissionOverwrites.FirstOrDefault(ow =>
                ow.TargetType == PermissionTarget.Role && ow.TargetId == Guild.EveryoneRole.Id
                );

            if (everyoneOw.TargetId == 0)
            {
                Log.Debug("Making channel private");
                await Channel.AddPermissionOverwriteAsync(Guild.EveryoneRole, new OverwritePermissions(viewChannel: PermValue.Deny));
            }

            var masterOw = Channel.PermissionOverwrites.FirstOrDefault(ow =>
                ow.TargetType == PermissionTarget.Role && ow.TargetId == MasterRole.Id
                );

            if (masterOw.TargetId == 0)
            {
                Log.Debug("Making channel visible for master");
                await Channel.AddPermissionOverwriteAsync(MasterRole, new OverwritePermissions(viewChannel: PermValue.Allow));
            }

            var slaveOw = Channel.PermissionOverwrites.FirstOrDefault(ow =>
                ow.TargetType == PermissionTarget.Role && ow.TargetId == SlaveRole.Id
                );

            if (slaveOw.TargetId == 0)
            {
                Log.Debug("Making channel visible for slaves");
                await Channel.AddPermissionOverwriteAsync(SlaveRole, new OverwritePermissions(viewChannel: PermValue.Allow));
            }
        }

        public static Bot Instance
        {
            get
            {
                if (Singletone == null)
                {
                    Singletone = new Bot();
                }

                return Singletone;
            }
        }
    }
}
