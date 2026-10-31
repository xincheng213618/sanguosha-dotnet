#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDaQiaoXiaoQiaoHappyGodRuntime
{
    internal bool HappyQiaoBoundMovement(ProgramSkillFrame frame, SkillProgramEffect? effect, ProgramMovementContinuation continuation)
    {
        if (frame.HappyQiao is not { Stage: HappyQiaoStage.GainChildren or HappyQiaoStage.DiscardChildren } || effect?.Op is not (SkillProgramEffectOp.ResolveHappyQiaoShutu or SkillProgramEffectOp.ResolveHappyQiaoTongxin) || continuation != frame.PendingMovementContinuation || continuation.SubjectSeat != frame.OwnerSeat || continuation.BeforeCount != 0 || continuation.CoverageResultBind is not null)
            return false;
        return true;
    }

    internal bool HappyQiaoFirstSkillChild(ProgramSkillFrame frame, ResolutionFrame child)
    {
        if (frame.HappyQiao is not { } receipt)
            return false;
        if (child is SkillInvocationFrame invocation)
            return receipt.Operation == SkillProgramEffectOp.ResolveHappyQiaoTongxin && receipt.Stage is HappyQiaoStage.HealChildren or HappyQiaoStage.DiscardChildren && invocation.ActorSeat == frame.OwnerSeat && invocation.SkillKey == frame.SkillId && invocation.SkillInstanceId == frame.SkillInstanceId && invocation.OccurrenceKey == "tongxin:" + frame.Id && _characterHuaTuoAnniversaryGodOperations.SkillInvocationStructuralEdge(frame, invocation);
        if (child is CardUseFrame use)
            return receipt.Stage is HappyQiaoStage.NativeUse or HappyQiaoStage.ReturnedUse && HappyQiaoNativeUseRidesOn(use, frame);
        if (child is CardDeclarationFrame declaration)
            return receipt.Stage == HappyQiaoStage.Declaration && MatchesHappyQiaoShutuDeclaration(frame, declaration);
        if (child is HappyQiaoHealthChoiceFrame health)
            return HappyQiaoHealthChoiceRidesOn(health, frame);
        if (child is RecoveryReplacementFrame recovery)
            return receipt.Stage == HappyQiaoStage.HealChildren && recovery.Attempt.SourceSeat == frame.OwnerSeat && recovery.Attempt.TargetSeat == frame.OwnerSeat && recovery.Attempt.Amount == 1 && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.Program && recovery.Return.Continuation == PostEventContinuation.Program && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, frame);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.ResumeFrameId == frame.Id && hp.Change.ParentFrameId == frame.Id && (receipt.Stage == HappyQiaoStage.HealChildren && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.TargetSeat == frame.OwnerSeat || receipt.Stage == HappyQiaoStage.DiscardChildren && hp.Change.TargetSeat == frame.OwnerSeat) && hp.Continuation is PostEventContinuation.Program or PostEventContinuation.AwaitedProgramMovement;
        if (child is CardsMovedTriggerWindowFrame moved)
            return receipt.Stage is HappyQiaoStage.GainChildren or HappyQiaoStage.DiscardChildren && frame.PendingMovementContinuation is { } continuation && HappyQiaoBoundMovement(frame, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(frame), continuation) && CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(frame, moved) && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => m.Sequence > receipt.Before && m.Sequence <= receipt.After && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m));
        return child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills && skills.ResumeProgramFrameId == frame.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal ProgramSkillFrame? HappyQiaoSkillObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((frame, child) => this.HappyQiaoFirstSkillChild(frame, child), (index) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(index));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 42400)]
    internal bool HappyQiaoSkillDying() => _coreDyingCursorRuntimeOperations.Value.ActiveDying is { } dying && HappyQiaoSkillObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 42400)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 42400)]
    internal bool HappyQiaoSkillRecoveryParent(ProgramSkillFrame frame, RecoveryReplacementFrame recovery)
    {
        if (frame.HappyQiao is null)
            return false;
        var root = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == frame.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == recovery.Id);
        return root >= 0 && child > root && HappyQiaoFirstSkillChild(frame, _resolutionStackOperations._resolutionStack[root + 1]) && (child == root + 1 || Enumerable.Range(root + 2, child - root - 1).All(_coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix)) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 42400)]
    internal bool AdvanceHappyQiaoSkillObservers() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && HappyQiaoSkillObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
