#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoCaoOlMagicRuntime
{
    internal bool MoCaoCaoPaidMovement(ProgramSkillFrame f, out int card, out int payer, out long before, out long after, out string reason)
    {
        card = payer = 0;
        before = after = 0;
        reason = "";
        if (!MoCaoCaoBachao.Valid(f))
            return false;
        if (f.MoCaoCaoBaibi is { Paid: true } b)
        {
            card = b.CardId;
            payer = b.Claim.TargetSeat;
            before = b.Before;
            after = b.After;
            reason = MoCaoCaoBachaoRuntime.BaibiReason;
        }
        else if (f.MoCaoCaoBachao is { Stage: MoCaoCaoBachaoStage.PaymentChildren } r && r.Cursor < r.Votes.Count)
        {
            card = r.PaidCardId;
            payer = r.Votes[r.Cursor].Seat;
            before = r.Before;
            after = r.After;
            reason = MoCaoCaoBachaoRuntime.GiftReason;
        }
        else
            return false;
        var id = card;
        var seat = payer;
        var lower = before;
        var upper = after;
        var why = reason;
        return id > 0 && lower >= 0 && upper <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == id && m.From.OwnerSeat == seat && m.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && m.Sequence > lower && m.Sequence <= upper && m.Reason.Value == why && (m.To == CardLocation.Hand(f.OwnerSeat) || m.To == CardLocation.OutsideGame));
    }

    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 33800)]
    internal bool MoCaoCaoMovementReturn(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => effect?.Op is SkillProgramEffectOp.ResolveMoCaoCaoBachao or SkillProgramEffectOp.ResolveMoCaoCaoBaibi && MoCaoCaoBachao.Valid(f) && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null;
    internal bool MoCaoCaoFirstNativeChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!MoCaoCaoBachao.Valid(f))
            return false;
        if (MoCaoCaoPaidMovement(f, out var card, out var payer, out var before, out var after, out var reason))
        {
            if (child is CardsMovedTriggerWindowFrame moved)
                return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > before && m.Sequence <= after);
            var lion = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == card && m.CardKind == CardKind.SilverLion && m.From == CardLocation.Equipment(payer) && m.Sequence > before && m.Sequence <= after);
            if (lion && child is RecoveryReplacementFrame recovery)
                return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == reason && recovery.Attempt.SourceSeat == payer && recovery.Attempt.TargetSeat == payer && recovery.Attempt.Amount == 1;
            if (lion && child is HpChangedTriggerWindowFrame hp)
                return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == payer && hp.Change.TargetSeat == payer && _coreProgramEventHistoryOperations.ProgramEventHistory<SilverLionRemovedRecoveryEvent>().Any(e => e.ResolutionId > f.Id && e.ResolutionId < hp.Change.Id && e.PlayerSeat == payer && e.Reason.Value == reason && e.RecoveredAmount == hp.Change.Amount && e.RemainingHp == hp.Change.HpAfter);
        }

        if (f.MoCaoCaoBachao is { Stage: MoCaoCaoBachaoStage.Damage, TargetSeat: { } target } && f.AttackAttempt is { } attack && f.AttackReturn is not null && attack.SourceSeat == f.OwnerSeat && attack.TargetSeat == target && !attack.SourceLess)
            return _coreDyingSuitsAndHistoricalUseObserversOperations.DyingSuitsStructuralEdge(f, child);
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal bool MoCaoCaoNativeEdge(int i) => _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(i) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations._resolutionStack[i - 1], _resolutionStackOperations._resolutionStack[i]) || _resolutionStackOperations._resolutionStack[i - 1] is ProgramSkillFrame f && MoCaoCaoFirstNativeChild(f, _resolutionStackOperations._resolutionStack[i]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]);
    internal bool MoCaoCaoNativeSuffix(int n) => _resolutionStackOperations._resolutionStack[n + 1] is DyingFrame d && n + 2 < _resolutionStackOperations._resolutionStack.Count && _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n + 1, d) || _coreObserverSubtreesOperations.HasObserverSuffix(n, (i) => _characterCaoCaoOlMagicOperations.MoCaoCaoNativeEdge(i), (i, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(i, d));
    internal ProgramSkillFrame? MoCaoCaoNativeRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterCaoCaoOlMagicOperations.MoCaoCaoFirstNativeChild(f, c), (n) => _characterCaoCaoOlMagicOperations.MoCaoCaoNativeSuffix(n));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 33800)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 33800)]
    internal bool MoCaoCaoDamageWindow(long id) => MoCaoCaoNativeRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 33800)]
    internal bool MoCaoCaoBeforeDamageWindow(long id) => MoCaoCaoNativeRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 33800)]
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 33800)]
    internal bool MoCaoCaoDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && MoCaoCaoNativeRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 33800)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 33800)]
    internal bool MoCaoCaoRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (MoCaoCaoFirstNativeChild(f, recovery))
            return true;
        var n = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var c = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return n >= 0 && c > n + 1 && MoCaoCaoFirstNativeChild(f, _resolutionStackOperations._resolutionStack[n + 1]) && MoCaoCaoNativeSuffix(n) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[c - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 33800)]
    internal bool MoCaoCaoAdvanceNativeChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MoCaoCaoNativeRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
