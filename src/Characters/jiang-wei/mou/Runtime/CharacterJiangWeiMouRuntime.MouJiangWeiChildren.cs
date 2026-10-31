#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterJiangWeiMouRuntime
{
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 35200)]
    internal bool MouJiangWeiAnniversaryMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => f.MouJiangWeiAnniversary is { Paid: true } r && effect?.Op == r.Operation && MouJiangWeiAnniversary.Valid(f) && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null;
    internal bool MouJiangWeiAnniversaryFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!MouJiangWeiAnniversary.Valid(f) || f.MouJiangWeiAnniversary is not { } r)
            return false;
        if (global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f))
            return r.Stage == MouJiangWeiAnniversaryStage.Drawing && f.NativeDraw is not null;
        if (child is CardsMovedTriggerWindowFrame moved && f.PendingMovementContinuation is { } p && MouJiangWeiAnniversaryMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p))
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After);
        if (r.Stage == MouJiangWeiAnniversaryStage.DiscardChildren && r.Card is { From.Zone: CardZoneKind.Equipment } card)
        {
            var lion = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == card.Id && m.From == card.From && m.CardKind == CardKind.SilverLion && m.Sequence > r.Before && m.Sequence <= r.After && m.Reason.Value == MouJiangWeiAnniversaryRuntime.CostReason);
            if (lion && child is RecoveryReplacementFrame rr)
                return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(rr, f) && rr.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && rr.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && rr.Attempt.SourceSeat == r.DamageSource && rr.Attempt.TargetSeat == r.DamageSource;
            if (lion && child is HpChangedTriggerWindowFrame hp)
                return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.TargetSeat == r.DamageSource;
        }

        if (r.Stage == MouJiangWeiAnniversaryStage.Damage && f.AttackAttempt is { } a && f.AttackReturn is not null && a.SourceSeat == r.DamageSource && a.TargetSeat == r.Victim && !a.SourceLess && _coreProgramEventHistoryOperations.ProgramEventHistory<MouJiangWeiAnniversaryDamageIssuedEvent>().Count(e => e.FrameId == f.Id && e.Source == a.SourceSeat && e.Victim == a.TargetSeat) == 1)
            return _coreDyingSuitsAndHistoricalUseObserversOperations.DyingSuitsStructuralEdge(f, child);
        if (r.Stage == MouJiangWeiAnniversaryStage.RecoveryChildren)
        {
            if (child is RecoveryReplacementFrame rr)
                return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(rr, f) && rr.Attempt.Completion.Producer == RecoveryAttemptProducer.Program && rr.Attempt.SourceSeat == f.OwnerSeat && rr.Attempt.TargetSeat == f.OwnerSeat;
            if (child is HpChangedTriggerWindowFrame hp)
                return hp.ResumeFrameId == f.Id && hp.Change.ParentFrameId == f.Id && hp.Change.TargetSeat == f.OwnerSeat;
        }

        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal bool MouJiangWeiAnniversaryEdge(int i) => _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(i) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations._resolutionStack[i - 1], _resolutionStackOperations._resolutionStack[i]) || _resolutionStackOperations._resolutionStack[i - 1] is ProgramSkillFrame f && MouJiangWeiAnniversaryFirstChild(f, _resolutionStackOperations._resolutionStack[i]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]);
    internal bool MouJiangWeiAnniversarySuffix(int n) => _resolutionStackOperations._resolutionStack[n + 1] is DyingFrame d && n + 2 < _resolutionStackOperations._resolutionStack.Count && _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n + 1, d) || _coreObserverSubtreesOperations.HasObserverSuffix(n, (i) => _characterJiangWeiMouOperations.MouJiangWeiAnniversaryEdge(i), (i, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(i, dying));
    internal ProgramSkillFrame? MouJiangWeiAnniversaryRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterJiangWeiMouOperations.MouJiangWeiAnniversaryFirstChild(f, c), (n) => _characterJiangWeiMouOperations.MouJiangWeiAnniversarySuffix(n));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 35200)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 35200)]
    internal bool MouJiangWeiAnniversaryDamageWindow(long id) => MouJiangWeiAnniversaryRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 35200)]
    internal bool MouJiangWeiAnniversaryBeforeDamage(long id) => MouJiangWeiAnniversaryRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 35200)]
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 35200)]
    internal bool MouJiangWeiAnniversaryDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && MouJiangWeiAnniversaryRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 35200)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 35200)]
    internal bool MouJiangWeiAnniversaryRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (MouJiangWeiAnniversaryFirstChild(f, recovery))
            return true;
        var n = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var c = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return n >= 0 && c > n + 1 && MouJiangWeiAnniversaryFirstChild(f, _resolutionStackOperations._resolutionStack[n + 1]) && MouJiangWeiAnniversarySuffix(n) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[c - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 35200)]
    internal bool MouJiangWeiAnniversaryAdvanceChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouJiangWeiAnniversaryRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
