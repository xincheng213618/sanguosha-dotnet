using System.Windows;
using System.Windows.Controls;

namespace CardGame.Wpf.Controls;

/// <summary>Original HUD ordering: three rows, first column next to the hero.</summary>
public sealed class ShanheColumnSlotsPanel : Panel
{
    public static readonly DependencyProperty CellWidthProperty = DependencyProperty.Register(nameof(CellWidth), typeof(double),
        typeof(ShanheColumnSlotsPanel), new FrameworkPropertyMetadata(32d, FrameworkPropertyMetadataOptions.AffectsMeasure));
    public static readonly DependencyProperty CellHeightProperty = DependencyProperty.Register(nameof(CellHeight), typeof(double),
        typeof(ShanheColumnSlotsPanel), new FrameworkPropertyMetadata(32d, FrameworkPropertyMetadataOptions.AffectsMeasure));
    public double CellWidth { get => (double)GetValue(CellWidthProperty); set => SetValue(CellWidthProperty, value); }
    public double CellHeight { get => (double)GetValue(CellHeightProperty); set => SetValue(CellHeightProperty, value); }
    protected override Size MeasureOverride(Size availableSize)
    {
        foreach (UIElement child in InternalChildren) child.Measure(new(CellWidth, CellHeight));
        return new(Math.Ceiling(InternalChildren.Count / 3d) * CellWidth, Math.Min(3, InternalChildren.Count) * CellHeight);
    }
    protected override Size ArrangeOverride(Size finalSize)
    {
        var columns = (InternalChildren.Count + 2) / 3;
        var originX = Math.Max(0, finalSize.Width - columns * CellWidth);
        for (var index = 0; index < InternalChildren.Count; index++)
            InternalChildren[index].Arrange(new(originX + (columns - 1 - index / 3) * CellWidth, index % 3 * CellHeight, CellWidth, CellHeight));
        return finalSize;
    }
}
