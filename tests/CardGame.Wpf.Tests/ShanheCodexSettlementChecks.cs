using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.Controls;
using CardGame.Wpf.ViewModels;

internal static class ShanheCodexSettlementChecks
{
    private const string Player = "fixture:shanhe-settlement-player";
    private const string Enemy = "fixture:shanhe-settlement-enemy";
    private const string Force = "fixture:shanhe-settlement-force";
    private static readonly Lazy<ContentRegistry> Registry = new(() => ContentRegistry.Build(new StandardContentPackage(), new StandardTeamModePackage(), new SettlementBattlePackage()));

    public static void CompletedVictoryCreditsIndependentlyOfExpeditionAutosave()
    {
        var definition = new ShanheCampaignDefinition("fixture:shanhe-settlement", "归来", [Player], ["battle"],
            [new("battle", "渡口守军", ShanheNodeKind.Battle, [], [new("guard", "守军", [new(Enemy)])])], []);
        foreach (var accountInitiallyUnavailable in new[] { false, true })
        {
            var options = new ShanheCampaignOptions(71, Player)
                { UseCodexRewardPool = true, ExpeditionId = Guid.NewGuid().ToString("N") };
            var session = ShanheCampaignSession.Create(definition, options, _ => Registry.Value);
            var progress = new FailingCampaignStore();
            var account = new FailingCodexStore();
            account.Write(new());
            using var vm = new MainViewModel(false, 71, true, new MemorySaveStore(), contentRegistry: Registry.Value,
                shanheSaveStore: progress, shanheCodexStore: account);
            Attach(vm, session);
            vm.RetryShanheSettlementCommand.Execute(null);
            Program.Assert(!vm.RetryShanheSettlementCommand.CanExecute(null) && account.Read().Books == 0 &&
                account.Read().RunRecords.Length == 0 && progress.WriteAttempts == 0,
                "An unfinished expedition must not grant books or a terminal record, even when its settlement command is invoked directly.");
            account.FailWrites = accountInitiallyUnavailable;
            Accept(session, new MoveShanheNodeCommand("battle", session.Revision));
            var battle = CreateCompletedBattle(session, "victory");
            var balanceNotifications = 0;
            vm.PropertyChanged += (_, args) =>
            {
                if (args.PropertyName == nameof(MainViewModel.ShanheBookBalance)) balanceNotifications++;
            };
            CaptureCompletedBattle(vm, session, battle);
            Program.Assert(session.Snapshot.Phase == ShanheCampaignPhase.Won && progress.WriteAttempts > 0 && !progress.Exists,
                "The native victory must pass campaign finalization while its independently failing expedition journal remains unwritten.");
            var journalAttempts = progress.WriteAttempts;
            if (accountInitiallyUnavailable)
            {
                Program.Assert(vm.IsShanheSettlementPending && vm.RetryShanheSettlementCommand.CanExecute(null) &&
                    vm.ShanheBookBalance == 0 && vm.ShanheRunRecords.Count == 0 && account.Read().ClaimedRewards.Length == 0,
                    "A failed account write must leave its ten books and run record pending without publishing an unpaid balance.");
                account.FailWrites = false;
                vm.RetryShanheSettlementCommand.Execute(null);
            }
            var credited = account.Read();
            Program.Assert(vm.ShanheBookBalance == 10 && credited.Books == 10 && balanceNotifications > 0 &&
                credited.ClaimedRewards.SequenceEqual([ShanheCodexSettlement.GetSettlementKey(session)!]) &&
                credited.RunRecords is [{ Won: true, BooksEarned: 10, Battles: 1 }] &&
                vm.ShanheRunRecords is [{ Record.Won: true, Record.BooksEarned: 10 }] &&
                vm.HasShanheRunRecords && !vm.IsShanheSettlementPending && !vm.RetryShanheSettlementCommand.CanExecute(null) &&
                progress.WriteAttempts == journalAttempts,
                "Completion or an account-only retry must persist ten books and one visible real victory record despite an unavailable expedition store.");
            vm.RetryShanheSettlementCommand.Execute(null);
            Program.Assert(account.Read().Books == 10 && account.Read().ClaimedRewards.Length == 1 &&
                account.Read().RunRecords.Length == 1 && progress.WriteAttempts == journalAttempts,
                "Repeated receipt attempts must neither duplicate the account reward nor retry the unrelated expedition journal.");
        }

        var unidentified = ShanheCampaignSession.Create(definition,
            new ShanheCampaignOptions(71, Player) { UseCodexRewardPool = true }, _ => Registry.Value);
        Accept(unidentified, new MoveShanheNodeCommand("battle", unidentified.Revision));
        var unidentifiedBattle = CreateCompletedBattle(unidentified, "victory");
        var unidentifiedAccount = new MemoryShanheCodexStore();
        unidentifiedAccount.Write(new());
        using var unidentifiedVm = new MainViewModel(false, 71, true, new MemorySaveStore(), contentRegistry: Registry.Value,
            shanheSaveStore: new FailingCampaignStore(), shanheCodexStore: unidentifiedAccount);
        CaptureCompletedBattle(unidentifiedVm, unidentified, unidentifiedBattle);
        unidentifiedVm.RetryShanheSettlementCommand.Execute(null);
        Program.Assert(unidentified.Snapshot.Phase == ShanheCampaignPhase.Won && unidentifiedVm.ShanheBookBalance == 0 &&
            unidentifiedAccount.Read().Books == 0 && unidentifiedAccount.Read().ClaimedRewards.Length == 0 &&
            unidentifiedVm.ShanheRunRecords.Count == 0 && !unidentifiedVm.IsShanheSettlementPending,
            "Even a native victory cannot create an account receipt without the expedition identity required to prevent forged or duplicate credit.");
    }

    private static void CaptureCompletedBattle(MainViewModel vm, ShanheCampaignSession session, GameEngine battle)
    {
        Attach(vm, session);
        typeof(MainViewModel).GetField("_shanheBattleGame", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(vm, battle);
        typeof(MainViewModel).GetMethod("ReplaceEngine", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(vm, [battle]);
        typeof(MainViewModel).GetMethod("CaptureShanheBattleResult", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(vm, null);
    }

    public static void ActualCompletedVictoryCreditsOnceAndRejectsOtherOutcomes(string output)
    {
        var expeditionId = Guid.ParseExact("5fcd86829f1d44149baedee006708289", "N");
        var options = new ShanheCampaignOptions(71, Player) { UseCodexRewardPool = true, ExpeditionId = expeditionId.ToString("N") };
        var definition = new ShanheCampaignDefinition("fixture:shanhe-settlement", "归来", [Player], ["battle"],
            [new("battle", "渡口守军", ShanheNodeKind.Battle, [], [new("guard", "守军", [new(Enemy)])])], []);
        var session = ShanheCampaignSession.Create(definition, options, _ => Registry.Value);
        var profile = new ShanheCodexProfile
        {
            Books = 7, ClaimedRewards = ["earlier-claim"], UnlockedRewardIds = ["known"],
            EquippedTacticIds = ["known", null, null, null, null, null]
        };
        Refuse(profile, session, 1);
        Accept(session, new MoveShanheNodeCommand("battle", session.Revision));
        Refuse(profile, session, 1);
        CompleteBattle(session, "victory");
        Program.Assert(session.Snapshot.Phase == ShanheCampaignPhase.Won &&
            ShanheCodexSettlement.EligibleBookAmount(profile, session, 1) == 10,
            "Only a fully resolved real expedition victory may prepare the verified ten-book sample award.");
        Program.Assert(ShanheCodexSettlement.TryPrepare(profile, session, 1, out var earned), "The unchanged source profile must be eligible before persistence.");
        var plansProfile = ShanheCodexLoadouts.EnsurePlans(profile);
        Program.Assert(ShanheCodexSettlement.TryPrepare(plansProfile, session, 1, out var plansEarned) &&
            plansEarned.FormatVersion == FileShanheCodexStore.CurrentFormatVersion && plansEarned.Books == earned.Books &&
            plansEarned.SelectedLoadoutPlanId == plansProfile.SelectedLoadoutPlanId &&
            plansEarned.EquippedTacticIds.SequenceEqual(plansProfile.EquippedTacticIds) &&
            plansEarned.LoadoutPlans!.Single().EquippedTacticIds.SequenceEqual(plansProfile.LoadoutPlans!.Single().EquippedTacticIds) &&
            !ReferenceEquals(plansEarned.LoadoutPlans, plansProfile.LoadoutPlans) &&
            !ReferenceEquals(plansEarned.LoadoutPlans!.Single().EquippedTacticIds, plansProfile.LoadoutPlans!.Single().EquippedTacticIds),
            "A current-format account earns the same real victory reward and retains independently copied plans and slot positions.");
        var plansStore = new MemoryShanheCodexStore();
        plansStore.Write(plansEarned);
        Refuse(plansStore.Read(), session, 1);
        Program.Assert(earned.Books == 17 && earned.ClaimedRewards.SequenceEqual(["earlier-claim", "expedition:" + expeditionId.ToString("N")]) &&
            profile.Books == 7 && profile.ClaimedRewards.SequenceEqual(["earlier-claim"]) &&
            earned.UnlockedRewardIds.SequenceEqual(profile.UnlockedRewardIds) && earned.EquippedTacticIds.SequenceEqual(profile.EquippedTacticIds) &&
            !ReferenceEquals(earned.UnlockedRewardIds, profile.UnlockedRewardIds) && !ReferenceEquals(earned.EquippedTacticIds, profile.EquippedTacticIds),
            "Preparation must detach the new profile, preserve unrelated research/loadout/claims and leave the original balance untouched.");
        Program.Assert(earned.RunRecords is [{ Won: true, Difficulty: 1, BooksEarned: 10, Battles: 1 }] &&
            earned.RunRecords[0].ExpeditionKey == "expedition:" + expeditionId.ToString("N") &&
            earned.RunRecords[0].CampaignId == definition.Id && earned.RunRecords[0].PlayerGeneralId == Player &&
            profile.RunRecords.Length == 0,
            "The same settled victory stores its actual campaign, general, difficulty, battle count and ten-book receipt independently of the next campaign save.");
        var store = new MemoryShanheCodexStore();
        store.Write(earned);
        var coldSession = ShanheCampaignSession.Restore(ShanheCampaignJson.Deserialize(ShanheCampaignJson.Serialize(session.CreateCheckpoint())), definition, _ => Registry.Value);
        Refuse(store.Read(), coldSession, 1);
        Program.Assert(ShanheCodexSettlement.ConfirmedBookAmount(coldSession, 1) == 10 && ShanheCodexSettlement.HasClaimed(store.Read(), coldSession) &&
            ShanheCodexSettlement.GetSettlementKey(coldSession) == "expedition:" + expeditionId.ToString("N"),
            "The completion view must retain the confirmed award and claimed status after persistence without offering a second credit.");
        var sameIdentityDifferentFormat = ShanheCampaignSession.Replay(definition, options with { ExpeditionId = expeditionId.ToString("D").ToUpperInvariant() }, session.AcceptedCommands, _ => Registry.Value);
        Refuse(store.Read(), sameIdentityDifferentFormat, 1);
        foreach (var difficulty in new[] { 2, 3 })
        {
            Program.Assert(ShanheCodexSettlement.TryPrepare(profile, session, difficulty, out var localTier) &&
                localTier.Books == 17 && localTier.RunRecords is [{ Won: true, BooksEarned: 10 }] &&
                localTier.RunRecords[0].Difficulty == difficulty,
                "Every current local difficulty awards its explicitly local ten-book completion floor and records the selected tier.");
            var localStore = new MemoryShanheCodexStore(); localStore.Write(localTier);
            Program.Assert(localStore.Read().RunRecords.Single().Difficulty == difficulty,
                "The higher-tier receipt is written and read with its actual difficulty.");
        }
        foreach (var difficulty in new[] { 0, 4 }) Refuse(profile, session, difficulty);
        foreach (var invalid in new[]
        {
            options with { UseCodexRewardPool = false }, options with { ExpeditionId = null },
            options with { ExpeditionId = "not-an-expedition-id" }, options with { ExpeditionId = Guid.Empty.ToString("N") }
        }) Refuse(profile, ShanheCampaignSession.Replay(definition, invalid, session.AcceptedCommands, _ => Registry.Value), 1);
        Refuse(profile with { Books = int.MaxValue }, session, 1);
        VerifyVmSaveOrderingColdReceiptAndActualPanel(session, definition, output);
        VerifyAccountFailureBlocksReplacementAndKeepsRecord(session);

        var loss = ShanheCampaignSession.Create(definition, options with { ExpeditionId = Guid.NewGuid().ToString("N") }, _ => Registry.Value);
        Accept(loss, new MoveShanheNodeCommand("battle", loss.Revision));
        CompleteBattle(loss, "surrender");
        Program.Assert(loss.Snapshot.Phase == ShanheCampaignPhase.Lost, "The rejected loss case must also come from a genuine completed battle.");
        var preparedLoss = ShanheCodexSettlement.TryPrepare(profile, loss, 1, out var lost);
        Program.Assert(ShanheCodexSettlement.EligibleBookAmount(profile, loss, 1) == 0 &&
            preparedLoss && lost.Books == profile.Books &&
            lost.RunRecords is [{ Won: false, BooksEarned: 0 }],
            "A genuine loss grants no books but retains a durable expedition record instead of disappearing.");
        Refuse(lost, loss, 1);
        var both = new MemoryShanheCodexStore(); both.Write(lost);
        Program.Assert(ShanheCodexSettlement.TryPrepare(both.Read(), session, 1, out var afterLoss) &&
            afterLoss.RunRecords.Length == 2 && afterLoss.RunRecords[0].Won && !afterLoss.RunRecords[1].Won,
            "A later complete expedition appends its own settlement without replacing the previous outcome.");
        both.Write(afterLoss);
        var plans = ShanheCodexLoadouts.Rename(both.Read(), "default", "通关构筑");
        Program.Assert(plans.RunRecords.SequenceEqual(afterLoss.RunRecords) && !ReferenceEquals(plans.RunRecords, afterLoss.RunRecords),
            "Editing a later loadout keeps detached completed-run records intact.");
        var older = Enumerable.Range(0, ShanheCodexSettlement.MaximumRunRecords)
            .Select(index => earned.RunRecords[0] with { ExpeditionKey = "expedition:" + index.ToString("x32") }).ToArray();
        var fullHistory = profile with { RunRecords = older, ClaimedRewards = older.Select(record => record.ExpeditionKey).ToArray() };
        Program.Assert(ShanheCodexSettlement.TryPrepare(fullHistory, session, 1, out var trimmed) &&
            trimmed.RunRecords.Length == ShanheCodexSettlement.MaximumRunRecords &&
            trimmed.RunRecords[0].ExpeditionKey == ShanheCodexSettlement.GetSettlementKey(session) &&
            !trimmed.RunRecords.Any(record => record.ExpeditionKey == older[^1].ExpeditionKey) &&
            trimmed.ClaimedRewards.Contains(older[^1].ExpeditionKey, StringComparer.Ordinal) && fullHistory.RunRecords.Length == older.Length,
            "The recent history is bounded without evicting its durable duplicate-settlement receipts or mutating the source history.");
        both.Write(trimmed);
    }

    private static void VerifyAccountFailureBlocksReplacementAndKeepsRecord(ShanheCampaignSession won)
    {
        var progress = new FailingCampaignStore { FailWrites = false };
        var account = new FailingCodexStore();
        account.Write(new() { Books = 7 }); account.FailWrites = true;
        using var vm = new MainViewModel(false, 71, true, new MemorySaveStore(), contentRegistry: Registry.Value,
            shanheSaveStore: progress, shanheCodexStore: account);
        Attach(vm, won);
        Program.Assert(vm.FlushShanheProgress() && vm.IsShanheSettlementPending && account.Read().Books == 7 &&
            account.Read().RunRecords.Length == 0 && vm.ShanheRunRecords.Count == 0,
            "A successful campaign journal write followed by an account failure does not publish unsaved books or records.");
        vm.StartShanheCommand.Execute(null);
        Program.Assert(ReferenceEquals(typeof(MainViewModel).GetField("_shanheSession", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(vm), won) &&
            vm.ShanheStatusText.Contains("尚未保存", StringComparison.Ordinal),
            "Starting another expedition cannot discard an unpaid terminal record when the account store is unavailable.");
        account.FailWrites = false;
        Program.Assert(vm.FlushShanheProgress() && !vm.IsShanheSettlementPending && vm.ShanheBookBalance == 17 &&
            vm.ShanheRunRecords is [{ Record.Won: true, Record.BooksEarned: 10 }] && vm.HasShanheRunRecords,
            "Retrying the same terminal journal commits both the books and its visible history record exactly once.");
        vm.StartShanheCommand.Execute(null);
        Program.Assert(!ReferenceEquals(typeof(MainViewModel).GetField("_shanheSession", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(vm), won) &&
            account.Read().RunRecords.Length == 1 && vm.ShanheRunRecords.Count == 1 && vm.ShanheBookBalance == 17,
            "Replacing the active expedition after settlement leaves its completed-run history and book balance available.");
    }

    private static void VerifyVmSaveOrderingColdReceiptAndActualPanel(ShanheCampaignSession won,
        ShanheCampaignDefinition definition, string output)
    {
        var progress = new FailingCampaignStore();
        var account = new MemoryShanheCodexStore();
        account.Write(new() { Books = 7 });
        using (var vm = CreateVm())
        {
            Attach(vm, won);
            var panel = new ShanheCampaignPanel { DataContext = vm };
            try
            {
                Layout(panel);
                Program.Assert(!vm.FlushShanheProgress() && vm.ShanheBookBalance == 7 && account.Read().Books == 7 &&
                    account.Read().ClaimedRewards.Length == 0 && vm.IsShanheSettlementOpen,
                    "A failed expedition journal write must leave both the account profile and visible balance uncredited.");
                var attempts = progress.WriteAttempts;
                var host = (ShanheSettlementPanel)panel.FindName("ShanheSettlementHost");
                Click((Button)host.FindName("SettlementCloseButton"));
                Layout(panel);
                Program.Assert(!vm.IsShanheSettlementOpen && host.Visibility == Visibility.Collapsed &&
                    progress.WriteAttempts == attempts && account.Read().Books == 7 && account.Read().ClaimedRewards.Length == 0,
                    "Closing an unpaid settlement must dismiss only the overlay, without attempting a save or granting books.");
                progress.FailWrites = false;
                Program.Assert(vm.FlushShanheProgress() && vm.ShanheBookBalance == 17 && account.Read().Books == 17 &&
                    account.Read().ClaimedRewards.SequenceEqual([ShanheCodexSettlement.GetSettlementKey(won)!]) &&
                    vm.ShanheRunRecords.Count == 1 && account.Read().RunRecords.Length == 1,
                    "Retrying after the expedition store recovers must persist exactly one ten-book credit and its expedition claim.");
                Program.Assert(vm.FlushShanheProgress() && account.Read().Books == 17 && account.Read().ClaimedRewards.Length == 1,
                    "Further ordinary flushes must not award the completed expedition again.");
            }
            finally { panel.DataContext = null; }
        }

        var coldSession = ShanheCampaignSession.Restore(progress.Read().Campaign, definition, _ => Registry.Value);
        using (var cold = CreateVm())
        {
            Attach(cold, coldSession);
            Program.Assert(cold.ShanheBookBalance == 17 && cold.FlushShanheProgress() && account.Read().Books == 17 &&
                account.Read().ClaimedRewards.Length == 1,
                "A cold VM sharing the persisted account must keep the restored expedition's receipt without issuing another credit.");
            var panel = new ShanheCampaignPanel { DataContext = cold };
            try
            {
                Layout(panel);
                var host = (ShanheSettlementPanel)panel.FindName("ShanheSettlementHost");
                Program.Assert(host.Visibility == Visibility.Visible && cold.IsShanheSettlementOpen && cold.HasShanheBookReward &&
                    Program.Find<TextBlock>(host).Any(text => text.Text == "× 10") &&
                    Program.Find<TextBlock>(host).Any(text => text.Text.Contains("图册已存入山河册", StringComparison.Ordinal)),
                    "The real settlement overlay must show the confirmed ten-book reward and the saved receipt after a cold restore.");
                Program.Render(panel, 1440, 880, Path.Combine(output, "shanhe-victory-settlement.png"));
                var attempts = progress.WriteAttempts;
                Click((Button)host.FindName("SettlementCloseButton"));
                Layout(panel);
                Program.Assert(host.Visibility == Visibility.Collapsed && !cold.IsShanheSettlementOpen &&
                    progress.WriteAttempts == attempts && account.Read().Books == 17 && account.Read().ClaimedRewards.Length == 1,
                    "Closing a paid receipt must only hide its UI and preserve the persisted balance and unique claim.");
            }
            finally { panel.DataContext = null; }
        }

        MainViewModel CreateVm() => new(false, 71, true, new MemorySaveStore(), contentRegistry: Registry.Value,
            shanheSaveStore: progress, shanheCodexStore: account);
    }

    private static void Attach(MainViewModel vm, ShanheCampaignSession session)
    {
        typeof(MainViewModel).GetField("_shanheSession", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(vm, session);
        typeof(MainViewModel).GetField("_shanheRunDifficulty", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(vm, 1);
        typeof(MainViewModel).GetMethod("RefreshShanhePresentation", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(vm, null);
    }
    private static void Layout(FrameworkElement panel)
    {
        panel.Measure(new Size(1440, 880)); panel.Arrange(new Rect(0, 0, 1440, 880)); panel.UpdateLayout();
        panel.Dispatcher.Invoke(() => { }, DispatcherPriority.DataBind); panel.UpdateLayout();
    }
    private static void Click(Button button)
    {
        Program.Assert(button.IsEnabled && button.Command?.CanExecute(button.CommandParameter) == true,
            "The actual settlement close button must be enabled and bound to the live VM command.");
        typeof(ButtonBase).GetMethod("OnClick", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(button, null);
    }
    private sealed class FailingCampaignStore : IShanheCampaignSaveStore
    {
        private readonly MemoryShanheCampaignSaveStore _inner = new();
        public bool FailWrites { get; set; } = true;
        public int WriteAttempts { get; private set; }
        public bool Exists => _inner.Exists;
        public ShanheCampaignSaveFile Read() => _inner.Read();
        public void Write(ShanheCampaignSaveFile save)
        {
            WriteAttempts++;
            if (FailWrites) throw new IOException("Settlement fixture journal write failure.");
            _inner.Write(save);
        }
    }

    private sealed class FailingCodexStore : IShanheCodexStore
    {
        private readonly MemoryShanheCodexStore _inner = new();
        public bool FailWrites { get; set; }
        public bool Exists => _inner.Exists;
        public ShanheCodexProfile Read() => _inner.Read();
        public void Write(ShanheCodexProfile profile)
        {
            if (FailWrites) throw new IOException("Settlement fixture account write failure.");
            _inner.Write(profile);
        }
    }

    private static void Refuse(ShanheCodexProfile profile, ShanheCampaignSession session, int difficulty) =>
        Program.Assert(ShanheCodexSettlement.EligibleBookAmount(profile, session, difficulty) == 0 &&
            !ShanheCodexSettlement.TryPrepare(profile, session, difficulty, out var next) && ReferenceEquals(profile, next),
            "An ineligible or already claimed expedition must leave the complete source profile untouched.");

    private static void CompleteBattle(ShanheCampaignSession session, string action)
    {
        var battle = CreateCompletedBattle(session, action);
        Program.Assert(session.CompleteBattle(battle, session.Revision).Accepted,
            "Settlement fixture outcomes must pass ordinary battle finalization and independent campaign replay validation.");
    }

    private static GameEngine CreateCompletedBattle(ShanheCampaignSession session, string action)
    {
        var setup = session.PendingBattleSetup!;
        var battle = GameEngine.CreateStandard(new()
        {
            Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
            ShanheBattle = setup, AdvanceAfterHumanCommands = false, UseInteractiveDiscard = false,
            AiPolicyVersion = AiDecisionPolicy.CurrentVersion, MaxTurns = 8
        }, Registry.Value);
        Accept(battle, new StartGameCommand());
        for (var step = 0; step < 40 && battle.PendingDecision is not { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }; step++)
            Accept(battle, new AdvanceOneStepCommand(battle.Revision));
        Program.Assert(battle.PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }, "The bounded settlement fixture must reach a real play prompt.");
        Accept(battle, new UseProgramSkillCommand(0, Force, action, [], action == "victory" ? [1] : [], battle.Revision, battle.PendingDecision!.PromptId));
        for (var step = 0; step < 40 && battle.State.Status != EngineStatus.Completed; step++)
            Accept(battle, new AdvanceOneStepCommand(battle.Revision));
        Program.Assert(battle.State.Status == EngineStatus.Completed, "The bounded settlement fixture must reach its native battle outcome.");
        return battle;
    }
    private static void Accept(ShanheCampaignSession session, ShanheCampaignCommand command)
    {
        var result = session.Submit(command); Program.Assert(result.Accepted, result.Error ?? "Campaign fixture command rejected.");
    }
    private static void Accept(GameEngine battle, GameCommand command)
    {
        var result = battle.Submit(command); Program.Assert(result.Accepted, result.Error?.ToString() ?? "Battle fixture command rejected.");
    }

    private sealed class SettlementBattlePackage : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shanhe-settlement", new Version(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load(JsonSerializer.Serialize(new
            {
                schemaVersion = SkillProgramCatalog.RulesSchemaVersion,
                skills = new[] { new { id = Force, revision = 1, activations = new[]
                {
                    new { id = "victory", minCards = 0, maxCards = 0, minTargets = 1, maxTargets = 1, targetKind = "otherLiving", usesPerTurn = (int?)null,
                        effects = new[] { new { op = "loseHp", target = "selectedTarget", amount = 20 } } },
                    new { id = "surrender", minCards = 0, maxCards = 0, minTargets = 0, maxTargets = 0, targetKind = "anyLiving", usesPerTurn = (int?)null,
                        effects = new[] { new { op = "loseHp", target = "owner", amount = 20 } } }
                } } }
            }), JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Force] = new { name = "破阵", description = "Bounded native settlement fixture" } } }));
            builder.AddSkill(new(Force, "破阵", "fixture") { Program = catalog.Programs[Force] });
            builder.AddGeneral(new(Player, "义军先锋", "", Force, "shu"));
            builder.AddGeneral(new(Enemy, "守军", "", Force, "qun"));
            builder.AddDeck(new("fixture:shanhe-settlement-dodge", "闪牌堆", 4, 2, [new("standard:dodge", 40)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "山河结算", 2, 2, new Dictionary<string, int>(),
                "fixture:shanhe-settlement-dodge", GeneralPoolIds: [Player, Enemy], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
