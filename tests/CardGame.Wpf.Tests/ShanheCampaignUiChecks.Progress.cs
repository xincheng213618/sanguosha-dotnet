using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Wpf;
using CardGame.Wpf.Controls;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.ViewModels;

internal static partial class ShanheCampaignUiChecks
{
    public static void CurrentConfigurationReplaysAndActualFailureResetsPreparation()
    {
        var store = new MemoryShanheCampaignSaveStore();
        using var original = Create(new MemorySaveStore(), store);
        original.OpenShanheCommand.Execute(null);
        original.StartShanheCommand.Execute(null);
        var saved = store.Read();
        var definition = StandardShanheCampaignCatalog.CreateDefinitionForCheckpoint(saved.Campaign, saved.Difficulty);
        var changed = saved with { Campaign = saved.Campaign with
            { CampaignHash = ShanheCampaignContent.GetContentHash(definition with { IntroText = "Earlier presentation" }) } };
        store.Write(changed);
        var source = JsonSerializer.Serialize(store.Read());
        using (var readable = Create(new MemorySaveStore(), store))
        {
            readable.ContinueShanheCommand.Execute(null);
            Program.Assert(readable.HasShanheSession && readable.IsShanheRunInProgress &&
                JsonSerializer.Serialize(store.Read()) == source,
                "A changed whole-definition fingerprint alone does not reject or rewrite valid expedition progress.");
        }
        var invalid = changed with { Campaign = changed.Campaign with
        {
            Commands = changed.Campaign.Commands.Append(new MoveShanheNodeCommand("missing-node", changed.Campaign.Revision)).ToArray(),
            Revision = changed.Campaign.Revision + 1
        } };
        store.Write(invalid);
        source = JsonSerializer.Serialize(store.Read());
        using var reset = Create(new MemorySaveStore(), store);
        reset.ContinueShanheCommand.Execute(null);
        Program.Assert(!reset.HasShanheSession && reset.CanPrepareShanheExpedition && reset.StartShanheCommand.CanExecute(null) &&
            !reset.CanContinueShanheExpedition && reset.ShanheStatusText.Contains("已重置") &&
            JsonSerializer.Serialize(store.Read()) == source,
            "An actual invalid journal resets to usable preparation, preserving the source until a new expedition is confirmed.");
    }

    public static void FailedAndAbandonedEncountersRetryFromSavedProgress(string output)
    {
        const BindingFlags hidden = BindingFlags.Instance | BindingFlags.NonPublic;
        var store = new SwitchingShanheSaveStore();
        using var vm = Create(new MemorySaveStore(), store);
        vm.SelectedShanheScenario = vm.ShanheScenarios.Single(option => option.Id == StandardShanheCampaignCatalog.LocalEditionId);
        vm.OpenShanheCommand.Execute(null);
        vm.ToggleShanheGeneralRosterCommand.Execute(null);
        vm.SelectedShanheGeneral = vm.ShanheGenerals.Single(general => general.Id == "classic:cao-cao");
        vm.StartShanheCommand.Execute(null);
        vm.SelectShanheNodeCommand.Execute(vm.ShanheMapRows.SelectMany(row => row.Nodes).First(node => node.IsAvailable));
        vm.ChooseShanheOptionCommand.Execute(vm.ShanheChoices.First(choice => choice.IsEnabled));
        vm.SelectShanheNodeCommand.Execute(vm.ShanheMapRows.SelectMany(row => row.Nodes).First(node => node.IsAvailable));
        var entry = store.Read();
        var entryJournal = ProgressJournal(entry.Campaign);
        var original = Program.Engine(vm);
        var setup = original.CreateCheckpoint().Options.ShanheBattle!;
        vm.StepAiCommand.Execute(null);
        Program.Assert(original.Revision > 1, "The saved progress test must first advance the actual battle beyond its entry.");
        vm.SaveGameCommand.Execute(null);
        Program.Assert(store.Read().Battle is null && ProgressJournal(store.Read().Campaign) == entryJournal,
            "Battle actions and a manual save retain only encounter-entry progress, with no resumable battle journal.");

        var legacy = new MemoryShanheCampaignSaveStore();
        legacy.Write(entry with { Battle = original.CreateCheckpoint() });
        var legacySource = JsonSerializer.Serialize(legacy.Read());
        using (var cold = Create(new MemorySaveStore(), legacy))
        {
            var ordinary = Program.Engine(cold);
            cold.ContinueShanheCommand.Execute(null);
            Program.Assert(cold.IsShanheBattleRetryReady && !cold.IsShanheBattleSession &&
                ReferenceEquals(Program.Engine(cold), ordinary) && JsonSerializer.Serialize(legacy.Read()) == legacySource,
                "Cold entry ignores a legacy in-flight game without executing it or rewriting the source save.");
            cold.ResumeShanheBattleCommand.Execute(null);
            Program.Assert(cold.IsShanheBattleSession && Program.Engine(cold).Revision == 1 &&
                FreshChallenge(cold) && legacy.Read().Battle is null,
                "Retry starts the saved encounter with a fresh shuffle and replaces the legacy save with progress only.");
        }

        var window = new MainWindow(vm);
        var root = (FrameworkElement)window.Content;
        var panel = (ShanheCampaignPanel)window.FindName("ShanhePanel");
        try
        {
            vm.OpenShanheCommand.Execute(null);
            Layout(root);
            store.FailWrites = true;
            Click((Button)panel.FindName("ShanheCloseButton"), root);
            Program.Assert(vm.IsShanheOpen && vm.IsShanheBattleSession && ReferenceEquals(Program.Engine(vm), original),
                "A failed progress write must block leaving and preserve the currently playable attempt.");
            store.FailWrites = false;
            Click((Button)panel.FindName("ShanheCloseButton"), root);
            Program.Assert(!vm.IsShanheOpen && !vm.IsShanheBattleSession && vm.IsNewGameSetupOpen &&
                vm.IsShanheBattleRetryReady && store.Read().Battle is null &&
                ProgressJournal(store.Read().Campaign) == entryJournal,
                "Saving and leaving returns to the lobby, discards the attempt and keeps the same expedition entry.");
            vm.ContinueShanheCommand.Execute(null);
            Layout(root);
            Click((Button)panel.FindName("ShanheResumeBattleButton"), root);
            Program.Assert(!ReferenceEquals(Program.Engine(vm), original) &&
                FreshChallenge(vm),
                "Returning from the lobby creates a new battle, rather than restoring the abandoned turn.");

            // A genuine bounded native timeout is a non-victory. It exercises the
            // same failed-attempt path without mutating HP or simulating a whole match.
            var failed = GameEngine.CreateStandard(Program.Engine(vm).CreateCheckpoint().Options with
                { MaxTurns = 1, UseInteractiveDiscard = false }, StandardShanheCampaignCatalog.CreateBattleRegistry(setup));
            AcceptBattle(new StartGameCommand());
            for (var step = 0; step < 120 && failed.State.Status != EngineStatus.Completed; step++)
                AcceptBattle(failed.PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }
                    ? new EndPlayPhaseCommand(0, failed.Revision)
                    : new AdvanceOneStepCommand(failed.Revision));
            Program.Assert(failed.State.Status == EngineStatus.Completed && failed.CreateSnapshot(0).WinnerTeamId != ShanheBattleSetup.PlayerTeamId,
                "The bounded native battle must finish without passing the encounter.");
            typeof(MainViewModel).GetField("_shanheBattleGame", hidden)!.SetValue(vm, failed);
            typeof(MainViewModel).GetMethod("AttachShanheBattle", hidden)!.Invoke(vm, [failed, failed.CreateCheckpoint().Options.ShanheBattle!]);
            typeof(MainViewModel).GetMethod("CaptureShanheBattleResult", hidden)!.Invoke(vm, null);
            Layout(root);
            Program.Assert(vm.IsShanheBattleRetryReady && vm.IsShanheRunInProgress && !vm.IsShanheSettlementOpen &&
                vm.ShanheRunRecords.Count == 0 && vm.ShanheBookBalance == 0 &&
                ProgressJournal(store.Read().Campaign) == entryJournal && store.Read().Battle is null &&
                ((Border)panel.FindName("ShanheBattleRetryNotice")).Visibility == Visibility.Visible,
                "A non-victory leaves prior progress, entry resources and rewards untouched and offers a real retry instead of ending the expedition.");
            Program.Render(root, 1440, 880, Path.Combine(output, "shanhe-progress-retry.png"));
            Click((Button)panel.FindName("ShanheResumeBattleButton"), root);
            Program.Assert(FreshChallenge(vm),
                "Retrying a failed attempt restores the same starting HP, roster and build, with a newly shuffled deck.");

            void AcceptBattle(GameCommand command)
            {
                var result = failed.Submit(command);
                Program.Assert(result.Accepted, result.Error?.Message ?? "Native progress fixture command rejected.");
            }
        }
        finally { window.Content = null; window.DataContext = null; window.Close(); }

        bool FreshChallenge(MainViewModel model)
        {
            var current = Program.Engine(model).CreateCheckpoint().Options.ShanheBattle!;
            return current.AttemptNumber > setup.AttemptNumber && current.BattleSeed != setup.BattleSeed &&
                ShanheBattleSetup.ValueEquals(current with { AttemptNumber = setup.AttemptNumber }, setup);
        }
        static string ProgressJournal(ShanheCampaignCheckpoint checkpoint) =>
            JsonSerializer.Serialize(checkpoint.Commands.Where(command => command is not BeginShanheBattleAttemptCommand));
    }

    public static void OpeningHandRerollsAndFreshRetryUseTheRealTable(string output)
    {
        var store = new SwitchingShanheSaveStore();
        using var vm = Create(new MemorySaveStore(), store, confirmOpeningHand: true);
        vm.SelectedShanheScenario = vm.ShanheScenarios.Single(option => option.Id == StandardShanheCampaignCatalog.LocalEditionId);
        vm.OpenShanheCommand.Execute(null);
        vm.ToggleShanheGeneralRosterCommand.Execute(null);
        vm.SelectedShanheGeneral = vm.ShanheGenerals.Single(general => general.Id == "classic:cao-cao");
        vm.StartShanheCommand.Execute(null);
        vm.SelectShanheNodeCommand.Execute(vm.ShanheMapRows.SelectMany(row => row.Nodes).First(node => node.IsAvailable));
        vm.ChooseShanheOptionCommand.Execute(vm.ShanheChoices.First(choice => choice.IsEnabled));
        vm.SelectShanheNodeCommand.Execute(vm.ShanheMapRows.SelectMany(row => row.Nodes).First(node => node.IsAvailable));
        var game = Program.Engine(vm);
        var setup = game.CreateCheckpoint().Options.ShanheBattle!;
        var initialHand = game.CreateSnapshot(0).Players[0].Hand.Select(card => card.Id).ToArray();
        var inventory = game.CreateCardZoneDiagnostics().Select(card => card.CardId).Order().ToArray();
        var window = new MainWindow(vm);
        var root = (FrameworkElement)window.Content;
        var panel = (ShanheCampaignPanel)window.FindName("ShanhePanel");
        try
        {
            Layout(root);
            var random = (Button)window.FindName("RerollOpeningHandButton");
            var confirm = (Button)window.FindName("ConfirmOpeningHandButton");
            var controls = (Border)window.FindName("OpeningHandControls");
            Program.Assert(vm.CanRerollOpeningHand && game.PendingDecision?.Kind == DecisionKind.OpeningHand &&
                game.CreateSnapshot(0).TurnNumber == 0 && controls.Visibility == Visibility.Visible &&
                random.Visibility == Visibility.Visible && confirm.Visibility == Visibility.Visible && !vm.CanEndTurn,
                "A real Shanhe encounter waits at its private opening hand, with the actual controls exposed before any turn or card play.");
            Click(random, root);
            Program.Assert(vm.CanRerollOpeningHand && !initialHand.SequenceEqual(game.CreateSnapshot(0).Players[0].Hand.Select(card => card.Id)) &&
                inventory.SequenceEqual(game.CreateCardZoneDiagnostics().Select(card => card.CardId).Order()),
                "The actual random button replaces the opening hand through native physical movements, retaining the whole deck.");
            var replayed = GameReplay.Restore(game.CreateCheckpoint(), StandardShanheCampaignCatalog.CreateBattleRegistry(setup));
            Program.Assert(SnapshotJson.Serialize(replayed.CreateSnapshot(0)) == SnapshotJson.Serialize(game.CreateSnapshot(0)),
                "The actual reroll command and pending confirmation replay exactly within the new challenge.");
            Program.Render(root, 1440, 880, Path.Combine(output, "shanhe-opening-hand-reroll.png"));
            Click(confirm, root);
            Program.Assert(!vm.CanRerollOpeningHand && controls.Visibility == Visibility.Collapsed && game.PendingDecision?.Kind != DecisionKind.OpeningHand,
                "Confirming the visible hand finishes setup and removes the pre-game controls.");
            vm.OpenShanheCommand.Execute(null); Layout(root);
            Click((Button)panel.FindName("ShanheCloseButton"), root);
            vm.ContinueShanheCommand.Execute(null); Layout(root);
            var ordinary = Program.Engine(vm);
            store.FailWrites = true;
            vm.ResumeShanheBattleCommand.Execute(null);
            Program.Assert(ReferenceEquals(Program.Engine(vm), ordinary) && vm.IsShanheBattleRetryReady,
                "A failed progress write cannot enter or consume a challenge behind the current view.");
            store.FailWrites = false;
            Click((Button)panel.FindName("ShanheResumeBattleButton"), root);
            var retried = Program.Engine(vm);
            var next = retried.CreateCheckpoint().Options.ShanheBattle!;
            Program.Assert(vm.CanRerollOpeningHand && next.AttemptNumber == setup.AttemptNumber + 1 &&
                next.BattleSeed != setup.BattleSeed && ShanheBattleSetup.ValueEquals(next with { AttemptNumber = setup.AttemptNumber }, setup) &&
                !initialHand.SequenceEqual(retried.CreateSnapshot(0).Players[0].Hand.Select(card => card.Id)) && store.Read().Battle is null,
                "Retry keeps the encounter and all entry resources, changes the initial shuffle, and offers opening rerolls again without saving a live game.");
        }
        finally { window.Content = null; window.DataContext = null; window.Close(); }
    }
}
