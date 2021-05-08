using Newtonsoft.Json;

namespace CompanionAPI.Battlelog.Models
{
    public class WeaponInfo
    {
        [JsonProperty("guid")]
        public string Guid { get; set; }
        [JsonProperty("category")]
        public string Category { get; set; }
        [JsonProperty("categorySID")]
        public string CategorySID { get; set; }
        [JsonProperty("slug")]
        public string Slug { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
