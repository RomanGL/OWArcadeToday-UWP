using Microsoft.Toolkit.Uwp.Helpers;
using OWArcadeToday.Helpers;
using OWArcadeToday.Views;
using System;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Background;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace OWArcadeToday
{
    /// <summary>
    /// Represents a main application class.
    /// </summary>
    public sealed partial class App : Application
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Overrides of Application

        /// <inheritdoc />
        protected override void OnLaunched(LaunchActivatedEventArgs e)
            => InitializeApplication(e);

        /// <inheritdoc />
        protected override void OnActivated(IActivatedEventArgs e)
            => InitializeApplication(e);

        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes the application.
        /// </summary>
        /// <param name="e">The application activation event args.</param>
        private void InitializeApplication(IActivatedEventArgs e)
        {
            var rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
            {
                rootFrame = new Frame();
                rootFrame.Navigated += OnRootFrameNavigated;
                SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;

                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                rootFrame.Navigate(typeof(MainView));
            }

            Window.Current.Activate();

            WindowHelper.HideTitleBar();
            WindowHelper.SetTitleBarColors();

            RegisterBackgroundTask();
        }

        /// <summary>
        /// Occurs when the back navigation requested.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            if (Window.Current.Content is Frame rootFrame && rootFrame.CanGoBack)
            {
                rootFrame.GoBack(new DrillInNavigationTransitionInfo());
            }
        }

        /// <summary>
        /// Occurs when the root frame navigated.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void OnRootFrameNavigated(object sender, NavigationEventArgs e)
        {
            if (!(Window.Current.Content is Frame frame))
                return;

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = frame.CanGoBack
                ? AppViewBackButtonVisibility.Visible
                : AppViewBackButtonVisibility.Collapsed;
        }

        /// <summary>
        /// Performs a registration of a background task.
        /// </summary>
        private async void RegisterBackgroundTask()
        {
            const string taskName = "ArcadeBackgroundTask";
            const string taskEntryPoint = "BackgroundTasks.ArcadeBackgroundTask";

            var accessStatus = await BackgroundExecutionManager.RequestAccessAsync();
            var isRegistered = BackgroundTaskHelper.IsBackgroundTaskRegistered(taskName);

            if (accessStatus == BackgroundAccessStatus.DeniedByUser ||
                accessStatus == BackgroundAccessStatus.DeniedBySystemPolicy ||
                isRegistered)
            {
                return;
            }

            var trigger = new TimeTrigger(120, false);

            BackgroundTaskHelper.Register(taskName,
                                          taskEntryPoint,
                                          trigger,
                                          conditions: new SystemCondition(SystemConditionType.InternetAvailable));
        }

        #endregion
    }
}