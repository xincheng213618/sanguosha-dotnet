#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuJinDingOlRuntime
{

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 26820)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 26820)]
    internal bool ContinuePaidHuJinDing(ProgramSkillFrame f)
    {
        if (f.Qingyuan is not { Stage: QingyuanStage.TakeChildren })
            return false;
        return true;
    }

    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 190)]
    internal bool IsHuJinDingMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending)
    {
        if (f.Qingyuan is not { Stage: QingyuanStage.TakeChildren } r || effect?.Op != r.Op || pending != f.PendingMovementContinuation)
            return false;
        return true;
    }

    internal bool HuJinDingFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.Qingyuan is not { Stage: QingyuanStage.TakeChildren } r)
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements is [var m] && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After && m.CardId == r.CardId && m.From == CardLocation.Hand(r.HolderSeat!.Value) && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == QingyuanTakeReason;
        return child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 900)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 840)]
    internal bool IsHuJinDingRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => false;
    internal ProgramSkillFrame? HuJinDingObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterHuJinDingOlOperations.HuJinDingFirstChild(f, c), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 1490)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 1600)]
    internal bool HasHuJinDingDamageObserver(long id) => HuJinDingObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is DamageTriggerWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 1620)]
    internal bool HasHuJinDingBeforeDamageObserver(long id) => HuJinDingObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is BeforeDamageProgramWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 26820)]
    internal bool AdvanceHuJinDingSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && HuJinDingObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
