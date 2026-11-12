using System.Collections;
using System.Reflection;
using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.EngineOwners;
using CardGame.Core.EngineStates;

internal static class NativeSlashQuotaChecks
{
    public static void OrdinaryAndIssuedAllowancesKeepNativeGuards()
    {
        const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = 7, PlayerCount = 4, HumanSeat = 0, HumanRole = Role.Lord,
            ModeId = Fixture.Mode, UseInteractiveSetup = true,
            AdvanceAfterHumanCommands = false, MaxTurns = 3
        }, registry);
        Accept(new StartGameCommand());
        Require(game.PendingDecision is { Kind: DecisionKind.SelectGeneral }, "The native quota fixture opens its real general choice.");
        Accept(new SelectGeneralCommand(0, Fixture.Owner, game.Revision, game.PendingDecision!.PromptId));
        for (var step = 0; game.PendingDecision is not { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }; step++)
        {
            Require(step < 64 && game.PendingDecision?.PlayerSeat != 0, "The fixed quota scene reaches only its original native Play boundary.");
            Accept(new AdvanceOneStepCommand(game.Revision));
        }
        var capabilities = (ProgramCapabilityRegistry)typeof(GameEngine).GetProperty("ProgramCapabilities", flags)!.GetValue(game)!;
        var participants = capabilities.Get<IMatchParticipantStateAccess>();
        var frames = capabilities.Get<IProgramRuntimeFrames>();
        var requests = capabilities.Get<INativeProgramSlashRequests>();
        var quota = game.GetRuntimeModule<CoreGameEngineRuleQueriesRuntime>();
        var statistics = State<CoreCurrentTurnStatisticsState>("_currentTurnStatistics");
        var progress = State<CoreActualTurnProgressState>("_actualTurnProgress");
        var actor = participants.At(0);
        var originalZones = JsonSerializer.Serialize(game.CreateCardZoneDiagnostics());
        Require(quota.GetSlashUseLimit(actor) == 1 && statistics.ReferenceToSlashCountThisTurn == 0 && Can(0, 1),
            "An ordinary native Slash has one available use without borrowing any exceptional target allowance.");
        statistics.ReferenceToSlashCountThisTurn = 1;
        Require(!Can(0, 1), "The same native actor cannot spend an exhausted ordinary Slash quota.");

        var unknownMaterials = new OpaqueMaterials();
        Require(Can(0, 1, ignoresCount: true, materials: unknownMaterials),
            "An explicit count exemption does not inspect or invent unknown material and rank arguments.");
        progress.ReferenceToPhase = TurnPhase.Draw;
        try { Require(Can(0, 1, materials: unknownMaterials), "Outside Play, the ordinary Slash count is not spent or used to derive materials."); }
        finally { progress.ReferenceToPhase = TurnPhase.Play; }
        Require(Can(1, 0, materials: unknownMaterials), "A foreign actor retains its independent native out-of-turn count permission.");

        Accept(new UseProgramSkillCommand(0, Fixture.Driver, "hold", [], [], game.Revision, game.PendingDecision!.PromptId));
        for (var step = 0; game.PendingDecision?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("option-id") == "continue") != true; step++)
        {
            Require(step < 64 && game.PendingDecision is null, "The actual activation must reach its original held option before issuing any quota fact.");
            Accept(new AdvanceOneStepCommand(game.Revision));
        }
        Require(frames.Top is ProgramSkillFrame { SkillId: Fixture.Driver }, "The true activation retains its original paused program frame.");
        var originalId = frames.Top!.Id;
        ProgramSkillFrame Current() => (ProgramSkillFrame)frames.Find(originalId)!;
        requests.Issue(Current(), 0, new(ProgramSlashRequestTargetSource.FrozenCandidates, [1], CountsTowardSlashLimit: false));
        Require(Can(0, 1, kind: CardKind.FireSlash) && !Can(0, 2, kind: CardKind.FireSlash),
            "After ordinary quota exhaustion, an actual issued no-count request permits only its original actor and requested target.");
        requests.Close(Current());
        Require(!Can(0, 1, kind: CardKind.FireSlash), "Closing that original offer removes its prospective BeforeLimit allowance.");

        var directed = game.GetRuntimeModule<CoreDirectedTurnCardPoliciesRuntime>();
        directed.GrantDirectedCardPolicy(Current(), 0, 2, [CardKind.FireSlash], DirectedTurnCardPolicyEffect.BypassSlashLimit,
            new() { RequiresEffectiveSource = false, CaptureOnUse = false, Until = [] });
        Require(Can(0, 2, kind: CardKind.FireSlash) && !Can(0, 1, kind: CardKind.FireSlash) && !Can(0, 2),
            "A real AfterLimit directed allowance restores only the named effective Slash kind and target after the request has closed.");
        var source = new CardUseEffectSource(Current().SkillId, Current().ActivationId, Current().OwnerSeat, Current().SkillInstanceId);
        var shield = new BeneficiarySuitShield(Current().Id, 2, Suit.Club,
            new(Current().SkillId, Current().ActivationId, Current().OwnerSeat, Current().SkillInstanceId));
        var shields = (List<BeneficiarySuitShield>)FixtureRuntimeAccess.Engine.GetField("_beneficiarySuitShields", flags)!.GetValue(game)!;
        var physical = ((CardZoneStore)typeof(GameEngine).GetField("_cardZones", flags)!.GetValue(game)!).CardsAt(CardLocation.Hand(0))[0];
        shields.Add(shield);
        try { Require(!Can(0, 2, ignoresCount: true, kind: CardKind.FireSlash, physical: physical), "Ignoring count still respects the actual physical material's beneficiary suit shield."); }
        finally { shields.Remove(shield); }
        directed.GrantDirectedCardPolicy(Current(), 0, 2, [CardKind.FireSlash], DirectedTurnCardPolicyEffect.ForbidTarget,
            new() { RequiresEffectiveSource = false, CaptureOnUse = false, Until = [] }, issuanceSlot: 1);
        Require(!Can(0, 2, ignoresCount: true, kind: CardKind.FireSlash),
            "An ignored count and an existing AfterLimit allowance cannot override the outer directed target prohibition.");

        var turnEffects = (TurnCardUseEffectStore)FixtureRuntimeAccess.Engine.GetField("_turnCardUseEffects", flags)!.GetValue(game)!;
        foreach (var seat in new[] { 0, 1 })
            turnEffects.GrantActionProhibition(game.State.TurnNumber, game.State.CurrentSeat, originalId, 10 + seat,
                source, [CardKind.FireSlash], [CardActionType.Use], affectedSeat: seat);
        Require(!Can(0, 1, ignoresCount: true, kind: CardKind.FireSlash, materials: unknownMaterials) &&
                !Can(1, 0, kind: CardKind.FireSlash, materials: unknownMaterials),
            "Both explicit count bypass and foreign-actor permission remain subordinate to the real outer use ban.");
        progress.ReferenceToPhase = TurnPhase.Draw;
        try { Require(!Can(0, 1, kind: CardKind.FireSlash, materials: unknownMaterials), "Out-of-Play count permission cannot bypass the same issued use prohibition."); }
        finally { progress.ReferenceToPhase = TurnPhase.Play; }
        Require(statistics.ReferenceToSlashCountThisTurn == 1 &&
                !game.Events.Any(e => e.Payload is CardUseDebitRecordedEvent) &&
                JsonSerializer.Serialize(game.CreateCardZoneDiagnostics()) == originalZones,
            "Quota reads neither debit an actual use nor move any physical entity while preserving the original unknown argument boundaries.");

        // The retained LongHandActionQueryChecks independently exercises the
        // same query's Crossbow limit with excluded card and excluded slot.
        bool Can(int actorSeat, int targetSeat, bool ignoresCount = false, CardKind kind = CardKind.Slash,
            Card? physical = null, IReadOnlyList<int>? materials = null)
        {
            var events = game.Events.Count;
            var allowed = quota.CanSpendSlashUse(participants.At(actorSeat), participants.At(targetSeat), ignoresCount,
                kind, physical, effectiveRank: null, physicalCardIds: materials, effectiveSuit: null);
            Require(game.Events.Count == events, "A native quota answer must not publish a use, payment or allowance.");
            return allowed;
        }
        T State<T>(string field) => (T)typeof(GameEngine).GetField(field, flags)!.GetValue(game)!;
        void Accept(GameCommand command)
        {
            var result = game.Submit(command);
            Require(result.Accepted, result.Error?.Message ?? "The fixed native quota setup command was rejected.");
        }
    }

    private sealed class OpaqueMaterials : IReadOnlyList<int>
    {
        public int Count => throw new InvalidOperationException("An exempt native quota read inspected unknown materials.");
        public int this[int index] => throw new InvalidOperationException("An exempt native quota read indexed unknown materials.");
        public IEnumerator<int> GetEnumerator() => throw new InvalidOperationException("An exempt native quota read enumerated unknown materials.");
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    private static void Require(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }

    private sealed class Fixture : IGameContentPackage
    {
        public const string Mode = "identity:classic-native-quota-check", Owner = "fixture:native-quota-owner", Driver = "fixture:native-quota-driver";
        public PackageManifest Manifest { get; } = new("fixture-native-quota", new Version(1, 0, 0), []);
        public void Register(IContentRegistryBuilder builder)
        {
            var definitions = new[] { new { id = Driver, revision = 1, activations = new[] { new
            {
                id = "hold", usesPerTurn = 1, minCards = 0, maxCards = 0, minTargets = 0, maxTargets = 0, targetKind = "anyLiving",
                effects = new[] { new { op = "chooseOption", target = "owner", resultBind = "hold", options = new[] { new { id = "continue" } } } }
            } } } };
            var catalog = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = definitions }),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Driver] = new { name = "原生额度准备", description = "固定原帧额度查询", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } } }));
            builder.AddSkill(new(Driver, "原生额度准备", "固定原帧额度查询") { Program = catalog.Programs[Driver], ProgramPresentation = catalog.Presentations[Driver] });
            builder.AddGeneral(new(Owner, "原生额度角色", "supporter", Driver, "wei", 8));
            var peers = Enumerable.Range(1, 3).Select(seat => $"fixture:native-quota-peer-{seat}").ToArray();
            foreach (var id in peers) builder.AddGeneral(new(id, id, "supporter", "standard:none", "wei", 8));
            builder.AddDeck(new("fixture:native-quota-deck", "固定原生额度实体", 4, 2, [])
            { PhysicalCards = Enumerable.Range(0, 64).Select(_ => new ContentDeckPhysicalCard("standard:slash", Suit.Club, 7)).ToArray() });
            builder.AddMode(new(Mode, "原生额度固定场景", 4, 4,
                new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Renegade)] = 3 },
                "fixture:native-quota-deck", GeneralCandidateCount: 4, GeneralPoolIds: [Owner, .. peers]));
        }
    }
}
