using System.Collections;
using System.IO;
using System.Resources;
using System.Text.Json;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CardGame.Wpf.Controls;
using CardGame.Wpf.Resources;
using CardGame.Wpf.ViewModels;

internal static class LocalAssetChecks
{
    public static void DeploymentDecodingAndRecovery(string output)
    {
        var assembly = typeof(GeneralArt).Assembly;
        using (var stream = assembly.GetManifestResourceStream("CardGame.Wpf.g.resources")!)
        using (var resources = new ResourceReader(stream))
        {
            var names = resources.Cast<DictionaryEntry>().Select(entry => (string)entry.Key).ToArray();
            Program.Assert(names.Any(name => name.EndsWith("mainwindow.baml", StringComparison.Ordinal)) &&
                !names.Any(name => name.StartsWith("assets/", StringComparison.OrdinalIgnoreCase)),
                "The application must contain its compiled UI while binary artwork stays outside the assembly.");
        }
        using (var stream = assembly.GetManifestResourceStream("CardGame.GeneralArtCatalog.json")!)
        using (var catalog = JsonDocument.Parse(stream))
        {
            var paths = catalog.RootElement.GetProperty("entries").EnumerateArray()
                .SelectMany(entry => entry.GetProperty("skins").EnumerateArray())
                .Select(skin => skin.GetProperty("localPath").GetString()!).Distinct().ToArray();
            Program.Assert(paths.Length > 0 && paths.All(path => File.Exists(AssetFiles.GetPath(path))),
                "All catalogued portraits and skins must be copied transitively beside the test/application host.");
        }

        var previousDirectory = Environment.CurrentDirectory;
        try
        {
            Environment.CurrentDirectory = output;
            var path = AssetFiles.GetPath("src/CardGame.Wpf/Assets/gallery-hp-wu.png");
            Program.Assert(path == AssetFiles.GetPath("Assets\\gallery-hp-wu.png") &&
                path.StartsWith(AppContext.BaseDirectory, StringComparison.OrdinalIgnoreCase),
                "Catalog and runtime paths must resolve against the installed application, independently of the working directory.");
            var image = AssetImages.Load("Assets/gallery-hp-wu.png")!;
            var thumbnail = AssetImages.Load("Assets/gallery-hp-wu.png", 12)!;
            Program.Assert(image is { IsFrozen: true, PixelWidth: > 0 } && thumbnail is { IsFrozen: true, PixelWidth: 12 } &&
                !ReferenceEquals(image, thumbnail) && ReferenceEquals(image, AssetImages.Load("Assets/gallery-hp-wu.png")),
                "Shared images must be frozen, reused while visible, and decoded separately at the requested thumbnail size.");
            Program.Assert(GeneralArt.GetPortrait("cao-cao") is ImageBrush { ImageSource: BitmapSource { PixelWidth: > 0 } } &&
                AssetImages.Load("Assets/official-mou-lu-zhi-mobile.jpg", 128) is { PixelWidth: 128 } &&
                AssetImages.Load("Assets/Table/Board.jpg", 128) is { PixelWidth: 128 } &&
                WushuangArt.Get("roguelike1v1_hufu_icon.png") is BitmapSource { PixelWidth: > 0 } &&
                CardArt.Get(CardGame.Core.CardKind.Slash) is BitmapSource { PixelWidth: > 0 },
                "Default portraits, JPEGs, table art, mode art and card faces must all decode from local files.");

            var lobby = new HomeLobby();
            Program.Render(lobby, 1120, 740, null);
            var backgrounds = Program.Find<Panel>(lobby).Select(panel => panel.Background).OfType<ImageBrush>()
                .Concat(Program.Find<Border>(lobby).Select(border => border.Background).OfType<ImageBrush>()).ToArray();
            Program.Assert(backgrounds.Length >= 4 && backgrounds.All(brush => brush.ImageSource is BitmapSource { PixelWidth: > 0 }),
                "Compiled XAML must resolve its site-of-origin decoration and portraits from the deployed Assets directory.");

            foreach (var invalid in new[] { "../outside.png", "Assets/../outside.png", "Assets/Audio/Official/../../outside.wav",
                "AssetsSibling/face.png", "C:/outside.png", "https://example.com/face.png", "Assets/C:/face.png" })
            {
                // Traversal that stays within Assets is also rejected, preserving narrower catalogue roots.
                try { AssetFiles.GetPath(invalid); }
                catch (InvalidDataException) { continue; }
                throw new InvalidOperationException($"Asset path must be rejected: {invalid}");
            }

            const string fixtureName = "Assets/.local assets 恢复 #.png";
            var fixture = AssetFiles.GetPath(fixtureName);
            Program.Assert(!File.Exists(fixture), "The recovery fixture must not replace an existing asset.");
            try
            {
                Program.Assert(AssetImages.Load(fixtureName) is null,
                    "A missing optional image must retain its text fallback.");
                File.WriteAllText(fixture, "invalid image");
                Program.Assert(AssetImages.Load(fixtureName) is null,
                    "An unreadable optional image must retain its text fallback.");
                File.Copy(path, fixture, true);
                Program.Assert(AssetImages.Load(fixtureName) is { IsFrozen: true, PixelWidth: > 0 },
                    "Restoring an asset must recover without a cached failure.");
                using var exclusive = File.Open(fixture, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            finally { File.Delete(fixture); }
        }
        finally { Environment.CurrentDirectory = previousDirectory; }
        Console.WriteLine($"WPF assembly with external assets: {new FileInfo(assembly.Location).Length:N0} bytes.");
    }
}
