using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Wpf;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.ViewModels;

internal static class ShanheOpeningPerformanceChecks
{
    private sealed record Measurement(string Stage, int Iterations, double ElapsedMilliseconds, long AllocatedBytes);

    public static void StagedBattleAndPendingHand(string output)
    {
        const string playerId = "ol:mo-zhang-fei";
        var measurements = new List<Measurement>();
        var observations = new Dictionary<string, object?>();
        var jsonPath = Path.Combine(output, "shanhe-opening-performance.json");
        MainWindow? window = null;
        try
        {
            var enemy = new ShanheCombatantDefinition(StandardShanheSourceWhiteHorsePackage.BaseId, MaxHpBonus: 11);
            var ally = new ShanheCombatantDefinition(StandardShanheCompanionPackage.ManzhuId);
            var catalogue = Measure("fixture.catalogue", () => StandardShanheCampaignCatalog.CreateCodexDefinition(2,
                StandardShanheCampaignCatalog.Returning2026EditionId));
            Measure("fixture.catalogue.warm", () => StandardShanheCampaignCatalog.CreateCodexDefinition(2,
                StandardShanheCampaignCatalog.Returning2026EditionId), 3);
            Measure("fixture.player_roster_registry.warm", () => StandardShanheCampaignCatalog.PlayerRosterRegistry, 20);
            var dragon = catalogue.Rewards.Single(reward => reward.TacticSkillId == StandardShanheDrawThresholdPackage.ShenlongBaiwei2SkillId);
            var tacticIds = new[] { dragon.Id, "tactic-economy-2089", "tactic-economy-2090", "tactic-economy-2091", "tactic-economy-2107", "tactic-economy-2092" };
            var shop = catalogue.Nodes.First(node => node.Kind == ShanheNodeKind.Shop && node.ChapterNumber == 1) with
            {
                NextNodeIds = [], ChapterRouteId = null, ChapterRole = ShanheChapterNodeRole.None,
                ChapterProgressStep = 0, RequiredChapterProgress = 0, ExtraChapterBattleIndex = 0
            };
            // Keep every authored reward, school and shop policy so live market projection has its production workload.
            // This route-free two-node fixture makes the market available without completing or restoring a battle.
            var definition = catalogue with
            {
                Id = "fixture:shanhe-opening-performance", Name = "开局分段计时", PlayerGeneralIds = [playerId],
                StartingNodeIds = ["battle"], RandomStartingNode = false, DefaultGodFactionId = "shu",
                Nodes = [new("battle", "白马义从", ShanheNodeKind.Battle, [shop.Id],
                    [new("white-horse", "白马义从", [enemy], [ally])]) { ChapterNumber = 1 }, shop]
            };
            var options = new ShanheCampaignOptions(71, playerId)
            {
                EquippedTacticIds = tacticIds, PersistentMarket = true, UseCodexRewardPool = true,
                UnlockedRewardIds = catalogue.Rewards.Select(reward => reward.Id).ToArray()
            };
            var coreSession = Measure("fixture.core_session", () => ShanheCampaignSession.Create(definition, options,
                setup => Measure("core.registry.session_initial", () => StandardShanheCampaignCatalog.CreateBattleRegistry(setup))));
            var moved = Measure("core.move_to_node", () => coreSession.Submit(new MoveShanheNodeCommand("battle", coreSession.Revision)));
            Program.Assert(moved.Accepted && coreSession.PendingBattleSetup?.MarketSetup is not null,
                "The forward fixture must produce an authored persistent live market alongside the battle setup.");
            var probe = coreSession.PendingBattleSetup!;
            // First/warm describe this probe's calls; another filtered check may already have warmed process caches.
            Measure("core.registry.first_in_probe", () => StandardShanheCampaignCatalog.CreateBattleRegistry(probe));
            var registry = Measure("core.registry.warm", () => StandardShanheCampaignCatalog.CreateBattleRegistry(probe));
            // The production CreateBattle options, with its registry stage already measured separately.
            var core = Measure("core.create_battle_constructor", () => GameEngine.CreateStandard(new GameOptions
            {
                Seed = probe.BattleSeed, PlayerCount = 1 + probe.AllyRoster.Count + probe.EnemyRoster.Count,
                HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
                ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = probe, UseInteractiveSetup = false,
                ConfirmOpeningHand = true, UseInteractiveDiscard = true, AdvanceAfterHumanCommands = false,
                MaxTurns = 150, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, registry));
            var started = Measure("core.submit_start_game", () => core.Submit(new StartGameCommand()));
            Program.Assert(started.Accepted && started.State.PendingDecision?.Kind == DecisionKind.OpeningHand,
                "The real magic Zhang Fei, Manzhu and white-horse battle must reach manual opening-hand confirmation.");
            Measure("core.player_snapshot", () => core.CreateSnapshot(0));

            var session = Measure("fixture.fresh_session", () => ShanheCampaignSession.Create(definition,
                options, StandardShanheCampaignCatalog.CreateBattleRegistry));
            using var vm = Measure("vm.constructor", () => new MainViewModel(false, 270029, true, new MemorySaveStore(),
                useExpandedContent: true, confirmOpeningHand: true, shanheSaveStore: new MemoryShanheCampaignSaveStore(),
                shanheCodexStore: new MemoryShanheCodexStore(), historyStore: new MemoryMatchHistoryStore(),
                preferencesStore: new MemoryPlayerPreferencesStore(), wushuangSaveStore: new MemorySaveStore(),
                customGeneralStore: new MemoryCustomGeneralLibraryStore(), exceptionReportStore: new ExceptionReportChecks.IgnoredReports())
                { IsMotionEnabled = false, IsSoundEnabled = false, IsMusicEnabled = false });
            typeof(MainViewModel).GetField("_shanheSession", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(vm, session);
            Stage("vm.open_shanhe", () => vm.OpenShanheCommand.Execute(null));
            var node = vm.ShanheMapRows.SelectMany(row => row.Nodes).Single(item => item.IsAvailable);
            Stage("vm.select_node_and_start_battle", () => vm.SelectShanheNodeCommand.Execute(node));
            var game = Program.Engine(vm);
            Program.Assert(vm.CanRerollOpeningHand && game.PendingDecision?.Kind == DecisionKind.OpeningHand &&
                game.State.TurnNumber == 0 && vm.HumanPlayer!.WushuangTactics.Count == 6 && game.CreateShanheMarketSnapshot(0) is not null,
                "The forward campaign fixture must retain six tactics and its real live market while awaiting the actual opening prompt.");
            window = Measure("ui.main_window_constructor", () => new MainWindow(vm));
            var root = (FrameworkElement)window.Content;
            Stage("ui.pending_hand_render", () => Program.Render(root, 1120, 740, null));

            var before = SnapshotJson.Serialize(game.CreateSnapshot(0));
            var commandsBefore = game.AcceptedCommands.Count;
            var campaignCommandsBefore = session.AcceptedCommands.Count;
            var marketProperty = typeof(MainViewModel).GetProperty("CurrentShanheMarketState", BindingFlags.Instance | BindingFlags.NonPublic)!;
            var liveProperty = typeof(MainViewModel).GetProperty("HasLiveShanheBattle", BindingFlags.Instance | BindingFlags.NonPublic)!;
            var live = Measure("vm.has_live_battle_getter", () => liveProperty.GetValue(vm), 20);
            ShanheCampaignSnapshot? stableMarket = null;
            var market = Measure("vm.current_market_state_getter", () =>
            {
                var view = (ShanheCampaignSnapshot?)marketProperty.GetValue(vm);
                if (stableMarket is null) stableMarket = view;
                else Program.Assert(ReferenceEquals(stableMarket, view),
                    "Every same-revision market getter must share the same immutable player view.");
                return view;
            }, 20);
            var getterAllocated = measurements.Where(measurement => measurement.Stage is
                "vm.has_live_battle_getter" or "vm.current_market_state_getter").Sum(measurement => measurement.AllocatedBytes);
            observations["GetterAllocatedBytes"] = getterAllocated;
            Program.Assert(getterAllocated < 2L * 1024 * 1024,
                $"Twenty reads of each battle/market getter must allocate less than 2 MiB altogether; allocated {getterAllocated} bytes.");
            observations["HasLiveBattle"] = live;
            observations["MarketProjectionAvailable"] = market is not null;
            Program.Assert(market is not null, "The measured market getter must project a real battle market rather than fall back to the expedition snapshot.");
            observations["PendingHandCount"] = vm.Hand.Count;
            observations["BattlePlayerCount"] = game.CreateSnapshot(0).Players.Count;
            Program.Assert(before == SnapshotJson.Serialize(game.CreateSnapshot(0)) && game.AcceptedCommands.Count == commandsBefore &&
                session.AcceptedCommands.Count == campaignCommandsBefore && vm.CanRerollOpeningHand,
                "Repeated read-only battle and market getters must retain the exact opening hand, prompt, revision and journals.");
            var oldHand = vm.Hand.Select(card => card.Id).ToArray();
            Stage("vm.reroll_command", () => vm.RerollOpeningHandCommand.Execute(null));
            Program.Assert(vm.CanRerollOpeningHand && game.AcceptedCommands.Count == commandsBefore + 1 &&
                vm.Hand.Count == oldHand.Length && !oldHand.SequenceEqual(vm.Hand.Select(card => card.Id)),
                "The measured reroll must exchange the real hand exactly once while retaining opening confirmation.");
            var rerolledBeforeRead = SnapshotJson.Serialize(game.CreateSnapshot(0));
            var rerolledMarket = (ShanheCampaignSnapshot?)marketProperty.GetValue(vm);
            var nativeMarket = game.CreateShanheMarketSnapshot(0);
            Program.Assert(rerolledMarket is not null && !ReferenceEquals(market, rerolledMarket) &&
                ReferenceEquals(rerolledMarket, marketProperty.GetValue(vm)) && nativeMarket is not null &&
                JsonSerializer.Serialize(rerolledMarket) == JsonSerializer.Serialize(nativeMarket) &&
                rerolledBeforeRead == SnapshotJson.Serialize(game.CreateSnapshot(0)) &&
                game.AcceptedCommands.Count == commandsBefore + 1 && session.AcceptedCommands.Count == campaignCommandsBefore &&
                vm.CanRerollOpeningHand,
                "A reroll revision must replace the cached market with an equivalent native view, then reuse it without changing the hand, prompt or journals.");
            Stage("ui.rerolled_hand_render", () => Program.Render(root, 1120, 740, null));
            Stage("vm.confirm_command", () => vm.DismissOpeningDealCommand.Execute(null));
            Program.Assert(!vm.CanRerollOpeningHand && game.AcceptedCommands.Count == commandsBefore + 2,
                "The measured confirmation must answer the real prompt once and leave manual opening setup.");
            Stage("ui.confirmed_hand_render", () => Program.Render(root, 1120, 740, null));
            observations["FinalBattleRevision"] = game.Revision;
        }
        finally
        {
            if (window is not null) { window.Content = null; window.DataContext = null; window.Close(); }
            File.WriteAllText(jsonPath, JsonSerializer.Serialize(new
            {
                Schema = 1, Fixture = "fresh battle: ol:mo-zhang-fei + Manzhu vs white-horse, six production tactics, complete production rewards and live persistent market",
                TimingNote = "Synchronous current-thread elapsed time and allocations; first registry call is first within this probe, not a process-cache guarantee.",
                Measurements = measurements, Observations = observations
            }, new JsonSerializerOptions { WriteIndented = true }));
            Console.WriteLine("Shanhe opening performance JSON: " + jsonPath);
        }

        T Measure<T>(string stage, Func<T> action, int iterations = 1)
        {
            var allocated = GC.GetAllocatedBytesForCurrentThread();
            var timestamp = Stopwatch.GetTimestamp();
            T result = default!;
            for (var index = 0; index < iterations; index++) result = action();
            var measurement = new Measurement(stage, iterations, Stopwatch.GetElapsedTime(timestamp).TotalMilliseconds,
                GC.GetAllocatedBytesForCurrentThread() - allocated);
            measurements.Add(measurement);
            Console.WriteLine($"Shanhe opening performance: {stage} x{iterations}: {measurement.ElapsedMilliseconds:F3} ms, {measurement.AllocatedBytes} bytes.");
            GC.KeepAlive(result);
            return result;
        }
        void Stage(string stage, Action action) => Measure(stage, () => { action(); return true; });
    }
}
