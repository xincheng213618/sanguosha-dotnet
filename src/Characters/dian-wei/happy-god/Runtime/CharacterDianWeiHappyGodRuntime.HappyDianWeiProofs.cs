#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDianWeiHappyGodRuntime
{
    internal bool HappyGuardOriginalCandidate(ProgramSkillFrame f)
    {
        if (f.WindowContext is not { } context || context.OwnerSeat != f.OwnerSeat || f.ActivationId != f.TriggerId || _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramBindingStartedEvent>().Count(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.SkillId == f.SkillId && e.SkillInstanceId == f.SkillInstanceId && e.BindingId == f.TriggerId && e.Window == context.Window) != 1)
            return false;
        return _resolutionStackOperations._resolutionStack.SingleOrDefault(p => p.Id == context.ParentFrameId) switch
        {
            ProgramLifecycleTriggerWindowFrame w => w.Window == context.Window && w.CandidateIndex >= 0 && w.CandidateIndex < w.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, w.Candidates[w.CandidateIndex]),
            BeforeDamageProgramWindowFrame w => context.Window == SkillProgramTriggerWindow.BeforeDamageApplied && w.CandidateIndex >= 0 && w.CandidateIndex < w.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, w.Candidates[w.CandidateIndex].Candidate) && w.TargetSeat == context.TargetSeat && w.Amount == context.Amount,
            _ => false
        };
    }

    internal bool ValidHappyGuardFrame(ProgramSkillFrame f)
    {
        if (f.HappyGuard is not { } r || r.Source != HappyGuardSource(f) || r.GameplayHash != f.GameplayHash || r.ActualTurn != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || r.ParentId != f.WindowContext?.ParentFrameId || !Enum.IsDefined(r.Stage) || !HappyGuardOriginalCandidate(f))
            return false;
        var initial = f.TriggerId switch
        {
            "grant-guards" => HappyGuardStage.GrantChoice,
            "guard-damage" => HappyGuardStage.GuardChoice,
            "removed-guard" => HappyGuardStage.ElaiChoice,
            _ => (HappyGuardStage? )null
        };
        if (initial is null || _coreProgramEventHistoryOperations.ProgramEventHistory<HappyGuardStartedEvent>().Count(e => e == new HappyGuardStartedEvent(f.Id, r.Source, r.GameplayHash, r.ParentId, r.ActualTurn, initial.Value, r.GuardedSeat, r.AttackId, r.Amount, r.GrantLimit)) != 1)
            return false;
        if (initial == HappyGuardStage.GrantChoice)
            return f.SkillId == HappyDianWeiComposition.Shenwei && f.WindowContext?.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow && r.Stage is HappyGuardStage.GrantChoice or HappyGuardStage.GrantCommitted or HappyGuardStage.Declined && r.GrantLimit is 1 or 2 && f.InstructionIndex == 1 && f.SelectedTargetSeats.Count == 0;
        if (initial == HappyGuardStage.ElaiChoice)
        {
            if (f.SkillId != HappyDianWeiComposition.Elai || f.WindowContext?.Window != SkillProgramTriggerWindow.HappyGuardRemoved || f.InstructionIndex is < 1 or > 3 || r.Stage is not (HappyGuardStage.ElaiChoice or HappyGuardStage.ElaiCommitted) || _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(w => w.Id == r.ParentId)?.HappyGuardRemoval is not { } removal || !HappyGuardRemovalIsPaid(removal))
                return false;
            if (r.Stage == HappyGuardStage.ElaiChoice)
                return r.ElaiMode is null && f.SelectedTargetSeats.Count == 0 && f.InstructionIndex == 1;
            return r.ElaiMode is ("recover" or "damage" or "none") && f.SelectedTargetSeats.SequenceEqual(r.ElaiMode == "damage" ? [r.ElaiTarget] : []) && f.ChoiceBindings.Count(b => b.Name == "elai-mode" && b.ChooserSeat == f.OwnerSeat && b.OptionId == r.ElaiMode) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<HappyGuardElaiChoiceEvent>().Count(e => e == new HappyGuardElaiChoiceEvent(f.Id, removal.RemovalId, f.OwnerSeat, r.ElaiMode, r.ElaiTarget)) == 1;
        }

        if (f.SkillId != HappyDianWeiComposition.Shenwei || f.WindowContext?.Window != SkillProgramTriggerWindow.BeforeDamageApplied || f.InstructionIndex != 1 || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.GuardedSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.DamageSource) || r.Amount <= 0 || r.AttackId <= 0 || r.Stage is not (HappyGuardStage.GuardChoice or HappyGuardStage.GuardPaid or HappyGuardStage.RemovedChildren or HappyGuardStage.Transferred or HappyGuardStage.Declined) || _resolutionStackOperations._resolutionStack.OfType<BeforeDamageProgramWindowFrame>().SingleOrDefault(w => w.Id == r.ParentId)is not { } window || window.SourceSeat != r.DamageSource || window.TargetSeat != r.GuardedSeat || window.Amount != r.Amount)
            return false;
        if (r.Stage is HappyGuardStage.GuardChoice or HappyGuardStage.Declined)
            return r.Stage == HappyGuardStage.Declined || _coreMatchParticipantsPlayersAccess._players[r.GuardedSeat].MarkerSourceCounts.GetValueOrDefault((PlayerMarkerKind.HappyGuard, f.OwnerSeat)) == 1;
        return (r.Stage != HappyGuardStage.Transferred || _coreProgramEventHistoryOperations.ProgramEventHistory<HappyGuardTransferredEvent>().Count(e => e == new HappyGuardTransferredEvent(f.Id, r.AttackId, r.GuardedSeat, window.RedirectedTargetSeat ?? r.GuardedSeat, r.Amount)) == 1) && _coreProgramEventHistoryOperations.ProgramEventHistory<HappyGuardPaidEvent>().Count(e => e == new HappyGuardPaidEvent(f.Id, r.Source, r.GameplayHash, r.ParentId, r.AttackId, r.GuardedSeat, r.DamageSource, r.Amount, r.Nature)) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<PlayerMarkerChangedEvent>().Count(e => e.ResolutionId == f.Id && e.PlayerSeat == r.GuardedSeat && e.SkillOwnerSeat == f.OwnerSeat && e.Marker == PlayerMarkerKind.HappyGuard && e.Delta == -1 && e.Count == 0 && e.Reason == "skill-program.happy:shenwei.guard") == 1;
    }

    internal bool HasExactHappyDianWeiElaiSelection(ProgramSkillFrame f) => f.HappyGuard is { Stage: HappyGuardStage.ElaiCommitted } && ValidHappyGuardFrame(f);
    internal bool TryAssertHappyDianWeiDeathRemoval(DeathFrame death)
    {
        if (death.HappyGuardRemovals is not { } removed)
            return false;
        if (!death.HappyGuardMarkerCleanupCompleted || !death.OwnerDiedProgramsResolved || !death.KillerProgramsResolved || _coreMatchParticipantsPlayersAccess._players[death.VictimSeat].IsAlive || removed.Count is < 1 || removed.Count > _coreMatchDefinitionPlayerCountAccess._playerCount || death.HappyGuardRemovalIndex < 0 || death.HappyGuardRemovalIndex > removed.Count || removed.Select(r => r.RemovedSeat).Distinct().Count() != removed.Count || removed.Where((r, i) => r.Ordinal != i || r.GuardOwner != death.VictimSeat || r.ResolutionId != death.Id || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.RemovedSeat)).Any())
            throw new InvalidOperationException("Guard-removal death stage lost its original completed cleanup or frozen queue.");
        if (death.HappyGuardRemovalIndex == removed.Count)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == death.Id);
        var entry = removed[death.HappyGuardRemovalIndex];
        if (index < 0 || index + 1 >= _resolutionStackOperations._resolutionStack.Count || _resolutionStackOperations._resolutionStack[index + 1] is not ProgramLifecycleTriggerWindowFrame { HappyGuardRemoval: { } ret } window || ret != new HappyGuardRemovalReturn(death.Id, entry.RemovedSeat, entry.GuardOwner, entry.ResolutionId, true) || !HappyDianWeiStructuralEdge(window, death) || window.OwnerSeat != entry.RemovedSeat || window.CandidateIndex < 0 || window.CandidateIndex > window.Candidates.Count)
            throw new InvalidOperationException("Guard-removal death stage requires its exact adjacent typed lifecycle return.");
        if (index + 2 == _resolutionStackOperations._resolutionStack.Count)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
                throw new InvalidOperationException("An unstarted guard-removal lifecycle window cannot own a skill prompt.");
            return true;
        }

        if (_resolutionStackOperations._resolutionStack[index + 2] is not ProgramSkillFrame { HappyGuard: { } r } elai || !HappyDianWeiStructuralEdge(elai, window))
            throw new InvalidOperationException("Guard-removal death stage lost its actual Elai binding.");
        for (var child = index + 3; child < _resolutionStackOperations._resolutionStack.Count; child++)
        {
            if (!_coreObserverSubtreesOperations.IsMovementObserverEdge(child) && !global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations._resolutionStack[child], _resolutionStackOperations._resolutionStack[child - 1]))
                throw new InvalidOperationException("Guard-removal death stage has an unproved native child return.");
            if (_resolutionStackOperations._resolutionStack[child] is DyingFrame dying && child + 1 < _resolutionStackOperations._resolutionStack.Count && _coreObserverSubtreesOperations.IsPaidRescueObserverSuffix(child, dying))
                break;
        }

        if (index + 3 == _resolutionStackOperations._resolutionStack.Count && r.Stage == HappyGuardStage.ElaiChoice && (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } prompt || prompt.PlayerSeat != elai.OwnerSeat || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(prompt.Choices, HappyGuardChoices(elai))))
            throw new InvalidOperationException("Guard-removal Elai lost its exact native chooser menu.");
        if (_resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(f => f.AttackAttempt is not null))
            _coreProgramAttackAttemptOperations.AssertProgramAttackState();
        return true;
    }

    internal bool HasHappyDianWeiGuardPrompt() => _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { HappyGuard: { Stage: HappyGuardStage.GuardChoice } r } f && _corePendingDecisionPolicyPipelineOperations._pendingDecision is { Kind: DecisionKind.ProgramTrigger } p && p.PlayerSeat == r.GuardedSeat && p.SkillPrompt?.SkillId == HappyDianWeiComposition.Shenwei && ValidHappyGuardFrame(f) && global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(p.Choices, HappyGuardChoices(f));
    internal bool IsOriginalDyingSuspendedByHappyGuardElai(DyingFrame dying)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id);
        if (index < 1 || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(dying.VictimSeat) || _coreMatchParticipantsPlayersAccess._players[dying.VictimSeat].IsAlive || dying.ResponderIndex != dying.ResponderSeats.Count || _resolutionStackOperations._resolutionStack[index - 1].Id != dying.ParentFrameId || index + 3 >= _resolutionStackOperations._resolutionStack.Count || _resolutionStackOperations._resolutionStack[index + 1] is not DeathFrame { ReturnKind: DeathReturnKind.Dying, HappyGuardMarkerCleanupCompleted: true } death || death.ParentFrameId != dying.Id || death.VictimSeat != dying.VictimSeat || _resolutionStackOperations._resolutionStack[index + 2] is not ProgramLifecycleTriggerWindowFrame { HappyGuardRemoval: { DeathParent: true } ret } window || ret.ParentId != death.Id || _resolutionStackOperations._resolutionStack[index + 3] is not ProgramSkillFrame { HappyGuard: { } r } elai || r.ParentId != window.Id || elai.SkillId != HappyDianWeiComposition.Elai || !HappyDianWeiStructuralEdge(window, death) || !HappyDianWeiStructuralEdge(elai, window) || _coreProgramEventHistoryOperations.ProgramEventHistory<PlayerDyingEvent>().Count(e => e.ResolutionId == dying.Id && e.VictimSeat == dying.VictimSeat && e.KillerSeat == dying.KillerSeat) != 1)
            return false;
        return dying.Continuation switch
        {
            DyingContinuationKind.ProgramSkill => _resolutionStackOperations._resolutionStack[index - 1] is ProgramSkillFrame && dying.KillerSeat is null,
            DyingContinuationKind.Damage => _resolutionStackOperations._resolutionStack[index - 1] is DamageFrame d && d.TargetSeat == dying.VictimSeat,
            DyingContinuationKind.AttackHpLoss => _resolutionStackOperations._resolutionStack[index - 1] is ProgramSkillFrame { AttackAttempt: { } a, AttackReturn: not null } && a.TargetSeat == dying.VictimSeat && dying.KillerSeat is null,
            _ => false
        };
    }

    internal bool HappyDianWeiStructuralEdge(ResolutionFrame child, ResolutionFrame parent) => child is ProgramSkillFrame { HappyGuard: { } r } f && r.ParentId == parent.Id && ValidHappyGuardFrame(f) && (parent is BeforeDamageProgramWindowFrame && f.SkillId == HappyDianWeiComposition.Shenwei && f.TriggerId == "guard-damage" || parent is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.HappyGuardRemoved } && f.SkillId == HappyDianWeiComposition.Elai) || child is ProgramLifecycleTriggerWindowFrame { HappyGuardRemoval: { } ret } w && ret.ParentId == parent.Id && w.Window == SkillProgramTriggerWindow.HappyGuardRemoved && w.Continuation == ProgramLifecycleContinuation.HappyGuardRemoved && w.ResumeProgramFrameId == parent.Id && HappyGuardRemovalIsPaid(ret) && (ret.DeathParent ? parent is DeathFrame : parent is ProgramSkillFrame { HappyGuard: { Stage: HappyGuardStage.RemovedChildren } });
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 41980)]
    internal void AssertHappyDianWei(ProgramSkillFrame f)
    {
        if (f.HappyGuard is not null && !ValidHappyGuardFrame(f))
            throw new InvalidOperationException("Happy Dian Wei lost its original native window, exact source, issued guard payment or Elai participant proof.");
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 41980)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 41980)]
    internal bool ContinuePaidHappyGuard(ProgramSkillFrame f) => f.HappyGuard is { Stage: HappyGuardStage.GuardPaid or HappyGuardStage.RemovedChildren or HappyGuardStage.Transferred or HappyGuardStage.ElaiCommitted } && ValidHappyGuardFrame(f);
}
