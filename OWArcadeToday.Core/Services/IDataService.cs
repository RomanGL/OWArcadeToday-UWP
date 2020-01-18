using OWArcadeToday.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace OWArcadeToday.Core.Services
{
    /// <summary>
    /// Represents an Overwatch Arcade data service.
    /// </summary>
    public interface IDataService
    {
        /// <summary>
        /// Gets the today Overwatch Arcade data.
        /// </summary>
        /// <returns>Returns a today Overwatch Arcade Data.</returns>
        /// <exception cref="NoDataException">No data of Overwatch Arcades.</exception>
        [ItemNotNull]
        Task<ArcadeDailyData> GetTodayArcadeAsync();

        /// <summary>
        /// Gets the week history of Overwatch Arcade.
        /// </summary>
        /// <returns>Returns a collection of an Overwatch Arcade daily data.</returns>
        /// <exception cref="NoDataException">No data of Overwatch Arcades.</exception>
        [ItemNotNull]
        Task<List<ArcadeDailyData>> GetWeekHistoryAsync();
    }
}