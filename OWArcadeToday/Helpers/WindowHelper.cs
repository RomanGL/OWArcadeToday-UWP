using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;

namespace OWArcadeToday.Helpers
{
    /// <summary>
    /// Represents a window helper.
    /// </summary>
    public static class WindowHelper
    {
        #region Public Methods

        /// <summary>
        /// Hides a window title bar.
        /// </summary>
        public static void HideTitleBar()
        {
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
        }

        /// <summary>
        /// Shows a window title bar.
        /// </summary>
        public static void ShowTitleBar()
        {
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = false;
        }

        /// <summary>
        /// Sets custom window title bar colors.
        /// </summary>
        public static void SetTitleBarColors()
        {
            var view = ApplicationView.GetForCurrentView();
            view.TitleBar.ButtonBackgroundColor = Colors.Transparent;
            view.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
        }

        /// <summary>
        /// Resets window title bar colors to default values.
        /// </summary>
        public static void ResetTitleBarColors()
        {
            var view = ApplicationView.GetForCurrentView();
            view.TitleBar.ButtonBackgroundColor = null;
            view.TitleBar.ButtonInactiveBackgroundColor = null;
        }

        #endregion
    }
}