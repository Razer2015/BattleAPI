using Newtonsoft.Json;

namespace CompanionAPI.Models
{
    public class ServerDetailsViewModel
    {
        [JsonProperty("gameId")]
        public string GameId { get; set; }
        [JsonProperty("guid")]
        public string Guid { get; set; }
        [JsonProperty("protocolVersion")]
        public string ProtocolVersion { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("region")]
        public string Region { get; set; }
        [JsonProperty("country")]
        public string Country { get; set; }
        [JsonProperty("ranked")]
        public string Ranked { get; set; }
        [JsonProperty("slots")]
        public SlotTypesViewModel Slots { get; set; }
        [JsonProperty("mapName")]
        public string MapName { get; set; }
        [JsonProperty("mapNamePretty")]
        public string MapNamePretty { get; set; }
        [JsonProperty("mapMode")]
        public string MapMode { get; set; }
        [JsonProperty("mapModePretty")]
        public string MapModePretty { get; set; }
        [JsonProperty("mapImageUrl")]
        public string MapImageUrl { get; set; }

        // TODO... rest
    }

    public class SlotTypesViewModel
    {
        [JsonProperty("Queue")]
        public SlotsViewModel Queue { get; set; }
        [JsonProperty("Soldier")]
        public SlotsViewModel Soldier { get; set; }
        [JsonProperty("Spectator")]
        public SlotsViewModel Spectator { get; set; }
    }

    public class SlotsViewModel
    {
        [JsonProperty("current")]
        public ushort Current { get; set; }
        [JsonProperty("max")]
        public ushort Max { get; set; }
    }
}
