using System;
using System.Threading.Tasks;
using Windows.Web.Http;
using BackgroundTasks.Models;
using Newtonsoft.Json;

namespace BackgroundTasks.Services
{
    internal sealed class DataService
    {
        private const string API_TODAY = "https://overwatcharcade.today/api/today";
        private const string API_WEEK = "https://overwatcharcade.today/api/week";

        public async Task<ArcadeDailyData> GetTodayArcadeAsync()
        {
            string json = await GetResponseAsync(API_TODAY);
            var data = JsonConvert.DeserializeObject<ArcadeDailyData>(json);
            return data;
        }

        private static async Task<string> GetResponseAsync(string url)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(new Uri(url));
                string responseString = await response.Content.ReadAsStringAsync();

                if (responseString == "[]")
                    throw new NoDataException();

                return responseString;
            }
        }
    }
}
