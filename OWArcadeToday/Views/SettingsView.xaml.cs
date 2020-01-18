using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using OWArcadeToday.Services;
using Microsoft.Toolkit.Uwp.Helpers;

namespace OWArcadeToday.Views
{
    public sealed partial class SettingsView : Page
    {
        private const string NOTIFICATIONS_PARAM_NAME = "NotificationsEnabled";
        private readonly SettingsService settingsService = new SettingsService();

        public SettingsView()
        {
            this.InitializeComponent();
            this.Loaded += SettingsView_Loaded;
        }

        private void SettingsView_Loaded(object sender, RoutedEventArgs e)
        {
            notificationsToggle.IsOn = settingsService.Get(NOTIFICATIONS_PARAM_NAME, true);
            var ver = SystemInformation.ApplicationVersion;
            VersionText.Text = $"{ver.Major}.{ver.Minor}.{ver.Build}.{ver.Revision}";
        }

        private void ToggleSwitch_OnToggled(object sender, RoutedEventArgs e)
        {
            settingsService.Set(NOTIFICATIONS_PARAM_NAME, notificationsToggle.IsOn);
        }
    }
}
