using Newtonsoft.Json;
using OWArcadeToday.Core.JsonConverters;
using System.Runtime.Serialization;

namespace OWArcadeToday.Core.Models
{
    /// <summary>
    /// Represents a game mode changes type.
    /// </summary>
    [JsonConverter(typeof(TolerantEnumConverter))]
    public enum GameModeChangesType
    {
        /// <summary>
        /// Game mode will not changed.
        /// </summary>
        [EnumMember(Value = "Unknown")]
        Unknown = 0,

        /// <summary>
        /// Game mode changes daily.
        /// </summary>
        [EnumMember(Value = "Daily")]
        Daily = 1,

        /// <summary>
        /// Game mode changes weekly.
        /// </summary>
        [EnumMember(Value = "Weekly")]
        Weekly = 2,
    }
}