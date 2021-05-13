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

        // TODO... map the rest properly, currently has just objects for some properties
        [JsonProperty("mapExpansion")]
        public object MapExpansion { get; set; }
        [JsonProperty("expansions")]
        public object Expansions { get; set; }
        [JsonProperty("game")]
        public string Game { get; set; }
        [JsonProperty("platform")]
        public string Platform { get; set; }
        [JsonProperty("passwordProtected")]
        public string PasswordProtected { get; set; }
        [JsonProperty("operationIndex")]
        public string OperationIndex { get; set; }
        [JsonProperty("ip")]
        public string Ip { get; set; }
        [JsonProperty("pingSiteAlias")]
        public string PingSiteAlias { get; set; }
        [JsonProperty("isFavorite")]
        public bool IsFavorite { get; set; }
        [JsonProperty("custom")]
        public bool Custom { get; set; }
        [JsonProperty("preset")]
        public string Preset { get; set; }
        [JsonProperty("tickRate")]
        public object TickRate { get; set; }
        [JsonProperty("serverType")]
        public string ServerType { get; set; }
        [JsonProperty("settings")]
        public object Settings { get; set; }
        [JsonProperty("rotation")]
        public object Rotation { get; set; }
        [JsonProperty("punkbusterEnabled")]
        public bool PunkbusterEnabled { get; set; }
        [JsonProperty("fairfightEnabled")]
        public bool FairfightEnabled { get; set; }
        [JsonProperty("experience")]
        public string Experience { get; set; }
        [JsonProperty("officialExperienceId")]
        public string OfficialExperienceId { get; set; }
        [JsonProperty("serverBookmarkCount")]
        public string ServerBookmarkCount { get; set; }
        [JsonProperty("mixId")]
        public string MixId { get; set; }
        [JsonProperty("serverMode")]
        public string ServerMode { get; set; }
        [JsonProperty("mapRotation")]
        public object MapRotation { get; set; }
        [JsonProperty("secret")]
        public string Secret { get; set; }
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
