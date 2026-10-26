using System.Windows.Media;
using CardGame.Wpf.Resources;

namespace CardGame.Wpf.ViewModels;

/// <summary>Optional local artwork, keyed by the public catalogue's numeric reward identity.</summary>
public static class ShanheArt
{
    public static ImageSource? Get(int sourceRewardId) =>
        sourceRewardId > 0 ? AssetImages.Load($"Assets/Shanhe/{sourceRewardId}.png") : null;
}
