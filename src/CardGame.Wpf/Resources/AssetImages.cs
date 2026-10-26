using System.IO;
using System.Windows.Media.Imaging;

namespace CardGame.Wpf.Resources;

/// <summary>Shared, on-demand image decoding for local game artwork; missing art retains the text fallback.</summary>
public static class AssetImages
{
    private static readonly Dictionary<(string Path, int Width), WeakReference<BitmapImage>> Images = [];

    public static BitmapImage? Load(string localPath, int decodeWidth = 0)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(decodeWidth);
        var key = (AssetFiles.GetPath(localPath), decodeWidth);
        lock (Images)
        {
            if (Images.TryGetValue(key, out var weak) && weak.TryGetTarget(out var cached)) return cached;
            try
            {
                using var stream = File.OpenRead(key.Item1);
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = stream;
                if (decodeWidth > 0) image.DecodePixelWidth = decodeWidth;
                image.EndInit();
                image.Freeze();
                Images[key] = new(image);
                return image;
            }
            catch (Exception error) when (error is IOException or FileFormatException or UnauthorizedAccessException or NotSupportedException)
            {
                // Failed loads are not cached: a subsequently restored asset can be loaded normally.
                return null;
            }
        }
    }
}
