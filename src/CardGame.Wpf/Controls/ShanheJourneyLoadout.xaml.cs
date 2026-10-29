using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CardGame.Wpf.Controls;

public partial class ShanheJourneyLoadout : UserControl
{
    private ToolTip? _keyboardRuleTip;

    public ShanheJourneyLoadout()
    {
        InitializeComponent();
        foreach (var scroll in new[] { ShanheJourneyHandScroll, ShanheJourneyTacticScroll, ShanheJourneySkillScroll, ShanheJourneyCompanionScroll })
            scroll.AddHandler(MouseWheelEvent, new MouseWheelEventHandler(ScrollHorizontally), handledEventsToo: true);
        IsVisibleChanged += (_, _) => { if (!IsVisible) { CloseRuleTip(); ShanheTraitsButton.IsChecked = false; } };
        Unloaded += (_, _) => { CloseRuleTip(); ShanheTraitsButton.IsChecked = false; };
    }

    private void Rule_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
        if (sender is not Border { IsKeyboardFocused: true, ToolTip: ToolTip tip } owner) return;
        CloseRuleTip();
        _keyboardRuleTip = tip;
        tip.PlacementTarget = owner;
        if (tip.Content is ScrollViewer rules) rules.ScrollToTop();
        tip.IsOpen = true;
    }

    private void Rule_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
        if (sender is Border { IsKeyboardFocusWithin: false }) CloseRuleTip();
    }

    private void Rule_Unloaded(object sender, RoutedEventArgs e)
    {
        if (ReferenceEquals(_keyboardRuleTip?.PlacementTarget, sender)) CloseRuleTip();
    }

    private void Rule_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Handled || e.KeyboardDevice.Modifiers != ModifierKeys.None ||
            sender is not Border { IsKeyboardFocused: true, ToolTip: ToolTip tip } owner) return;
        if (e.Key is Key.Enter or Key.Space)
        {
            _keyboardRuleTip = tip;
            tip.PlacementTarget = owner;
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

    private void CloseRuleTip()
    {
        if (_keyboardRuleTip is { } tip) tip.IsOpen = false;
        _keyboardRuleTip = null;
    }

    private void ScrollHorizontally(object sender, MouseWheelEventArgs e)
    {
        if (sender is not ScrollViewer scroll) return;

        // The ScrollViewer class handler consumes wheel input even when vertical
        // scrolling is disabled. Release it at horizontal edges so ancestors can scroll.
        if (e.Delta == 0 || Keyboard.Modifiers != ModifierKeys.None || scroll.ScrollableWidth <= 0)
        {
            e.Handled = false;
            return;
        }

        var offset = Math.Clamp(scroll.HorizontalOffset - e.Delta * 108.0 / 120,
            0, scroll.ScrollableWidth);
        if (offset == scroll.HorizontalOffset)
        {
            e.Handled = false;
            return;
        }

        scroll.ScrollToHorizontalOffset(offset);
        e.Handled = true;
    }

    private void KeepRightEdge(object sender, ScrollChangedEventArgs e)
    {
        // New columns appear to the left, preserving the original first column beside the hero.
        if (e.ExtentWidthChange != 0 && sender is ScrollViewer scroll) scroll.ScrollToRightEnd();
    }

    private void TraitsOpened(object sender, EventArgs e) => ((FrameworkElement)ShanheTraitsPopup.Child).MoveFocus(new TraversalRequest(FocusNavigationDirection.First));

    private void TraitsClosed(object sender, EventArgs e)
    {
        ShanheTraitsButton.IsChecked = false;
        if (IsVisible && IsEnabled) ShanheTraitsButton.Focus();
    }

    private void CloseTraits(object sender, RoutedEventArgs e) => ShanheTraitsButton.IsChecked = false;

    public bool HandleEscape()
    {
        if (_keyboardRuleTip is { IsOpen: true }) { CloseRuleTip(); return true; }
        if (!ShanheTraitsPopup.IsOpen) return false;
        ShanheTraitsButton.IsChecked = false;
        return true;
    }

    private void TraitsKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key != Key.Escape) return;
        e.Handled = HandleEscape();
    }
}
