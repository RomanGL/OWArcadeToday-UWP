using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using OWArcadeToday.Models;
using OWArcadeToday.Services;

namespace OWArcadeToday.ViewModels
{
    public sealed class MainViewModel : ViewModelBase
    {
        private readonly DataService _service;
        private readonly SettingsService _settingsService;
        private readonly DispatcherTimer _timer;

        private ArcadeDailyData _data;
        private string _timeLeft = "00:00:00";

        public MainViewModel()
        {
            _service = new DataService();
            _settingsService = new SettingsService();

            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += Timer_Tick;
        }

        public ArcadeDailyData Data
        {
            get => _data;
            set
            {
                if (value != _data)
                {
                    _data = value;
                    OnPropertyChanged();
                }
            }
        }

        public string TimeLeft
        {
            get => _timeLeft;
            set
            {
                if (value != _timeLeft)
                {
                    _timeLeft = value;
                    OnPropertyChanged();
                }
            }
        }

        public async Task LoadDataAsync()
        {
            if (Data != null)
                return;

            Data = await _service.GetTodayArcadeAsync();

            _settingsService.Set("UpdatedAt", Data.UpdatedAt);
            _settingsService.Set("LastData", Data);
        }

        public void StartTimer()
        {
            UpdateTimeLeft();
            _timer.Start();
        }

        public void StopTimer() => _timer.Stop();
        private void Timer_Tick(object sender, object e) => UpdateTimeLeft();

        private void UpdateTimeLeft()
        {
            var utcTomorrow = DateTime.Today.AddDays(1);
            var left = utcTomorrow - DateTime.UtcNow;

            TimeLeft = left.ToString(@"hh\:mm\:ss");
        }
    }
}
