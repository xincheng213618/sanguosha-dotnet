#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuSuMouRuntime
{
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 36100)]
    internal bool MouLuSuMovement(ProgramSkillFrame f, SkillProgramEffect? op, ProgramMovementContinuation p) => f.MouLuSuAnniversary is { } r && op?.Op == r.Operation && MouLuSuAnniversary.Valid(f) && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null;
    internal bool MouLuSuFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!MouLuSuAnniversary.Valid(f) || f.MouLuSuAnniversary is not { } r)
            return false;
        if (child is CardDeclarationFrame declaration)
            return MatchesMouLuSuPlayedDeclaration(f, declaration);
        if (global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f))
            return r.Stage == MouLuSuAnniversaryStage.Drawing && f.NativeDraw is not null;
        if (child is CardUseFrame use)
            return r.Stage == MouLuSuAnniversaryStage.NativeUse && use.Id == r.UseId && MouLuSuNativeMatches(use);
        if (child is DrawFundedDistinctBasicFrame funded)
            return IsMouLuSuPaymentFrame(funded);
        if (child is ChainedStateBasicFrame chained)
            return IsMouLuSuPaymentFrame(chained);
        if (child is ProgramSkillFrame { EquipmentSlotNamedUse: { } slot })
            return MouLuSuSlotParent(slot)?.Id == f.Id;
        if (child is ProgramCardTriggerWindowFrame card)
            return card.MouLuSuAnniversaryPlayed is { } back && back.FrameId == f.Id && back.ActionId == r.ActionId && card.ParentFrameId == f.Id && r.Response?.ActionId == card.Action.ActionId && back.Window == (r.Stage == MouLuSuAnniversaryStage.PlayedAccepted ? SkillProgramTriggerWindow.CardResponseAccepted : r.Stage == MouLuSuAnniversaryStage.PlayedCompleted ? SkillProgramTriggerWindow.CardResponseCompleted : (SkillProgramTriggerWindow)(-1));
        if (child is PublicHandRevealWindowFrame reveal)
            return r.Stage == MouLuSuAnniversaryStage.Recipient && reveal.ParentFrameId == f.Id && reveal.ReturnKind == PublicHandRevealReturnKind.Registered && reveal.ReturnKey == "mou-lu-su-mingshi-three" && _characterXiaoQiaoMouOlOperations.IsPublicHandRevealProducerSuspended(f.Id, reveal.ReturnKey);
        if (child is CardsMovedTriggerWindowFrame moved && f.PendingMovementContinuation is { } p && MouLuSuMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p))
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After);
        if (r.Stage == MouLuSuAnniversaryStage.HpChildren && _coreProgramEventHistoryOperations.ProgramEventHistory<MouLuSuAnniversaryHpIssuedEvent>().Count(e => e.FrameId == f.Id && e.Cursor == r.Cursor - 1 && e.Target == r.Other && !e.Recovery) == 1)
            return _coreDyingSuitsAndHistoricalUseObserversOperations.DyingSuitsStructuralEdge(f, child);
        if (r.Stage == MouLuSuAnniversaryStage.RecoveryChildren)
        {
            var target = (r.Cursor - 1) % 2 == 0 ? f.OwnerSeat : r.Other;
            if (_coreProgramEventHistoryOperations.ProgramEventHistory<MouLuSuAnniversaryHpIssuedEvent>().Count(e => e.FrameId == f.Id && e.Cursor == r.Cursor - 1 && e.Target == target && e.Recovery) != 1)
                return false;
            if (child is RecoveryReplacementFrame rr)
                return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(rr, f) && rr.Attempt.Completion.Producer == RecoveryAttemptProducer.Program && rr.Attempt.SourceSeat == f.OwnerSeat && rr.Attempt.TargetSeat == target && rr.Attempt.Amount == 1;
            if (child is HpChangedTriggerWindowFrame hp)
                return hp.ResumeFrameId == f.Id && hp.Change.ParentFrameId == f.Id && hp.Change.TargetSeat == target && hp.Change.Kind == HpChangeKind.Recovery;
        }

        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal bool MouLuSuEdge(int i) => MouLuSuPlayedDeclarationEdge(i) || _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(i) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations._resolutionStack[i - 1], _resolutionStackOperations._resolutionStack[i]) || _resolutionStackOperations._resolutionStack[i - 1] is ProgramSkillFrame f && MouLuSuFirstChild(f, _resolutionStackOperations._resolutionStack[i]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]) || _coreDyingSuitsAndHistoricalUseObserversOperations.DyingSuitsStructuralEdge(_resolutionStackOperations._resolutionStack[i - 1], _resolutionStackOperations._resolutionStack[i]);
    internal bool MouLuSuSuffix(int n) => _resolutionStackOperations._resolutionStack[n + 1] is DyingFrame d && n + 2 < _resolutionStackOperations._resolutionStack.Count && _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n + 1, d) || _coreObserverSubtreesOperations.HasObserverSuffix(n, (i) => _characterLuSuMouOperations.MouLuSuEdge(i), (i, d) => _coreObserverSubtreesOperations2.IsSuspendedOrPaidRescueObserverSuffix(i, d));
    internal ProgramSkillFrame? MouLuSuRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterLuSuMouOperations.MouLuSuFirstChild(f, c), (n) => _characterLuSuMouOperations.MouLuSuSuffix(n));
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 36100)]
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 36100)]
    internal bool MouLuSuDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && MouLuSuRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 36100)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 36100)]
    internal bool MouLuSuDamageWindow(long id) => MouLuSuRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 36100)]
    internal bool MouLuSuBeforeDamage(long id) => MouLuSuRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 36100)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 36100)]
    internal bool MouLuSuRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame rr)
    {
        if (MouLuSuFirstChild(f, rr))
            return true;
        var n = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var c = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == rr.Id);
        return n >= 0 && c > n + 1 && MouLuSuFirstChild(f, _resolutionStackOperations._resolutionStack[n + 1]) && MouLuSuSuffix(n) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(rr, _resolutionStackOperations._resolutionStack[c - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 36100)]
    internal bool MouLuSuAdvanceChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouLuSuRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
