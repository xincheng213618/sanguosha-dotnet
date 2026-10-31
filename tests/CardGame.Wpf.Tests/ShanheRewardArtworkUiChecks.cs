using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Wpf.Controls;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.ViewModels;

internal static partial class ShanheRewardUiChecks
{

    private static void AssertSharedRewardChoiceFrame(ShanheRewardChoiceCard card, int rarity)
    {
        var expected = WushuangArt.Get($"rogue_award_quality_bg_{rarity}.png");
        Program.Assert(card.Rarity == rarity && ReferenceEquals(card.FrameArtwork, expected) &&
                card.FrameArtwork is BitmapSource { PixelWidth: 500, PixelHeight: 768 } &&
                Program.Find<Image>(card).Any(image => image.Name == "FrameArtworkImage" &&
                    ReferenceEquals(image.Source, expected) && image.Visibility == Visibility.Visible && image.ActualWidth > 0 && image.ActualHeight > 0),
            "Rest and reward cards render the same official 500-by-768 quality-frame family at their actual rarity.");
        if (!string.IsNullOrWhiteSpace(card.Footer))
        {
            var footer = (TextBlock)card.FindName("ChoiceFooterText");
            var frame = Program.Find<Image>(card).Single(image => image.Name == "FrameArtworkImage");
            var footerBounds = footer.TransformToAncestor(card).TransformBounds(new Rect(footer.RenderSize));
            var frameBounds = frame.TransformToAncestor(card).TransformBounds(new Rect(frame.RenderSize));
            Program.Assert(footer.Text == card.Footer && footerBounds.Width > 0 && footerBounds.Height > 0 &&
                    footerBounds.Left >= frameBounds.Left - .7 && footerBounds.Top >= frameBounds.Top - .7 &&
                    footerBounds.Right <= frameBounds.Right + .7 && footerBounds.Bottom <= frameBounds.Bottom + .7,
                "The actual camp growth value and reward footer remain entirely inside their unchanged source frame.");
        }
    }
}
