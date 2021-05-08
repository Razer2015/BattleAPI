using Newtonsoft.Json;

namespace CompanionAPI.Battlelog.Models
{
    public class MainWeaponStat
    {
        [JsonProperty("guid")]
        public string Guid { get; set; }
        [JsonProperty("serviceStars")]
        public int ServiceStars { get; set; }
        [JsonProperty("serviceStarsProgress")]
        public double ServiceStarsProgress { get; set; }
        [JsonProperty("category")]
        public string Category { get; set; }
        [JsonProperty("categorySID")]
        public string CategorySID { get; set; }
        [JsonProperty("slug")]
        public string Slug { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("kills")]
        public double? Kills { get; set; }
        [JsonProperty("shotsFired")]
        public double? ShotsFired { get; set; }
        [JsonProperty("shotsHit")]
        public double? ShotsHit { get; set; }
        [JsonProperty("accuracy")]
        public double? Accuracy { get; set; }
        [JsonProperty("headshots")]
        public double? Headshots { get; set; }
        [JsonProperty("timeEquipped")]
        public double? TimeEquipped { get; set; }
    }
}
