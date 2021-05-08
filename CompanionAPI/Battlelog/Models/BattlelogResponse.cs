using Newtonsoft.Json;

namespace CompanionAPI.Battlelog.Models
{
    interface IBattlelogResponse<T>
    {
        [JsonProperty("type")]
        string Type { get; }
        [JsonProperty("message")]
        string Message { get; }
        [JsonProperty("data")]
        T Data { get; set; }
    }


    public class BattlelogResponse<T> : IBattlelogResponse<T>
    {
        public string Type { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
