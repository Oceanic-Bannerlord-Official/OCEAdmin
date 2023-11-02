using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCEAdmin.Core.Missions
{
    public struct MissionData
    {
        public string gameType;
        public string mapId;
        public string cultureTeam1;
        public string cultureTeam2;
        public bool cultureVote;
        public bool mapVote;
        public int roundTime;
        public int warmupTime;
        public int mapTime;
        public int numRounds;

        public override string ToString()
        {
            return "gameType: " + gameType + "\n" +
                "mapId: " + mapId + "\n" +
                "cultureTeam1: " + cultureTeam1 + "\n" +
                "cultureTeam2: " + cultureTeam2 + "\n" +
                "cultureTeam2: " + cultureTeam2 + "\n" +
                "cultureVote: " + cultureVote + "\n" +
                "mapVote: " + mapVote + "\n" +
                "roundTime: " + roundTime + "\n" +
                "warmupTime: " + warmupTime + "\n" +
                "mapTime: " + mapTime + "\n" +
                "numRounds: " + numRounds + "\n";
        }
    }
}
