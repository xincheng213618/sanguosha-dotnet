#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoRenBoundaryRuntime
{
    internal SkillProgramStepOutcome BeginMoveFieldEquipment(ProgramSkillFrame frame)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (active.SelectedTargetSeats is not [var destination])
            throw new InvalidOperationException("Jiewei lost its destination participant.");
        var candidates = _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive).SelectMany(player => _corePhysicalOwnedCardQueriesOperations.GetEquipment(player)).Where(card =>
        {
            var location = _cardZonesOperations._cardZones.GetLocation(card.Id);
            return !card.IsGeneralWeapon && location.OwnerSeat is { } source && source != destination && _coreProgramCardPaymentsOperations.CanEnterEquipmentSlot(destination, card);
        }).Select(card => card.Id).OrderBy(id => id).ToArray();
        if (candidates.Length == 0)
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = active with { StrategicDraft = new(SkillProgramEffectOp.MoveFieldEquipment, destination, null, 1) { Candidates = candidates } });
        _coreStrategicProgramsOperations.PublishStrategicPrompt(frame);
        return _corePendingDecisionPolicyPipelineOperations._pendingDecision is null ? SkillProgramStepOutcome.Continue : SkillProgramStepOutcome.AwaitChoice;
    }

    internal void ResolveMoveFieldEquipment(ProgramSkillFrame frame, int cardId)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (active.SelectedTargetSeats is not [var destination])
            throw new InvalidOperationException("Jiewei lost its destination participant.");
        var location = _cardZonesOperations._cardZones.GetLocation(cardId);
        if (location.Zone != CardZoneKind.Equipment || location.OwnerSeat is not { } source || source == destination || !_cardZonesOperations._cardZones.CardsAt(location).Any(item => item.Id == cardId))
            throw new InvalidOperationException("The Jiewei equipment left the field.");
        var card = _cardZonesOperations._cardZones.CardsAt(location).Single(item => item.Id == cardId);
        if (card.IsGeneralWeapon || !_coreProgramCardPaymentsOperations.CanEnterEquipmentSlot(destination, card))
            throw new InvalidOperationException("The Jiewei destination no longer accepts the equipment.");
        var target = _coreEphemeralCardMaterialsOperations.NormalizeEphemeralCardDestination(card, location, CardLocation.Equipment(destination));
        var reason = new CardMoveReason($"skill-program.{(frame.SkillId)}.jiewei-move");
        _coreRuntimeOperations.ReplaceRuntimeTop(active with { PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        var batch = _coreCardMovementProgramsOperations.BeginCardMovementBatch([location], [target]);
        var records = new List<CardMovementRecord>(1);
        var committed = false;
        try
        {
            _cardZonesOperations._cardZones.Move(cardId, location, CardLocation.Processing);
            _cardZonesOperations._cardZones.Move(cardId, CardLocation.Processing, target);
            records.Add(_corePhysicalMovementFacadesOperations.RecordMovement(card, location, target, reason));
            _coreEquipmentSkillsOperations.ResolveEquipmentSkillGrant(card, location, target);
            _corePhysicalMovementFacadesOperations.ResolveSilverLionRemoval(card, location, reason);
            committed = true;
        }
        finally
        {
            _coreCardMovementProgramsOperations.CompleteCardMovementBatch(batch, records, committed);
        }

        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreRuntimeOperations.ReturnRuntimeProgramMovement(frame.Id);
    }
}
