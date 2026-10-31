#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineNativeRecastInvocationsRuntime
{
    internal bool TracksNativeRecastInvocations => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMobileShiHuangZuRecast) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMouHuangYueyingBingcai) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveChanshuangSimultaneousChoices) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveChanshuangEnding);

    internal NativeRecastInvocationDraft? CaptureNativeRecastInvocation(int initiatorSeat, int executorSeat, IReadOnlyList<int> originalPrimaryIds, long? parentFrameId = null, int? instructionIndex = null, int? participantCursor = null, CardMoveReason? reason = null)
    {
        if (!TracksNativeRecastInvocations || !_coreMatchSetupSetupCompleteAccess._setupComplete || _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber < 1 || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(_coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat) || originalPrimaryIds.Count == 0)
            return null;
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(initiatorSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(executorSeat) || originalPrimaryIds.Distinct().Count() != originalPrimaryIds.Count)
            throw new InvalidOperationException("A native recast needs one real participant and distinct original primary materials.");
        var materials = originalPrimaryIds.Select(id =>
        {
            var from = _cardZonesOperations._cardZones.GetLocation(id);
            if (from.OwnerSeat != executorSeat || from.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment or CardZoneKind.Judgment or CardZoneKind.WoodenOxGrain or CardZoneKind.PublicDeferredPile or CardZoneKind.PublicPersistentPile))
                throw new InvalidOperationException("A native recast must capture its participant's actual owned primary materials before payment.");
            var card = _cardZonesOperations._cardZones.CardsAt(from).Single(c => c.Id == id);
            return new NativeRecastMaterial(id, card.Kind, from, card.IsGeneralWeapon);
        }).ToArray();
        return new NativeRecastInvocationDraft
        {
            InvocationId = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence,
            TurnNumber = _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber,
            ActualTurnOwnerSeat = _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat,
            InitiatorSeat = initiatorSeat,
            ExecutorSeat = executorSeat,
            SequenceBefore = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            ParentFrameId = parentFrameId ?? _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id,
            InstructionIndex = instructionIndex,
            ParticipantCursor = participantCursor,
            Reason = reason ?? CardMoveReasons.RecastDiscard,
            Materials = materials
        };
    }

    internal void CompleteNativeRecastInvocation(NativeRecastInvocationDraft? draft, int actualDrawCount)
    {
        if (draft is null)
            return;
        if (!TracksNativeRecastInvocations || draft.InvocationId <= 0 || draft.InvocationId > _coreResolutionIdentityBoundaryOperations._resolutionSequence || draft.Materials.Count == 0 || actualDrawCount < 0 || actualDrawCount > draft.Materials.Count || draft.Materials.Select(m => m.CardId).Distinct().Count() != draft.Materials.Count || _coreProgramEventHistoryOperations.ProgramEventHistory<NativeRecastInvocationRecordedEvent>().Any(e => e.InvocationId == draft.InvocationId))
            throw new InvalidOperationException("A native recast must complete its whole original participant group exactly once.");
        foreach (var material in draft.Materials)
        {
            var paid = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > draft.SequenceBefore && m.Sequence <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence && m.CardId == material.CardId && m.CardKind == material.PrintedKind && m.From == material.From && m.Reason == draft.Reason && (m.To == CardLocation.DiscardPile || (material.IsGeneralWeapon || CoreGameEngineGuanduEquipmentRuntime.IsGuanduThunderChariotDeparture(_coreAdvancedLifecycleOperations.GetAdvancedCard(material.CardId), material.From, m.To) || _coreEphemeralCardMaterialsOperations.IsEphemeralCardDestroyedMaterial(m)) && m.To == CardLocation.OutsideGame)).ToArray();
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NativeRecastInvocationRecordedEvent(draft.InvocationId, draft.TurnNumber, draft.ActualTurnOwnerSeat, draft.InitiatorSeat, draft.ExecutorSeat, draft.Materials.Count, draft.Materials.Count, actualDrawCount, draft.ParentFrameId, draft.InstructionIndex, draft.ParticipantCursor, draft.SequenceBefore, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
    }

    internal ProgramSkillFrame BeginFrameNativeRecastInvocation(ProgramSkillFrame frame, int executorSeat, IReadOnlyList<int> originalPrimaryIds, int? participantCursor = null)
    {
        if (!TracksNativeRecastInvocations)
            return frame;
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (frame.NativeRecastInvocation is not null)
            throw new InvalidOperationException("A program cannot overlap two unfinished recasts on one owning frame.");
        var draft = CaptureNativeRecastInvocation(frame.OwnerSeat, executorSeat, originalPrimaryIds, frame.Id, frame.InstructionIndex, participantCursor);
        if (draft is not null)
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { NativeRecastInvocation = draft });
        return frame;
    }

    internal ProgramSkillFrame CompleteFrameNativeRecastInvocation(ProgramSkillFrame frame, int actualDrawCount)
    {
        if (!TracksNativeRecastInvocations)
            return frame;
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (frame.NativeRecastInvocation is not { } draft)
            return frame;
        if (draft.ParentFrameId != frame.Id || draft.InstructionIndex != frame.InstructionIndex)
            throw new InvalidOperationException("A paid native recast changed its owning program instruction.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { NativeRecastInvocation = null });
        CompleteNativeRecastInvocation(draft, actualDrawCount);
        return _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
    }
}
