using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using OWArcadeToday.Models;

namespace OWArcadeToday.Controls
{
    public sealed partial class Tile : UserControl
    {
        private const string MODE_IMAGE_URL_MASK = "https://overwatcharcade.today/img/modes/{0}.jpg";
        private const string MODE_LARGE_IMG_URL_MASK = "https://overwatcharcade.today/img/modes_large/{0}.jpg";

        public Tile()
        {
            this.InitializeComponent();
        }

        public static readonly DependencyProperty TileDataProperty = DependencyProperty.Register(
            nameof(TileData), typeof(ArcadeTileData), typeof(Tile), new PropertyMetadata(default(ArcadeTileData), OnTileDataChanged));
        
        public static readonly DependencyProperty TileImageProperty = DependencyProperty.Register(
            nameof(TileImage), typeof(ImageSource), typeof(Tile), new PropertyMetadata(default(ImageSource)));

        public static readonly DependencyProperty IsLargeTileProperty = DependencyProperty.Register(
            nameof(IsLargeTile), typeof(bool), typeof(Tile), new PropertyMetadata(default(bool), OnIsLargeTileChanged));

        public static readonly DependencyProperty IsBadgeVisibleProperty = DependencyProperty.Register(
            nameof(IsBadgeVisible), typeof(bool), typeof(Tile), new PropertyMetadata(default(bool), OnIsBadgeVisibleChanged));
        
        public static readonly DependencyProperty BadgeTextProperty = DependencyProperty.Register(
            nameof(BadgeText), typeof(string), typeof(Tile), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty BadgeBackgroundProperty = DependencyProperty.Register(
            nameof(BadgeBackground), typeof(Brush), typeof(Tile), new PropertyMetadata(new SolidColorBrush(Colors.ForestGreen)));

        public static readonly DependencyProperty BadgeBackground2Property = DependencyProperty.Register(
            nameof(BadgeBackground2), typeof(Brush), typeof(Tile), new PropertyMetadata(new SolidColorBrush(Colors.DarkGreen)));
        
        public ArcadeTileData TileData
        {
            get => (ArcadeTileData)GetValue(TileDataProperty);
            set => SetValue(TileDataProperty, value);
        }

        public ImageSource TileImage
        {
            get => (ImageSource)GetValue(TileImageProperty);
            set => SetValue(TileImageProperty, value);
        }

        public bool IsLargeTile
        {
            get => (bool)GetValue(IsLargeTileProperty);
            set => SetValue(IsLargeTileProperty, value);
        }

        public bool IsBadgeVisible
        {
            get => (bool)GetValue(IsBadgeVisibleProperty);
            set => SetValue(IsBadgeVisibleProperty, value);
        }

        public string BadgeText
        {
            get => (string)GetValue(BadgeTextProperty);
            set => SetValue(BadgeTextProperty, value);
        }

        public Brush BadgeBackground
        {
            get => (Brush)GetValue(BadgeBackgroundProperty);
            set => SetValue(BadgeBackgroundProperty, value);
        }

        public Brush BadgeBackground2
        {
            get => (Brush)GetValue(BadgeBackground2Property);
            set => SetValue(BadgeBackground2Property, value);
        }

        private static void OnTileDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (Tile)d;
            control.UpdateImage();
        }

        private static void OnIsLargeTileChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (Tile)d;
            control.UpdateImage();
        }

        private static void OnIsBadgeVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (Tile)d;
            control.UpdateBadge();
        }

        private void UpdateImage()
        {
            if (TileData == null)
            {
                TileImage = null;
            }
            else
            {
                string url = String.Format(IsLargeTile ? MODE_LARGE_IMG_URL_MASK : MODE_IMAGE_URL_MASK, TileData.Code);

                TileImage = new BitmapImage(new Uri(url));
            }
        }

        private void UpdateBadge()
        {
            badgeBlock.Visibility = IsBadgeVisible ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
