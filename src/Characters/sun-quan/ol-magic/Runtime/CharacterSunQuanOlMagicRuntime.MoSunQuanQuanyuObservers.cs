#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunQuanOlMagicRuntime
{
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 440)]
    internal bool IsMoQuanyuMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => f.MoQuanyu is { Stage: MoQuanyuStage.DrawChildren, DrawOutcome: not null } && effect is { Target: SkillProgramEffectTarget.Owner, Condition.Kind: SkillProgramConditionKind.Always } && effect.Op is (SkillProgramEffectOp.ResolveMoSunQuanQuanyuRoundChoices or SkillProgramEffectOp.ResolveMoSunQuanQuanyuTargetChoice) && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null;
    internal bool MoQuanyuDrawMovement(ProgramSkillFrame f, CardMovementRecord m)
    {
        if (f.MoQuanyu is not { Stage: MoQuanyuStage.DrawChildren, DrawOutcome: { } o } || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) || m.Sequence <= o.SequenceBefore || m.Sequence > o.SequenceAfter)
            return false;
        return o.ActualDrawIds.Contains(m.CardId) && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == MoQuanyuDrawReason || o.ReplacementGainedIds.Contains(m.CardId) && m.To == CardLocation.Hand(f.OwnerSeat) || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle;
    }

    internal bool MoQuanyuFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.MoQuanyu is not { } r || !ValidMoQuanyuReceipt(f))
            return false;
        if (r.Stage == MoQuanyuStage.Drawing && f.NativeDraw is not null)
            return global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f);
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state)
            return state.ResumeProgramFrameId == f.Id && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id && e.Change.TargetSeat == state.OwnerSeat && e.Change.Window == state.Window);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        return child is CardsMovedTriggerWindowFrame moved && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => MoQuanyuDrawMovement(f, m));
    }

    internal bool MoQuanyuObserverSuffix(int index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[n - 1], _resolutionStackOperations2._resolutionStack()[n]) || _resolutionStackOperations2._resolutionStack()[n - 1] is ProgramSkillFrame { Zenrun: { Paid: true } } replacement && _characterSunHongOlOperations.ZenrunFirstChild(replacement, _resolutionStackOperations2._resolutionStack()[n]) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, dying));
    internal ProgramSkillFrame? MoQuanyuObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => _characterSunQuanOlMagicOperations.MoQuanyuFirstChild(f, child), (index) => _characterSunQuanOlMagicOperations.MoQuanyuObserverSuffix(index));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 1420)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 1530)]
    internal bool HasMoQuanyuDamageObserver(long id) => MoQuanyuObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 1550)]
    internal bool HasMoQuanyuBeforeDamageObserver(long id) => MoQuanyuObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 630)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 570)]
    internal bool IsMoQuanyuRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.MoQuanyu is null || recovery.ParentFrameId == f.Id)
            return false;
        var parent = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return parent >= 0 && child > parent + 1 && MoQuanyuFirstChild(f, _resolutionStackOperations._resolutionStack[parent + 1]) && MoQuanyuObserverSuffix(parent) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 28910)]
    internal bool AdvanceMoQuanyuChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MoQuanyuObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
