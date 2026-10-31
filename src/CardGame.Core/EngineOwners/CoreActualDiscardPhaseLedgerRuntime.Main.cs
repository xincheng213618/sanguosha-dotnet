#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreActualDiscardPhaseLedgerRuntime
{
    internal void CollectDiscardPhaseHandDiscard(Card card, CardLocation from, CardLocation to)
    {
        if (_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Discard || from is not { Zone: CardZoneKind.Hand, OwnerSeat: { } ownerSeat } || ownerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || to.Zone != CardZoneKind.DiscardPile)
        {
            return;
        }

        if (_coreActualDiscardLedgerDiscardPhaseHandDiscardIdsDiscardPhaseHandDiscardTurnNumberAccess._discardPhaseHandDiscardTurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber)
        {
            _coreActualDiscardLedgerDiscardPhaseHandDiscardIdsDiscardPhaseHandDiscardTurnNumberAccess._discardPhaseHandDiscardTurnNumber = _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber;
            _coreActualDiscardLedgerDiscardPhaseHandDiscardIdsDiscardPhaseHandDiscardTurnNumberAccess._discardPhaseHandDiscardIds.Clear();
        }

        _coreActualDiscardLedgerDiscardPhaseHandDiscardIdsDiscardPhaseHandDiscardTurnNumberAccess._discardPhaseHandDiscardIds.Add(card.Id);
    }

    internal int TurnOwnerDiscardPhaseHandDiscardCount => _coreActualDiscardLedgerDiscardPhaseHandDiscardIdsDiscardPhaseHandDiscardTurnNumberAccess._discardPhaseHandDiscardTurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber ? _coreActualDiscardLedgerDiscardPhaseHandDiscardIdsDiscardPhaseHandDiscardTurnNumberAccess._discardPhaseHandDiscardIds.Count : 0;

    internal SkillProgramStepOutcome BindProgramDiscardPhaseDiscards(ProgramSkillFrame frame, string resultBind)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (active.OwnerSeat != frame.OwnerSeat || active.SkillId != frame.SkillId)
            throw new InvalidOperationException("A discard-phase bind requires the active program frame.");
        var context = active.WindowContext ?? throw new InvalidOperationException("A discard-phase bind requires a trigger window context.");
        if (context.Window is not SkillProgramTriggerWindow.TurnEnding)
            throw new InvalidOperationException("A discard-phase bind requires a turnEnding window.");
        var discardPileIds = _cardZonesOperations._cardZones.CardsAt(CardLocation.DiscardPile).Select(card => card.Id).ToHashSet();
        var poolIds = _coreActualDiscardLedgerDiscardPhaseHandDiscardIdsDiscardPhaseHandDiscardTurnNumberAccess._discardPhaseHandDiscardTurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber ? _coreActualDiscardLedgerDiscardPhaseHandDiscardIdsDiscardPhaseHandDiscardTurnNumberAccess._discardPhaseHandDiscardIds.Where(discardPileIds.Contains).Take(CardSubsetSelector.MaximumCandidateCount).ToArray() : [];
        _coreProgramLifecycleOperations.SetProgramCardSet(frame.Id, resultBind, poolIds, SkillProgramCardSetVisibility.Public);
        return SkillProgramStepOutcome.Continue;
    }
}
