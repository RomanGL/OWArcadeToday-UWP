using System;
using System.Diagnostics;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;
using Microsoft.Toolkit.Uwp.Notifications;
using OWArcadeToday.Core.Models;
using OWArcadeToday.Core.Services;
using OWArcadeToday.Helpers;
using OWArcadeToday.Services;

namespace BackgroundTasks
{
    /// <summary>
    /// Represents a background task for notification.
    /// </summary>
    public sealed class ArcadeBackgroundTask : IBackgroundTask
    {
        #region Fields

        private const string MODE_IMAGE_URL_MASK = "https://overwatcharcade.today/img/modes/{0}.jpg";
        private const string MODE_LARGE_IMG_URL_MASK = "https://overwatcharcade.today/img/modes_large/{0}.jpg";

        private const string NEW_DAILY_ARCADE = "New daily arcade";
        private const string NEW_WEEKLY_ARCADE = "New weekly arcade";
        private const string NEW_PERMANENT_ARCADE = "New permanent arcade";

        private readonly IDataService dataService;
        private readonly ISettingsService settingsService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ArcadeBackgroundTask"/> class.
        /// </summary>
        public ArcadeBackgroundTask()
        {
            dataService = new DataService(HttpHelper.GetUserAgent());
            settingsService = new SettingsService();
        }

        #endregion

        #region Implementation of IBackgroundTask

        /// <inheritdoc />
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();

            try
            {
                if (settingsService.GetNoCache("NotificationsEnabled", true))
                {
                    var todayArcades = await dataService.GetTodayArcadeAsync();
                    
                    if (todayArcades.CreatedAt > settingsService.GetNoCache("CreatedAt", DateTime.MinValue))
                    {
                        var lastData = settingsService.Get<ArcadeDailyData>("LastData");
                        settingsService.Set("LastData", todayArcades);
                        
                        if (lastData == null)
                        {
                            //ShowTileToast(todayArcades.TileLarge, NEW_WEEKLY_ARCADE);
                            //ShowTileToast(todayArcades.TileDaily, NEW_DAILY_ARCADE);
                            //ShowTileToast(todayArcades.TileWeekly1, NEW_WEEKLY_ARCADE);
                            //ShowTileToast(todayArcades.TileWeekly2, NEW_WEEKLY_ARCADE);
                            //ShowTileToast(todayArcades.TilePermanent, NEW_PERMANENT_ARCADE);
                        }
                        else
                        {
                            //if (lastData.TileLarge.Id != todayArcades.TileLarge.Id)
                            //    ShowTileToast(todayArcades.TileLarge, NEW_DAILY_ARCADE);
                            //if (lastData.TileDaily.Id != todayArcades.TileDaily.Id)
                            //    ShowTileToast(todayArcades.TileDaily, NEW_DAILY_ARCADE);
                            //if (lastData.TileWeekly1.Id != todayArcades.TileWeekly1.Id)
                            //    ShowTileToast(todayArcades.TileWeekly1, NEW_WEEKLY_ARCADE);
                            //if (lastData.TileWeekly2.Id != todayArcades.TileWeekly2.Id)
                            //    ShowTileToast(todayArcades.TileWeekly2, NEW_WEEKLY_ARCADE);
                            //if (lastData.TilePermanent.Id != todayArcades.TilePermanent.Id)
                            //    ShowTileToast(todayArcades.TilePermanent, NEW_PERMANENT_ARCADE);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Background Task Exception: {ex.Message}");
            }
            finally
            {
                deferral.Complete();
            }
        }

        #endregion

        #region Private Methods

        private void ShowTileToast(ArcadeTileData data, string title)
        {
            var toastContent = new ToastContent()
            {
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = title,
                                HintMaxLines = 1
                            },
                            new AdaptiveText()
                            {
                                Text = data.Name
                            },
                            new AdaptiveGroup()
                            {
                                Children =
                                {
                                    new AdaptiveSubgroup()
                                    {
                                        HintWeight = 1,
                                        Children =
                                        {
                                            new AdaptiveText()
                                            {
                                                Text = "Players",
                                                HintStyle = AdaptiveTextStyle.Body
                                            },
                                        }
                                    },
                                    new AdaptiveSubgroup()
                                    {
                                        HintWeight = 1,
                                        Children =
                                        {
                                            new AdaptiveText()
                                            {
                                                Text = data.Players,
                                                HintStyle = AdaptiveTextStyle.CaptionSubtle,
                                                HintAlign = AdaptiveTextAlign.Right
                                            }
                                        }
                                    }
                                }
                            }
                        },
                        HeroImage = new ToastGenericHeroImage()
                        {
                            Source = GetGameModeImageUrl(data.Code)
                        }
                    }
                }
            };

            var notification = new ToastNotification(toastContent.GetXml());
            ToastNotificationManager.CreateToastNotifier().Show(notification);
        }


        /// <summary>
        /// Gets the url of the game mode image.
        /// </summary>
        /// <param name="gameModeCode">The game mode code.</param>
        /// <param name="isLargeTile">The value indicating whether a tile is large.</param>
        /// <returns>Returns the url to the game mode image.</returns>
        private static string GetGameModeImageUrl(string gameModeCode, bool isLargeTile = false)
        {
            return string.Format(isLargeTile ? MODE_LARGE_IMG_URL_MASK : MODE_IMAGE_URL_MASK, gameModeCode);
        }

        #endregion
    }
}
