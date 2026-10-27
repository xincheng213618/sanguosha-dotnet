using System.IO;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using CardGame.Core;
using CardGame.Wpf.Resources;
using CardGame.Wpf.ViewModels;

internal static class ShanheSanshouArtworkChecks
{
    public static void PublicCatalogueFaceLoadsAndUsesItsPrintedTitle(string output)
    {
        CheckOriginalFace(CardKind.ShanheSanshou, "三首", "B82D9CCF44E19A7B160C1A00E150AEC283CC88FD71ABC82AAECC54BF762C674B",
            Path.Combine(output, "shanhe-sanshou-original-card-face.png"));
        CheckOriginalFace(CardKind.WushuangHalberd, "无双方天戟", "0ECBB32FC66C2C3BE8C682A2DFBA2D09F2CC46E58D275253BA5C399971E32642",
            Path.Combine(output, "wushuang-halberd-original-card-face.png"));
        CheckOriginalFace(CardKind.BlackLightArmor, "黑光铠", "F5EE9D6839716F0F59144628FF94ED2AA05D250CD205E80D35E0D061D202D414",
            Path.Combine(output, "black-light-armor-original-card-face.png"));
        CheckOriginalFace(CardKind.GouXiang, "钩镶", "828B194F65658419932CE796C700793CCDE2F7A3B998E79F4552C989124CC032",
            Path.Combine(output, "gouxiang-original-card-face.png"));
        CheckOriginalFace(CardKind.Duanhun, "断魂", "B7847C951231F17D1CEEED009CD9D5A1FC3E1F855315E9256E1E2A6F7CC723C7",
            Path.Combine(output, "duanhun-original-card-face.png"));
        CheckOriginalFace(CardKind.KunlunMirror, "昆仑镜", "DDBEC2890A883D37318273E7C96EEA7276DBA343AC02317B7DEFF36ED2A58C16",
            Path.Combine(output, "kunlun-mirror-original-card-face.png"));
        Program.Assert(CardArt.GetEquipment(CardKind.ShanheSanshou) is null &&
            CardArt.GetEquipment(CardKind.WushuangHalberd) is BitmapSource &&
            ReferenceEquals(CardArt.GetEquipment(CardKind.WushuangHalberd), AssetImages.Load("Assets/Cards/equip-WushuangHalberd.png")) &&
            EquipmentCatalog.Get(CardKind.WushuangHalberd).WeaponAttackRange == 4,
            "Sanshou retains its equipment text fallback; Wushuang Halberd retains the existing separate equipment strip and its actual printed range4 identity.");
        Program.Assert(CardArt.GetEquipment(CardKind.BlackLightArmor) is null && CardArt.GetEquipment(CardKind.GouXiang) is null &&
            EquipmentCatalog.Get(CardKind.BlackLightArmor).Slot == EquipmentSlot.Armor && EquipmentCatalog.Get(CardKind.GouXiang).Slot == EquipmentSlot.Armor,
            "Both original public armour faces keep their real armor identities and the existing equipment text fallback without a synthesized strip.");
        Program.Assert(CardArt.GetEquipment(CardKind.Duanhun) is null && CardArt.GetEquipment(CardKind.KunlunMirror) is null &&
            EquipmentCatalog.Get(CardKind.Duanhun) is { Slot: EquipmentSlot.Weapon, WeaponAttackRange: 2, IgnoresArmor: false } &&
            EquipmentCatalog.Get(CardKind.KunlunMirror).Slot == EquipmentSlot.Treasure,
            "The original new faces retain Duanhun's range2 weapon and Kunlun Mirror's Treasure slot, with text fallback for unknown equipment strips.");
    }

    private static void CheckOriginalFace(CardKind kind, string name, string hash, string output)
    {
        var asset = "Assets/Cards/" + kind + ".png";
        var face = AssetImages.Load(asset);
        var info = CardCatalog.Get(kind);
        Program.Assert(face is BitmapSource { PixelWidth: 186, PixelHeight: 260, IsFrozen: true } &&
            Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(AssetFiles.GetPath(asset)))) == hash &&
            ReferenceEquals(CardArt.Get(kind), face) && info.DisplayName == name && !CardArt.NeedsRuntimeNameOverlay(kind),
            "The unchanged public catalogue full face, including its original printed name, loads through the normal CardKind artwork lookup.");
        Program.Assert(ReferenceEquals(new GuideCardEntry(info).Artwork, face), "The card information uses the same original full face.");

        var model = new CardViewModel
        {
            Id = 9901, Kind = kind, Name = info.DisplayName, KindLabel = info.CategoryName,
            SuitGlyph = "♥", Rank = "Q", Description = info.Description, IsPlayable = true
        };
        var control = new ContentControl
        {
            Content = model, ContentTemplate = (DataTemplate)Application.Current.FindResource("CardFaceTemplate"),
            HorizontalContentAlignment = HorizontalAlignment.Stretch, VerticalContentAlignment = VerticalAlignment.Stretch
        };
        Program.Render(control, 186, 260, output);
        Program.Assert(ReferenceEquals(model.Artwork, face) && !model.HasDynamicWeaponName &&
            Program.Find<Image>(control).Any(image => image.Name == "CardArtwork" && ReferenceEquals(image.Source, face) &&
                image.ActualWidth > 0 && image.ActualHeight > 0) &&
            Program.Find<TextBlock>(control).Single(text => text.Name == "RuntimeCardName").Visibility == Visibility.Collapsed &&
            Program.Find<Grid>(control).Single(grid => grid.Name == "CardTextFallback").Visibility == Visibility.Collapsed &&
            Program.Find<TextBlock>(control).Single(text => text.Name == "CardRank").Text == "Q" &&
            Program.Find<TextBlock>(control).Single(text => text.Name == "CardSuit").Text == "♥",
            "The production face template renders the original title once while runtime rank and suit remain independent bindings.");
        control.Content = null;
    }
}
