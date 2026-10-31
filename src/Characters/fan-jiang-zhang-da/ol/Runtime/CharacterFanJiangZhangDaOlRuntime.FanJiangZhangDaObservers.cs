#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterFanJiangZhangDaOlRuntime
{
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.BeforeSelectionState, 27350)]
    internal void AssertJuesheng(ProgramSkillFrame f)
    {
        if (f.Juesheng is not { } r)
            return;
        var o = r.Origin;
        if (f.SkillId != "ol:juesheng" || f.ActivationId != "limited-duel" || f.TriggerId is not null || f.WindowContext is not null || f.InstructionIndex != 1 || r.InstructionIndex != 1 || o.ProgramFrameId != f.Id || o.Source != new CardConversionSource(f.SkillId, f.ActivationId, f.OwnerSeat, f.SkillInstanceId) || o.GameplayHash != f.GameplayHash || o.ActorSeat != f.OwnerSeat || o.ActualTurn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || f.SelectedCardIds.Count != 0 || !f.SelectedTargetSeats.SequenceEqual([o.InitialTargetSeat]) || _coreProgramEventHistoryOperations.ProgramEventHistory<JueshengSpentEvent>().Count(e => e.OwnerSeat == f.OwnerSeat && e.SkillId == f.SkillId) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<JueshengDuelIssuedEvent>().Count(e => e.Origin == o && e.InstructionIndex == r.InstructionIndex) != 1 || r.GrantCursor < 0 || r.GrantCursor > r.FinalTargets.Count || r.Targets.Select(t => t.TargetSeat).Distinct().Count() != r.Targets.Count || r.Targets.Any(t => t.SlashCount < 0 || t.Amount != Math.Max(1, t.SlashCount)) || !_coreProgramEventHistoryOperations.ProgramEventHistory<JueshengTargetDamageFrozenEvent>().Where(e => e.FrameId == f.Id && e.UseId == o.UseId).OrderBy(e => e.Index).Select(e => new JueshengTargetDamage(e.TargetSeat, e.SlashCount, e.Amount)).SequenceEqual(r.Targets) || r.FinalTargets.Distinct().Count() != r.FinalTargets.Count || !_coreProgramEventHistoryOperations.ProgramEventHistory<JueshengFinalTargetEvent>().Where(e => e.FrameId == f.Id && e.UseId == o.UseId).OrderBy(e => e.Index).Select(e => e.TargetSeat).SequenceEqual(r.FinalTargets))
            throw new InvalidOperationException("Juesheng lost its exact permanent spend, immutable native target damage invoices or then-clause cursor.");
        if (r.NativeCompleted ? _coreCardUseLifecycleOperations.LifecycleCardUse(o.UseId)is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == o.UseId) != 1 : _coreCardUseLifecycleOperations.LifecycleCardUse(o.UseId)is not { } use || !IsJueshengDuelUse(use.Id))
            throw new InvalidOperationException("Juesheng must preserve its exact whole native Use or one completed native return.");
        if (r.Stage == JueshengStage.GrantChildren && !r.NativeCompleted || f.PendingMovementContinuation is not null)
            throw new InvalidOperationException("Juesheng's issued grants remain on their owning receipt and resume through the program continuation.");
        var processed = _coreProgramEventHistoryOperations.ProgramEventHistory<JueshengSkillGrantedEvent>().Where(e => e.FrameId == f.Id).Select(e => (e.Index, e.RecipientSeat)).Concat(_coreProgramEventHistoryOperations.ProgramEventHistory<JueshengGrantSkippedEvent>().Where(e => e.FrameId == f.Id).Select(e => (e.Index, e.RecipientSeat))).OrderBy(e => e.Index).ToArray();
        if (processed.Length != r.GrantCursor || processed.Where((e, i) => e.Index != i || e.RecipientSeat != r.FinalTargets[i]).Any())
            throw new InvalidOperationException("Juesheng cannot pay or skip the same recipient twice.");
    }

    internal bool FanJiangZhangDaFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.Juesheng is not { } r)
            return false;
        AssertJuesheng(f);
        if (child is CardUseFrame use)
            return r.Stage == JueshengStage.NativeUse && use.JueshengDuel == r.Origin && IsJueshengDuelUse(use.Id);
        return r.Stage == JueshengStage.GrantChildren && child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } changed && changed.ResumeProgramFrameId == f.Id && changed.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal ProgramSkillFrame? FanJiangZhangDaObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterFanJiangZhangDaOlOperations.FanJiangZhangDaFirstChild(f, c), (i) => _coreObserverSubtreesOperations2.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations2.IsMovementObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations2.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 810)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 920)]
    internal bool HasFanJiangZhangDaDamageObserver(long id) => FanJiangZhangDaObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 950)]
    internal bool HasFanJiangZhangDaBeforeDamageObserver(long id) => FanJiangZhangDaObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 780)]
    internal bool IsFanJiangZhangDaDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && FanJiangZhangDaObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27350)]
    internal bool AdvanceFanJiangZhangDaSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && FanJiangZhangDaObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
