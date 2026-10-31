using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

namespace CardGame.Core.Tests;

internal static class HappyGuixinChecks
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

    private static long Sweep(Scene s, int count, bool coldFirst, bool offered = false)
    {
        if (!offered) s.Offer();
        s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        long frameId = 0;
        for (var cursor = 0; cursor < count; cursor++)
        {
            s.Reach(p => p.SkillPrompt?.SkillId == Watch && p.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "continue"));
            var frame = s.Parent; var receipt = frame.SeatAcquisition!; var paid = receipt.Payments.Last(); frameId = frame.Id;
            s.Need(receipt.Stage == ProgramSeatAcquisitionStage.MovementChildren && receipt.Cursor == cursor && receipt.Payments.Count == cursor + 1 &&
                receipt.OtherSeats.SequenceEqual(Enumerable.Range(1, count)) && paid.TargetSeat == cursor + 1 &&
                paid.From == CardLocation.Hand(cursor + 1) && paid.Movements is [{ To: var destination }] && destination == CardLocation.Hand(0) &&
                s.Zone(paid.CardId) == CardLocation.Hand(0) &&
                s.E<HappyGuixinMovementPaidEvent>().Count(e => e.FrameId == frame.Id) == cursor + 1,
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
            s.Need(s.G.CardMovements.Count(m => m.CardId == card && m.Reason.Value == Reason && m.To == CardLocation.Hand(0)) == 1,
                "Spending the obtained entity inside the gain observer does not duplicate its original paid acquisition.");
        }
        return frameId;
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry, int players)
    {
        public GameEngine G { get; private set; } = game;
        public PlayerSnapshot View => G.CreateSnapshot(0).Players.Single(p => p.Seat == 0);
        public PendingDecision? P => Enumerable.Range(0, players).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public ProgramSkillFrame Parent => G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Guixin && f.SeatAcquisition is not null);
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public CardLocation Zone(int card) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == card).Location;
        public void Play() => Reach(p => p is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Offer() => Reach(p => p.SkillPrompt?.SkillId == Guixin && p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
        public void UseDamage() => Accept(new UseProgramSkillCommand(0, Driver, "damage", [], [], G.Revision, P!.PromptId));
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
        public static Scene Start(int players)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(players));
            var g = GameEngine.CreateStandard(new GameOptions { Seed = 7, PlayerCount = players, HumanSeat = 0, HumanRole = Role.Lord,
                ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, registry);
            var s = new Scene(g, registry, players); s.Accept(new StartGameCommand()); s.Reach(p => p.Kind == DecisionKind.SelectGeneral);
            s.Accept(new SelectGeneralCommand(0, Hero, g.Revision, s.P!.PromptId)); s.Play();
            s.Need(s.View.HandCount == 2 && !s.View.IsFaceDown && Enumerable.Range(1, players - 1).All(i => g.CreateSnapshot(i).Players.Single(p => p.Seat == i).HandCount == 2),
                "A fixed two-card, zero-normal-draw fixture starts with real cards and an initially face-up owner.");
            return s;
        }
    }

    private sealed class Fixture(int players) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:happy-guixin", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(new OwnerBuilder(b), Hero);
            var catalog = SkillProgramCatalog.Load($$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
              {"id":"{{Driver}}","revision":1,"activations":[{"id":"damage","targetKind":"anyLiving","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"usesPerTurn":null,"effects":[{"op":"damage","target":"owner","amount":{{(players == 6 ? 2 : 1)}}]}]},
              {"id":"{{Watch}}","revision":1,"triggers":[{"id":"paid-gain","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["{{Reason}}"],"optional":false,"isSkillInvocation":false,"effects":[
                {"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]},
                {"op":"selectOwnedCards","target":"owner","minimumCards":1,"maximumCards":1,"zones":["hand"],"resultBind":"spent"},
                {"op":"moveBoundCards","target":"owner","sourceBind":"spent","destination":"discardPile","awaitMovementTriggers":true}]}]}
            ]}
            """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = new Dictionary<string, object>
            {
                [Driver] = new { name = "真实伤害点", description = "从真实自身伤害进入归心。" },
                [Watch] = new { name = "真实获得子窗", description = "在原段返回前使用本次获得的牌。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } }
            } }));
            foreach (var (id, p) in catalog.Programs) b.AddSkill(new(id, id, "欢乐归心共享结算检查") { Program = p, ProgramPresentation = catalog.Presentations[id] });
            var peers = Enumerable.Range(1, players - 1).Select(i => $"fixture:happy-guixin-peer-{i}").ToArray();
            foreach (var id in peers) b.AddGeneral(new(id, "固定真实同桌", "", "standard:none", "wei", 8));
            b.AddDeck(new(Deck, "固定实体杀牌堆", 2, 0, [])
            { PhysicalCards = Enumerable.Range(0, 40).Select(i => new ContentDeckPhysicalCard("standard:slash", (Suit)(i % 4), i % 13 + 1)).ToArray() });
            b.AddMode(new(Mode, "欢乐归心真实累计获得", players, players,
                new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = players - 1 }, Deck,
                GeneralCandidateCount: players, GeneralPoolIds: [Hero, ..peers]));
        }
    }

    private sealed class OwnerBuilder(IContentRegistryBuilder b) : IContentRegistryBuilder
    {
        public void AddGeneral(ContentGeneralDefinition d) => b.AddGeneral(d.Id == Hero ? d with { AdditionalSkillIds = [..d.AdditionalSkillIds!, Driver, Watch] } : d);
        public void AddSkill(ContentSkillDefinition d) => b.AddSkill(d);
        public void AddCard(ContentCardDefinition d) => b.AddCard(d);
        public void AddDeck(ContentDeckRecipe d) => b.AddDeck(d);
        public void AddMode(ContentModeDefinition d) => b.AddMode(d);
    }
}
