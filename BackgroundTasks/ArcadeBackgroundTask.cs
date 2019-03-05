using System;
using System.Diagnostics;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;
using BackgroundTasks.Models;
using BackgroundTasks.Services;
using Microsoft.Toolkit.Uwp.Notifications;

namespace BackgroundTasks
{
    public sealed class ArcadeBackgroundTask : IBackgroundTask
    {
        private const string MODE_IMAGE_URL_MASK = "https://overwatcharcade.today/img/modes/{0}.jpg";
        private const string MODE_LARGE_IMG_URL_MASK = "https://overwatcharcade.today/img/modes_large/{0}.jpg";

        private const string newDailyArcade = "New daily arcade";
        private const string newWeeklyArcade = "New weekly arcade";
        private const string newPermanentArcade = "New permanent arcade";

        private readonly DataService _dataService = new DataService();
        private readonly SettingsService _settingsService = new SettingsService();

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();

            try
            {
                if (_settingsService.GetNoCache("NotificationsEnabled", true))
                {
                    var todayArcades = await _dataService.GetTodayArcadeAsync();
                    
                    if (todayArcades.UpdatedAt > _settingsService.GetNoCache("UpdatedAt", DateTime.MinValue))
                    {
                        var lastData = _settingsService.Get<ArcadeDailyData>("LastData");
                        _settingsService.Set("LastData", todayArcades);
                        
                        if (lastData == null)
                        {
                            ShowTileToast(todayArcades.TileLarge, newWeeklyArcade);
                            ShowTileToast(todayArcades.TileDaily, newDailyArcade);
                            ShowTileToast(todayArcades.TileWeekly1, newWeeklyArcade);
                            ShowTileToast(todayArcades.TileWeekly2, newWeeklyArcade);
                            ShowTileToast(todayArcades.TilePermanent, newPermanentArcade);
                        }
                        else
                        {
                            if (lastData.TileLarge.Id != todayArcades.TileLarge.Id)
                                ShowTileToast(todayArcades.TileLarge, newDailyArcade);
                            if (lastData.TileDaily.Id != todayArcades.TileDaily.Id)
                                ShowTileToast(todayArcades.TileDaily, newDailyArcade);
                            if (lastData.TileWeekly1.Id != todayArcades.TileWeekly1.Id)
                                ShowTileToast(todayArcades.TileWeekly1, newWeeklyArcade);
                            if (lastData.TileWeekly2.Id != todayArcades.TileWeekly2.Id)
                                ShowTileToast(todayArcades.TileWeekly2, newWeeklyArcade);
                            if (lastData.TilePermanent.Id != todayArcades.TilePermanent.Id)
                                ShowTileToast(todayArcades.TilePermanent, newPermanentArcade);
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
                            Source = TileToImageUrl(data)
                        }
                    }
                }
            };

            var notification = new ToastNotification(toastContent.GetXml());
            ToastNotificationManager.CreateToastNotifier().Show(notification);
        }

        private static string TileToImageUrl(ArcadeTileData tile, bool isLargeTile = false)
        {
            return String.Format(isLargeTile ? MODE_LARGE_IMG_URL_MASK : MODE_IMAGE_URL_MASK, tile.Code);
        }
    }
}
