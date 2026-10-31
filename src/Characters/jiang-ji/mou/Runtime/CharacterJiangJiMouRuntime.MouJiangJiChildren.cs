#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterJiangJiMouRuntime
{
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 36300)]
    internal bool MouJjMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => f.MouJiangJi is { } r && effect?.Op == r.Operation && MouJiangJi.Valid(f) && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null;
    internal bool MouJjFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!MouJiangJi.Valid(f) || f.MouJiangJi is not { } r)
            return false;
        if (child is CardUseFrame u)
            return MouJjNativeRidesOn(u, f);
        if (child is CardsMovedTriggerWindowFrame moved && f.PendingMovementContinuation is { } p && MouJjMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p))
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After);
        if (global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f))
            return r.Stage == MouJiangJiStage.Drawing;
        var lion = r.Card is { Kind: CardKind.SilverLion, From.Zone: CardZoneKind.Equipment } card && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == card.Id && m.From == card.From && m.Sequence > r.Before && m.Sequence <= r.After) ? card : null;
        var paidLionHolder = lion?.From.OwnerSeat ?? (r.Stage == MouJiangJiStage.DiscardChildren && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => r.Selected.Contains(m.CardId) && m.CardKind == CardKind.SilverLion && m.From == CardLocation.Equipment(r.Target) && m.Sequence > r.Before && m.Sequence <= r.After) ? r.Target : -1);
        if (paidLionHolder >= 0)
        {
            if (child is RecoveryReplacementFrame rr)
                return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(rr, f) && rr.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && rr.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && rr.Attempt.SourceSeat == paidLionHolder && rr.Attempt.TargetSeat == paidLionHolder;
            if (child is HpChangedTriggerWindowFrame hp)
                return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.TargetSeat == paidLionHolder;
        }

        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal bool MouJjEdge(int i) => _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(i) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations._resolutionStack[i - 1], _resolutionStackOperations._resolutionStack[i]) || _resolutionStackOperations._resolutionStack[i - 1] is ProgramSkillFrame f && MouJjFirstChild(f, _resolutionStackOperations._resolutionStack[i]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]);
    internal bool MouJjSuffix(int n) => _resolutionStackOperations._resolutionStack[n + 1] is DyingFrame d && n + 2 < _resolutionStackOperations._resolutionStack.Count && _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n + 1, d) || _coreObserverSubtreesOperations.HasObserverSuffix(n, (i) => _characterJiangJiMouOperations.MouJjEdge(i), (i, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(i, dying));
    internal ProgramSkillFrame? MouJjObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterJiangJiMouOperations.MouJjFirstChild(f, c), (n) => _characterJiangJiMouOperations.MouJjSuffix(n));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 36300)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 36300)]
    internal bool MouJjDamageWindow(long id) => MouJjObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 36300)]
    internal bool MouJjBeforeDamage(long id) => MouJjObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 36300)]
    internal bool MouJjDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && MouJjObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 36300)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 36300)]
    internal bool MouJjRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (MouJjFirstChild(f, recovery))
            return true;
        var n = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var c = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return n >= 0 && c > n + 1 && MouJjFirstChild(f, _resolutionStackOperations._resolutionStack[n + 1]) && MouJjSuffix(n) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[c - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 36300)]
    internal bool MouJjAdvanceChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouJjObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
