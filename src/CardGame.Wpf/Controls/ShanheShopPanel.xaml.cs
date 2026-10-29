using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
namespace CardGame.Wpf.Controls;
public partial class ShanheShopPanel : UserControl
{
    private ToolTip? _keyboardProductTip;

    public ShanheShopPanel()
    {
        InitializeComponent();
        MarketCategories.SelectionChanged += (_, _) => { CloseProductRuleTip(); MarketCardsScroll.ScrollToLeftEnd(); };
        MarketCardsScroll.ScrollChanged += (_, _) => UpdatePageNavigation();
        Loaded += (_, _) => UpdatePageNavigation();
        Unloaded += (_, _) => CloseProductRuleTip();
        IsVisibleChanged += (_, _) => { if (!IsVisible) CloseProductRuleTip(); };
        MarketCardsScroll.PreviewMouseWheel += (_, args) =>
        {
            for (var current = args.OriginalSource as DependencyObject; current is not null && current != MarketCardsScroll;
                 current = current is Visual ? VisualTreeHelper.GetParent(current) : LogicalTreeHelper.GetParent(current))
                if (current is ScrollViewer { ScrollableHeight: > 0 }) return;
            if (MarketCardsScroll.ScrollableWidth <= 0 ||
                MarketCardsScroll.ScrollableHeight > 0 && !Keyboard.Modifiers.HasFlag(ModifierKeys.Shift)) return;
            MarketCardsScroll.ScrollToHorizontalOffset(MarketCardsScroll.HorizontalOffset - args.Delta * 1.5);
            args.Handled = true;
        };
    }

    public void FocusMarket() => MarketCloseButton.Focus();

    private void Product_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
        if (sender is not Border { IsKeyboardFocused: true, ToolTip: ToolTip tip } product) return;
        CloseProductRuleTip();
        _keyboardProductTip = tip;
        tip.PlacementTarget = product;
        tip.IsOpen = true;
    }

    private void Product_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
        if (sender is Border { IsKeyboardFocusWithin: false }) CloseProductRuleTip();
    }

    private void Product_Unloaded(object sender, RoutedEventArgs e)
    {
        if (ReferenceEquals(_keyboardProductTip?.PlacementTarget, sender)) CloseProductRuleTip();
    }

    private void Product_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Handled || e.KeyboardDevice.Modifiers != ModifierKeys.None ||
            sender is not Border { IsKeyboardFocused: true, ToolTip: ToolTip tip } product) return;
        if (e.Key is Key.Enter or Key.Space)
        {
            _keyboardProductTip = tip;
            tip.PlacementTarget = product;
            tip.IsOpen = true;
            e.Handled = true;
            return;
        }
        if (!tip.IsOpen || tip.Content is not ScrollViewer { ScrollableHeight: > 0 } rules) return;
        switch (e.Key)
        {
            case Key.PageDown: rules.PageDown(); break;
            case Key.PageUp: rules.PageUp(); break;
            case Key.Home: rules.ScrollToTop(); break;
            case Key.End: rules.ScrollToBottom(); break;
            default: return;
        }
        e.Handled = true;
    }

    private void CloseProductRuleTip()
    {
        if (_keyboardProductTip is { } tip) tip.IsOpen = false;
        _keyboardProductTip = null;
    }

    private double PageDistance => Math.Max(276, Math.Floor(MarketCardsScroll.ViewportWidth / 276) * 276);
    private void PreviousPage(object sender, RoutedEventArgs e) =>
        MarketCardsScroll.ScrollToHorizontalOffset(Math.Max(0, MarketCardsScroll.HorizontalOffset - PageDistance));
    private void NextPage(object sender, RoutedEventArgs e) =>
        MarketCardsScroll.ScrollToHorizontalOffset(MarketCardsScroll.HorizontalOffset + PageDistance);
    private void UpdatePageNavigation()
    {
        MarketPreviousPageButton.IsEnabled = MarketCardsScroll.HorizontalOffset > 0.5;
        MarketNextPageButton.IsEnabled = MarketCardsScroll.HorizontalOffset + 0.5 < MarketCardsScroll.ScrollableWidth;
    }
}
