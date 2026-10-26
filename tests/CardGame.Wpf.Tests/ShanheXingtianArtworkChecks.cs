using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using CardGame.Core;
using CardGame.Wpf;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.Resources;
using CardGame.Wpf.ViewModels;

internal static class ShanheXingtianArtworkChecks
{
    public static void ExistingAxeAssetsDecodeAndRenderInEquipmentSlot(string output)
    {
        var face = AssetImages.Load("Assets/Cards/XingtianAxe.png");
        var icon = AssetImages.Load("Assets/Cards/equip-XingtianAxe.png");
        Program.Assert(face is BitmapSource { PixelWidth: 186, PixelHeight: 260, IsFrozen: true } &&
            icon is BitmapSource { PixelWidth: 284, PixelHeight: 50, IsFrozen: true } &&
            ReferenceEquals(CardArt.Get(CardKind.ShanheXingtianAxe), face) &&
            ReferenceEquals(CardArt.GetEquipment(CardKind.ShanheXingtianAxe), icon) &&
            !ReferenceEquals(CardArt.Get(CardKind.GeneralWeapon), face) &&
            !ReferenceEquals(CardArt.GetEquipment(CardKind.GeneralWeapon), icon),
            "The independent S5 axe decodes the existing official axe face and equipment strip instead of the generic sword template.");
        Program.Assert(ReferenceEquals(new GuideCardEntry(CardCatalog.Get(CardKind.ShanheXingtianAxe)).Artwork, face) &&
            !CardArt.NeedsRuntimeNameOverlay(CardKind.ShanheXingtianAxe),
            "The standard card preview uses the same complete axe face without painting a duplicate title over its printed name.");

        using var model = new MainViewModel(false, 17, false, new MemorySaveStore())
            { IsMotionEnabled = false, IsSoundEnabled = false, IsMusicEnabled = false };
        Program.StartLordFixture(model);
        model.SelectGeneralChoiceCommand.Execute(model.GeneralChoices[0]);
        Program.AdvanceToDecision(model);
        var snapshot = Program.Engine(model).CreateSnapshot(0);
        var axe = new CardSnapshot(9901, CardKind.ShanheXingtianAxe, Suit.Spade, 5,
            CardCatalog.Get(CardKind.ShanheXingtianAxe).DisplayName, "5");
        snapshot = snapshot with { Players = snapshot.Players.Select(player => player.IsHuman
            ? player with { Equipment = [axe], Hand = [axe with { Id = 9902 }], HandCount = 1 } : player).ToArray() };
        var window = new MainWindow(model);
        var root = (FrameworkElement)window.Content;
        try
        {
            typeof(MainViewModel).GetMethod("Refresh", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(model, [snapshot]);
            Program.Render(root, 1440, 880, Path.Combine(output, "shanhe-xingtian-equipment-artwork.png"));
            var slot = model.HumanEquipmentSlots.Single(item => item.Card?.Id == axe.Id);
            var hand = model.Hand.Single(card => card.Id == 9902);
            var equipment = (ItemsControl)window.FindName("HumanEquipmentSlots");
            Program.Assert(ReferenceEquals(slot.Artwork, icon) && !slot.NeedsNameOverlay &&
                slot.Tooltip.Contains(EquipmentCatalog.Get(CardKind.ShanheXingtianAxe).Description) &&
                Program.Find<Image>(equipment).Any(image => ReferenceEquals(image.DataContext, slot) &&
                    ReferenceEquals(image.Source, icon) && image.ActualWidth > 0 && image.ActualHeight > 0) &&
                ReferenceEquals(hand.Artwork, face) && !hand.HasDynamicWeaponName &&
                Program.Find<Image>(root).Any(image => ReferenceEquals(image.DataContext, hand) && ReferenceEquals(image.Source, face)),
                "The actual equipment strip and hand-card templates bind the axe images, while the tooltip keeps the S5 rule description.");
        }
        finally { window.Content = null; window.DataContext = null; window.Close(); }
    }
}
