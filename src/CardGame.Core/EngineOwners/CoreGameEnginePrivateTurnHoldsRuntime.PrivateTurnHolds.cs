#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEnginePrivateTurnHoldsRuntime
{
    SkillProgramStepOutcome global::CardGame.Core.IPrivateTurnHoldProgramHost.HoldOwnerHandUntilTurnEnd(ProgramSkillFrame f) => HoldOwnerHandUntilTurnEnd(f);
    internal SkillProgramStepOutcome HoldOwnerHandUntilTurnEnd(ProgramSkillFrame f)
    {
        if (f.PrivateTurnHoldDraft is not null)
            throw new InvalidOperationException("A hold cost cannot be paid twice.");
        var cards = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).ToArray();
        if (cards.Length == 0)
            return SkillProgramStepOutcome.Continue;
        var grant = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].SkillGrants.Grants.Single(g => g.SkillId == f.SkillId && g.SkillInstanceId == f.SkillInstanceId);
        var location = new CardLocation(CardZoneKind.PrivateTurnHold, f.OwnerSeat, privateTurnHold: new(f.Id, f.SkillId, f.SkillInstanceId, grant.SourceId, _coreActualTurnProgressTurnNumberAccess._turnNumber));
        _cardZonesOperations._cardZones.EnsurePrivateTurnHold(location);
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { PrivateTurnHoldDraft = new(location, true), PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _corePhysicalMovementFacadesOperations.MoveCards(cards, CardLocation.Hand(f.OwnerSeat), location, new("skill.private-turn-hold.pay"));
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreRuntimeOperations.ReturnRuntimeProgramMovement(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 880)]
    internal bool ResumePrivateTurnHold(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        if (f.PrivateTurnHoldDraft is not { } draft)
            return false;
        if (!draft.Paid || draft.Location.PrivateTurnHold?.HoldId != id)
            throw new InvalidOperationException("A private hold lost its paid owning draft.");
        if (_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            return true;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { PrivateTurnHoldDraft = null });
        return false;
    }

    internal bool HasActualTurnEndMovementPrelude => _coreAfterTurnEndedProgramsOperations.HasAfterTurnEndedPrograms || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.HoldOwnerHandUntilTurnEnd) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.HoldUsedSuitHandAndDraw);

    internal void ReturnPrivateTurnHoldsAtTurnEnd()
    {
        foreach (var location in _cardZonesOperations._cardZones.PrivateTurnHoldLocations.Where(l => l.PrivateTurnHold!.DeferredOffer is null && l.PrivateTurnHold.ExpiresTurnNumber <= _coreActualTurnProgressTurnNumberAccess._turnNumber))
            _corePhysicalMovementFacadesOperations.MoveCards(_cardZonesOperations._cardZones.CardsAt(location).ToArray(), location, _coreMatchParticipantsPlayersAccess._players[location.OwnerSeat!.Value].IsAlive ? CardLocation.Hand(location.OwnerSeat.Value) : CardLocation.DiscardPile, new("skill.private-turn-hold.return"));
    }

    internal IReadOnlyList<PrivateTurnHoldSnapshot>? ProjectPrivateTurnHolds(int owner, int viewer, bool revealAll)
    {
        var holds = _cardZonesOperations._cardZones.PrivateTurnHoldLocations.Where(l => (l.PrivateTurnHold?.DeferredOffer?.TargetSeat ?? l.OwnerSeat) == owner).Select(l =>
        {
            var h = l.PrivateTurnHold!;
            var cards = _cardZonesOperations._cardZones.CardsAt(l);
            return new PrivateTurnHoldSnapshot(h.HoldId, owner, h.SkillId, h.SkillInstanceId, h.SourceId, h.ExpiresTurnNumber, cards.Count, (h.DeferredOffer is null ? owner == viewer : l.OwnerSeat == viewer || _corePrivateCardOffersOperations.DeferredPrivateOfferViewedCards(viewer).Any(c => cards.Any(card => card.Id == c.Id))) || revealAll ? Array.AsReadOnly(cards.Select(_coreSnapshotValueProjectionOperations.ToSnapshot).ToArray()) : null)
            {
                DeferredSourceSeat = h.DeferredOffer is null ? null : l.OwnerSeat
            };
        }).ToArray();
        return holds.Length == 0 ? null : Array.AsReadOnly(holds);
    }
}
