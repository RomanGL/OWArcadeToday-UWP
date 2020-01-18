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
                            ShowTileToast(todayArcades.Tile1, GetUpdateDescription(todayArcades.Tile1.GameModeChangesType), true);
                            ShowTileToast(todayArcades.Tile2, GetUpdateDescription(todayArcades.Tile2.GameModeChangesType));
                            ShowTileToast(todayArcades.Tile3, GetUpdateDescription(todayArcades.Tile3.GameModeChangesType));
                            ShowTileToast(todayArcades.Tile4, GetUpdateDescription(todayArcades.Tile4.GameModeChangesType));
                            ShowTileToast(todayArcades.Tile5, GetUpdateDescription(todayArcades.Tile5.GameModeChangesType));
                            ShowTileToast(todayArcades.Tile6, GetUpdateDescription(todayArcades.Tile6.GameModeChangesType));
                            ShowTileToast(todayArcades.Tile7, GetUpdateDescription(todayArcades.Tile7.GameModeChangesType));
                        }
                        else
                        {
                            if (lastData.Tile1.Id != todayArcades.Tile1.Id)
                            {
                                ShowTileToast(todayArcades.Tile1, GetUpdateDescription(todayArcades.Tile1.GameModeChangesType), true);
                            }

                            if (lastData.Tile2.Id != todayArcades.Tile2.Id)
                            {
                                ShowTileToast(todayArcades.Tile2, GetUpdateDescription(todayArcades.Tile2.GameModeChangesType));
                            }

                            if (lastData.Tile3.Id != todayArcades.Tile3.Id)
                            {
                                ShowTileToast(todayArcades.Tile3, GetUpdateDescription(todayArcades.Tile3.GameModeChangesType));
                            }

                            if (lastData.Tile4.Id != todayArcades.Tile4.Id)
                            {
                                ShowTileToast(todayArcades.Tile4, GetUpdateDescription(todayArcades.Tile4.GameModeChangesType));
                            }

                            if (lastData.Tile5.Id != todayArcades.Tile5.Id)
                            {
                                ShowTileToast(todayArcades.Tile5, GetUpdateDescription(todayArcades.Tile5.GameModeChangesType));
                            }

                            if (lastData.Tile6.Id != todayArcades.Tile6.Id)
                            {
                                ShowTileToast(todayArcades.Tile6, GetUpdateDescription(todayArcades.Tile6.GameModeChangesType));
                            }

                            if (lastData.Tile7.Id != todayArcades.Tile7.Id)
                            {
                                ShowTileToast(todayArcades.Tile7, GetUpdateDescription(todayArcades.Tile7.GameModeChangesType));
                            }
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

        private void ShowTileToast(ArcadeTileData data, string title, bool isLarge = false)
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
                            Source = GetGameModeImageUrl(data.Code, isLarge)
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

        /// <summary>
        /// Gets the update description.
        /// </summary>
        /// <param name="changesType">The game mode changes type.</param>
        /// <returns>Return the description.</returns>
        private static string GetUpdateDescription(GameModeChangesType changesType)
        {
            switch (changesType)
            {
                case GameModeChangesType.Unknown:
                    return NEW_PERMANENT_ARCADE;
                case GameModeChangesType.Daily:
                    return NEW_DAILY_ARCADE;
                case GameModeChangesType.Weekly:
                    return NEW_WEEKLY_ARCADE;
                default:
                    throw new ArgumentOutOfRangeException(nameof(changesType), changesType, null);
            }
        }

        #endregion
    }
}
