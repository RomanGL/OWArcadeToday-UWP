using Newtonsoft.Json;

namespace OWArcadeToday.Core.Models
{
    /// <summary>
    /// Represents a user.
    /// </summary>
    public sealed class User
    {
        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the user Blizzard BattleTag.
        /// </summary>
        [JsonProperty("battletag")]
        public string BattleTag { get; set; }

        /// <summary>
        /// Gets or sets the user avatar url.
        /// </summary>
        [JsonProperty("avatar")]
        public string AvatarUrl { get; set; }
    }
}