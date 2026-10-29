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

    public static void EditionSelectionExclusiveBossAndColdRestore(string output)
    {
        var ordinary = new MemorySaveStore();
        var campaign = new MemoryShanheCampaignSaveStore();
        using var vm = Create(ordinary, campaign, seed: 1);
        var window = new MainWindow(vm);
        window.ApplyTemplate();
        var root = (FrameworkElement)window.Content;
        try
        {
            vm.OpenShanheCommand.Execute(null);
            Layout(root);
            var panel = (ShanheCampaignPanel)window.FindName("ShanhePanel");
            ClickNavigation((Button)panel.FindName("ShanhePrepareButton"), root);
            var selector = (ComboBox)panel.FindName("ShanheScenarioSelector");
            selector.SelectedItem = vm.ShanheScenarios.Single(option => option.Id == StandardShanheCampaignCatalog.Returning2026EditionId);
            vm.ToggleShanheGeneralRosterCommand.Execute(null);
            vm.SelectedShanheGeneral = vm.ShanheGenerals.Single(general => general.Id == "classic:shen-guan-yu");
            Layout(root);
            Program.Assert(vm.SelectedShanheScenario.Id == StandardShanheCampaignCatalog.Returning2026EditionId &&
                vm.ShanheEditionNotice.Contains("2026") && vm.ShanheContextDescription.Contains("神鬼无前") &&
                vm.ShanheEditionNotice.Contains("神武将出征默认按群") &&
                Program.Find<TextBlock>(root).Any(text => text.Text == vm.SelectedShanheGeneralMeta && text.Text.Contains("出征默认群")),
                "The actual edition selector must explain its verified exclusive form before starting a run.");
            Program.Assert(vm.ShanheCodexNodes.Single(node => node.TreeId == 35) is
                { IsAvailable: true, RuntimeRewardId: StandardShanheCampaignCatalog.CurrentFarStrikeRewardId },
                "The actual S5 Far Strike node is usable only where its current rule has an expedition reward.");
            selector.SelectedItem = vm.ShanheScenarios.Single(option => option.Id == StandardShanheCampaignCatalog.LocalEditionId);
            Layout(root);
            Program.Assert(!vm.SelectedShanheGeneralMeta.Contains("出征默认") && !vm.ShanheEditionNotice.Contains("神武将出征默认按群") &&
                !Program.Find<TextBlock>(root).Any(text => text.Text.Contains("出征默认群")),
                "Switching to the old edition updates the real selected-general binding and removes the new god-faction policy notice.");
            Program.Assert(!vm.ShanheCodexNodes.Single(node => node.TreeId == 35).IsAvailable,
                "The historical edition retains the source-tree preview without offering the current Far Strike variant.");
            selector.SelectedItem = vm.ShanheScenarios.Single(option => option.Id == StandardShanheCampaignCatalog.Returning2026EditionId);
            Layout(root);
            Program.Assert(vm.SelectedShanheGeneralMeta.Contains("出征默认群") &&
                Program.Find<TextBlock>(root).Any(text => text.Text == vm.SelectedShanheGeneralMeta),
                "Switching back republishes the actual selected god general's default faction before expedition creation.");
            Click((Button)panel.FindName("ShanheStartButton"), root);
            var bossNode = vm.ShanheMapRows.SelectMany(row => row.Nodes).Single(node => node.IsAvailable && node.Id == "hulao-start");
            Program.Assert(vm.ShanheMapRows.SelectMany(row => row.Nodes).Count(node => node.IsAvailable) == 2,
                "The current edition opens two actual first-chapter battles instead of a forced preparation event.");
            vm.SelectShanheNodeCommand.Execute(bossNode);
            var game = Program.Engine(vm);
            var setup = game.CreateCheckpoint().Options.ShanheBattle!;
            var bossId = setup.EnemyRoster[0].GeneralId;
            var boss = game.CreateSnapshot(0).Players.Single(player => player.GeneralId == bossId);
            // The S5 candidate pool now contains complete published groups beyond the
            // original Lv Bu/Zhang Liao forms. Bind expectations to this exact drawn
            // recipe and its registered source body rather than an old seed's four ids.
            var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup);
            var expectedGeneral = registry.Generals[bossId];
            var expectedSkills = expectedGeneral.SkillIds.Concat(setup.EnemyRoster[0].ExtraSkillIds ?? []).ToHashSet(StringComparer.Ordinal);
            var knownForm = boss.Skills!.Select(skill => skill.Id).ToHashSet(StringComparer.Ordinal).SetEquals(expectedSkills);
            if (bossId == StandardShanheBossPackage.ShenguiLvbuId)
                knownForm &= game.CreateSnapshot(0).Players.Single(player => player.GeneralId == StandardShanheBossPackage.FenghuangId)
                    .Skills!.Select(skill => skill.Id).ToHashSet().SetEquals(["ol:fudao", "ol:xiuhao"]);
            var verifiedSourcePortrait = ShanheBossPortraitChecks.GetVerifiedSourcePortraitPath(bossId);
            var declaredMissingArt = verifiedSourcePortrait is null && expectedGeneral.PortraitKey == "supporter";
            var sourcePortraitKey = expectedGeneral.PortraitKey == "dong_bai" ? "ol-dong-bai" : expectedGeneral.PortraitKey.Replace('_', '-');
            var expectedPortraitPath = verifiedSourcePortrait ?? GeneralArt.GetSkin(sourcePortraitKey)?.LocalPath;
            var portraitMatchesSource = declaredMissingArt
                ? !GeneralArt.HasPortrait(bossId) && GeneralArt.GetPortrait(bossId) is SolidColorBrush { Color.A: 0 }
                : expectedPortraitPath is not null && GeneralArt.GetSkin(bossId)?.LocalPath == expectedPortraitPath &&
                    GeneralArt.GetPortrait(bossId, decodeWidth: 96) is ImageBrush
                        { ImageSource: BitmapSource { PixelWidth: > 0, PixelHeight: > 0 } };
            Program.Assert(vm.IsShanheBattleSession && knownForm && portraitMatchesSource,
                "The selected edition starts the actually drawn dedicated source form with its exact skills and declared portrait or missing-art fallback: " +
                JsonSerializer.Serialize(new { bossId, ExpectedSkills = expectedSkills, ActualSkills = boss.Skills!.Select(skill => skill.Id), expectedGeneral.PortraitKey, declaredMissingArt }));
            Program.Assert(game.CreateCheckpoint().Options.ShanheBattle!.DefaultGodFactionId == "qun" &&
                game.CreateSnapshot(0).Players[0].FactionId == "qun",
                "The selected edition's visible god-faction notice reaches the actual battle recipe and public table faction.");
            LegacySaveFixtures.Save(vm);
            Program.Assert(vm.FlushPendingSave(), "The real exclusive encounter must save successfully.");
            var original = ShanheCampaignJsonText(campaign);
            var originalSetup = game.CreateCheckpoint().Options.ShanheBattle!;
            Program.Render(root, 1440, 880, Path.Combine(output, "180-shanhe-2026-exclusive-boss.png"));
            using var cold = Create(ordinary, campaign);
            cold.SelectedShanheScenario = cold.ShanheScenarios.Single(option => option.Id == StandardShanheCampaignCatalog.LocalEditionId);
            Program.Assert(cold.SelectedShanheScenario.Id == StandardShanheCampaignCatalog.LocalEditionId,
                "Explicitly selecting the other edition requires restoration to obtain the saved edition from its checkpoint, independently of the new preparation default.");
            cold.ContinueShanheCommand.Execute(null);
            Program.Assert(cold.IsShanheBattleRetryReady && !cold.IsShanheBattleSession && ShanheCampaignJsonText(campaign) == original,
                "Restoring an edition exposes the encounter entry before its new battle starts.");
            cold.ResumeShanheBattleCommand.Execute(null);
            var nextSetup = Program.Engine(cold).CreateCheckpoint().Options.ShanheBattle!;
            Program.Assert(cold.IsShanheBattleSession && cold.SelectedShanheScenario.Id == StandardShanheCampaignCatalog.Returning2026EditionId &&
                cold.SelectedShanheGeneralMeta.Contains("出征默认群") &&
                nextSetup.AttemptNumber == originalSetup.AttemptNumber + 1 && nextSetup.BattleSeed != originalSetup.BattleSeed &&
                ShanheBattleSetup.ValueEquals(nextSetup with { AttemptNumber = originalSetup.AttemptNumber }, originalSetup),
                "The saved edition starts the same exclusive boss and resources with a new independently recorded shuffle.");
            var mismatched = new MemoryShanheCampaignSaveStore();
            var saved = campaign.Read();
            mismatched.Write(saved with { Difficulty = saved.Difficulty == 1 ? 2 : 1 });
            using var rejected = Create(ordinary, mismatched);
            var untouched = SnapshotJson.Serialize(Program.Engine(rejected).CreateSnapshot(0));
            rejected.ContinueShanheCommand.Execute(null);
            Program.Assert(!rejected.IsShanheBattleSession && rejected.ShanheStatusText.Contains("失败") &&
                SnapshotJson.Serialize(Program.Engine(rejected).CreateSnapshot(0)) == untouched &&
                mismatched.Read().Campaign.CampaignId == saved.Campaign.CampaignId,
                "A mismatched edition/difficulty save must preserve its source and the live ordinary game.");
        }
        finally { window.Content = null; window.DataContext = null; window.Close(); }
        VerifyFengyiPreviewAndFiveSeatTable(output);
    }

    public static void LauncherSelectionMapAndMainWindowNavigation(string output)
    {
        var store = new SwitchingShanheSaveStore();
        using var vm = Create(new MemorySaveStore(), store);
        var owner = new MainWindow(vm);
        try
        {
            owner.Show();
            vm.OpenShanheCommand.Execute(null);
            var root = (FrameworkElement)owner.Content;
            Layout(root);
            var panel = (ShanheCampaignPanel)owner.FindName("ShanhePanel");
            vm.SelectedShanheScenario = vm.ShanheScenarios.Single(s => s.Id == StandardShanheCampaignCatalog.Returning2026EditionId);
            Layout(root);
            CheckPreparationLayout(panel);
            Program.Render(root, 1440, 880, Path.Combine(output, "shanhe-launcher-2026.png"));
            ClickNavigation((Button)panel.FindName("ShanhePrepareButton"), root);
            var setup = (Border)panel.FindName("ShanheSetupOverlay");
            var generals = (ListBox)panel.FindName("ShanheGeneralSelector");
            Click((Button)panel.FindName("ShanheToggleGeneralRosterButton"), root);
            Click((Button)panel.FindName("ShanheNextGeneralPageButton"), root);
            var chosen = vm.ShanheGeneralPage.Last();
            generals.SelectedItem = chosen;
            Layout(root);
            Program.Assert(setup.Visibility == Visibility.Visible &&
                !((Grid)panel.FindName("CampaignContent")).IsEnabled &&
                KeyboardNavigation.GetControlTabNavigation(setup) == KeyboardNavigationMode.Cycle &&
                vm.SelectedShanheGeneral == chosen &&
                !string.IsNullOrWhiteSpace(vm.SelectedShanheGeneralDescription) &&
                !string.IsNullOrWhiteSpace(vm.SelectedShanheSkillText),
                "The visible portrait roster must update the actual selected general and its skill preview.");
            var scenario = (ComboBox)panel.FindName("ShanheScenarioSelector");
            scenario.IsDropDownOpen = true;
            Layout(root);
            Program.Assert(!TableShortcutChecks.Input(owner, Key.Escape) && scenario.IsDropDownOpen &&
                setup.Visibility == Visibility.Visible && vm.IsShanheOpen,
                "An expanded selector owns Escape before the main window can dismiss its preparation surface.");
            scenario.IsDropDownOpen = false;
            Program.Assert(TableShortcutChecks.Input(owner, Key.Escape) && vm.IsShanheOpen &&
                setup.Visibility == Visibility.Collapsed && !store.Exists,
                "Escape must close only preparation and must not begin or save an expedition.");
            ClickNavigation((Button)panel.FindName("ShanheRulesButton"), root);
            var rules = (Border)panel.FindName("ShanheRulesOverlay");
            var details = (Expander)panel.FindName("ShanheScenarioDetailsExpander");
            details.IsExpanded = true;
            Layout(root);
            Program.Assert(((TextBlock)panel.FindName("ShanheScenarioDetailsText")).Text ==
                StandardShanheCampaignCatalog.CreateCodexDefinition(vm.SelectedShanheDifficulty.Value,
                    StandardShanheCampaignCatalog.Returning2026EditionId).IntroText && !store.Exists,
                "The short 2026 launcher keeps the exact full scenario introduction reachable through rules without starting or saving a run.");
            var rulesScroll = (ScrollViewer)panel.FindName("ShanheRulesScroll");
            rulesScroll.ScrollToBottom();
            Layout(root);
            Program.Assert(rulesScroll.VerticalOffset > 0 && rulesScroll.ScrollableHeight > 0,
                "The expanded full introduction can actually scroll to its ending inside the rules surface.");
            Program.Render(root, 1440, 880, Path.Combine(output, "shanhe-rules.png"));
            Program.Assert(rules.Visibility == Visibility.Visible && TableShortcutChecks.Input(owner, Key.Escape) &&
                rules.Visibility == Visibility.Collapsed && vm.IsShanheOpen,
                "Rules must be a dismissible inner surface without closing the expedition.");
            owner.Width = owner.MinWidth; owner.Height = owner.MinHeight;
            owner.Dispatcher.Invoke(() => { }, DispatcherPriority.ApplicationIdle);
            owner.UpdateLayout();
            CheckPreparationLayout(panel);
            Program.Render(owner, (int)owner.ActualWidth, (int)owner.ActualHeight,
                Path.Combine(output, "shanhe-launcher-compact.png"));
            ClickNavigation((Button)panel.FindName("ShanhePrepareButton"), root);
            Click((Button)panel.FindName("ShanheStartButton"), root);
            Program.Assert(store.Exists && vm.HasShanheSession && vm.ShanheMapNodes.Count > 1 &&
                vm.ShanheMapLinks.Count > 0 && vm.ShanheMapNodes.Any(node => node.IsAvailable),
                $"Preparation in the main window must create a real saved campaign and actionable linked map. Status: {vm.ShanheStatusText}");
            owner.Dispatcher.Invoke(() => { }, DispatcherPriority.ApplicationIdle);
            owner.UpdateLayout();
            var compactBounds = panel.TransformToAncestor(root).TransformBounds(new Rect(panel.RenderSize));
            Program.Assert(compactBounds.Left >= 0 && compactBounds.Top >= 0 &&
                compactBounds.Right <= root.ActualWidth + 1 && compactBounds.Bottom <= root.ActualHeight + 1,
                "The minimum main window keeps the complete campaign surface inside its client bounds.");
            var hud = Program.Find<ShanheJourneyLoadout>(panel).Single();
            var hudBounds = hud.TransformToAncestor(panel).TransformBounds(new Rect(hud.RenderSize));
            Program.Assert(hud.IsVisible && hudBounds.Right <= panel.ActualWidth + 1 &&
                hudBounds.Bottom <= panel.ActualHeight + 1 && hudBounds.Top >= 0,
                "The map's real player and build display stays visible within the compact campaign client.");
            Program.Render(owner, (int)owner.ActualWidth, (int)owner.ActualHeight,
                Path.Combine(output, "shanhe-main-window-compact.png"));
            owner.Width = 1440; owner.Height = 920;
            owner.Dispatcher.Invoke(() => { }, DispatcherPriority.ApplicationIdle);
            owner.UpdateLayout();
            Program.Render(owner, (int)owner.ActualWidth, (int)owner.ActualHeight,
                Path.Combine(output, "shanhe-main-window.png"));
            store.FailWrites = true;
            vm.CloseShanheCommand.Execute(null);
            Program.Assert(vm.IsShanheOpen && vm.ShanheStatusText.Contains("未保存"),
                "A failed campaign save keeps the main window's map and retry status available.");
            store.FailWrites = false;
            Program.Assert(TableShortcutChecks.Input(owner, Key.F5) && vm.ShanheStatusText.Contains("已保存"),
                "F5 on the campaign surface must retry the independent map save, including outside a battle.");
            vm.CloseShanheCommand.Execute(null);
            Layout(root);
            Program.Assert(!vm.IsShanheOpen && store.Exists,
                "Returning from the main window's campaign map preserves its independent save.");
            vm.OpenShanheCommand.Execute(null);
            Program.Assert(vm.IsShanheOpen && vm.ContinueShanheCommand.CanExecute(null),
                "Returning from the campaign map keeps the same main-window model alive for the next entry.");
        }
        finally { store.FailWrites = false; owner.Close(); }

        static void CheckPreparationLayout(ShanheCampaignPanel target)
        {
            var copy = (ScrollViewer)target.FindName("ShanhePreparationCopy");
            var actions = (StackPanel)target.FindName("ShanhePreparationActions");
            var overview = (TextBlock)target.FindName("ShanheScenarioOverview");
            var copyBounds = copy.TransformToAncestor(target).TransformBounds(new Rect(copy.RenderSize));
            var actionsBounds = actions.TransformToAncestor(target).TransformBounds(new Rect(actions.RenderSize));
            Program.Assert(copy.ViewportHeight > 0 && overview.ActualHeight <= copy.ViewportHeight &&
                copyBounds.Bottom <= actionsBounds.Top && actions.IsVisible,
                "The entire overview stays visible above the real preparation actions, including the minimum main window.");
        }
    }

    public static void FullRosterSearchPagesAndSelection(string output)
    {
        var store = new MemoryShanheCampaignSaveStore();
        using var vm = Create(new MemorySaveStore(), store);
        var window = new MainWindow(vm);
        var root = (FrameworkElement)window.Content;
        try
        {
            vm.OpenShanheCommand.Execute(null);
            Layout(root);
            var panel = (ShanheCampaignPanel)window.FindName("ShanhePanel");
            ClickNavigation((Button)panel.FindName("ShanhePrepareButton"), root);
            var roster = (ListBox)panel.FindName("ShanheGeneralSelector");
            var search = (TextBox)panel.FindName("ShanheGeneralSearchBox");
            var candidates = vm.ShanheGeneralPage.Select(general => general.Id).ToArray();
            Program.Assert(!vm.IsShanheFreeGeneralSelection && candidates.Length ==
                StandardWushuangContentRegistry.Create().Modes[WushuangMode.ModeId].GeneralCandidateCount &&
                !search.IsVisible && !store.Exists,
                "Normal preparation starts with the single-combat candidate count and does not expose the full roster.");
            AssertVisibleCards();
            Program.Render(root, 1440, 880, Path.Combine(output, "shanhe-candidate-selection.png"));
            var unavailable = vm.ShanheGenerals.First(general => !candidates.Contains(general.Id));
            vm.SelectedShanheGeneral = unavailable;
            Program.Assert(vm.SelectedShanheGeneral != unavailable, "An unpublished general cannot bypass normal selection.");
            ClickNavigation((Button)panel.FindName("ShanheSetupCloseButton"), root);
            ClickNavigation((Button)panel.FindName("ShanhePrepareButton"), root);
            Program.Assert(candidates.SequenceEqual(vm.ShanheGeneralPage.Select(general => general.Id)) && !store.Exists,
                "Cancelling and reopening keeps the same candidates and does not create an expedition.");
            Click((Button)panel.FindName("ShanheToggleGeneralRosterButton"), root);
            Program.Assert(vm.ShanheGenerals.Count > 100 && roster.Items.Count is > 0 and <= 8 &&
                Program.Find<ListBoxItem>(roster).Count() <= 8 && vm.ShanheGeneralCountText.Contains(vm.ShanheGenerals.Count.ToString()),
                "The campaign must offer the complete ordinary roster while realizing only a bounded page of portraits.");
            AssertVisibleCards();
            var firstPage = vm.ShanheGeneralPage.Select(general => general.Id).ToArray();
            var selectedBeforePage = vm.SelectedShanheGeneral;
            Click((Button)panel.FindName("ShanheNextGeneralPageButton"), root);
            Program.Assert(!vm.ShanheGeneralPage.Select(general => general.Id).SequenceEqual(firstPage) &&
                ReferenceEquals(vm.SelectedShanheGeneral, selectedBeforePage) && roster.Items.Count <= 8,
                "Turning the roster page must expose more generals while preserving the chosen expedition general.");
            search.Text = "诸葛亮";
            Layout(root);
            var target = vm.ShanheGeneralPage.Single(general => general.Id == "classic:zhuge-liang");
            roster.SelectedItem = target;
            Layout(root);
            Program.Assert(ReferenceEquals(vm.SelectedShanheGeneral, target) &&
                vm.ShanheGeneralPage.All(general => general.Name.Contains("诸葛亮")) &&
                vm.SelectedShanheGeneralDescription.Contains("观星"),
                "Searching the actual roster must allow selecting a general outside the initial eight and display real skills.");
            Program.Render(root, 1440, 880, Path.Combine(output, "shanhe-full-roster-search.png"));
            search.Text = "观星";
            Layout(root);
            Program.Assert(vm.ShanheGeneralPage.Contains(target), "Campaign search must include registered skill names.");
            search.Text = "OL";
            Layout(root);
            Program.Assert(vm.ShanheGeneralPage.Count > 0 && vm.ShanheGeneralPage.All(general =>
                general.SearchText.Contains("OL", StringComparison.OrdinalIgnoreCase)),
                "Campaign search must match edition metadata as well as names.");
            search.Text = "__missing_shanhe_general__";
            Layout(root);
            Program.Assert(vm.HasNoShanheGeneralResults && roster.Items.Count == 0 &&
                ReferenceEquals(vm.SelectedShanheGeneral, target),
                "An empty search must explain its empty state without silently clearing the selected general.");
            vm.ClearShanheGeneralSearchCommand.Execute(null);
            Layout(root);
            Program.Assert(!vm.HasNoShanheGeneralResults && vm.ShanheGenerals.Contains(target) &&
                ReferenceEquals(vm.SelectedShanheGeneral, target) && !store.Exists,
                "Clearing search must restore the full roster without starting a run or changing its chosen general.");
            Program.Render(root, 1440, 880, Path.Combine(output, "shanhe-full-roster.png"));
            Click((Button)panel.FindName("ShanheStartButton"), root);
            Program.Assert(store.Exists && store.Read().Campaign.Options.PlayerGeneralId == target.Id,
                "Confirming a searched general must create the real campaign with that exact content ID.");

            void AssertVisibleCards()
            {
                foreach (var item in Program.Find<ListBoxItem>(roster))
                {
                    var bounds = item.TransformToAncestor(roster).TransformBounds(new Rect(item.RenderSize));
                    Program.Assert(bounds.Left >= 0 && bounds.Top >= 0 && bounds.Right <= roster.ActualWidth + 1 &&
                        bounds.Bottom <= roster.ActualHeight + 1,
                        "Every candidate and full-roster card must fit entirely inside the selection viewport.");
                }
            }
        }
        finally { window.Content = null; window.DataContext = null; window.Close(); }
    }

    public static void PreparationLocksAndEncounterPreview(string output)
    {
        var store = new MemoryShanheCampaignSaveStore();
        using var vm = Create(new MemorySaveStore(), store);
        using var host = new NativeResponseStatusChecks.NativeHost(vm);
        var window = host.Window;
        var root = host.Root;
        try
        {
            vm.OpenShanheCommand.Execute(null);
            Layout(root);
            var panel = (ShanheCampaignPanel)window.FindName("ShanhePanel");
            Program.Assert(vm.IsShanhePreparation && vm.CanPrepareShanheExpedition,
                "A fresh preparation surface must permit choosing a general and editing a loadout.");
            var rankSelector = (ComboBox)panel.FindName("ShanheRankSelector");
            rankSelector.SetCurrentValue(Selector.SelectedItemProperty, vm.ShanheRanks.Single(option => option.Value == 5));
            Layout(root);
            Program.Assert(vm.ShanheRanks.Count == 5 && vm.SelectedShanheRank.Value == 5 &&
                ((Border)panel.FindName("ShanheSetupOverlay")).Visibility == Visibility.Collapsed &&
                rankSelector.Visibility == Visibility.Visible && rankSelector.ActualHeight > 0 && rankSelector.IsEnabled &&
                vm.SelectedShanheRankDescription.Contains("获得5张") && vm.SelectedShanheRankDescription.Contains("整次征程"),
                $"The actual preparation selector must expose all five ranks and distinguish gaining five cards from refilling a hand. Rank={vm.SelectedShanheRank.Value}, enabled={rankSelector.IsEnabled}, height={rankSelector.ActualHeight}.");
            Program.Render(root, 1440, 880, Path.Combine(output, "shanhe-rank-five-preparation.png"));
            ClickNavigation((Button)panel.FindName("ShanhePrepareButton"), root);
            vm.ToggleShanheGeneralRosterCommand.Execute(null);
            vm.SelectedShanheGeneral = vm.ShanheGenerals.Single(general => general.Id == "classic:qu-yi");
            vm.StartShanheCommand.Execute(null);
            Layout(root);
            var saved = ShanheCampaignJsonText(store);
            Program.Assert(vm.ShanheVitalText.Contains("麹义") && vm.ShanheVitalText.Contains("6/6"),
                "The actual fifth-rank Qu Yi map uses four printed HP plus two rank HP, with no hidden old preparation bonus.");
            Program.Assert(vm.IsShanheRunInProgress && !vm.IsShanhePreparation &&
                !vm.CanPrepareShanheExpedition && !vm.StartShanheCommand.CanExecute(null) &&
                !vm.EquipCodexTacticCommand.CanExecute(null) && !vm.UnlockCodexNodeCommand.CanExecute(null) &&
                !vm.RemoveCodexTacticCommand.CanExecute(null),
                "Beginning an expedition must lock the general, new-run action and all codex edits.");
            vm.SelectedShanheRank = vm.ShanheRanks[0];
            Program.Assert(vm.SelectedShanheRank.Value == 5 && !rankSelector.IsEnabled &&
                store.Read().Campaign.Options.PlayerRank == 5 && ShanheCampaignJsonText(store) == saved &&
                vm.ShanheRankStatusText.Contains("重生可用"),
                "The chosen rank must be frozen into the saved expedition and neither a hidden selector nor a programmatic edit can downgrade it.");
            using (var cold = Create(new MemorySaveStore(), store))
            {
                cold.SelectedShanheRank = cold.ShanheRanks[1];
                cold.ContinueShanheCommand.Execute(null);
                Program.Assert(cold.SelectedShanheRank.Value == 5 && cold.ShanheRankStatusText.Contains("重生可用") &&
                    cold.ShanheVitalText.Contains("6/6") &&
                    ShanheCampaignJsonText(store) == saved,
                    "Continuing restores the saved fifth rank instead of the current preparation preference and leaves its journal untouched.");
            }
            typeof(ButtonBase).GetMethod("OnClick", BindingFlags.Instance | BindingFlags.NonPublic)!
                .Invoke((Button)panel.FindName("ShanhePrepareButton"), null);
            Layout(root);
            Program.Assert(((Border)panel.FindName("ShanheSetupOverlay")).Visibility == Visibility.Collapsed &&
                ShanheCampaignJsonText(store) == saved,
                "A routed click against the hidden preparation entry must not reopen setup during a run.");
            var availableBattles = vm.ShanheMapRows.SelectMany(row => row.Nodes).Where(node => node.IsAvailable).ToArray();
            Program.Assert(availableBattles.Length == 2 && vm.ShanheChoices.Count == 0 && !vm.IsShanheRewardOpen,
                "Without inheritance the actual opening exposes two battle locations with no forced event or initial skill choice.");
            var battleNode = availableBattles[0];
            var game = Program.Engine(vm);
            var journal = ShanheCampaignJsonText(store);
            var campaignSession = (ShanheCampaignSession)typeof(MainViewModel)
                .GetField("_shanheSession", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(vm)!;
            var currentRoute = campaignSession.ActiveChapterRouteId;
            var opening = campaignSession.Snapshot.AvailableNodes;
            var currentChapter = opening[0];
            var routeDefinitions = campaignSession.Definition.Nodes.Where(node => node.ChapterRouteId == currentRoute)
                .ToDictionary(node => node.Id, StringComparer.Ordinal);
            var currentRows = vm.ShanheMapRows.SelectMany(row => row.Nodes).ToArray();
            var rowIds = currentRows.Select(node => node.Id).ToHashSet(StringComparer.Ordinal);
            Program.Assert(currentRoute is not null && opening.Count == 2 &&
                opening.All(node => node.Kind == ShanheNodeKind.Battle && node.ChapterNumber == 1 && node.ChapterRouteId == currentRoute) &&
                vm.ShanheChapterTitle == currentChapter.ChapterTitle &&
                currentRows.Length > 0 && currentRows.All(node => routeDefinitions.ContainsKey(node.Id)) &&
                routeDefinitions.Values.All(node => node.ChapterNumber == currentChapter.ChapterNumber) &&
                vm.ShanheMapNodes.Count > 0 && vm.ShanheMapNodes.All(node => rowIds.Contains(node.Node.Id)) &&
                vm.ShanheMapNodes.Where(node => node.IsAvailable).Select(node => node.Node.Id).ToHashSet(StringComparer.Ordinal)
                    .SetEquals(opening.Select(node => node.Id)),
                "The active chapter title, logical map rows and geographic markers must belong to the actual first route and expose exactly its two opening battles.");
            var previewRecipe = campaignSession.PreviewEncounter(battleNode.Id);
            Program.Assert(campaignSession.CreateCheckpoint().Options.PreferSourceOrdinaryEncounters &&
                    previewRecipe.SourceBattleId is > 0 && previewRecipe.SourceCityId is { } previewCity &&
                    battleNode.Id == StandardShanheCampaignCatalog.Source2026CityNodeId(previewCity),
                "The real new source expedition opts into an original source opening at its matching map city instead of a local fallback roster.");
            var previewDefinition = routeDefinitions[battleNode.Id];
            var originalStory = !string.IsNullOrWhiteSpace(previewRecipe.StoryText) ? previewRecipe.StoryText!
                : previewDefinition.StoryText ?? battleNode.Description;
            var previewCheckpoint = ShanheCampaignJson.Serialize(campaignSession.CreateCheckpoint());
            var ordinaryCheckpoint = GameCheckpointJson.Serialize(game.CreateCheckpoint());
            Click(Bound(panel, vm.PreviewShanheNodeCommand, battleNode), root);
            Program.Assert(vm.IsShanheEncounterPreviewOpen && !vm.IsShanheBattleActive &&
                ReferenceEquals(Program.Engine(vm), game) && ShanheCampaignJsonText(store) == journal &&
                vm.ShanheEncounterEnemyName.Length > 0 && vm.ShanheEncounterRewardText.Contains("铜币"),
                "Previewing an encounter must show real opponents and rewards without committing a move, save or battle.");
            var routeHint = (TextBlock)panel.FindName("ShanheEncounterRouteHint");
            var routeBanner = (Border)panel.FindName("ShanheEncounterRouteBanner");
            var storyText = (TextBlock)panel.FindName("ShanheEncounterStoryText");
            Program.Assert(vm.SelectedShanheScenario.Id == StandardShanheCampaignCatalog.Source2026EditionId &&
                    currentRoute?.StartsWith("source-region-", StringComparison.Ordinal) == true &&
                    previewDefinition.ChapterRole == ShanheChapterNodeRole.OpeningBattle && vm.ShanheChapterProgressTarget == 6 &&
                    routeBanner.Visibility == Visibility.Visible && routeHint.Text == vm.ShanheEncounterRouteHint && routeHint.FontSize >= 14 &&
                    routeHint.Text.StartsWith("主线第1格：", StringComparison.Ordinal) &&
                    routeHint.Text.Contains("另一处保留为可选战斗", StringComparison.Ordinal) &&
                    storyText.Text == originalStory && vm.ShanheEncounterDescription == originalStory &&
                    ShanheCampaignJson.Serialize(campaignSession.CreateCheckpoint()) == previewCheckpoint &&
                    GameCheckpointJson.Serialize(game.CreateCheckpoint()) == ordinaryCheckpoint,
                "The naturally available source opening explains its real first-step/optional-branch role separately from the unchanged authoritative story, without submitting either game's command.");
            ShanheResponsiveMarketUiChecks.AssertTextGlyphsWithin(routeHint, routeBanner, "The actual source-opening route hint");
            AssertEncounterBackdrop(panel, 1);
            for (var index = 0; index < availableBattles.Length; index++)
            {
                var candidate = availableBattles[index];
                if (index != 0)
                {
                    vm.CloseShanheEncounterPreviewCommand.Execute(null); Layout(root);
                    Click(Bound(panel, vm.PreviewShanheNodeCommand, candidate), root);
                }
                var expectedTitle = campaignSession.PreviewEncounter(candidate.Id).Name;
                foreach (var width in new[] { 1120, 1440 })
                {
                    host.Capture(output, "shanhe-encounter-title-opening-" + index, width);
                    AssertEncounterTitleLayout(vm, panel, root, expectedTitle);
                    AssertEncounterRewardSummaryLayout(vm, panel, root);
                    var rewardSummary = vm.ShanheEncounterRewardSummary;
                    var preview = campaignSession.PreviewEncounter(candidate.Id);
                    var category = campaignSession.PreviewRewardCategory(candidate.Id);
                    Program.Assert(rewardSummary[0].Title == "基础铜币" && rewardSummary[0].ValueText == "× " + preview.VictoryGold &&
                            category is not null && rewardSummary.Count == 2 &&
                            rewardSummary[1].ValueText.StartsWith("基础 ", StringComparison.Ordinal) &&
                            rewardSummary[1].ValueText.EndsWith(" 选一", StringComparison.Ordinal) &&
                            rewardSummary[1].Detail.Contains("战后以实际候选为准", StringComparison.Ordinal),
                        "The actual opening exposes base copper and its determined base reward class without claiming already-frozen post-battle offers.");
                    AssertEncounterRewardIcon(rewardSummary[0], "Assets/Shanhe/Encounter/14021801.png");
                    AssertEncounterRewardIcon(rewardSummary[1], "Assets/Wushuang/rogue_award_type_icon_" +
                        (category == ShanheRewardCategory.Tactic ? 2 : category == ShanheRewardCategory.Skill ? 3 : 4) + ".png");
                    Program.Assert(vm.IsShanheEncounterPreviewOpen && !vm.IsShanheBattleActive &&
                            ReferenceEquals(Program.Engine(vm), game) && ShanheCampaignJsonText(store) == journal &&
                            ShanheCampaignJson.Serialize(campaignSession.CreateCheckpoint()) == previewCheckpoint &&
                            GameCheckpointJson.Serialize(game.CreateCheckpoint()) == ordinaryCheckpoint,
                        "Reading each actual opening encounter title at both native widths leaves the campaign, ordinary battle and saved expedition unchanged.");
                }
            }
            Program.Assert(panel.HandleEscape() && !vm.IsShanheEncounterPreviewOpen &&
                vm.ShanheEncounterRewardSummary.Count == 0 &&
                vm.ShanheEncounterRouteHint.Length == 0 && ShanheCampaignJsonText(store) == journal &&
                ShanheCampaignJson.Serialize(campaignSession.CreateCheckpoint()) == previewCheckpoint &&
                GameCheckpointJson.Serialize(game.CreateCheckpoint()) == ordinaryCheckpoint,
                "Escape must dismiss only the uncommitted encounter preview and its route hint, leaving story progress and the ordinary game unchanged.");
            battleNode = vm.ShanheMapRows.SelectMany(row => row.Nodes).First(node => node.IsAvailable);
            Click(Bound(panel, vm.PreviewShanheNodeCommand, battleNode), root);
            Click(Program.Find<Button>(panel).Single(button => ReferenceEquals(button.Command, vm.ChallengeShanheEncounterCommand)), root);
            Program.Assert(!vm.IsShanheEncounterPreviewOpen && vm.IsShanheBattleActive && vm.IsShanheBattleSession &&
                store.Read().Battle is null && Program.Engine(vm).CreateCheckpoint().Options.ShanheBattle is { PlayerGeneralId: "classic:qu-yi", PlayerRank: 5, RebirthAvailable: true,
                    Hp: 6, MaxHpBonus: 2, InitialHandBonus: 2, DrawBonus: 1, SlashBonus: 1 } &&
                Program.Engine(vm).CreateSnapshot(0).Players[0] is { Hp: 6, MaxHp: 6 } &&
                Program.Engine(vm).CreateSnapshot(0).Players[0].Skills!.Count(skill => skill.ContentId == ShanheGeneralRanks.RebirthSkillId) == 1 &&
                !ReferenceEquals(Program.Engine(vm), game),
                "Confirming the actual challenge button must enter and save the real native battle exactly once.");
            var committed = ShanheCampaignJsonText(store);
            vm.ChallengeShanheEncounterCommand.Execute(null);
            Program.Assert(ShanheCampaignJsonText(store) == committed,
                "A repeated challenge confirmation cannot replace or restart the committed encounter.");
        }
        finally { window.Content = null; window.DataContext = null; }
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

    private static void AssertEncounterTitleLayout(MainViewModel model, ShanheCampaignPanel panel,
        FrameworkElement root, string expectedTitle)
    {
        var title = (TextBlock)panel.FindName("ShanheEncounterTitleText");
        var banner = (Border)panel.FindName("ShanheEncounterRouteBanner");
        var story = (ScrollViewer)panel.FindName("ShanheEncounterStoryScroll");
        var challenge = (Button)panel.FindName("ShanheChallengeButton");
        var close = (Button)panel.FindName("ShanheEncounterCloseButton");
        var titleBounds = Bounds(title);
        var storyBounds = Bounds(story);
        Program.Assert(title.IsVisible && PresentationSource.FromVisual(title) is HwndSource &&
                title.Text == expectedTitle && title.Text == model.ShanheEncounterTitle &&
                BindingOperations.GetBinding(title, TextBlock.TextProperty)?.Path.Path == nameof(MainViewModel.ShanheEncounterTitle) &&
                title.TextTrimming == TextTrimming.None && title.TextWrapping == TextWrapping.Wrap && title.FontSize >= 32 &&
                titleBounds.Width > 0 && titleBounds.Height > 0 && new Rect(root.RenderSize).Contains(titleBounds) &&
                storyBounds.Height > 0 && titleBounds.Bottom <= storyBounds.Top + .7,
            "The real encounter title preserves its complete authoritative name and readable type size above the separate story viewport: " +
            JsonSerializer.Serialize(new { title.Text, title.RenderSize, Root = root.RenderSize,
                Title = titleBounds.ToString(), Story = storyBounds.ToString() }));
        if (banner.IsVisible)
        {
            var bannerBounds = Bounds(banner);
            Program.Assert(titleBounds.Bottom <= bannerBounds.Top + .7 && bannerBounds.Bottom <= storyBounds.Top + .7 &&
                    !Overlaps(titleBounds, bannerBounds) && !Overlaps(titleBounds, storyBounds),
                "A wrapping encounter title remains above the independent route banner and scrollable original story without overlap: " +
                JsonSerializer.Serialize(new { Title = titleBounds.ToString(), Banner = bannerBounds.ToString(), Story = storyBounds.ToString() }));
        }
        ShanheResponsiveMarketUiChecks.AssertTextGlyphsWithin(title, title,
            "The complete native encounter title", checkAncestorClips: true);
        foreach (var action in new[] { challenge, close })
        {
            var bounds = Bounds(action);
            var hit = root.InputHitTest(new Point(bounds.Left + bounds.Width / 2, bounds.Top + bounds.Height / 2)) as DependencyObject;
            Program.Assert(action.IsVisible && action.IsEnabled && action.Command?.CanExecute(action.CommandParameter) == true &&
                    bounds.Width > 0 && bounds.Height > 0 && new Rect(root.RenderSize).Contains(bounds) &&
                    !bounds.IntersectsWith(titleBounds) && hit is not null &&
                    (ReferenceEquals(hit, action) || action.IsAncestorOf(hit)),
                "The real encounter challenge and close controls retain separate, reachable native hit targets after the title wraps.");
        }
        Console.WriteLine($"Encounter title: {expectedTitle}; chars={expectedTitle.Length}; title={title.RenderSize}; root={root.RenderSize}.");

        Rect Bounds(FrameworkElement element) => element.TransformToAncestor(root).TransformBounds(new Rect(element.RenderSize));
        static bool Overlaps(Rect first, Rect second)
        {
            var intersection = Rect.Intersect(first, second);
            return !intersection.IsEmpty && intersection.Width > .7 && intersection.Height > .7;
        }
    }

    public static void PublishedFixedNpcCardsAreReadOnlyInEncounterPreview(string output)
    {
        var published = StandardShanheCampaignCatalog.CreateDefinition(1, StandardShanheCampaignCatalog.Returning2026EditionId);
        var guangcheng = published.Nodes.Single(node => node.Id == "guangcheng");
        var fixedEnemies = new[] { StandardShanheS5LiJuePackage.LijueId, StandardShanheS5GuosiPackage.GuosiId, StandardShanheS5ZhangxiuPackage.ZhangxiuId }
            .Select(id => guangcheng.Encounters!.SelectMany(encounter => encounter.Enemies).Single(enemy => enemy.GeneralId == id)).ToArray();
        Program.Assert(fixedEnemies.SelectMany(enemy => enemy.StartingCards ?? []).Select(card => card.Kind)
                .SequenceEqual([CardKind.ScarletBloodSword, CardKind.ZhanhunDao, CardKind.Leizhen]) &&
            fixedEnemies.All(enemy => enemy.StartingHand is null),
            "The actual published Guangcheng candidates retain random opening hands and expose their three additional fixed weapons.");
        foreach (var kind in new[] { CardKind.Leizhen, CardKind.ShanheXingtianAxe })
            Program.Assert(CardArt.Get(kind) is not null && CardArt.GetEquipment(kind) is not null &&
                CardArt.NeedsRuntimeNameOverlay(kind) == (kind == CardKind.Leizhen),
                "Fixed weapons have readable card faces and equipment icons; only a generic artwork template needs a runtime name overlay.");
        // A preview now reveals only this run's concrete encounter. Exercise
        // each authentic fixed-card recipe separately, without seed searches or
        // combining otherwise independent candidate rosters into a fake battle.
        foreach (var fixedEnemy in fixedEnemies)
        {
            var encounter = guangcheng.Encounters!.Single(item => item.Enemies.Contains(fixedEnemy));
            // This isolated fixture tests the published opening-card recipe, not its optional elite upgrade.
            var isolatedNode = guangcheng with { NextNodeIds = [], Encounters = [encounter], EliteUpgradeNodeId = null };
            var session = ShanheCampaignSession.Create(published with { StartingNodeIds = [guangcheng.Id], Nodes = [isolatedNode] },
                new(89, "classic:guan-yu"), StandardShanheCampaignCatalog.CreateBattleRegistry);
            var store = new PreviewTrackingShanheStore();
            using var model = Create(new MemorySaveStore(), store);
            model.SelectedShanheScenario = model.ShanheScenarios.Single(option => option.Id == StandardShanheCampaignCatalog.Returning2026EditionId);
            typeof(MainViewModel).GetField("_shanheSession", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(model, session);
            using var host = new NativeResponseStatusChecks.NativeHost(model);
            var window = host.Window;
            var root = host.Root;
            try
            {
                model.OpenShanheCommand.Execute(null); Layout(root);
                var panel = (ShanheCampaignPanel)window.FindName("ShanhePanel");
                var node = model.ShanheMapRows.SelectMany(row => row.Nodes).Single(candidate => candidate.Id == guangcheng.Id);
                Program.Assert(node.IsAvailable && session.Snapshot.Phase == ShanheCampaignPhase.Map &&
                    session.CreateCheckpoint().Commands.Count == 0 && !store.Exists,
                    "The original fixed-card recipe is available for a read-only preview before any map command or save write.");
                var ordinary = Program.Engine(model);
                var ordinaryCheckpoint = GameCheckpointJson.Serialize(ordinary.CreateCheckpoint());
                var journal = ShanheCampaignJson.Serialize(session.CreateCheckpoint());
                var snapshot = JsonSerializer.Serialize(session.Snapshot);
                var writes = store.WriteCount;
                var revision = session.Revision;
                Click(Bound(panel, model.PreviewShanheNodeCommand, node), root);
                var scroll = (ScrollViewer)panel.FindName("ShanheEncounterEnemyDetailScroll");
                Program.Assert(model.IsShanheEncounterPreviewOpen && scroll.Content is TextBlock detail &&
                    detail.Text == model.ShanheEncounterEnemyDetail && detail.Visibility == Visibility.Visible && detail.ActualHeight > 0 &&
                    detail.Text.Contains(model.ShanheEncounterEnemyPortraitName, StringComparison.Ordinal) &&
                    detail.Text.Contains("额外起手牌（随机起手之外）", StringComparison.Ordinal) &&
                    fixedEnemy.StartingCards!.Select(card => card.Kind).All(kind =>
                        detail.Text.Contains("【" + CardCatalog.Get(kind).DisplayName + "】", StringComparison.Ordinal) &&
                        detail.Text.Contains(CardCatalog.Get(kind).Description, StringComparison.Ordinal)),
                    "The concrete preview control retains its original enemy's complete fixed-card rules as additional hand cards.");
                Program.Assert(session.PendingBattleSetup is null && session.Revision == revision && session.Snapshot.EncounterOrdinal == 0 &&
                    ShanheCampaignJson.Serialize(session.CreateCheckpoint()) == journal && JsonSerializer.Serialize(session.Snapshot) == snapshot &&
                    ReferenceEquals(Program.Engine(model), ordinary) && GameCheckpointJson.Serialize(ordinary.CreateCheckpoint()) == ordinaryCheckpoint &&
                    !model.IsShanheBattleSession && store.WriteCount == writes && !store.Exists,
                    "The public fixed-card preview does not roll an encounter, deal or reveal random opening cards, create a battle, change either journal or write a save.");
                scroll.ScrollToBottom(); Layout(root);
                Program.Assert(Math.Abs(scroll.VerticalOffset - scroll.ScrollableHeight) < 1,
                    "The actual scroll surface reaches the complete extra-card rules at the end of the published candidate roster.");
                foreach (var width in new[] { 1120, 1440 })
                {
                    host.Capture(output, "shanhe-public-fixed-card-preview-" + fixedEnemy.GeneralId.Split(':')[^1], width);
                    AssertEncounterTitleLayout(model, panel, root, encounter.Name);
                    Program.Assert(session.Revision == revision && ShanheCampaignJson.Serialize(session.CreateCheckpoint()) == journal &&
                            JsonSerializer.Serialize(session.Snapshot) == snapshot && ReferenceEquals(Program.Engine(model), ordinary) &&
                            GameCheckpointJson.Serialize(ordinary.CreateCheckpoint()) == ordinaryCheckpoint &&
                            store.WriteCount == writes && !store.Exists,
                        "Reading the shorter unmodified published encounter names at both native widths preserves both journals and the unwritten save.");
                }
                Click((Button)panel.FindName("ShanheEncounterCloseButton"), root);
                Program.Assert(!model.IsShanheEncounterPreviewOpen && session.PendingBattleSetup is null &&
                    ShanheCampaignJson.Serialize(session.CreateCheckpoint()) == journal && store.WriteCount == writes && !store.Exists,
                    "Closing the actual fixed-card preview preserves the unissued encounter and read-only save boundary.");
            }
            finally { window.Content = null; window.DataContext = null; }
        }
    }

    public static void EightSeatBossStagesAndFinalSettlementPreview(string output)
    {
        var full = StandardShanheCampaignCatalog.CreateDefinition(2, StandardShanheCampaignCatalog.Returning2026EditionId);
        VerifyPublishedChapterBossRewardSummary(full, output);
        var final = full.Nodes.Single(node => node.Id == "luoyang");
        var encounter = final.Encounters!.Single(item => item.Id == "luoyang:chapter-support");
        var nodeDefinition = final with { Encounters = [encounter] };
        var definition = full with { Nodes = [nodeDefinition], StartingNodeIds = [final.Id] };
        var session = ShanheCampaignSession.Create(definition, new(83, "classic:guan-yu"), StandardShanheCampaignCatalog.CreateBattleRegistry);
        var store = new PreviewTrackingShanheStore();
        using var model = Create(new MemorySaveStore(), store);
        model.SelectedShanheScenario = model.ShanheScenarios.Single(option => option.Id == StandardShanheCampaignCatalog.Returning2026EditionId);
        typeof(MainViewModel).GetField("_shanheSession", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(model, session);
        var host = new NativeResponseStatusChecks.NativeHost(model);
        var window = host.Window;
        try
        {
            var root = (FrameworkElement)window.Content;
            root.Width = 1440; root.Height = 880; window.SizeToContent = SizeToContent.WidthAndHeight;
            model.OpenShanheCommand.Execute(null); Layout(root);
            var panel = (ShanheCampaignPanel)window.FindName("ShanhePanel");
            var node = model.ShanheMapRows.SelectMany(row => row.Nodes).Single(item => item.Id == final.Id);
            var ordinary = Program.Engine(model);
            var ordinaryJournal = GameCheckpointJson.Serialize(ordinary.CreateCheckpoint());
            var journal = ShanheCampaignJson.Serialize(session.CreateCheckpoint());
            var snapshot = JsonSerializer.Serialize(session.Snapshot);
            Click(Bound(panel, model.PreviewShanheNodeCommand, node), root);
            var detail = model.ShanheEncounterEnemyDetail;
            Program.Assert(model.IsShanheEncounterPreviewOpen && detail.Contains("8人阵容：敌军 6 · 同行 1 · 玩家 1") &&
                detail.Contains("郭嘉") && detail.Contains("华佗") && detail.Contains("体力 19/19") &&
                detail.Contains("二阶段：") && detail.Contains("体力 28/28") && detail.Contains("补牌 3") &&
                detail.Contains("第一阶段血量打空后同席转换") && detail.Contains("首领开始新的自然回合") &&
                Program.Find<TextBlock>(panel).Any(text => text.Text == detail && text.ActualHeight > 0),
                "The actual preview presents the complete eight-seat support formation, grown starting HP and lethal-only second stage on its bound control.");
            Program.Assert(model.ShanheEncounterRewardText.Contains("最终结算") && model.ShanheEncounterRewardText.Contains("图册") &&
                !model.ShanheEncounterRewardText.Contains("战利品") && !model.ShanheEncounterRewardText.Contains("技能槽") &&
                !model.ShanheEncounterRewardText.Contains("放弃"),
                "The final chapter advertises run settlement and books without advertising another construction reward or skip payment.");
            var finalSummary = model.ShanheEncounterRewardSummary;
            Program.Assert(finalSummary.Count == 2 && finalSummary[0].Title == "基础铜币" &&
                    finalSummary[0].ValueText == "× " + encounter.VictoryGold &&
                    finalSummary[1] is { Title: "最终结算", ValueText: "通关后结算", Icon: null } &&
                    finalSummary.All(item => !item.Title.Contains("战利品", StringComparison.Ordinal) && !item.Title.Contains("技能槽", StringComparison.Ordinal)),
                "The final encounter's compact preview adds only base copper and run settlement, with no extra construction group, skill-slot grant or invented book quantity.");
            AssertEncounterRewardIcon(finalSummary[0], "Assets/Shanhe/Encounter/14021801.png");
            Program.Assert(ShanheCampaignJson.Serialize(session.CreateCheckpoint()) == journal &&
                JsonSerializer.Serialize(session.Snapshot) == snapshot && session.PendingBattleSetup is null &&
                ReferenceEquals(Program.Engine(model), ordinary) && GameCheckpointJson.Serialize(ordinary.CreateCheckpoint()) == ordinaryJournal &&
                store.WriteCount == 0,
                "Reading a future boss stage must not consume encounter randomness, issue a battle command or write either save.");
            AssertEncounterBackdrop(panel, 2);
            var cards = (ItemsControl)panel.FindName("ShanheEncounterEnemyCards");
            var enemyScroll = (ScrollViewer)panel.FindName("ShanheEncounterEnemyScroll");
            var names = ExpectedEncounterEnemyNames(session, encounter);
            Program.Assert(model.ShanheEncounterEnemyCards.All(card => card.HasPortrait && card.Portrait is ImageBrush
                    { ImageSource: BitmapSource { PixelWidth: > 0, PixelHeight: > 0 } }) &&
                Program.Find<TextBlock>(cards).Count(text => text.Name == "EncounterPortraitFallback" && text.Visibility == Visibility.Collapsed) == 6,
                "The real six-seat preview decodes Dong Min and all three troop roles into original card art and removes their old letter placeholders.");
            Program.Assert(encounter.Enemies.Count == 6 && model.ShanheEncounterEnemyCards.Count == encounter.Enemies.Count &&
                cards.Items.Count == encounter.Enemies.Count && model.ShanheEncounterEnemyCards.Select(card => card.Name).SequenceEqual(names) &&
                enemyScroll.HorizontalScrollBarVisibility == ScrollBarVisibility.Auto &&
                enemyScroll.VerticalScrollBarVisibility == ScrollBarVisibility.Auto,
                "The six actual enemy seats retain their published order in the bound portrait cards and allow scrolling in either overflowing direction.");
            AssertEncounterEnemyVitals(session, encounter, model.ShanheEncounterEnemyCards);
            Program.Render(root, 1440, 880, Path.Combine(output, "shanhe-eight-seat-stage-preview.png"));
            var scroll = (ScrollViewer)panel.FindName("ShanheEncounterEnemyDetailScroll");
            scroll.ScrollToBottom(); Layout(root);
            Program.Render(root, 1440, 880, Path.Combine(output, "shanhe-eight-seat-stage-preview-bottom.png"));
            foreach (var size in new[] { new Size(1440, 880), new Size(1120, 780) })
            {
                ArrangePreview(size);
                AssertEncounterRewardSummaryLayout(model, panel, root);
                Program.Assert(ShanheCampaignJson.Serialize(session.CreateCheckpoint()) == journal &&
                        JsonSerializer.Serialize(session.Snapshot) == snapshot && store.WriteCount == 0 && !store.Exists &&
                        ReferenceEquals(Program.Engine(model), ordinary) && GameCheckpointJson.Serialize(ordinary.CreateCheckpoint()) == ordinaryJournal,
                    "Reading all compact final rewards, their explanations and complete rules at both actual widths writes no save or campaign/ordinary-game command.");
                Program.Assert(enemyScroll.ScrollableWidth > 0,
                    "Six full-size published enemy cards must expose the horizontal scrolling needed to reach the final enemy.");
                for (var index = 0; index < cards.Items.Count; index++)
                {
                    var card = model.ShanheEncounterEnemyCards[index];
                    var container = (FrameworkElement)cards.ItemContainerGenerator.ContainerFromIndex(index);
                    Program.Assert(ReferenceEquals(container.DataContext, card),
                        "Each real enemy seat owns a generated portrait container, including the sixth enemy.");
                    var portrait = AssertEncounterPortraitPresentation(container, card);
                    portrait.BringIntoView(); ArrangePreview(size);
                    AssertInEnemyViewport(portrait,
                        "Every actual enemy's complete portrait, vertical name and health rail remain reachable at both preview sizes.");
                    if (card.Skills.Count == 0) continue;
                    var lastSkill = card.Skills[^1];
                    var chip = Program.Find<Border>(container).Single(border =>
                        ReferenceEquals(border.DataContext, lastSkill) && border.ToolTip is ToolTip);
                    chip.BringIntoView(); ArrangePreview(size);
                    AssertInEnemyViewport(chip, "Every enemy's last actual skill remains reachable in the portrait scroll viewport.");
                    if (index == cards.Items.Count - 1 && size.Width == 1120)
                    {
                        var skin = Program.Find<ShanheHudSkin>(chip).Single(element => element.Name == "EncounterSkillSkin");
                        host.FocusKeyboard();
                        Program.Assert(((Button)panel.FindName("ShanheChallengeButton")).Focus(),
                            "The actual challenge button must receive focus before checking the unfocused final skill frame.");
                        ArrangePreview(size);
                        AssertSkillSkin(skin, "normal");
                        Program.Assert(chip.Focus() && ReferenceEquals(Keyboard.FocusedElement, chip),
                            "The sixth enemy's final actual skill chip must receive keyboard focus in the loaded native preview.");
                        ArrangePreview(size);
                        AssertSkillSkin(skin, "over");
                    }
                    var tooltip = (ToolTip)chip.ToolTip;
                    tooltip.PlacementTarget = chip;
                    tooltip.Placement = PlacementMode.AbsolutePoint; tooltip.HorizontalOffset = -30000; tooltip.VerticalOffset = -30000;
                    tooltip.Opened += ConcealEncounterTooltip;
                    try
                    {
                        tooltip.IsOpen = true; ArrangePreview(size);
                        Program.Assert(tooltip.IsOpen && tooltip.Content is StackPanel contents &&
                            Program.Find<TextBlock>(contents).Any(text => text.Text == lastSkill.Description),
                            "The reachable skill chip opens its complete published rule tooltip.");
                        var visibleText = Program.Find<TextBlock>(tooltip).Where(text =>
                            text.IsVisible && text.ActualWidth > 0 && text.ActualHeight > 0).ToArray();
                        Program.Assert(tooltip.ContentTemplate is null && PresentationSource.FromVisual(tooltip) is HwndSource &&
                            visibleText.Any(text => text.Text == lastSkill.Name) &&
                            visibleText.Any(text => text.Text == lastSkill.Description) &&
                            visibleText.All(text => !text.Text.StartsWith("System.Windows.Controls.", StringComparison.Ordinal)),
                            "The opened native tooltip visually renders the real skill name and complete description instead of a StackPanel type name.");
                        if (index == cards.Items.Count - 1 && size.Width == 1120)
                            Program.Render(tooltip, (int)Math.Ceiling(tooltip.ActualWidth), (int)Math.Ceiling(tooltip.ActualHeight),
                                Path.Combine(output, "shanhe-encounter-skill-tooltip.png"));
                    }
                    finally { tooltip.IsOpen = false; tooltip.Opened -= ConcealEncounterTooltip; }
                }
                if (size.Width == 1120)
                {
                    AssertScrollInteractions(enemyScroll, Orientation.Horizontal, size);
                    AssertScrollInteractions(scroll, Orientation.Vertical, size);
                    Program.Assert(model.IsShanheEncounterPreviewOpen &&
                        ShanheCampaignJson.Serialize(session.CreateCheckpoint()) == journal &&
                        JsonSerializer.Serialize(session.Snapshot) == snapshot && session.PendingBattleSetup is null &&
                        ReferenceEquals(Program.Engine(model), ordinary) &&
                        GameCheckpointJson.Serialize(ordinary.CreateCheckpoint()) == ordinaryJournal && store.WriteCount == 0,
                        "Native paging, dragging, arrows, keyboard, wheel and skill focus change only the preview's reading position, never either game's state or save.");
                }
                enemyScroll.ScrollToRightEnd(); enemyScroll.ScrollToBottom(); ArrangePreview(size);
                AssertInEnemyViewport((FrameworkElement)cards.ItemContainerGenerator.ContainerFromIndex(5),
                    "Scrolling to the end must reveal the complete sixth enemy container at both supported preview sizes.");
                foreach (var name in new[] { "ShanheEncounterCloseButton", "ShanheChallengeButton" })
                {
                    var button = (Button)panel.FindName(name);
                    var bounds = button.TransformToAncestor(root).TransformBounds(new Rect(button.RenderSize));
                    var scrollBounds = enemyScroll.TransformToAncestor(root).TransformBounds(new Rect(enemyScroll.RenderSize));
                    var center = new Point(bounds.Left + bounds.Width / 2, bounds.Top + bounds.Height / 2);
                    var hit = root.InputHitTest(center) as DependencyObject;
                    while (hit is not null && !ReferenceEquals(hit, button)) hit = VisualTreeHelper.GetParent(hit);
                    Program.Assert(button.IsVisible && button.IsEnabled && button.Command?.CanExecute(button.CommandParameter) == true &&
                        bounds.Width > 0 && bounds.Height > 0 && bounds.Left >= 0 && bounds.Top >= 0 &&
                        bounds.Right <= size.Width && bounds.Bottom <= size.Height && !enemyScroll.IsAncestorOf(button) &&
                        !bounds.IntersectsWith(scrollBounds) && ReferenceEquals(hit, button),
                        "The close and challenge buttons stay fully visible and receive pointer hits outside the scrolled enemy surface, including at 1120 by 780.");
                }
            }
            Program.Render(root, 1120, 780, Path.Combine(output, "shanhe-eight-seat-stage-preview-1120.png"));
            Click((Button)panel.FindName("ShanheEncounterCloseButton"), root);
            Program.Assert(!model.IsShanheEncounterPreviewOpen && model.ShanheEncounterRewardSummary.Count == 0 &&
                ShanheCampaignJson.Serialize(session.CreateCheckpoint()) == journal &&
                JsonSerializer.Serialize(session.Snapshot) == snapshot && store.WriteCount == 0 && session.PendingBattleSetup is null,
                "Closing the fully scrolled narrow preview leaves its actual encounter and save untouched.");

            Program.Assert(session.Submit(new MoveShanheNodeCommand(final.Id, session.Revision)).Accepted,
                "The published support recipe enters through the real campaign command.");
            var setup = session.PendingBattleSetup!;
            var battle = StandardShanheCampaignCatalog.CreateBattle(setup);
            Program.Assert(setup.BossTransition == encounter.BossTransition && battle.Submit(new StartGameCommand()).Accepted && battle.PlayerCount == 8,
                "The same previewed recipe starts all eight actual seats with its reserved second form.");
            typeof(MainViewModel).GetField("_shanheTableGame", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(model, battle);
            typeof(MainViewModel).GetField("_shanheTableSetup", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(model, setup);
            typeof(MainViewModel).GetMethod("ReplaceEngine", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(model, [battle]);
            model.IsNewGameSetupOpen = false;
            model.CloseShanheCommand.Execute(null);
            ArrangePreview(new Size(1440, 880));
            Program.Render(root, 1440, 880, Path.Combine(output, "shanhe-eight-seat-support-table.png"));
            AssertUniqueOpponentRings(window, model, 8);
            Program.Assert(model.Seats.Count(seat => seat.IsTeammate) == 1 &&
                model.Seats.Count(seat => seat.GeneralId is "classic:guo-jia" or "classic:hua-tuo") == 2 &&
                model.Seats.Where(seat => seat.GeneralId.StartsWith("shanhe:local-", StringComparison.Ordinal)).Count() == 3,
                "The native seat templates retain the companion, both support generals and every locally authored soldier.");

            void ArrangePreview(Size size)
            {
                root.Width = size.Width; root.Height = size.Height; window.SizeToContent = SizeToContent.WidthAndHeight;
                root.Measure(size); root.Arrange(new Rect(size)); root.UpdateLayout();
                root.Dispatcher.Invoke(() => { }, DispatcherPriority.ContextIdle); root.UpdateLayout();
            }

            void AssertScrollInteractions(ScrollViewer target, Orientation orientation, Size size)
            {
                var horizontal = orientation == Orientation.Horizontal;
                Reset();
                var bar = Program.Find<ScrollBar>(target).Single(element => element.Orientation == orientation && element.IsVisible);
                var track = (Track)bar.Template.FindName("PART_Track", bar);
                var pageBack = horizontal ? ScrollBar.PageLeftCommand : ScrollBar.PageUpCommand;
                var pageForward = horizontal ? ScrollBar.PageRightCommand : ScrollBar.PageDownCommand;
                var lineBack = horizontal ? ScrollBar.LineLeftCommand : ScrollBar.LineUpCommand;
                var lineForward = horizontal ? ScrollBar.LineRightCommand : ScrollBar.LineDownCommand;
                Program.Assert((horizontal ? target.ScrollableWidth : target.ScrollableHeight) > 0 &&
                    track.Orientation == orientation && track.Thumb is { IsVisible: true, ActualWidth: > 0, ActualHeight: > 0 } &&
                    ReferenceEquals(track.DecreaseRepeatButton?.Command, pageBack) &&
                    ReferenceEquals(track.IncreaseRepeatButton?.Command, pageForward),
                    "Each overflowing encounter axis keeps a real native track, draggable thumb and correctly directed page buttons.");
                Invoke(track.IncreaseRepeatButton!);
                var afterPage = Offset();
                Program.Assert(afterPage > 0, "The styled encounter track pages forward through actual overflowing content.");
                Invoke(track.DecreaseRepeatButton!);
                Program.Assert(Offset() < afterPage, "The styled encounter track also pages backward through the same content.");

                var arrows = Program.Find<RepeatButton>(bar).ToArray();
                var forward = arrows.Single(button => ReferenceEquals(button.Command, lineForward));
                var backward = arrows.Single(button => ReferenceEquals(button.Command, lineBack));
                Reset(); Invoke(forward);
                var afterArrow = Offset();
                Program.Assert(afterArrow > 0, "The original-art encounter arrow retains its native forward line command.");
                Invoke(backward);
                Program.Assert(Offset() < afterArrow, "The original-art opposite arrow retains its native backward line command.");

                Reset();
                track.Thumb!.RaiseEvent(new DragStartedEventArgs(0, 0) { RoutedEvent = Thumb.DragStartedEvent });
                track.Thumb.RaiseEvent(new DragDeltaEventArgs(horizontal ? 30 : 0, horizontal ? 0 : 30)
                    { RoutedEvent = Thumb.DragDeltaEvent });
                track.Thumb.RaiseEvent(new DragCompletedEventArgs(horizontal ? 30 : 0, horizontal ? 0 : 30, false)
                    { RoutedEvent = Thumb.DragCompletedEvent });
                ArrangePreview(size);
                Program.Assert(Offset() > 0, "Dragging the encounter thumb moves the actual corresponding scroll offset.");

                Reset();
                host.FocusKeyboard();
                Program.Assert(target.Focus() && ReferenceEquals(Keyboard.FocusedElement, target),
                    "The real encounter ScrollViewer must own keyboard navigation in its hidden loaded native host.");
                TableShortcutChecks.WithModifiers(ModifierKeys.None, () =>
                {
                    var input = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromVisual(target)!,
                        Environment.TickCount, horizontal ? Key.Right : Key.Down) { RoutedEvent = Keyboard.PreviewKeyDownEvent };
                    target.RaiseEvent(input);
                    Program.Assert(!input.Handled, "Preview navigation keys must pass through the owner window to the focused native ScrollViewer.");
                    input.RoutedEvent = Keyboard.KeyDownEvent; target.RaiseEvent(input);
                    Program.Assert(input.Handled, "The focused native ScrollViewer must handle its own axis navigation key.");
                    return true;
                });
                ArrangePreview(size);
                Program.Assert(Offset() > 0, "Native keyboard navigation moves the styled encounter scroll offset.");
                if (!horizontal)
                {
                    Reset();
                    var wheel = new MouseWheelEventArgs(Mouse.PrimaryDevice, Environment.TickCount, -120)
                        { RoutedEvent = Mouse.PreviewMouseWheelEvent };
                    target.RaiseEvent(wheel);
                    if (!wheel.Handled) { wheel.RoutedEvent = Mouse.MouseWheelEvent; target.RaiseEvent(wheel); }
                    ArrangePreview(size);
                    Program.Assert(wheel.Handled && Offset() > 0,
                        "A real routed mouse-wheel event continues scrolling the complete encounter roster vertically.");
                }

                double Offset() => horizontal ? target.HorizontalOffset : target.VerticalOffset;
                void Reset()
                {
                    if (horizontal) target.ScrollToHorizontalOffset(0); else target.ScrollToVerticalOffset(0);
                    ArrangePreview(size);
                }
                void Invoke(RepeatButton button)
                {
                    Program.Assert(button.IsEnabled && button.Command is RoutedCommand command &&
                        command.CanExecute(button.CommandParameter, button.CommandTarget ?? button),
                        "A real styled scrollbar repeat button must retain an enabled routed command.");
                    typeof(ButtonBase).GetMethod("OnClick", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(button, null);
                    ArrangePreview(size);
                }
            }

            void AssertSkillSkin(ShanheHudSkin skin, string state)
            {
                var expected = CardGame.Wpf.Resources.AssetImages.Load($"Assets/Shanhe/Encounter/win_noborder_btn_{state}.png");
                Program.Assert(skin.Source is BitmapSource actual && expected is not null &&
                    actual.PixelWidth == expected.PixelWidth && actual.PixelHeight == expected.PixelHeight,
                    "The final real skill uses the original decoded encounter button artwork for its actual keyboard-focus state.");
                var bitmap = (BitmapSource)skin.Source!;
                var stride = bitmap.PixelWidth * 4;
                var pixels = new byte[stride * bitmap.PixelHeight]; var expectedPixels = new byte[pixels.Length];
                new FormatConvertedBitmap(bitmap, PixelFormats.Bgra32, null, 0).CopyPixels(pixels, stride, 0);
                new FormatConvertedBitmap(expected!, PixelFormats.Bgra32, null, 0).CopyPixels(expectedPixels, stride, 0);
                Program.Assert(pixels.AsSpan().SequenceEqual(expectedPixels),
                    "Keyboard focus switches the actual sixth enemy's final skill from the original normal PNG to the original over PNG.");
            }

            void AssertInEnemyViewport(FrameworkElement element, string message)
            {
                var presenter = Program.Find<ScrollContentPresenter>(enemyScroll).Single();
                var bounds = element.TransformToAncestor(presenter).TransformBounds(new Rect(element.RenderSize));
                Program.Assert(bounds.Width > 0 && bounds.Height > 0 && bounds.Left >= -.5 && bounds.Top >= -.5 &&
                    bounds.Right <= presenter.ActualWidth + .5 && bounds.Bottom <= presenter.ActualHeight + .5,
                    message + $" Element={bounds}; viewport={presenter.RenderSize}.");
            }
        }
        finally { host.Dispose(); }

        // This genuine six-enemy recipe repeats two soldier types. Do not replace
        // it with a combined candidate roster or duplicate the save-boundary fixture.
        var warband = final.Encounters!.Single(item => item.Id == "luoyang:chapter-warband");
        var repeatedDefinition = full with
        {
            Nodes = [final with { Encounters = [warband] }], StartingNodeIds = [final.Id]
        };
        var repeatedSession = ShanheCampaignSession.Create(repeatedDefinition, new(83, "classic:guan-yu"),
            StandardShanheCampaignCatalog.CreateBattleRegistry);
        using var repeated = Create(new MemorySaveStore(), new MemoryShanheCampaignSaveStore());
        repeated.SelectedShanheScenario = repeated.ShanheScenarios.Single(option => option.Id == StandardShanheCampaignCatalog.Returning2026EditionId);
        typeof(MainViewModel).GetField("_shanheSession", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(repeated, repeatedSession);
        repeated.OpenShanheCommand.Execute(null);
        repeated.PreviewShanheNodeCommand.Execute(repeated.ShanheMapRows.SelectMany(row => row.Nodes).Single(item => item.Id == final.Id));
        Program.Assert(warband.Enemies.Count == 6 && warband.Enemies.Select(enemy => enemy.GeneralId).Distinct().Count() < warband.Enemies.Count &&
            repeated.ShanheEncounterEnemyCards.Count == warband.Enemies.Count &&
            repeated.ShanheEncounterEnemyCards.Select(card => card.Name).SequenceEqual(ExpectedEncounterEnemyNames(repeatedSession, warband)),
            "The published six-enemy warband keeps repeated soldier seats and their actual order instead of deduplicating its portrait cards.");
        Program.Assert(repeated.ShanheEncounterMainEnemyCards.Count == 6 && repeated.ShanheEncounterMinorEnemyCards.Count == 0 &&
            !repeated.HasShanheEncounterMinorEnemies && repeated.ShanheEncounterEnemyCards.All(card => card.IsCommander is null),
            "The locally authored repeated warband has no proven source commander flags and keeps all six full-size readable cards.");
        AssertEncounterEnemyVitals(repeatedSession, warband, repeated.ShanheEncounterEnemyCards);

        // These are complete source recipes, not commander roles inferred from
        // a local enemy's name, position, difficulty or the battle-node kind.
        VerifySourceGrouping(2634, 1, commanderIndex: 1, backdrop: 2);
        VerifySourceGrouping(2499, 16, commanderIndex: 0, backdrop: 1);
        VerifySourceGrouping(2685, 16, commanderIndex: 0, backdrop: 2);
        VerifySourceGrouping(2686, 16, commanderIndex: 0, backdrop: 2);
        VerifySourceGrouping(2686, 16, commanderIndex: 0, backdrop: 2, width: 1440);
        VerifySourceGrouping(2642, 16, commanderIndex: 0, backdrop: 2);
        VerifySourceGrouping(2650, 1, commanderIndex: 1, backdrop: 2);
        VerifySourceGrouping(2661, 1, commanderIndex: 0, backdrop: 2);
        VerifySourceGrouping(2674, 11, commanderIndex: 1, backdrop: 2);
        VerifySourceGrouping(2679, 11, commanderIndex: 1, backdrop: 2);
        VerifySourceGrouping(2683, 11, commanderIndex: 1, backdrop: 2, width: 1440);
        VerifySourceGrouping(2633, 1, commanderIndex: 0, backdrop: 2);
        VerifySourceGrouping(2656, 11, commanderIndex: 0, backdrop: 2);
        VerifySourceGrouping(2657, 11, commanderIndex: 0, backdrop: 2);
        VerifySourceGrouping(2654, 6, commanderIndex: 1, backdrop: 2);
        VerifySourceGrouping(2662, 1, commanderIndex: 0, backdrop: 2);
        VerifySourceGrouping(2663, 1, commanderIndex: 1, backdrop: 2);
        VerifySourceGrouping(2455, 25, commanderIndex: -1, backdrop: 1);
        VerifySourceGrouping(2459, 25, commanderIndex: -1, backdrop: 1);
        VerifySourceGrouping(2456, 25, commanderIndex: -1, backdrop: 1);
        VerifySourceGrouping(2465, 25, commanderIndex: -1, backdrop: 1, width: 1440);
        VerifySourceGrouping(2464, 25, commanderIndex: -1, backdrop: 1, width: 1440);
        VerifySourceGrouping(2453, 25, commanderIndex: -1, backdrop: 1);
        VerifySourceGrouping(2494, 25, commanderIndex: -1, backdrop: 1);
        VerifySourceGrouping(2491, 25, commanderIndex: -1, backdrop: 1, width: 1440);
        VerifySourceGrouping(2472, 25, commanderIndex: -1, backdrop: 1, width: 1440);
        VerifySourceGrouping(2450, 25, commanderIndex: 1, backdrop: 1);
        VerifySourceGrouping(2475, 25, commanderIndex: 1, backdrop: 1);
        VerifySourceGrouping(2485, 25, commanderIndex: 0, backdrop: 1);
        VerifySourceGrouping(2463, 25, commanderIndex: 1, backdrop: 1);
        VerifySourceGrouping(2471, 25, commanderIndex: -1, backdrop: 1);
        VerifySourceGrouping(2479, 25, commanderIndex: -1, backdrop: 1);
        VerifySourceGrouping(2489, 25, commanderIndex: -1, backdrop: 1);
        VerifySourceGrouping(2684, 25, commanderIndex: 0, backdrop: 2);
        foreach (var difficulty in new[] { 6, 11, 16, 25 })
            VerifySourceGrouping(2653, difficulty, commanderIndex: 0, backdrop: 2);
        foreach (var tier in new[] { 10, 11, 15, 16, 20, 21, 25 })
        {
            var fixture = SourcePreview(2499, tier);
            using var adapter = fixture.Model;
            var officer = tier <= 10 ? StandardShanheSourceYulinOfficerPackage.BaseId :
                tier <= 20 ? StandardShanheSourceYulinOfficerPackage.HardId : StandardShanheSourceYulinOfficerPackage.HellId;
            var guard = tier <= 10 ? StandardShanheSourceYulinGuardPackage.BaseId :
                tier <= 15 ? StandardShanheSourceYulinGuardPackage.HardId : StandardShanheSourceYulinGuardPackage.HellId;
            Program.Assert(fixture.Encounter.Enemies.Select(enemy => enemy.GeneralId).SequenceEqual(new[] { officer, guard }) &&
                adapter.ShanheEncounterEnemyCards.Select(card => card.SeatOrdinal).SequenceEqual(new[] { 1, 2 }) &&
                adapter.ShanheEncounterEnemyCards.Select(card => card.IsCommander).SequenceEqual(new bool?[] { true, false }) &&
                adapter.ShanheEncounterMainEnemyCards.Count == 1 && adapter.ShanheEncounterMinorEnemyCards.Count == 1 &&
                ReferenceEquals(adapter.ShanheEncounterMainEnemyCards[0], adapter.ShanheEncounterEnemyCards[0]) &&
                ReferenceEquals(adapter.ShanheEncounterMinorEnemyCards[0], adapter.ShanheEncounterEnemyCards[1]),
                $"Source2499/{tier} matches the exact complete officer/guard identity pair, including their different hard and hell thresholds.");
            var officerSkills = new List<string> { "恇守" };
            var guardSkills = new List<string> { "据守" };
            if (tier >= 11) { officerSkills.Add("恪守"); guardSkills.Add("恪守"); }
            if (tier >= 16) guardSkills.Add("穷守");
            if (tier >= 21) officerSkills.Add("卫境");
            Program.Assert(adapter.ShanheEncounterEnemyCards[0].Skills.Select(skill => skill.Name).SequenceEqual(officerSkills) &&
                adapter.ShanheEncounterEnemyCards[1].Skills.Select(skill => skill.Name).SequenceEqual(guardSkills) &&
                fixture.Session.Revision == 0 && fixture.Session.PendingBattleSetup is null && fixture.Store.WriteCount == 0,
                $"Source2499/{tier} previews the independently upgraded published skills without issuing a campaign or save command.");
        }
        foreach (var fallback in new[] { (Battle: 2634, Tier: 1, Count: 2), (Battle: 2686, Tier: 16, Count: 3), (Battle: 2657, Tier: 11, Count: 2) })
        foreach (var mismatch in new[] { "unknown-source", "reversed-source-order" })
        {
            var fixture = SourcePreview(fallback.Battle, fallback.Tier, encounter => mismatch == "unknown-source"
                ? encounter with { SourceBattleId = 987654321 }
                : encounter with { Enemies = encounter.Enemies.Reverse().ToArray() });
            using var adapter = fixture.Model;
            Program.Assert(adapter.ShanheEncounterEnemyCards.Count == fallback.Count &&
                adapter.ShanheEncounterEnemyCards.Select(card => card.Name).SequenceEqual(ExpectedEncounterEnemyNames(fixture.Session, fixture.Encounter)) &&
                adapter.ShanheEncounterEnemyCards.All(card => card.IsCommander is null && card.IdentityArtwork is null) &&
                adapter.ShanheEncounterMainEnemyCards.Count == fallback.Count && adapter.ShanheEncounterMinorEnemyCards.Count == 0 &&
                !adapter.HasShanheEncounterMinorEnemies &&
                adapter.ShanheEncounterMainEnemyCards.Zip(adapter.ShanheEncounterEnemyCards).All(pair => ReferenceEquals(pair.First, pair.Second)) &&
                fixture.Session.Revision == 0 && fixture.Session.PendingBattleSetup is null && fixture.Store.WriteCount == 0,
                $"The genuine source{fallback.Battle} roster with {mismatch} keeps every actual seat as a main card and does not invent source commander roles.");
        }
        foreach (var partial in new[]
        {
            (Battle: 2635, GeneralId: StandardShanheSourceXuShengPackage.MediumGeneralId),
            (Battle: 2655, GeneralId: StandardShanheSourceSunCePackage.SunCeId),
            (Battle: 2669, GeneralId: StandardShanheSourceQiangWangPackage.GeneralId),
            (Battle: 2675, GeneralId: StandardShanheSourceXunYuPackage.GeneralId)
        })
        {
            var fixture = SourcePreview(partial.Battle, 25);
            using var adapter = fixture.Model;
            var actual = fixture.Encounter;
            var cards = adapter.ShanheEncounterEnemyCards;
            AssertEncounterEnemyVitals(fixture.Session, actual, cards);
            Program.Assert(actual.SourceBattleId == partial.Battle && actual.SourceVisibleEnemyCount == 1 && actual.Enemies.Count == 1 &&
                actual.Enemies[0].GeneralId == partial.GeneralId && cards.Count == actual.Enemies.Count &&
                cards.Select(card => card.Name).SequenceEqual(ExpectedEncounterEnemyNames(fixture.Session, actual)) &&
                cards.All(card => card.IsCommander is null && card.IdentityArtwork is null &&
                    card.FullDetails.Contains(card.Stats, StringComparison.Ordinal) && card.Skills.All(skill =>
                        card.FullDetails.Contains(skill.Name, StringComparison.Ordinal) && card.FullDetails.Contains(skill.Description, StringComparison.Ordinal))) &&
                adapter.ShanheEncounterMainEnemyCards.Count == cards.Count && adapter.ShanheEncounterMinorEnemyCards.Count == 0 &&
                !adapter.HasShanheEncounterMinorEnemies &&
                adapter.ShanheEncounterMainEnemyCards.Zip(cards).All(pair => ReferenceEquals(pair.First, pair.Second)) &&
                fixture.Session.Revision == 0 && fixture.Session.PendingBattleSetup is null && fixture.Store.WriteCount == 0,
                $"The intentionally partial source{partial.Battle} recipe preserves its actual single supported body and complete rules as a full card, without borrowing omitted source members or their commander roles.");
            if (partial.Battle != 2675) continue;

            var journal = ShanheCampaignJson.Serialize(fixture.Session.CreateCheckpoint());
            var snapshot = JsonSerializer.Serialize(fixture.Session.Snapshot);
            var ordinary = Program.Engine(adapter);
            var ordinaryJournal = GameCheckpointJson.Serialize(ordinary.CreateCheckpoint());
            using var native = new NativeResponseStatusChecks.NativeHost(adapter);
            var partialRoot = (FrameworkElement)native.Window.Content;
            partialRoot.Width = 1120; partialRoot.Height = 780; native.Window.SizeToContent = SizeToContent.WidthAndHeight;
            PresentPartial();
            var partialPanel = (ShanheCampaignPanel)native.Window.FindName("ShanhePanel");
            var mainControl = (ItemsControl)partialPanel.FindName("ShanheEncounterEnemyCards");
            var minorControl = (ItemsControl)partialPanel.FindName("ShanheEncounterMinorEnemyCards");
            var partialCard = cards.Single();
            var portrait = AssertEncounterPortraitPresentation(mainControl, partialCard);
            var path = ShanheBossPortraitChecks.GetVerifiedSourcePortraitPath(partial.GeneralId);
            var expected = path is null ? null : CardGame.Wpf.Resources.AssetImages.Load(path, decodeWidth: 360);
            var bitmap = (portrait.Background as ImageBrush)?.ImageSource as BitmapSource;
            Program.Assert(path is not null && GeneralArt.GetSkin(partial.GeneralId)?.LocalPath == path &&
                partialCard.HasPortrait && bitmap is not null && expected is not null &&
                bitmap.PixelWidth == expected.PixelWidth && bitmap.PixelHeight == expected.PixelHeight,
                "The real partial XunYu preview binds its exact native source portrait without inventing a source commander identity.");
            var stride = bitmap!.PixelWidth * 4;
            var pixels = new byte[stride * bitmap.PixelHeight]; var expectedPixels = new byte[pixels.Length];
            new FormatConvertedBitmap(bitmap, PixelFormats.Bgra32, null, 0).CopyPixels(pixels, stride, 0);
            new FormatConvertedBitmap(expected!, PixelFormats.Bgra32, null, 0).CopyPixels(expectedPixels, stride, 0);
            Program.Assert(pixels.AsSpan().SequenceEqual(expectedPixels) && Enumerable.Range(0, pixels.Length / 4).Any(pixel => pixels[pixel * 4 + 3] > 0) &&
                mainControl.Items.Count == actual.Enemies.Count && mainControl.Items.Count == 1 &&
                ReferenceEquals(mainControl.Items[0], partialCard) && minorControl.Items.Count == 0 && !minorControl.IsVisible &&
                !adapter.HasShanheEncounterMinorEnemies && partialCard.IsCommander is null && partialCard.IdentityArtwork is null &&
                Program.Find<Image>(portrait).Single(image => image.Name == "EncounterIdentityBadge").Source is null &&
                Program.Find<TextBlock>(mainControl).Any(text => ReferenceEquals(text.DataContext, partialCard) && text.Text == partialCard.Stats && text.IsVisible) &&
                adapter.ShanheEncounterEnemyDetail.Contains($"{1 + actual.Enemies.Count + (actual.Allies?.Count ?? 0)}人阵容：敌军 {actual.Enemies.Count} · 同行 {actual.Allies?.Count ?? 0} · 玩家 1", StringComparison.Ordinal),
                "The loaded partial source preview paints the verified portrait pixels, exposes the complete actual formation count and keeps its sole real card without a minor strip or guessed role badge.");
            Program.Render(partialRoot, 1120, 780, Path.Combine(output, "shanhe-source-2675-partial-preview.png"));
            typeof(ButtonBase).GetMethod("OnClick", BindingFlags.Instance | BindingFlags.NonPublic)!
                .Invoke((Button)partialPanel.FindName("ShanheEncounterCloseButton"), null); PresentPartial();
            Program.Assert(!adapter.IsShanheEncounterPreviewOpen && fixture.Session.PendingBattleSetup is null && fixture.Store.WriteCount == 0 &&
                ShanheCampaignJson.Serialize(fixture.Session.CreateCheckpoint()) == journal && JsonSerializer.Serialize(fixture.Session.Snapshot) == snapshot &&
                ReferenceEquals(Program.Engine(adapter), ordinary) && GameCheckpointJson.Serialize(ordinary.CreateCheckpoint()) == ordinaryJournal,
                "Reading and closing the actual partial source portrait preserves both games, the campaign journal and all save boundaries.");

            void PresentPartial()
            {
                partialRoot.Measure(new Size(1120, 780)); partialRoot.Arrange(new Rect(0, 0, 1120, 780)); partialRoot.UpdateLayout();
                partialRoot.Dispatcher.Invoke(() => { }, DispatcherPriority.ContextIdle); partialRoot.UpdateLayout();
            }
        }

        (MainViewModel Model, ShanheCampaignSession Session, ShanheEncounterDefinition Encounter, PreviewTrackingShanheStore Store)
            SourcePreview(int battleId, int tier, Func<ShanheEncounterDefinition, ShanheEncounterDefinition>? change = null)
        {
            var source = StandardShanheCampaignCatalog.CreateSourceCodexDefinition(tier);
            var authored = source.Nodes.First(node => node.Encounters?.Any(candidate => candidate.SourceBattleId == battleId) == true);
            var actual = authored.Encounters!.Single(candidate => candidate.SourceBattleId == battleId);
            var selected = change?.Invoke(actual) ?? actual;
            var isolated = authored with
            {
                NextNodeIds = [], EliteUpgradeNodeId = null, Encounters = [selected],
                ChapterRouteId = null, ChapterRole = ShanheChapterNodeRole.None, ChapterProgressStep = 0, RequiredChapterProgress = 0
            };
            var entry = source with { StartingNodeIds = [authored.Id], RandomStartingNode = false, Nodes = [isolated] };
            var campaign = ShanheCampaignSession.Create(entry, new(83, "classic:guan-yu")
                { SourceDifficultyId = tier, UseCodexRewardPool = true }, StandardShanheCampaignCatalog.CreateBattleRegistry);
            var save = new PreviewTrackingShanheStore();
            var adapter = Create(new MemorySaveStore(), save);
            adapter.SelectedShanheScenario = adapter.ShanheScenarios.Single(option => option.Id == StandardShanheCampaignCatalog.Source2026EditionId);
            adapter.SelectedShanheDifficulty = adapter.ShanheDifficulties.Single(option => option.Value == tier);
            typeof(MainViewModel).GetField("_shanheSession", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(adapter, campaign);
            typeof(MainViewModel).GetField("_shanheRunDifficulty", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(adapter, tier);
            adapter.OpenShanheCommand.Execute(null);
            adapter.PreviewShanheNodeCommand.Execute(adapter.ShanheMapRows.SelectMany(row => row.Nodes).Single(node => node.Id == authored.Id));
            var prepared = campaign.PreviewEncounter(authored.Id);
            Program.Assert(adapter.IsShanheEncounterPreviewOpen && campaign.Revision == 0 && campaign.PendingBattleSetup is null && save.WriteCount == 0,
                "The isolated fixture opens the actual source recipe through its available map node without changing the campaign.");
            Program.Assert(prepared.Id == selected.Id && prepared.SourceBattleId == selected.SourceBattleId &&
                    prepared.Enemies.Select(enemy => enemy.GeneralId).SequenceEqual(selected.Enemies.Select(enemy => enemy.GeneralId)),
                "The fixture's expected encounter is the exact frozen source-growth preview of its isolated recipe, preserving source identity and seat order.");
            return (adapter, campaign, prepared, save);
        }

        void VerifySourceGrouping(int battleId, int tier, int commanderIndex, int backdrop, int width = 1120)
        {
            var fixture = SourcePreview(battleId, tier);
            using var adapter = fixture.Model;
            var campaign = fixture.Session;
            var actual = fixture.Encounter;
            var cards = adapter.ShanheEncounterEnemyCards;
            AssertEncounterEnemyVitals(campaign, actual, cards);
            var hasCommander = commanderIndex >= 0;
            // A known all-front-line source formation still has a first large
            // portrait; that display position does not make it a commander.
            var commander = cards[hasCommander ? commanderIndex : 0];
            var followers = hasCommander ? cards.Where(card => !ReferenceEquals(card, commander)).ToArray() : [];
            var mainCards = hasCommander ? new[] { commander } : cards.ToArray();
            var height = width == 1440 ? 880 : 780;
            var renderSuffix = battleId == 2653 ? "-d" + tier : battleId == 2686 || width != 1120 ? "-" + width : string.Empty;
            var expectedIds = battleId switch
            {
                2634 => new[] { StandardShanheS5XujingXuqiuPackage.XujingId, StandardShanheS5XujingXuqiuPackage.XuqiuId },
                2499 => new[] { StandardShanheSourceYulinOfficerPackage.HardId, StandardShanheSourceYulinGuardPackage.HellId },
                2685 => new[] { StandardShanheS5DongshiPackage.DongZhuoBaxinId, StandardShanheS5DongshiPackage.DongJiQiaoxiaoId },
                2686 => new[] { StandardShanheS5BossPackage.DongZhuoFengyiId, StandardShanheS5BossPackage.LvbuBajiId, StandardShanheS5BossPackage.DiaoChanBiyueId },
                2642 => new[] { StandardShanheS5MaSiblingsPackage.MachaoId, StandardShanheS5MaSiblingsPackage.MayunluId },
                2650 => new[] { StandardShanheS5DianweiWeiziPackage.DianweiId, StandardShanheS5DianweiWeiziPackage.WeiziId },
                2661 => new[] { StandardShanheS5XuyouJushouPackage.XuyouId, StandardShanheS5XuyouJushouPackage.JushouId },
                2674 => new[] { StandardShanheS5MoushiPackage.XunchenId, StandardShanheS5MoushiPackage.GuotuId, StandardShanheS5MoushiPackage.ShenpeiId },
                2679 => new[] { StandardShanheS5CaoWeiPioneersPackage.CaozhenId, StandardShanheS5CaoWeiPioneersPackage.CaocaoId, StandardShanheS5CaoWeiPioneersPackage.CaohongId },
                2683 => new[] { StandardShanheS5TaoyuanPackage.ZhangfeiId, StandardShanheS5TaoyuanPackage.LiubeiId, StandardShanheS5TaoyuanPackage.GuanyuId },
                2633 => new[] { StandardShanheS5YuduPackage.YuduId },
                2656 => new[] { StandardShanheS5ZhangliangPackage.ZhangliangId },
                2657 => new[] { StandardShanheS5XuncanCaoshiPackage.XuncanId, StandardShanheS5XuncanCaoshiPackage.CaoshiId },
                2654 => new[] { StandardShanheS5YuanshuPackage.YuanyaoId, StandardShanheS5YuanshuPackage.YuanshuId, StandardShanheS5YuanshuPackage.YuanyinId },
                2662 => new[] { StandardShanheS5GongsunZanPackage.GongsunZanId, StandardShanheS5GongsunZanPackage.GongsunYueId },
                2663 => new[] { StandardShanheS5XiliangDarkPackage.QinyiId, StandardShanheS5XiliangDarkPackage.ChenweiId, StandardShanheS5XiliangDarkPackage.LiheiId },
                2455 => new[] { StandardShanheSourceCoalitionScoutPackage.GetGeneralId(tier) },
                2459 => new[] { StandardShanheSourceCoalitionSpyPackage.GetGeneralId(tier) },
                2456 => new[] { StandardShanheSourceCoalitionTrioPackage.GetQuartermasterGeneralId(tier) },
                2465 => new[] { StandardShanheSourceCoalitionTrioPackage.GetSpyGeneralId(tier),
                    StandardShanheSourceCoalitionTrioPackage.GetScoutGeneralId(tier), StandardShanheSourceCoalitionTrioPackage.GetQuartermasterGeneralId(tier) },
                2464 => new[] { StandardShanheSourceDemonAssassinPackage.GetGeneralId(tier),
                    StandardShanheSourceDemonArcherPackage.GetTrioGeneralId(tier), StandardShanheSourceDemonRaiderPackage.GetTrioGeneralId(tier) },
                2453 => new[] { StandardShanheSourceWarElephantPackage.GetGeneralId(tier) },
                2494 => new[] { StandardShanheSourceVanguardPackage.GetGeneralId(tier) },
                2491 => StandardShanheSourceGuardTrioPackage.GetRosterGeneralIds(tier).ToArray(),
                2472 => StandardShanheSourceElementalTrioPackage.GetRosterGeneralIds(tier).ToArray(),
                2450 => StandardShanheSourceBeigongDuoPackage.GetRosterGeneralIds(tier).ToArray(),
                2475 => StandardShanheSourceMilitiaTrioPackage.GetRosterGeneralIds(tier).ToArray(),
                2485 => StandardShanheSourceCaoSongTrioPackage.GetRosterGeneralIds(tier).ToArray(),
                2463 => StandardShanheSourceLiuYanTrioPackage.GetRosterGeneralIds(tier).ToArray(),
                2471 => StandardShanheSourceQianZhaoPairPackage.GetRosterGeneralIds(tier).ToArray(),
                2479 => new[] { StandardShanheSourceLiuSandaoPackage.GetGeneralId(tier) },
                2489 => new[] { StandardShanheSourceXinPingPackage.GetGeneralId(tier) },
                2684 => new[] { StandardShanheSourceMouDongZhuoPackage.GeneralId },
                2653 => new[] { tier < 11 ? StandardShanheSourceZhangYanPackage.ZhangniujiaoMediumId : tier < 16
                    ? StandardShanheSourceZhangYanPackage.ZhangniujiaoHardId : StandardShanheSourceZhangYanPackage.ZhangniujiaoNightmareId },
                _ => throw new InvalidOperationException("Only the verified complete source profiles belong in this check.")
            };
            Program.Assert(actual.SourceBattleId == battleId && actual.SourceVisibleEnemyCount == expectedIds.Length && actual.Enemies.Count == expectedIds.Length &&
                actual.Enemies.Select(enemy => enemy.GeneralId).SequenceEqual(expectedIds) &&
                cards.Select(card => card.Name).SequenceEqual(ExpectedEncounterEnemyNames(campaign, actual)) &&
                cards.Select(card => card.SeatOrdinal).SequenceEqual(Enumerable.Range(1, expectedIds.Length)) &&
                (hasCommander ? commander.IsCommander == true && followers.All(card => card.IsCommander == false)
                    : cards.All(card => card.IsCommander == false) && !adapter.HasShanheEncounterMinorEnemies) &&
                adapter.HasShanheEncounterMinorEnemies == (followers.Length > 0) &&
                adapter.ShanheEncounterMainEnemyCards.Count == mainCards.Length && adapter.ShanheEncounterMinorEnemyCards.Count == followers.Length &&
                adapter.ShanheEncounterMainEnemyCards.Zip(mainCards).All(pair => ReferenceEquals(pair.First, pair.Second)) &&
                adapter.ShanheEncounterMinorEnemyCards.Zip(followers).All(pair => ReferenceEquals(pair.First, pair.Second)) &&
                adapter.ShanheEncounterEnemyPortraitName == commander.Name,
                $"Source{battleId}/{tier} preserves every source seat and known role: only a proven commander separates followers; a proven all-front-line formation retains every full-size card without inventing a commander.");
            if (battleId == 2653)
            {
                var secondId = tier < 11 ? StandardShanheSourceZhangYanPackage.ZhangYanMediumId : StandardShanheSourceZhangYanPackage.ZhangYanHardId;
                Program.Assert(actual.BossTransition?.GeneralId == secondId && cards.Count == 1 && followers.Length == 0 &&
                    adapter.ShanheEncounterEnemyDetail.Contains("二阶段：张燕", StringComparison.Ordinal),
                    "The real ZhangYan reserved form is disclosed as a same-seat second stage, never added as a current follower portrait.");
            }
            for (var index = 0; index < cards.Count; index++)
            {
                var card = cards[index];
                AssertOriginalIdentity(card.IdentityArtwork, card.IsCommander == true ? "master" : "winger");
                if (ShanheBossPortraitChecks.GetVerifiedSourcePortraitPath(actual.Enemies[index].GeneralId) is { } portraitPath)
                {
                    var expected = CardGame.Wpf.Resources.AssetImages.Load(portraitPath, decodeWidth: 360);
                    var bitmap = (card.Portrait as ImageBrush)?.ImageSource as BitmapSource;
                    Program.Assert(bitmap is not null && expected is not null && bitmap.PixelWidth == expected.PixelWidth && bitmap.PixelHeight == expected.PixelHeight &&
                        GeneralArt.GetSkin(actual.Enemies[index].GeneralId)?.LocalPath == portraitPath,
                        "Every verified actual enemy card decodes its exact original source portrait, including newly restored single commanders.");
                    var stride = bitmap!.PixelWidth * 4;
                    var pixels = new byte[stride * bitmap.PixelHeight]; var expectedPixels = new byte[pixels.Length];
                    new FormatConvertedBitmap(bitmap, PixelFormats.Bgra32, null, 0).CopyPixels(pixels, stride, 0);
                    new FormatConvertedBitmap(expected!, PixelFormats.Bgra32, null, 0).CopyPixels(expectedPixels, stride, 0);
                    Program.Assert(pixels.AsSpan().SequenceEqual(expectedPixels) && Enumerable.Range(0, pixels.Length / 4).Any(pixel => pixels[pixel * 4 + 3] > 0),
                        "The actual enemy preview portrait preserves the verified source pixels and contains painted artwork.");
                }
                Program.Assert(card.FullDetails.Contains(card.Name, StringComparison.Ordinal) && card.FullDetails.Contains(card.Stats, StringComparison.Ordinal) &&
                    card.Skills.All(skill => card.FullDetails.Contains(skill.Name, StringComparison.Ordinal) && card.FullDetails.Contains(skill.Description, StringComparison.Ordinal)),
                    "Both main and small portrait cards retain the exact complete statistics and published skill rules.");
            }
            var journal = ShanheCampaignJson.Serialize(campaign.CreateCheckpoint());
            var snapshot = JsonSerializer.Serialize(campaign.Snapshot);
            var ordinary = Program.Engine(adapter);
            var ordinaryJournal = GameCheckpointJson.Serialize(ordinary.CreateCheckpoint());
            var baseDetail = adapter.ShanheEncounterEnemyDetail;
            using var native = new NativeResponseStatusChecks.NativeHost(adapter);
            var root = (FrameworkElement)native.Window.Content;
            root.Width = width; root.Height = height; native.Window.SizeToContent = SizeToContent.WidthAndHeight;
            Present();
            var panel = (ShanheCampaignPanel)native.Window.FindName("ShanhePanel");
            var mainControl = (ItemsControl)panel.FindName("ShanheEncounterEnemyCards");
            var minorControl = (ItemsControl)panel.FindName("ShanheEncounterMinorEnemyCards");
            var detailScroll = (ScrollViewer)panel.FindName("ShanheEncounterEnemyDetailScroll");
            var commanderScroll = (ScrollViewer)panel.FindName("ShanheEncounterEnemyScroll");
            var selectionLabel = (TextBlock)panel.FindName("ShanheEncounterSelectedEnemyLabel");
            Program.Assert(mainControl.Items.Count == mainCards.Length && mainControl.Items.Cast<object>().Zip(mainCards).All(pair => ReferenceEquals(pair.First, pair.Second)) &&
                minorControl.IsVisible == (followers.Length > 0) && minorControl.Items.Count == followers.Length &&
                minorControl.Items.Cast<object>().Zip(followers).All(pair => ReferenceEquals(pair.First, pair.Second)) &&
                mainCards.All(card => Program.Find<TextBlock>(mainControl).Any(text => ReferenceEquals(text.DataContext, card) &&
                    text.Name == "EncounterVerticalName" && text.Text == card.VerticalName && text.IsVisible)),
                "The loaded native preview preserves every large source portrait and only displays a minor formation when a real commander exists.");
            if (battleId is 2485 or 2463 or 2450)
            {
                // Check the opening layout before any BringIntoView can hide
                // a clipped skill row by scrolling the commander's portrait away.
                AssertInitialCommanderContent();
                var originalWidth = width; var originalHeight = height;
                try
                {
                    width = 1440; height = 880; root.Width = width; root.Height = height; Present();
                    AssertInitialCommanderContent();
                    Program.Render(root, width, height, Path.Combine(output, $"shanhe-source-{battleId}-grouped-preview-1440.png"));
                }
                finally { width = originalWidth; height = originalHeight; root.Width = width; root.Height = height; Present(); }
                AssertInitialCommanderContent();

                void AssertInitialCommanderContent()
                {
                    var viewport = Program.Find<ScrollContentPresenter>(commanderScroll).Single();
                    var portrait = AssertEncounterPortraitPresentation(mainControl, commander);
                    var stats = Program.Find<TextBlock>(mainControl).Single(text =>
                        ReferenceEquals(text.DataContext, commander) && text.Text == commander.Stats);
                    var chips = Program.Find<Border>(mainControl).Where(border => border.ToolTip is ToolTip &&
                        commander.Skills.Any(skill => ReferenceEquals(border.DataContext, skill))).ToArray();
                    var portraitBounds = InViewport(portrait);
                    var statsBounds = InViewport(stats);
                    var skillBounds = chips.Select(InViewport).ToArray();
                    var lastCharacter = stats.ContentEnd.GetCharacterRect(System.Windows.Documents.LogicalDirection.Backward);
                    var lastStatsBounds = stats.TransformToAncestor(viewport).TransformBounds(lastCharacter);
                    Program.Assert(hasCommander && followers.Length > 0 && commander.IsCommander == true && mainControl.Items.Count == 1 &&
                            Math.Abs(commanderScroll.HorizontalOffset) <= .7 && Math.Abs(commanderScroll.VerticalOffset) <= .7 &&
                            portrait.IsVisible && stats.IsVisible && portraitBounds.Width > 0 && portraitBounds.Height > 0 &&
                            statsBounds.Width > 0 && statsBounds.Height > 0 && Fits(portraitBounds) && Fits(statsBounds) &&
                            !lastCharacter.IsEmpty && Fits(lastStatsBounds) &&
                            chips.Length == commander.Skills.Count && chips.Length > 0 &&
                            chips.Select(chip => chip.DataContext).Zip(commander.Skills).All(pair => ReferenceEquals(pair.First, pair.Second)) &&
                            chips.All(chip => chip.IsVisible && chip.ActualWidth > 0 && chip.ActualHeight > 0 &&
                                Program.Find<TextBlock>(chip).Any(text => text.IsVisible && text.Text == ((ShanheEncounterSkill)chip.DataContext).Name)) &&
                            skillBounds.All(Fits),
                        "A newly opened real grouped encounter simultaneously shows its commander's complete portrait, every statistic line through the last character and every actual skill without initial scrolling, at either supported window width: " +
                        JsonSerializer.Serialize(new { battleId, width, height, viewport = viewport.RenderSize,
                            commanderScroll.HorizontalOffset, commanderScroll.VerticalOffset, commander.Stats,
                            portraitBounds, statsBounds, lastStatsCharacter = lastStatsBounds.ToString(), skillBounds,
                            expectedSkills = commander.Skills.Select(skill => skill.Name), actualSkills = chips.Select(chip => ((ShanheEncounterSkill)chip.DataContext).Name) }));

                    Rect InViewport(FrameworkElement element) => element.TransformToAncestor(viewport).TransformBounds(new Rect(element.RenderSize));
                    bool Fits(Rect bounds) => !bounds.IsEmpty && bounds.Left >= -.7 && bounds.Top >= -.7 &&
                        bounds.Right <= viewport.ActualWidth + .7 && bounds.Bottom <= viewport.ActualHeight + .7;
                }
            }
            foreach (var card in mainCards)
            {
                var portrait = AssertEncounterPortraitPresentation(mainControl, card);
                portrait.BringIntoView(); Present();
                var viewport = Program.Find<ScrollContentPresenter>(commanderScroll).Single();
                var bounds = portrait.TransformToAncestor(viewport).TransformBounds(new Rect(portrait.RenderSize));
                Program.Assert(bounds.Left >= -.7 && bounds.Top >= -.7 && bounds.Right <= viewport.ActualWidth + .7 && bounds.Bottom <= viewport.ActualHeight + .7,
                    "Every real large source card, including a known front-line body without any commander, remains completely reachable in the native viewport.");
                var badge = Program.Find<Image>(portrait).Single(image => image.Name == "EncounterIdentityBadge" && ReferenceEquals(image.DataContext, card));
                Program.Assert(badge.IsVisible && badge.ActualWidth > 0 && badge.ActualHeight > 0,
                    "Every actual large source card displays its own proven source identity artwork.");
                AssertOriginalIdentity(badge.Source, card.IsCommander == true ? "master" : "winger");
            }
            commanderScroll.ScrollToHorizontalOffset(0); commanderScroll.ScrollToTop(); Present();
            if (battleId is 2465 or 2464 or 2491 or 2472)
            {
                AssertCompactTrio();
                var originalWidth = width; var originalHeight = height;
                try
                {
                    width = 1120; height = 780; root.Width = width; root.Height = height; Present();
                    AssertCompactTrio();
                    Program.Render(root, width, height, Path.Combine(output, $"shanhe-source-{battleId}-grouped-preview-1120.png"));
                }
                finally { width = originalWidth; height = originalHeight; root.Width = width; root.Height = height; Present(); }

                void AssertCompactTrio()
                {
                    var viewport = Program.Find<ScrollContentPresenter>(commanderScroll).Single();
                    var fullCards = Enumerable.Range(0, mainControl.Items.Count).Select(index =>
                        Program.Find<StackPanel>((FrameworkElement)mainControl.ItemContainerGenerator.ContainerFromIndex(index))
                            .Single(card => card.Name == "EncounterMajorCard")).ToArray();
                    var bounds = fullCards.Select(card => card.TransformToAncestor(viewport).TransformBounds(new Rect(card.RenderSize))).ToArray();
                    Program.Assert(!hasCommander && cards.Count == 3 && cards.All(card => card.IsCommander == false && card.RoleLabel == "前锋") &&
                            mainControl.Items.Count == 3 && adapter.ShanheEncounterMainEnemyCards.Count == 3 &&
                            minorControl.Items.Count == 0 && !minorControl.IsVisible && !adapter.HasShanheEncounterMinorEnemies &&
                            Math.Abs(root.ActualWidth - width) < 1 && Math.Abs(root.ActualHeight - height) < 1 &&
                            commanderScroll.ScrollableWidth <= .7 && commanderScroll.HorizontalOffset <= .7 &&
                            bounds.All(rect => rect.Width > 0 && rect.Height > 0 && rect.Left >= -.7 && rect.Top >= -.7 &&
                                rect.Right <= viewport.ActualWidth + .7 && rect.Bottom <= viewport.ActualHeight + .7) &&
                            bounds.SelectMany((rect, index) => bounds.Skip(index + 1).Select(other => !rect.IntersectsWith(other))).All(separate => separate),
                        $"The verified source{battleId} three-front-line formation simultaneously displays all three complete compact cards and their real names, health, stats and skills without a horizontal scroll or an invented commander. " +
                        JsonSerializer.Serialize(new { width, height, viewport = viewport.RenderSize, commanderScroll.ScrollableWidth, commanderScroll.HorizontalOffset, bounds }));
                }
            }
            AssertEncounterBackdrop(panel, backdrop);
            Program.Assert(!adapter.HasShanheEncounterEnemySelection && adapter.ShanheEncounterEnemyDetail == baseDetail &&
                adapter.ShanheEncounterSelectedEnemyLabel.Length == 0 && selectionLabel.Visibility == Visibility.Collapsed,
                "A newly opened source preview initially presents the complete formation without silently selecting a follower.");
            if (followers.Length > 0)
            {
                var minorScroll = Program.Find<ScrollViewer>(panel).Single(scroll => ReferenceEquals(scroll.Content, minorControl));
                var minorPresenter = Program.Find<ScrollContentPresenter>(minorScroll).Single();
                foreach (var card in followers)
                {
                    var button = Bound(minorControl, adapter.SelectShanheEncounterEnemyCommand, card);
                    var bounds = button.TransformToAncestor(minorPresenter).TransformBounds(new Rect(button.RenderSize));
                    var rootBounds = button.TransformToAncestor(root).TransformBounds(new Rect(button.RenderSize));
                    var hit = root.InputHitTest(new Point(rootBounds.Left + rootBounds.Width / 2, rootBounds.Top + rootBounds.Height / 2)) as DependencyObject;
                    while (hit is not null && !ReferenceEquals(hit, button)) hit = VisualTreeHelper.GetParent(hit);
                    Program.Assert(button.IsVisible && bounds.Width > 0 && bounds.Height > 0 && bounds.Left >= -.5 && bounds.Top >= -.5 &&
                        bounds.Right <= minorPresenter.ActualWidth + .5 && bounds.Bottom <= minorPresenter.ActualHeight + .5 && ReferenceEquals(hit, button),
                        "Each actual follower is fully visible and receives its own pointer hits, including both source2686 portraits at either window width.");
                }
            }
            Program.Render(root, width, height, Path.Combine(output, $"shanhe-source-{battleId}-grouped-preview{renderSuffix}.png"));
            var skillsToRead = hasCommander
                ? followers.Length == 0 ? commander.Skills : commander.Skills.TakeLast(1)
                : mainCards.SelectMany(card => card.Skills);
            foreach (var lastSkill in skillsToRead)
            {
                var chip = Program.Find<Border>(mainControl).Single(border => ReferenceEquals(border.DataContext, lastSkill) && border.ToolTip is ToolTip);
                chip.BringIntoView(); Present();
                var presenter = Program.Find<ScrollContentPresenter>(commanderScroll).Single();
                var bounds = chip.TransformToAncestor(presenter).TransformBounds(new Rect(chip.RenderSize));
                Program.Assert(chip.IsVisible && bounds.Width > 0 && bounds.Height > 0 &&
                    bounds.Left >= -.5 && bounds.Top >= -.5 && bounds.Right <= presenter.ActualWidth + .5 &&
                    bounds.Bottom <= presenter.ActualHeight + .5 &&
                    commanderScroll.VerticalScrollBarVisibility == ScrollBarVisibility.Auto,
                    "The source commander's final actual skill remains reachable in the native scroll viewport above the compact formation and challenge rows: " +
                    JsonSerializer.Serialize(new { battleId, bounds, presenter.ActualWidth, presenter.ActualHeight,
                        commanderScroll.VerticalOffset, commanderScroll.ScrollableHeight, chip.IsVisible }));
                var lastTooltip = (ToolTip)chip.ToolTip;
                lastTooltip.PlacementTarget = chip; lastTooltip.Placement = PlacementMode.AbsolutePoint;
                lastTooltip.HorizontalOffset = -30000; lastTooltip.VerticalOffset = -30000;
                lastTooltip.Opened += ConcealEncounterTooltip;
                try
                {
                    lastTooltip.IsOpen = true; Present();
                    var rules = Program.Find<TextBlock>(lastTooltip)
                        .Where(text => text.IsVisible && text.ActualWidth > 0 && text.ActualHeight > 0).ToArray();
                    Program.Assert(lastTooltip.ContentTemplate is null && PresentationSource.FromVisual(lastTooltip) is HwndSource &&
                        rules.Any(text => text.Text == lastSkill.Name) && rules.Any(text => text.Text == lastSkill.Description),
                        "The source commander's last skill opens its actual native tooltip with the complete visible name and rule.");
                }
                finally { lastTooltip.IsOpen = false; lastTooltip.Opened -= ConcealEncounterTooltip; }
                commanderScroll.ScrollToTop(); Present();
            }
            if (followers.Length == 0)
            {
                Program.Assert((hasCommander ? cards.Count == 1 : cards.All(card => card.IsCommander == false)) &&
                    mainControl.Items.Count == mainCards.Length && minorControl.Items.Count == 0 && !minorControl.IsVisible &&
                    !adapter.HasShanheEncounterEnemySelection && selectionLabel.Visibility == Visibility.Collapsed &&
                    baseDetail.Contains($"{1 + actual.Enemies.Count + (actual.Allies?.Count ?? 0)}人阵容：敌军 {actual.Enemies.Count} · 同行 {actual.Allies?.Count ?? 0} · 玩家 1", StringComparison.Ordinal) &&
                    mainCards.All(card => baseDetail.Contains(card.Name, StringComparison.Ordinal) &&
                        Program.Find<TextBlock>(mainControl).Any(text => ReferenceEquals(text.DataContext, card) && text.Text == card.Stats && text.IsVisible)),
                    "A single commander or a proven all-front-line formation keeps every decoded large card, complete statistics and correct seat count without inventing a minor strip or selection.");
                Program.Assert(ShanheCampaignJson.Serialize(campaign.CreateCheckpoint()) == journal && JsonSerializer.Serialize(campaign.Snapshot) == snapshot &&
                    campaign.PendingBattleSetup is null && fixture.Store.WriteCount == 0 && ReferenceEquals(Program.Engine(adapter), ordinary) &&
                    GameCheckpointJson.Serialize(ordinary.CreateCheckpoint()) == ordinaryJournal,
                    "Reading every actual full-size card, skill and native tooltip remains entirely read-only.");
                typeof(ButtonBase).GetMethod("OnClick", BindingFlags.Instance | BindingFlags.NonPublic)!
                    .Invoke((Button)panel.FindName("ShanheEncounterCloseButton"), null); Present();
                Program.Assert(!adapter.IsShanheEncounterPreviewOpen && campaign.PendingBattleSetup is null && fixture.Store.WriteCount == 0,
                    "The actual full-size source formation closes without entering battle or writing a save.");
                return;
            }
            var follower = followers[0];
            var small = Bound(minorControl, adapter.SelectShanheEncounterEnemyCommand, follower);
            Program.Assert(small.IsVisible && small.ActualWidth > 0 && small.ActualHeight > 0 && small.ToolTip is ToolTip &&
                small.IsEnabled && small.Command!.CanExecute(follower) && small.Focusable &&
                System.Windows.Automation.AutomationProperties.GetName(small).Contains(follower.Name, StringComparison.Ordinal),
                "The real small portrait keeps its named, keyboard-accessible reading action and full-rule tooltip.");
            typeof(ButtonBase).GetMethod("OnClick", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(small, null);
            Present();
            var selectedDetail = adapter.ShanheEncounterEnemyDetail;
            var firstFollowerLabel = adapter.ShanheEncounterSelectedEnemyLabel;
            Program.Assert(selectedDetail.StartsWith(baseDetail, StringComparison.Ordinal) && selectedDetail.EndsWith(follower.FullDetails, StringComparison.Ordinal) &&
                Program.Find<TextBlock>(detailScroll).Any(text => text.Text == selectedDetail && text.IsVisible && text.ActualHeight > 0) &&
                (detailScroll.ScrollableHeight <= .5 || Math.Abs(detailScroll.VerticalOffset - detailScroll.ScrollableHeight) < 1),
                "Clicking a real small portrait preserves the full formation and reveals that seat's complete rules at the reachable detail bottom.");
            adapter.SelectShanheEncounterEnemyCommand.Execute(follower); Present();
            Program.Assert(adapter.ShanheEncounterEnemyDetail == selectedDetail,
                "Selecting the same real follower twice does not append duplicate statistics or skill rules.");
            AssertFixedSelectionLabel(follower);
            Program.Render(root, width, height, Path.Combine(output, $"shanhe-source-{battleId}-grouped-preview{renderSuffix}-selected.png"));
            // Move to the commander's details first, so Enter must perform a
            // genuine different-seat selection rather than pass as a no-op.
            adapter.SelectShanheEncounterEnemyCommand.Execute(commander); Present();
            Program.Assert(adapter.ShanheEncounterEnemyDetail.EndsWith(commander.FullDetails, StringComparison.Ordinal) &&
                adapter.ShanheEncounterEnemyDetail != selectedDetail,
                "The source commander's full rules can be selected without changing the base formation.");
            native.FocusKeyboard();
            Program.Assert(small.Focus() && ReferenceEquals(Keyboard.FocusedElement, small),
                "The actual small formation Button receives keyboard focus in its loaded hidden native host.");
            TableShortcutChecks.WithModifiers(ModifierKeys.None, () =>
            {
                var enter = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromVisual(small)!, Environment.TickCount, Key.Enter)
                    { RoutedEvent = Keyboard.PreviewKeyDownEvent };
                small.RaiseEvent(enter);
                Program.Assert(!enter.Handled,
                    "The encounter owner window leaves Enter available to the focused small portrait Button.");
                enter.RoutedEvent = Keyboard.KeyDownEvent; small.RaiseEvent(enter);
                Program.Assert(enter.Handled,
                    "The real focused small portrait Button handles Enter through its normal keyboard activation.");
                return true;
            });
            Present();
            Program.Assert(adapter.HasShanheEncounterEnemySelection && adapter.ShanheEncounterEnemyDetail == selectedDetail &&
                Program.Find<TextBlock>(detailScroll).Any(text => text.Text == selectedDetail && text.IsVisible && text.ActualHeight > 0) &&
                (detailScroll.ScrollableHeight <= .5 || Math.Abs(detailScroll.VerticalOffset - detailScroll.ScrollableHeight) < 1) &&
                ShanheCampaignJson.Serialize(campaign.CreateCheckpoint()) == journal && JsonSerializer.Serialize(campaign.Snapshot) == snapshot &&
                campaign.PendingBattleSetup is null && fixture.Store.WriteCount == 0 && ReferenceEquals(Program.Engine(adapter), ordinary) &&
                GameCheckpointJson.Serialize(ordinary.CreateCheckpoint()) == ordinaryJournal,
                "Native Enter selects the follower's complete visible rules and scrolls to them while preserving both games, the campaign RNG and all save boundaries.");
            AssertFixedSelectionLabel(follower);
            var tooltip = (ToolTip)small.ToolTip;
            tooltip.PlacementTarget = small; tooltip.Placement = PlacementMode.AbsolutePoint;
            tooltip.HorizontalOffset = -30000; tooltip.VerticalOffset = -30000;
            tooltip.Opened += ConcealEncounterTooltip;
            try
            {
                tooltip.IsOpen = true; Present();
                var visible = Program.Find<TextBlock>(tooltip).Where(text => text.IsVisible && text.ActualWidth > 0 && text.ActualHeight > 0).ToArray();
                var displayed = string.Join("\n", visible.Select(text => text.Text));
                Program.Assert(tooltip.ContentTemplate is null && PresentationSource.FromVisual(tooltip) is HwndSource &&
                    displayed.Contains(follower.Name, StringComparison.Ordinal) && displayed.Contains(follower.Stats, StringComparison.Ordinal) &&
                    follower.Skills.All(skill => displayed.Contains(skill.Name, StringComparison.Ordinal) && displayed.Contains(skill.Description, StringComparison.Ordinal)) &&
                    visible.All(text => !text.Text.StartsWith("System.Windows.Controls.", StringComparison.Ordinal)) &&
                    Program.Find<ScrollViewer>(tooltip).Any(scroll => scroll.VerticalScrollBarVisibility == ScrollBarVisibility.Auto),
                    "The actual small-portrait native tooltip visually renders every published rule in a scrollable surface instead of a UIElement type name.");
            }
            finally { tooltip.IsOpen = false; tooltip.Opened -= ConcealEncounterTooltip; }
            foreach (var other in followers.Skip(1))
            {
                var otherButton = Bound(minorControl, adapter.SelectShanheEncounterEnemyCommand, other);
                typeof(ButtonBase).GetMethod("OnClick", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(otherButton, null); Present();
                var otherDetail = adapter.ShanheEncounterEnemyDetail;
                // Separate source seats can have identical names, stats and rules.
                // Their selected seat label must still change independently.
                Program.Assert(otherDetail.StartsWith(baseDetail, StringComparison.Ordinal) && otherDetail.EndsWith(other.FullDetails, StringComparison.Ordinal) &&
                    (other.FullDetails == follower.FullDetails ? otherDetail == selectedDetail : otherDetail != selectedDetail) &&
                    adapter.ShanheEncounterSelectedEnemyLabel != firstFollowerLabel &&
                    otherDetail.Length == selectedDetail.Length - follower.FullDetails.Length + other.FullDetails.Length &&
                    Program.Find<TextBlock>(detailScroll).Any(text => text.Text == otherDetail && text.IsVisible && text.ActualHeight > 0),
                    "Clicking the second actual follower replaces only the selected rules and retains the complete three-seat formation without accumulating the first follower's details.");
                adapter.SelectShanheEncounterEnemyCommand.Execute(other); Present();
                Program.Assert(adapter.ShanheEncounterEnemyDetail == otherDetail,
                    "Repeatedly selecting the second real follower also keeps a single complete details block.");
                AssertFixedSelectionLabel(other);
                Program.Render(root, width, height, Path.Combine(output, $"shanhe-source-{battleId}-grouped-preview{renderSuffix}-selected-seat-{other.SeatOrdinal}.png"));
                adapter.SelectShanheEncounterEnemyCommand.Execute(commander); Present();
                native.FocusKeyboard();
                Program.Assert(otherButton.Focus() && ReferenceEquals(Keyboard.FocusedElement, otherButton),
                    "The second actual follower owns keyboard focus independently of the first small portrait.");
                TableShortcutChecks.WithModifiers(ModifierKeys.None, () =>
                {
                    var enter = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromVisual(otherButton)!, Environment.TickCount, Key.Enter)
                        { RoutedEvent = Keyboard.PreviewKeyDownEvent };
                    otherButton.RaiseEvent(enter);
                    Program.Assert(!enter.Handled, "The owner window preserves the second follower's normal Enter activation.");
                    enter.RoutedEvent = Keyboard.KeyDownEvent; otherButton.RaiseEvent(enter);
                    Program.Assert(enter.Handled, "The second native small portrait Button handles its Enter activation.");
                    return true;
                });
                Present();
                Program.Assert(adapter.ShanheEncounterEnemyDetail == otherDetail,
                    "Enter on the second real follower selects that follower's complete statistics and skills, never the first portrait's rules.");
                AssertFixedSelectionLabel(other);
                var otherTooltip = (ToolTip)otherButton.ToolTip;
                otherTooltip.PlacementTarget = otherButton; otherTooltip.Placement = PlacementMode.AbsolutePoint;
                otherTooltip.HorizontalOffset = -30000; otherTooltip.VerticalOffset = -30000;
                otherTooltip.Opened += ConcealEncounterTooltip;
                try
                {
                    otherTooltip.IsOpen = true; Present();
                    var visible = Program.Find<TextBlock>(otherTooltip).Where(text => text.IsVisible && text.ActualWidth > 0 && text.ActualHeight > 0).ToArray();
                    var displayed = string.Join("\n", visible.Select(text => text.Text));
                    Program.Assert(otherTooltip.ContentTemplate is null && PresentationSource.FromVisual(otherTooltip) is HwndSource &&
                        displayed.Contains(other.Name, StringComparison.Ordinal) && displayed.Contains(other.Stats, StringComparison.Ordinal) &&
                        other.Skills.All(skill => displayed.Contains(skill.Name, StringComparison.Ordinal) && displayed.Contains(skill.Description, StringComparison.Ordinal)) &&
                        visible.All(text => !text.Text.StartsWith("System.Windows.Controls.", StringComparison.Ordinal)) &&
                        Program.Find<ScrollViewer>(otherTooltip).Any(scroll => scroll.VerticalScrollBarVisibility == ScrollBarVisibility.Auto),
                        "The second follower's actual native tooltip renders its own complete rules in a scrollable surface.");
                }
                finally { otherTooltip.IsOpen = false; otherTooltip.Opened -= ConcealEncounterTooltip; }
            }
            Program.Assert(ShanheCampaignJson.Serialize(campaign.CreateCheckpoint()) == journal && JsonSerializer.Serialize(campaign.Snapshot) == snapshot &&
                campaign.PendingBattleSetup is null && fixture.Store.WriteCount == 0 && ReferenceEquals(Program.Engine(adapter), ordinary) &&
                GameCheckpointJson.Serialize(ordinary.CreateCheckpoint()) == ordinaryJournal,
                "Reading source groups, selecting a follower and opening full rules changes only the preview UI, never a campaign command, RNG, battle or save.");
            typeof(ButtonBase).GetMethod("OnClick", BindingFlags.Instance | BindingFlags.NonPublic)!
                .Invoke((Button)panel.FindName("ShanheEncounterCloseButton"), null); Present();
            Program.Assert(!adapter.IsShanheEncounterPreviewOpen && campaign.PendingBattleSetup is null && fixture.Store.WriteCount == 0,
                "The actual grouped source preview closes without entering its battle or writing a save.");

            void AssertFixedSelectionLabel(ShanheEncounterEnemyCard selected)
            {
                native.FocusKeyboard();
                var challenge = (Button)panel.FindName("ShanheChallengeButton");
                Program.Assert(challenge.Focus() && ReferenceEquals(Keyboard.FocusedElement, challenge),
                    "The selected portrait can lose focus to the challenge button without losing its displayed rules.");
                detailScroll.ScrollToBottom(); Present();
                var bounds = selectionLabel.TransformToAncestor(root).TransformBounds(new Rect(selectionLabel.RenderSize));
                var scrollBounds = detailScroll.TransformToAncestor(root).TransformBounds(new Rect(detailScroll.RenderSize));
                Program.Assert(adapter.ShanheEncounterSelectedEnemyLabel == $"当前查看：{selected.SeatOrdinal}号敌席 · {selected.Name}" &&
                    selectionLabel.Text == adapter.ShanheEncounterSelectedEnemyLabel && selectionLabel.IsVisible &&
                    bounds.Width > 0 && bounds.Height > 0 && bounds.Left >= 0 && bounds.Top >= 0 && bounds.Right <= width && bounds.Bottom <= height &&
                    bounds.Bottom <= scrollBounds.Top + .5 && !detailScroll.IsAncestorOf(selectionLabel) &&
                    adapter.ShanheEncounterEnemyDetail.StartsWith(baseDetail, StringComparison.Ordinal) &&
                    adapter.ShanheEncounterEnemyDetail.EndsWith(selected.FullDetails, StringComparison.Ordinal) &&
                    cards.All(card => baseDetail.Contains(card.Name, StringComparison.Ordinal)) &&
                    (detailScroll.ScrollableHeight <= .5 || Math.Abs(detailScroll.VerticalOffset - detailScroll.ScrollableHeight) < 1),
                    "The fixed actual selected-enemy title remains fully visible outside the scrolled details and identifies the correct follower after focus moves away, while the complete formation stays intact.");
            }

            void Present()
            {
                root.Measure(new Size(width, height)); root.Arrange(new Rect(0, 0, width, height)); root.UpdateLayout();
                root.Dispatcher.Invoke(() => { }, DispatcherPriority.ContextIdle); root.UpdateLayout();
            }
        }

        void AssertOriginalIdentity(ImageSource? source, string role)
        {
            var expected = CardGame.Wpf.Resources.AssetImages.Load($"Assets/Shanhe/Encounter/figure_cool_{role}.png");
            Program.Assert(source is BitmapSource && expected is not null,
                "A verified source role uses its original decoded identity PNG.");
            var bitmap = (BitmapSource)source!;
            Program.Assert(bitmap.PixelWidth == expected!.PixelWidth && bitmap.PixelHeight == expected.PixelHeight,
                "The original role identity image retains its source dimensions.");
            var stride = bitmap.PixelWidth * 4;
            var actualPixels = new byte[stride * bitmap.PixelHeight];
            var expectedPixels = new byte[actualPixels.Length];
            new FormatConvertedBitmap(bitmap, PixelFormats.Bgra32, null, 0).CopyPixels(actualPixels, stride, 0);
            new FormatConvertedBitmap(expected, PixelFormats.Bgra32, null, 0).CopyPixels(expectedPixels, stride, 0);
            Program.Assert(actualPixels.AsSpan().SequenceEqual(expectedPixels),
                "Commander and ordinary identities use the corresponding original pixels rather than a locally redrawn role.");
        }
    }

    private static string[] ExpectedEncounterEnemyNames(ShanheCampaignSession session, ShanheEncounterDefinition encounter)
    {
        var registry = EncounterPreviewRegistry(session, encounter);
        return encounter.Enemies.Select(enemy => registry.Generals[enemy.GeneralId].Name).ToArray();
    }

    private static ContentRegistry EncounterPreviewRegistry(ShanheCampaignSession session, ShanheEncounterDefinition encounter)
    {
        var probe = new ShanheBattleSetup(83, 1, session.Definition.StartingNodeIds[0], "classic:guan-yu", 4,
            0, 0, 0, 0, [], [], [], encounter.Enemies, encounter.Allies ?? [], session.ContentHash)
            { BossTransition = encounter.BossTransition };
        return StandardShanheCampaignCatalog.CreateBattleRegistry(probe);
    }

    private static void AssertEncounterEnemyVitals(ShanheCampaignSession session, ShanheEncounterDefinition encounter,
        IReadOnlyList<ShanheEncounterEnemyCard> cards)
    {
        var registry = EncounterPreviewRegistry(session, encounter);
        Program.Assert(cards.Count == encounter.Enemies.Count, "Typed preview health retains every actual enemy seat, including repeated troops.");
        for (var index = 0; index < cards.Count; index++)
        {
            var enemy = encounter.Enemies[index];
            var general = registry.Generals[enemy.GeneralId];
            var maximum = general.BaseHp + enemy.MaxHpBonus;
            var hp = enemy.Hp ?? general.InitialHp ?? maximum;
            var card = cards[index];
            var numeric = maximum > 5 || hp < 0;
            var count = numeric ? 1 : Math.Clamp(maximum, 0, 5);
            var filled = numeric ? hp > 0 ? 1 : 0 : Math.Clamp(hp, 0, count);
            var expectedImages = Enumerable.Range(0, count).Select(position =>
                "pack://siteoforigin:,,,/Assets/gallery-hp-" + (position >= count - filled ? "wu" : "empty") + ".png").ToArray();
            var color = (Color)ColorConverter.ConvertFromString(hp <= 1 ? "#EF7264" : hp == 2 ? "#EFC64E" : "#6FBD48");
            Program.Assert(card.Name == general.Name && card.VerticalName == string.Join("\n", general.Name.ToCharArray()) &&
                    card.HasHealth && card.Hp == hp && card.MaxHp == maximum && card.HealthValue == $"{hp}/{maximum}" &&
                    card.Stats.Contains($"体力 {hp}/{maximum}", StringComparison.Ordinal) &&
                    card.ShowNumericHealth == numeric && card.NumericHealthText == $"{hp}\n/\n{maximum}" &&
                    card.HealthImages.SequenceEqual(expectedImages) && card.HealthBrush is SolidColorBrush brush &&
                    brush.Color == color && brush.IsFrozen,
                $"Preview seat {index + 1}/{enemy.GeneralId} must project actual current/max health and the bounded source-style bean rail. " +
                $"Source={session.Definition.SourceDifficultyId}, encounter={encounter.Id}/{encounter.SourceBattleId}; " +
                $"expected name={general.Name}, vertical={string.Join("|", general.Name.ToCharArray())}, hp={hp}/{maximum}, " +
                $"recipeHp={enemy.Hp}, generalInitialHp={general.InitialHp}, generalBaseHp={general.BaseHp}, maxHpBonus={enemy.MaxHpBonus}, " +
                $"numeric={numeric}, beans=[{string.Join(",", expectedImages)}], color={color}; " +
                $"actual name={card.Name}, vertical={card.VerticalName.Replace('\n', '|')}, hp={card.Hp}/{card.MaxHp}, " +
                $"hasHealth={card.HasHealth}, healthValue={card.HealthValue}, numeric={card.ShowNumericHealth}/{card.NumericHealthText.Replace('\n', '|')}, " +
                $"beans=[{string.Join(",", card.HealthImages)}], color={card.HealthBrush}, frozen={card.HealthBrush.IsFrozen}, stats={card.Stats}.");
        }
    }

    private static Border AssertEncounterPortraitPresentation(FrameworkElement container, ShanheEncounterEnemyCard card)
    {
        var portrait = Program.Find<Border>(container).Single(border => border.Name == "EncounterPortrait" && ReferenceEquals(border.DataContext, card));
        var nameView = Program.Find<Viewbox>(portrait).Single(view => view.Name == "EncounterNameView");
        var name = Program.Find<TextBlock>(nameView).Single(text => text.Name == "EncounterVerticalName");
        var health = Program.Find<Border>(portrait).Single(border => border.Name == "EncounterHealthRail");
        var icons = Program.Find<ItemsControl>(health).Single(control => control.Name == "EncounterHealthIcons");
        var number = Program.Find<TextBlock>(health).Single(text => text.Name == "EncounterNumericHealth");
        var numberView = Program.Find<Viewbox>(health).Single(view => view.IsAncestorOf(number));
        Program.Assert(ReferenceEquals(portrait.Background, card.Portrait) &&
                System.Windows.Automation.AutomationProperties.GetName(portrait) == card.AccessibleName &&
                System.Windows.Automation.AutomationProperties.GetHelpText(portrait) == card.FullDetails &&
                portrait.ToolTip is ToolTip { ContentTemplate: null } &&
                name.Text == card.VerticalName && name.FontSize == 20 && name.LineHeight == 22 &&
                name.LineStackingStrategy == LineStackingStrategy.BlockLineHeight &&
                Equals(nameView.ToolTip, card.Name) && System.Windows.Automation.AutomationProperties.GetName(nameView) == card.Name &&
                health.Visibility == Visibility.Visible && !health.IsHitTestVisible &&
                System.Windows.Automation.AutomationProperties.GetName(health) == card.HealthValue &&
                icons.Items.Count == card.HealthImages.Count && icons.Items.Cast<string>().SequenceEqual(card.HealthImages) &&
                number.Text == card.NumericHealthText && numberView.Visibility == (card.ShowNumericHealth ? Visibility.Visible : Visibility.Collapsed),
            "Every preview portrait preserves its original art and complete accessible identity/rules while rendering the actual vertical name and bounded health indicators.");
        AssertInside(nameView, portrait, "The complete vertical-name view remains inside its own original portrait.");
        AssertInside(name, nameView, "The full vertical name fits its scaled name rail without clipped characters.");
        AssertInside(health, portrait, "The actual current/max health rail stays inside its own portrait.");
        AssertInside(icons, health, "Every health bean remains inside its bounded health rail.");
        var filledIcons = Program.Find<System.Windows.Shapes.Rectangle>(icons).Where(icon => icon.Name == "EncounterFilledHealthPip").ToArray();
        var emptyIcons = Program.Find<Image>(icons).Where(icon => icon.Name == "EncounterEmptyHealthPip").ToArray();
        Program.Assert(filledIcons.Length == icons.Items.Count && emptyIcons.Length == icons.Items.Count &&
                filledIcons.Zip(emptyIcons).All(pair => pair.First.OpacityMask is ImageBrush { ImageSource: BitmapSource { PixelWidth: > 0, PixelHeight: > 0 } } &&
                    pair.Second.Source is BitmapSource { PixelWidth: > 0, PixelHeight: > 0 } &&
                    (pair.First.DataContext is string path && path.EndsWith("gallery-hp-empty.png", StringComparison.Ordinal)
                        ? pair.First.Visibility == Visibility.Collapsed && pair.Second.Visibility == Visibility.Visible
                        : pair.First.Visibility == Visibility.Visible && pair.Second.Visibility == Visibility.Collapsed &&
                          ReferenceEquals(pair.First.Fill, card.HealthBrush))),
            "The native preview bean visuals decode their real source masks and empty-bean images and display the actual colored health state.");
        if (card.ShowNumericHealth) AssertInside(number, health, "High-health and wounded preview numerals are fully visible inside the original portrait.");
        var nameBounds = nameView.TransformToAncestor(portrait).TransformBounds(new Rect(nameView.RenderSize));
        var healthBounds = health.TransformToAncestor(portrait).TransformBounds(new Rect(health.RenderSize));
        Program.Assert(!nameBounds.IntersectsWith(healthBounds), "The vertical name and actual health rail must not cover one another.");
        return portrait;

        static void AssertInside(FrameworkElement element, FrameworkElement parent, string message)
        {
            var bounds = element.TransformToAncestor(parent).TransformBounds(new Rect(element.RenderSize));
            Program.Assert(element.Visibility == Visibility.Visible && bounds.Width > 0 && bounds.Height > 0 &&
                    (PresentationSource.FromVisual(element) is null || element.IsVisible) &&
                    bounds.Left >= -.7 && bounds.Top >= -.7 && bounds.Right <= parent.ActualWidth + .7 && bounds.Bottom <= parent.ActualHeight + .7,
                message + $" Element={element.Name}; bounds={bounds}; parent={parent.RenderSize}.");
        }
    }

    private static void VerifyPublishedChapterBossRewardSummary(ShanheCampaignDefinition published, string output)
    {
        var node = published.Nodes.First(candidate => candidate.ChapterNumber is 1 or 2 && candidate.BossRewards is not null);
        var bundle = node.BossRewards!;
        // Expose an existing chapter boss as a direct preview destination. Its
        // recipe, complete reward bundle, role bodies and rules stay unchanged.
        var definition = published with
        {
            StartingNodeIds = published.StartingNodeIds.Append(node.Id).Distinct(StringComparer.Ordinal).ToArray(),
            RandomStartingNode = false
        };
        var session = ShanheCampaignSession.Create(definition, new(83, "classic:guan-yu"), StandardShanheCampaignCatalog.CreateBattleRegistry);
        var store = new PreviewTrackingShanheStore();
        using var model = Create(new MemorySaveStore(), store);
        model.SelectedShanheScenario = model.ShanheScenarios.Single(option => option.Id == StandardShanheCampaignCatalog.Returning2026EditionId);
        typeof(MainViewModel).GetField("_shanheSession", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(model, session);
        using var host = new NativeResponseStatusChecks.NativeHost(model);
        model.OpenShanheCommand.Execute(null);
        var panel = (ShanheCampaignPanel)host.Window.FindName("ShanhePanel");
        var before = ShanheCampaignJson.Serialize(session.CreateCheckpoint());
        var game = Program.Engine(model);
        var ordinaryBefore = GameCheckpointJson.Serialize(game.CreateCheckpoint());
        model.PreviewShanheNodeCommand.Execute(model.ShanheMapRows.SelectMany(row => row.Nodes).Single(candidate => candidate.Id == node.Id));
        foreach (var width in new[] { 1120, 1440 })
        {
            host.Capture(output, "shanhe-encounter-chapter-boss-reward-summary", width);
            AssertEncounterRewardSummaryLayout(model, panel, host.Root);
            var summary = model.ShanheEncounterRewardSummary;
            var copper = summary.Single(item => item.Title == "基础铜币");
            Program.Assert(copper.ValueText == "× " + bundle.FixedGold &&
                    summary.Count == 1 + bundle.Groups.Count + (bundle.SkillSlots > 0 ? 1 : 0),
                "The real chapter boss previews its original fixed copper, each authored reward group and only its actual skill-slot grant.");
            AssertEncounterRewardIcon(copper, "Assets/Shanhe/Encounter/14021801.png");
            foreach (var group in bundle.Groups)
            {
                var item = summary.Single(candidate => candidate.Title == group.Name);
                var pool = group.RewardIds.Select(id => published.Rewards.Single(reward => reward.Id == id)).ToArray();
                var icon = pool.All(reward => reward.SkillId is not null) ? 3
                    : pool.All(reward => reward.StartingCards?.Count > 0 || reward.StartingEquipment?.Count > 0) ? 4
                    : pool.All(reward => reward.TacticSkillId is not null || reward.CampaignEconomy is not null || reward.Bonuses is not null ||
                        reward.OpeningCompanionPool is not null || reward.OpeningSkillRewardIds is not null) ? 2 : 0;
                Program.Assert(item.ValueText == $"基础 {group.ChoiceCount} 选一" && item.Detail.Contains("战后以实际候选为准", StringComparison.Ordinal),
                    "Each authored chapter-boss group advertises its base selection count without promising already-drawn reward identities or promoted quality.");
                AssertEncounterRewardIcon(item, $"Assets/Wushuang/rogue_award_type_icon_{icon}.png");
            }
            if (bundle.SkillSlots > 0)
            {
                var slots = summary.Single(item => item.Title == "技能槽");
                Program.Assert(slots.ValueText == "+ " + bundle.SkillSlots, "The compact skill-slot grant keeps the actual fixed source quantity.");
                AssertEncounterRewardIcon(slots, "Assets/Shanhe/2293.png");
            }
            Program.Assert(ShanheCampaignJson.Serialize(session.CreateCheckpoint()) == before && session.PendingBattleSetup is null &&
                    ReferenceEquals(Program.Engine(model), game) && GameCheckpointJson.Serialize(game.CreateCheckpoint()) == ordinaryBefore &&
                    store.WriteCount == 0 && !store.Exists,
                "Reading the real chapter-boss fixed rewards, base groups and complete rules at either width changes neither game nor save.");
        }
        model.CloseShanheEncounterPreviewCommand.Execute(null);
        Program.Assert(!model.IsShanheEncounterPreviewOpen && model.ShanheEncounterRewardSummary.Count == 0,
            "Closing the real chapter-boss preview removes its compact summaries along with the encounter.");
    }

    private static void ConcealEncounterTooltip(object sender, RoutedEventArgs args)
    {
        if (PresentationSource.FromVisual((ToolTip)sender) is HwndSource source) ShowEncounterTooltipWindow(source.Handle, 0);
    }

    [DllImport("user32.dll", EntryPoint = "ShowWindow")]
    private static extern bool ShowEncounterTooltipWindow(nint handle, int command);

    private static void AssertEncounterBackdrop(ShanheCampaignPanel panel, int variant)
    {
        var image = (Image)panel.FindName("ShanheEncounterBackdrop");
        var expected = CardGame.Wpf.Resources.AssetImages.Load($"Assets/Shanhe/Encounter/roguelike_fight_bg{variant}.png");
        Program.Assert(image.Visibility == Visibility.Visible && image.Source is BitmapSource && expected is not null,
            "A regular battle and a boss encounter each display their own decoded original encounter background.");
        var actual = (BitmapSource)image.Source;
        Program.Assert(actual.PixelWidth == expected!.PixelWidth && actual.PixelHeight == expected.PixelHeight,
            "The selected encounter background must preserve its original source dimensions.");
        var stride = actual.PixelWidth * 4;
        var pixels = new byte[stride * actual.PixelHeight];
        var expectedPixels = new byte[pixels.Length];
        new FormatConvertedBitmap(actual, PixelFormats.Bgra32, null, 0).CopyPixels(pixels, stride, 0);
        new FormatConvertedBitmap(expected, PixelFormats.Bgra32, null, 0).CopyPixels(expectedPixels, stride, 0);
        Program.Assert(pixels.AsSpan().SequenceEqual(expectedPixels),
            "The preview backdrop selects the actual battle-node kind's original background without repainting or substituting another variant.");
    }

    public static void InheritanceOffersBeforeFirstMapAndSurvivesColdRestore(string output)
    {
        var store = new MemoryShanheCampaignSaveStore();
        var codex = new MemoryShanheCodexStore();
        codex.Write(new() { UnlockedRewardIds = [StandardShanheCampaignCatalog.OpeningInheritanceRewardId],
            EquippedTacticIds = [StandardShanheCampaignCatalog.OpeningInheritanceRewardId, null, null, null, null, null] });
        using var vm = new MainViewModel(false, 270019, true, new MemorySaveStore(), useExpandedContent: true,
            shanheSaveStore: store, shanheCodexStore: codex) { IsMotionEnabled = false };
        var panel = new ShanheCampaignPanel { DataContext = vm };
        try
        {
            vm.OpenShanheCommand.Execute(null);
            vm.StartShanheCommand.Execute(null);
            Layout(panel);
            var before = ShanheCampaignJsonText(store);
            Program.Assert(vm.IsShanheRunInProgress && vm.IsShanheRewardOpen && !vm.CanPrepareShanheExpedition &&
                vm.ShanheRewardNotice.Contains("传承") && vm.ShanheRewardCards.Count == 3 &&
                vm.ShanheRewardCards.All(card => !card.IsSkip) && vm.ShanheMapNodes.All(node => !node.IsAvailable) &&
                store.Read().Campaign.Commands.Count == 0,
                "Equipping inheritance must present its three real rare skills before any map action and prevent skipping or restarting.");
            var objective = (TextBlock)panel.FindName("ShanheChapterObjective");
            Program.Assert(vm.ShanheRewardNotice == $"传承：从{vm.ShanheRewardCards.Count}项稀有技能中选择1项" &&
                    objective.Text == vm.ShanheRewardNotice && vm.ShanheChapterObjectiveText == objective.Text &&
                    !objective.Text.Contains("放弃", StringComparison.Ordinal),
                "The inheritance selector and bound chapter objective show the actual frozen offer count and the same required initial choice.");
            var offers = vm.ShanheRewardCards.Select(card => card.Id).ToArray();
            using (var cold = new MainViewModel(false, 270019, true, new MemorySaveStore(), useExpandedContent: true,
                shanheSaveStore: store, shanheCodexStore: codex))
            {
                cold.ContinueShanheCommand.Execute(null);
                Program.Assert(cold.IsShanheRewardOpen && cold.ShanheRewardCards.Select(card => card.Id).SequenceEqual(offers) &&
                    ShanheCampaignJsonText(store) == before,
                    "Cold restoration must reopen the same inheritance candidates without redrawing or writing the save.");
            }
            var selected = vm.ShanheRewardCards[0];
            Click(Bound(panel, vm.SelectShanheRewardCardCommand, selected), panel);
            Program.Assert(ShanheCampaignJsonText(store) == before && vm.ShanheExtraSkillCards.Count == 0,
                "Selecting an inheritance card must not grant it before confirmation.");
            Program.Render(panel, 1200, 750, Path.Combine(output, "shanhe-inheritance-choice.png"));
            Click((Button)panel.FindName("ShanheConfirmRewardButton"), panel);
            var saved = store.Read().Campaign;
            var expectedSkill = StandardShanheCampaignCatalog.CreateCodexDefinition(1,
                StandardShanheCampaignCatalog.Returning2026EditionId).Rewards.Single(reward => reward.Id == selected.Id).SkillId;
            Program.Assert(!vm.IsShanheRewardOpen && vm.ShanheExtraSkillCards.Single().Id == expectedSkill &&
                saved.Commands.Count == 1 && saved.Commands[0] is CardGame.Core.Campaigns.ChooseShanheOpeningSkillCommand &&
                vm.ShanheMapRows.SelectMany(row => row.Nodes).Count(node => node.IsAvailable) == 2,
                "Confirming inheritance must grant exactly the chosen skill then unlock both untouched first-chapter battle locations.");
            Program.Assert(objective.Text == vm.ShanheChapterObjectiveText &&
                    !objective.Text.Contains("传承", StringComparison.Ordinal),
                "The existing inheritance confirmation immediately replaces its completed draft instruction with the current map objective.");
            var committed = ShanheCampaignJsonText(store);
            vm.SelectShanheRewardCardCommand.Execute(selected);
            vm.ConfirmShanheRewardCommand.Execute(null);
            Program.Assert(ShanheCampaignJsonText(store) == committed, "A stale inheritance card cannot grant a second skill.");
        }
        finally { panel.DataContext = null; }
    }

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

    private static void VerifyFengyiPreviewAndFiveSeatTable(string output)
    {
        var full = StandardShanheCampaignCatalog.CreateDefinition(1, StandardShanheCampaignCatalog.Returning2026EditionId);
        var luoyang = full.Nodes.Single(node => node.Id == "luoyang");
        var encounter = luoyang.Encounters!.Single(candidate => candidate.Id == "luoyang:fengyi-s5");
        // Exercise the authored wounded boss directly. Previews now show the one
        // seeded encounter, so unrelated alternative rosters must not be merged.
        var definition = full with { StartingNodeIds = [luoyang.Id], Nodes = [luoyang with { Encounters = [encounter] }] };
        var session = ShanheCampaignSession.Create(definition, new(83, "classic:guan-yu"), StandardShanheCampaignCatalog.CreateBattleRegistry);
        var store = new PreviewTrackingShanheStore();
        using (var model = Create(new MemorySaveStore(), store))
        {
            model.SelectedShanheScenario = model.ShanheScenarios.Single(option => option.Id == StandardShanheCampaignCatalog.Returning2026EditionId);
            typeof(MainViewModel).GetField("_shanheSession", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(model, session);
            var window = new MainWindow(model); window.ApplyTemplate();
            var root = (FrameworkElement)window.Content;
            try
            {
                model.OpenShanheCommand.Execute(null); Layout(root);
                var panel = (ShanheCampaignPanel)window.FindName("ShanhePanel");
                var node = model.ShanheMapRows.SelectMany(row => row.Nodes).Single(candidate => candidate.Id == luoyang.Id);
                var ordinary = Program.Engine(model); var ordinaryCheckpoint = GameCheckpointJson.Serialize(ordinary.CreateCheckpoint());
                var journal = ShanheCampaignJson.Serialize(session.CreateCheckpoint()); var state = JsonSerializer.Serialize(session.Snapshot);
                var writes = store.WriteCount;
                Click(Bound(panel, model.PreviewShanheNodeCommand, node), root);
                var expected = "董卓·覆銮凶影  体力 10/15";
                Program.Assert(model.IsShanheEncounterPreviewOpen && model.ShanheEncounterEnemyDetail.Contains(expected, StringComparison.Ordinal) &&
                    !model.ShanheEncounterEnemyDetail.Contains("董卓·覆銮凶影  体力 15/15", StringComparison.Ordinal) &&
                    Program.Find<TextBlock>(panel).Any(text => text.Text.Contains(expected, StringComparison.Ordinal)),
                    "The real Luoyang preview binding uses General.InitialHp for the new partial-health boss instead of silently displaying full 15/15.");
                var displayedBoss = model.ShanheEncounterEnemyCards.Single(card => card.Name == "董卓·覆銮凶影");
                AssertEncounterEnemyVitals(session, encounter, model.ShanheEncounterEnemyCards);
                var portraitCaption = Program.Find<TextBlock>((ItemsControl)panel.FindName("ShanheEncounterEnemyCards")).Single(text =>
                    ReferenceEquals(text.DataContext, displayedBoss) && text.Name == "EncounterVerticalName" && text.Text == displayedBoss.VerticalName);
                var portraitHost = AssertEncounterPortraitPresentation((ItemsControl)panel.FindName("ShanheEncounterEnemyCards"), displayedBoss);
                Program.Assert(displayedBoss.Name == model.ShanheEncounterEnemyPortraitName &&
                    displayedBoss.Name == "董卓·覆銮凶影" && displayedBoss.Stats.Contains("体力 10/15") &&
                    displayedBoss is { Hp: 10, MaxHp: 15, HasHealth: true, ShowNumericHealth: true, HealthValue: "10/15" } &&
                    Program.Find<TextBlock>(portraitHost).Any(text => text.Name == "EncounterNumericHealth" && text.Text == "10\n/\n15") &&
                    model.ShanheEncounterEnemyCards.Count == encounter.Enemies.Count &&
                    portraitCaption.Parent is Viewbox { Name: "EncounterNameView" } nameHost &&
                    nameHost.ActualHeight > 0 && nameHost.ActualHeight < portraitHost.ActualHeight,
                    "Each actual enemy retains its full identity and statistics; the wounded boss renders its complete vertical name and actual 10/15 health inside the portrait.");
                Program.Assert(session.PendingBattleSetup is null && session.Revision == 0 && session.Snapshot.EncounterOrdinal == 0 &&
                    ShanheCampaignJson.Serialize(session.CreateCheckpoint()) == journal && JsonSerializer.Serialize(session.Snapshot) == state &&
                    ReferenceEquals(Program.Engine(model), ordinary) && GameCheckpointJson.Serialize(ordinary.CreateCheckpoint()) == ordinaryCheckpoint &&
                    store.WriteCount == writes && !store.Exists,
                    "Opening the actual preview neither rolls or commits an encounter, changes either command journal, replaces the ordinary engine nor attempts a save.");
                var scroll = (ScrollViewer)panel.FindName("ShanheEncounterEnemyDetailScroll");
                Program.Assert(scroll is { ScrollableHeight: > 0, Content: TextBlock detail } &&
                    detail.Text.Contains(expected, StringComparison.Ordinal) && detail.ActualHeight > scroll.ViewportHeight,
                    "The complete selected Luoyang roster remains in a scrollable native text surface without clipping the wounded boss's 10/15 statistics.");
                scroll.ScrollToBottom(); Layout(root);
                Program.Assert(scroll.VerticalOffset > 0 && Math.Abs(scroll.VerticalOffset - scroll.ScrollableHeight) < 1,
                    "The actual encounter detail control can reach the bottom of the complete source roster.");
                Program.Render(root, 1440, 880, Path.Combine(output, "shanhe-fengyi-preview-scrolled.png"));
                Click((Button)panel.FindName("ShanheEncounterCloseButton"), root);
                Program.Assert(!model.IsShanheEncounterPreviewOpen && ShanheCampaignJson.Serialize(session.CreateCheckpoint()) == journal &&
                    store.WriteCount == writes && !store.Exists,
                    "Closing the read-only native preview preserves the same unissued encounter and save boundary.");
            }
            finally { window.Content = null; window.DataContext = null; window.Close(); }
        }

        var setup = new ShanheBattleSetup(83, 1, luoyang.Id, "classic:guan-yu", 4, 0, 0, 0, 0, [], [], [],
            encounter.Enemies, encounter.Allies ?? [], ShanheCampaignContent.GetContentHash(full)) { DefaultGodFactionId = full.DefaultGodFactionId };
        var battle = StandardShanheCampaignCatalog.CreateBattle(setup);
        var accepted = battle.Submit(new StartGameCommand());
        Program.Assert(accepted.Accepted && battle.PlayerCount == 5, "The full Fengyi layout uses a genuine five-seat native encounter.");
        using var table = Create(new MemorySaveStore(), new MemoryShanheCampaignSaveStore());
        typeof(MainViewModel).GetField("_shanheTableGame", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(table, battle);
        typeof(MainViewModel).GetField("_shanheTableSetup", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(table, setup);
        typeof(MainViewModel).GetMethod("ReplaceEngine", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(table, [battle]);
        table.IsNewGameSetupOpen = false;
        var tableWindow = new MainWindow(table); tableWindow.ApplyTemplate();
        try
        {
            var root = (FrameworkElement)tableWindow.Content;
            Program.Render(root, 1440, 880, Path.Combine(output, "shanhe-fengyi-five-seat.png"));
            AssertUniqueOpponentRings(tableWindow, table, 5);
            Program.Assert(table.RightSeats.Count == 1 && table.TopSeats.Count == 2 && table.LeftSeats.Count == 1 &&
                table.Seats.Where(seat => !seat.IsHuman).Select(seat => seat.GeneralId).ToHashSet().SetEquals(
                    ["classic:sun-jian", StandardShanheS5BossPackage.DongZhuoFengyiId, StandardShanheS5BossPackage.LvbuBajiId, StandardShanheS5BossPackage.DiaoChanBiyueId]) &&
                table.Seats.Count(seat => seat.IsTeammate) == 1 && table.Seats.Single(seat => seat.GeneralId == "classic:sun-jian").IsTeammate,
                "The actual four non-human seats occupy disjoint 1/2/1 rings, including the real Sun Jian ally and all three distinct source enemies.");
            foreach (var seat in table.Seats.Where(seat => !seat.IsHuman))
                Program.Assert(Program.Find<TextBlock>(root).Any(text => ReferenceEquals(text.DataContext, seat) && text.Text == seat.VerticalName &&
                    text.Visibility == Visibility.Visible && text.ActualWidth > 0 && text.ActualHeight > 0),
                    "Every actual opponent/ally model has its own laid-out name in the real seat template: " + seat.GeneralId);
            var dong = table.Seats.Single(seat => seat.GeneralId == StandardShanheS5BossPackage.DongZhuoFengyiId);
            Program.Assert(dong.HealthValue == "10/15" && !dong.IsTeammate && dong.HasPortrait &&
                GeneralArt.NormalizeKey(dong.Portrait.GeneralId) == GeneralArt.NormalizeKey(dong.GeneralId) &&
                dong.Portrait.Image is System.Windows.Media.Imaging.BitmapSource { PixelWidth: > 0, PixelHeight: > 0 } &&
                GeneralArt.GetSkin(dong.GeneralId)!.LocalPath == "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_470.png",
                "The new boss's real seat displays wounded 10/15 and his exact native source470 card, with a decoded image rather than a transparent fallback.");
            foreach (var (generalId, portraitFile) in new[]
                {
                    (StandardShanheS5BossPackage.LvbuBajiId, "generalface_319.png"),
                    (StandardShanheS5BossPackage.DiaoChanBiyueId, "generalface_445.png")
                })
            {
                var seat = table.Seats.Single(item => item.GeneralId == generalId);
                Program.Assert(seat.HasPortrait && GeneralArt.NormalizeKey(seat.Portrait.GeneralId) == GeneralArt.NormalizeKey(generalId) &&
                    seat.Portrait.Image is System.Windows.Media.Imaging.BitmapSource { PixelWidth: > 0, PixelHeight: > 0 } &&
                    GeneralArt.GetSkin(generalId)!.LocalPath == "src/CardGame.Wpf/Assets/Shanhe/Npc/" + portraitFile,
                    "Each follower's exact source form renders its own decoded native source card: " + generalId);
            }
        }
        finally { tableWindow.Content = null; tableWindow.DataContext = null; tableWindow.Close(); }
    }

    private sealed class PreviewTrackingShanheStore : IShanheCampaignSaveStore
    {
        private readonly MemoryShanheCampaignSaveStore _inner = new();
        public int WriteCount { get; private set; }
        public bool Exists => _inner.Exists;
        public ShanheCampaignSaveFile Read() => _inner.Read();
        public void Write(ShanheCampaignSaveFile save) { WriteCount++; _inner.Write(save); }
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
