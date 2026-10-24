using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CardGame.Wpf.ViewModels;

namespace CardGame.Wpf.Controls;

public partial class DeckReorderPicker : UserControl
{
    private Point _pressedAt;
    private CardViewModel? _pressedCard;
    private sealed record CardDrag(MainViewModel Model, CardViewModel Card);
    public DeckReorderPicker() => InitializeComponent();
    private void Card_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        _pressedAt = e.GetPosition(this);
        _pressedCard = (sender as FrameworkElement)?.DataContext as CardViewModel;
    }
    private void Card_PreviewMouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton != MouseButtonState.Pressed || _pressedCard is not { } card || DataContext is not MainViewModel model) return;
        var point = e.GetPosition(this);
        if (Math.Abs(point.X - _pressedAt.X) < SystemParameters.MinimumHorizontalDragDistance &&
            Math.Abs(point.Y - _pressedAt.Y) < SystemParameters.MinimumVerticalDragDistance) return;
        _pressedCard = null;
        // Use an in-process object; external drops cannot supply a card command parameter.
        DragDrop.DoDragDrop((DependencyObject)sender, new DataObject(typeof(CardDrag), new CardDrag(model, card)), DragDropEffects.Move);
        e.Handled = true;
    }
    private void Row_DragOver(object sender, DragEventArgs e)
    {
        e.Effects = e.Data.GetData(typeof(CardDrag)) is CardDrag drag && ReferenceEquals(drag.Model, DataContext)
            ? DragDropEffects.Move : DragDropEffects.None;
        e.Handled = true;
    }
    private void Row_Drop(object sender, DragEventArgs e)
    {
        if (sender is not Border row || DataContext is not MainViewModel model ||
            e.Data.GetData(typeof(CardDrag)) is not CardDrag drag || !ReferenceEquals(drag.Model, model)) return;
        var bottom = row.Tag as string == "bottom";
        var items = bottom ? model.DeckBottomCards : model.DeckTopCards;
        var index = items.Count;
        for (var target = e.OriginalSource as DependencyObject; target is not null && !ReferenceEquals(target, row);
            target = target is Visual ? VisualTreeHelper.GetParent(target) : LogicalTreeHelper.GetParent(target))
        {
            if (target is FrameworkElement { DataContext: CardViewModel targetCard } element && items.Contains(targetCard))
            {
                index = items.IndexOf(targetCard) + (e.GetPosition(element).X >= element.ActualWidth / 2 ? 1 : 0);
                break;
            }
        }
        if (model.MoveDeckOrderCard(drag.Card, bottom, index)) e.Effects = DragDropEffects.Move;
        e.Handled = true;
    }
}
