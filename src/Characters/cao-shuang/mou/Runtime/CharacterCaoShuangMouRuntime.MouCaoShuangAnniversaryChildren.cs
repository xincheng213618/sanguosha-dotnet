#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoShuangMouRuntime
{
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 35500)]
    internal bool MouCaoShuangAnniversaryMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => f.MouCaoShuangAnniversary is { } r && effect?.Op == r.Operation && MouCaoShuangAnniversary.Valid(f) && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null;
    internal bool MouCaoShuangAnniversaryFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!MouCaoShuangAnniversary.Valid(f) || f.MouCaoShuangAnniversary is not { } r)
            return false;
        if (global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f))
            return r.Stage == MouCaoShuangAnniversaryStage.Drawing && f.NativeDraw is not null;
        if (child is CardsMovedTriggerWindowFrame moved && f.PendingMovementContinuation is { } p && MouCaoShuangAnniversaryMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p))
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After);
        if (r.Stage == MouCaoShuangAnniversaryStage.MoveChildren && r.Option != 3 && r.Cards.Any(c => c.From.Zone == CardZoneKind.Equipment && c.Kind == CardKind.SilverLion))
        {
            var lion = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => r.Cards.Any(c => c.Id == m.CardId && c.From == m.From) && m.CardKind == CardKind.SilverLion && m.Sequence > r.Before && m.Sequence <= r.After && m.Reason.Value == MouCaoShuangAnniversaryRuntime.DiscardReason);
            if (lion && child is RecoveryReplacementFrame rr)
                return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(rr, f) && rr.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && rr.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && rr.Attempt.SourceSeat == r.Payer && rr.Attempt.TargetSeat == r.Payer;
            if (lion && child is HpChangedTriggerWindowFrame hp)
                return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.TargetSeat == r.Payer;
        }

        if (r.Stage == MouCaoShuangAnniversaryStage.HpChildren && _coreProgramEventHistoryOperations.ProgramEventHistory<MouCaoShuangAnniversaryHpIssuedEvent>().Count(e => e.FrameId == f.Id && e.Cursor == r.Cursor - 1 && e.Target == r.Payer) == 1)
            return _coreDyingSuitsAndHistoricalUseObserversOperations.DyingSuitsStructuralEdge(f, child);
        if (r.Stage == MouCaoShuangAnniversaryStage.RecoveryChildren)
        {
            if (child is RecoveryReplacementFrame rr)
                return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(rr, f) && rr.Attempt.Completion.Producer == RecoveryAttemptProducer.Program && rr.Attempt.SourceSeat == f.OwnerSeat && rr.Attempt.TargetSeat == f.OwnerSeat;
            if (child is HpChangedTriggerWindowFrame hp)
                return hp.ResumeFrameId == f.Id && hp.Change.ParentFrameId == f.Id && hp.Change.TargetSeat == f.OwnerSeat;
        }

        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal bool MouCaoShuangAnniversaryEdge(int i) => _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(i) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations._resolutionStack[i - 1], _resolutionStackOperations._resolutionStack[i]) || _resolutionStackOperations._resolutionStack[i - 1] is ProgramSkillFrame f && MouCaoShuangAnniversaryFirstChild(f, _resolutionStackOperations._resolutionStack[i]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]) || _coreDyingSuitsAndHistoricalUseObserversOperations.DyingSuitsStructuralEdge(_resolutionStackOperations._resolutionStack[i - 1], _resolutionStackOperations._resolutionStack[i]);
    internal bool MouCaoShuangAnniversarySuffix(int n) => _resolutionStackOperations._resolutionStack[n + 1] is DyingFrame d && n + 2 < _resolutionStackOperations._resolutionStack.Count && _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n + 1, d) || _coreObserverSubtreesOperations.HasObserverSuffix(n, (i) => _characterCaoShuangMouOperations.MouCaoShuangAnniversaryEdge(i), (i, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(i, dying));
    internal ProgramSkillFrame? MouCaoShuangAnniversaryRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterCaoShuangMouOperations.MouCaoShuangAnniversaryFirstChild(f, c), (n) => _characterCaoShuangMouOperations.MouCaoShuangAnniversarySuffix(n));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 35500)]
    internal bool MouCaoShuangAnniversaryDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && MouCaoShuangAnniversaryRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 35500)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 35500)]
    internal bool MouCaoShuangAnniversaryRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (MouCaoShuangAnniversaryFirstChild(f, recovery))
            return true;
        var n = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var c = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return n >= 0 && c > n + 1 && MouCaoShuangAnniversaryFirstChild(f, _resolutionStackOperations._resolutionStack[n + 1]) && MouCaoShuangAnniversarySuffix(n) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[c - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 35500)]
    internal bool MouCaoShuangAnniversaryAdvanceChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouCaoShuangAnniversaryRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
