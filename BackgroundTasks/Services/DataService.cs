using System;
using System.Threading.Tasks;
using Windows.Web.Http;
using BackgroundTasks.Models;
using Newtonsoft.Json;
using Microsoft.Toolkit.Uwp.Helpers;

namespace BackgroundTasks.Services
{
    internal sealed class DataService
    {
        private const string API_TODAY = "https://overwatcharcade.today/api/today";
        private readonly string UserAgent;

        public DataService()
        {
            var ver = SystemInformation.ApplicationVersion;
            var osVer = SystemInformation.OperatingSystemVersion;
            var arch = SystemInformation.OperatingSystemArchitecture;
            UserAgent = $"OWArcadeToday/{ver.Major}.{ver.Minor}.{ver.Build}.{ver.Revision} (Windows {osVer.Major}.{osVer.Minor}.{osVer.Build}; {arch}; UWP)";
        }

        public async Task<ArcadeDailyData> GetTodayArcadeAsync()
        {
            string json = await GetResponseAsync(API_TODAY);
            var data = JsonConvert.DeserializeObject<ArcadeDailyData>(json);
            return data;
        }

        private async Task<string> GetResponseAsync(string url)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders["User-Agent"] = UserAgent;

                var response = await client.GetAsync(new Uri(url));
                string responseString = await response.Content.ReadAsStringAsync();

                if (responseString == "[]")
                    throw new NoDataException();

                return responseString;
            }
        }
    }
}
