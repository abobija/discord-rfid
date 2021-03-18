using Discord;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordRfid
{
    public class Bot
    {
        protected static Bot Singletone { get; set; }

        public DiscordSocketClient Client { get; set; }

        public string Name => Client?.CurrentUser?.Username;
        public SocketGuild Guild => Client.Guilds.ElementAt(0);
        public IRole MasterRole { get; set; }
        public IRole SlaveRole { get; set; }
        public ITextChannel Channel { get; set; }

        public event Action<Exception> ConnectError;
        public event Action<Exception> EnvironmentCreationError;
        public event Action Ready;

        public Func<string> TokenProvider { get; set; }
        public Func<bool> RolesCreationPrompter { get; set; }
        public Func<bool> ChannelCreationPrompter { get; set; }

        protected Bot()
        {
            Client = new DiscordSocketClient();

            Client.GuildAvailable += async g =>
            {
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
                    EnvironmentCreationError?.Invoke(ex);
                }
            };
        }

        public async Task ConnectAsync()
        {
            var config = Configuration.Instance;
            string token = config.Token;
            bool check = true;

            while (check)
            {
                if (token == null)
                {
                    if(TokenProvider == null)
                    {
                        break;
                    }

                    token = TokenProvider();
                }

                try
                {
                    await Client.LoginAsync(TokenType.Bot, token);
                    await Client.StartAsync();
                    config.Token = token;
                    check = false;
                }
                catch (Exception ex)
                {
                    config.Token = token = null;
                    ConnectError?.Invoke(ex);
                }
            }
        }

        protected async Task CheckRolesAsync()
        {
            if (MasterRole == null || SlaveRole == null)
            {
                if(! RolesCreationPrompter())
                {
                    throw new Exception("Roles are required");
                }

                await CreateRolesAsync();
            }

            await SelfAssignMasterRoleAsync();
        }

        protected async Task CheckChannelAsync()
        {
            var bot = Bot.Instance;

            if (bot.Channel == null)
            {
                if(! ChannelCreationPrompter())
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
                MasterRole = await Guild.CreateRoleAsync(Constants.MasterRoleName, perms, null, false, false, null);
            }

            if (SlaveRole == null)
            {
                SlaveRole = await Guild.CreateRoleAsync(Constants.SlaveRoleName, perms, null, false, false, null);
            }
        }

        protected async Task SelfAssignMasterRoleAsync()
        {
            var self = Guild.GetUser(Client.CurrentUser.Id);
            var masterRoleAssigned = self.Roles.FirstOrDefault(r => r.Id == MasterRole.Id) != null;

            if (!masterRoleAssigned)
            {
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
                // make channel private
                await Channel.AddPermissionOverwriteAsync(Guild.EveryoneRole, new OverwritePermissions(viewChannel: PermValue.Deny));
            }

            var masterOw = Channel.PermissionOverwrites.FirstOrDefault(ow =>
                ow.TargetType == PermissionTarget.Role && ow.TargetId == MasterRole.Id
                );

            if (masterOw.TargetId == 0)
            {
                // make channel visible for master
                await Channel.AddPermissionOverwriteAsync(MasterRole, new OverwritePermissions(viewChannel: PermValue.Allow));
            }

            var slaveOw = Channel.PermissionOverwrites.FirstOrDefault(ow =>
                ow.TargetType == PermissionTarget.Role && ow.TargetId == SlaveRole.Id
                );

            if (slaveOw.TargetId == 0)
            {
                // make channel visible for slave
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
