using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Wpf.Controls;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.ViewModels;

internal static partial class ShanheRewardUiChecks
{
    public static void BossCardAndRareSkillDeclinesKeepTacticGroupMandatory(string output)
    {
        var store = new MemoryShanheCampaignSaveStore();
        using var vm = new MainViewModel(false, 270018, true, new MemorySaveStore(),
            contentRegistry: FixtureRegistry.Value, shanheSaveStore: store,
            shanheCodexStore: new MemoryShanheCodexStore());
        var panel = new ShanheCampaignPanel { DataContext = vm };
        try
        {
            var definition = new ShanheCampaignDefinition("fixture:shanhe-group-decline-ui", "首领奖励取舍", [Player], ["boss"],
            [
                new("boss", "渡口首领", ShanheNodeKind.BossBattle, ["home"], [new("guard", "守军", [new(Enemy)], VictoryGold: 500)])
                {
                    ChapterNumber = 1, RewardSkipGold = 100,
                    BossRewards = new(500, 1,
                    [new("tactics", "史诗战法", ["tactic"], 1), new("cards", "史诗手牌", ["card"], 1), new("skills", "稀有技能", ["skill"], 1)])
                },
                new("home", "营地", ShanheNodeKind.Camp, [], Choices: [new("rest", "休整")])
            ],
            [
                new("tactic", "刀阵", Bonuses: new(Slash: 1)) { Rarity = 3 },
                new("card", "随身杀", StartingCards: [new(CardKind.Slash)]) { Rarity = 3 },
                new("skill", "传习·振奋", SkillId: Learned) { Rarity = 2 }
            ], StartingGold: 10);
            var session = ShanheCampaignSession.Create(definition, new(71, Player), _ => FixtureRegistry.Value);
            Accept(session, new MoveShanheNodeCommand("boss", session.Revision));
            WinPendingBattle(session);
            Attach(vm, session); Layout(panel);
            var confirm = (Button)panel.FindName("ShanheConfirmRewardButton");
            var finish = (Button)panel.FindName("ShanheConfirmBossRewardButton");
            AssertOverviewObjective(3);
            Open("cards");
            var skip = vm.ShanheRewardCards.Single(card => card.IsSkip);
            var staleSkipButton = Bound(panel, vm.SelectShanheRewardCardCommand, skip);
            Program.Assert(skip.Category.Contains("史诗随身牌") && skip.BonusText.Contains("100") &&
                vm.ShanheRewardNotice.Contains("放弃") && vm.ShanheRewardNotice.Contains("100"),
                "The boss hand-card decline visibly identifies the real category, rarity and configured copper amount.");
            ReadOnlyUi(() => Click(staleSkipButton, panel),
                "Staging the legitimate hand-card compensation leaves both games, the campaign command journal and the saved expedition unchanged.");
            Program.Assert(confirm.IsEnabled && confirm.Content.ToString()!.Contains("铜"),
                "Selecting the group decline enables its explicit copper confirmation.");
            Click(confirm, panel);
            Program.Assert(vm.IsShanheBossRewardOpen && !finish.IsEnabled && session.Snapshot.Gold == 610 &&
                session.Snapshot.StartingCards.Count == 0 && vm.ShanheBossRewardGroups.Single(group => group.Id == "cards").IsResolved &&
                vm.ShanheBossRewardGroups.Single(group => group.Id == "cards").SelectedName.Contains("100") &&
                vm.ShanheBossRewardGroups.Where(group => group.Id != "cards").All(group => !group.IsResolved),
                "Confirming card compensation returns to the summary, marks only that group and leaves the next route closed.");
            AssertOverviewObjective(2);
            AssertSkippedOverview("cards", 3);
            var afterCard = Snapshot(session);
            RawClick(staleSkipButton); vm.SelectShanheRewardCardCommand.Execute(skip); vm.ConfirmShanheRewardCommand.Execute(null);
            Layout(panel);
            Program.Assert(Snapshot(session) == afterCard, "A retired group decline control cannot receive the payment twice.");
            Open("tactics");
            Program.Assert(vm.ShanheRewardCards.Count == 1 && vm.ShanheRewardCards.All(card => !card.IsSkip) &&
                !vm.ShanheRewardNotice.Contains("放弃"), "Boss tactics have an actual mandatory choice without a compensation card or notice.");
            var actualTactic = vm.ShanheRewardCards.Single();
            ReadOnlyUi(() => Click(Bound(panel, vm.SelectShanheRewardCardCommand, actualTactic), panel),
                "Staging the mandatory tactic does not claim it or change the pending campaign and saved journals.");
            Click(confirm, panel);
            AssertOverviewObjective(1);
            AssertClaimedTacticOverview(actualTactic);
            Open("skills");
            skip = vm.ShanheRewardCards.Single(card => card.IsSkip);
            Program.Assert(skip.Category.Contains("稀有额外技能") && skip.BonusText.Contains("100"),
                "The rare-skill decline visibly uses its specified hundred-coin price.");
            Program.Assert(Program.Find<TextBlock>((ItemsControl)panel.FindName("ShanheRewardCardRows"))
                .Any(text => text.Text == "稀有 · 额外技能"),
                "The rendered reward card must label its actual rare quality beside the skill category.");
            ReadOnlyUi(() => Click(Bound(panel, vm.SelectShanheRewardCardCommand, skip), panel),
                "Staging the legitimate rare-skill compensation preserves the same unresolved reward until confirmation.");
            Program.Render(panel, 1440, 880, Path.Combine(output, "shanhe-boss-rare-skill-decline.png"));
            Click(confirm, panel);
            Program.Assert(finish.IsEnabled && session.Snapshot.Gold == 710 && session.Snapshot.ExtraSkillIds.Count == 0 &&
                session.Snapshot.Bonuses.Slash == 1 && session.Snapshot.CompletedNodeIds.Count == 0,
                "A rare-skill decline pays once, keeps the mandatory tactic and waits for the final boss summary confirmation.");
            AssertOverviewObjective(0);
            AssertSkippedOverview("cards", 3);
            AssertSkippedOverview("skills", 2);
            AssertClaimedTacticOverview(actualTactic);
            Program.Render(panel, 1440, 880, Path.Combine(output, "shanhe-boss-compensation-shared-frames.png"));
            Click(finish, panel);
            Program.Assert(session.Snapshot.Phase == ShanheCampaignPhase.Map && session.Snapshot.AvailableNodes.Single().Id == "home" &&
                session.AcceptedCommands.OfType<SkipShanheRewardCommand>().Select(command => command.GroupId).SequenceEqual(["cards", "skills"]),
                "The real controls journal the two exact group declines and open the route only after final confirmation.");

            void Open(string id)
            {
                var actual = session.Snapshot.BossRewardSummary!.Groups.Single(group => group.Id == id);
                ReadOnlyUi(() => Click(Bound(panel, vm.OpenShanheBossRewardGroupCommand,
                    vm.ShanheBossRewardGroups.Single(group => group.Id == id)), panel),
                    "Opening the actual boss reward group only changes local presentation and leaves both games and saved journals intact: " + id);
                var objective = ((TextBlock)panel.FindName("ShanheChapterObjective")).Text;
                Program.Assert(objective == vm.ShanheChapterObjectiveText && objective == vm.ShanheRewardNotice &&
                    (actual.CanSkip ? objective.Contains("放弃", StringComparison.Ordinal) &&
                        objective.Contains(actual.SkipGold.ToString(), StringComparison.Ordinal)
                        : !objective.Contains("放弃", StringComparison.Ordinal) && objective.Contains("必须领取", StringComparison.Ordinal)),
                    "The bound chapter objective refreshes immediately on the actual group opening and advertises only its authoritative decline permission and compensation: " + id);
            }
            void AssertSkippedOverview(string id, int rarity)
            {
                var actual = session.Snapshot.BossRewardSummary!.Groups.Single(group => group.Id == id);
                var group = vm.ShanheBossRewardGroups.Single(group => group.Id == id);
                var button = Bound(panel, vm.OpenShanheBossRewardGroupCommand, group);
                var shared = Program.Find<ShanheRewardChoiceCard>(button).Single();
                var compensation = Program.Find<Image>(shared).Single(image => image.Name == "ShanheBossCompensationArtwork");
                var gallery = Program.Find<ItemsControl>(shared).Single(items => ReferenceEquals(items.ItemsSource, group.Artworks));
                AssertSharedRewardChoiceFrame(shared, rarity);
                Program.Assert(actual.IsSkipped && group.IsSkipped && group.IsResolved && !button.IsEnabled &&
                        !vm.OpenShanheBossRewardGroupCommand.CanExecute(group) && group.Artworks.Count == 0 && gallery.Items.Count == 0 &&
                        group.Rarity == rarity && group.SkipGold == actual.SkipGold && group.OverviewTitle == "已折换" &&
                        group.OverviewDescription == group.DisplaySelectedName && group.OverviewDescription.Contains(actual.SkipGold.ToString(), StringComparison.Ordinal) &&
                        group.OverviewFooter == "已处理" && shared.Title == group.OverviewTitle &&
                        shared.Description == group.OverviewDescription && shared.Footer == group.OverviewFooter &&
                        Program.Find<TextBlock>(shared).Any(text => text.Text == group.OverviewTitle && text.ActualWidth > 0 && text.ActualHeight > 0) &&
                        ((TextBlock)shared.FindName("ChoiceDescriptionText")).Text == group.DisplaySelectedName &&
                        compensation.Visibility == Visibility.Visible && compensation.ActualWidth > 0 && compensation.ActualHeight > 0 &&
                        ReferenceEquals(compensation.Source, vm.ShanheBossGoldArtwork) &&
                        compensation.Source is BitmapSource { PixelWidth: 100, PixelHeight: 100 },
                    "A processed decline group retains its real quality skin and compensation amount, displays the exact original copper icon and cannot reopen; its original reward artwork collection stays empty: " + id);
                var frame = (Image)shared.FindName("FrameArtworkImage");
                foreach (var originalArtwork in new[] { frame, compensation })
                {
                    var reachesButton = false;
                    for (DependencyObject? owner = originalArtwork; owner is not null; owner = VisualTreeHelper.GetParent(owner))
                    {
                        if (owner is UIElement element)
                            Program.Assert(Math.Abs(element.Opacity - 1) < .0001 && element.OpacityMask is null,
                                "Disabling the processed group must preserve its original frame and compensation illustration brightness: " + id);
                        if (ReferenceEquals(owner, button)) { reachesButton = true; break; }
                    }
                    Program.Assert(reachesButton, "The processed source artwork remains rendered inside its original disabled group button: " + id);
                }
            }
            void AssertClaimedTacticOverview(ShanheRewardCard reward)
            {
                var group = vm.ShanheBossRewardGroups.Single(group => group.Id == "tactics");
                var actual = session.Snapshot.BossRewardSummary!.Groups.Single(group => group.Id == "tactics");
                var button = Bound(panel, vm.OpenShanheBossRewardGroupCommand, group);
                var shared = Program.Find<ShanheRewardChoiceCard>(button).Single();
                var gallery = Program.Find<ItemsControl>(shared).Single(items => ReferenceEquals(items.ItemsSource, group.Artworks));
                var art = group.Artworks.Single();
                var illustration = Program.Find<Image>(gallery).Single(image => image.Name == "ShanheBossRewardArtwork");
                AssertSharedRewardChoiceFrame(shared, reward.Rarity);
                Program.Assert(actual.SelectedReward?.Id == reward.Id && !actual.IsSkipped && group.IsResolved && !group.IsSkipped &&
                        !button.IsEnabled && group.Artworks.Count == 1 && gallery.Items.Count == 1 &&
                        ReferenceEquals(gallery.Items[0], art) && art.Name == reward.Name && ReferenceEquals(art.Image, reward.Artwork) &&
                        ReferenceEquals(illustration.Source, reward.Artwork) &&
                        illustration.Visibility == (art.HasImage ? Visibility.Visible : Visibility.Collapsed) &&
                        (art.HasImage || Program.Find<TextBlock>(gallery).Any(text => text.Text == art.Glyph && text.ActualWidth > 0 && text.ActualHeight > 0)) &&
                        shared.Title == "已领取" && shared.Description == group.DisplaySelectedName && shared.Footer == "已处理" &&
                        Program.Find<Image>(shared).Single(image => image.Name == "ShanheBossCompensationArtwork").Visibility == Visibility.Collapsed &&
                        !Program.Find<Image>(shared).Any(image => ReferenceEquals(image.Source, vm.ShanheBossGoldArtwork) && image.Visibility == Visibility.Visible),
                    "The mandatory claimed tactic retains its one actual selected reward artwork or original named fallback, and never substitutes a compensation icon.");
            }
            void AssertOverviewObjective(int unresolved)
            {
                var summary = session.Snapshot.BossRewardSummary!;
                var objective = ((TextBlock)panel.FindName("ShanheChapterObjective")).Text;
                Program.Assert(summary.Groups.Count(group => !group.IsResolved) == unresolved &&
                    objective == vm.ShanheChapterObjectiveText && objective == vm.ShanheBossRewardNotice &&
                    objective.Contains("进入下一章", StringComparison.Ordinal) &&
                    (unresolved > 0 ? objective.Contains(unresolved.ToString(), StringComparison.Ordinal) &&
                        objective.Contains("待处理", StringComparison.Ordinal)
                        : objective.Contains("已处理", StringComparison.Ordinal) && !objective.Contains("待处理", StringComparison.Ordinal)),
                    "The bound overview objective follows the authoritative remaining boss groups and asks for the actual chapter continuation after all have been processed: " + unresolved);
            }
            void ReadOnlyUi(Action action, string message)
            {
                var journal = ShanheCampaignJson.Serialize(session.CreateCheckpoint());
                var saved = store.Exists ? JsonSerializer.Serialize(store.Read()) : null;
                var ordinary = Program.Engine(vm).Revision;
                action();
                Layout(panel);
                Program.Assert(ShanheCampaignJson.Serialize(session.CreateCheckpoint()) == journal &&
                    (store.Exists ? JsonSerializer.Serialize(store.Read()) : null) == saved && Program.Engine(vm).Revision == ordinary, message);
            }
        }
        finally { panel.DataContext = null; }
    }
}
