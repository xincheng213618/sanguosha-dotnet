#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuoXianOlRuntime
{
    internal bool TracksPublicHandReveals => _contentRegistryOperations._contentRegistry?.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveDailiTurnEnd) == true;

    internal long? CurrentHandEntry(int owner, int id)
    {
        if (_cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Hand(owner))
            return null;
        for (var index = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count - 1; index >= 0; index--)
        {
            var movement = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements[index];
            if (movement.CardId == id && movement.From != movement.To)
                return movement.To == CardLocation.Hand(owner) ? movement.Sequence : null;
        }

        return 0;
    }

    internal IEnumerable<PublicHandEntityRevealedEvent> CurrentPublicHandReveals() => !TracksPublicHandReveals ? [] : _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<PublicHandEntityRevealedEvent>().GroupBy(e => (e.OwnerSeat, e.Card.Id, e.HandEntryMovementSequence)).Select(g => g.Last()).Where(e => CurrentHandEntry(e.OwnerSeat, e.Card.Id) == e.HandEntryMovementSequence);
    internal int PubliclyRevealedHandCount(int owner) => CurrentPublicHandReveals().Count(e => e.OwnerSeat == owner);
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 26420)]
    internal void ObserveExplicitPublicHandReveal(IGameEvent payload)
    {
        if (!TracksPublicHandReveals)
            return;
        var(frame, ids) = payload switch
        {
            ProgramCardsRevealedEvent e => (e.FrameId, e.Cards.Select(c => c.Id)),
            CardsRevealedEvent e => (e.ResolutionId, e.Cards.Select(c => c.Id)),
            FireAttackCardRevealedEvent e => (e.ResolutionId, new[] { e.CardId }.AsEnumerable()),
            ProgramHandComparisonSourceRevealedEvent e => (e.FrameId, new[] { e.CardId }.AsEnumerable()),
            ProgramHandComparisonCardRevealedEvent e => (e.FrameId, new[] { e.CardId }.AsEnumerable()),
            _ => (0L, Enumerable.Empty<int>())};
        foreach (var id in ids.Distinct().ToArray())
        {
            var location = _cardZonesOperations._cardZones.GetLocation(id);
            if (location is not { Zone: CardZoneKind.Hand, OwnerSeat: { } owner } || CurrentHandEntry(owner, id)is not { } entry)
                continue;
            if (_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<PublicHandEntityRevealedEvent>().Any(e => e.OwnerSeat == owner && e.Card.Id == id && e.HandEntryMovementSequence == entry))
                continue;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PublicHandEntityRevealedEvent(frame, owner, entry, _coreSnapshotValueProjectionOperations.ToSnapshot(_coreAdvancedLifecycleOperations.GetAdvancedCard(id))));
        }
    }
}
