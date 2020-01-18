using Newtonsoft.Json;
using System;

namespace OWArcadeToday.Core.Models
{
    /// <summary>
    /// Represents an Overwatch Arcade daily data.
    /// </summary>
    public sealed class ArcadeDailyData
    {
        /// <summary>
        /// Gets or sets the first tile data.
        /// </summary>
        [JsonProperty("tile_1")]
        public ArcadeTileData Tile1 { get; set; }

        /// <summary>
        /// Gets or sets the second tile data.
        /// </summary>
        [JsonProperty("tile_2")]
        public ArcadeTileData Tile2 { get; set; }

        /// <summary>
        /// Gets or sets the third tile data.
        /// </summary>
        [JsonProperty("tile_3")]
        public ArcadeTileData Tile3 { get; set; }

        /// <summary>
        /// Gets or sets the fourth tile data.
        /// </summary>
        [JsonProperty("tile_4")]
        public ArcadeTileData Tile4 { get; set; }

        /// <summary>
        /// Gets or sets the fifth tile data.
        /// </summary>
        [JsonProperty("tile_5")]
        public ArcadeTileData Tile5 { get; set; }

        /// <summary>
        /// Gets or sets the sixth tile data.
        /// </summary>
        [JsonProperty("tile_6")]
        public ArcadeTileData Tile6 { get; set; }

        /// <summary>
        /// Gets or sets the seventh tile data.
        /// </summary>
        [JsonProperty("tile_7")]
        public ArcadeTileData Tile7 { get; set; }

        /// <summary>
        /// Gets or sets the last data update time.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the user which made an update.
        /// </summary>
        [JsonProperty("by_user")]
        public User ByUser { get; set; }
    }
}