using Newtonsoft.Json;

namespace OWArcadeToday.Core.Models
{
    /// <summary>
    /// Represents an Overwatch Arcade tile data.
    /// </summary>
    public sealed class ArcadeTileData
    {
        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the name of a game mode.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the count of players in this game mode.
        /// </summary>
        [JsonProperty("players")]
        public string Players { get; set; }

        /// <summary>
        /// Gets or sets the code of a game mode.
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the game mode changes type.
        /// </summary>
        [JsonProperty("label")]
        public GameModeChangesType GameModeChangesType { get; set; }
    }
}