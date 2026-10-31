#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSimaYiMouRuntime
{
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 36200)]
    internal bool MouSmMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => f.MouSimaYi is { } r && effect?.Op == r.Operation && MouSimaYi.Valid(f) && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null;
    internal bool MouSmFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!MouSimaYi.Valid(f) || f.MouSimaYi is not { } r)
            return false;
        if (child is CardDeclarationFrame declaration)
            return MatchesMouSmPlayedDeclaration(f, declaration);
        if (global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f))
            return r.Stage == MouSmStage.Drawing && f.NativeDraw is not null;
        if (child is ProgramCardTriggerWindowFrame card)
            return card.MouSimaYiPlayed is { } back && back.FrameId == f.Id && card.ParentFrameId == f.Id && back.ActionId == r.ResponseActionId && ValidMouSmPlayedWindow(card);
        if (r.Stage == MouSmStage.DamageChildren && r.Cursor > 0 && r.Cursor <= r.Targets.Count && f.AttackAttempt is { } attack && attack.TargetSeat == r.Targets[r.Cursor - 1] && _coreProgramEventHistoryOperations.ProgramEventHistory<MouSmDamageIssuedEvent>().Count(e => e.FrameId == f.Id && e.Cursor == r.Cursor - 1 && e.Target == attack.TargetSeat) == 1)
        {
            if (child is BeforeDamageProgramWindowFrame before)
                return before.ParentFrameId == f.Id && before.SourceSeat == f.OwnerSeat && before.TargetSeat == attack.TargetSeat && before.Amount == 1;
            if (_coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(f, child) || _coreDyingSuitsAndHistoricalUseObserversOperations.DyingSuitsStructuralEdge(f, child))
                return true;
        }

        if (child is CardsMovedTriggerWindowFrame moved && f.PendingMovementContinuation is { } p && MouSmMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p))
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After);
        var lion = r.Materials.FirstOrDefault(c => c.From.Zone == CardZoneKind.Equipment && c.Kind == CardKind.SilverLion && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == c.Id && m.From == c.From && m.Sequence > r.Before && m.Sequence <= r.After));
        if (lion?.From.OwnerSeat is { } holder)
        {
            if (child is RecoveryReplacementFrame rr)
                return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(rr, f) && rr.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && rr.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && rr.Attempt.SourceSeat == holder && rr.Attempt.TargetSeat == holder;
            if (child is HpChangedTriggerWindowFrame hp)
                return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.TargetSeat == holder;
        }

        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal bool MouSmEdge(int i) => MouSmDeclarationEdge(i) || _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(i) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations._resolutionStack[i - 1], _resolutionStackOperations._resolutionStack[i]) || _resolutionStackOperations._resolutionStack[i - 1] is ProgramSkillFrame f && MouSmFirstChild(f, _resolutionStackOperations._resolutionStack[i]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]);
    internal bool MouSmSuffix(int n) => _resolutionStackOperations._resolutionStack[n + 1] is DyingFrame d && n + 2 < _resolutionStackOperations._resolutionStack.Count && _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n + 1, d) || _coreObserverSubtreesOperations.HasObserverSuffix(n, (i) => _characterSimaYiMouOperations.MouSmEdge(i), (i, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(i, dying));
    internal ProgramSkillFrame? MouSmRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterSimaYiMouOperations.MouSmFirstChild(f, c), (n) => _characterSimaYiMouOperations.MouSmSuffix(n));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 36200)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 36200)]
    internal bool MouSmDamageWindow(long id) => MouSmRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 36200)]
    internal bool MouSmBeforeDamage(long id) => MouSmRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 36200)]
    internal bool MouSmDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && MouSmRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 36200)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 36200)]
    internal bool MouSmRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (MouSmFirstChild(f, recovery))
            return true;
        var n = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var c = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return n >= 0 && c > n + 1 && MouSmFirstChild(f, _resolutionStackOperations._resolutionStack[n + 1]) && MouSmSuffix(n) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[c - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 36200)]
    internal bool MouSmAdvanceChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouSmRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
