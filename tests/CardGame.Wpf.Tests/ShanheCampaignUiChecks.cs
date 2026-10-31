using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Wpf;
using CardGame.Wpf.Controls;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.ViewModels;

internal static partial class ShanheCampaignUiChecks
{
    public static void StoryMapChoicesAndIndependentColdRestore(string output)
    {
        var fixture = Path.Combine(Path.GetFullPath(output), "shanhe-save-" + Guid.NewGuid().ToString("N"));
        var campaign = new FileShanheCampaignSaveStore(fixture);
        var ordinary = new MemorySaveStore();
        try
        {
            using var vm = Create(ordinary, campaign);
            vm.SelectedShanheScenario = vm.ShanheScenarios.Single(option => option.Id == StandardShanheCampaignCatalog.LocalEditionId);
            LegacySaveFixtures.Save(vm);
            var ordinarySave = GameCheckpointJson.Serialize(ordinary.Read(GameSaveSlot.Manual).Checkpoint);
            var ordinarySaveStatus = vm.SaveStatus;
            var window = new MainWindow(vm);
            window.ApplyTemplate();
            var root = (FrameworkElement)window.Content;
            try
            {
                vm.OpenShanheCommand.Execute(null);
                Layout(root);
                var panel = (ShanheCampaignPanel)window.FindName("ShanhePanel");
                Program.Assert(vm.IsShanheOpen && vm.ShanheEditionNotice.Contains("本地起步版") &&
                    vm.ShanheContextDescription.Contains("诸侯") && vm.ShanheGenerals.Count > 1 &&
                    vm.ShanheDifficulties.Count == 3,
                    "The distinct campaign entry must explain its local edition, story, character choice and three local difficulties.");
                Program.Render(root, 1440, 880, Path.Combine(output, "shanhe-startup.png"));
                ClickNavigation((Button)panel.FindName("ShanhePrepareButton"), root);
                Program.Render(root, 1440, 880, Path.Combine(output, "shanhe-general-selection.png"));
                Click((Button)panel.FindName("ShanheStartButton"), root);
                var start = vm.ShanheMapRows.SelectMany(row => row.Nodes).Single(node => node.IsAvailable);
                Program.Assert(campaign.Exists && start.KindLabel == "奇遇" && vm.ShanheContextDescription.Length > 0,
                    "A real new expedition must save its randomly selected story entry before any encounter starts.");
                var ordinaryGame = Program.Engine(vm);
                var ordinaryRevision = ordinaryGame.Revision;
                var focusSurface = (FrameworkElement)typeof(MainWindow)
                    .GetMethod("CurrentFocusSurface", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(window, [vm])!;
                Program.Assert(focusSurface.Name == "ShanheOverlay" && FocusManager.GetIsFocusScope(panel) &&
                    KeyboardNavigation.GetTabNavigation(panel) == KeyboardNavigationMode.Cycle &&
                    !TableShortcutChecks.Input(window, Key.Enter) &&
                    TableShortcutChecks.Input(window, Key.Enter, ModifierKeys.Control) &&
                    ordinaryGame.Revision == ordinaryRevision && campaign.Read().Campaign.Commands.Count == 0,
                    "The map must own modal focus and its Enter routes must leave the hidden table and campaign journal untouched.");
                Program.Assert(TableShortcutChecks.Input(window, Key.Escape) && !vm.IsShanheOpen &&
                    ordinaryGame.Revision == ordinaryRevision,
                    "Escape must close the campaign surface without answering the hidden ordinary prompt.");
                vm.OpenShanheCommand.Execute(null);
                Layout(root);
                // Use the currently published node after the panel has refreshed its immutable UI choices.
                start = vm.ShanheMapRows.SelectMany(row => row.Nodes).Single(node => node.IsAvailable);
                Program.Render(root, 1440, 880, Path.Combine(output, "shanhe-campaign-map.png"));
                Click(Bound(panel, vm.PreviewShanheNodeCommand, start), root);
                Program.Assert(vm.IsShanheOpen && vm.ShanheChoices.Count >= 2 &&
                    vm.ShanheContextDescription.Length > 15 && !vm.IsShanheBattleSession,
                    "The real opening story node must offer meaningful choices without replacing the ordinary battle.");
                var choice = vm.ShanheChoices.First(option => option.IsEnabled);
                Program.Render(root, 1440, 880, Path.Combine(output, "178-shanhe-story-choice.png"));
                Click(Bound(panel, vm.ChooseShanheOptionCommand, choice), root);
                Program.Assert(vm.ShanheChoices.Count == 0 && vm.ShanheRouteSummary.Contains(start.Name) &&
                    campaign.Read().Campaign.Commands.Count == 2 && vm.SaveStatus == ordinarySaveStatus && !vm.HasSaveError,
                    "One actual story choice must close the event, retain the visited route and save exactly two campaign actions.");
                var expectedVital = vm.ShanheVitalText;
                var expectedGold = vm.ShanheGoldText;
                var expectedRoute = vm.ShanheRouteSummary;
                var originalBytes = File.ReadAllText(campaign.PathName);
                using var cold = Create(ordinary, campaign);
                cold.ContinueShanheCommand.Execute(null);
                Program.Assert(cold.IsShanheOpen && cold.ShanheVitalText == expectedVital && cold.ShanheGoldText == expectedGold &&
                    cold.ShanheRouteSummary == expectedRoute && File.ReadAllText(campaign.PathName) == originalBytes &&
                    GameCheckpointJson.Serialize(ordinary.Read(GameSaveSlot.Manual).Checkpoint) == ordinarySave,
                    "A cold campaign restore must preserve route, resources and its source file while leaving the ordinary manual save unchanged.");
                var invalid = campaign.Read() with { Difficulty = 4 };
                var rejected = false;
                try { campaign.Write(invalid); } catch (InvalidDataException) { rejected = true; }
                Program.Assert(rejected && File.ReadAllText(campaign.PathName) == originalBytes &&
                    !Directory.EnumerateFiles(fixture, ".shanhe-*.tmp").Any(),
                    "An invalid save must be rejected before replacing the previous file or leaving temporary output.");
                var fresh = ShanheCampaignJsonText(campaign);
                Program.Assert(!vm.StartShanheCommand.CanExecute(null), "An active expedition must lock its chosen general and starting loadout until it ends.");
                vm.StartShanheCommand.Execute(null);
                Program.Assert(!vm.SelectShanheNodeCommand.CanExecute(start), "An earlier revision's map node must be retired.");
                vm.SelectShanheNodeCommand.Execute(start);
                vm.ChooseShanheOptionCommand.Execute(choice);
                Program.Assert(ShanheCampaignJsonText(campaign) == fresh,
                    "A disabled restart and retired map or story objects must leave the active expedition unchanged.");
            }
            finally { window.Content = null; window.DataContext = null; window.Close(); }
        }
        finally
        {
            // Exact files in this check's fresh fixture; never consume the historical cleanup queue.
            if (File.Exists(campaign.PathName)) File.Delete(campaign.PathName);
            if (File.Exists(campaign.PathName + ".previous")) File.Delete(campaign.PathName + ".previous");
            if (Directory.Exists(fixture)) Directory.Delete(fixture, recursive: false);
        }
    }

    public static void ActualEncounterSaveRoutingAndStaleMapChoices(string output)
    {
        var ordinary = new MemorySaveStore();
        var campaign = new SwitchingShanheSaveStore();
        using var vm = Create(ordinary, campaign);
        vm.SelectedShanheScenario = vm.ShanheScenarios.Single(option => option.Id == StandardShanheCampaignCatalog.LocalEditionId);
        LegacySaveFixtures.Save(vm);
        var ordinarySave = GameCheckpointJson.Serialize(ordinary.Read(GameSaveSlot.Manual).Checkpoint);
        vm.OpenShanheCommand.Execute(null);
        vm.StartShanheCommand.Execute(null);
        var opening = vm.ShanheMapRows.SelectMany(row => row.Nodes).Single(node => node.IsAvailable);
        vm.SelectShanheNodeCommand.Execute(opening);
        var definition = StandardShanheCampaignCatalog.CreateDefinition();
        var openingDefinition = definition.Nodes.Single(node => node.Id == opening.Id);
        var tacticChoiceIds = (openingDefinition.Choices ?? []).Where(choice => choice.RewardId is { } rewardId &&
            definition.Rewards.Single(reward => reward.Id == rewardId).TacticSkillId is not null)
            .Select(choice => choice.Id).ToHashSet(StringComparer.Ordinal);
        var startingTactic = vm.ShanheChoices.FirstOrDefault(option => option.IsEnabled && tacticChoiceIds.Contains(option.Id));
        Program.Assert(startingTactic is not null, "The opening must offer at least one real tactic so the first encounter can validate its native skill grant.");
        vm.ChooseShanheOptionCommand.Execute(startingTactic);
        var tacticCard = vm.ShanheTacticCards.Single();
        Program.Assert(tacticCard.Description.Length > 10 && tacticCard.SourceText.Contains("不占额外技能槽") &&
            vm.ShanheExtraSkillCards.Count == 0,
            "Acquiring a tactic must publish its full build description without occupying a learned-general skill slot.");
        var battleNode = vm.ShanheMapRows.SelectMany(row => row.Nodes).Single(node => node.IsAvailable);
        vm.SelectShanheNodeCommand.Execute(battleNode);
        var game = Program.Engine(vm);
        var tacticIds = game.CreateCheckpoint().Options.ShanheBattle!.TacticSkillIds;
        Program.Assert(tacticIds.Count == 1 && tacticIds.All(id => vm.HumanSkillCards.All(skill => skill.ContentId != id) &&
            vm.HumanPlayer!.WushuangTactics.Any(tactic => tactic.SkillId == id &&
                tactic.Tooltip.Contains(game.ContentRegistry!.GetSkill(id).Description, StringComparison.Ordinal))) && vm.ShanheExtraSkillCards.Count == 0,
            "The real encounter exposes its native passive tactic through the shared tactic badge rail without consuming a skill slot.");
        Program.Assert(vm.IsShanheBattleSession && vm.IsShanheBattleActive && !vm.IsShanheOpen &&
            game.CreateSnapshot(0).Players.Count == 2 && game.AcceptedCommands.OfType<StartGameCommand>().Count() == 1,
            "The real route must create one native two-seat encounter and submit exactly one normal StartGame command.");
        Program.Assert(vm.TableModeText.Contains(battleNode.Name) && vm.WindowTitle.Contains("山河图") &&
            !vm.TableModeText.Contains("2v2") && vm.IdentityObjective.Contains("独自迎敌") &&
            vm.IdentityRevealRoster.Contains("义军 1 席") && vm.AliveText.Contains("敌军 1") &&
            vm.Seats.Select(seat => seat.RoleLabel).ToHashSet().SetEquals(["义军", "敌军"]) &&
            vm.ShanheCarryoverNotice.Contains("技能槽") && vm.ShanheCarryoverNotice.Contains("剩余体力"),
            "The native table must describe its actual single-player encounter, public sides and persistent build rather than a standard 2v2.");
        LegacySaveFixtures.Save(vm);
        Program.Assert(vm.FlushPendingSave() && campaign.Read().Battle is null && !vm.HasSaveError &&
            vm.SaveStatus.Contains("山河图进度已保存") &&
            GameCheckpointJson.Serialize(ordinary.Read(GameSaveSlot.Manual).Checkpoint) == ordinarySave,
            "Manual and close-flush saves during a campaign encounter must use the independent campaign file.");
        var expected = SnapshotJson.Serialize(game.CreateSnapshot(0));
        var campaignJournal = ShanheCampaignJsonText(campaign);
        var previousSave = JsonSerializer.Serialize(campaign.Read());
        campaign.FailWrites = true;
        LegacySaveFixtures.Save(vm);
        Program.Assert(vm.HasSaveError && vm.SaveStatus.Contains("独立征程未保存") &&
            (bool)typeof(MainViewModel).GetField("_autoSavePending", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(vm)! &&
            SnapshotJson.Serialize(game.CreateSnapshot(0)) == expected && ShanheCampaignJsonText(campaign) == campaignJournal &&
            JsonSerializer.Serialize(campaign.Read()) == previousSave,
            "A failed campaign manual save must expose its unsaved state on the table, retain its pending retry and preserve the prior file and live battle.");
        campaign.FailWrites = false;
        LegacySaveFixtures.Save(vm);
        Program.Assert(!vm.HasSaveError && vm.SaveStatus.Contains("山河图进度已保存") &&
            !(bool)typeof(MainViewModel).GetField("_autoSavePending", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(vm)!,
            "A later successful manual campaign save must clear both the visible error and the pending retry.");
        var saved = campaign.Read();
        var savedBattle = game.CreateCheckpoint();
        var wrongBattle = savedBattle with { Options = savedBattle.Options with { Seed = savedBattle.Options.Seed ^ 1 } };
        foreach (var invalidBattle in new[] { wrongBattle, savedBattle with { Options = null! } })
        {
            var mismatchedStore = new MemoryShanheCampaignSaveStore();
            mismatchedStore.Write(saved with { Battle = invalidBattle });
            var source = JsonSerializer.Serialize(mismatchedStore.Read());
            using var invalid = Create(ordinary, mismatchedStore);
            var untouched = SnapshotJson.Serialize(Program.Engine(invalid).CreateSnapshot(0));
            invalid.ContinueShanheCommand.Execute(null);
            Program.Assert(invalid.HasShanheSession && !invalid.IsShanheBattleSession && invalid.IsShanheBattleRetryReady &&
                SnapshotJson.Serialize(Program.Engine(invalid).CreateSnapshot(0)) == untouched &&
                JsonSerializer.Serialize(mismatchedStore.Read()) == source,
                "Legacy in-flight battle data is ignored, so invalid battle options cannot run or prevent restoring the validated expedition progress.");
        }
        using var cold = Create(ordinary, campaign);
        cold.ContinueShanheCommand.Execute(null);
        Program.Assert(cold.IsShanheOpen && cold.IsShanheBattleRetryReady && !cold.IsShanheBattleSession,
            "A fresh view model restores the encounter entry without reviving an in-flight battle.");
        cold.ResumeShanheBattleCommand.Execute(null);
        var retriedSetup = Program.Engine(cold).CreateCheckpoint().Options.ShanheBattle!;
        var savedSetup = savedBattle.Options.ShanheBattle!;
        Program.Assert(retriedSetup.AttemptNumber == savedSetup.AttemptNumber + 1 &&
            retriedSetup.BattleSeed != savedSetup.BattleSeed &&
            ShanheBattleSetup.ValueEquals(retriedSetup with { AttemptNumber = savedSetup.AttemptNumber }, savedSetup),
            "Retry keeps the same genuine encounter recipe and resources while issuing a new shuffled challenge.");
        var retriedJournal = ShanheCampaignJsonText(campaign);
        Program.Assert(!cold.IsShanheOpen && !cold.SelectShanheNodeCommand.CanExecute(battleNode),
            "Returning to the restored battle must retire all earlier view model map objects.");
        cold.SelectShanheNodeCommand.Execute(battleNode);
        Program.Assert(ShanheCampaignJsonText(campaign) == retriedJournal,
            "An old encounter button must not restart or advance a restored battle.");
        var window = new MainWindow(cold);
        window.ApplyTemplate();
        try
        {
            var root = (FrameworkElement)window.Content;
            Program.Assert(TableShortcutChecks.Input(window, Key.F5), "The real table F5 route must accept a campaign manual save.");
            Program.Render(root, 1440, 880, Path.Combine(output, "179-shanhe-real-encounter.png"));
            AssertUniqueOpponentRings(window, cold, 2);
            Program.Assert(cold.TopSeats.Count == 1 && cold.LeftSeats.Count == 0 && cold.RightSeats.Count == 0,
                "A two-seat encounter must display its sole opponent once, centered above the human seat.");
            Program.Assert(Program.Find<TextBlock>(root).Any(text => text.Text.Contains("山河图进度已保存") &&
                BindingOperations.GetBinding(text, TextBlock.TextProperty)?.Path?.Path == nameof(MainViewModel.SessionSaveStatus)),
                "The actual battle dock must bind the independent campaign manual-save confirmation while the map is closed.");
            cold.OpenShanheCommand.Execute(null);
            Layout(root);
            var panel = (ShanheCampaignPanel)window.FindName("ShanhePanel");
            ((TabControl)panel.FindName("ShanheJourneyTabs")).SelectedIndex = 1;
            cold.ShanheCodexShowCollection = true;
            Program.Render(root, 1440, 880, Path.Combine(output, "shanhe-campaign-build.png"));
            Program.Assert(Program.Find<ShanheCodexPanel>(panel).Single().DataContext == cold &&
                cold.ShanheCodexCards.Any(card => card.Id == tacticCard.Id && card.Description == tacticCard.Description) &&
                cold.ShanheTacticCards.Single().Description == tacticCard.Description,
                "The actual build tab must render the restored tactic's complete description.");
            cold.CloseShanheCommand.Execute(null);
        }
        finally { window.Content = null; window.DataContext = null; window.Close(); }
        cold.StartNewGameCommand.Execute(null);
        Program.Assert(!cold.IsShanheBattleSession, "An ordinary new match must detach the campaign table marker.");
        cold.OpenShanheCommand.Execute(null);
        cold.ResumeShanheBattleCommand.Execute(null);
        var switchedSetup = Program.Engine(cold).CreateCheckpoint().Options.ShanheBattle!;
        Program.Assert(cold.IsShanheBattleSession && switchedSetup.AttemptNumber == retriedSetup.AttemptNumber + 1 &&
            switchedSetup.BattleSeed != retriedSetup.BattleSeed &&
            ShanheBattleSetup.ValueEquals(switchedSetup with { AttemptNumber = savedSetup.AttemptNumber }, savedSetup),
            "After switching modes the expedition resumes the same encounter and resources, with another new shuffle.");
        RenderThreeSeatRepresentative(output, savedBattle.Options.ShanheBattle!);
    }

    internal static void AssertEncounterRewardSummaryLayout(MainViewModel model, ShanheCampaignPanel panel, FrameworkElement root)
    {
        var rows = (ItemsControl)panel.FindName("ShanheEncounterRewardSummaryRows");
        var scroll = (ScrollViewer)panel.FindName("ShanheEncounterRewardScroll");
        var description = (TextBlock)panel.FindName("ShanheEncounterRewardDescription");
        var summary = model.ShanheEncounterRewardSummary;
        Program.Assert(model.IsShanheEncounterPreviewOpen && summary.Count > 0 && rows.Items.Count == summary.Count &&
                rows.Items.Cast<ShanheEncounterRewardSummaryItem>().SequenceEqual(summary) &&
                scroll.VerticalScrollBarVisibility == ScrollBarVisibility.Auto &&
                scroll.HorizontalScrollBarVisibility == ScrollBarVisibility.Disabled &&
                scroll.ViewportHeight > 0 && description.Text == model.ShanheEncounterRewardText &&
                description.TextWrapping == TextWrapping.Wrap && description.FontSize >= 12,
            "The encounter exposes compact bound base-reward summaries above its complete original rule text in one bounded scroll surface.");
        var tiles = new List<Border>();
        for (var index = 0; index < summary.Count; index++)
        {
            var item = summary[index];
            var container = (FrameworkElement)rows.ItemContainerGenerator.ContainerFromIndex(index);
            var tile = Program.Find<Border>(container).Single(border => border.Name == "EncounterRewardSummaryTile");
            tiles.Add(tile);
            var title = Program.Find<TextBlock>(tile).Single(text => text.Name == "EncounterRewardSummaryTitle");
            var value = Program.Find<TextBlock>(tile).Single(text => text.Name == "EncounterRewardSummaryValue");
            var icon = Program.Find<Image>(tile).Single(image => image.Name == "EncounterRewardSummaryIcon");
            Program.Assert(title.Text == item.Title && value.Text == item.ValueText && title.FontSize >= 13 && value.FontSize >= 14 &&
                    ReferenceEquals(icon.Source, item.Icon) && icon.Visibility == (item.Icon is null ? Visibility.Collapsed : Visibility.Visible) &&
                    AutomationProperties.GetName(tile) == item.Title && AutomationProperties.GetItemStatus(tile) == item.ValueText &&
                    AutomationProperties.GetHelpText(tile) == item.Detail && tile.ToolTip is ToolTip,
                "Each actual base-reward tile binds its exact title, base value, original icon and complete accessible explanation.");
            foreach (var text in new[] { title, value })
            {
                text.BringIntoView(); Flush();
                ShanheResponsiveMarketUiChecks.AssertTextGlyphsWithin(text, tile,
                    "The actual encounter base-reward tile caption", checkAncestorClips: true);
            }
            if (PresentationSource.FromVisual(root) is HwndSource)
            {
                var tip = (ToolTip)tile.ToolTip;
                tip.PlacementTarget = tile; tip.Placement = PlacementMode.AbsolutePoint;
                tip.HorizontalOffset = -30000; tip.VerticalOffset = -30000;
                tip.Opened += ConcealEncounterTooltip;
                try
                {
                    tip.IsOpen = true; Flush(); tip.UpdateLayout();
                    Program.Assert(tip.IsOpen && tip.ContentTemplate is null && PresentationSource.FromVisual(tip) is HwndSource &&
                            Program.Find<TextBlock>(tip).Any(text => text.Text == item.Detail && text.ActualWidth > 0 && text.ActualHeight > 0),
                        "The actual native base-reward explanation tooltip renders its complete bound meaning rather than a type name.");
                }
                finally { tip.IsOpen = false; tip.Opened -= ConcealEncounterTooltip; }
            }
        }
        if (tiles.Count > 1)
        {
            var first = tiles[0].TransformToAncestor(rows).TransformBounds(new Rect(tiles[0].RenderSize));
            var second = tiles[1].TransformToAncestor(rows).TransformBounds(new Rect(tiles[1].RenderSize));
            Program.Assert(Math.Abs(first.Top - second.Top) <= .7 && second.Left >= first.Right - .7,
                "The compact copper and next base-reward tile use separate parallel columns rather than a single tall text stack.");
        }
        scroll.ScrollToBottom(); Flush();
        var lastCharacter = description.ContentEnd.GetCharacterRect(System.Windows.Documents.LogicalDirection.Backward);
        var lastBounds = description.TransformToAncestor(scroll).TransformBounds(lastCharacter);
        Program.Assert(!lastCharacter.IsEmpty && lastBounds.Left >= -.7 && lastBounds.Top >= -.7 &&
                lastBounds.Right <= scroll.ViewportWidth + .7 && lastBounds.Bottom <= scroll.ViewportHeight + .7,
            "The complete original reward explanation remains scrollable to its final actual character below every compact summary: " +
            JsonSerializer.Serialize(new { description.Text, Last = lastBounds.ToString(), scroll.ViewportWidth, scroll.ViewportHeight }));
        var scrollBounds = scroll.TransformToAncestor(root).TransformBounds(new Rect(scroll.RenderSize));
        foreach (var action in new[] { (Button)panel.FindName("ShanheChallengeButton"), (Button)panel.FindName("ShanheEncounterCloseButton") })
        {
            var actionBounds = action.TransformToAncestor(root).TransformBounds(new Rect(action.RenderSize));
            var overlap = Rect.Intersect(scrollBounds, actionBounds);
            Program.Assert(action.IsEnabled && action.Command?.CanExecute(action.CommandParameter) == true &&
                    (overlap.IsEmpty || overlap.Width <= .7 || overlap.Height <= .7),
                "Scrolling through the compact reward preview leaves the actual challenge and close actions separate and executable.");
        }
        scroll.ScrollToTop(); Flush();

        void Flush() { root.Dispatcher.Invoke(() => { }, DispatcherPriority.ContextIdle); root.UpdateLayout(); }
    }

    internal static void AssertEncounterRewardIcon(ShanheEncounterRewardSummaryItem item, string path)
    {
        var actual = item.Icon as BitmapSource;
        var expected = CardGame.Wpf.Resources.AssetImages.Load(path);
        Program.Assert(actual is not null && expected is not null && actual.PixelWidth == expected.PixelWidth && actual.PixelHeight == expected.PixelHeight,
            "The base-reward category uses the exact dimensions of its established original source icon: " + path);
        var stride = actual!.PixelWidth * 4;
        var actualPixels = new byte[stride * actual.PixelHeight];
        var sourcePixels = new byte[actualPixels.Length];
        new FormatConvertedBitmap(actual, PixelFormats.Bgra32, null, 0).CopyPixels(actualPixels, stride, 0);
        new FormatConvertedBitmap(expected!, PixelFormats.Bgra32, null, 0).CopyPixels(sourcePixels, stride, 0);
        Program.Assert(actualPixels.AsSpan().SequenceEqual(sourcePixels),
            "The base-reward category retains the original source pixels and does not substitute a particular unrolled reward: " + path);
    }

    private static void ConcealEncounterTooltip(object sender, RoutedEventArgs args)
    {
        if (PresentationSource.FromVisual((ToolTip)sender) is HwndSource source) ShowEncounterTooltipWindow(source.Handle, 0);
    }

    [DllImport("user32.dll", EntryPoint = "ShowWindow")]
    private static extern bool ShowEncounterTooltipWindow(nint handle, int command);

    private static MainViewModel Create(IGameSaveStore ordinary, IShanheCampaignSaveStore campaign, int seed = 270017, bool confirmOpeningHand = false) =>
        new(false, seed, true, ordinary, useExpandedContent: true, shanheSaveStore: campaign, confirmOpeningHand: confirmOpeningHand)
        { IsMotionEnabled = false, IsSoundEnabled = false, IsMusicEnabled = false };

    private sealed class SwitchingShanheSaveStore : IShanheCampaignSaveStore
    {
        private readonly MemoryShanheCampaignSaveStore _inner = new();
        public bool FailWrites { get; set; }
        public bool Exists => _inner.Exists;
        public ShanheCampaignSaveFile Read() => _inner.Read();
        public void Write(ShanheCampaignSaveFile save)
        {
            if (FailWrites) throw new IOException("The campaign fixture disk is temporarily unavailable.");
            _inner.Write(save);
        }
    }

    private static void RenderThreeSeatRepresentative(string output, CardGame.Core.Campaigns.ShanheBattleSetup previousSetup)
    {
        var definition = StandardShanheCampaignCatalog.CreateDefinition();
        var node = definition.Nodes.First(candidate => candidate.Encounters?.Any(encounter =>
            encounter.Enemies.Count == 1 && encounter.Allies?.Count == 1) == true);
        var encounter = node.Encounters!.First(candidate => candidate.Enemies.Count == 1 && candidate.Allies?.Count == 1);
        var setup = previousSetup with
        {
            NodeId = node.Id, EncounterOrdinal = previousSetup.EncounterOrdinal + 1,
            EnemyRoster = encounter.Enemies, AllyRoster = encounter.Allies!
        };
        var battle = StandardShanheCampaignCatalog.CreateBattle(setup);
        var started = battle.Submit(new StartGameCommand());
        Program.Assert(started.Accepted && battle.CreateSnapshot(0).Players.Count == 3,
            "The three-seat layout fixture must start a real catalog encounter with one AI ally and one enemy.");
        using var model = Create(new MemorySaveStore(), new MemoryShanheCampaignSaveStore());
        // Bind a real native encounter without fabricating victories through the preceding campaign chapters.
        typeof(MainViewModel).GetField("_shanheTableGame", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(model, battle);
        typeof(MainViewModel).GetField("_shanheTableSetup", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(model, setup);
        typeof(MainViewModel).GetMethod("ReplaceEngine", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(model, [battle]);
        model.IsNewGameSetupOpen = false;
        var window = new MainWindow(model);
        window.ApplyTemplate();
        try
        {
            Program.Render((FrameworkElement)window.Content, 1440, 880, Path.Combine(output, "shanhe-three-seat-encounter.png"));
            AssertUniqueOpponentRings(window, model, 3);
            Program.Assert(model.LeftSeats.Count == 1 && model.RightSeats.Count == 1 && model.TopSeats.Count == 0 &&
                model.Seats.Count(seat => seat.IsTeammate) == 1 && model.IdentityRevealRoster.Contains("义军 2 席"),
                "A three-seat native encounter must render the distinct AI ally and enemy once on opposite sides.");
        }
        finally { window.Content = null; window.DataContext = null; window.Close(); }
    }

    private static void AssertUniqueOpponentRings(MainWindow window, MainViewModel model, int playerCount)
    {
        var rings = new[] { "TopSeatRing", "LeftSeatRing", "RightSeatRing" }
            .Select(name => (ItemsControl)window.FindName(name)).ToArray();
        var displayed = rings.SelectMany(ring => ring.Items.Cast<SeatViewModel>()).ToArray();
        var expected = model.Seats.Where(seat => !seat.IsHuman).Select(seat => seat.Seat).ToHashSet();
        Program.Assert(model.Seats.Count == playerCount && displayed.Length == playerCount - 1 &&
            displayed.Select(seat => seat.Seat).Distinct().Count() == displayed.Length &&
            displayed.Select(seat => seat.Seat).ToHashSet().SetEquals(expected),
            "The actual three XAML rings must partition all non-human seats without duplicating or dropping a target.");
        foreach (var ring in rings)
        foreach (var seat in ring.Items.Cast<SeatViewModel>())
            Program.Assert(ring.ItemContainerGenerator.ContainerFromItem(seat) is FrameworkElement container &&
                ReferenceEquals(container.DataContext, seat),
                "Each opponent must have one generated native seat container bound to its own unique seat model.");
    }

    private static string ShanheCampaignJsonText(IShanheCampaignSaveStore store) =>
        CardGame.Core.Campaigns.ShanheCampaignJson.Serialize(store.Read().Campaign);

    private static Button Bound(FrameworkElement root, System.Windows.Input.ICommand command, object parameter) =>
        Program.Find<Button>(root).Single(button => ReferenceEquals(button.Command, command) && ReferenceEquals(button.CommandParameter, parameter));

    private static void Click(Button button, FrameworkElement root)
    {
        Program.Assert(button.IsEnabled && button.Command?.CanExecute(button.CommandParameter) == true,
            "The actual campaign button must be enabled before its routed click.");
        typeof(ButtonBase).GetMethod("OnClick", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(button, null);
        Layout(root);
    }

    private static void ClickNavigation(Button button, FrameworkElement root)
    {
        Program.Assert(button.IsEnabled, "A campaign navigation button must be enabled before its routed click.");
        typeof(ButtonBase).GetMethod("OnClick", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(button, null);
        Layout(root);
    }

    private static void Layout(FrameworkElement root)
    {
        root.Measure(new Size(1440, 880)); root.Arrange(new Rect(0, 0, 1440, 880)); root.UpdateLayout();
        root.Dispatcher.Invoke(() => { }, DispatcherPriority.DataBind); root.UpdateLayout();
    }
}
