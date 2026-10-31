#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangLiaoAnniversaryGodRuntime
{
    internal bool AnnZlMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending)
    {
        if (f.AnniversaryZhangLiao is not { } r || effect?.Op != r.Operation || pending != f.PendingMovementContinuation || pending.SubjectSeat != f.OwnerSeat || pending.BeforeCount != 0 || pending.CoverageResultBind is not null || r.Stage is not (AnniversaryZhangLiaoStage.PaymentChildren or AnniversaryZhangLiaoStage.DiscardChildren or AnniversaryZhangLiaoStage.SlotChildren or AnniversaryZhangLiaoStage.DrawChildren))
            return false;
        return r.Stage == AnniversaryZhangLiaoStage.DrawChildren ? r.Draw is { RecipientSeat: var recipient, RequestedCount: 2 } && recipient == f.OwnerSeat && r.Branch == 1 : r.PaidCount > 0;
    }

    internal bool AnnZlFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.AnniversaryZhangLiao is not { } r)
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && f.PendingMovementContinuation is { } p && AnnZlMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p) && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.Movements.All(m => r.Paid.Contains(m));
        if (child is RecoveryReplacementFrame recovery)
            return f.PendingMovementContinuation is not null && r.Paid.Any(m => m.From.Zone == CardZoneKind.Equipment && m.CardKind == CardKind.SilverLion && m.From.OwnerSeat == recovery.Attempt.TargetSeat) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion;
        if (child is HpChangedTriggerWindowFrame hp)
            return f.PendingMovementContinuation is not null && hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && r.Paid.Any(m => m.From.Zone == CardZoneKind.Equipment && m.CardKind == CardKind.SilverLion && m.From.OwnerSeat == hp.Change.TargetSeat);
        return r.Stage == AnniversaryZhangLiaoStage.Damage && f.AttackAttempt is { SourceLess: false, Nature: DamageNature.Normal } a && a.SourceSeat == f.OwnerSeat && f.AttackReturn is not null && _coreDyingSuitsAndHistoricalUseObserversOperations.DyingSuitsStructuralEdge(f, child);
    }

    internal ProgramSkillFrame? AnnZlObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterZhangLiaoAnniversaryGodOperations.AnnZlFirstChild(f, c), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 40750)]
    internal bool AnnZlDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && AnnZlObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 40750)]
    internal bool AdvanceAnnZlChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && AnnZlObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 40750)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 40750)]
    internal bool RetainAnnZlPaid(ProgramSkillFrame f)
    {
        if (f.AnniversaryZhangLiao is not { PaidCount: > 0 })
            return false;
        return true;
    }
}
