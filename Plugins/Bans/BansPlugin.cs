using OCEAdmin.Core.Plugin;
using OCEAdmin.Plugins.Commands;
using OCEAdmin.Plugins.Groupfight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace OCEAdmin.Plugins.Bans
{
    public class BansPlugin : PluginBase
    {
        public override string Name => "Bans";

        public override string Description => "Manages bans and unbanning of players.";

        public override bool IsCore => true;

        public IBanTransport Handler;

        private List<Ban> _bans = new List<Ban>();

        public BansPlugin() { }

        public override async Task Load()
        {
            await base.Load();

            CommandsPlugin commands = OCEAdminSubModule.GetPlugin<CommandsPlugin>();

            await commands.Register(new BanCommand());
            await commands.Register(new Unban());

            Handler = new LocalBanTransport();

            MPUtil.WriteToConsole("Loading bans...");

            await Handler.Load();
        }

        public override void OnMultiplayerGameStart(Game game, object starterObject)
        {
            base.OnMultiplayerGameStart(game, starterObject);
            game.AddGameHandler<BansGameHandler>();
        }

        public override void OnGameEnd(Game game)
        {
            base.OnGameEnd(game);
            game.RemoveGameHandler<BansGameHandler>();
        }

        public void Update(List<Ban> bans)
        {
            _bans = bans;
        }

        public List<Ban> GetBans()
        {
            return _bans;
        }

        public void AddBan(Ban ban)
        {
            GetBans().Add(ban);
        }

        public void RemoveBan(string id)
        {
            foreach (Ban ban in GetBans())
            {
                if (ban.steamid == id)
                {
                    GetBans().Remove(ban);

                    break;
                }
            }
        }

        public bool IsBanned(NetworkCommunicator peer)
        {
            foreach (Ban ban in GetBans())
            {
                if (ban.steamid.Contains(peer.VirtualPlayer.Id.Id2.ToString()))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
