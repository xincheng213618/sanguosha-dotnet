using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using CardGame.Content.Standard;
using CardGame.Core.Campaigns;
using CardGame.Wpf.Controls;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.ViewModels;

internal static class ShanheInheritanceHitTargetUiChecks
{
    public static void RealWindowInheritanceCardsAndConfirmHaveCompleteHitTargets(string output)
    {
        var store = new MemoryShanheCampaignSaveStore();
        var codex = new MemoryShanheCodexStore();
        codex.Write(new() { UnlockedRewardIds = [StandardShanheCampaignCatalog.OpeningInheritanceRewardId],
            EquippedTacticIds = [StandardShanheCampaignCatalog.OpeningInheritanceRewardId, null, null, null, null, null] });
        using var vm = new MainViewModel(false, 270019, true, new MemorySaveStore(), useExpandedContent: true,
            shanheSaveStore: store, shanheCodexStore: codex)
            { IsMotionEnabled = false, IsSoundEnabled = false, IsMusicEnabled = false };
        using var host = new NativeResponseStatusChecks.NativeHost(vm);
        var root = host.Root;
        var window = host.Window;
        var panel = (ShanheCampaignPanel)window.FindName("ShanhePanel");
        vm.OpenShanheCommand.Execute(null);
        vm.StartShanheCommand.Execute(null);
        var before = Journal();
        Program.Assert(vm.IsShanheOpen && vm.IsShanheRewardOpen && vm.ShanheRewardNotice.Contains("传承") &&
            vm.ShanheRewardCards.Count == 3 && vm.ShanheRewardCards.All(card => !card.IsSkip) &&
            store.Read().Campaign.Commands.Count == 0 && vm.ShanheExtraSkillCards.Count == 0 &&
            vm.ShanheMapNodes.All(node => !node.IsAvailable),
            "The real equipped inheritance must open three candidates in the main window before any map location or skill is granted.");
        var offerIds = vm.ShanheRewardCards.Select(card => card.Id).ToArray();
        foreach (var (width, height) in new[] { (1120, 740), (1950, 1248) })
        {
            Capture(width, height, "choice");
            foreach (var card in vm.ShanheRewardCards) AssertCardHitTargets(Bound(card));
            var first = vm.ShanheRewardCards[0];
            var selected = vm.ShanheRewardCards[1];
            ClickAt(Bound(first), new Point(.08, .33), "card blank area");
            Program.Assert(first.IsSelected && vm.ShanheRewardCards.Count(card => card.IsSelected) == 1,
                "A click in the card's blank frame area must select that actual candidate.");
            ClickAt(Bound(selected), new Point(.5, .29), "card illustration area");
            Program.Assert(selected.IsSelected && !first.IsSelected && ReferenceEquals(vm.SelectedShanheRewardCard, selected) &&
                vm.ShanheRewardCards.Count(card => card.IsSelected) == 1 && Journal() == before && vm.ShanheExtraSkillCards.Count == 0,
                "Selecting another card must clear the old selection without granting either skill or writing a campaign command.");

            // The normal open command refreshes the live projection; no save is loaded.
            vm.OpenShanheCommand.Execute(null);
            Capture(width, height, "refreshed");
            selected = vm.ShanheRewardCards.Single(card => card.Id == offerIds[1]);
            Program.Assert(vm.ShanheRewardCards.Select(card => card.Id).SequenceEqual(offerIds) && selected.IsSelected &&
                ReferenceEquals(vm.SelectedShanheRewardCard, selected) && vm.ShanheRewardCards.Count(card => card.IsSelected) == 1 &&
                Program.Find<ShanheRewardChoiceCard>(panel).Count(card => card.IsVisible && card.IsSelected) == 1 && Journal() == before,
                "A live refresh must retain the same offers and visibly retain exactly the selected inheritance card.");
            AssertCardHitTargets(Bound(selected));
            var confirm = (Button)panel.FindName("ShanheConfirmRewardButton");
            foreach (var point in new[] { new Point(.5, .5), new Point(.03, .5), new Point(.97, .5), new Point(.5, .03), new Point(.5, .97) })
                AssertHit(confirm, confirm, Local(confirm, point), "confirm center or edge");
        }

        var chosen = vm.ShanheRewardCards.Single(card => card.IsSelected);
        var retiredButton = Bound(chosen);
        var confirmation = (Button)panel.FindName("ShanheConfirmRewardButton");
        ClickAt(confirmation, new Point(.97, .5), "confirm edge");
        var saved = store.Read().Campaign;
        var expectedSkill = StandardShanheCampaignCatalog.CreateCodexDefinition(1,
            StandardShanheCampaignCatalog.Returning2026EditionId).Rewards.Single(reward => reward.Id == chosen.Id).SkillId;
        Program.Assert(!vm.IsShanheRewardOpen && vm.ShanheExtraSkillCards.Single().Id == expectedSkill &&
            saved.Commands.Count == 1 && saved.Commands[0] is ChooseShanheOpeningSkillCommand command && command.RewardId == chosen.Id &&
            vm.ShanheMapRows.SelectMany(row => row.Nodes).Count(node => node.IsAvailable) == 2,
            "The actual confirmation button must submit one opening-skill command, grant only the chosen skill and open both first map locations.");
        var committed = Journal();
        RawClick(confirmation); RawClick(retiredButton); RawClick(confirmation);
        vm.SelectShanheRewardCardCommand.Execute(chosen);
        vm.ConfirmShanheRewardCommand.Execute(null);
        Program.Assert(Journal() == committed && store.Read().Campaign.Commands.Count == 1 &&
            vm.ShanheExtraSkillCards.Single().Id == expectedSkill,
            "Repeated confirmation and stale card clicks must never claim a second inheritance skill.");

        string Journal() => ShanheCampaignJson.Serialize(store.Read().Campaign);
        Button Bound(ShanheRewardCard card) => Program.Find<Button>(panel).Single(button =>
            ReferenceEquals(button.Command, vm.SelectShanheRewardCardCommand) && ReferenceEquals(button.CommandParameter, card));
        static Point Local(FrameworkElement element, Point fraction) => new(element.ActualWidth * fraction.X, element.ActualHeight * fraction.Y);
        void AssertCardHitTargets(Button button)
        {
            var face = Program.Find<ShanheRewardChoiceCard>(button).Single();
            var artwork = (Image)face.FindName("ChoiceArtwork");
            var illustrationArea = (FrameworkElement)artwork.Parent;
            var description = Program.Find<ScrollViewer>(face).Single();
            AssertHit(button, illustrationArea, Local(illustrationArea, new Point(.5, .5)), "card illustration or fallback");
            AssertHit(button, button, Local(button, new Point(.08, .33)), "card blank area");
            AssertHit(button, description, Local(description, new Point(.5, .5)), "card description area");
        }
        void AssertHit(Button expected, FrameworkElement target, Point point, string label)
        {
            var hit = root.InputHitTest(target.TranslatePoint(point, root)) as DependencyObject;
            Program.Assert(expected.IsVisible && expected.IsEnabled && target.ActualWidth > 0 && target.ActualHeight > 0 &&
                expected.Command?.CanExecute(expected.CommandParameter) == true && hit is not null &&
                (ReferenceEquals(hit, expected) || expected.IsAncestorOf(hit)),
                $"The full main-window {label} must hit its own reward button at {root.RenderSize}; " +
                $"visible={expected.IsVisible}, enabled={expected.IsEnabled}, target={target.RenderSize}, hit={hit?.GetType().Name}.");
        }
        void ClickAt(Button button, Point fraction, string label)
        {
            AssertHit(button, button, Local(button, fraction), label);
            RawClick(button); root.UpdateLayout();
        }
        static void RawClick(Button button) => typeof(ButtonBase).GetMethod("OnClick", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(button, null);
        void Capture(int width, int height, string label)
        {
            root.Width = width; root.Height = height; window.SizeToContent = SizeToContent.WidthAndHeight; window.UpdateLayout();
            Program.Render(root, width, height, Path.Combine(output, $"shanhe-inheritance-hit-{label}-{width}.png"));
            Program.Assert(root.RenderSize == new Size(width, height) && panel.IsVisible && panel.IsDescendantOf(root),
                "The inheritance hit test must use the real main-window panel at the requested size.");
        }
    }
}
