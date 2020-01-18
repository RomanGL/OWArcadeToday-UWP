using OWArcadeToday.Core.Services;
using OWArcadeToday.ViewModels;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace OWArcadeToday.Views
{
    public sealed partial class MainView : Page
    {
        public MainView()
        {
            this.InitializeComponent();

            this.Loaded += MainView_Loaded;
            this.Unloaded += MainView_Unloaded;
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        public MainViewModel Vm { get; } = new MainViewModel();

        private void MainView_Loaded(object sender, RoutedEventArgs e)
        {
            Vm.StartTimer();
            LoadData();
        }

        private void MainView_Unloaded(object sender, RoutedEventArgs e)
        {
            Vm.StopTimer();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsView), null, new DrillInNavigationTransitionInfo());
        }

        private void RetryButton_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private async void LoadData()
        {
            try
            {
                OnLoading();
                await Vm.LoadDataAsync();
                OnCompleted();
            }
            catch (NoDataException)
            {
                OnNoData();
            }
            catch (Exception)
            {
                OnError();
            }
        }

        private void OnLoading()
        {
            contentBlock.Visibility = Visibility.Collapsed;
            loadingBlock.Visibility = Visibility.Visible;
            errorBlock.Visibility = Visibility.Collapsed;
            noDataBlock.Visibility = Visibility.Collapsed;
        }

        private void OnError()
        {
            contentBlock.Visibility = Visibility.Collapsed;
            loadingBlock.Visibility = Visibility.Collapsed;
            errorBlock.Visibility = Visibility.Visible;
            noDataBlock.Visibility = Visibility.Collapsed;
        }

        private void OnNoData()
        {
            contentBlock.Visibility = Visibility.Collapsed;
            loadingBlock.Visibility = Visibility.Collapsed;
            errorBlock.Visibility = Visibility.Collapsed;
            noDataBlock.Visibility = Visibility.Visible;
        }

        private void OnCompleted()
        {
            contentBlock.Visibility = Visibility.Visible;
            loadingBlock.Visibility = Visibility.Collapsed;
            errorBlock.Visibility = Visibility.Collapsed;
            noDataBlock.Visibility = Visibility.Collapsed;
        }
    }
}