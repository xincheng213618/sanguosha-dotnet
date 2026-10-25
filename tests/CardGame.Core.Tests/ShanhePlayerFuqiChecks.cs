using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class ShanhePlayerFuqiChecks
{
    private const string Mode = "identity:classic-player-fuqi-fixture";
    private const string Deck = "fixture:player-fuqi-deck";
    private static string General(int seat) => "fixture:player-fuqi-seat-" + seat;
    private static string HandSkill(int seat) => "fixture:player-fuqi-hand-" + seat;

    public static void NearbyNonTargetsAndDelayedCardsRespectDistance()
    {
        var s = Scene.Start(counterspell: false);
        var slash = s.V(2).Hand.Single(c => c.Kind == CardKind.Slash).Id;
        Need(s.G.GetCombatDistance(0, 1) == 1 && s.G.GetCombatDistance(0, 2) == 2 &&
            s.G.GetCombatDistance(0, 3) == 1, "The native table supplies two nearby non-targets and one distant Duel target.");
        var duel = s.BeginCard(0, CardKind.Duel, 2);
        s.Prompt(DecisionKind.Nullification, 2);
        Need(s.P!.Choices.Any(c => c.Parameters.GetValueOrDefault("response") == "nullification"),
            "The distance-two target retains its native counterspell choice.");
        s.Pass();
        s.Prompt(DecisionKind.Nullification, 0);
        Need(s.P!.Choices.Any(c => c.Parameters.GetValueOrDefault("response") == "nullification"),
            "Fuqi's owner may respond to his own actual trick.");
        s.Pass();
        s.Prompt(DecisionKind.RespondSlash, 2);
        s.Answer(c => c.Cards.Contains(slash));
        s.Until(() => s.P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } or
            { Kind: DecisionKind.RespondSlash, PlayerSeat: 0 });
        if (s.P is { Kind: DecisionKind.RespondSlash }) s.Pass();
        s.Prompt(DecisionKind.PlayCard, 0);
        var use = s.E<CardUseDeclaredEvent>().Single(e => e.CardId == duel).ResolutionId;
        Need(s.E<NullificationRequestedEvent>().Where(e => e.ResolutionId == use)
                .Select(e => e.ResponderSeat).SequenceEqual([2, 0]) &&
            s.E<CardRespondedEvent>().Any(e => e.CardId == slash && e.ResponderSeat == 2 && e.EffectiveCardKind == CardKind.Slash) &&
            s.G.CardMovements.Count(m => m.CardId == slash && m.From == CardLocation.Hand(2) && m.Reason == CardMoveReasons.Respond) == 1 &&
            s.E<DamageAppliedEvent>().Single(e => e.SourceCardUseResolutionId == use) is { SourceSeat: 2, TargetSeat: 0, Amount: 1 },
            "Nearby non-target holders never get a counterspell prompt, while the distant target really pays its Slash and wins the native Duel exchange.");

        var lightning = s.BeginCard(0, CardKind.Lightning, 0);
        s.Prompt(DecisionKind.PlayCard, 0);
        Need(s.V(0).Judgment.Any(c => c.Id == lightning) &&
            !s.E<NullificationRequestedEvent>().Any(e => e.EffectCardId == lightning),
            "The paid Lightning enters its owner's judgment area through native delayed-card placement.");
        s.EndPlay();
        s.Until(() => s.P is { Kind: DecisionKind.Nullification, PlayerSeat: 2 }, endOtherPlayPhases: true);
        var window = s.G.ResolutionStack.OfType<NullificationWindowFrame>().Single();
        Need(window is { EffectCardKind: CardKind.Lightning, SourceSeat: 0, ChainDepth: 0 } &&
            window.DelayedReturn?.CardId == lightning && window.TargetSeats.SequenceEqual([0]),
            "The next native judgment opens an actual delayed-trick window with the holder as source.");
        s.Pass();
        s.Prompt(DecisionKind.Nullification, 0);
        var counter = s.V(0).Hand.Single(c => c.Kind == CardKind.Nullification).Id;
        s.Answer(c => c.Parameters.GetValueOrDefault("response") == "nullification" && c.Cards.Contains(counter));
        s.Prompt(DecisionKind.Nullification, 2);
        s.Pass();
        s.Prompt(DecisionKind.PlayCard, 0);
        Need(s.E<NullificationRequestedEvent>().Where(e => e.WindowFrameId == window.Id && e.ChainDepth == 0)
                .Select(e => e.ResponderSeat).SequenceEqual([2, 0]) &&
            s.E<NullificationRequestedEvent>().Where(e => e.WindowFrameId == window.Id).All(e => e.ResponderSeat is 0 or 2) &&
            s.E<NullificationResolvedEvent>().Single(e => e.WindowFrameId == window.Id) is { EffectNullified: true, ChainDepth: 1 } &&
            s.G.CardMovements.Count(m => m.CardId == counter && m.From == CardLocation.Hand(0) && m.Reason == CardMoveReasons.Nullification) == 1 &&
            s.G.CardMovements.Any(m => m.CardId == lightning && m.From == CardLocation.Judgment(0) &&
                m.To == CardLocation.Judgment(1) && m.Reason == CardMoveReasons.DelayedCardTransfer) &&
            s.V(1).Hand.Any(c => c.Kind == CardKind.Nullification) && s.V(3).Hand.Any(c => c.Kind == CardKind.Nullification),
            "Fuqi also prohibits nearby non-targets during the delayed window, permits the owner's real self-counterspell, and preserves distant responses and the native canceled-Lightning transfer.");
    }

    public static void CounterspellUsesLatestActualUserAndDistance()
    {
        var s = Scene.Start(counterspell: true);
        s.EndPlay();
        s.Prompt(DecisionKind.PlayCard, 1);
        var ownCounter = s.V(0).Hand.Single(c => c.Kind == CardKind.Nullification).Id;
        var distantCounter = s.V(2).Hand.Single(c => c.Kind == CardKind.Nullification).Id;
        var draw = s.BeginCard(1, CardKind.DrawTwo);
        s.Prompt(DecisionKind.Nullification, 2); s.Pass();
        s.Prompt(DecisionKind.Nullification, 3); s.Pass();
        s.Prompt(DecisionKind.Nullification, 0);
        var window = s.G.ResolutionStack.OfType<NullificationWindowFrame>().Single();
        Need(window is { SourceSeat: 1, EffectCardKind: CardKind.DrawTwo, ChainDepth: 0 },
            "The original native trick user has no Fuqi policy.");
        s.Answer(c => c.Parameters.GetValueOrDefault("response") == "nullification" && c.Cards.Contains(ownCounter));
        s.Prompt(DecisionKind.Nullification, 2);
        Need(s.G.GetCombatDistance(0, 2) == 2 && s.G.GetCombatDistance(1, 2) == 1 &&
            s.E<NullificationRespondedEvent>().Single(e => e.WindowFrameId == window.Id) is { ResponderSeat: 0, ChainDepth: 1 } &&
            s.P!.Choices.Any(c => c.Cards.Contains(distantCounter)),
            "The counterspell now responds to actual user zero: seat two is distant from that user even though it is adjacent to the original trick user.");
        s.Answer(c => c.Parameters.GetValueOrDefault("response") == "nullification" && c.Cards.Contains(distantCounter));
        s.Prompt(DecisionKind.Nullification, 3); s.Pass();
        s.Prompt(DecisionKind.Nullification, 1); s.Pass();
        s.Prompt(DecisionKind.PlayCard, 1);
        Need(s.E<NullificationRequestedEvent>().Where(e => e.WindowFrameId == window.Id && e.ChainDepth == 1)
                .Select(e => e.ResponderSeat).SequenceEqual([2]) &&
            s.E<NullificationRequestedEvent>().Where(e => e.WindowFrameId == window.Id && e.ChainDepth == 2)
                .Select(e => e.ResponderSeat).SequenceEqual([3, 1]) &&
            s.E<NullificationRespondedEvent>().Where(e => e.WindowFrameId == window.Id)
                .Select(e => e.ResponderSeat).SequenceEqual([0, 2]) &&
            s.E<NullificationResolvedEvent>().Single(e => e.WindowFrameId == window.Id) is { EffectNullified: false, ChainDepth: 2 } &&
            s.G.CardMovements.Count(m => m.CardId == ownCounter && m.From == CardLocation.Hand(0) && m.Reason == CardMoveReasons.Nullification) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == distantCounter && m.From == CardLocation.Hand(2) && m.Reason == CardMoveReasons.Nullification) == 1 &&
            s.G.CardMovements.Count(m => m.To == CardLocation.Hand(1) && m.Reason == CardMoveReasons.Draw) == 2 &&
            s.E<CardUseFinishedEvent>().Any(e => e.CardId == draw),
            "The actual Fuqi counterspell bans seats one and three, then the distant non-Fuqi user's paid counterspell reopens both; each material is paid once and the original DrawTwo resumes exactly once.");
    }

    private static void Need(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(seat => G.CreateSnapshot(seat).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players[seat];
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public void Accept(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.ToString() ?? "The native player-Fuqi command was rejected."); }
        public void Answer(Func<PromptChoice, bool> predicate)
        { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(predicate).Id, G.Revision)); }
        public void Pass() => Answer(c => c.Parameters.GetValueOrDefault("response") == "pass");
        public void EndPlay() => Accept(new EndPlayPhaseCommand(P!.PlayerSeat, G.Revision, P.PromptId));
        public void Prompt(DecisionKind kind, int seat) => Until(() => P is { } p && p.Kind == kind && p.PlayerSeat == seat);
        public int BeginCard(int actor, CardKind kind, int? target = null)
        {
            var hand = V(actor).Hand;
            var card = hand.FirstOrDefault(c => c.Kind == kind);
            Need(card is not null, $"Seat {actor} has no physical {kind}: " + JsonSerializer.Serialize(new { P, Hand = hand }));
            var legal = G.GetLegalActions(actor);
            var matches = legal.Where(a => a.CardId == card!.Id && (target is { } seat
                ? a.TargetSeats.SequenceEqual([seat]) : a.TargetSeats.Count == 0)).ToArray();
            Need(matches.Length == 1, $"Expected one native {kind} action for seat {actor} with target {target?.ToString() ?? "implicit"}, got {matches.Length}: " +
                JsonSerializer.Serialize(new { P, Hand = hand, LegalActions = legal }));
            var action = matches[0];
            Accept(new PlayCardCommand(actor, card!.Id, action.TargetSeats, G.Revision, P!.PromptId,
                action.PlayedCardKind, action.TargetCardId) { ConversionSource = action.ConversionSource });
            return card!.Id;
        }
        public void Until(Func<bool> done, bool endOtherPlayPhases = false)
        {
            for (var n = 0; n < 160; n++)
            {
                if (done()) return;
                Need(G.State.Winner == Winner.None, "The short native fixture must retain all four participants.");
                if (endOtherPlayPhases && P is { Kind: DecisionKind.PlayCard }) { EndPlay(); continue; }
                Need(P is null, "Unexpected native player-Fuqi prompt: " + JsonSerializer.Serialize(P));
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("The bounded player-Fuqi continuation was not reached.");
        }
        public static Scene Start(bool counterspell)
        {
            CardKind[][] hands = counterspell
                ? [[CardKind.Nullification], [CardKind.DrawTwo, CardKind.Nullification], [CardKind.Nullification], [CardKind.Nullification]]
                : [[CardKind.Duel, CardKind.Lightning, CardKind.Nullification], [CardKind.Nullification], [CardKind.Slash, CardKind.Nullification], [CardKind.Nullification]];
            var stock = Enumerable.Range(0, 48).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Spade, 7)).ToArray();
            var probe = Boot(ContentRegistry.Build(new StandardContentPackage(), new StandardShanheFuqiPackage(), new Fixture(hands, stock)));
            for (var seat = 0; seat < 4; seat++)
            {
                var ids = probe.V(seat).Hand.Select(c => c.Id).ToArray();
                Need(ids.Length == hands[seat].Length, "The fixed native opening preserves each sparse hand size.");
                for (var i = 0; i < ids.Length; i++)
                    stock[ids[i] - 1] = new(probe.G.ContentRegistry.Cards.Values.Single(c => c.LegacyKind == hands[seat][i]).Id, Suit.Spade, 7);
            }
            return Boot(ContentRegistry.Build(new StandardContentPackage(), new StandardShanheFuqiPackage(), new Fixture(hands, stock)));
        }
        private static Scene Boot(ContentRegistry registry)
        {
            var g = GameEngine.CreateStandard(new()
            {
                Seed = 83, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
                ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 8
            }, registry);
            var s = new Scene(g); s.Accept(new StartGameCommand());
            for (var n = 0; n < 40; n++)
            {
                if (s.P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }) return s;
                if (s.P is { Kind: DecisionKind.SelectGeneral } p)
                { s.Accept(new SelectGeneralCommand(p.PlayerSeat, General(p.PlayerSeat), g.Revision, p.PromptId)); continue; }
                Need(s.P is null, "The native opening exposes only legal general choices before Play.");
                s.Accept(new AdvanceOneStepCommand(g.Revision));
            }
            throw new InvalidOperationException("The fixed native opening did not reach the Lord's Play phase.");
        }
    }

    private sealed class Fixture(CardKind[][] hands, IReadOnlyList<ContentDeckPhysicalCard> stock) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-player-fuqi", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load(JsonSerializer.Serialize(new
            {
                schemaVersion = SkillProgramCatalog.RulesSchemaVersion,
                skills = Enumerable.Range(0, 4).Select(seat => new
                {
                    id = HandSkill(seat), revision = 1,
                    modifiers = new[] { new { id = "sparse-hand", query = "initialHandSize", operation = "add", value = hands[seat].Length, priority = 0 } }
                })
            }), JsonSerializer.Serialize(new
            {
                schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = Enumerable.Range(0, 4).ToDictionary(HandSkill, _ => new { name = "固定原生手牌", description = "从固定实体牌库正常发牌。" })
            }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                    { Program = program, ProgramPresentation = catalog.Presentations[id] });
            for (var seat = 0; seat < 4; seat++)
                builder.AddGeneral(new(General(seat), "原生伏骑同桌" + seat, "supporter",
                    seat == 0 ? StandardShanheFuqiPackage.SkillId : HandSkill(seat), "qun", 12,
                    seat == 0 ? [HandSkill(seat)] : []));
            builder.AddDeck(new(Deck, "原生伏骑实体牌库", 0, 0, []) { PhysicalCards = stock });
            builder.AddMode(new(Mode, "玩家伏骑原生响应", 4, 4,
                new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 3 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Enumerable.Range(0, 4).Select(General).ToArray()));
        }
    }
}
