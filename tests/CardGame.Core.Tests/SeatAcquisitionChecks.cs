using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

namespace CardGame.Core.Tests;

internal static class SeatAcquisitionChecks
{
    private const string Hero = "happy:shen-cao-cao", Guixin = "happy:guixin", Driver = "fixture:happy-guixin-driver",
        Watch = "fixture:happy-guixin-watch", Deck = "fixture:happy-guixin-deck", Mode = "identity:classic-happy-guixin-fixture";
    private const string Reason = "skill-program.happy:guixin.obtain";

    public static void ActualGainsControlTurnoverAndColdRandomPayments()
    {
        var six = Scene.Start(6); six.UseDamage();
        var first = Sweep(six, 5, coldFirst: true);
        six.Offer();
        six.Need(six.View.IsFaceDown && six.View.HandCount == 2 && six.E<HappyGuixinTurnoverIssuedEvent>() is
            [{ FrameId: var turned, ActualObtained: 5, Before: false, After: true }] && turned == first,
            "Five real gains flip the initially face-up owner even though each acquired entity was spent in its gain child and final hand size remains two.");
        var second = Sweep(six, 5, coldFirst: false, offered: true); six.Play();
        six.Need(six.View.IsFaceDown && six.View.HandCount == 2 && six.E<HappyGuixinCompletedEvent>() is
            [{ FrameId: var a, PaidCharacters: 5, ActualObtained: 5, TurnoverIssued: true, Cancelled: false },
             { FrameId: var b, PaidCharacters: 5, ActualObtained: 5, TurnoverIssued: false, Cancelled: false }] && a == first && b == second &&
            six.E<HappyGuixinStartedEvent>().Select(e => e.OccurrenceIndex).SequenceEqual([0, 1]) &&
            six.E<HappyGuixinTurnoverIssuedEvent>().Length == 1 && six.E<HappyGuixinMovementPaidEvent>().Length == 10 &&
            six.G.CardMovements.Count(m => m.Reason.Value == Reason) == 10 &&
            six.G.ResolutionStack.OfType<ProgramSkillFrame>().All(f => f.SkillId != Guixin),
            "The second actual damage point independently acquires five cards without flipping an already face-down owner; both paid sweeps and their ten discarded gains complete once.");

        var four = Scene.Start(4); four.UseDamage(); Sweep(four, 3, coldFirst: false); four.Play();
        four.Need(!four.View.IsFaceDown && four.View.HandCount == 2 &&
            four.E<HappyGuixinCompletedEvent>() is [{ PaidCharacters: 3, ActualObtained: 3, TurnoverIssued: false, Cancelled: false }] &&
            four.E<HappyGuixinTurnoverIssuedEvent>().Length == 0,
            "Three actual gains in a four-player sweep do not reach Happy Guixin's greater-than-four turnover threshold.");
    }

    public static void PriorityRegionFallbackAndBothPaidContinuationsCold()
    {
        var scene = Scene.Start(4, ol: true); scene.UseDamage();
        var frame = Sweep(scene, 3, coldFirst: true, priority: CardZoneKind.Equipment); scene.Play();
        scene.Need(scene.View.IsFaceDown && scene.View.HandCount == 2 &&
            scene.E<OlGuixinPriorityChosenEvent>() is [{ FrameId: var chosen, PreferredZone: CardZoneKind.Equipment }] && chosen == frame &&
            scene.E<OlGuixinMovementPaidEvent>().Length == 3 &&
            scene.E<OlGuixinMovementPaidEvent>().All(e => e.FrameId == frame && e.ChosenZone == CardZoneKind.Hand) &&
            scene.E<OlGuixinTurnoverIssuedEvent>() is [{ FrameId: var turned, Before: false, After: true }] && turned == frame &&
            scene.E<OlGuixinCompletedEvent>() is [{ FrameId: var completed, PaidCharacters: 3, TurnoverIssued: true, Cancelled: false }] && completed == frame &&
            scene.G.CardMovements.Count(m => m.Reason.Value == scene.MovementReason) == 3,
            "OL priority selection cold-restores before payment, falls back from empty equipment to hand, and completes each cold paid gain once before its unconditional turnover.");
    }

    public static void RetainedGainSettlesOnceWithoutFalseMovementAndRecoversCold()
    {
        var scene = Scene.Start(6, retained: true);
        scene.EndPlay(0); scene.Reach(p => p is { PlayerSeat: 1, Kind: DecisionKind.PlayCard });
        scene.Use(1, "happy:gujing", "bone-path", [0]);
        scene.Reach(p => p.SkillPrompt?.SkillId == "happy:gujing" && p.Choices.Any(c => c.Parameters.GetValueOrDefault("option")?.StartsWith("card:") == true));
        scene.Answer(c => c.Parameters.GetValueOrDefault("option")?.StartsWith("card:") == true);
        scene.Reach(p => p.Choices.Any(c => c.Parameters.GetValueOrDefault("option") == "slot:Weapon"));
        scene.Answer(c => c.Parameters.GetValueOrDefault("option") == "slot:Weapon");
        scene.Reach(p => p.Choices.Any(c => c.Targets.SequenceEqual([0]) && c.Parameters.GetValueOrDefault("option")?.StartsWith("abolish:") == true));
        scene.Answer(c => c.Targets.SequenceEqual([0]) && c.Parameters.GetValueOrDefault("option")?.StartsWith("abolish:") == true);
        scene.Answer(c => c.Parameters.GetValueOrDefault("option") == "finish");
        scene.Reach(p => p is { PlayerSeat: 1, Kind: DecisionKind.PlayCard });
        scene.Need(scene.E<HappyWeiYanGujingIssuedEvent>() is [{ Source.OwnerSeat: 1 }] &&
            scene.E<HappyWeiYanGujingAbolishedEvent>() is [{ Holder: 0, Slot: EquipmentSlot.Weapon }],
            "A real limited Gujing places and discards its bone before abolishing the acquisition owner's weapon slot.");
        for (var seat = 1; seat < 6; seat++)
        {
            scene.Reach(p => p.PlayerSeat == seat && p.Kind == DecisionKind.PlayCard);
            scene.EndPlay(seat);
        }
        scene.Play(); scene.UseDamage(); scene.Offer();
        scene.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        ProgramSeatAcquisitionPayment? retained = null;
        long frameId = 0;
        for (var cursor = 1; cursor < 5; cursor++)
        {
            scene.Reach(p => p.SkillPrompt?.SkillId == Watch && p.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "continue"));
            var frame = scene.Parent; var receipt = frame.SeatAcquisition!;
            retained ??= receipt.Payments[0]; frameId = frame.Id;
            scene.Need(receipt.Cursor == cursor && receipt.Payments.Count == cursor + 1 &&
                receipt.Payments[0].CardId == retained.CardId && receipt.Payments[0].RandomBefore == retained.RandomBefore &&
                receipt.Payments[0].RandomAfter == retained.RandomAfter &&
                retained is { Cursor: 0, TargetSeat: 1, BatchId: null, Movements.Count: 0 } &&
                retained.Before == retained.After && retained.Candidates.Count == 2 && retained.RandomBefore != retained.RandomAfter &&
                scene.Zone(retained.CardId) == CardLocation.Hand(1) && frame.HappyWeiYanRetainedGains is [{ Plan.CardId: var kept }] && kept == retained.CardId &&
                receipt.Payments.Skip(1).All(p => p.BatchId is not null && p.Movements is [{ To: var actual }] && actual == CardLocation.Hand(1)) &&
                scene.E<HappyGuixinMovementPaidEvent>().Count(e => e.FrameId == frame.Id) == cursor &&
                scene.E<HappyGuixinMovementPaidEvent>().All(e => e.FrameId != frame.Id || e.Cursor > 0),
                "The first selected weapon stays in Gujing's source hand with one settled RNG receipt and no fabricated batch; later seats still issue real redirected movements once.");
            if (cursor == 1)
            {
                scene.Cold();
                var restored = scene.Parent.SeatAcquisition!.Payments[0];
                scene.Need(restored.CardId == retained.CardId && restored.RandomBefore == retained.RandomBefore &&
                    restored.RandomAfter == retained.RandomAfter && restored.BatchId is null && restored.Movements.Count == 0 &&
                    scene.Parent.HappyWeiYanRetainedGains?.Count == 1,
                    "Cold recovery during the following real gain preserves the earlier no-movement settlement without rerolling or settling it twice.");
                retained = restored;
            }
            scene.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        }
        scene.Play();
        scene.Need(retained is not null && scene.Zone(retained.CardId) == CardLocation.Hand(1) &&
            scene.G.CardMovements.All(m => m.CardId != retained.CardId || m.Reason.Value != Reason) &&
            scene.G.CardMovements.Count(m => m.Reason.Value == Reason) == 4 &&
            scene.G.CardMovements.Where(m => m.Reason.Value == Reason).All(m => m.To == CardLocation.Hand(1)) &&
            scene.E<HappyGuixinCompletedEvent>() is [{ FrameId: var completed, PaidCharacters: 4, ActualObtained: 0, TurnoverIssued: false, Cancelled: false }] &&
            completed == frameId && !scene.View.IsFaceDown && scene.View.HandCount == 3 && scene.E<HappyGuixinTurnoverIssuedEvent>().Length == 0,
            "Five settled requests produce only four physical acquisitions to the replacement recipient, zero actual gains to Guixin's owner and no conditional turnover.");
    }

    private static long Sweep(Scene s, int count, bool coldFirst, bool offered = false, CardZoneKind? priority = null)
    {
        if (!offered) s.Offer();
        s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        if (priority is { } zone)
        {
            s.Reach(p => p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "ol-guixin-priority"));
            s.Need(s.Parent.SeatAcquisition is { Stage: ProgramSeatAcquisitionStage.PriorityChoice, Payments.Count: 0 },
                "The priority region is chosen before issuing the first physical payment.");
            s.Cold();
            s.Answer(c => c.Parameters.GetValueOrDefault("priority-zone") == zone.ToString());
        }
        long frameId = 0;
        for (var cursor = 0; cursor < count; cursor++)
        {
            s.Reach(p => p.SkillPrompt?.SkillId == Watch && p.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "continue"));
            var frame = s.Parent; var receipt = frame.SeatAcquisition!; var paid = receipt.Payments.Last(); frameId = frame.Id;
            s.Need(receipt.Stage == ProgramSeatAcquisitionStage.MovementChildren && receipt.Cursor == cursor && receipt.Payments.Count == cursor + 1 &&
                receipt.OtherSeats.SequenceEqual(Enumerable.Range(1, count)) && paid.TargetSeat == cursor + 1 &&
                paid.From == CardLocation.Hand(cursor + 1) && paid.Movements is [{ To: var destination }] && destination == CardLocation.Hand(0) &&
                s.Zone(paid.CardId) == CardLocation.Hand(0) &&
                (s.E<HappyGuixinMovementPaidEvent>().Count(e => e.FrameId == frame.Id) +
                 s.E<OlGuixinMovementPaidEvent>().Count(e => e.FrameId == frame.Id)) == cursor + 1,
                "Each random acquisition pays one real original opposing hand entity and suspends its original damage-point frame before visiting another seat.");
            if (coldFirst && cursor == 0)
            {
                s.Need(paid.Candidates.Count == 2 && paid.RandomBefore != paid.RandomAfter,
                    "The first paid acquisition retains its genuine two-card random candidate pool and advanced RNG receipt.");
                s.Cold();
                s.Need(s.Parent.Id == frameId && s.Parent.SeatAcquisition!.Payments.Count == 1 &&
                    s.Parent.SeatAcquisition.Payments[0].RandomBefore == paid.RandomBefore &&
                    s.Parent.SeatAcquisition.Payments[0].RandomAfter == paid.RandomAfter &&
                    s.Parent.SeatAcquisition.Payments[0].CardId == paid.CardId,
                    "Cold recovery preserves the issued random draw and exact paid card instead of rerolling or acquiring another entity.");
            }
            var card = paid.CardId;
            s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
            s.Reach(p => p.SkillPrompt?.SkillId == Watch && p.Choices.Any(c => c.Cards.SequenceEqual([card])));
            s.Answer(c => c.Cards.SequenceEqual([card]));
            if (s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "finish-owned-cards") == true)
                s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "finish-owned-cards");
            s.Reach(_ => s.Zone(card) == CardLocation.DiscardPile);
            s.Need(s.G.CardMovements.Count(m => m.CardId == card && m.Reason.Value == s.MovementReason && m.To == CardLocation.Hand(0)) == 1,
                "Spending the obtained entity inside the gain observer does not duplicate its original paid acquisition.");
        }
        return frameId;
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry, int players, bool ol = false, bool retained = false)
    {
        public GameEngine G { get; private set; } = game;
        public string SkillId => ol ? "ol:guixin" : Guixin;
        public string MovementReason => ol ? "skill-program.ol:guixin.obtain" : Reason;
        public PlayerSnapshot View => G.CreateSnapshot(0).Players.Single(p => p.Seat == 0);
        public PendingDecision? P => Enumerable.Range(0, players).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public ProgramSkillFrame Parent => G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == SkillId && f.SeatAcquisition is not null);
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public CardLocation Zone(int card) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == card).Location;
        public void Play() => Reach(p => p is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Offer() => Reach(p => p.SkillPrompt?.SkillId == SkillId && p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
        public void UseDamage() => Use(0, Driver, "damage", []);
        public void Use(int seat, string skill, string activation, IReadOnlyList<int> targets) =>
            Accept(new UseProgramSkillCommand(seat, skill, activation, [], targets, G.Revision, P!.PromptId));
        public void EndPlay(int seat) => Accept(new EndPlayPhaseCommand(seat, G.Revision, P!.PromptId));
        public void Answer(Func<PromptChoice, bool> predicate)
        {
            var p = P!; var c = p.Choices.FirstOrDefault(predicate);
            Need(c is not null, "The real Guixin observer publishes its required current choice.");
            Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, c!.Id, G.Revision));
        }
        public void Reach(Func<PendingDecision, bool> ready)
        {
            for (var i = 0; i < 150; i++)
            {
                if (P is { } p)
                {
                    if (ready(p)) return;
                    if (p.Kind == DecisionKind.SelectFaction) { Answer(_ => true); continue; }
                    if (retained && p.Kind == DecisionKind.SelectGeneral)
                    {
                        var general = p.PlayerSeat == 1 ? "happy:shen-wei-yan" : "fixture:happy-guixin-peer-" + p.PlayerSeat;
                        Accept(new SelectGeneralCommand(p.PlayerSeat, general, G.Revision, p.PromptId)); continue;
                    }
                    if (retained && p.SkillPrompt?.SkillId == "happy:zigu")
                    { Answer(c => c.Parameters.GetValueOrDefault("program-action") == "decline"); continue; }
                    if (retained && p.SkillPrompt?.SkillId == "happy:ezhi" && p.Choices.Any(c => c.Parameters.GetValueOrDefault("option") == "skip"))
                    { Answer(c => c.Parameters.GetValueOrDefault("option") == "skip"); continue; }
                    throw new InvalidOperationException("Unexpected Happy Guixin boundary: " + JsonSerializer.Serialize(p));
                }
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(false, "Happy Guixin reaches its bounded real command boundary.");
        }
        public void Cold()
        {
            var before = State();
            G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(State() == before, "The cold command prefix exactly restores all views, original paid random receipt, physical zones, observer and events.");
        }
        private string State() => JsonSerializer.Serialize(new
        {
            Views = Enumerable.Range(0, players).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(),
            Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(),
            Events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray(),
            G.CardMovements, Zones = G.CreateCardZoneDiagnostics(), Commands = CommandJson.Serialize(G.AcceptedCommands)
        });
        public void Need(bool yes, string message) { if (!yes) throw new InvalidOperationException(message); }
        private void Accept(GameCommand command)
        {
            var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single());
            Need(result.Accepted, result.Error?.Message ?? "Real Happy Guixin command rejected.");
        }
        public static Scene Start(int players, bool ol = false, bool retained = false)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(players, ol, retained));
            var g = GameEngine.CreateStandard(new GameOptions { Seed = 7, PlayerCount = players, HumanSeat = 0, HumanRole = Role.Lord,
                ModeId = Mode, HumanSeats = retained ? Enumerable.Range(0, players).ToArray() : null,
                UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, registry);
            var s = new Scene(g, registry, players, ol, retained); s.Accept(new StartGameCommand()); s.Reach(p => p.Kind == DecisionKind.SelectGeneral);
            s.Accept(new SelectGeneralCommand(0, ol ? "ol:shen-cao-cao" : Hero, g.Revision, s.P!.PromptId)); s.Play();
            var initial = retained ? 3 : 2;
            s.Need(s.View.HandCount == initial && !s.View.IsFaceDown && Enumerable.Range(1, players - 1).All(i => g.CreateSnapshot(i).Players.Single(p => p.Seat == i).HandCount == initial),
                "A fixed opening hand and zero normal draws start with real cards and an initially face-up owner.");
            return s;
        }
    }

    private sealed class Fixture(int players, bool ol, bool retained) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:happy-guixin", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            var hero = ol ? "ol:shen-cao-cao" : Hero;
            var reason = ol ? "skill-program.ol:guixin.obtain" : Reason;
            CharacterFixtureContent.Register(new OwnerBuilder(b, hero, retained), retained ? [hero, "happy:shen-wei-yan"] : [hero]);
            var consumeGain = retained ? "" : """
                ,{"op":"selectOwnedCards","target":"owner","minimumCards":1,"maximumCards":1,"zones":["hand"],"resultBind":"spent"},
                {"op":"moveBoundCards","target":"owner","sourceBind":"spent","destination":"discardPile","awaitMovementTriggers":true}
                """;
            var catalog = SkillProgramCatalog.Load($$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
              {"id":"{{Driver}}","revision":1,"activations":[{"id":"damage","targetKind":"anyLiving","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"usesPerTurn":null,"effects":[{"op":"damage","target":"owner","amount":{{(players == 6 && !retained ? 2 : 1)}}}]}]},
              {"id":"{{Watch}}","revision":1,"triggers":[{"id":"paid-gain","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["{{reason}}"],"optional":false,"isSkillInvocation":false,"effects":[
                {"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]}{{consumeGain}}]}]}
            ]}
            """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = new Dictionary<string, object>
            {
                [Driver] = new { name = "真实伤害点", description = "从真实自身伤害进入归心。" },
                [Watch] = new { name = "真实获得子窗", description = "在原段返回前使用本次获得的牌。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } }
            } }));
            foreach (var (id, p) in catalog.Programs) b.AddSkill(new(id, id, "欢乐归心共享结算检查") { Program = p, ProgramPresentation = catalog.Presentations[id] });
            var peers = Enumerable.Range(1, players - 1).Select(i => retained && i == 1 ? "happy:shen-wei-yan" : $"fixture:happy-guixin-peer-{i}").ToArray();
            foreach (var id in peers.Where(id => id != "happy:shen-wei-yan")) b.AddGeneral(new(id, "固定真实同桌", "", "standard:none", "wei", 8));
            b.AddDeck(new(Deck, "固定实体牌堆", retained ? 3 : 2, 0, [])
            { PhysicalCards = Enumerable.Range(0, 40).Select(i => new ContentDeckPhysicalCard(retained ? "standard:crossbow" : "standard:slash", (Suit)(i % 4), i % 13 + 1)).ToArray() });
            b.AddMode(new(Mode, "欢乐归心真实累计获得", players, players,
                new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = players - 1 }, Deck,
                GeneralCandidateCount: players, GeneralPoolIds: [hero, ..peers]));
        }
    }

    private sealed class OwnerBuilder(IContentRegistryBuilder b, string hero, bool retained) : IContentRegistryBuilder
    {
        public void AddGeneral(ContentGeneralDefinition d) => b.AddGeneral(d.Id == hero
            ? d with { AdditionalSkillIds = [..d.AdditionalSkillIds!, Driver, Watch] }
            : retained && d.Id == "happy:shen-wei-yan"
                ? d with { AdditionalSkillIds = [..d.AdditionalSkillIds!, Watch] } : d);
        public void AddSkill(ContentSkillDefinition d) => b.AddSkill(d);
        public void AddCard(ContentCardDefinition d) => b.AddCard(d);
        public void AddDeck(ContentDeckRecipe d) => b.AddDeck(d);
        public void AddMode(ContentModeDefinition d) => b.AddMode(d);
    }
}
