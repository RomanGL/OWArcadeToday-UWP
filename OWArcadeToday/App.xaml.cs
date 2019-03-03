using System;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Background;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Microsoft.Toolkit.Uwp.Helpers;
using OWArcadeToday.Helpers;
using OWArcadeToday.Views;

namespace OWArcadeToday
{
    sealed partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            var rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
            {
                rootFrame = new Frame();
                rootFrame.Navigated += RootFrame_Navigated;
                SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;

                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    rootFrame.Navigate(typeof(MainView), e.Arguments);
                }
                
                Window.Current.Activate();
            }

            WindowHelper.HideTitleBar();
            WindowHelper.SetTitleBarColors();

            RegisterBackgroundTask();
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            var rootFrame = Window.Current.Content as Frame;
            if (rootFrame != null && rootFrame.CanGoBack)
            {
                rootFrame.GoBack(new DrillInNavigationTransitionInfo());
            }
        }

        private void RootFrame_Navigated(object sender, NavigationEventArgs e)
        {
            var frame = Window.Current.Content as Frame;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = frame.CanGoBack ?
                AppViewBackButtonVisibility.Visible :
                AppViewBackButtonVisibility.Collapsed;
        }

        private async void RegisterBackgroundTask()
        {
            const string taskName = "ArcadeBackgroundTask";
            const string taskEntryPoint = "BackgroundTasks.ArcadeBackgroundTask";

            var accessStatus = await BackgroundExecutionManager.RequestAccessAsync();
            bool isRegistered = BackgroundTaskHelper.IsBackgroundTaskRegistered(taskName);

            if (isRegistered || accessStatus == BackgroundAccessStatus.DeniedByUser || accessStatus == BackgroundAccessStatus.DeniedBySystemPolicy)
                return;

            BackgroundTaskHelper.Register(
                taskName,
                taskEntryPoint,
                new TimeTrigger(15, false),
                conditions: new SystemCondition(SystemConditionType.InternetAvailable));
        }
    }
}
