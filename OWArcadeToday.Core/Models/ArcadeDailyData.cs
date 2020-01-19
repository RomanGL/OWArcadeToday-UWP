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
        /// Gets or sets the today modes data.
        /// </summary>
        [JsonProperty("modes")]
        public ArcadeModesData Modes { get; set; }

        /// <summary>
        /// Gets or sets the last data update time.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the user which made an update.
        /// </summary>
        [JsonProperty("user")]
        public User ByUser { get; set; }
    }
}