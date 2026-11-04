using System.Reflection;
using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class NativeCardRepresentationChecks
{
    public static void OriginalFacesAndPhysicalDeparturesKeepOwningUseBoundaries()
    {
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 7 }, StandardContentRegistry.Create());
        var capabilities = (ProgramCapabilityRegistry)typeof(GameEngine)
            .GetProperty("ProgramCapabilities", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(game)!;
        var frames = capabilities.Get<IProgramRuntimeFrames>();
        var zones = (CardZoneStore)capabilities.Get<ICardZoneQueries>();
        var journal = new List<CardMovementRecord>();
        var history = new CardMovementHistoryQueries(journal);
        var runtime = new NativeCardRepresentationRuntime(frames, zones, history,
            capabilities.Get<INativeMaterialClaims>());
        var priorTop = frames.Top;
        CheckOriginalFaces(frames, zones, runtime);
        CheckPhysicalDepartures(frames, zones, history, journal, runtime);
        Need(ReferenceEquals(priorTop, frames.Top), "Representation checks return to the original scheduler frame.");
    }

    private static void CheckOriginalFaces(IProgramRuntimeFrames frames, CardZoneStore zones,
        INativeCardRepresentations runtime)
    {
        var printed = zones.CardsAt(CardLocation.DrawPile).First();
        zones.Move(printed.Id, CardLocation.DrawPile, CardLocation.Processing);
        var effectiveKind = printed.Kind == CardKind.Duel ? CardKind.FireAttack : CardKind.Duel;
        var effective = printed with { Kind = effectiveKind, Suit = Suit.Heart, Rank = 13 };
        var physical = new CardUseFrame(frames.NextId(), 0, printed.Id, effectiveKind, [1], PhysicalCardIds: [printed.Id])
            { NativeRepresentation = effective };
        frames.Push(physical);
        try
        {
            Need(SameFace(runtime.Get(physical.Id, printed.Id, true), printed),
                "A converted native use reads the physical store's original printed face, independently of its effective kind.");
            Need(SameFace(runtime.ReadAppearance(physical.Id, new(printed.Id, effectiveKind, Suit.Diamond, 9)),
                    printed with { Kind = effectiveKind, Suit = Suit.Diamond, Rank = 9 }),
                "A continuation's explicit effective appearance overlays the real physical entity without changing its printed face.");
            Reject(() => runtime.Get(physical.Id, 0));
            var foreign = zones.CardsAt(CardLocation.DrawPile).First();
            Reject(() => runtime.Get(physical.Id, foreign.Id));
            Need(SameFace(zones.CardsAt(CardLocation.Processing).Single(card => card.Id == printed.Id), printed),
                "Reading an effective continuation never rewrites the entity in the physical store.");
        }
        finally { frames.Pop(physical.Id, physical.Kind); }

        var grantedSkills = new List<string> { "fixture:representation-grant" };
        var zeroCard = new Card(0, CardKind.DrawTwo, Suit.Club, 11) { GrantedSkillIds = grantedSkills };
        var zero = new CardUseFrame(frames.NextId(), 0, 0, zeroCard.Kind, [1], PhysicalCardIds: [])
            { NativeRepresentation = zeroCard };
        grantedSkills.Add("fixture:later-mutation");
        Need(zero.NativeRepresentation!.GrantedSkillIds.SequenceEqual(["fixture:representation-grant"]) &&
                zero.NativeRepresentation.GrantedSkillIds is System.Collections.IList { IsReadOnly: true },
            "The new frame-owned representation freezes its nested granted-skill collection at issuance.");
        frames.Push(zero);
        var nested = new CardUseFrame(frames.NextId(), 1, 0, CardKind.Duel, [0], PhysicalCardIds: [])
            { NativeRepresentation = new(0, CardKind.Duel, Suit.None, 0) };
        frames.Push(nested);
        try
        {
            Need(SameFace(runtime.Get(zero.Id, 0, true), zeroCard) && runtime.Get(nested.Id, 0).Kind == CardKind.Duel,
                "A zero-entity card retains its original suit and rank and cannot borrow the innermost use's representation.");
            var round = zero with { TargetIndex = 1, ExtraCardEffect = new()
                { Source = new("fixture:representation", "repeat", 0, "fixture:instance"),
                  ProgramFrameId = 0, ActionId = 0, PaidCardId = 0, WholeGroupRepeated = true } };
            frames.Replace(zero.Id, JsonSerializer.Deserialize<CardUseFrame>(JsonSerializer.Serialize(round))!);
            Need(SameFace(runtime.ReadAppearance(zero.Id, new(0, zeroCard.Kind, zeroCard.Suit, zeroCard.Rank)), zeroCard) &&
                    ((CardUseFrame)frames.Find(zero.Id)!).ExtraCardEffect is { WholeGroupRepeated: true },
                "Frame replacement, an extra effect and JSON recovery retain the originally issued zero-entity face.");
            Reject(() => runtime.ReadAppearance(zero.Id, new(0, CardKind.Duel, zeroCard.Suit, zeroCard.Rank)));
            Reject(() => runtime.ReadAppearance(zero.Id, new(0, zeroCard.Kind, Suit.None, zeroCard.Rank)));
            Reject(() => runtime.ReadAppearance(zero.Id, new(0, zeroCard.Kind, zeroCard.Suit, 0)));
            Reject(() => runtime.Get(long.MaxValue, 0));
            var effectFace = new CardAppearanceReference(0, CardKind.BarbarianAssault, zeroCard.Suit, zeroCard.Rank);
            var current = (CardUseFrame)frames.Find(zero.Id)!;
            var effect = current with { Continuations = current.Continuations with { GroupCard = new()
                { SourceSeat = 0, DamageSourceSeat = 0, Card = effectFace, TargetSeats = [1],
                  Effect = GroupCardEffect.ResponseAttack, RequiredCardKind = CardKind.Slash, PhysicalCardIds = [] } } };
            frames.Replace(zero.Id, JsonSerializer.Deserialize<CardUseFrame>(JsonSerializer.Serialize(effect))!);
            Need(SameFace(runtime.ReadAppearance(zero.Id, effectFace), zeroCard with { Kind = effectFace.Kind }) &&
                    SameFace(runtime.Get(zero.Id, 0), zeroCard),
                "A real issued group continuation may retain a different effect face through JSON recovery while its original zero-entity use representation stays unchanged.");
            Reject(() => runtime.ReadAppearance(zero.Id, effectFace with { Rank = 0 }));
            Reject(() => runtime.ReadAppearance(nested.Id, effectFace));
            frames.Replace(zero.Id, current);
            Reject(() => runtime.ReadAppearance(zero.Id, effectFace));
        }
        finally
        {
            frames.Pop(nested.Id, nested.Kind);
            frames.Pop(zero.Id, zero.Kind);
        }

        var carrier = zones.CardsAt(CardLocation.DrawPile).First();
        zones.Move(carrier.Id, CardLocation.DrawPile, CardLocation.Judgment(1));
        var carried = new CardUseFrame(frames.NextId(), 0, carrier.Id, CardKind.Lightning, [1], PhysicalCardIds: [])
            { NativeRepresentation = carrier, NativeRepresentationIsCarrier = true };
        frames.Push(carried);
        try
        {
            frames.Replace(carried.Id, JsonSerializer.Deserialize<CardUseFrame>(JsonSerializer.Serialize(carried))!);
            Need(SameFace(runtime.Get(carried.Id, carrier.Id, true), carrier) &&
                    ((CardUseFrame)frames.Find(carried.Id)!).PhysicalCardIds is { Count: 0 },
                "A declared physical carrier remains readable after placement without becoming a paid material.");
            Reject(() => runtime.Get(carried.Id, printed.Id, true));
        }
        finally { frames.Pop(carried.Id, carried.Kind); }
    }

    private static void CheckPhysicalDepartures(IProgramRuntimeFrames frames, CardZoneStore zones,
        ICardMovementHistoryQueries history, List<CardMovementRecord> journal, INativeCardRepresentations runtime)
    {
        var cards = zones.CardsAt(CardLocation.DrawPile).Take(3).ToArray();
        foreach (var card in cards) Move(card, CardLocation.Processing);
        var issuedAfter = journal.Count;
        var previousMovement = journal.Single(move => move.CardId == cards[0].Id).Sequence;
        var use = new CardUseFrame(frames.NextId(), 0, cards[0].Id, CardKind.Duel, [1],
            PhysicalCardIds: [cards[0].Id, cards[1].Id])
            { NativeRepresentation = cards[0], NativeRepresentationIssuedAfterMovementSequence = issuedAfter };
        var foreign = new CardUseFrame(frames.NextId(), 1, cards[2].Id, cards[2].Kind, [0],
            PhysicalCardIds: [cards[2].Id]) { NativeRepresentation = cards[2] };
        frames.Push(use);
        frames.Push(foreign);
        try
        {
            var departure = Move(cards[0], CardLocation.OutsideGame);
            Reject(() => runtime.Get(use.Id, cards[0].Id, true));
            Reject(() => runtime.CaptureDeparture(use.Id, cards[0].Id, previousMovement));
            Reject(() => runtime.CaptureDeparture(use.Id, cards[0].Id, long.MaxValue));
            Reject(() => runtime.CaptureDeparture(use.Id, cards[2].Id, departure));
            Reject(() => runtime.CaptureDeparture(foreign.Id, cards[0].Id, departure));
            runtime.CaptureDeparture(use.Id, cards[0].Id, departure);
            var captured = (CardUseFrame)frames.Find(use.Id)!;
            runtime.CaptureDeparture(use.Id, cards[0].Id, departure);
            Need(ReferenceEquals(captured, frames.Find(use.Id)) && captured.NativeRepresentationDepartures is
                    [{ CardId: var id, Destination: var destination, MovementSequence: var sequence }] &&
                    id == cards[0].Id && destination == CardLocation.OutsideGame && sequence == departure &&
                    SameFace(runtime.Get(use.Id, cards[0].Id, true), cards[0]),
                "One actual material departure issues an exact, idempotent destination receipt on its owning use.");
            Need(captured.NativeRepresentationDepartures is System.Collections.IList { IsReadOnly: true },
                "Exposed departure receipts are frozen on the original frame.");
            frames.Replace(use.Id, JsonSerializer.Deserialize<CardUseFrame>(JsonSerializer.Serialize(captured with
                { TargetIndex = 1 }))!);
            Need(SameFace(runtime.Get(use.Id, cards[0].Id, true), cards[0]),
                "The departure lifetime survives an ordinary continuation and JSON recovery.");
            Move(cards[0], CardLocation.Hand(0));
            Reject(() => runtime.Get(use.Id, cards[0].Id, true));
            Need(SameFace(runtime.Get(use.Id, cards[0].Id), cards[0]),
                "A departure authorizes only its actual destination; an unrestricted read still finds the current physical entity.");

            var reason = new CardMoveReason("fixture.representation-claim");
            var before = journal.Count;
            Move(cards[1], CardLocation.Hand(1), reason);
            new CardUseMaterialClaimRuntime(frames, history, zones).Capture(use.Id, 1, [cards[1]], before, reason);
            Need(SameFace(runtime.Get(use.Id, cards[1].Id, true), cards[1]) &&
                    ((CardUseFrame)frames.Find(use.Id)!).NativeRepresentationDepartures.Count == 1,
                "An existing shared material claim permits the original physical entity without creating another departure ledger.");
            Move(cards[1], CardLocation.Equipment(1));
            Reject(() => runtime.Get(use.Id, cards[1].Id, true));
        }
        finally
        {
            frames.Pop(foreign.Id, foreign.Kind);
            frames.Pop(use.Id, use.Kind);
        }

        long Move(Card card, CardLocation destination, CardMoveReason? reason = null)
        {
            var source = zones.GetLocation(card.Id);
            zones.Move(card.Id, source, destination);
            var sequence = journal.Count + 1;
            journal.Add(new(sequence, 0, card.Id, card.Kind, source, destination,
                reason ?? new CardMoveReason("fixture.representation-movement")));
            return sequence;
        }
    }

    private static bool SameFace(Card actual, Card expected) => actual.Id == expected.Id && actual.Kind == expected.Kind &&
        actual.Suit == expected.Suit && actual.Rank == expected.Rank;
    private static void Need(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }
    private static void Reject(Action action)
    {
        try { action(); }
        catch (InvalidOperationException) { return; }
        throw new InvalidOperationException("A native representation escaped its original card, frame or physical destination boundary.");
    }
}
