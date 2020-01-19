using JetBrains.Annotations;
using Newtonsoft.Json;
using OWArcadeToday.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OWArcadeToday.Core.Services
{
    /// <inheritdoc />
    public sealed class DataService : IDataService
    {
        #region Fields

        private const string API_TODAY = "https://overwatcharcade.today/api/today";
        private const string API_TODAY_FALLBACK = "https://overwatcharcade.today/api/today?fallback=true";

        private readonly HttpClient client;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DataService"/> class.
        /// </summary>
        /// <param name="userAgent">The User-Agent HTTP header.</param>
        public DataService([NotNull] string userAgent)
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", userAgent);
        }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public Task<ArcadeDailyData> GetTodayArcadeAsync() => GetArcadeDailyDataAsync(API_TODAY);

        /// <inheritdoc />
        public Task<ArcadeDailyData> GetLastSetTodayArcadeAsync() => GetArcadeDailyDataAsync(API_TODAY_FALLBACK);

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets arcade daily data from the specified url.
        /// </summary>
        /// <param name="url">The url.</param>
        /// <exception cref="NoDataException">No data of Overwatch Arcades.</exception>
        [ItemNotNull]
        private async Task<ArcadeDailyData> GetArcadeDailyDataAsync([NotNull] string url)
        {
            var json = await GetResponseAsync(url).ConfigureAwait(false);
            var data = JsonConvert.DeserializeObject<List<ArcadeDailyData>>(json);
            return data.FirstOrDefault() ?? throw new NoDataException();
        }

        /// <summary>
        /// Gets the response from the specified <paramref name="url"/>.
        /// </summary>
        /// <param name="url">The request URL.</param>
        /// <returns>Returns a response string.</returns>
        /// <exception cref="NoDataException">No data of Overwatch Arcades.</exception>
        /// <exception cref="InvalidOperationException">The <paramref name="url"/> is <c>null</c>.</exception>
        [ItemCanBeNull]
        private async Task<string> GetResponseAsync([NotNull] string url)
        {
            var response = await client.GetAsync(new Uri(url)).ConfigureAwait(false);
            string responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (string.IsNullOrEmpty(responseString))
                throw new NoDataException();

            return responseString;
        }

        #endregion
    }
}