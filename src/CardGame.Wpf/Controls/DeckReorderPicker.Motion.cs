using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using CardGame.Wpf.ViewModels;

namespace CardGame.Wpf.Controls;

public partial class DeckReorderPicker
{
    private static readonly Duration SlideDuration = new(TimeSpan.FromMilliseconds(180));
    private DragPreview? _dragPreview;
    private bool MotionEnabled => IsVisible && InteractionMotion.GetMotionEnabled(this) && SystemParameters.ClientAreaAnimation;

    private sealed class DragPreview(FrameworkElement container, Border ghost, TranslateTransform position, Vector grabOffset)
    {
        public FrameworkElement Container { get; set; } = container;
        public Border Ghost { get; } = ghost;
        public TranslateTransform Position { get; } = position;
        public Vector GrabOffset { get; } = grabOffset;
    }

    private (FrameworkElement Container, FrameworkElement Face)? FindCardFace(CardViewModel card)
    {
        foreach (var items in new[] { DeckTopItems, DeckBottomItems })
            if (items.ItemContainerGenerator.ContainerFromItem(card) is FrameworkElement container &&
                items.ItemTemplate.FindName("OrderCardFace", container) is FrameworkElement face)
                return (container, face);
        return null;
    }

    private void BeginDragPreview(CardViewModel card, Point pointer)
    {
        ClearDragPreview();
        if (!MotionEnabled || FindCardFace(card) is not { } source) return;
        var position = new TranslateTransform();
        var ghost = new Border
        {
            Width = source.Face.ActualWidth,
            Height = source.Face.ActualHeight,
            Background = new VisualBrush(source.Face),
            Opacity = .96,
            RenderTransform = position
        };
        var grabOffset = _pressedAt - source.Face.TranslatePoint(new Point(), DragOverlay);
        _dragPreview = new DragPreview(source.Container, ghost, position, grabOffset);
        source.Container.Opacity = .32;
        DragOverlay.Children.Add(ghost);
        UpdateDragPreview(pointer);
    }

    private void UpdateDragPreview(Point pointer)
    {
        if (_dragPreview is not { } preview) return;
        var origin = pointer - preview.GrabOffset;
        preview.Position.X = origin.X;
        preview.Position.Y = origin.Y;
    }

    private void SettleDragPreview(CardViewModel card)
    {
        if (_dragPreview is not { } preview) return;
        if (!MotionEnabled || FindCardFace(card) is not { } destination)
        {
            ClearDragPreview();
            return;
        }

        var start = new Point(preview.Position.X, preview.Position.Y);
        var end = destination.Face.TranslatePoint(new Point(), DragOverlay);
        if ((start - end).Length < .5)
        {
            ClearDragPreview();
            return;
        }

        preview.Container.ClearValue(OpacityProperty);
        preview.Container = destination.Container;
        destination.Container.Opacity = 0;
        preview.Ghost.Background = new VisualBrush(destination.Face);
        Canvas.SetLeft(preview.Ghost, end.X);
        Canvas.SetTop(preview.Ghost, end.Y);
        SlideFrom(preview.Ghost, start - end, (_, _) =>
        {
            if (ReferenceEquals(_dragPreview, preview)) ClearDragPreview();
        });
    }

    private void ClearDragPreview()
    {
        if (_dragPreview is not { } preview) return;
        _dragPreview = null;
        DragOverlay.Children.Remove(preview.Ghost);
        preview.Ghost.Background = null;
        preview.Container.ClearValue(OpacityProperty);
    }

    private IEnumerable<(CardViewModel Card, FrameworkElement Container)> CardContainers()
    {
        foreach (var items in new[] { DeckTopItems, DeckBottomItems })
            foreach (var card in items.Items.OfType<CardViewModel>())
                if (items.ItemContainerGenerator.ContainerFromItem(card) is FrameworkElement container)
                    yield return (card, container);
    }

    private Dictionary<CardViewModel, Point> CaptureCardPositions(CardViewModel dragged) =>
        CardContainers().Where(item => !ReferenceEquals(item.Card, dragged)).ToDictionary(
            item => item.Card, item => item.Container.TranslatePoint(new Point(), DragOverlay));

    private void AnimateCardReflow(Dictionary<CardViewModel, Point> previous)
    {
        UpdateLayout();
        foreach (var (card, container) in CardContainers())
        {
            // Centered rows shift when their counts change. Start at each card's last painted position.
            container.ClearValue(RenderTransformProperty);
            if (!previous.TryGetValue(card, out var start)) continue;
            var displacement = start - container.TranslatePoint(new Point(), DragOverlay);
            if (displacement.Length >= .5) SlideFrom(container, displacement);
        }
    }

    private static void SlideFrom(FrameworkElement element, Vector offset, EventHandler? completed = null)
    {
        var transform = new TranslateTransform();
        element.RenderTransform = transform;
        var horizontal = AxisAnimation(offset.X);
        if (completed is not null) horizontal.Completed += completed;
        transform.BeginAnimation(TranslateTransform.XProperty, horizontal);
        transform.BeginAnimation(TranslateTransform.YProperty, AxisAnimation(offset.Y));

        static DoubleAnimation AxisAnimation(double start) => new(start, 0, SlideDuration)
        {
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut },
            FillBehavior = FillBehavior.Stop
        };
    }
}
