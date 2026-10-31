#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuoJiaMouRuntime
{
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 37100)]
    internal bool MouGjMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => f.MouGuoJia is { } r && MouGuoJia.Valid(f) && e?.Op == r.Operation && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null;
    internal bool MouGjFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!MouGuoJia.Valid(f) || f.MouGuoJia is not { } r)
            return false;
        if (child is JudgmentFrame j)
            return IsMouGjJudgment(j, f);
        if (child is CardsMovedTriggerWindowFrame moved && f.PendingMovementContinuation is { } p && MouGjMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p))
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && (moved.Batch.ParentFrameId == f.Id && m.Sequence > r.Before && m.Sequence <= r.After || r.Stage == MouGuoJiaStage.JudgmentChildren && moved.Batch.ParentFrameId == r.JudgmentId && m.Sequence == r.JudgmentHandoff && m.CardId == r.JudgmentCard && m.From == CardLocation.Judgment(f.OwnerSeat) && m.To == CardLocation.Processing && m.Reason.Value == "skill-program.judgment.result"));
        if (r.Stage == MouGuoJiaStage.DamageChildren && f.AttackAttempt is { TargetSeat: var target, SourceSeat: var source, SourceLess: true } a && target == f.OwnerSeat && source == f.OwnerSeat && a.Nature == DamageNature.Thunder && f.AttackReturn is not null && _coreProgramEventHistoryOperations.ProgramEventHistory<MouGjLightningIssuedEvent>().Count(e => e.FrameId == f.Id && e.JudgmentId == r.JudgmentId && e.Target == target && e.Amount == 3) == 1)
        {
            if (child is BeforeDamageProgramWindowFrame before)
                return before.ParentFrameId == f.Id && before.SourceSeat == f.OwnerSeat && before.TargetSeat == f.OwnerSeat && before.Nature == DamageNature.Thunder;
            if (_coreDyingSuitsAndHistoricalUseObserversOperations.DyingSuitsStructuralEdge(f, child) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(f, child))
                return true;
        }

        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal bool MouGjJudgmentEdge(int i) => i > 0 && _resolutionStackOperations._resolutionStack[i - 1] is JudgmentFrame j && IsMouGjLightningJudgment(j) && _resolutionStackOperations._resolutionStack[i] is ProgramJudgmentTriggerWindowFrame p && p.ParentFrameId == j.Id && p.Judgment.JudgmentFrameId == j.Id;
    internal bool MouGjEdge(int i) => MouGjJudgmentEdge(i) || _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(i) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations._resolutionStack[i - 1], _resolutionStackOperations._resolutionStack[i]) || _resolutionStackOperations._resolutionStack[i - 1] is ProgramSkillFrame f && MouGjFirstChild(f, _resolutionStackOperations._resolutionStack[i]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]);
    internal bool MouGjSuffix(int n) => _resolutionStackOperations._resolutionStack[n + 1] is DyingFrame d && n + 2 < _resolutionStackOperations._resolutionStack.Count && _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n + 1, d) || _coreObserverSubtreesOperations.HasObserverSuffix(n, (i) => _characterGuoJiaMouOperations.MouGjEdge(i), (i, dying) => _coreObserverSubtreesOperations2.IsSuspendedOrPaidRescueObserverSuffix(i, dying));
    internal ProgramSkillFrame? MouGjRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterGuoJiaMouOperations.MouGjFirstChild(f, c), (n) => _characterGuoJiaMouOperations.MouGjSuffix(n));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 37100)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 37100)]
    internal bool MouGjDamageWindow(long id) => MouGjRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 37100)]
    internal bool MouGjBeforeDamage(long id) => MouGjRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 37100)]
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 37100)]
    internal bool MouGjDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && MouGjRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 37100)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 37100)]
    internal bool MouGjRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        var i = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var c = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return i >= 0 && c > i + 1 && MouGjFirstChild(f, _resolutionStackOperations._resolutionStack[i + 1]) && MouGjSuffix(i) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[c - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 37100)]
    internal bool MouGjAdvanceChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouGjRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
