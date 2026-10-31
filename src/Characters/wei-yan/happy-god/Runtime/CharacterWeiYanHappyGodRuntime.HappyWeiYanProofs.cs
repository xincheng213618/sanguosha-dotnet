#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWeiYanHappyGodRuntime
{
    internal bool HappyWeiYanMovement(ProgramSkillFrame frame, SkillProgramEffect? effect, ProgramMovementContinuation pending)
    {
        if (frame.HappyWeiYan is not { } receipt || effect?.Op != receipt.Operation || pending != frame.PendingMovementContinuation || pending.SubjectSeat != frame.OwnerSeat || pending.BeforeCount != 0 || pending.CoverageResultBind is not null || receipt.Stage is not (HappyWeiYanStage.Replacing or HappyWeiYanStage.Placing or HappyWeiYanStage.GainChildren or HappyWeiYanStage.DrawChildren or HappyWeiYanStage.BoneDiscardChildren or HappyWeiYanStage.GujingDiscardChildren))
            return false;
        if (receipt.Stage == HappyWeiYanStage.DrawChildren)
        {
            return receipt.Draw is { } draw && ValidHappyWeiYanDraw(draw, frame.OwnerSeat, frame.Id);
        }

        if (receipt.Paid.Count == 0 || receipt.Stage != HappyWeiYanStage.GujingDiscardChildren && receipt.Paid.Count != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<HappyWeiYanMovementEvent>().Count(e => e == new HappyWeiYanMovementEvent(frame.Id, receipt.Stage, receipt.Before, receipt.After, receipt.Paid.Count)) != 1)
            return false;
        return true;
    }

    internal bool HappyWeiYanFirstChild(ProgramSkillFrame frame, ResolutionFrame child)
    {
        if (frame.HappyWeiYan is not { } receipt)
            return false;
        if (child is CardUseFrame use)
            return receipt.Operation == SkillProgramEffectOp.ResolveHappyWeiYanBoneReturn && HappyBoneUseRidesOn(use, frame);
        if (child is CardsMovedTriggerWindowFrame moved)
            return frame.PendingMovementContinuation is { } pending && HappyWeiYanMovement(frame, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(frame), pending) && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(frame, moved) && moved.Batch.ParentFrameId == frame.Id && moved.Batch.OriginOwnerSeat == frame.OwnerSeat && moved.Batch.OriginSkillId == frame.SkillId && moved.Batch.OriginSkillInstanceId == frame.SkillInstanceId && moved.Batch.Movements.All(receipt.Paid.Contains);
        if (child is RecoveryReplacementFrame recovery)
            return frame.PendingMovementContinuation is not null && receipt.Paid.Any(m => m.From.Zone == CardZoneKind.Equipment && m.CardKind == CardKind.SilverLion && m.From.OwnerSeat == recovery.Attempt.TargetSeat) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, frame) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion;
        if (child is HpChangedTriggerWindowFrame hp)
            return frame.PendingMovementContinuation is not null && hp.Change.ParentFrameId == frame.Id && hp.ResumeFrameId == frame.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        return receipt.Stage == HappyWeiYanStage.Damage && frame.AttackAttempt is { } attack && !attack.SourceLess && attack.SourceSeat == frame.OwnerSeat && attack.TargetSeat == receipt.Target && frame.AttackReturn is not null && _coreDyingSuitsAndHistoricalUseObserversOperations.DyingSuitsStructuralEdge(frame, child);
    }

    internal ProgramSkillFrame? HappyWeiYanObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((frame, child) => _characterWeiYanHappyGodOperations.HappyWeiYanFirstChild(frame, child), (index) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(index));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 42300)]
    internal bool HappyWeiYanDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && HappyWeiYanObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 42300)]
    internal bool AdvanceHappyWeiYanObserver() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && HappyWeiYanObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 42300)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 42300)]
    internal bool RetainHappyWeiYanAccepted(ProgramSkillFrame frame)
    {
        if (frame.HappyWeiYan is null)
            return false;
        return true;
    }
}
