using Shared.Models;
using System;

namespace TimescaleDAL
{
    public class PlayerCountsData
    {
        public string ServerGuid { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Map { get; set; }
        public long? MapMode { get; set; }
        public ushort? BattlelogQueue { get; set; }
        public ushort? BattlelogPlayers { get; set; }
        public ushort? BattlelogSpectators { get; set; }
        public ushort? CompanionQueue { get; set; }
        public ushort? CompanionPlayers { get; set; }
        public ushort? CompanionSpectators { get; set; }
        public ushort? SnapshotQueue { get; set; }
        public ushort? SnapshotPlayers { get; set; }

        public PlayerCountsData()
        {

        }

        public PlayerCountsData(string guid, CombinedPlayerCountsViewModel data)
        {
            ServerGuid = guid;
            TimeStamp = DateTime.UtcNow;
            Map = data?.Map;
            MapMode = (long?)data?.MapMode;
            BattlelogQueue = data?.BattlelogSlots?.Queue?.Current;
            BattlelogPlayers = data?.BattlelogSlots?.Soldier?.Current;
            BattlelogSpectators = data?.BattlelogSlots?.Spectator?.Current;

            CompanionQueue = data?.CompanionSlots?.Queue?.Current;
            CompanionPlayers = data?.CompanionSlots?.Soldier?.Current;
            CompanionSpectators = data?.CompanionSlots?.Spectator?.Current;

            SnapshotQueue = data?.SnapshotSlots?.Queue?.Current;
            SnapshotPlayers = data?.SnapshotSlots?.Soldier?.Current;
        }
    }
}
