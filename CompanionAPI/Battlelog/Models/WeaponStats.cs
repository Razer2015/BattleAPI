using System.Collections.Generic;
using Newtonsoft.Json;

namespace CompanionAPI.Battlelog.Models
{
    public class WeaponStats
    {
        [JsonProperty("personaId")]
        public ulong PersonaId { get; set; }
        [JsonProperty("mainWeaponStats")]
        public List<MainWeaponStat> MainWeaponStats { get; set; }
    }
}
