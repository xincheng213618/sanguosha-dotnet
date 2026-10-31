using System.IO;
using System.Reflection;
using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.ViewModels;

internal static class ShanheGuideTaskSettlementChecks
{
    private const string Hero = "classic:cao-cao", Enemy = "fixture:guide-task-enemy", Force = "fixture:guide-task-force";
    private static readonly Lazy<ContentRegistry> Registry = new(() => ContentRegistry.Build(new StandardContentPackage(), new Fixture()));
    private const int SourceDifficulty = 11;
    private static readonly int[] VerifiedSourceDifficulties = [1, 2, 6, 11];

    public static void NativeBattleProgressAndAtomicTaskBooksRetryCold()
    {
        var definition = new ShanheCampaignDefinition(StandardShanheCampaignCatalog.Source2026CampaignId(SourceDifficulty), "任务结算", [Hero], ["one"],
        [
            Node("one", "two", 2644, 5048), Node("two", "three", 2645, 5048), Node("three", null, 22644, 5107)
        ], []) { SourceDifficultyId = 11,
            SourceEnemyGrowth = StandardShanheSourceEnemyGrowth.Season5,
            SourceEnemyGrowthHash = StandardShanheSourceEnemyGrowth.Season5.ContentHash,
            SourceGrowthPolicy = ShanheSourceDifficultyRuntime.PolicyId };
        var options = new ShanheCampaignOptions(71, Hero)
        {
            UseCodexRewardPool = true, SourceDifficultyId = 11,
            ExpeditionId = Guid.Parse("0ab3834b-aa8e-461d-8636-6b724ab53f14").ToString("N")
        };
        foreach (var sourceDifficulty in new int?[] { 6, null })
        {
            var rejected = false;
            try { _ = ShanheCampaignSession.Create(definition, options with { SourceDifficultyId = sourceDifficulty }, _ => Registry.Value); }
            catch (ArgumentException) { rejected = true; }
            Program.Assert(rejected, "Options cannot borrow a different source difficulty from the authored campaign definition.");
        }
        var session = ShanheCampaignSession.Create(definition, options, _ => Registry.Value);
        var store = new FailingStore(); store.Write(new());
        foreach (var node in new[] { "one", "two" })
        {
            Win(session, node);
            Program.Assert(ShanheGuideTaskSettlement.TryPrepare(store.Read(), session, VerifiedSourceDifficulties, out var progress),
                "A real qualifying victory must prepare account task progress before the expedition ends.");
            store.Write(progress);
        }
        Program.Assert(store.Read().Books == 0 && store.Read().GuideTaskProgress!.Count(200352) == 2 && store.Read().ClaimedRewards.Length == 0,
            "Two real Fengxian wins retain bounded account progress without claiming the three-win reward.");
        session = ShanheCampaignSession.Restore(ShanheCampaignJson.Deserialize(ShanheCampaignJson.Serialize(session.CreateCheckpoint())),
            definition, _ => Registry.Value);
        Program.Assert(session.Snapshot.CompletedBattles.Count == 2 && session.Snapshot.PlayerCharacterId == "character:cao-cao" &&
            !ShanheGuideTaskSettlement.TryPrepare(store.Read(), session, VerifiedSourceDifficulties, out _),
            "Independent cold replay reconstructs the same completed battle evidence without repeating stored task progress.");
        Win(session, "three");
        var before = store.Read();
        foreach (var capacity in new[] { 10, 30 })
        {
            var overflow = before with { Books = int.MaxValue - capacity };
            Program.Assert(ShanheGuideTaskSettlement.HasPending(overflow, session, SourceDifficulty, VerifiedSourceDifficulties) &&
                !ShanheGuideTaskSettlement.TryPrepareCombined(overflow, session, SourceDifficulty, VerifiedSourceDifficulties, out var unchanged) && ReferenceEquals(overflow, unchanged),
                "Insufficient capacity for the combined books cannot persist either a task-only or base-only subset.");
        }
        foreach (var capacity in new[] { 1, 3 })
        {
            var full = before with { ClaimedRewards = Enumerable.Range(0, 20000 - capacity).Select(id => "fixture:claim:" + id).ToArray() };
            Program.Assert(!ShanheGuideTaskSettlement.TryPrepareCombined(full, session, SourceDifficulty, VerifiedSourceDifficulties, out var unchanged) && ReferenceEquals(full, unchanged),
                "A ledger that fits only one part of the receipt cannot partially settle the combined account value.");
        }
        Program.Assert(ShanheCodexSettlement.TryPrepare(before, session, SourceDifficulty, out var baseOnly) &&
            ShanheGuideTaskSettlement.HasPending(baseOnly, session, SourceDifficulty, VerifiedSourceDifficulties) &&
            ShanheGuideTaskSettlement.TryPrepareCombined(baseOnly, session, SourceDifficulty, VerifiedSourceDifficulties, out var lateTasks) && lateTasks.Books == 40,
            "An existing base receipt cannot hide remaining valid task progress or disable its account-only retry.");
        VerifyVmPendingTaskBlocksReplacementAndRetriesOnce(baseOnly, session);
        var next = PrepareCombined(before, session);
        Program.Assert(next.Books == 40 && next.ClaimedRewards.Length == 4 && next.RunRecords is [{ BooksEarned: 10, SourceDifficultyId: SourceDifficulty }] &&
            next.GuideTaskProgress!.Count(200352) == 3 && next.GuideTaskProgress.Count(200357) == 1 && next.GuideTaskProgress.Count(200358) == 1,
            "The native final win prepares the existing ten-book completion and three separate ten-book task rewards in one account value.");
        store.FailWrites = true;
        try { store.Write(next); throw new InvalidOperationException("The fixture account write must fail."); }
        catch (IOException) { }
        Program.Assert(store.Read().Books == 0 && store.Read().ClaimedRewards.Length == 0 && store.Read().GuideTaskProgress!.Count(200352) == 2,
            "An account write failure publishes neither books nor task claims nor their accompanying progress.");
        store.FailWrites = false; store.Write(PrepareCombined(store.Read(), session));
        session = ShanheCampaignSession.Restore(ShanheCampaignJson.Deserialize(ShanheCampaignJson.Serialize(session.CreateCheckpoint())),
            definition, _ => Registry.Value);
        Program.Assert(store.Read().Books == 40 && !ShanheCodexSettlement.TryPrepare(store.Read(), session, SourceDifficulty, out _) &&
            !ShanheGuideTaskSettlement.TryPrepare(store.Read(), session, VerifiedSourceDifficulties, out _) &&
            !ShanheGuideTaskSettlement.TryPrepareCombined(store.Read(), session, SourceDifficulty, VerifiedSourceDifficulties, out _) &&
            !ShanheGuideTaskSettlement.HasPending(store.Read(), session, SourceDifficulty, VerifiedSourceDifficulties) &&
            ShanheGuideTaskCatalog.Tasks.Where(task => task.RequiresScore).All(task => !ShanheGuideTaskSettlement.HasClaimed(store.Read(), task.SourceId)),
            "One atomic retry survives account and campaign cold recovery without duplicating any book reward or enabling unverified scores.");

        var unbound = ShanheCampaignSession.Create(definition with { Id = "fixture:guide-tasks-local", SourceDifficultyId = null,
                SourceEnemyGrowth = null, SourceEnemyGrowthHash = null, SourceGrowthPolicy = null },
            options with { SourceDifficultyId = null, ExpeditionId = Guid.Parse("1b595189-e8d5-43a7-9b05-a083226c83b9").ToString("N") }, _ => Registry.Value);
        Win(unbound, "one");
        Program.Assert(!ShanheGuideTaskSettlement.TryPrepare(new(), unbound, VerifiedSourceDifficulties, out _),
            "Real local battles without a bound original difficulty remain ineligible for source task books.");
    }

    private static void VerifyVmPendingTaskBlocksReplacementAndRetriesOnce(ShanheCodexProfile baseOnly, ShanheCampaignSession won)
    {
        var account = new FailingStore(); account.Write(baseOnly); account.FailWrites = true;
        using var vm = new MainViewModel(false, 71, true, new MemorySaveStore(), contentRegistry: Registry.Value,
            shanheSaveStore: new MemoryShanheCampaignSaveStore(), shanheCodexStore: account);
        var sessionField = typeof(MainViewModel).GetField("_shanheSession", BindingFlags.Instance | BindingFlags.NonPublic)!;
        sessionField.SetValue(vm, won);
        typeof(MainViewModel).GetField("_shanheRunDifficulty", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(vm, SourceDifficulty);
        typeof(MainViewModel).GetMethod("RefreshShanhePresentation", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(vm, null);
        Program.Assert(vm.IsShanheSettlementPending && vm.RetryShanheSettlementCommand.CanExecute(null) &&
            vm.ShanheSettlementReceipt.Contains("未保存", StringComparison.Ordinal) && !vm.ShanheSettlementReceipt.Contains("已入账", StringComparison.Ordinal),
            "The actual VM exposes remaining task settlement instead of treating its previously paid base reward as complete.");
        vm.RetryShanheSettlementCommand.Execute(null);
        Program.Assert(account.Read().Books == 10 && vm.ShanheBookBalance == 10 && vm.IsShanheSettlementPending,
            "A failed account-only task retry leaves the actual visible and persisted base balance unchanged.");
        vm.StartShanheCommand.Execute(null);
        Program.Assert(ReferenceEquals(sessionField.GetValue(vm), won) && vm.ShanheStatusText.Contains("尚未保存", StringComparison.Ordinal),
            "The real start command cannot replace authoritative task evidence while its account write is failing.");
        account.FailWrites = false;
        var writes = account.SuccessfulWrites;
        vm.RetryShanheSettlementCommand.Execute(null);
        Program.Assert(account.SuccessfulWrites == writes + 1 && account.Read().Books == 40 && account.Read().ClaimedRewards.Length == 4 &&
            vm.ShanheBookBalance == 40 && !vm.IsShanheSettlementPending && !vm.RetryShanheSettlementCommand.CanExecute(null),
            "Restoring the store lets one combined account write finish the three task rewards without repeating the existing base receipt.");
        vm.StartShanheCommand.Execute(null);
        Program.Assert(!ReferenceEquals(sessionField.GetValue(vm), won) && account.SuccessfulWrites == writes + 1 && account.Read().Books == 40,
            "After complete settlement the real start command may replace the old expedition without another account credit.");
    }

    private static ShanheCodexProfile PrepareCombined(ShanheCodexProfile before, ShanheCampaignSession session)
    {
        Program.Assert(ShanheGuideTaskSettlement.TryPrepareCombined(before, session, SourceDifficulty, VerifiedSourceDifficulties, out var combined),
            "A native terminal victory prepares its complete ordinary and task settlement for one atomic account write.");
        return combined;
    }

    private static ShanheNodeDefinition Node(string id, string? next, int battleId, int cityId) =>
        new(id, "真实任务战斗", ShanheNodeKind.Battle, next is null ? [] : [next],
            [new("encounter:" + id, "来源战斗", [new(Enemy)]) { SourceBattleId = battleId, SourceCityId = cityId }]) { ChapterNumber = 1 };

    private static void Win(ShanheCampaignSession session, string node)
    {
        var moved = session.Submit(new MoveShanheNodeCommand(node, session.Revision));
        Program.Assert(moved.Accepted, moved.Error ?? "Guide-task node move rejected.");
        var setup = session.PendingBattleSetup!;
        var battle = GameEngine.CreateStandard(new()
        {
            Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            AdvanceAfterHumanCommands = false, UseInteractiveDiscard = false, AiPolicyVersion = AiDecisionPolicy.CurrentVersion, MaxTurns = 8
        }, Registry.Value);
        Accept(battle, new StartGameCommand());
        for (var step = 0; step < 40 && battle.PendingDecision is not { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }; step++)
            Accept(battle, new AdvanceOneStepCommand(battle.Revision));
        Program.Assert(battle.PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }, "The bounded task fixture reaches a native play prompt.");
        Accept(battle, new UseProgramSkillCommand(0, Force, "victory", [], [1], battle.Revision, battle.PendingDecision!.PromptId));
        for (var step = 0; step < 40 && battle.State.Status != EngineStatus.Completed; step++) Accept(battle, new AdvanceOneStepCommand(battle.Revision));
        var completed = session.CompleteBattle(battle, session.Revision);
        Program.Assert(completed.Accepted && session.Snapshot.CompletedBattles.Last().Won,
            completed.Error ?? "Independent native battle replay must supply the task evidence.");
    }

    private static void Accept(GameEngine battle, GameCommand command)
    {
        var result = battle.Submit(command); Program.Assert(result.Accepted, result.Error?.ToString() ?? "Guide-task battle command rejected.");
    }

    private sealed class FailingStore : IShanheCodexStore
    {
        private readonly MemoryShanheCodexStore _inner = new();
        public bool FailWrites { get; set; }
        public int SuccessfulWrites { get; private set; }
        public bool Exists => _inner.Exists;
        public ShanheCodexProfile Read() => _inner.Read();
        public void Write(ShanheCodexProfile profile)
        {
            if (FailWrites) throw new IOException("Guide-task fixture account write failure.");
            _inner.Write(profile);
            SuccessfulWrites++;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shanhe-guide-tasks", new Version(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load(JsonSerializer.Serialize(new
            {
                schemaVersion = SkillProgramCatalog.RulesSchemaVersion,
                skills = new[] { new { id = Force, revision = 1, activations = new[] { new
                {
                    id = "victory", minCards = 0, maxCards = 0, minTargets = 1, maxTargets = 1,
                    targetKind = "otherLiving", usesPerTurn = (int?)null,
                    effects = new[] { new { op = "loseHp", target = "selectedTarget", amount = 20 } }
                } } } }
            }), JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Force] = new { name = "破阵", description = "Native task settlement fixture" } } }));
            builder.AddSkill(new(Force, "破阵", "fixture") { Program = catalog.Programs[Force] });
            builder.AddGeneral(new(Hero, "同名不作为证据", "", Force, "wei") { CharacterId = "character:cao-cao" });
            builder.AddGeneral(new(Enemy, "守军", "", Force, "qun"));
            builder.AddDeck(new("fixture:guide-task-deck", "闪牌堆", 4, 2, [new("standard:dodge", 40)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "章节任务", 2, 2, new Dictionary<string, int>(),
                "fixture:guide-task-deck", GeneralPoolIds: [Hero, Enemy], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
