#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuoHuaiBoundaryMobileRuntime
{
    internal bool HasBoundaryGuoHuaiJingce => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.PrepareBoundaryGuoHuaiJingce);

    internal void CaptureBoundaryGuoHuaiDiscardBatch(CardMovementBatchContext batch)
    {
        if (!HasBoundaryGuoHuaiJingce || batch.TurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber || batch.ParentFrameId is not { } parent || _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber <= 0)
            return;
        foreach (var move in batch.Movements.Where(m => _coreNativeUsedDiscardOriginsOperations.Value.IsNativeTrueUseDiscardMovement(batch, m)))
        {
            var entry = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Last(m => m.CardId == move.CardId && m.Sequence < move.Sequence);
            var actionId = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<NativeTrueUseProcessingEntryEvent>().SingleOrDefault(e => e.ParentFrameId == parent && e.CardId == move.CardId && e.EntrySequence == entry.Sequence)?.ParentActionId ?? 0;
            RecordBoundaryGuoHuaiDiscard(move, parent, actionId, batch.TurnNumber, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat);
        }
    }

    internal void RecordBoundaryGuoHuaiDiscard(CardMovementRecord move, long nativeParent, long action, int turn, int actualOwner)
    {
        if (move.From != CardLocation.Processing || move.To != CardLocation.DiscardPile || move.TurnNumber != turn || move.Sequence <= 0 || move.Sequence > _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements[move.Sequence - 1] != move || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<BoundaryGuoHuaiDiscardedEntityEvent>().Any(e => e.MovementSequence == move.Sequence))
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryGuoHuaiDiscardedEntityEvent(turn, actualOwner, nativeParent, action, move.CardId, move.Sequence));
    }

    internal int BoundaryGuoHuaiCount(int turn, int owner) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<BoundaryGuoHuaiDiscardedEntityEvent>().Count(e => e.TurnNumber == turn && e.ActualTurnOwnerSeat == owner);
    internal TurnEndingBoundaryFrame? BoundaryGuoHuaiEnding(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (candidate.SkillId != BoundaryGuoHuaiComposition.Skill || candidate.BindingId != BoundaryGuoHuaiComposition.Ending || context is not { Window: SkillProgramTriggerWindow.TurnEnding, SourceSeat: { } actor, TargetSeat: { } target } || actor != candidate.OwnerSeat || target != actor || context.OwnerSeat != actor || actor != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat || _resolutionStackOperations._resolutionStack.OfType<TurnEndingBoundaryFrame>().SingleOrDefault(f => f.Id == context.ParentFrameId)is not { } ending || ending.OwnerSeat != actor || ending.TurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber || ending.ItemIndex < 0 || ending.ItemIndex >= ending.Items.Count || ending.Items[ending.ItemIndex].Candidate != candidate)
            return null;
        return ending;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 56400, SkillProgramEffectOp.PrepareBoundaryGuoHuaiJingce)]
    internal bool QualifyBoundaryGuoHuaiJingce(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (context.Window == SkillProgramTriggerWindow.CardResponseCompleted)
            return true;
        return BoundaryGuoHuaiEnding(candidate, context)is not null && _coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat] is { IsAlive: true, HasHpValue: true } owner && BoundaryGuoHuaiCount(_coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat) >= owner.Hp;
    }

    void IBoundaryGuoHuaiHost.Prepare(ProgramSkillFrame frame) => PrepareBoundaryGuoHuaiJingce(frame);
    internal void PrepareBoundaryGuoHuaiJingce(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.WindowContext is not { } context || frame.InstructionIndex != 1)
            throw new InvalidOperationException("Mobile Jingce lost its original native observation frame.");
        if (context.Window == SkillProgramTriggerWindow.CardResponseCompleted)
        {
            if (_resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(f => f.Id == context.ParentFrameId)is not { ResponseCompletion: { } completion } window || completion.ActionId != window.Action.ActionId || completion.ActualTurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber || completion.ActualTurnOwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || window.Action.Type != CardActionType.Response)
                throw new InvalidOperationException("Mobile Jingce requires the exact typed response completion and native materials.");
            foreach (var move in completion.NativeCosts)
                RecordBoundaryGuoHuaiDiscard(move, completion.ParentFrameId, completion.ActionId, completion.ActualTurnNumber, completion.ActualTurnOwnerSeat);
            return;
        }

        if (_resolutionStackOperations._resolutionStack.OfType<TurnEndingBoundaryFrame>().SingleOrDefault(f => f.Id == context.ParentFrameId)is not { } ending || ending.ItemIndex < 0 || ending.ItemIndex >= ending.Items.Count || ending.Items[ending.ItemIndex].Candidate is not { } candidate || BoundaryGuoHuaiEnding(candidate, context)is null || candidate.SkillInstanceId != frame.SkillInstanceId || candidate.GameplayHash != frame.GameplayHash)
            throw new InvalidOperationException("Mobile Jingce lost its own original actual Ending candidate.");
        var count = BoundaryGuoHuaiCount(_coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat);
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        if (!owner.IsAlive || !owner.HasHpValue || count < owner.Hp)
            throw new InvalidOperationException("Mobile Jingce no longer satisfies its actual discarded-entity threshold.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryGuoHuaiEndingQualifiedEvent(frame.Id, new(frame.SkillId, frame.TriggerId!, frame.OwnerSeat, frame.SkillInstanceId), frame.GameplayHash, ending.Id, ending.TurnNumber, ending.OwnerSeat, count, owner.Hp, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CompleteMovementBatch, 4100)]
    internal CardMovementBatchContext CompleteMovementBatchCaptureBoundaryGuoHuaiDiscardBatch(CardMovementBatchBuilder batch, CardMovementBatchContext completed)
    {
        CaptureBoundaryGuoHuaiDiscardBatch(completed);
        return completed;
    }
}
