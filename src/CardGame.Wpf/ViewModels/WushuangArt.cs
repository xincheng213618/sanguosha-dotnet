using System.Windows.Media;
using CardGame.Wpf.Resources;

namespace CardGame.Wpf.ViewModels;

public static class WushuangArt
{
    public static ImageSource? Get(string file) => AssetImages.Load("Assets/Wushuang/" + file);
}
