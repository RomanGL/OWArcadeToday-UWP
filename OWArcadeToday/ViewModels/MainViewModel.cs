using JetBrains.Annotations;
using OWArcadeToday.Core.Models;
using OWArcadeToday.Core.Services;
using OWArcadeToday.Helpers;
using OWArcadeToday.Services;
using OWArcadeToday.Views;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace OWArcadeToday.ViewModels
{
    /// <summary>
    /// Represents a <seealso cref="MainView"/> view model.
    /// </summary>
    public sealed class MainViewModel : ViewModelBase
    {
        #region Fields

        private const string DEFAULT_TIME_LEFT = "00:00:00";

        private readonly IDataService service;
        private readonly ISettingsService settingsService;
        private readonly DispatcherTimer timer;

        private ArcadeDailyData data;
        private bool isDataObsoleted;
        private string timeLeft;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel()
        {
            service = new DataService(HttpHelper.GetUserAgent());
            settingsService = new SettingsService();

            timer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(1)};
            timer.Tick += OnTimerTick;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Overwatch Arcade Daily data.
        /// </summary>
        [CanBeNull]
        public ArcadeDailyData Data
        {
            get => data;
            private set
            {
                if (value != data)
                {
                    data = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets a value indicating that data is obsoleted.
        /// </summary>
        public bool IsDataObsoleted
        {
            get => isDataObsoleted;
            private set
            {
                if (value != isDataObsoleted)
                {
                    isDataObsoleted = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets the string representation of a time left.
        /// </summary>
        [NotNull]
        public string TimeLeft
        {
            get => timeLeft ?? DEFAULT_TIME_LEFT;
            private set
            {
                if (value != timeLeft)
                {
                    timeLeft = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Loads a data.
        /// </summary>
        public async Task LoadDataAsync()
        {
            if (Data != null)
                return;

            try
            {
                Data = await service.GetTodayArcadeAsync();
            }
            catch (NoDataException)
            {
                var history = await service.GetWeekHistoryAsync();
                if (!history.Any())
                    throw new NoDataException();

                IsDataObsoleted = true;
                Data = history.First();
            }

            settingsService.Set("CreatedAt", Data.CreatedAt);
            settingsService.Set("LastData", Data);
        }

        /// <summary>
        /// Starts a time left timer.
        /// </summary>
        public void StartTimer()
        {
            UpdateTimeLeft();
            timer.Start();
        }

        /// <summary>
        /// Stops a time left timer.
        /// </summary>
        public void StopTimer() => timer.Stop();

        #endregion

        #region Private Methods

        /// <summary>
        /// Updates a string representation of the time left.
        /// </summary>
        private void UpdateTimeLeft()
        {
            var utcTomorrow = DateTime.Today.AddDays(1);
            var left = utcTomorrow - DateTime.UtcNow;

            TimeLeft = left.ToString(@"hh\:mm\:ss");
        }

        /// <summary>
        /// Occurs when the timer ticks.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void OnTimerTick(object sender, object e) => UpdateTimeLeft();

        #endregion
    }
}