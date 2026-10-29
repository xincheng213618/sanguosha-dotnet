using System.Collections.Specialized;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Input;

namespace CardGame.Wpf;

public partial class MainWindow
{
    private const int ExpandedTacticCount = 11;
    private const int CollapsedTacticCount = 3;
    private const double TacticBadgeWidth = 62;
    private const double TacticArrowWidth = 28;
    private bool _humanTacticsExpanded;

    private void InitializeHumanTactics()
    {
        ((INotifyCollectionChanged)HumanTacticItems.Items).CollectionChanged += (_, _) => UpdateHumanTacticsLayout();
        Battlefield.SizeChanged += (_, _) => UpdateHumanTacticsLayout();
        UpdateHumanTacticsLayout();
    }

    private void UpdateHumanTacticsLayout()
    {
        var count = HumanTacticItems.Items.Count;
        var canFold = count > ExpandedTacticCount;
        if (!canFold) _humanTacticsExpanded = false;
        var collapsed = canFold && !_humanTacticsExpanded;
        var limit = Math.Min(count, collapsed ? CollapsedTacticCount : ExpandedTacticCount);
        var available = Math.Max(TacticBadgeWidth, Battlefield.ActualWidth - 16);
        var arrows = canFold ? TacticArrowWidth : 0;
        var needsPages = !collapsed && count > limit;
        if (needsPages) arrows += 2 * TacticArrowWidth;
        var capacity = Math.Max(1, (int)((available - arrows) / TacticBadgeWidth));
        if (!collapsed && !needsPages && count > capacity)
        {
            needsPages = true;
            arrows += 2 * TacticArrowWidth;
            capacity = Math.Max(1, (int)((available - arrows) / TacticBadgeWidth));
        }
        HumanWushuangTactics.MaxWidth = available;
        HumanTacticsViewport.Width = Math.Min(limit, capacity) * TacticBadgeWidth;
        ToggleHumanTacticsButton.Visibility = canFold ? Visibility.Visible : Visibility.Collapsed;
        ToggleHumanTacticsButton.Content = _humanTacticsExpanded ? "»" : "«";
        var toggleText = _humanTacticsExpanded ? "收起战法" : "展开战法（每屏最多11个）";
        ToggleHumanTacticsButton.ToolTip = toggleText;
        AutomationProperties.SetName(ToggleHumanTacticsButton, toggleText);
        PreviousHumanTacticsButton.Visibility = NextHumanTacticsButton.Visibility = needsPages ? Visibility.Visible : Visibility.Collapsed;
        if (collapsed) HumanTacticsViewport.ScrollToLeftEnd();
        UpdateHumanTacticPageButtons();
    }

    private void ToggleHumanTactics_Click(object sender, RoutedEventArgs e)
    {
        _humanTacticsExpanded = !_humanTacticsExpanded;
        HumanTacticsViewport.ScrollToLeftEnd();
        UpdateHumanTacticsLayout();
    }

    private void PreviousHumanTactics_Click(object sender, RoutedEventArgs e) =>
        HumanTacticsViewport.ScrollToHorizontalOffset(HumanTacticsViewport.HorizontalOffset - HumanTacticsViewport.ViewportWidth);

    private void NextHumanTactics_Click(object sender, RoutedEventArgs e) =>
        HumanTacticsViewport.ScrollToHorizontalOffset(HumanTacticsViewport.HorizontalOffset + HumanTacticsViewport.ViewportWidth);

    private void HumanTactics_ScrollChanged(object sender, ScrollChangedEventArgs e) => UpdateHumanTacticPageButtons();

    private void UpdateHumanTacticPageButtons()
    {
        if (PreviousHumanTacticsButton is null || NextHumanTacticsButton is null) return;
        PreviousHumanTacticsButton.IsEnabled = HumanTacticsViewport.HorizontalOffset > 0.5;
        NextHumanTacticsButton.IsEnabled = HumanTacticsViewport.HorizontalOffset < HumanTacticsViewport.ScrollableWidth - 0.5;
    }

    private void HumanTactics_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
        if (NextHumanTacticsButton.Visibility != Visibility.Visible || HumanTacticsViewport.ScrollableWidth <= 0) return;
        HumanTacticsViewport.ScrollToHorizontalOffset(HumanTacticsViewport.HorizontalOffset - e.Delta / 120d * TacticBadgeWidth);
        e.Handled = true;
    }
}
