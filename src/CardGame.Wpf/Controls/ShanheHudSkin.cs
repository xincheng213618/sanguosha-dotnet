using System.Windows;
using System.Windows.Media;

namespace CardGame.Wpf.Controls;

/// <summary>Stretch source HUD panels while keeping the original corner artwork.</summary>
public sealed class ShanheHudSkin : FrameworkElement
{
    public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(nameof(Source), typeof(ImageSource),
        typeof(ShanheHudSkin), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, Rebuild));
    public static readonly DependencyProperty InsetsProperty = DependencyProperty.Register(nameof(Insets), typeof(Thickness),
        typeof(ShanheHudSkin), new FrameworkPropertyMetadata(new Thickness(4), FrameworkPropertyMetadataOptions.AffectsRender, Rebuild));
    public static readonly DependencyProperty SourceScaleProperty = DependencyProperty.Register(nameof(SourceScale), typeof(double),
        typeof(ShanheHudSkin), new FrameworkPropertyMetadata(2d, FrameworkPropertyMetadataOptions.AffectsRender, Rebuild));
    private readonly ImageBrush?[] _pieces = new ImageBrush?[9];
    public ImageSource? Source { get => (ImageSource?)GetValue(SourceProperty); set => SetValue(SourceProperty, value); }
    public Thickness Insets { get => (Thickness)GetValue(InsetsProperty); set => SetValue(InsetsProperty, value); }
    public double SourceScale { get => (double)GetValue(SourceScaleProperty); set => SetValue(SourceScaleProperty, value); }
    private static void Rebuild(DependencyObject owner, DependencyPropertyChangedEventArgs args) => ((ShanheHudSkin)owner).Rebuild();
    private void Rebuild()
    {
        Array.Clear(_pieces);
        if (Source is not { Width: > 0, Height: > 0 } source) return;
        var x = Edges(source.Width, Insets.Left * SourceScale, Insets.Right * SourceScale);
        var y = Edges(source.Height, Insets.Top * SourceScale, Insets.Bottom * SourceScale);
        for (var row = 0; row < 3; row++)
            for (var column = 0; column < 3; column++)
            {
                if (x[column + 1] <= x[column] || y[row + 1] <= y[row]) continue;
                var brush = new ImageBrush(source) { ViewboxUnits = BrushMappingMode.Absolute,
                    Viewbox = new Rect(x[column], y[row], x[column + 1] - x[column], y[row + 1] - y[row]), Stretch = Stretch.Fill };
                if (brush.CanFreeze) brush.Freeze();
                _pieces[row * 3 + column] = brush;
            }
    }
    protected override void OnRender(DrawingContext drawingContext)
    {
        var x = Edges(ActualWidth, Insets.Left, Insets.Right);
        var y = Edges(ActualHeight, Insets.Top, Insets.Bottom);
        for (var row = 0; row < 3; row++)
            for (var column = 0; column < 3; column++)
                if (_pieces[row * 3 + column] is { } brush && x[column + 1] > x[column] && y[row + 1] > y[row])
                    drawingContext.DrawRectangle(brush, null, new Rect(x[column], y[row], x[column + 1] - x[column], y[row + 1] - y[row]));
    }
    private static double[] Edges(double length, double leading, double trailing)
    {
        var scale = leading + trailing > length ? length / (leading + trailing) : 1;
        return [0, leading * scale, length - trailing * scale, length];
    }
}
