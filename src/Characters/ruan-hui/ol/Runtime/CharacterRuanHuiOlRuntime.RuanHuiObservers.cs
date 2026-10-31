#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterRuanHuiOlRuntime
{

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27080)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27080)]
    internal bool ContinuePaidRuanHui(ProgramSkillFrame f)
    {
        if (f.RuanHui is not null)
        {
            return true;
        }

        return f.InstructionIndex is 0 or 1 && f.TriggerId is { } id && f.ActivationId == id && f.WindowContext is { } context && IsIssuedRuanHuiCandidate(new(f.OwnerSeat, f.SkillId, id, f.SkillInstanceId, f.GameplayHash, 0, context.OccurrenceIndex)) && CanRunIssuedRuanHuiCandidate(new(f.OwnerSeat, f.SkillId, id, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex), context);
    }

    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 260)]
    internal bool IsRuanHuiMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p)
    {
        if (f.RuanHui is not { } r || e?.Op != r.Op || p != f.PendingMovementContinuation || r.Stage is RuanHuiStage.DrawChoice or RuanHuiStage.TargetChoice)
            return false;
        return true;
    }

    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 790)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 730)]
    internal bool IsRuanHuiRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.RuanHui is not { Stage: RuanHuiStage.RandomChildren, RandomEntity: { Kind: CardKind.SilverLion } card, Steps.Count: > 0 } r || f.PendingMovementContinuation is null)
            return false;
        var step = r.Steps[^1];
        return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Amount == 1 && recovery.Attempt.SourceSeat == card.From.OwnerSeat && recovery.Attempt.TargetSeat == card.From.OwnerSeat && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == card.CardId && m.Sequence > step.Before && m.Sequence <= step.After && m.From == card.From && m.Reason.Value == MingchaRandom);
    }

    internal bool RuanHuiFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.RuanHui is not { } r)
            return false;
        if (child is ProgramLifecycleTriggerWindowFrame skills && skills.ResumeProgramFrameId == f.Id)
            return skills.Window == SkillProgramTriggerWindow.SkillsChanged && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (f.PendingMovementContinuation is null || r.Steps.Count == 0)
            return false;
        var step = r.Steps[^1];
        if (child is CardsMovedTriggerWindowFrame moved)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > step.Before && m.Sequence <= step.After);
        if (child is RecoveryReplacementFrame recovery)
            return IsRuanHuiRecoveryParent(f, recovery);
        if (child is HpChangedTriggerWindowFrame hp)
            return r.RandomEntity is { Kind: CardKind.SilverLion } card && hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.Amount == 1 && hp.Change.SourceSeat == card.From.OwnerSeat && hp.Change.TargetSeat == card.From.OwnerSeat;
        return false;
    }

    internal ProgramSkillFrame? RuanHuiObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterRuanHuiOlOperations.RuanHuiFirstChild(f, c), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 620)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 730)]
    internal bool HasRuanHuiDamageObserver(long id) => RuanHuiObserverRoot()is { } r && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == r.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 760)]
    internal bool HasRuanHuiBeforeDamageObserver(long id) => RuanHuiObserverRoot()is { } r && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == r.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27080)]
    internal bool AdvanceRuanHuiSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && RuanHuiObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
