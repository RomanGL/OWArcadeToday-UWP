using JetBrains.Annotations;
using OWArcadeToday.Core.Models;
using System.Threading.Tasks;

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
        /// Gets the last available today Overwatch Arcade data.
        /// </summary>
        /// <returns>Return a last available today Overwatch Arcade Data.</returns>
        /// <exception cref="NoDataException">No data of Overwatch Arcades.</exception>
        [ItemNotNull]
        Task<ArcadeDailyData> GetLastSetTodayArcadeAsync();
    }
}