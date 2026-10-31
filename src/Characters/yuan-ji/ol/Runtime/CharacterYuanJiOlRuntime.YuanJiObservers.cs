#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYuanJiOlRuntime
{

    internal bool IsYuanJiOwnMovement(ProgramSkillFrame f, CardMovementRecord m)
    {
        if (f.YuanJi is not { } r || m.Sequence <= r.Before || m.Sequence > r.After)
            return false;
        if (r.Stage == YuanJiStage.ReturnChildren)
            return m.From == CardLocation.Hand(f.OwnerSeat) && m.To == CardLocation.DrawPile && r.Eligible.Contains(m.CardId) && m.Reason.Value == (r.Op == SkillProgramEffectOp.ResolveJinghuaTurnReturn ? YuanJiBottomReason : YuanJiTopReason);
        if (r.Stage != YuanJiStage.DrawChildren || r.Draws.LastOrDefault()is not { } d)
            return false;
        return m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle || m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(d.RecipientSeat) && m.Reason.Value == (r.Op == SkillProgramEffectOp.ResolveJieyanOppositeEndDrawPair ? YuanJiPairReason : r.Op == SkillProgramEffectOp.ResolveJinghuaRoundDraw ? YuanJiJinghuaReason : YuanJiShuiyueReason);
    }

    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 470)]
    internal bool IsYuanJiMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => f.YuanJi is { Stage: YuanJiStage.DrawChildren or YuanJiStage.ReturnChildren } r && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null && e?.Op == r.Op;
    internal bool YuanJiFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.YuanJi is not { Stage: YuanJiStage.DrawChildren or YuanJiStage.ReturnChildren })
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => IsYuanJiOwnMovement(f, m));
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange)
            return state.ResumeProgramFrameId == f.Id;
        return child is RecoveryReplacementFrame recovery && IsYuanJiRecoveryParent(f, recovery);
    }

    internal bool YuanJiObserverEdge(int n) => n > 0 && _resolutionStackOperations._resolutionStack[n - 1] is ProgramSkillFrame f && YuanJiFirstChild(f, _resolutionStackOperations._resolutionStack[n]);
    internal ProgramSkillFrame? YuanJiObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterYuanJiOlOperations.YuanJiFirstChild(f, c), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _characterYuanJiOlOperations.YuanJiObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 990)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 1100)]
    internal bool HasYuanJiDamageObserver(long id) => YuanJiObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 1130)]
    internal bool HasYuanJiBeforeDamageObserver(long id) => YuanJiObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 230)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 220)]
    internal bool IsYuanJiRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => f.YuanJi is { Stage: YuanJiStage.DrawChildren } r && r.Draws.Count > 0 && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement;
}
