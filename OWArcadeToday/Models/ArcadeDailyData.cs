using System;
using Newtonsoft.Json;

namespace OWArcadeToday.Models
{
    public sealed class ArcadeDailyData
    {
        [JsonProperty("tile_large")]
        public ArcadeTileData TileLarge { get; set; }
        [JsonProperty("tile_weekly_1")]
        public ArcadeTileData TileWeekly1 { get; set; }
        [JsonProperty("tile_weekly_2")]
        public ArcadeTileData TileWeekly2 { get; set; }
        [JsonProperty("tile_daily")]
        public ArcadeTileData TileDaily { get; set; }
        [JsonProperty("tile_permanent")]
        public ArcadeTileData TilePermanent { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }
        [JsonProperty("by_user")]
        public User ByUser { get; set; }
    }
}
