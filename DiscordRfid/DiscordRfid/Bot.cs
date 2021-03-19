using Discord;
using Discord.Net;
using Discord.WebSocket;
using Serilog;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DiscordRfid
{
    public class Bot
    {
        protected static Bot Singletone;
        public DiscordSocketClient Client;

        public string Name => Client?.CurrentUser?.Username;
        public SocketGuild Guild => Client.Guilds.ElementAt(0);
        public IRole MasterRole;
        public IRole SlaveRole;
        public ITextChannel Channel;

        public event Action<Exception> UnauthorizedError;
        public event Action<Exception> EnvironmentCreationError;
        public event Action Ready;

        public Func<string> TokenProvider;
        public Func<bool> RolesCreationPrompter;
        public Func<bool> ChannelCreationPrompter;

        protected Bot()
        {
            Log.Verbose("Bot constructor");

            Client = new DiscordSocketClient();

            Client.Log += DiscordLog;
            Client.Rest.Log += DiscordLog;

            Client.GuildAvailable += async g =>
            {
                Log.Debug($"Guild available");

                MasterRole = Guild.Roles.FirstOrDefault(r => r.Name == Constants.MasterRoleName);
                SlaveRole = Guild.Roles.FirstOrDefault(r => r.Name == Constants.SlaveRoleName);
                Channel = Guild.TextChannels.FirstOrDefault(c => c.Name == Constants.ChannelName);

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
                    Log.Debug("Token is null. Calling token provider");

                    if (TokenProvider == null)
                    {
                        Log.Error("Token provider is not set");
                        break;
                    }

                    token = TokenProvider();

                    if(token == null)
                    {
                        Log.Error("Token not provided");
                        throw new Exception("Token not provided");
                    }
                }

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
                        config.Token = token = null;
                        UnauthorizedError?.Invoke(ex);
                    }
                    else throw (ex);
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
                    throw new Exception($"Textual channel \"{Constants.ChannelName}\" is required");
                }

                bot.Channel = await bot.Guild.CreateTextChannelAsync(Constants.ChannelName);
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
                MasterRole = await Guild.CreateRoleAsync(Constants.MasterRoleName, perms, null, false, false, null);
            }

            if (SlaveRole == null)
            {
                Log.Debug("Creating slave role");
                SlaveRole = await Guild.CreateRoleAsync(Constants.SlaveRoleName, perms, null, false, false, null);
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
