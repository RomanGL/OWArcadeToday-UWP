using Newtonsoft.Json;

namespace BackgroundTasks.Models
{
    internal sealed class ArcadeTileData
    {
        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("players")]
        public string Players { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }
    }
}
