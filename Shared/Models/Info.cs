using Newtonsoft.Json;

namespace Shared.Models
{
    public class Info
    {
        [JsonProperty("lastLogin")]
        public ulong? LastLogin { get; set; }
        [JsonProperty("loginCounter")]
        public int? LoginCounter { get; set; }
        [JsonProperty("location")]
        public string Location { get; set; }
        [JsonProperty("locality")]
        public string Locality { get; set; }
        [JsonProperty("presentation")]
        public string Presentation { get; set; }
        // TODO: Rest of this
    }
}
