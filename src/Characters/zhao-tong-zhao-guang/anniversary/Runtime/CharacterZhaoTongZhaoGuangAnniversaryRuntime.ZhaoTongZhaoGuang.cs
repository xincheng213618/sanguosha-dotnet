#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhaoTongZhaoGuangAnniversaryRuntime
{
    internal const string AnniversaryQingrenDrawReason = "skill-program.anniversary:qingren.draw";
    SkillProgramStepOutcome IAnniversaryZhaoTongZhaoGuangHost.Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginAnniversaryZhaoTongZhaoGuang(f, op);
    internal CardConversionSource AnniversaryZtgSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal int AnniversaryYizanCount(int owner, int? actualTurn = null, long cutoff = long.MaxValue) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversaryYizanInvokedEvent>().Count(e => e.Source.OwnerSeat == owner && e.ActionId <= cutoff && (actualTurn is null || e.ActualTurn == actualTurn) && _contentRegistryOperations._contentRegistry.GetSkill(e.Source.SkillId).Program?.GameplayHash == e.GameplayHash);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.Collection, 44600, SkillProgramEffectOp.AwakenAnniversaryZhaoTongZhaoGuangLongyuan)]
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 44600, SkillProgramEffectOp.AwakenAnniversaryZhaoTongZhaoGuangLongyuan)]
    internal bool QualifyAnniversaryLongyuan(ProgramTriggerLegalityContext q) => !AnniversaryYizanModified(q.OwnerSeat) && (AnniversaryYizanCount(q.OwnerSeat) >= 3 || _characterGuoJiaMobileGodOperations.HasShenGuoJiaAwakeningCondition(q.OwnerSeat, q.SkillId, q.Candidate.BindingId));
    internal SkillProgramStepOutcome BeginAnniversaryZhaoTongZhaoGuang(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (!AnniversaryZtgOriginal(f) || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || op == SkillProgramEffectOp.AwakenAnniversaryZhaoTongZhaoGuangLongyuan && (f.SkillId != AnniversaryZhaoTongZhaoGuangComposition.Longyuan || f.InstructionIndex != 3 || AnniversaryYizanModified(f.OwnerSeat)) || op == SkillProgramEffectOp.DrawAnniversaryZhaoTongZhaoGuangQingren && (f.SkillId != AnniversaryZhaoTongZhaoGuangComposition.Qingren || f.InstructionIndex != 1 || f.AnniversaryQingren is not null))
            throw new InvalidOperationException($"Zhao Tong/Zhao Guang requires the exact original ending candidate and current instruction ({(f.Id)}/{(f.SkillId)}/{(f.TriggerId)}/{(f.InstructionIndex)}).");
        var source = AnniversaryZtgSource(f);
        if (op == SkillProgramEffectOp.AwakenAnniversaryZhaoTongZhaoGuangLongyuan)
        {
            _coreProgramSkillChangesOperations.CommitProgramSkillChangeFact(f, new AnniversaryLongyuanAwakenedEvent(f.Id, source, f.GameplayHash, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SkillAwakenedEvent(f.OwnerSeat, f.SkillId, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp, Array.AsReadOnly(Array.Empty<string>())));
            return SkillProgramStepOutcome.Continue;
        }

        var count = AnniversaryYizanCount(f.OwnerSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence);
        var receipt = new AnniversaryQingrenReceipt(source, f.GameplayHash, f.WindowContext!.ParentFrameId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence, count, count == 0 ? AnniversaryQingrenStage.Completed : AnniversaryQingrenStage.Drawing);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryQingren = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryQingrenIssuedEvent(f.Id, source, f.GameplayHash, receipt.ParentId, receipt.Turn, receipt.CutoffActionId, count));
        if (count == 0)
            return SkillProgramStepOutcome.Continue;
        _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, count, new(AnniversaryQingrenDrawReason));
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.DrawAnniversaryZhaoTongZhaoGuangQingren)]
    internal void ReturnAnniversaryQingrenDraw(ProgramSkillFrame supplied, NativeDrawOutcome draw)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var r = f.AnniversaryQingren!;
        if (!ValidAnniversaryQingren(f) || r.Stage != AnniversaryQingrenStage.Drawing || f.NativeDraw is not null || draw.RecipientSeat != f.OwnerSeat || draw.RequestedCount != r.Count || draw.SequenceAfter != _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.Draw is not null)
            throw new InvalidOperationException("Qingren requires its original once-issued native draw return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { AnniversaryQingren = r with { Stage = AnniversaryQingrenStage.DrawChildren, Draw = draw }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryQingrenDrawReturnedEvent(f.Id, r.Count, draw.ActualDrawIds.Count, draw.SequenceBefore, draw.SequenceAfter));
        ContinueAnniversaryQingren(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 44600)]
    internal bool ContinueAnniversaryQingren(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryQingren: { Stage: AnniversaryQingrenStage.DrawChildren } r } f || f.Id != id || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            return false;
        if (!ValidAnniversaryQingren(f))
            throw new InvalidOperationException("Qingren lost its original paid draw or native movement children.");
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.Program) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return true;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { AnniversaryQingren = r with { Stage = AnniversaryQingrenStage.Completed }, PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 44600)]
    internal bool ReturnAnniversaryQingrenMovement(ProgramSkillFrame f) => f.AnniversaryQingren is not null && ContinueAnniversaryQingren(f.Id);
}
