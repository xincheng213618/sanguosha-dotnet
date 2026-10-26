using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
namespace CardGame.Wpf.Controls;
public partial class ShanheShopPanel : UserControl
{
    public ShanheShopPanel()
    {
        InitializeComponent();
        MarketCategories.SelectionChanged += (_, _) => MarketCardsScroll.ScrollToLeftEnd();
        MarketCardsScroll.ScrollChanged += (_, _) => UpdatePageNavigation();
        Loaded += (_, _) => UpdatePageNavigation();
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
