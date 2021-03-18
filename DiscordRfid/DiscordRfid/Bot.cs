using Discord;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiscordRfid
{
    public class Bot
    {
        protected static Bot Singletone { get; set; }

        public DiscordSocketClient Client { get; set; }

        public SocketGuild Guild => Client.Guilds.ElementAt(0);
        public IRole MasterRole { get; set; }
        public IRole SlaveRole { get; set; }
        public ITextChannel Channel { get; set; }

        protected Bot()
        {
            Client = new DiscordSocketClient();

            Client.GuildAvailable += g =>
            {
                MasterRole = Guild.Roles.FirstOrDefault(r => r.Name == Constants.MasterRoleName);
                SlaveRole = Guild.Roles.FirstOrDefault(r => r.Name == Constants.SlaveRoleName);
                Channel = Guild.TextChannels.FirstOrDefault(c => c.Name == Constants.ChannelName);

                return Extensions.NoopTask();
            };
        }

        public async Task CreateRolesAsync()
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

        public async Task SelfAssignMasterRoleAsync()
        {
            var self = Guild.GetUser(Client.CurrentUser.Id);
            var masterRoleAssigned = self.Roles.FirstOrDefault(r => r.Id == MasterRole.Id) != null;

            if (!masterRoleAssigned)
            {
                await self.AddRoleAsync(MasterRole);
            }
        }

        public async Task SetChannelPermissionsAsync()
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
