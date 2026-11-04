using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.EngineOwners;
using CardGame.Core.EngineStates;

internal static class VirtualSlashTargetChecks
{
    public static void FinalAppearanceAndCommonRestrictions()
    {
        var game = Start();
        var players = Field<IReadOnlyList<CharacterState>>(game, "_players");
        var actor = players[0];
        var target = players[1];
        var legality = game.GetRuntimeModule<CoreNativeTargetLegalityRuntime>();
        var events = game.Events.Count;
        var zones = JsonSerializer.Serialize(game.CreateCardZoneDiagnostics());
        Need(Can(CardKind.Slash) && Can(CardKind.FireSlash), "The real fixture must initially allow both Slash kinds.");

        WithPolicy("normal", () => Need(!Can(CardKind.Slash) && Can(CardKind.FireSlash),
            "A normal-Slash target prohibition must not be reused as the answer for Fire Slash."));
        WithPolicy("fire", () => Need(Can(CardKind.Slash) && !Can(CardKind.FireSlash),
            "The common target query must enforce the declared effective Slash kind."));
        var driver = actor.SkillGrants.Grants.Single(grant => grant.SkillId == "fixture:wj-driver");
        var source = new CardUseEffectSource(driver.SkillId, "target-check", actor.Seat, driver.SkillInstanceId);
        var turn = game.State.TurnNumber;
        var turnSeat = game.State.CurrentSeat;
        var effects = Field<TurnCardUseEffectStore>(game, "_turnCardUseEffects");
        var restriction = effects.GrantTargetRestriction(turn, turnSeat, 9001, 0, source, SkillProgramCardTargetRestriction.SelfOnly);
        try { Need(!Can(CardKind.Slash), "SelfOnly applies to a cardless Slash before declaration."); }
        finally { ((IList<TurnCardTargetRestriction>)effects.TargetRestrictions).Remove(restriction); }

        var directed = Field<List<DirectedTurnCardPolicy>>(game, "_directedTurnCardPolicies");
        var policy = new DirectedTurnCardPolicy(9002, turn, turnSeat, 9002, 0, source, actor.Seat, target.Seat,
            [CardKind.FireSlash], DirectedTurnCardPolicyEffect.ForbidTarget);
        directed.Add(policy);
        try { Need(Can(CardKind.Slash) && !Can(CardKind.FireSlash), "Directed policies use the actual actor, target and effective kind."); }
        finally { directed.Remove(policy); }

        var shields = Field<List<BeneficiarySuitShield>>(game, "_beneficiarySuitShields");
        WithShield(Suit.None, () => Need(Can(CardKind.Slash) && !Can(CardKind.Slash, Suit.None) && Can(CardKind.Slash, Suit.Spade),
            "Unknown suit stays unknown; an explicitly suitless declaration is a distinct known suit."));
        WithShield(Suit.Spade, () => Need(Can(CardKind.Slash) && !Can(CardKind.Slash, Suit.Spade) && Can(CardKind.Slash, Suit.None),
            "Beneficiary shielding must share the same known-suit target answer."));

        var physical = game.CreateSnapshot(0).Players[0].Hand.First(card => card.Kind == CardKind.Slash && card.Suit == Suit.Spade);
        WithShield(Suit.Spade, () => Need(legality.CanUseVirtualSlashTarget(actor, target, CardKind.Slash,
            physical.Suit, false, physical.Rank, [physical.Id], ignoreDistance: true,
            actualUnconvertedPhysical: true, countsTowardLimit: false),
            "Liubing's first physical Slash has a final Diamond appearance, so the printed Spade cannot prohibit its target."));
        WithShield(Suit.Diamond, () => Need(!legality.CanUseVirtualSlashTarget(actor, target, CardKind.Slash,
            physical.Suit, false, physical.Rank, [physical.Id], ignoreDistance: true,
            actualUnconvertedPhysical: true, countsTowardLimit: false),
            "Final appearance must be computed before the common target policy check."));
        Need(game.Events.Count == events && JsonSerializer.Serialize(game.CreateCardZoneDiagnostics()) == zones,
            "Legality queries must not issue a use, move cards or consume Liubing's first-use opportunity.");
        CheckBatchTargets();
        CheckRankRangeBranches();

        bool Can(CardKind kind, Suit? suit = null) => legality.CanUseVirtualSlashTarget(actor, target, kind,
            suit, physicalCardIds: [], ignoreDistance: true, countsTowardLimit: false);
        void WithPolicy(string name, Action check)
        {
            var id = "fixture:virtual-target-" + name;
            var grant = new SkillGrant(id, id, id, "acquired:virtual-target-check");
            target.SkillGrants.Grant(grant);
            try { check(); }
            finally { target.SkillGrants.RemoveGrant(grant.GrantId); }
        }
        void WithShield(Suit suit, Action check)
        {
            var shield = new BeneficiarySuitShield(9003, target.Seat, suit,
                new(driver.SkillId, "target-check", actor.Seat, driver.SkillInstanceId));
            shields.Add(shield);
            try { check(); }
            finally { shields.Remove(shield); }
        }

        void CheckBatchTargets()
        {
            CharacterState[] candidates = [players[3], actor, players[2], target, players[3]];
            var ordered = Match();
            Need(ordered.SequenceEqual([3, 2, 1, 3]) && ordered is IList<int> { IsReadOnly: true },
                "A native batch freezes legal occurrences in candidate order, preserving repeated seats while excluding self.");
            candidates[0] = actor;
            Need(ordered.SequenceEqual([3, 2, 1, 3]) && Match().SequenceEqual([2, 1, 3]),
                "The returned seats cannot borrow the caller's mutable candidate array.");
            candidates[0] = players[3];
            var alive = players[2].IsAlive;
            players[2].IsAlive = false;
            try
            {
                Need(Match().SequenceEqual([3, 1, 3]), "A native batch excludes a dead candidate without changing the other occurrences.");
                Need(legality.GetVirtualSlashTargetSeats(actor, [actor, players[2], actor],
                    physicalCardIds: [int.MaxValue], actualUnconvertedPhysical: true).Count == 0 &&
                    legality.GetVirtualSlashTargetSeats(actor, [], physicalCardIds: [int.MaxValue],
                        actualUnconvertedPhysical: true).Count == 0,
                    "An empty or entirely self/dead candidate domain never reads material or appearance that no target admitted.");
            }
            finally { players[2].IsAlive = alive; }

            foreach (var face in new (Suit? Suit, bool? Color, int? Rank)[]
                { (null, null, null), (Suit.None, null, 0), (Suit.Heart, true, 3), (Suit.Spade, false, 1) })
                _ = Match(suit: face.Suit, color: face.Color, rank: face.Rank, ignoreDistance: false);
            WithShield(Suit.None, () => Need(Match().Contains(target.Seat) && !Match(suit: Suit.None).Contains(target.Seat),
                "Batch admission preserves an unknown face separately from an explicitly suitless use."));
            WithPolicy("fire", () => Need(Match().Contains(target.Seat) && !Match(kind: CardKind.FireSlash).Contains(target.Seat),
                "The batched target tail retains the supplied effective Slash kind instead of reusing normal-Slash permission."));

            var conversion = new CardConversionSource(driver.SkillId, "target-check", actor.Seat, driver.SkillInstanceId);
            WithShield(Suit.Spade, () => Need(
                Match(suit: physical.Suit, color: false, rank: physical.Rank, ids: [physical.Id], actualPhysical: true).Contains(target.Seat) &&
                !Match(suit: physical.Suit, color: false, rank: physical.Rank, ids: [physical.Id],
                    conversion: conversion, actualPhysical: true).Contains(target.Seat),
                "Batch admission applies actual unconverted Liubing appearance once, while retaining an explicitly supplied conversion source."));
            WithShield(Suit.Diamond, () => Need(
                !Match(suit: physical.Suit, color: false, rank: physical.Rank, ids: [physical.Id], actualPhysical: true).Contains(target.Seat),
                "The batch target tail consumes Liubing's final Diamond appearance rather than its printed Spade."));

            var statistics = Field<CoreCurrentTurnStatisticsState>(game, "_currentTurnStatistics");
            var count = statistics.ReferenceToSlashCountThisTurn;
            statistics.ReferenceToSlashCountThisTurn = game.GetRuntimeModule<CoreGameEngineRuleQueriesRuntime>().GetSlashUseLimit(actor);
            try { Need(Match(counts: true).Count == 0 && Match(counts: false).SequenceEqual([3, 2, 1, 3]),
                "Batch count qualification preserves the ordinary exhausted quota and the caller's explicit count exemption."); }
            finally { statistics.ReferenceToSlashCountThisTurn = count; }

            var queries = game.GetRuntimeModule<CoreGameEngineActionQueriesRuntime>();
            var controls = Field<CharacterCaoPiMouState>(game, "_characterCaoPiMou").ReferenceToMouCaoPiControls;
            var control = new MouCaoPiControl(9020, 3, actor.Seat, turn, MouCaoPiControlKind.TrickHandOnly);
            using (queries.BeginActionQuery())
                Need(Match(ids: [physical.Id]).Count > 0, "The original read-only query starts with an allowed physical Slash material.");
            controls.Add(control);
            try
            {
                using (queries.BeginActionQuery())
                    Need(Match(ids: [physical.Id]).Count == 0 && Match(ids: []).Count > 0,
                        "A later query applies the actual lasting material ban without requiring its recipient to possess the issuing skill or banning a cardless use.");
            }
            finally { controls.Remove(control); }
            using (queries.BeginActionQuery())
                Need(Match(ids: [physical.Id]).Count > 0, "A fresh query reads removal of the material restriction instead of retaining an earlier batch answer.");
            Need(game.Events.Count == events && JsonSerializer.Serialize(game.CreateCardZoneDiagnostics()) == zones,
                "Single and batched target reads issue no use, payment, movement or Liubing consumption.");

            IReadOnlyList<int> Match(Suit? suit = null, bool? color = null, int? rank = null,
                IReadOnlyList<int>? ids = null, bool ignoreDistance = true, CardConversionSource? conversion = null,
                bool actualPhysical = false, bool counts = false, CardKind kind = CardKind.Slash)
            {
                var expected = candidates.Where(candidate => legality.CanUseVirtualSlashTarget(actor, candidate,
                    kind, suit, color, rank, ids, ignoreDistance, conversion, actualPhysical, counts))
                    .Select(candidate => candidate.Seat).ToArray();
                var actual = legality.GetVirtualSlashTargetSeats(actor, candidates,
                    kind, suit, color, rank, ids, ignoreDistance, conversion, actualPhysical, counts);
                Need(actual.SequenceEqual(expected),
                    "The production batch must match the original single-target answer for every ordered candidate and complete native input.");
                return actual;
            }
        }

        void CheckRankRangeBranches()
        {
            const string rankSkill = "fixture:virtual-target-rank";
            var far = players[2];
            var cards = (CardZoneStore)typeof(GameEngine).GetField("_cardZones", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(game)!;
            // This finite-deck fixture contains only Slash cards. Register one
            // real host-owned weapon to isolate ordinary range from rank range.
            var weapon = new Card(game.CreateCardZoneDiagnostics().Max(card => card.CardId) + 1,
                CardKind.ZhangbaSerpentSpear, Suit.Spade, 12);
            cards.AddGeneratedCard(weapon);
            cards.Move(weapon.Id, CardLocation.OutsideGame, CardLocation.Equipment(actor.Seat));
            var slash = new Card(weapon.Id + 1, CardKind.Slash, Suit.Spade, 1);
            cards.AddGeneratedCard(slash);
            cards.Move(slash.Id, CardLocation.OutsideGame, CardLocation.Hand(actor.Seat));
            var grant = new SkillGrant(rankSkill, rankSkill, rankSkill, "acquired:shared-range-check");
            actor.SkillGrants.Grant(grant);
            var capabilities = (ProgramCapabilityRegistry)typeof(GameEngine).GetProperty("ProgramCapabilities", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(game)!;
            var ranges = capabilities.Get<IAttackRangeQueries>();
            var frames = capabilities.Get<IProgramRuntimeFrames>();
            var finalRange = game.GetRuntimeModule<CoreGameEngineFinalTargetSlashRuntime>();
            var ruleQueries = game.GetRuntimeModule<CoreGameEngineRuleQueriesRuntime>();
            var actionQueries = game.GetRuntimeModule<CoreGameEngineActionQueriesRuntime>();
            var relation = new DirectedTurnCardPolicy(9010, turn, turnSeat, 9010, 0, source,
                actor.Seat, far.Seat, [], DirectedTurnCardPolicyEffect.InAttackRange)
                { RequiresEffectiveSource = false };
            try
            {
                Need(CoreGameEngineRuleQueriesRuntime.ConvertRuleValue(ruleQueries.CalculateAttackRange(actor)) == 3 &&
                    CoreGameEngineRuleQueriesRuntime.ConvertRuleValue(ruleQueries.CalculateDistance(actor, far)) == 2 &&
                    ranges.IsWithinAttackRange(actor.Seat, far.Seat) &&
                    finalRange.HasRankSlashRange(actor, CardKind.Slash) &&
                    !Range(1) && Range(3) && Range(null) && Range(0) && Range(-1) &&
                    !Virtual(1) && Virtual(3) && Virtual(null) && Virtual(0) && Virtual(-1),
                    "A known positive rank replaces ordinary weapon range; unknown and nonpositive ranks retain the original ordinary-range fallback.");
                Need(!Physical() && Physical(specificRank: 3) && Physical(noRank: true) &&
                    Physical(specificRank: 0) && !Physical(noRank: true, specificRank: 1),
                    "Physical Slash preserves the material rank, explicit rank override and unknown-rank fallback when checking ordinary range first.");
                Need(!finalRange.IsWithinSpecificSlashRange(actor, far, CardKind.Slash, null, excludedEquipmentId: weapon.Id) &&
                    !finalRange.IsWithinSpecificSlashRange(actor, far, CardKind.Slash, null, excludedEquipmentSlot: EquipmentSlot.Weapon) &&
                    finalRange.IsWithinSpecificSlashRange(actor, far, CardKind.Slash, 3, excludedEquipmentId: weapon.Id),
                    "Excluding a paid weapon changes ordinary range without widening or removing an independent known-rank allowance.");
                Need(!Physical(noRank: true, excludedId: weapon.Id) &&
                    !Physical(noRank: true, excludedSlot: EquipmentSlot.Weapon) &&
                    Physical(specificRank: 3, excludedId: weapon.Id),
                    "Physical Slash forwards both equipment exclusions without replacing the declared rank allowance with weapon range.");
                directed.Add(relation);
                try { Need(Range(1) && Physical(), "An issued shared attack-range relation independently allows a target beyond the known rank."); }
                finally { directed.Remove(relation); }
                Need(!Range(1), "Removing that relation restores the rank boundary instead of borrowing the weapon's ordinary range.");

                var useId = frames.NextId();
                var actionId = frames.NextId();
                var action = new CardActionContext(actionId, null, CardActionType.Use, actor.Seat, actor.Seat,
                    null, null, null, CardKind.Slash, [far.Seat], [], [], effectiveRank: 1);
                var policy = new TurnJudgedRankSlashPolicy(9011, turn, turnSeat, useId, 0, source,
                    "fixture:issued-rank", useId, physical.Id, 7);
                var use = new CardUseFrame(useId, actor.Seat, 0, CardKind.Slash, [far.Seat])
                {
                    Action = action,
                    JudgedRankSlashUse = new(useId, actionId, actor.Seat, 1, policy, IgnoresDistance: true, IgnoresQuota: false)
                };
                frames.Push(use);
                try
                {
                    Need(finalRange.IsWithinSpecificSlashRange(actor, far, CardKind.Slash, 1, useId) &&
                        Physical(useId: useId) && !Physical(useId: useId + 1000) &&
                        !Range(1) && !finalRange.IsWithinSpecificSlashRange(actor, far, CardKind.Slash, 1, useId + 1000) &&
                        !finalRange.IsWithinSpecificSlashRange(far, actor, CardKind.Slash, 1, useId),
                        "A real owning-use typed distance allowance is still reached after failed rank range, with its exact use identity and beneficiary actor.");
                }
                finally { frames.Pop(useId, ResolutionFrameKind.CardUse); }
                var distanceAllowance = new DirectedTurnCardPolicy(9014, turn, turnSeat, 9014, 0, source,
                    actor.Seat, far.Seat, [CardKind.Slash], DirectedTurnCardPolicyEffect.IgnoreDistance)
                    { RequiresEffectiveSource = false };
                directed.Add(distanceAllowance);
                try
                {
                    Need(!Range(1) && Virtual(1),
                        "A failed complete rank-aware range check still reaches an independently issued directed distance allowance.");
                    actor.SkillGrants.RemoveGrant(driver.GrantId);
                    try { Need(!Range(1) && Virtual(1), "The issued independent distance allowance remains effective after its original source skill is lost."); }
                    finally { actor.SkillGrants.Grant(driver); }

                    var prohibition = effects.GrantActionProhibition(turn, turnSeat, 9012, 0, source,
                        [CardKind.Slash], [CardActionType.Use], affectedSeat: actor.Seat);
                    try { Need(!Physical(noRank: true) && !Virtual(1), "Neither ordinary physical range nor an issued virtual distance allowance can bypass the native use prohibition."); }
                    finally { ((IList<TurnCardActionProhibition>)effects.ActionProhibitions).Remove(prohibition); }
                    var targetBan = new DirectedTurnCardPolicy(9013, turn, turnSeat, 9013, 0, source,
                        actor.Seat, far.Seat, [CardKind.Slash], DirectedTurnCardPolicyEffect.ForbidTarget);
                    directed.Add(targetBan);
                    try { Need(!Physical(noRank: true) && !Virtual(1), "Neither ordinary physical range nor an issued virtual distance allowance can bypass the later directed-target prohibition."); }
                    finally { directed.Remove(targetBan); }
                    Need(Virtual(1), "Removing the outer target prohibition restores only the independent distance allowance.");
                }
                finally { directed.Remove(distanceAllowance); }
                Need(!Virtual(1), "A fresh query after withdrawing that allowance restores the failed rank boundary.");
                Need(!legality.CanUseVirtualSlashTarget(actor, actor, CardKind.Slash, effectiveRank: 3,
                    physicalCardIds: [], countsTowardLimit: false) &&
                    !legality.CanUseSlashTarget(actor, actor, slash, specificEffectiveRank: 3),
                    "Native Slash target legality rejects self even when the range branch would allow it.");
                var alive = far.IsAlive;
                far.IsAlive = false;
                try
                {
                    Need(!legality.CanUseVirtualSlashTarget(actor, far, CardKind.Slash, effectiveRank: 3,
                        physicalCardIds: [], countsTowardLimit: false) && !Physical(specificRank: 3),
                        "Native Slash target legality rejects a dead target before consulting range allowances.");
                }
                finally { far.IsAlive = alive; }
            }
            finally
            {
                actor.SkillGrants.RemoveGrant(grant.GrantId);
                cards.Move(slash.Id, CardLocation.Hand(actor.Seat), CardLocation.OutsideGame);
                cards.Move(weapon.Id, CardLocation.Equipment(actor.Seat), CardLocation.OutsideGame);
            }
            bool Range(int? rank) => finalRange.IsWithinSpecificSlashRange(actor, far, CardKind.Slash, rank);
            bool Virtual(int? rank)
            {
                using var query = actionQueries.BeginActionQuery();
                var allowed = legality.CanUseVirtualSlashTarget(actor, far, CardKind.Slash,
                    effectiveRank: rank, physicalCardIds: [], countsTowardLimit: false);
                Need(legality.GetVirtualSlashTargetSeats(actor, [actor, far, far], CardKind.Slash,
                    effectiveRank: rank, physicalCardIds: [], countsTowardLimit: false)
                    .SequenceEqual(allowed ? new[] { far.Seat, far.Seat } : []),
                    "Single and batched virtual target queries retain the complete rank-aware range, independent allowances and outer prohibitions.");
                return allowed;
            }
            bool Physical(bool noRank = false, int? specificRank = null, long? useId = null,
                int? excludedId = null, EquipmentSlot? excludedSlot = null) =>
                legality.CanUseSlashTarget(actor, far, slash, existingUseFrameId: useId,
                    noEffectiveRank: noRank, specificEffectiveRank: specificRank,
                    excludedEquipmentId: excludedId, excludedEquipmentSlot: excludedSlot);
        }
    }

    private static GameEngine Start()
    {
        var type = typeof(WuJingChecks).GetNestedType("Fixture", BindingFlags.NonPublic)!;
        var native = (IGameContentPackage)Activator.CreateInstance(type,
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, ["black"], null)!;
        var registry = ContentRegistry.Build(new StandardContentPackage(), native, new Policies());
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = 31, PlayerCount = 4, HumanSeat = 0, HumanRole = Role.Lord,
            ModeId = "identity:classic-wu-jing-fixture", UseInteractiveSetup = true,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 12
        }, registry);
        Accept(new StartGameCommand());
        Invoke("Reach", game, (Func<bool>)(() => game.PendingDecision is { PlayerSeat: 0, Kind: DecisionKind.SelectGeneral }));
        Accept(new SelectGeneralCommand(0, "fixture:wj-owner", game.Revision, game.PendingDecision!.PromptId));
        Invoke("Play", game);
        return game;

        void Accept(GameCommand command)
        {
            var result = game.Submit(command);
            Need(result.Accepted, result.Error?.Message ?? "Shared target fixture command was rejected.");
        }
    }

    private static void Invoke(string name, params object[] args)
    {
        try { typeof(WuJingChecks).GetMethod(name, BindingFlags.Static | BindingFlags.NonPublic)!.Invoke(null, args); }
        catch (TargetInvocationException error) when (error.InnerException is not null)
        { ExceptionDispatchInfo.Capture(error.InnerException).Throw(); }
    }
    private static T Field<T>(GameEngine game, string name) =>
        (T)FixtureRuntimeAccess.Engine.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(game)!;
    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }

    private sealed class Policies : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-virtual-slash-targets", new(1, 0, 0), []);
        public void Register(IContentRegistryBuilder builder)
        {
            var specs = new[] { ("normal", "slash"), ("fire", "fireSlash"), ("rank", "slash") };
            var skills = specs.Select(row => new
            {
                id = "fixture:virtual-target-" + row.Item1, revision = 1,
                cardPolicies = new[] { new Dictionary<string, object>
                {
                    ["id"] = "target", ["kind"] = row.Item1 == "rank" ? "slashRangeFromEffectiveRank" : "prohibitTarget",
                    ["cardKinds"] = new[] { row.Item2 }
                } }
            }).ToArray();
            var catalog = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills }),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = skills.ToDictionary(row => row.id, _ => new { name = "共同目标规则", description = "共享杀目标资格" }) }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = catalog.Presentations[id] });
        }
    }
}
