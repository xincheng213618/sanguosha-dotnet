using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Wpf;
using CardGame.Wpf.Controls;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.ViewModels;

internal static partial class ShanheCampaignUiChecks
{
    public static void EndConfirmationCancelFailureColdRestoreAndRestart(string output)
    {
        var campaign = new SwitchingShanheSaveStore();
        var ordinary = new MemorySaveStore();
        using var vm = Create(ordinary, campaign);
        vm.SelectedShanheScenario = vm.ShanheScenarios.Single(option => option.Id == StandardShanheCampaignCatalog.LocalEditionId);
        vm.OpenShanheCommand.Execute(null);
        vm.StartShanheCommand.Execute(null);
        var firstId = campaign.Read().Campaign.Options.ExpeditionId;
        var window = new MainWindow(vm);
        window.ApplyTemplate();
        var root = (FrameworkElement)window.Content;
        var panel = (ShanheCampaignPanel)window.FindName("ShanhePanel");
        try
        {
            Layout(root);
            var before = JsonSerializer.Serialize(campaign.Read());
            Click((Button)panel.FindName("ShanheEndButton"), root);
            Program.Assert(vm.IsShanheEndConfirmationOpen && vm.IsShanheRunInProgress && JsonSerializer.Serialize(campaign.Read()) == before,
                "Requesting the end must show confirmation without changing the expedition or its save.");
            Program.Render(root, 1440, 880, Path.Combine(output, "shanhe-end-confirmation.png"));
            Program.Assert(TableShortcutChecks.Input(window, Key.Escape) && !vm.IsShanheEndConfirmationOpen && vm.IsShanheOpen,
                "Escape cancels only the end confirmation and returns to the current expedition.");
            Click((Button)panel.FindName("ShanheEndButton"), root);
            Click((Button)panel.FindName("ShanheCancelEndButton"), root);
            Program.Assert(JsonSerializer.Serialize(campaign.Read()) == before && vm.IsShanheRunInProgress,
                "The actual cancel button preserves the expedition exactly.");

            // Enter a real battle, then end from the table's own action.
            vm.SelectShanheNodeCommand.Execute(vm.ShanheMapRows.SelectMany(row => row.Nodes).First(node => node.IsAvailable));
            vm.ChooseShanheOptionCommand.Execute(vm.ShanheChoices.First(choice => choice.IsEnabled));
            vm.SelectShanheNodeCommand.Execute(vm.ShanheMapRows.SelectMany(row => row.Nodes).First(node => node.IsAvailable));
            var battle = Program.Engine(vm);
            Program.Assert(vm.IsShanheBattleActive && vm.IsShanheBattleSession, "The regression must use a live native encounter.");
            Layout(root);
            Click((Button)window.FindName("EndShanheBattleButton"), root);
            before = JsonSerializer.Serialize(campaign.Read());
            campaign.FailWrites = true;
            Click((Button)panel.FindName("ShanheConfirmEndButton"), root);
            Program.Assert(vm.IsShanheEndConfirmationOpen && vm.IsShanheRunInProgress && vm.ShanheEndError.Contains("未保存") &&
                ReferenceEquals(Program.Engine(vm), battle) && JsonSerializer.Serialize(campaign.Read()) == before,
                "A failed terminal save must preserve the live battle and source file and offer retry/cancel.");
            campaign.FailWrites = false;
            Click((Button)panel.FindName("ShanheConfirmEndButton"), root);
            Program.Assert(!vm.IsShanheRunInProgress && !vm.IsShanheBattleActive && !vm.IsShanheBattleSession &&
                !vm.IsShanheEndConfirmationOpen && !vm.IsShanheSettlementOpen && vm.CanPrepareShanheExpedition &&
                !ReferenceEquals(Program.Engine(vm), battle) && campaign.Read().Battle is null &&
                campaign.Read().Campaign.Commands.Last() is EndShanheExpeditionCommand &&
                vm.ShanheRunRecords.Single().Record is { Won: false, BooksEarned: 0 },
                "Confirmed end returns to outer preparation, detaches the battle, and records one non-victory with no books.");
            Program.Render(root, 1440, 880, Path.Combine(output, "shanhe-ended-preparation.png"));
            vm.ConfirmEndShanheCommand.Execute(null);
            Program.Assert(vm.ShanheRunRecords.Count == 1, "Stale confirm must not duplicate the run record.");
            using var cold = Create(new MemorySaveStore(), campaign);
            cold.ContinueShanheCommand.Execute(null);
            Program.Assert(cold.CanPrepareShanheExpedition && !cold.IsShanheRunInProgress && !cold.IsShanheBattleActive,
                "Restarting the app must restore the terminal journal instead of resurrecting the old battle.");
            vm.StartShanheCommand.Execute(null);
            Program.Assert(vm.IsShanheRunInProgress && campaign.Read().Campaign.Options.ExpeditionId != firstId &&
                campaign.Read().Campaign.Commands.Count == 0,
                "The outer preparation module starts an independent new expedition after ending the previous one.");
        }
        finally { window.Content = null; window.DataContext = null; window.Close(); }
    }

    public static void OrdinaryModesKeepHistoryWithoutResumeSaves(string output)
    {
        var ordinary = new UnusedMatchSaveStore();
        var wushuang = new UnusedMatchSaveStore();
        var history = new MemoryMatchHistoryStore();
        using var vm = new MainViewModel(false, 270017, true, ordinary, useExpandedContent: true,
            historyStore: history, wushuangSaveStore: wushuang, shanheSaveStore: new MemoryShanheCampaignSaveStore())
            { IsMotionEnabled = false, IsSoundEnabled = false, IsMusicEnabled = false };
        var original = Program.Engine(vm);
        Program.Assert(!vm.HasAutomaticSave && !vm.HasManualSave && !vm.HasWushuangSave && !vm.HasWushuangManualSave &&
            !vm.ContinueGameCommand.CanExecute(null) && !vm.LoadManualGameCommand.CanExecute(null) &&
            !vm.ContinueWushuangCommand.CanExecute(null) && !vm.SaveGameCommand.CanExecute(null),
            "Legacy match files must not advertise restore or save actions.");
        vm.ContinueGameCommand.Execute(null); vm.LoadManualGameCommand.Execute(null);
        vm.ContinueWushuangCommand.Execute(null); vm.LoadWushuangManualGameCommand.Execute(null);
        vm.SaveGameCommand.Execute(null);
        Program.Assert(ReferenceEquals(original, Program.Engine(vm)), "Retired restore actions cannot replace a current match.");
        vm.StartNewGameCommand.Execute(null);
        Program.Assert(!ReferenceEquals(original, Program.Engine(vm)) && vm.FlushPendingSave(),
            "An ordinary mode starts a fresh match and closes without writing a resumable save.");
        vm.OpenWushuangModeCommand.Execute(null);
        vm.StartNewGameCommand.Execute(null);
        var duel = Program.Engine(vm);
        Program.Assert(vm.IsWushuangTable && vm.FlushPendingSave() && !vm.SaveGameCommand.CanExecute(null),
            "Single combat uses the same fresh-match policy without touching its old save directory.");
        vm.OpenWushuangModeCommand.Execute(null);
        vm.StartNewGameCommand.Execute(null);
        Program.Assert(!ReferenceEquals(duel, Program.Engine(vm)), "Entering single combat again must create a fresh game.");

        // A bounded completed engine verifies history capture independently of disk saves.
        var completed = GameEngine.CreateStandard(new GameOptions { Seed = 7, MaxTurns = 1, HumanRole = Role.Lord,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false },
            StandardContentRegistry.Create());
        completed.Submit(new StartGameCommand());
        for (var step = 0; step < 150 && completed.State.Status != EngineStatus.Completed; step++)
            completed.Submit(completed.PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } play
                ? new EndPlayPhaseCommand(0, completed.Revision, play.PromptId)
                : new AdvanceOneStepCommand(completed.Revision));
        Program.Assert(completed.State.Status == EngineStatus.Completed, "The history fixture must complete.");
        typeof(MainViewModel).GetMethod("ReplaceEngine", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(vm, [completed]);
        typeof(MainViewModel).GetMethod("CaptureCompletedHistory", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(vm, null);
        Program.Assert(history.Read().Entries.Count == 1 && vm.MatchHistory.Count == 1,
            "Completed game records remain available when resume saves are disabled.");
        Program.Assert(ordinary.Accesses == 0 && wushuang.Accesses == 0,
            "Starting, switching, saving, closing and history capture must not read or write retired match save stores.");
        var window = new MainWindow(vm);
        try { Program.Render((FrameworkElement)window.Content, 1440, 880, Path.Combine(output, "lobby-without-resume-saves.png")); }
        finally { window.Content = null; window.DataContext = null; window.Close(); }
    }

    private sealed class UnusedMatchSaveStore : IGameSaveStore
    {
        public int Accesses { get; private set; }
        public DateTimeOffset? GetSavedAt(GameSaveSlot slot) { Accesses++; throw new IOException("Legacy save must remain untouched."); }
        public GameSaveFile Read(GameSaveSlot slot) { Accesses++; throw new IOException("Legacy save must remain untouched."); }
        public void Write(GameSaveSlot slot, GameSaveFile save) { Accesses++; throw new IOException("Legacy save must remain untouched."); }
    }
}
