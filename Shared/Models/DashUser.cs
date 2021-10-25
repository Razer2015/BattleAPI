using Newtonsoft.Json;

namespace Shared.Models
{
    public class DashUser
    {
        [JsonProperty("persona")]
        public Persona Persona { get; set; }
        [JsonProperty("emblemUrl")]
        public string EmblemUrl { get; set; }
        [JsonProperty("gameExpansions")]
        public object GameExpansions { get; set; } // TODO: Map this
        [JsonProperty("presence")]
        public Presence Presence { get; set; }
        [JsonProperty("userId")]
        public string UserId { get; set; }
        [JsonProperty("connectedDevices")]
        public object ConnectedDevices { get; set; } // TODO: Map this
        [JsonProperty("dogtags")]
        public object Dogtags { get; set; } // TODO: Map this
        [JsonProperty("info")]
        public Info Info { get; set; }
        [JsonProperty("personaId")]
        public string PersonaId { get; set; }
        [JsonProperty("stats")]
        public object Stats { get; set; } // TODO: Map this
        [JsonProperty("isLightWeight")]
        public bool IsLightWeight { get; set; }
        [JsonProperty("isPartyMember")]
        public bool IsPartyMember { get; set; }
        [JsonProperty("isExternalUser")]
        public bool IsExternalUser { get; set; }
        [JsonProperty("user")]
        public User User { get; set; }
    }
}
