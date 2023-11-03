﻿using HarmonyLib;
using System.Threading.Tasks;
using TaleWorlds.Core;

namespace OCEAdmin.Core.Plugin
{
    public interface IPluginBase
    {
        string Name { get; }
        string Description { get; }
        bool IsCore { get; }
        bool IsEnabled { get; }
        Task Load();
        void OnMultiplayerGameStart(Game game, object starterObject);
        void OnGameEnd(Game game);
        void OnPatch(Harmony harmony);
    }
}
