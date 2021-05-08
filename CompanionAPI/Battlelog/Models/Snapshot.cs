using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace CompanionAPI.Battlelog.Models
{
    public class Snapshot
    {
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("gameId")]
        public ulong GameId { get; set; }
        [JsonProperty("gameMode")]
        public string GameMode { get; set; }
        [JsonProperty("mapVariant")]
        public byte MapVariant { get; set; }
        [JsonProperty("currentMap")]
        public string CurrentMap { get; set; }
        [JsonProperty("maxPlayers")]
        public int MaxPlayers { get; set; }
        [JsonProperty("waitingPlayers")]
        public int WaitingPlayers { get; set; }
        [JsonProperty("roundTime")]
        public uint RoundTime { get; set; }
        [JsonProperty("defaultRoundTimeMultiplier")]
        public uint DefaultRoundTimeMultiplier { get; set; }

        [JsonProperty("rush", NullValueHandling = NullValueHandling.Ignore)]
        public Rush Rush { get; set; }

        [JsonProperty("teamInfo", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, TeamInfo> TeamInfo { get; set; }

        public Snapshot()
        {
            TeamInfo = new Dictionary<string, TeamInfo>();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("##############################################################################################################");
            sb.AppendLine("#                                            General Information                                             #");
            sb.AppendLine("##############################################################################################################");
            sb.AppendLine($"CurrentMap: {CurrentMap}");
            sb.AppendLine($"DefaultRoundTimeMultiplier: {DefaultRoundTimeMultiplier}");
            sb.AppendLine($"GameId: {GameId}");
            sb.AppendLine($"GameMode: {GameMode}");
            sb.AppendLine($"MapVariant: {MapVariant}");
            sb.AppendLine($"MaxPlayers: {MaxPlayers}");
            sb.AppendLine($"WaitingPlayers: {WaitingPlayers}");
            sb.AppendLine($"RoundTime: {RoundTime}");
            sb.AppendLine();

            if (Rush != null)
                sb.AppendLine(Rush.ToString());
            for (int i = 0; i < TeamInfo.Count; i++)
            {
                var team = TeamInfo[i.ToString()];
                sb.AppendLine("##############################################################################################################");
                sb.AppendLine($"#                                             Team {i} Information                                             #");
                sb.AppendLine("##############################################################################################################");

                sb.AppendLine($"| {"PersonaId",13} | {"Tag",4} | {"Name",30} | {"Rank",4} | {"Score",10} | {"Kills",5} | {"Deaths",6} | {"SquadId",7} | {"Role",4} |");
                sb.AppendLine($"|---------------|------|--------------------------------|------|------------|-------|--------|---------|------|");
                foreach (var player in team.Players)
                {
                    sb.AppendLine(player.Value.ToString());
                }
                sb.AppendLine($"|_______________|______|________________________________|______|____________|_______|________|_________|______|");
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
