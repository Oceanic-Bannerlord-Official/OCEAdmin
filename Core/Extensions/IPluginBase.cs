using HarmonyLib;
using System.Threading.Tasks;
using TaleWorlds.Core;

namespace OCEAdmin.Core.Extensions
{
    public interface IPluginBase
    {
        string Name { get; }
        Task Load();
        void OnMultiplayerGameStart(Game game, object starterObject);
        void OnGameEnd(Game game);
        void OnPatch(Harmony harmony);
    }
}
