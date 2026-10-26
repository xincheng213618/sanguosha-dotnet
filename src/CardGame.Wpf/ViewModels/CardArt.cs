using System.Windows.Media;
using CardGame.Core;
using CardGame.Wpf.Resources;

namespace CardGame.Wpf.ViewModels;

/// <summary>Presentation assets keyed by CardKind; rank, suit and rules remain independent.</summary>
public static class CardArt
{
    public static bool NeedsRuntimeNameOverlay(CardKind? kind) =>
        kind is CardKind.GeneralWeapon or CardKind.ScarletBloodSword or CardKind.Xuanjian or CardKind.ColdSlash or
            CardKind.AnqiFeiren or CardKind.YaodaoChitong or CardKind.ShanheXingtianAxe or CardKind.Leizhen or CardKind.MantianGuohai;

    public static ImageSource? Get(CardKind? kind)
    {
        if (kind is not { } key || !Enum.IsDefined(key)) return null;
        return Load($"{(key is CardKind.Xuanjian or CardKind.AnqiFeiren or CardKind.YaodaoChitong or CardKind.ShanheXingtianAxe or CardKind.Leizhen ? CardKind.GeneralWeapon : key == CardKind.ColdSlash ? CardKind.Slash : key)}.png");
    }

    public static ImageSource? GetRank(string rank, string suit) =>
        rank is "A" or "2" or "3" or "4" or "5" or "6" or "7" or "8" or "9" or "10" or "J" or "Q" or "K"
            ? Load($"rank-{(suit is "♥" or "♦" ? "red" : "black")}_{rank}.png") : null;

    public static ImageSource? GetSuit(string suit) => suit switch
    {
        "♥" => Load("suit-heart.png"), "♦" => Load("suit-diamond.png"),
        "♠" => Load("suit-spade.png"), "♣" => Load("suit-club.png"), _ => null
    };

    public static ImageSource? GetEquipment(CardKind kind) =>
        EquipmentCatalog.IsEquipment(kind) ? Load($"equip-{(kind is CardKind.Xuanjian or CardKind.ShanheXingtianAxe or CardKind.Leizhen ? CardKind.GeneralWeapon : kind)}.png") : null;

    private static ImageSource? Load(string key) => AssetImages.Load("Assets/Cards/" + key);
}
