using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheBossStageChecks
{
    private const string Owner = "fixture:shanhe-stage-owner", First = "fixture:shanhe-stage-first", Second = "fixture:shanhe-stage-second";
    private const string Driver = "fixture:shanhe-stage-driver", Retaliation = "fixture:shanhe-stage-retaliation", Quiet = "fixture:shanhe-stage-quiet";
    private const string MaximumLoss = "fixture:shanhe-stage-maximum-loss";
    private const string Deck = "fixture:shanhe-stage-deck";

    public static void LethalSlashRetiresPaidMaterialAndSecondFormCold()
    {
        foreach (var temporary in new[] { true, false })
        {
            var s = Scene.Start(temporary); s.ReachPlay();
            var equipment = (temporary ? s.V().Hand : s.V().Equipment).Single(c => c.Kind == CardKind.Crossbow).Id;
            if (temporary) { s.Card(equipment, []); s.ReachPlay(); }
            var slash = s.V().Hand.Single(c => c.Kind == CardKind.Slash).Id;
            var untouched = s.V().Hand.Single(c => c.Kind == CardKind.Peach).Id;
            var hp = s.V().Hp; var originCount = s.E<ShanheStartingResourceCreatedEvent>().Length;
            s.Cold();
            s.Card(slash, [1]); s.Until(() => s.E<ShanheBossStageStartedEvent>().Length != 0);
            Need(s.E<DamageAppliedEvent>().Single() is { TargetSeat: 1, Amount: 1, RemainingHp: 0 } &&
                s.V(1) is { GeneralId: Second, IsAlive: true, MaxHp: 6, Hp: 5 } &&
                s.G.CreateSnapshot(0) is { Winner: Winner.None, CurrentSeat: 1, Phase: TurnPhase.NotStarted },
                "A genuine lethal physical Slash replaces the living first form in the same battle and gives the boss the next natural turn before dying or victory.");
            Need(s.E<PlayerDiedEvent>().Length == 0 && s.E<PlayerDyingEvent>().Length == 0 &&
                s.E<WinnerDeterminedEvent>().Length == 0 && s.E<ShanheBossTurnInterruptedEvent>().Single() is { PlayerSeat: 0, BossSeat: 1 } &&
                s.G.ResolutionStack.Count == 0 && s.G.CreateSnapshot(0).ProcessingCardCount == 0,
                "The transition retires the entire old command subtree and actual turn without first-form death or settlement.");
            Need(s.Zone(slash) == (temporary ? CardLocation.OutsideGame : CardLocation.DiscardPile) &&
                s.G.CardMovements.Count(m => m.CardId == slash && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1 &&
                s.G.CardMovements.Count(m => m.CardId == slash && m.From == CardLocation.Processing) == 1 &&
                (!temporary || s.E<ShanheStartingResourceDestroyedEvent>().Single().CardId == slash &&
                    s.E<ShanheBossMaterialCancelledEvent>().Single().CardId == slash),
                "A retired native Use retains its exact paid movement: temporary material leaves play and ordinary material enters discard, exactly once.");
            Need(s.V().Hp == hp && s.Zone(equipment) == CardLocation.Equipment(0) && s.Zone(untouched) == CardLocation.Hand(0) &&
                s.V().Skills!.Any(skill => skill.ContentId == Driver) && s.E<ShanheStartingResourceCreatedEvent>().Length == originCount &&
                s.V(1).Hand.Count == 2,
                "Transformation preserves the player's HP, original hand/equipment and tactic while drawing only the configured two cards for the second form.");
            var logCut = RequireNativeStageLogBoundary(s.G, requireLethalDamage: true);
            s.Cold();
            Need(RequireNativeStageLogBoundary(s.G, requireLethalDamage: true) == logCut,
                "Cold physical-Slash replay preserves the same native log cut, fatal first-form damage and second-form output partition.");
            s.ReachPlay();
            s.Accept(new UseProgramSkillCommand(0, Driver, "finish", [], [1], s.G.Revision, s.P!.PromptId));
            s.Until(() => s.G.CreateSnapshot(0).Winner != Winner.None);
            Need(s.E<ShanheBossStageStartedEvent>().Length == 1 && s.E<PlayerDiedEvent>().Single().VictimSeat == 1 &&
                s.E<ShanheStartingResourceCreatedEvent>().Length == originCount,
                "The second form dies normally; neither replay nor later lethal damage repeats transformation or opening resources.");
            s.Cold();
        }
    }

    public static void BossTurnRetaliationRetiresOriginalTurnCold()
    {
        var s = Scene.Start(temporary: true, retaliation: true); s.ReachPlay(); s.Cold();
        s.Accept(new EndPlayPhaseCommand(0, s.G.Revision, s.P!.PromptId));
        s.Until(() => s.E<ShanheBossStageStartedEvent>().Length != 0);
        var interrupted = s.E<ShanheBossTurnInterruptedEvent>().Single();
        Need(interrupted.PlayerSeat == 1 && interrupted.BossSeat == 1 &&
            s.E<DamageAppliedEvent>().Any(e => e.SourceSeat == 1 && e.TargetSeat == 0) &&
            s.E<DamageAppliedEvent>().Last() is { SourceSeat: 0, TargetSeat: 1, RemainingHp: 0 } &&
            s.E<TurnEndedEvent>().Count(e => e.TurnNumber == interrupted.TurnNumber && e.ActorSeat == 1) == 1 &&
            s.G.CreateSnapshot(0) is { Winner: Winner.None, CurrentSeat: 1, Phase: TurnPhase.NotStarted } && s.G.ResolutionStack.Count == 0,
            "A genuine reflected hit during the boss's own Slash retires that exact old actual turn and all paid children before beginning the second form's new natural turn.");
        var enemySlash = s.G.CardMovements.Single(m => m.From == CardLocation.Hand(1) && m.To == CardLocation.Processing && m.CardKind == CardKind.Slash).CardId;
        Need(s.Zone(enemySlash) == CardLocation.DiscardPile &&
            s.G.CardMovements.Count(m => m.CardId == enemySlash && m.From == CardLocation.Processing) == 1,
            "The interrupted enemy Slash remains paid and is cleaned exactly once without a stale response or damage cursor.");
        s.Cold();
    }

    public static void ZeroMaximumNativeReplacementStillTransformsCold()
    {
        var s = Scene.Start(temporary: true, zeroMaximum: true); s.ReachPlay(); s.Cold();
        var slash = s.V().Hand.Single(c => c.Kind == CardKind.Slash).Id;
        s.Card(slash, [1]); s.Until(() => s.E<ShanheBossStageStartedEvent>().Length != 0);
        Need(s.E<CardDamageReplacedWithMaximumHpLossEvent>().Single().Receipt is
                { TargetSeat: 1, MaximumHpBefore: 1, MaximumHpAfter: 0, HpAfter: 0 } &&
            s.E<DamageAppliedEvent>().Length == 0 && s.E<PlayerDiedEvent>().Length == 0 &&
            s.E<PlayerDyingEvent>().Length == 0 && s.E<WinnerDeterminedEvent>().Length == 0 &&
            s.V(1) is { GeneralId: Second, IsAlive: true, MaxHp: 6, Hp: 5 } &&
            s.Zone(slash) == CardLocation.OutsideGame && s.E<ShanheStartingResourceDestroyedEvent>().Single().CardId == slash,
            "A real native card-damage replacement that exhausts the first form's maximum HP transforms before direct death, preserving its exact paid temporary Slash cleanup.");
        var logCut = RequireNativeStageLogBoundary(s.G, requireLethalDamage: false);
        s.Cold();
        Need(s.E<ShanheBossStageStartedEvent>().Length == 1 && s.G.ResolutionStack.Count == 0 &&
            s.G.CreateSnapshot(0) is { Winner: Winner.None, CurrentSeat: 1, Phase: TurnPhase.NotStarted } &&
            RequireNativeStageLogBoundary(s.G, requireLethalDamage: false) == logCut,
            "Cold replay retains the single zero-maximum transition and retires its replacement/death parent.");
    }

    internal static int RequireNativeStageLogBoundary(GameEngine game, bool requireLethalDamage)
    {
        var stage = game.Events.Select(envelope => envelope.Payload).OfType<ShanheBossStageStartedEvent>().Single();
        var cut = stage.LastPreviousFormLogSequence ?? throw new InvalidOperationException("A native stage must publish its actual previous-form log boundary.");
        var announcement = game.Log.Single(log => log.Type == "ShanheBossTransformed" && log.ActorSeat == stage.BossSeat);
        var draw = game.Log.Last(log => log.Type == "CardsDrawn" && log.ActorSeat == stage.BossSeat && log.Sequence < announcement.Sequence);
        // Both sides use GameLogEntry.Sequence. Typed-event sequence/counts are a separate
        // clock and cannot locate this boundary. In these existing scenes, retirement emits
        // facts/material movements; the configured second-form draw is the first new log.
        Need(game.Log.LastOrDefault(log => log.Sequence < draw.Sequence)?.Sequence == cut &&
            draw.Sequence > cut && announcement.Sequence > draw.Sequence,
            "The cut is the actual last native log before second-form drawing and announcement, including zero-maximum replacement paths with no ordinary HP-loss result log.");
        if (requireLethalDamage)
            Need(game.Log.Last(log => log.Type == "Damage" && log.TargetSeat == stage.BossSeat).Sequence <= cut,
                "The original form's actual fatal Damage log belongs to the previous-form side of the cut.");
        return cut;
    }

    private static void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public PendingDecision? P => G.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot V(int seat = 0) => G.CreateSnapshot(seat).Players[seat];
        public T[] E<T>() where T : IGameEvent => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public CardLocation Zone(int id) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location;
        public void Accept(GameCommand command) { var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(result.Accepted, result.Error?.Message ?? "Shanhe stage command rejected."); }
        public void Card(int id, IReadOnlyList<int> targets)
        {
            var action = G.GetHumanLegalActions().First(a => a.CardId == id && a.TargetSeats.SequenceEqual(targets));
            Accept(new PlayCardCommand(0, id, targets, G.Revision, P!.PromptId, action.PlayedCardKind, action.TargetCardId));
        }
        public void ReachPlay() => Until(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Until(Func<bool> predicate)
        {
            for (var i = 0; i < 150; i++)
            {
                if (predicate()) return;
                if (P is { Kind: DecisionKind.RespondDodge or DecisionKind.RescueDying } prompt)
                {
                    var pass = prompt.Choices.First(c => c.Cards.Count == 0);
                    Accept(new AnswerPromptCommand(0, prompt.PromptId, pass.Id, G.Revision));
                }
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("The bounded boss-stage scene did not reach its boundary: " + JsonSerializer.Serialize(P));
        }
        public void Cold()
        {
            var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(Enumerable.Range(0, G.PlayerCount).All(seat => SnapshotJson.Serialize(G.CreateSnapshot(seat)) == SnapshotJson.Serialize(restored.CreateSnapshot(seat))) &&
                JsonSerializer.Serialize(G.CardMovements) == JsonSerializer.Serialize(restored.CardMovements) &&
                JsonSerializer.Serialize(G.Events) == JsonSerializer.Serialize(restored.Events) &&
                JsonSerializer.Serialize(G.Log) == JsonSerializer.Serialize(restored.Log),
                "Cold command replay preserves all views, material proofs, native logs and the once-only stage marker with its log boundary.");
            G = restored;
        }
        public static Scene Start(bool temporary, bool retaliation = false, bool zeroMaximum = false)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(retaliation, zeroMaximum));
            var setup = new ShanheBattleSetup(71, 1, "fixture:shanhe-stage", Owner, 3, 0, 0, 0, 0, [],
                retaliation ? [] : temporary
                    ? [new(CardKind.Slash, Suit.Spade, 7), new(CardKind.Peach, Suit.Heart, 3), new(CardKind.Crossbow, Suit.Club, 1)]
                    : [new(CardKind.Slash, Suit.Spade, 7), new(CardKind.Peach, Suit.Heart, 3)],
                retaliation || temporary ? [] : [new(CardKind.Crossbow, Suit.Club, 1)],
                [new(First, Hp: 1) { StartingHand = retaliation ? [new(CardKind.Slash, Suit.Spade, 8)] : [] }], [], new string('a', 64))
            { StartingResourcesExpireOnUse = temporary, BossTransition = new(0, Second, 6, 5, 2), TacticSkillIds = zeroMaximum ? [Driver, MaximumLoss] : [Driver] };
            var game = GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8 }, registry);
            var scene = new Scene(game, registry); scene.Accept(new StartGameCommand()); return scene;
        }
    }

    private sealed class Fixture(bool retaliation, bool zeroMaximum) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shanhe-boss-stage", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var rules = $$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
              {"id":"{{Driver}}","revision":1,"activations":[{"id":"finish","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":20}]}]},
              {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]},
              {"id":"{{MaximumLoss}}","revision":1,"cardPolicies":[{"id":"first-slash-maximum","kind":"firstRoundCardDamageBecomesMaximumHpLoss","cardKinds":["slash","fireSlash","thunderSlash"]}]},
              {"id":"{{Retaliation}}","revision":1,"triggers":[{"id":"counter","window":"afterDamageApplied","subject":"owner","damageOccurrence":"perDamage","usageScope":"game","usageLimit":1,"optional":false,"isSkillInvocation":false,"effects":[{"op":"damage","target":"owner","targetRef":{"kind":"eventSource"},"amount":1}]}]}
            ]}
            """;
            var programs = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new[] { Driver, Quiet, Retaliation, MaximumLoss }.ToDictionary(id => id, id => new { name = id, description = "Actual command boss-stage fixture" }) }));
            foreach (var (id, program) in programs.Programs) builder.AddSkill(new(id, id, "fixture") { Program = program, ProgramPresentation = programs.Presentations[id] });
            builder.AddSkill(new("fixture:shanhe-stage-none", "静默", "无额外能力"));
            builder.AddGeneral(new(Owner, "玩家", "supporter", retaliation ? Retaliation : "fixture:shanhe-stage-none", "shu", 4));
            builder.AddGeneral(new(First, "第一阶段", "supporter", "fixture:shanhe-stage-none", "wei", zeroMaximum ? 1 : 4));
            builder.AddGeneral(new(Second, "第二阶段", "supporter", Quiet, "wei", 6));
            builder.AddDeck(new(Deck, "无闪的原生杀场景", 0, 0, [new("standard:peach", 30)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "山河图二阶段夹具", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, First, Second], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
