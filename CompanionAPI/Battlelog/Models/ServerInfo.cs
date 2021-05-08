using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace CompanionAPI.Battlelog.Models
{
    public class ServerInfo
    {
        [JsonProperty("LastUpdated")]
        public long lastUpdated { get; set; }
        [JsonProperty("snapshot")]
        public Snapshot Snapshot { get; set; }

        public ServerInfo() { }

        public ulong? GetPersonaId(string soldierName)
        {
            var persona = GetPersona(soldierName);
            if (persona != null)
            {
                return ulong.Parse(persona?.Key);
            }

            return null;
        }

        public Player GetPlayer(string personaId)
        {
            var persona = GetPlayerByPersonaId(personaId);
            if (persona != null)
            {
                return persona.Value.Value;
            }

            return null;
        }

        private KeyValuePair<string, Player>? GetPlayerByPersonaId(string personaId)
        {
            try
            {
                return Snapshot?.TeamInfo?
                    .SelectMany(x => x.Value.Players)
                    .First(x => x.Key.Equals(personaId));
            }
            catch
            {
                return null;
            }
        }

        private KeyValuePair<string, Player>? GetPersona(string soldierName)
        {
            try
            {
                return Snapshot?.TeamInfo?
                .SelectMany(x => x.Value.Players)
                .First(x => x.Value.Name.Equals(soldierName));
            }
            catch
            {
                return null;
            }
        }
    }
}
