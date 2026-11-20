using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using CardGame.Wpf.ViewModels;

namespace CardGame.Wpf.Controls;

public partial class DeckReorderPicker : UserControl
{
    private Point _pressedAt;
    private FrameworkElement? _pressedSurface;
    private sealed record CardDrag(MainViewModel Model, CardViewModel Card);

    public DeckReorderPicker()
    {
        InitializeComponent();
        IsVisibleChanged += (_, _) =>
        {
            if (!IsVisible) ClearDragPreview();
        };
    }

    private void Card_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is not FrameworkElement { DataContext: CardViewModel card } surface ||
            DataContext is not MainViewModel model || !model.CanDragDeckOrderCard(card) ||
            !surface.CaptureMouse()) return;
        _pressedAt = e.GetPosition(DragOverlay);
        _pressedSurface = surface;
    }

    private void Picker_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e) =>
        _pressedSurface?.ReleaseMouseCapture();

    private void Card_LostMouseCapture(object sender, MouseEventArgs e)
    {
        if (ReferenceEquals(sender, _pressedSurface)) _pressedSurface = null;
    }

    private void Card_PreviewMouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton != MouseButtonState.Pressed || _pressedSurface is not { DataContext: CardViewModel card } surface ||
            !ReferenceEquals(sender, surface) ||
            DataContext is not MainViewModel model || !model.CanDragDeckOrderCard(card)) return;
        var point = e.GetPosition(DragOverlay);
        var distance = DragOverlay.PointToScreen(point) - DragOverlay.PointToScreen(_pressedAt);
        var dpi = VisualTreeHelper.GetDpi(DragOverlay);
        if (Math.Abs(distance.X) < SystemParameters.MinimumHorizontalDragDistance * dpi.DpiScaleX &&
            Math.Abs(distance.Y) < SystemParameters.MinimumVerticalDragDistance * dpi.DpiScaleY) return;
        surface.ReleaseMouseCapture();
        e.Handled = true;

        BeginDragPreview(card, point);
        try
        {
            DragDrop.DoDragDrop(surface, new DataObject(typeof(CardDrag), new CardDrag(model, card)), DragDropEffects.Move);
        }
        finally
        {
            SettleDragPreview(card);
        }
    }

    private void Picker_PreviewDragOver(object sender, DragEventArgs e)
    {
        e.Effects = DragDropEffects.None;
        if (_dragPreview is not null && GetLocalDrag(e) is not null)
            UpdateDragPreview(e.GetPosition(DragOverlay));
    }

    private CardDrag? GetLocalDrag(DragEventArgs e) =>
        e.Data.GetData(typeof(CardDrag)) is CardDrag drag && ReferenceEquals(drag.Model, DataContext) ? drag : null;

    private void Row_DragOver(object sender, DragEventArgs e)
    {
        var bottom = ReferenceEquals(sender, DeckBottomRow);
        var items = bottom ? DeckBottomItems : DeckTopItems;
        e.Effects = GetLocalDrag(e) is { } drag && drag.Model.CanDropDeckOrderCard(drag.Card, bottom, GetDropIndex(items, e))
            ? DragDropEffects.Move : DragDropEffects.None;
        e.Handled = true;
    }

    private void Row_Drop(object sender, DragEventArgs e)
    {
        e.Handled = true;
        e.Effects = DragDropEffects.None;
        if (GetLocalDrag(e) is not { } drag) return;
        var bottom = ReferenceEquals(sender, DeckBottomRow);
        var items = bottom ? DeckBottomItems : DeckTopItems;
        var index = GetDropIndex(items, e);
        UpdateDragPreview(e.GetPosition(DragOverlay));
        var positions = _dragPreview is not null && MotionEnabled ? CaptureCardPositions(drag.Card) : null;
        if (!drag.Model.MoveDeckOrderCard(drag.Card, bottom, index)) return;
        e.Effects = DragDropEffects.Move;
        if (positions is not null) AnimateCardReflow(positions);
    }

    private static int GetDropIndex(ItemsControl items, DragEventArgs e)
    {
        if (items.ItemContainerGenerator.ContainerFromIndex(0) is not FrameworkElement first ||
            VisualTreeHelper.GetParent(first) is not Panel panel) return items.Items.Count;
        var pointer = e.GetPosition(panel);
        var lineTop = LayoutInformation.GetLayoutSlot(first).Top;
        int? insertion = null;
        for (var index = 0; index < items.Items.Count; index++)
        {
            if (items.ItemContainerGenerator.ContainerFromIndex(index) is not FrameworkElement container) continue;
            // Use the layout slot so an earlier slide cannot move a drop target away from the pointer.
            var slot = LayoutInformation.GetLayoutSlot(container);
            if (slot.Top > lineTop)
            {
                if (pointer.Y < slot.Top) return insertion ?? index;
                lineTop = slot.Top;
                insertion = null;
            }
            if (pointer.X < slot.Left + slot.Width / 2) insertion ??= index;
        }
        return insertion ?? items.Items.Count;
    }
}
