using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;

namespace OWArcadeToday.Helpers
{
    public static class WindowHelper
    {
        public static void HideTitleBar()
        {
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
        }

        public static void ShowTitleBar()
        {
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = false;
        }

        public static void SetTitleBarColors()
        {
            var view = ApplicationView.GetForCurrentView();
            view.TitleBar.ButtonBackgroundColor = Colors.Transparent;
            view.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
        }

        public static void ResetTitleBarColors()
        {
            var view = ApplicationView.GetForCurrentView();
            view.TitleBar.ButtonBackgroundColor = null;
            view.TitleBar.ButtonInactiveBackgroundColor = null;
        }
    }
}
