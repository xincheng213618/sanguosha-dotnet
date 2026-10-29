using System.Reflection;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Wpf;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.Controls;
using CardGame.Wpf.ViewModels;

internal static class ShanheSourceDifficultyUiChecks
{
    private const string SourcePlayerPortraitPath = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_664.png";

    public static void SourceSelectionStartsActualBattleAndRestoresTwentyFifthTier(string output)
    {
        DefaultSourceSixStepMarketPurchaseReachesNextSetup(output);
        var progress = new MemoryShanheCampaignSaveStore();
        var account = new MemoryShanheCodexStore();
        account.Write(new());
        using var vm = Create();
        vm.SelectedShanheScenario = vm.ShanheScenarios.Single(option => option.Id == StandardShanheCampaignCatalog.Source2026EditionId);
        Program.Assert(vm.ShanheDifficulties.Select(option => option.Value).SequenceEqual(Enumerable.Range(1, 25)) &&
            vm.ShanheDifficulties[24].Name.Contains("炼狱5", StringComparison.Ordinal),
            "The source-reproduction scenario exposes the genuine twenty-five distinct source identities.");
        vm.SelectedShanheDifficulty = vm.ShanheDifficulties[24];
        vm.ShanheGeneralSearch = "SP刘备·煮酒";
        vm.ToggleShanheGeneralRosterCommand.Execute(null);
        vm.SelectedShanheGeneral = vm.ShanheGenerals.Single(general => general.Id == StandardOlSpLiuBei664Package.GeneralId);
        AssertSourcePlayerPortrait(vm.SelectedShanhePortrait, "The selected official SP664 roster entry resolves its own original portrait.");
        vm.SelectedShanheScenario = vm.ShanheScenarios.Single(option => option.Id == StandardShanheCampaignCatalog.LocalEditionId);
        Program.Assert(vm.ShanheDifficulties.Count == 3 && vm.SelectedShanheDifficulty.Value == 1,
            "Returning to the old local scenario safely replaces a source-only selection with a valid local difficulty.");
        vm.SelectedShanheScenario = vm.ShanheScenarios.Single(option => option.Id == StandardShanheCampaignCatalog.Source2026EditionId);
        vm.SelectedShanheDifficulty = vm.ShanheDifficulties[24];
        var panel = new ShanheCampaignPanel { DataContext = vm };
        panel.Measure(new Size(1440, 880)); panel.Arrange(new Rect(0, 0, 1440, 880)); panel.UpdateLayout();
        panel.Dispatcher.Invoke(() => { }, DispatcherPriority.DataBind);
        var selector = (ComboBox)panel.FindName("ShanheDifficultySelector");
        Program.Assert(selector.Items.Count == 25 && selector.SelectedItem is ShanheDifficultyOption { Value: 25 } &&
            selector.ToolTip is string description && description.Contains("炼狱5", StringComparison.Ordinal),
            "The real bound selector shows tier twenty-five and its difficulty explanation.");
        vm.StartShanheCommand.Execute(null);
        var session = Session(vm);
        Program.Assert(session.Definition.SourceDifficultyId == 25 && session.CreateCheckpoint().Options.SourceDifficultyId == 25 &&
            session.CreateCheckpoint().Options.PreferSourceOrdinaryEncounters &&
            session.CreateCheckpoint().Options.UseSourceSceneCityBindings &&
            session.CreateCheckpoint().Options.PlayerGeneralId == StandardOlSpLiuBei664Package.GeneralId &&
            session.Snapshot.PlayerGeneralId == StandardOlSpLiuBei664Package.GeneralId &&
            vm.ShanheGuideTaskChapters.SelectMany(chapter => chapter.Tasks).Where(task => !task.RequiresScore)
                .All(task => !task.Availability.StartsWith("待开放", StringComparison.Ordinal)),
            "The actual start command binds source difficulty, opts the new expedition into source ordinary encounters and opens the four non-score tasks.");
        panel.UpdateLayout();
        Program.Render(panel, 1440, 880, Path.Combine(output, "shanhe-source-25-map.png"));
        AssertSourcePlayerMapPortrait(vm, panel);
        var offeredButtons = Program.Find<Button>(panel).Where(button => button.DataContext is ShanheJourneyNode { IsAvailable: true }).ToArray();
        var bounds = offeredButtons.Select(button => button.TransformToAncestor(panel).TransformBounds(new Rect(new Size(button.ActualWidth, button.ActualHeight)))).ToArray();
        Program.Assert(offeredButtons.Select(button => ((ShanheJourneyNode)button.DataContext).Node.Id).ToHashSet()
                .SetEquals(session.Snapshot.AvailableNodes.Select(node => node.Id)) && vm.ShanheSourceRegions.Count == 1 &&
            bounds.SelectMany((rectangle, index) => bounds.Skip(index + 1).Select(other => rectangle.IntersectsWith(other))).All(overlap => !overlap),
            "Every available route destination has a separate clickable rectangle; future chapters and unchosen regions do not crowd the map.");
        var naturalRegionSelector = (ComboBox)panel.FindName("ShanheSourceRegionSelector");
        var naturalGold = (FrameworkElement)panel.FindName("ShanheMapGoldCounter");
        var naturalViewport = (ScrollViewer)panel.FindName("ShanheMapScroll");
        var naturalMap = (ShanheJourneyMap)panel.FindName("ShanheMapRows");
        var naturalViewportBounds = naturalViewport.TransformToAncestor(panel).TransformBounds(new Rect(naturalViewport.RenderSize));
        var naturalGoldBounds = naturalGold.TransformToAncestor(panel).TransformBounds(new Rect(naturalGold.RenderSize));
        Program.Assert(vm.HasShanheNativeMap && !vm.HasShanheSourceRegionChoices &&
                naturalRegionSelector.Visibility == Visibility.Collapsed && naturalGold.Visibility == Visibility.Visible &&
                naturalGold.ActualWidth > 0 && naturalGold.ActualHeight > 0 && naturalViewportBounds.Contains(naturalGoldBounds) &&
                !naturalGold.IsHitTestVisible && !naturalMap.IsAncestorOf(naturalGold) && !naturalMap.IsAncestorOf(naturalRegionSelector) &&
                ((TextBlock)panel.FindName("ShanheMapGoldValue")).Text == vm.ShanheGoldText,
            "A natural single-region source opening keeps its real copper balance visible in the unscaled viewport toolbar while hiding the unnecessary region selector.");
        Program.Assert(session.Snapshot.AvailableNodes.Count == 2 &&
                session.Snapshot.AvailableNodes.All(node => node.Kind == ShanheNodeKind.Battle && node.ChapterRole == ShanheChapterNodeRole.OpeningBattle) &&
                vm.ShanheMapNodes.Count(node => node.IsBattle && node.IsAvailable) == 2 &&
                vm.ShanheMapNodes.All(node => node.Kind != ShanheNodeKind.Shop) &&
                session.ChapterProgress == 0 && vm.ShanheChapterMainProgress == 0 && vm.ShanheChapterStepCount == 6 &&
                vm.ShanheChapterProgressTarget == 6 && vm.ShanheChapterProgressText.Contains("0 / 6", StringComparison.Ordinal) &&
                !session.IsChapterMarketUnlocked && !vm.OpenShanheMarketCommand.CanExecute(null),
            "The persistent market stays outside the itinerary and locked; a fresh chapter offers exactly its two opening battles while keeping later mainline and optional destinations locked.");
        var first = vm.ShanheMapRows.SelectMany(row => row.Nodes).First(node => node.IsAvailable);
        var preview = session.PreviewEncounter(first.Id);
        Program.Assert(first.Id == "source-city-5041" && preview.SourceBattleId is > 0 && preview.SourceCityId == 5041,
            "The existing seed-71 source25 opening naturally selects a published source recipe at its actual source city, without replacing an enemy or searching seeds.");
        var previewJournal = ShanheCampaignJson.Serialize(session.CreateCheckpoint());
        var previewSave = ShanheCampaignJson.Serialize(progress.Read().Campaign);
        vm.PreviewShanheNodeCommand.Execute(first);
        Program.Assert(vm.ShanheEncounterEnemyCards.Count == preview.Enemies.Count && preview.Enemies.Count > 0 &&
            preview.Enemies.Select((enemy, index) => enemy.InitialArmor <= 0 ||
                vm.ShanheEncounterEnemyCards[index].Stats.Contains($"初始护甲 {enemy.InitialArmor}", StringComparison.Ordinal)).All(matches => matches),
            "The real encounter preview reports the same grown initial armor that will enter battle.");
        Program.Render(panel, 1440, 880, Path.Combine(output, "shanhe-source-25-native-opening-preview.png"));
        AssertNativeSourcePreview(vm, session, panel, first, preview);
        Program.Assert(ShanheCampaignJson.Serialize(session.CreateCheckpoint()) == previewJournal &&
                ShanheCampaignJson.Serialize(progress.Read().Campaign) == previewSave,
            "Reading and painting every natural source enemy's original card removes its placeholder without entering battle, rolling another encounter or writing progress.");
        panel.DataContext = null;
        vm.ChallengeShanheEncounterCommand.Execute(null);
        Program.Assert(vm.IsShanheBattleSession && session.PendingBattleSetup is { } && vm.FlushShanheProgress(),
            $"A real source-tier map command starts and saves the native encounter, including all generated growth. Phase={session.Snapshot.Phase}; Status={vm.ShanheStatusText}");
        Program.Assert(Program.Engine(vm).CreateSnapshot(0).Players[0].GeneralId == StandardOlSpLiuBei664Package.GeneralId &&
            Program.Engine(vm).CreateSnapshot(0).Players[0].Skills!.Any(skill => skill.ContentId == StandardOlSpLiuBei664Package.ZhujiuSkillId),
            "The newly registered SP version is selected through the real roster and enters battle with its executable brewing skill.");
        AssertSourcePlayerBattlePortrait(vm, output, "shanhe-source-25-player-seat.png");
        var originalSetup = Program.Engine(vm).CreateCheckpoint().Options.ShanheBattle!;
        AssertSourceEntryMatchesPreview(session, originalSetup, preview);
        var entry = progress.Read().Campaign;
        var saved = ShanheCampaignJson.Serialize(entry);
        using var cold = Create();
        cold.ContinueShanheCommand.Execute(null);
        Program.Assert(cold.IsShanheBattleRetryReady && !cold.IsShanheBattleSession &&
                Session(cold).Snapshot.PlayerGeneralId == StandardOlSpLiuBei664Package.GeneralId &&
                Session(cold).CreateCheckpoint().Options.PlayerGeneralId == StandardOlSpLiuBei664Package.GeneralId &&
                Session(cold).CreateCheckpoint().Options.PreferSourceOrdinaryEncounters &&
                Session(cold).CreateCheckpoint().Options.UseSourceSceneCityBindings &&
                ShanheCampaignJson.Serialize(progress.Read().Campaign) == saved,
            "Source progress restores the current encounter entry without an in-flight game or a write to the saved source.");
        var restoredPanel = new ShanheCampaignPanel { DataContext = cold };
        try
        {
            Program.Render(restoredPanel, 1440, 880, Path.Combine(output, "shanhe-source-25-restored-player-map.png"));
            AssertSourcePlayerMapPortrait(cold, restoredPanel);
            Program.Assert(cold.IsShanheBattleRetryReady && !cold.IsShanheBattleSession &&
                    ShanheCampaignJson.Serialize(progress.Read().Campaign) == saved,
                "Rendering the restored original player portrait keeps the unfinished encounter and saved retry boundary untouched.");
        }
        finally { restoredPanel.DataContext = null; }
        cold.ResumeShanheBattleCommand.Execute(null);
        Program.Assert(cold.IsShanheBattleSession && cold.SelectedShanheScenario.Id == StandardShanheCampaignCatalog.Source2026EditionId &&
                cold.SelectedShanheDifficulty.Value == 25,
            $"Retry must enter a real battle at source tier twenty-five. Status={cold.ShanheStatusText}");
        var retrySetup = Program.Engine(cold).CreateCheckpoint().Options.ShanheBattle!;
        AssertSourceEntryMatchesPreview(Session(cold), retrySetup, preview);
        var expectedJournal = entry with
        {
            Revision = entry.Revision + 1,
            Commands = entry.Commands.Append(new BeginShanheBattleAttemptCommand(entry.Revision)).ToArray()
        };
        Program.Assert(retrySetup.AttemptNumber == originalSetup.AttemptNumber + 1 && retrySetup.BattleSeed != originalSetup.BattleSeed &&
                ShanheBattleSetup.ValueEquals(retrySetup with { AttemptNumber = originalSetup.AttemptNumber }, originalSetup) &&
                ShanheCampaignJson.Serialize(progress.Read().Campaign) == ShanheCampaignJson.Serialize(expectedJournal),
            "Retry preserves the source roster, growth, entry resources and build, changing only the attempt seed and appending its single start command.");
        var fresh = StandardShanheCampaignCatalog.CreateBattle(retrySetup);
        var started = fresh.Submit(new StartGameCommand());
        Program.Assert(started.Accepted &&
                SnapshotJson.Serialize(Program.Engine(cold).CreateSnapshot(0)) == SnapshotJson.Serialize(fresh.CreateSnapshot(0)),
            "The retried source battle must match an independently constructed native opening for its newly issued attempt.");
        AssertSourcePlayerBattlePortrait(cold, output, "shanhe-source-25-player-seat-retry.png");
        VerifyRegionChoiceOnlyChangesPresentation(output);

        MainViewModel Create() => new(false, 71, true, new MemorySaveStore(), useExpandedContent: true,
            shanheSaveStore: progress, shanheCodexStore: account);
    }

    private static void DefaultSourceSixStepMarketPurchaseReachesNextSetup(string output)
    {
        var account = new MemoryShanheCodexStore();
        account.Write(new());
        var progress = new MemoryShanheCampaignSaveStore();
        using var vm = new MainViewModel(false, 71, true, new MemorySaveStore(), useExpandedContent: true,
            shanheSaveStore: progress, shanheCodexStore: account);
        Program.Assert(vm.SelectedShanheScenario.Id == StandardShanheCampaignCatalog.Source2026EditionId &&
                vm.SelectedShanheDifficulty.Value == 1,
            "A fresh launcher actually defaults to source difficulty one without a scenario or difficulty override.");
        vm.OpenShanheCommand.Execute(null);
        vm.ShanheGeneralSearch = "SP刘备·煮酒";
        vm.ToggleShanheGeneralRosterCommand.Execute(null);
        vm.SelectedShanheGeneral = vm.ShanheGenerals.Single(general => general.Id == StandardOlSpLiuBei664Package.GeneralId);
        Program.Assert(vm.StartShanheCommand.CanExecute(null), "The natural source launcher accepts the existing selectable SP664 player.");
        vm.StartShanheCommand.Execute(null);
        var session = Session(vm);
        var factory = StandardShanheCampaignCatalog.CreateCodexDefinition(1, StandardShanheCampaignCatalog.Source2026EditionId);
        var panel = new ShanheCampaignPanel { DataContext = vm };
        try
        {
            Layout();
            Program.Assert(session.ContentHash == ShanheCampaignContent.GetContentHash(factory) &&
                    session.Definition.SourceDifficultyId == 1 && session.Snapshot.Gold == factory.StartingGold &&
                    session.CreateCheckpoint().Options.PreferSourceOrdinaryEncounters &&
                    session.CreateCheckpoint().Options.UseSourceSceneCityBindings &&
                    session.ChapterProgress == 0 && vm.ShanheChapterMainProgress == 0 && vm.ShanheChapterStepCount == 6 &&
                    vm.ShanheChapterProgressTarget == 6 && vm.ShanheChapterProgressSlots.Count == 6 &&
                    vm.ShanheChapterProgressText.Contains("0 / 6", StringComparison.Ordinal) &&
                    session.Snapshot.AvailableNodes.Count == 2 && !session.IsChapterMarketUnlocked &&
                    !vm.OpenShanheMarketCommand.CanExecute(null),
                "The real default start uses the unchanged production factory and starting wallet, displays0/6 and keeps its market locked.");

            var opening = session.Snapshot.AvailableNodes.First(node => node.ChapterRole == ShanheChapterNodeRole.OpeningBattle);
            var encounter = session.PreviewEncounter(opening.Id);
            var openingMarker = vm.ShanheMapRows.SelectMany(row => row.Nodes).Single(node => node.Id == opening.Id);
            var openingJournal = ShanheCampaignJson.Serialize(session.CreateCheckpoint());
            var openingSave = ShanheCampaignJson.Serialize(progress.Read().Campaign);
            var ordinary = Program.Engine(vm);
            var ordinaryJournal = GameCheckpointJson.Serialize(ordinary.CreateCheckpoint());
            vm.PreviewShanheNodeCommand.Execute(openingMarker);
            Program.Render(panel, 1440, 880, Path.Combine(output, "shanhe-source-default-native-opening-preview.png"));
            AssertNativeSourcePreview(vm, session, panel, openingMarker, encounter);
            Program.Assert(ShanheCampaignJson.Serialize(session.CreateCheckpoint()) == openingJournal &&
                    ShanheCampaignJson.Serialize(progress.Read().Campaign) == openingSave && ReferenceEquals(Program.Engine(vm), ordinary) &&
                    GameCheckpointJson.Serialize(ordinary.CreateCheckpoint()) == ordinaryJournal,
                "The actual default source opening previews its exact native roster and original portraits without changing a journal, save or the covered ordinary game.");
            vm.CloseShanheEncounterPreviewCommand.Execute(null);
            var entered = session.Submit(new MoveShanheNodeCommand(opening.Id, session.Revision));
            Program.Assert(entered.Accepted && session.PendingBattleSetup is { SourceDifficultyId: 1, MarketSetup: null },
                "The natural opening command prepares its real source battle while the third-step market is still unavailable.");
            AssertSourceEntryMatchesPreview(session, session.PendingBattleSetup!, encounter);
            // This is still a journal of real commands. The reward-boundary seam
            // below is intentionally not replayable, so prove cold recovery here
            // on a detached copy without changing the later purchase fixture.
            Program.Assert(vm.FlushShanheProgress(), "The genuine first source1 entry saves before the UI-only victory seam.");
            var sourceEntry = progress.Read();
            var sourceEntryJournal = ShanheCampaignJson.Serialize(sourceEntry.Campaign);
            var sourceEntrySetup = session.PendingBattleSetup!;
            var retryProgress = new MemoryShanheCampaignSaveStore();
            retryProgress.Write(sourceEntry);
            using (var cold = new MainViewModel(false, 71, true, new MemorySaveStore(), useExpandedContent: true,
                shanheSaveStore: retryProgress, shanheCodexStore: account))
            {
                cold.ContinueShanheCommand.Execute(null);
                var restored = Session(cold);
                var saveSame = ShanheCampaignJson.Serialize(retryProgress.Read().Campaign) == sourceEntryJournal;
                Program.Assert(cold.IsShanheBattleRetryReady && !cold.IsShanheBattleSession &&
                        restored.CreateCheckpoint().Options.PreferSourceOrdinaryEncounters &&
                        restored.CreateCheckpoint().Options.UseSourceSceneCityBindings &&
                        restored.Definition.SourceDifficultyId == 1 && saveSame &&
                        ShanheCampaignJson.Serialize(progress.Read().Campaign) == sourceEntryJournal,
                    "A cold source1 restore preserves the opted-in genuine source entry without writing either saved journal: " +
                    JsonSerializer.Serialize(new { cold.IsShanheBattleRetryReady, cold.IsShanheBattleSession, cold.HasShanheSession,
                        cold.HasSaveError, cold.ShanheStatusText, cold.SaveStatus, Phase = restored?.Snapshot.Phase.ToString(),
                        PreferSource = restored?.CreateCheckpoint().Options.PreferSourceOrdinaryEncounters,
                        SourceDifficulty = restored?.Definition.SourceDifficultyId, SaveSame = saveSame }));
                cold.ResumeShanheBattleCommand.Execute(null);
                var retry = Program.Engine(cold).CreateCheckpoint().Options.ShanheBattle!;
                AssertSourceEntryMatchesPreview(restored!, retry, encounter);
                Program.Assert(cold.IsShanheBattleSession && retry.AttemptNumber == sourceEntrySetup.AttemptNumber + 1 &&
                        ShanheBattleSetup.ValueEquals(retry with { AttemptNumber = sourceEntrySetup.AttemptNumber }, sourceEntrySetup) &&
                        ShanheCampaignJson.Serialize(progress.Read().Campaign) == sourceEntryJournal,
                    "The actual source1 cold attempt uses the same previewed source enemy roster and entry build, writes only the detached retry journal, and changes only its deterministic attempt identity.");
            }
            var fields = BindingFlags.Instance | BindingFlags.NonPublic;
            var state = typeof(ShanheCampaignSession).GetField("_state", fields)!.GetValue(session)!;
            var paidGold = (int)typeof(ShanheCampaignSession).GetMethod("EconomyBattleBalance", fields)!
                .Invoke(session, [state, encounter.VictoryGold])!;
            // Reuse the existing UI reward-boundary seam with the production income calculation.
            // This supplies one winning reward stage, not a completed AI match or a replay-verified
            // battle receipt. All reward, event, purchase and subsequent setup commands stay real.
            typeof(ShanheRewardUiChecks).GetMethod("PublishPrimaryRewardStageForUi", BindingFlags.Static | BindingFlags.NonPublic)!
                .Invoke(null, [session, session.Snapshot.CurrentBattleNode!, paidGold]);
            vm.OpenShanheCommand.Execute(null);
            for (var rewardStep = 0; rewardStep < 4 && session.Snapshot.Phase == ShanheCampaignPhase.Reward; rewardStep++)
            {
                var card = vm.ShanheRewardCards.Where(card => card.IsEnabled)
                    .OrderByDescending(card => card.IsSkip).First();
                vm.SelectShanheRewardCardCommand.Execute(card);
                Program.Assert(vm.ConfirmShanheRewardCommand.CanExecute(null), "The actual opening reward has an immediately confirmable published choice.");
                vm.ConfirmShanheRewardCommand.Execute(null);
            }
            Program.Assert(session.Snapshot.Phase == ShanheCampaignPhase.Map && session.ChapterProgress == 1 &&
                    vm.ShanheChapterMainProgress == 1 && !session.IsChapterMarketUnlocked,
                "Claiming the production opening reward completes exactly the first mainline slot without unlocking the market.");

            foreach (var step in new[] { 2, 3 })
            {
                var story = session.Snapshot.AvailableNodes.Single(node => node.ChapterProgressStep == step);
                var marker = vm.ShanheMapRows.SelectMany(row => row.Nodes).Single(node => node.Id == story.Id);
                Program.Assert(vm.SelectShanheNodeCommand.CanExecute(marker), "The next natural story is an actual available map command.");
                vm.SelectShanheNodeCommand.Execute(marker);
                var choices = session.Snapshot.NodeChoices.Where(choice => choice.LeavesNode && choice.BattleNodeId is null &&
                        choice.GoldCost == 0 && choice.HpCost == 0 && choice.Companion is null)
                    .Select(choice => vm.ShanheChoices.SingleOrDefault(item => item.Id == choice.Id && item.IsEnabled))
                    .OfType<ShanheCampaignChoice>().ToArray();
                Program.Assert(choices.Length > 0,
                    $"The fixed natural story must offer an enabled free noncombat continuation, without replacing its event. " +
                    $"story={session.Snapshot.CurrentStoryEventName}; enabled={string.Join(",", vm.ShanheChoices.Where(choice => choice.IsEnabled).Select(choice => choice.Id))}.");
                vm.ChooseShanheOptionCommand.Execute(choices[0]);
                Program.Assert(session.Snapshot.Phase == ShanheCampaignPhase.Map && session.ChapterProgress == step &&
                        vm.ShanheChapterMainProgress == step && session.IsChapterMarketUnlocked == (step == 3) &&
                        vm.OpenShanheMarketCommand.CanExecute(null) == (step == 3),
                    "Each actual story choice advances one slot; only completing the third slot enables persistent trade.");
            }

            vm.OpenShanheMarketCommand.Execute(null);
            var before = session.Snapshot;
            var product = before.ShopItems.Where(item => item.CanPurchase && item.GoldCost > 0 && item.GoldCost <= before.Gold &&
                    item.HpCost == 0 && item.ReplaceableSkillIds.Count == 0 && item.Reward is { } reward && ProjectsIntoSetup(reward))
                .OrderBy(item => item.Reward!.StartingCards is { Count: > 0 } ? 0 : 1).ThenBy(item => item.GoldCost).FirstOrDefault();
            Program.Assert(product is not null,
                $"The unchanged seed71 source wallet must afford an actual setup-bearing supply; gold={before.Gold}, " +
                $"published={string.Join(",", before.ShopItems.Where(item => item.CanPurchase).Select(item => item.Name + ":" + item.GoldCost))}.");
            var purchased = product!;
            var purchasedReward = purchased.Reward!;
            var displayed = vm.ShanheShopItems.Single(item => item.Id == purchased.ChoiceId);
            vm.SelectedShanheMarketCategory = displayed.MarketCategory;
            Program.Assert(displayed.Price == purchased.GoldCost && displayed.CanBuy && vm.RequestShanheShopPurchaseCommand.CanExecute(displayed),
                "The affordable published product and real shop command agree on their current price and eligibility.");
            vm.RequestShanheShopPurchaseCommand.Execute(displayed);
            var after = session.Snapshot;
            var rewardCards = purchasedReward.StartingCards ?? [];
            var rewardEquipment = purchasedReward.StartingEquipment ?? [];
            var bonus = purchasedReward.Bonuses;
            Layout();
            Program.Assert(after.Gold == before.Gold - purchased.GoldCost && session.ChapterProgress == 3 &&
                    vm.ShanheGoldText == $"铜币   {after.Gold}" && vm.ShanheMarketGoldText == $"铜币  {after.Gold}" &&
                    ((TextBlock)panel.FindName("ShanheMapGoldValue")).Text == vm.ShanheGoldText &&
                    vm.ShanheJourneyHandCards.Count == after.StartingCards.Count &&
                    (after.Bonuses != before.Bonuses || !after.StartingCards.SequenceEqual(before.StartingCards) ||
                        !after.StartingEquipment.SequenceEqual(before.StartingEquipment) || !after.ExtraSkillIds.SequenceEqual(before.ExtraSkillIds) ||
                        !after.TacticIds.SequenceEqual(before.TacticIds)),
                "One genuine purchase spends its quoted affordable price once, refreshes wallet/HUD and changes the persistent build without spending a mainline slot.");
            Program.Assert((rewardCards.Count == 0 || after.StartingCards.Skip(before.StartingCards.Count).SequenceEqual(rewardCards)) &&
                    rewardEquipment.All(after.StartingEquipment.Contains) &&
                    (purchasedReward.SkillId is not { } skill || after.ExtraSkillIds.Contains(skill)) &&
                    (purchasedReward.TacticSkillId is null || after.TacticIds.Contains(purchasedReward.Id)) &&
                    (bonus is null || after.Bonuses.MaxHp == before.Bonuses.MaxHp + bonus.MaxHp &&
                        after.Bonuses.InitialHand == before.Bonuses.InitialHand + bonus.InitialHand &&
                        after.Bonuses.Draw == before.Bonuses.Draw + bonus.Draw && after.Bonuses.Slash == before.Bonuses.Slash + bonus.Slash),
                "The paid product grants its own exact cards, equipment, skill or tactic bonuses, rather than merely changing some other build field.");
            Program.Render(panel, 1440, 880, Path.Combine(output, "shanhe-source-default-sixstep-market.png"));
            vm.CloseShanheMarketCommand.Execute(null);
            var next = session.Snapshot.AvailableNodes.First(node => node.ChapterRole == ShanheChapterNodeRole.OpeningBattle);
            var nextMarker = vm.ShanheMapRows.SelectMany(row => row.Nodes).Single(node => node.Id == next.Id);
            var previewJournal = ShanheCampaignJson.Serialize(session.CreateCheckpoint());
            var previewEncounter = session.PreviewEncounter(next.Id);
            var expectedStory = !string.IsNullOrWhiteSpace(previewEncounter.StoryText) ? previewEncounter.StoryText
                : session.PreviewBattleNode(next.Id).StoryText ?? nextMarker.Description;
            vm.PreviewShanheNodeCommand.Execute(nextMarker);
            Program.Render(panel, 1440, 880, Path.Combine(output, "shanhe-source-default-optional-route-preview.png"));
            AssertNativeSourcePreview(vm, session, panel, nextMarker, previewEncounter);
            Program.Assert(vm.ShanheMapNodes.Single(node => node.Node.Id == next.Id).RouteFlagText == "可选" &&
                    vm.ShanheEncounterRouteHint.Contains("可选战斗", StringComparison.Ordinal) &&
                    vm.ShanheEncounterRouteHint.Contains("可跳过", StringComparison.Ordinal) &&
                    vm.ShanheEncounterRouteHint.Contains("不消耗六格主线进度", StringComparison.Ordinal) &&
                    ((TextBlock)panel.FindName("ShanheEncounterRouteHint")).Text == vm.ShanheEncounterRouteHint &&
                    vm.ShanheEncounterDescription == expectedStory &&
                    ((TextBlock)panel.FindName("ShanheEncounterStoryText")).Text == expectedStory &&
                    session.ChapterProgress == 3 && ShanheCampaignJson.Serialize(session.CreateCheckpoint()) == previewJournal &&
                    session.PendingBattleSetup is null,
                "The actual remaining opening battle is now explicitly optional, preserves its original enemy story and displays the skip/no-mainline-cost rule before a real challenge without spending progress or a command.");
            vm.ChallengeShanheEncounterCommand.Execute(null);
            var setup = session.PendingBattleSetup;
            Program.Assert(vm.IsShanheBattleSession && setup is { SourceDifficultyId: 1 } &&
                    setup.MaxHpBonus == after.Bonuses.MaxHp && setup.InitialHandBonus == after.Bonuses.InitialHand &&
                    setup.DrawBonus == after.Bonuses.Draw && setup.SlashBonus == after.Bonuses.Slash &&
                    setup.StartingCards.SequenceEqual(after.StartingCards) && setup.StartingEquipment.SequenceEqual(after.StartingEquipment) &&
                    setup.ExtraSkillIds.SequenceEqual(after.ExtraSkillIds) && setup.MarketSetup?.State.Gold == after.Gold &&
                    (purchasedReward.TacticSkillId is not { } tactic || setup.TacticSkillIds.Contains(tactic)),
                "The next actual native battle receives the paid build, owned cards/equipment/skills and the identical detached market wallet.");
            AssertSourceEntryMatchesPreview(session, setup!, previewEncounter);
        }
        finally { panel.DataContext = null; }

        void Layout()
        {
            panel.Measure(new Size(1440, 880)); panel.Arrange(new Rect(0, 0, 1440, 880)); panel.UpdateLayout();
            panel.Dispatcher.Invoke(() => { }, DispatcherPriority.DataBind); panel.UpdateLayout();
        }
        static bool ProjectsIntoSetup(ShanheRewardDefinition reward) => reward.StartingCards is { Count: > 0 } ||
            reward.StartingEquipment is { Count: > 0 } || reward.SkillId is not null ||
            reward.TacticSkillId is not null && !reward.BossBattleOnly ||
            reward.Bonuses is { MaxHp: not 0 } or { InitialHand: not 0 } or { Draw: not 0 } or { Slash: not 0 };
    }

    private static void AssertNativeSourcePreview(MainViewModel model, ShanheCampaignSession session,
        ShanheCampaignPanel panel, ShanheCampaignNode node, ShanheEncounterDefinition preview)
    {
        var profile = session.PreviewBattleNode(node.Id);
        var authored = (profile.Encounters ?? []).SingleOrDefault(encounter => encounter.Id == preview.Id);
        Program.Assert(session.CreateCheckpoint().Options.PreferSourceOrdinaryEncounters &&
                session.Definition.SourceDifficultyId is not null && profile.Kind == ShanheNodeKind.Battle &&
                preview.SourceBattleId is > 0 && preview.SourceCityId is > 0 && authored is not null &&
                authored.SourceBattleId == preview.SourceBattleId && authored.SourceCityId == preview.SourceCityId &&
                node.Id == StandardShanheCampaignCatalog.Source2026CityNodeId(preview.SourceCityId.Value) &&
                authored.Enemies.Select(enemy => enemy.GeneralId).SequenceEqual(preview.Enemies.Select(enemy => enemy.GeneralId)) &&
                model.ShanheEncounterTitle == preview.Name && model.ShanheEncounterEnemyCards.Count == preview.Enemies.Count,
            "The opted-in ordinary opening previews one unchanged published source recipe at its matching real city, preserving every authored enemy identity and order.");
        var effectiveHealing = session.Definition.SourceDifficultyId is >= 6 ? 0 : preview.HealAfterVictory;
        Program.Assert(effectiveHealing > 0
                ? model.ShanheEncounterRewardText.Contains($"胜利后恢复 {effectiveHealing} 点体力", StringComparison.Ordinal)
                : !model.ShanheEncounterRewardText.Contains("胜利后恢复", StringComparison.Ordinal),
            "The actual source preview describes only the healing permitted by its real difficulty: low-tier recovery stays accurate and source6-or-higher never promises disabled automatic recovery.");
        for (var index = 0; index < preview.Enemies.Count; index++)
        {
            var member = preview.Enemies[index];
            var card = model.ShanheEncounterEnemyCards[index];
            var path = ShanheBossPortraitChecks.GetVerifiedSourcePortraitPath(member.GeneralId);
            Program.Assert(path is not null && card.SeatOrdinal == index + 1 && card.HasPortrait &&
                    (member.Hp is null || card.Hp == member.Hp),
                "Every actual source-preview enemy seat has its verified original native portrait binding rather than a local stand-in.");
            AssertExactSourcePortrait(card.Portrait, member.GeneralId, path!,
                "The actual source-preview model resolves the exact original portrait for this authored body.");
            var faces = Program.Find<Border>(panel).Where(border => ReferenceEquals(border.DataContext, card) &&
                border.Background is ImageBrush && border.Visibility == Visibility.Visible && border.ActualWidth > 0 && border.ActualHeight > 0).ToArray();
            Program.Assert(faces.Length > 0, "Each actual source enemy paints its own original card in the full-size or minor-enemy preview.");
            foreach (var face in faces)
            {
                AssertExactSourcePortrait(face.Background, member.GeneralId, path!,
                    "The actual source enemy's laid-out portrait paints its exact verified original pixels.");
                Program.Assert(Program.Find<TextBlock>((FrameworkElement)face.Parent).Any(text =>
                        text.Text == card.Glyph && text.Visibility == Visibility.Collapsed),
                    "The actual original source portrait hides its corresponding letter fallback instead of painting both.");
            }
        }
        Program.Assert(model.ShanheEncounterMainEnemyCards.Count + model.ShanheEncounterMinorEnemyCards.Count == preview.Enemies.Count &&
                (model.ShanheEncounterEnemyCards.Any(card => card.IsCommander == true)
                    ? model.ShanheEncounterMainEnemyCards.All(card => card.IsCommander == true) &&
                        model.ShanheEncounterMinorEnemyCards.All(card => card.IsCommander == false)
                    : model.ShanheEncounterMainEnemyCards.Count == preview.Enemies.Count && model.ShanheEncounterMinorEnemyCards.Count == 0),
            "The real selected source roster keeps every enemy seat, with a separate front line only when its source projection identifies a commander.");
    }

    private static void AssertSourceEntryMatchesPreview(ShanheCampaignSession session, ShanheBattleSetup setup,
        ShanheEncounterDefinition preview)
    {
        // The battle setup carries the roster, while the campaign owns the frozen
        // encounter ID. Read that existing ID; do not add a replay schema field.
        var state = typeof(ShanheCampaignSession).GetField("_state", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(session)!;
        var encounterId = (string?)state.GetType().GetProperty("EncounterId")!.GetValue(state);
        var actual = session.Snapshot.CurrentBattleNode!.Encounters!.Single(encounter => encounter.Id == encounterId);
        Program.Assert(session.CreateCheckpoint().Options.PreferSourceOrdinaryEncounters &&
                setup.SourceDifficultyId == session.Definition.SourceDifficultyId && setup.NodeId == session.Snapshot.CurrentNodeId &&
                actual.Id == preview.Id && actual.SourceBattleId == preview.SourceBattleId && actual.SourceCityId == preview.SourceCityId &&
                JsonSerializer.Serialize(setup.EnemyRoster) == JsonSerializer.Serialize(preview.Enemies),
            "The actual challenge or cold retry enters the exact source battle/city that was previewed, with its same full grown enemy roster and opted-in campaign policy.");
    }

    private static void AssertSourcePlayerMapPortrait(MainViewModel model, ShanheCampaignPanel panel)
    {
        var session = Session(model);
        var journal = ShanheCampaignJson.Serialize(session.CreateCheckpoint());
        var ordinary = Program.Engine(model);
        var ordinaryJournal = GameCheckpointJson.Serialize(ordinary.CreateCheckpoint());
        Program.Assert(session.Snapshot.PlayerGeneralId == StandardOlSpLiuBei664Package.GeneralId && model.HasShanheJourneyGeneralPortrait,
            "The source map retains the selected official SP664 identity and a real player portrait after starting or restoring the expedition.");
        AssertSourcePlayerPortrait(model.CurrentShanheGeneralPortrait, "The source map model resolves the selected SP664 original card.");
        var loadout = Program.Find<ShanheJourneyLoadout>(panel).Single();
        var hero = (Grid)loadout.FindName("ShanheHeroCard");
        var paintedHero = Program.Find<Border>(hero).Single(border =>
            BindingOperations.GetBindingExpression(border, Border.BackgroundProperty)?.ParentBinding.Path?.Path == nameof(model.CurrentShanheGeneralPortrait));
        AssertSourcePlayerPortrait(paintedHero.Background, "The actual journey HUD paints the same official SP664 original card.");
        var fallback = Program.Find<TextBlock>(hero).Single(text =>
            BindingOperations.GetBindingExpression(text, TextBlock.TextProperty)?.ParentBinding.Path?.Path == nameof(model.ShanheJourneyGeneralGlyph));
        var map = (ShanheJourneyMap)panel.FindName("ShanheMapRows");
        var pin = Program.Find<Grid>(map).Single(grid => grid.Name == "GeneralPin" && grid.Visibility == Visibility.Visible);
        var face = Program.Find<System.Windows.Shapes.Ellipse>(pin).Single();
        AssertSourcePlayerPortrait(face.Fill, "The actual current-location pin paints the selected SP664 original portrait.");
        var viewport = (ScrollViewer)panel.FindName("ShanheMapScroll");
        var viewportBounds = viewport.TransformToAncestor(panel).TransformBounds(new Rect(viewport.RenderSize));
        var faceBounds = face.TransformToAncestor(panel).TransformBounds(new Rect(face.RenderSize));
        var heroBounds = paintedHero.TransformToAncestor(panel).TransformBounds(new Rect(paintedHero.RenderSize));
        Program.Assert(fallback.Visibility == Visibility.Collapsed && Program.Find<TextBlock>(pin).Single(text => text.Text == "将").Visibility == Visibility.Collapsed &&
                paintedHero.ActualWidth > 0 && paintedHero.ActualHeight > 0 && face.ActualWidth > 0 && face.ActualHeight > 0 &&
                new Rect(panel.RenderSize).Contains(heroBounds) && viewportBounds.Contains(faceBounds),
            "The decoded player artwork is visibly laid out in both HUD and map pin, with neither SP-letter nor generic-general fallback covering it.");
        Program.Assert(ShanheCampaignJson.Serialize(session.CreateCheckpoint()) == journal && ReferenceEquals(Program.Engine(model), ordinary) &&
                GameCheckpointJson.Serialize(ordinary.CreateCheckpoint()) == ordinaryJournal,
            "Reading the selected player's map/HUD/pin artwork neither submits a campaign command nor changes the covered game.");
    }

    private static void AssertSourcePlayerBattlePortrait(MainViewModel model, string output, string filename)
    {
        var session = Session(model);
        var game = Program.Engine(model);
        var journal = ShanheCampaignJson.Serialize(session.CreateCheckpoint());
        var battleJournal = GameCheckpointJson.Serialize(game.CreateCheckpoint());
        var human = model.HumanPlayer;
        Program.Assert(model.IsShanheBattleSession && human is { GeneralId: StandardOlSpLiuBei664Package.GeneralId } &&
                human.Portrait.GeneralId == StandardOlSpLiuBei664Package.GeneralId && human.Portrait.ArtworkId == StandardOlSpLiuBei664Package.GeneralId && human.HasPortrait,
            "The genuine source battle's human seat preserves the selected exact SP664 identity rather than substituting its PortraitKey or another Liu Bei.");
        var window = new MainWindow(model); window.ApplyTemplate();
        try
        {
            var root = (FrameworkElement)window.Content;
            Program.Render(root, 1440, 880, Path.Combine(output, filename));
            var portrait = Program.Find<System.Windows.Shapes.Rectangle>(root).Single(rectangle =>
                rectangle.Name == "SinglePortrait" && ReferenceEquals(rectangle.DataContext, human));
            AssertSourcePlayerPortrait(portrait.Fill, "The actual live human SinglePortrait seat paints the verified SP664 original pixels.");
            AssertSourcePlayerPortrait(human!.Portrait.Brush, "The actual source human seat model retains the verified SP664 original pixels.");
            Program.Assert(portrait.Visibility == Visibility.Visible && portrait.ActualWidth > 0 && portrait.ActualHeight > 0 &&
                    Program.Find<TextBlock>(root).Single(text => text.Name == "UnknownGeneral" && ReferenceEquals(text.DataContext, human)).Visibility == Visibility.Collapsed,
                "The real source player seat displays its original card and hides its letter placeholder.");
            var painted = new RenderTargetBitmap((int)Math.Ceiling(portrait.ActualWidth), (int)Math.Ceiling(portrait.ActualHeight), 96, 96, PixelFormats.Pbgra32);
            painted.Render(portrait);
            var pixels = new byte[painted.PixelWidth * painted.PixelHeight * 4];
            painted.CopyPixels(pixels, painted.PixelWidth * 4, 0);
            Program.Assert(Enumerable.Range(0, pixels.Length / 4).Any(index => pixels[index * 4 + 3] > 0),
                "The actual laid-out human seat renders nontransparent original SP664 card pixels.");
            var setup = game.CreateCheckpoint().Options.ShanheBattle!;
            var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup);
            Program.Assert(setup.SourceDifficultyId == 25 && setup.EnemyRoster.Count > 0 &&
                    game.PlayerCount == 1 + setup.AllyRoster.Count + setup.EnemyRoster.Count && model.Seats.Count == game.PlayerCount,
                "The actual opening and retry preserve the selected source25 roster's full native seat count.");
            for (var index = 0; index < setup.EnemyRoster.Count; index++)
            {
                var member = setup.EnemyRoster[index];
                var enemy = model.Seats.Single(seat => seat.Seat == 1 + setup.AllyRoster.Count + index);
                var enemyPortrait = Program.Find<System.Windows.Shapes.Rectangle>(root).Single(rectangle =>
                    rectangle.Name == "SinglePortrait" && ReferenceEquals(rectangle.DataContext, enemy));
                var path = ShanheBossPortraitChecks.GetVerifiedSourcePortraitPath(member.GeneralId);
                Program.Assert(enemy.GeneralId == member.GeneralId && enemy.Portrait.ArtworkId == member.GeneralId &&
                        enemy.HasPortrait && path is not null && enemy.Hp == member.Hp &&
                        enemy.MaxHp == registry.Generals[member.GeneralId].BaseHp + member.MaxHpBonus &&
                        enemy.Armor == member.InitialArmor && enemyPortrait.Visibility == Visibility.Visible &&
                        enemyPortrait.ActualWidth > 0 && enemyPortrait.ActualHeight > 0 &&
                        Program.Find<TextBlock>(root).Single(text => text.Name == "UnknownGeneral" && ReferenceEquals(text.DataContext, enemy)).Visibility == Visibility.Collapsed,
                    "Every actual enemy seat keeps its exact authored source body, grown statistics and original portrait on the opening and retry.");
                AssertExactSourcePortrait(enemyPortrait.Fill, member.GeneralId, path!,
                    "Each actual source enemy seat paints the verified official pixels for that exact body.");
                var enemyPainted = new RenderTargetBitmap((int)Math.Ceiling(enemyPortrait.ActualWidth), (int)Math.Ceiling(enemyPortrait.ActualHeight), 96, 96, PixelFormats.Pbgra32);
                enemyPainted.Render(enemyPortrait);
                var enemyPixels = new byte[enemyPainted.PixelWidth * enemyPainted.PixelHeight * 4];
                enemyPainted.CopyPixels(enemyPixels, enemyPainted.PixelWidth * 4, 0);
                Program.Assert(Enumerable.Range(0, enemyPixels.Length / 4).Any(pixel => enemyPixels[pixel * 4 + 3] > 0),
                    "Each actual source25 enemy rectangle paints nontransparent official pixels on the initial battle and its real retry.");
            }
        }
        finally { window.Content = null; window.DataContext = null; window.Close(); }
        Program.Assert(ReferenceEquals(Program.Engine(model), game) && ShanheCampaignJson.Serialize(session.CreateCheckpoint()) == journal &&
                GameCheckpointJson.Serialize(game.CreateCheckpoint()) == battleJournal,
            "Inspecting and closing the player's actual native seat preserves the complete current source battle and campaign journals.");
    }

    private static void AssertSourcePlayerPortrait(Brush brush, string message)
        => AssertExactSourcePortrait(brush, StandardOlSpLiuBei664Package.GeneralId, SourcePlayerPortraitPath, message);

    private static void AssertExactSourcePortrait(Brush brush, string generalId, string path, string message)
    {
        var actual = (brush as ImageBrush)?.ImageSource as BitmapSource;
        var native = CardGame.Wpf.Resources.AssetImages.Load(path);
        var expected = actual is null ? null : CardGame.Wpf.Resources.AssetImages.Load(path, decodeWidth: actual.PixelWidth);
        Program.Assert(GeneralArt.GetSkin(generalId)?.LocalPath == path &&
                native is { PixelWidth: 146, PixelHeight: 172 } && actual is not null && expected is not null &&
                actual.PixelWidth == expected.PixelWidth && actual.PixelHeight == expected.PixelHeight, message);
        var stride = actual!.PixelWidth * 4;
        var pixels = new byte[stride * actual.PixelHeight]; var sourcePixels = new byte[pixels.Length];
        new FormatConvertedBitmap(actual, PixelFormats.Bgra32, null, 0).CopyPixels(pixels, stride, 0);
        new FormatConvertedBitmap(expected!, PixelFormats.Bgra32, null, 0).CopyPixels(sourcePixels, stride, 0);
        Program.Assert(pixels.AsSpan().SequenceEqual(sourcePixels), message + " The decoded pixels must match the exact official source card.");
    }

    private static void VerifyRegionChoiceOnlyChangesPresentation(string output)
    {
        var source = StandardShanheCampaignCatalog.CreateSourceCodexDefinition(11);
        var cities = Enumerable.Range(5011, 7).Concat(Enumerable.Range(5081, 7)).Select(StandardShanheCampaignCatalog.Source2026CityNodeId).ToHashSet();
        var definition = ShanheCampaignContent.FreezeAndValidate(source with
        {
            StartingNodeIds = cities.ToArray(), RandomStartingNode = false,
            Nodes = source.Nodes.Where(node => cities.Contains(node.Id)).Select(node => node with
            {
                NextNodeIds = [], EliteUpgradeNodeId = null,
                ChapterRouteId = null, ChapterRole = ShanheChapterNodeRole.None, ChapterProgressStep = 0, RequiredChapterProgress = 0
            }).ToArray()
        });
        var session = ShanheCampaignSession.Create(definition,
            new(71, StandardOlSpLiuBei664Package.GeneralId) { SourceDifficultyId = 11, UseCodexRewardPool = true },
            StandardShanheCampaignCatalog.CreateBattleRegistry);
        using var model = new MainViewModel(false, 71, true, new MemorySaveStore(), useExpandedContent: true,
            shanheSaveStore: new MemoryShanheCampaignSaveStore(), shanheCodexStore: new MemoryShanheCodexStore());
        model.SelectedShanheScenario = model.ShanheScenarios.Single(option => option.Id == StandardShanheCampaignCatalog.Source2026EditionId);
        model.SelectedShanheDifficulty = model.ShanheDifficulties.Single(option => option.Value == 11);
        typeof(MainViewModel).GetField("_shanheSession", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(model, session);
        typeof(MainViewModel).GetField("_shanheRunDifficulty", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(model, 11);
        typeof(MainViewModel).GetMethod("RefreshShanhePresentation", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(model, null);
        var before = session.Snapshot;
        var panel = new ShanheCampaignPanel { DataContext = model };
        try
        {
            // This deliberately supported multi-region presentation fixture keeps
            // its existing non-route graph; natural current routes offer one region.
            foreach (var size in new[] { new Size(1120, 780), new Size(1440, 880) })
            {
                panel.Measure(size); panel.Arrange(new Rect(new Point(), size)); panel.UpdateLayout();
                var selector = Program.Find<ComboBox>(panel).Single(control => control.Name == "ShanheSourceRegionSelector");
                var toolbar = (FrameworkElement)panel.FindName("ShanheMapToolbar");
                var gold = (FrameworkElement)panel.FindName("ShanheMapGoldCounter");
                var viewport = (ScrollViewer)panel.FindName("ShanheMapScroll");
                var map = (ShanheJourneyMap)panel.FindName("ShanheMapRows");
                Program.Assert(model.HasShanheSourceRegionChoices && selector.Visibility == Visibility.Visible &&
                        selector.Items.Cast<ShanheSourceRegionOption>().Select(option => option.Name).ToHashSet().SetEquals(["大谷关", "孟津"]) &&
                        selector.IsDescendantOf(toolbar) && gold.IsDescendantOf(toolbar) &&
                        !map.IsAncestorOf(selector) && !map.IsAncestorOf(gold) &&
                        ReferenceEquals(panel.FindName("ShanheSourceRegionSelector"), selector),
                    "The two real region options and copper balance share the panel's fixed toolbar outside the scaled map and its name scope.");
                var selectorBounds = selector.TransformToAncestor(panel).TransformBounds(new Rect(selector.RenderSize));
                var goldBounds = gold.TransformToAncestor(panel).TransformBounds(new Rect(gold.RenderSize));
                var toolbarBounds = toolbar.TransformToAncestor(panel).TransformBounds(new Rect(toolbar.RenderSize));
                var viewportBounds = viewport.TransformToAncestor(panel).TransformBounds(new Rect(viewport.RenderSize));
                var objective = (TextBlock)panel.FindName("ShanheChapterObjective");
                var objectiveBounds = objective.TransformToAncestor(panel).TransformBounds(new Rect(objective.RenderSize));
                Program.Assert(selector.ActualWidth > 0 && selector.ActualHeight > 0 && gold.ActualWidth > 0 && gold.ActualHeight > 0 &&
                        viewportBounds.Contains(toolbarBounds) && viewportBounds.Contains(selectorBounds) && viewportBounds.Contains(goldBounds) &&
                        !selectorBounds.IntersectsWith(objectiveBounds) && !goldBounds.IntersectsWith(objectiveBounds) &&
                        !selectorBounds.IntersectsWith(goldBounds) && !gold.IsHitTestVisible &&
                        ((TextBlock)panel.FindName("ShanheMapGoldValue")).Text == model.ShanheGoldText &&
                        selector.Foreground is System.Windows.Media.SolidColorBrush foreground && foreground.Color.R < 128 && foreground.Color.G < 128 && foreground.Color.B < 128,
                    $"At {size}, the readable region selector and actual copper balance must remain fully inside the viewport without covering each other or the chapter objective. " +
                    $"viewport={viewportBounds}, toolbar={toolbarBounds}, selector={selectorBounds}, gold={goldBounds}, objective={objectiveBounds}.");
                selector.SelectedItem = model.ShanheSourceRegions.First();
                panel.Dispatcher.Invoke(() => { }, DispatcherPriority.DataBind); panel.UpdateLayout();
                Program.Assert(model.ShanheMapNodes.Count == 7 && model.ShanheMapNodes.All(node => node.Node.Name.StartsWith("大谷关", StringComparison.Ordinal)),
                    "The first actual region option still reveals its own seven destinations.");
                selector.SelectedItem = model.ShanheSourceRegions.Last();
                panel.Dispatcher.Invoke(() => { }, DispatcherPriority.DataBind); panel.UpdateLayout();
                Program.Assert(model.ShanheMapNodes.Count == 7 && model.ShanheMapNodes.All(node => node.Node.Name.StartsWith("孟津", StringComparison.Ordinal)) &&
                        ReferenceEquals(before, session.Snapshot) && session.AcceptedCommands.Count == 0,
                    "Switching regions at either supported width reveals all seven Mengjin destinations without choosing a route, rerolling an encounter or committing a campaign command.");
                Program.Render(panel, (int)size.Width, (int)size.Height, Path.Combine(output,
                    size.Width == 1440 ? "shanhe-source-region-choice.png" : "shanhe-source-region-choice-1120.png"));
            }
        }
        finally { panel.DataContext = null; }
    }

    private static ShanheCampaignSession Session(MainViewModel model) => (ShanheCampaignSession)typeof(MainViewModel)
        .GetField("_shanheSession", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(model)!;
}
