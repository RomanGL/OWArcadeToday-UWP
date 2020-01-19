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
                            ShowTileToast(todayArcades.Modes.Tile1, GetUpdateDescription(todayArcades.Modes.Tile1.GameModeChangesType), true);
                            ShowTileToast(todayArcades.Modes.Tile2, GetUpdateDescription(todayArcades.Modes.Tile2.GameModeChangesType));
                            ShowTileToast(todayArcades.Modes.Tile3, GetUpdateDescription(todayArcades.Modes.Tile3.GameModeChangesType));
                            ShowTileToast(todayArcades.Modes.Tile4, GetUpdateDescription(todayArcades.Modes.Tile4.GameModeChangesType));
                            ShowTileToast(todayArcades.Modes.Tile5, GetUpdateDescription(todayArcades.Modes.Tile5.GameModeChangesType));
                            ShowTileToast(todayArcades.Modes.Tile6, GetUpdateDescription(todayArcades.Modes.Tile6.GameModeChangesType));
                            ShowTileToast(todayArcades.Modes.Tile7, GetUpdateDescription(todayArcades.Modes.Tile7.GameModeChangesType));
                        }
                        else
                        {
                            if (lastData.Modes.Tile1.Name != todayArcades.Modes.Tile1.Name)
                            {
                                ShowTileToast(todayArcades.Modes.Tile1, GetUpdateDescription(todayArcades.Modes.Tile1.GameModeChangesType), true);
                            }

                            if (lastData.Modes.Tile2.Name != todayArcades.Modes.Tile2.Name)
                            {
                                ShowTileToast(todayArcades.Modes.Tile2, GetUpdateDescription(todayArcades.Modes.Tile2.GameModeChangesType));
                            }

                            if (lastData.Modes.Tile3.Name != todayArcades.Modes.Tile3.Name)
                            {
                                ShowTileToast(todayArcades.Modes.Tile3, GetUpdateDescription(todayArcades.Modes.Tile3.GameModeChangesType));
                            }

                            if (lastData.Modes.Tile4.Name != todayArcades.Modes.Tile4.Name)
                            {
                                ShowTileToast(todayArcades.Modes.Tile4, GetUpdateDescription(todayArcades.Modes.Tile4.GameModeChangesType));
                            }

                            if (lastData.Modes.Tile5.Name != todayArcades.Modes.Tile5.Name)
                            {
                                ShowTileToast(todayArcades.Modes.Tile5, GetUpdateDescription(todayArcades.Modes.Tile5.GameModeChangesType));
                            }

                            if (lastData.Modes.Tile6.Name != todayArcades.Modes.Tile6.Name)
                            {
                                ShowTileToast(todayArcades.Modes.Tile6, GetUpdateDescription(todayArcades.Modes.Tile6.GameModeChangesType));
                            }

                            if (lastData.Modes.Tile7.Name != todayArcades.Modes.Tile7.Name)
                            {
                                ShowTileToast(todayArcades.Modes.Tile7, GetUpdateDescription(todayArcades.Modes.Tile7.GameModeChangesType));
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
                                HintMaxLines = 1,
                            },
                            new AdaptiveText()
                            {
                                Text = data.Name,
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
                                                HintStyle = AdaptiveTextStyle.Body,
                                            },
                                        },
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
                                                HintAlign = AdaptiveTextAlign.Right,
                                            }
                                        },
                                    },
                                },
                            },
                        },
                        HeroImage = new ToastGenericHeroImage()
                        {
                            Source = data.Image,
                        },
                    },
                },
            };

            var notification = new ToastNotification(toastContent.GetXml());
            ToastNotificationManager.CreateToastNotifier().Show(notification);
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
