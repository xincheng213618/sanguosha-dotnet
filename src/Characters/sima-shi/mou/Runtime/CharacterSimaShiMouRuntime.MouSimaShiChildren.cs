#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSimaShiMouRuntime
{
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 36400)]
    internal bool MouSimaShiMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => f.MouSimaShi is { Stage: MouSimaShiStage.MovementChildren } r && effect?.Op == r.Operation && MouSimaShiRules.Valid(f) && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null;
    internal bool MouSimaShiFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!MouSimaShiRules.Valid(f) || f.MouSimaShi is not { } r)
            return false;
        if (child is CardsMovedTriggerWindowFrame moved && f.PendingMovementContinuation is { } p && MouSimaShiMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p))
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After);
        var lion = r.Cards.FirstOrDefault(c => c.From.Zone == CardZoneKind.Equipment && c.Kind == CardKind.SilverLion && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == c.Id && m.From == c.From && m.Sequence > r.Before && m.Sequence <= r.After));
        if (lion?.From.OwnerSeat is { } holder)
        {
            if (child is RecoveryReplacementFrame rr)
                return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(rr, f) && rr.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && rr.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && rr.Attempt.SourceSeat == holder && rr.Attempt.TargetSeat == holder;
            if (child is HpChangedTriggerWindowFrame hp)
                return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.TargetSeat == holder;
        }

        if (r.Stage == MouSimaShiStage.Damage && f.AttackAttempt is { } a && f.AttackReturn is not null && a.SourceSeat == f.OwnerSeat && a.TargetSeat == r.Target && !a.SourceLess && _coreProgramEventHistoryOperations.ProgramEventHistory<MouSimaShiTargetEvent>().Count(e => e.FrameId == f.Id && e.Owner == a.SourceSeat && e.Target == a.TargetSeat) == 1)
            return _coreDyingSuitsAndHistoricalUseObserversOperations.DyingSuitsStructuralEdge(f, child);
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal bool MouSimaShiEdge(int i) => _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(i) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations._resolutionStack[i - 1], _resolutionStackOperations._resolutionStack[i]) || _resolutionStackOperations._resolutionStack[i - 1] is ProgramSkillFrame f && MouSimaShiFirstChild(f, _resolutionStackOperations._resolutionStack[i]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]);
    internal bool MouSimaShiSuffix(int n) => _resolutionStackOperations._resolutionStack[n + 1] is DyingFrame d && n + 2 < _resolutionStackOperations._resolutionStack.Count && _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n + 1, d) || _coreObserverSubtreesOperations.HasObserverSuffix(n, (i) => _characterSimaShiMouOperations.MouSimaShiEdge(i), (i, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(i, dying));
    internal ProgramSkillFrame? MouSimaShiRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterSimaShiMouOperations.MouSimaShiFirstChild(f, c), (n) => _characterSimaShiMouOperations.MouSimaShiSuffix(n));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 36400)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 36400)]
    internal bool MouSimaShiDamageWindow(long id) => MouSimaShiRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 36400)]
    internal bool MouSimaShiBeforeDamage(long id) => MouSimaShiRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 36400)]
    internal bool MouSimaShiDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && MouSimaShiRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 36400)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 36400)]
    internal bool MouSimaShiRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (MouSimaShiFirstChild(f, recovery))
            return true;
        var n = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var c = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return n >= 0 && c > n + 1 && MouSimaShiFirstChild(f, _resolutionStackOperations._resolutionStack[n + 1]) && MouSimaShiSuffix(n) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[c - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 36400)]
    internal bool MouSimaShiAdvanceChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouSimaShiRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
