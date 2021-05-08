using Newtonsoft.Json;
using System.Collections.Generic;

namespace CompanionAPI.Companion.Models
{
    public class WeaponStatCategory
    {
        [JsonProperty("weapons")]
        public List<Weapon> Weapons { get; set; }
    }

    public class Weapon
    {
        [JsonProperty("guid")]
        public string Guid { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("category")]
        public string Category { get; set; }
        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }
        [JsonProperty("stats")]
        public WeaponStat Stats { get; set; }
    }

    public class WeaponStat
    {
        public class ValuesModel
        {
            [JsonProperty("kills")]
            public double Kills { get; set; }
            [JsonProperty("headshots")]
            public double Headshots { get; set; }
            [JsonProperty("accuracy")]
            public double Accuracy { get; set; }
            [JsonProperty("seconds")]
            public double Seconds { get; set; }
            [JsonProperty("hits")]
            public double Hits { get; set; }
            [JsonProperty("shots")]
            public double Shots { get; set; }
        }
        
        [JsonProperty("values")]
        public ValuesModel Values { get; set; }
    }
}
