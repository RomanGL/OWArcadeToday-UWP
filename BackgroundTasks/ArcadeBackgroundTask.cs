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

        private readonly DataService _dataService = new DataService();

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();
            var settingsService = new SettingsService();

            try
            {
                if (settingsService.GetNoCache("NotificationsEnabled", true))
                {
                    var todayArcades = await _dataService.GetTodayArcadeAsync();
                    ShowToasts(todayArcades);
                    if (todayArcades.UpdatedAt > settingsService.GetNoCache("UpdatedAt", DateTime.MinValue))
                    {

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

        private void ShowToasts(ArcadeDailyData data)
        {
            ShowDailyToast(data.TileDaily);
        }

        private void ShowDailyToast(ArcadeTileData data)
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
                                Text = "New daily arcade",
                                HintMaxLines = 1
                            },
                            new AdaptiveText()
                            {
                                Text = "Total Mayhem"
                            },
                            new AdaptiveGroup()
                            {
                                Children =
                                {
                                    new AdaptiveSubgroup()
                                    {
                                        Children =
                                        {
                                            new AdaptiveText()
                                            {
                                                Text = "Players",
                                                HintStyle = AdaptiveTextStyle.Body
                                            },
                                            new AdaptiveText()
                                            {
                                                Text = "6v6",
                                                HintStyle = AdaptiveTextStyle.CaptionSubtle
                                            }
                                        }
                                    },
                                    new AdaptiveSubgroup()
                                    {
                                        Children =
                                        {
                                            new AdaptiveText()
                                            {
                                                Text = "Power up and embrace the chaos.",
                                                HintStyle = AdaptiveTextStyle.CaptionSubtle,
                                                HintWrap = true,
                                                HintAlign = AdaptiveTextAlign.Right
                                            }
                                        },
                                        HintTextStacking = AdaptiveSubgroupTextStacking.Bottom
                                    }
                                }
                            }
                        },
                        HeroImage = new ToastGenericHeroImage()
                        {
                            Source = "https://overwatcharcade.today/img/modes/totalmayhem.jpg"
                        }
                    }
                }
            };

            // Create the toast notification
            var toastNotif = new ToastNotification(toastContent.GetXml());

            // And send the notification
            ToastNotificationManager.CreateToastNotifier().Show(toastNotif);
        }

        private static string TileToImageUrl(ArcadeTileData tile, bool isLargeTile = false)
        {
            return String.Format(isLargeTile ? MODE_LARGE_IMG_URL_MASK : MODE_IMAGE_URL_MASK, tile.Code);
        }
    }
}
