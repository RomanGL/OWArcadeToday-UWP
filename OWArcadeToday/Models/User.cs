using Newtonsoft.Json;

namespace OWArcadeToday.Models
{
    public sealed class User
    {
        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("battletag")]
        public string BattleTag { get; set; }
        [JsonProperty("avatar")]
        public string AvatarUrl { get; set; }
    }
}
