using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CardGame.Wpf.ViewModels;

namespace CardGame.Wpf.Controls;

/// <summary>Preserves native map sprite padding without making its transparent pixels clickable.</summary>
public sealed class ShanheMapNodeButton : Button
{
    private static readonly ConditionalWeakTable<BitmapSource, AlphaMask> Masks = new();
    private static readonly string[] MarkerNames = ["BattleMarker", "EventMarker", "CampMarker", "BossMarker"];
    private static readonly string[] LabelNames = ["NativeArenaNameplate", "RouteFlag"];

    protected override HitTestResult? HitTestCore(PointHitTestParameters hitTestParameters)
    {
        if (DataContext is not ShanheJourneyNode { NativeLandmark: { } landmark } node)
            return base.HitTestCore(hitTestParameters);

        var point = hitTestParameters.HitPoint;
        if (node.NativeArtwork is BitmapSource artwork &&
            HitsBitmap(artwork, point, new Rect(0, 0, landmark.MapWidth, landmark.MapHeight)))
            return new PointHitTestResult(this, point);

        foreach (var name in MarkerNames)
            if (Template?.FindName(name, this) is DependencyObject marker && HitsMarker(marker, point))
                return new PointHitTestResult(this, point);

        foreach (var name in LabelNames)
        {
            var label = Template?.FindName(name, this) as FrameworkElement;
            if (label is null && Template?.FindName("NodeEmblem", this) is DependencyObject emblem)
                label = FindNamedElement(emblem, name);
            if (label is { Visibility: Visibility.Visible, Opacity: > 0 } &&
                TryRelativePoint(label, point, out var labelPoint) &&
                new Rect(label.RenderSize).Contains(labelPoint))
                return new PointHitTestResult(this, point);
        }

        return null;
    }

    private bool HitsMarker(DependencyObject visual, Point point)
    {
        // Marker containers are deliberately not input targets. Their rendered PNGs
        // still belong to this button, including portions above its layout rectangle.
        if (visual is UIElement { Visibility: not Visibility.Visible } or UIElement { Opacity: <= 0 })
            return false;
        if (visual is Image { Source: BitmapSource bitmap } image &&
            TryRelativePoint(image, point, out var localPoint) &&
            (image.Clip is null || image.Clip.FillContains(localPoint)))
        {
            var bounds = VisualTreeHelper.GetContentBounds(image);
            if (bounds.IsEmpty) bounds = new Rect(image.RenderSize);
            if (HitsBitmap(bitmap, localPoint, bounds)) return true;
        }

        for (var index = 0; index < VisualTreeHelper.GetChildrenCount(visual); index++)
            if (HitsMarker(VisualTreeHelper.GetChild(visual, index), point)) return true;
        return false;
    }

    private bool TryRelativePoint(Visual visual, Point point, out Point relative)
    {
        var inverse = visual.TransformToAncestor(this).Inverse;
        relative = default;
        return inverse is not null && inverse.TryTransform(point, out relative);
    }

    private static FrameworkElement? FindNamedElement(DependencyObject visual, string name)
    {
        if (visual is FrameworkElement { Visibility: not Visibility.Visible }) return null;
        if (visual is FrameworkElement element && element.Name == name) return element;
        for (var index = 0; index < VisualTreeHelper.GetChildrenCount(visual); index++)
            if (FindNamedElement(VisualTreeHelper.GetChild(visual, index), name) is { } found) return found;
        return null;
    }

    private static bool HitsBitmap(BitmapSource source, Point point, Rect bounds)
    {
        if (bounds.IsEmpty || bounds.Width <= 0 || bounds.Height <= 0 ||
            point.X < bounds.Left || point.X >= bounds.Right || point.Y < bounds.Top || point.Y >= bounds.Bottom)
            return false;
        var mask = Masks.GetValue(source, static bitmap => new AlphaMask(bitmap));
        var x = (int)((point.X - bounds.Left) * mask.Width / bounds.Width);
        var y = (int)((point.Y - bounds.Top) * mask.Height / bounds.Height);
        return mask.IsOpaque(x, y);
    }

    private sealed class AlphaMask
    {
        private readonly byte[] _alpha;
        public int Width { get; }
        public int Height { get; }

        public AlphaMask(BitmapSource source)
        {
            Width = source.PixelWidth;
            Height = source.PixelHeight;
            BitmapSource pixels = source;
            if (source.Format != PixelFormats.Bgra32)
            {
                // Freeze only a private conversion. Never freeze or modify an
                // unfrozen source passed by the caller.
                var privateSource = source.IsFrozen ? source : source.CloneCurrentValue();
                var converted = new FormatConvertedBitmap(privateSource, PixelFormats.Bgra32, null, 0);
                converted.Freeze();
                pixels = converted;
            }
            var stride = checked(Width * 4);
            var bgra = new byte[checked(stride * Height)];
            pixels.CopyPixels(bgra, stride, 0);
            _alpha = new byte[checked(Width * Height)];
            for (var index = 0; index < _alpha.Length; index++) _alpha[index] = bgra[index * 4 + 3];
        }

        public bool IsOpaque(int x, int y) => x >= 0 && x < Width && y >= 0 && y < Height && _alpha[y * Width + x] > 16;
    }
}
