using System;
using Windows.ApplicationModel.Background;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using OWArcadeToday.Services;

namespace OWArcadeToday.Views
{
    public sealed partial class SettingsView : Page
    {
        private const string NOTIFICATIONS_PARAM_NAME = "NotificationsEnabled";
        private readonly SettingsService _settingsService = new SettingsService();

        public SettingsView()
        {
            this.InitializeComponent();
            this.Loaded += SettingsView_Loaded;
        }

        private void SettingsView_Loaded(object sender, RoutedEventArgs e)
        {
            notificationsToggle.IsOn = _settingsService.Get(NOTIFICATIONS_PARAM_NAME, true);
        }

        private void ToggleSwitch_OnToggled(object sender, RoutedEventArgs e)
        {
            _settingsService.Set(NOTIFICATIONS_PARAM_NAME, notificationsToggle.IsOn);
        }
    }
}
