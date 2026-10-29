using System.Text.Json;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CardGame.Wpf.Resources;

namespace CardGame.Wpf.ViewModels;

internal static class ShanheMapArt
{
    internal const double Scale = 1000d / 1220;
    private static ImageSource? _lockedBossEmblem;
    internal static ImageSource? BossEmblem(bool ready)
    {
        var artwork = AssetImages.Load("Assets/Shanhe/Map/rogue_boss_icon.png");
        return ready ? artwork : _lockedBossEmblem ??= Grayscale(artwork);
    }

    internal static ImageSource? Grayscale(ImageSource? artwork)
    {
        if (artwork is not BitmapSource source) return null;
        var converted = new FormatConvertedBitmap(source, PixelFormats.Bgra32, null, 0);
        var stride = source.PixelWidth * 4;
        var pixels = new byte[stride * source.PixelHeight];
        converted.CopyPixels(pixels, stride, 0);
        for (var index = 0; index < pixels.Length; index += 4)
        {
            var gray = (byte)((pixels[index + 2] * 77 + pixels[index + 1] * 150 + pixels[index] * 29 + 128) >> 8);
            pixels[index] = pixels[index + 1] = pixels[index + 2] = gray;
        }
        var bitmap = BitmapSource.Create(source.PixelWidth, source.PixelHeight, source.DpiX, source.DpiY, PixelFormats.Bgra32, null, pixels, stride);
        bitmap.Freeze();
        return bitmap;
    }
    private static readonly IReadOnlyList<ShanheNativeMapRegion> Regions = ReadRegions();
    internal static ShanheNativeMapRegion? Region(string? route) => Regions.FirstOrDefault(region => route == "source-region-" + region.RegionId);
    internal static ShanheNativeMapLandmark? Landmark(string? route, string key)
    {
        if (Region(route) is not { } region) return null;
        if (key == "arena") return region.Arena;
        var index = key.StartsWith("native-city-", StringComparison.Ordinal) && int.TryParse(key.AsSpan(12), out var ordinal) ? ordinal
        : region.RegionId == 3 ? key switch
        { "city" => 1, "upper-cave" => 2, "platform" => 3, "forest" => 4, "bridge" => 5, "lake" => 6, "lower-cave" => 7, "boss" => 8, _ => 0 }
        : key switch
        { "puban" => 1, "hebei" => 2, "huxian" => 3, "cave" => 4, "hongnong" => 5, "caoyang" => 6, "shanjin" => 7, "tongguan" => 8, _ => 0 };
        return region.Cities.FirstOrDefault(city => city.CityId == 5000 + region.RegionId * 10 + index);
    }
    private static IReadOnlyList<ShanheNativeMapRegion> ReadRegions()
    {
        using var stream = typeof(ShanheMapArt).Assembly.GetManifestResourceStream("CardGame.ShanheMapLayouts.json")!;
        return JsonSerializer.Deserialize<Source>(stream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!.Regions.AsReadOnly();
    }
    private sealed class Source { public List<ShanheNativeMapRegion> Regions { get; init; } = []; }
}

public sealed class ShanheNativeMapRegion
{
    public int RegionId { get; init; }
    public string Background { get; init; } = string.Empty;
    public IReadOnlyList<ShanheNativeMapLandmark> Cities { get; init; } = [];
    public ShanheNativeMapLandmark Arena { get; init; } = new();
    public ImageSource? Artwork => AssetImages.Load("Assets/Shanhe/Map/" + Background);
}

public sealed class ShanheNativeMapLandmark
{
    public int CityId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Image { get; init; } = string.Empty;
    public double X { get; init; }
    public double Y { get; init; }
    public double Width { get; init; }
    public double Height { get; init; }
    public double MapX => X * ShanheMapArt.Scale;
    public double MapY => Y * ShanheMapArt.Scale;
    public double MapWidth => Width * ShanheMapArt.Scale;
    public double MapHeight => Height * ShanheMapArt.Scale;
    public ImageSource? Artwork => AssetImages.Load("Assets/Shanhe/Map/" + Image);
    private ImageSource? _dormantArtwork;
    public ImageSource? DormantArtwork => _dormantArtwork ??= ShanheMapArt.Grayscale(Artwork);
}
