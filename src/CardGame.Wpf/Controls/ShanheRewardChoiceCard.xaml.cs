using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using CardGame.Wpf.ViewModels;

namespace CardGame.Wpf.Controls;

/// <summary>The same source reward frame is used by camp growth and battle spoils.</summary>
public partial class ShanheRewardChoiceCard : UserControl
{
    public static readonly DependencyProperty HeadingProperty = TextProperty(nameof(Heading));
    public static readonly DependencyProperty TitleProperty = TextProperty(nameof(Title));
    public static readonly DependencyProperty DescriptionProperty = TextProperty(nameof(Description));
    public static readonly DependencyProperty FooterProperty = TextProperty(nameof(Footer));
    public static readonly DependencyProperty FallbackTextProperty = TextProperty(nameof(FallbackText));
    public static readonly DependencyProperty ArtworkProperty = DependencyProperty.Register(nameof(Artwork), typeof(ImageSource), typeof(ShanheRewardChoiceCard));
    public static readonly DependencyProperty ArtworkContentProperty = DependencyProperty.Register(nameof(ArtworkContent), typeof(object), typeof(ShanheRewardChoiceCard));
    public static readonly DependencyProperty ArtworkContentTemplateProperty = DependencyProperty.Register(nameof(ArtworkContentTemplate), typeof(DataTemplate), typeof(ShanheRewardChoiceCard));
    public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(nameof(IsSelected), typeof(bool), typeof(ShanheRewardChoiceCard), new PropertyMetadata(false));
    private static readonly DependencyPropertyKey IsChoiceFocusedPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsChoiceFocused), typeof(bool), typeof(ShanheRewardChoiceCard), new PropertyMetadata(false));
    public static readonly DependencyProperty IsChoiceFocusedProperty = IsChoiceFocusedPropertyKey.DependencyProperty;
    public static readonly DependencyProperty RarityProperty = DependencyProperty.Register(nameof(Rarity), typeof(int), typeof(ShanheRewardChoiceCard), new PropertyMetadata(2, SkinChanged));
    public static readonly DependencyProperty TypeIconIndexProperty = DependencyProperty.Register(nameof(TypeIconIndex), typeof(int), typeof(ShanheRewardChoiceCard), new PropertyMetadata(0, SkinChanged));
    private static readonly DependencyPropertyKey FrameArtworkPropertyKey = DependencyProperty.RegisterReadOnly(nameof(FrameArtwork), typeof(ImageSource), typeof(ShanheRewardChoiceCard), new PropertyMetadata(null));
    private static readonly DependencyPropertyKey NameArtworkPropertyKey = DependencyProperty.RegisterReadOnly(nameof(NameArtwork), typeof(ImageSource), typeof(ShanheRewardChoiceCard), new PropertyMetadata(null));
    private static readonly DependencyPropertyKey TypeArtworkPropertyKey = DependencyProperty.RegisterReadOnly(nameof(TypeArtwork), typeof(ImageSource), typeof(ShanheRewardChoiceCard), new PropertyMetadata(null));
    public static readonly DependencyProperty FrameArtworkProperty = FrameArtworkPropertyKey.DependencyProperty;
    public static readonly DependencyProperty NameArtworkProperty = NameArtworkPropertyKey.DependencyProperty;
    public static readonly DependencyProperty TypeArtworkProperty = TypeArtworkPropertyKey.DependencyProperty;

    private Control? _choiceOwner;

    public ShanheRewardChoiceCard()
    {
        InitializeComponent();
        RefreshSkin();
        Loaded += ChoiceLoaded;
        Unloaded += ChoiceUnloaded;
    }
    public string Heading { get => (string)GetValue(HeadingProperty); set => SetValue(HeadingProperty, value); }
    public string Title { get => (string)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }
    public string Description { get => (string)GetValue(DescriptionProperty); set => SetValue(DescriptionProperty, value); }
    public string Footer { get => (string)GetValue(FooterProperty); set => SetValue(FooterProperty, value); }
    public string FallbackText { get => (string)GetValue(FallbackTextProperty); set => SetValue(FallbackTextProperty, value); }
    public ImageSource? Artwork { get => (ImageSource?)GetValue(ArtworkProperty); set => SetValue(ArtworkProperty, value); }
    public object? ArtworkContent { get => GetValue(ArtworkContentProperty); set => SetValue(ArtworkContentProperty, value); }
    public DataTemplate? ArtworkContentTemplate { get => (DataTemplate?)GetValue(ArtworkContentTemplateProperty); set => SetValue(ArtworkContentTemplateProperty, value); }
    public bool IsSelected { get => (bool)GetValue(IsSelectedProperty); set => SetValue(IsSelectedProperty, value); }
    public bool IsChoiceFocused => (bool)GetValue(IsChoiceFocusedProperty);
    public int Rarity { get => (int)GetValue(RarityProperty); set => SetValue(RarityProperty, value); }
    public int TypeIconIndex { get => (int)GetValue(TypeIconIndexProperty); set => SetValue(TypeIconIndexProperty, value); }
    public ImageSource? FrameArtwork => (ImageSource?)GetValue(FrameArtworkProperty);
    public ImageSource? NameArtwork => (ImageSource?)GetValue(NameArtworkProperty);
    public ImageSource? TypeArtwork => (ImageSource?)GetValue(TypeArtworkProperty);

    private void ChoiceLoaded(object sender, RoutedEventArgs e)
    {
        DetachChoiceOwner();
        for (var ancestor = VisualTreeHelper.GetParent(this); ancestor is not null; ancestor = VisualTreeHelper.GetParent(ancestor))
        {
            if (ancestor is not (ButtonBase or ListBoxItem)) continue;
            _choiceOwner = (Control)ancestor;
            _choiceOwner.IsKeyboardFocusWithinChanged += ChoiceFocusChanged;
            _choiceOwner.PreviewKeyDown += ChoicePreviewKeyDown;
            SetValue(IsChoiceFocusedPropertyKey, _choiceOwner.IsKeyboardFocusWithin);
            break;
        }
    }

    private void ChoiceUnloaded(object sender, RoutedEventArgs e) => DetachChoiceOwner();

    private void DetachChoiceOwner()
    {
        if (_choiceOwner is not null)
        {
            _choiceOwner.IsKeyboardFocusWithinChanged -= ChoiceFocusChanged;
            _choiceOwner.PreviewKeyDown -= ChoicePreviewKeyDown;
            _choiceOwner = null;
        }
        SetValue(IsChoiceFocusedPropertyKey, false);
    }

    private void ChoiceFocusChanged(object sender, DependencyPropertyChangedEventArgs e) =>
        SetValue(IsChoiceFocusedPropertyKey, _choiceOwner?.IsKeyboardFocusWithin == true);

    private void ChoicePreviewKeyDown(object sender, KeyEventArgs e)
    {
        // Keep the description unfocusable so body clicks still select the card.
        // Its owner supplies read-only paging without taking choice/confirm keys.
        if (e.Handled || _choiceOwner?.IsKeyboardFocused != true || e.KeyboardDevice.Modifiers != ModifierKeys.None ||
            ChoiceDescriptionScroll.ScrollableHeight <= 0) return;
        switch (e.Key)
        {
            case Key.PageDown: ChoiceDescriptionScroll.PageDown(); break;
            case Key.PageUp: ChoiceDescriptionScroll.PageUp(); break;
            case Key.Home: ChoiceDescriptionScroll.ScrollToTop(); break;
            case Key.End: ChoiceDescriptionScroll.ScrollToBottom(); break;
            default: return;
        }
        e.Handled = true;
    }

    private static DependencyProperty TextProperty(string name) => DependencyProperty.Register(name, typeof(string), typeof(ShanheRewardChoiceCard), new PropertyMetadata(string.Empty));
    private static void SkinChanged(DependencyObject owner, DependencyPropertyChangedEventArgs args) => ((ShanheRewardChoiceCard)owner).RefreshSkin();
    private void RefreshSkin()
    {
        var quality = Math.Clamp(Rarity, 1, 4);
        SetValue(FrameArtworkPropertyKey, WushuangArt.Get($"rogue_award_quality_bg_{quality}.png"));
        SetValue(NameArtworkPropertyKey, WushuangArt.Get($"rogue_award_name_bg_{quality}.png"));
        SetValue(TypeArtworkPropertyKey, WushuangArt.Get($"rogue_award_type_icon_{Math.Clamp(TypeIconIndex, 0, 4)}.png"));
    }
}
