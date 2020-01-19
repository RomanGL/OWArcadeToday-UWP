using JetBrains.Annotations;
using OWArcadeToday.Core.Models;
using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace OWArcadeToday.Controls
{
    /// <summary>
    /// Represents an Overwatch Arcade tile control.
    /// </summary>
    public sealed partial class Tile : UserControl
    {
        #region Dependency Properties

        public static readonly DependencyProperty TileDataProperty = DependencyProperty.Register(
            nameof(TileData),
            typeof(ArcadeTileData),
            typeof(Tile),
            new PropertyMetadata(default, OnTileDataChanged));

        public static readonly DependencyProperty TileImageProperty = DependencyProperty.Register(
            nameof(TileImage),
            typeof(ImageSource),
            typeof(Tile),
            new PropertyMetadata(default));

        public static readonly DependencyProperty IsLargeTileProperty = DependencyProperty.Register(
            nameof(IsLargeTile),
            typeof(bool),
            typeof(Tile),
            new PropertyMetadata(default, OnIsLargeTileChanged));

        public static readonly DependencyProperty IsBadgeVisibleProperty = DependencyProperty.Register(
            nameof(IsBadgeVisible),
            typeof(bool),
            typeof(Tile),
            new PropertyMetadata(default, OnIsBadgeVisibleChanged));

        public static readonly DependencyProperty BadgeTextProperty = DependencyProperty.Register(
            nameof(BadgeText),
            typeof(string),
            typeof(Tile),
            new PropertyMetadata(default));

        public static readonly DependencyProperty BadgeBackgroundProperty = DependencyProperty.Register(
            nameof(BadgeBackground),
            typeof(Brush),
            typeof(Tile),
            new PropertyMetadata(new SolidColorBrush(Colors.ForestGreen)));

        public static readonly DependencyProperty BadgeBackground2Property = DependencyProperty.Register(
            nameof(BadgeBackground2),
            typeof(Brush),
            typeof(Tile),
            new PropertyMetadata(new SolidColorBrush(Colors.DarkGreen)));

        #endregion

        #region Fields

        private const string MODE_IMAGE_URL_MASK = "https://overwatcharcade.today/img/modes/{0}.jpg";
        private const string MODE_LARGE_IMG_URL_MASK = "https://overwatcharcade.today/img/modes_large/{0}.jpg";

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Tile"/> class.
        /// </summary>
        public Tile()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Overwatch Arcade tile data.
        /// </summary>
        [CanBeNull]
        public ArcadeTileData TileData
        {
            get => (ArcadeTileData)GetValue(TileDataProperty);
            set => SetValue(TileDataProperty, value);
        }

        /// <summary>
        /// Gets or sets the tile background image.
        /// </summary>
        [CanBeNull]
        public ImageSource TileImage
        {
            get => (ImageSource)GetValue(TileImageProperty);
            set => SetValue(TileImageProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <seealso cref="Tile"/> is a large.
        /// </summary>
        public bool IsLargeTile
        {
            get => (bool)GetValue(IsLargeTileProperty);
            set => SetValue(IsLargeTileProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether a badge is visible.
        /// </summary>
        public bool IsBadgeVisible
        {
            get => (bool)GetValue(IsBadgeVisibleProperty);
            set => SetValue(IsBadgeVisibleProperty, value);
        }

        /// <summary>
        /// Gets or sets the badge text.
        /// </summary>
        [CanBeNull]
        public string BadgeText
        {
            get => (string)GetValue(BadgeTextProperty);
            set => SetValue(BadgeTextProperty, value);
        }

        /// <summary>
        /// Gets or sets the badge background.
        /// </summary>
        [CanBeNull]
        public Brush BadgeBackground
        {
            get => (Brush)GetValue(BadgeBackgroundProperty);
            set => SetValue(BadgeBackgroundProperty, value);
        }

        /// <summary>
        /// Gets or sets the second badge background.
        /// </summary>
        [CanBeNull]
        public Brush BadgeBackground2
        {
            get => (Brush)GetValue(BadgeBackground2Property);
            set => SetValue(BadgeBackground2Property, value);
        }

        #endregion

        #region Dependency Property Callbacks

        /// <inheritdoc cref="PropertyChangedCallback"/>
        private static void OnTileDataChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var control = (Tile)sender;
            control.UpdateImage();
        }

        /// <inheritdoc cref="PropertyChangedCallback"/>
        private static void OnIsLargeTileChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var control = (Tile)sender;
            control.UpdateImage();
        }

        /// <inheritdoc cref="PropertyChangedCallback"/>
        private static void OnIsBadgeVisibleChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var control = (Tile)sender;
            control.UpdateBadge();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Updates the <seealso cref="TileImage"/>.
        /// </summary>
        private void UpdateImage()
        {
            if (TileData == null)
            {
                TileImage = null;
            }
            else
            {
                TileImage = new BitmapImage(new Uri(TileData.Image));
            }
        }

        /// <summary>
        /// Updates the badge state.
        /// </summary>
        private void UpdateBadge()
        {
            badgeBlock.Visibility = IsBadgeVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        #endregion
    }
}