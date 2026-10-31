#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineTurnDefaultStatsValidationRuntime
{
    internal bool ExactTurnDefaultStatPhaseParent(ProgramSkillFrame f)
    {
        if (f.TriggerId is null || f.WindowContext is not { } c || c.OwnerSeat != f.OwnerSeat || c.SourceSeat != f.OwnerSeat || c.TargetSeat != f.OwnerSeat || f.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (index < 1)
            return false;
        if (c.Window == SkillProgramTriggerWindow.PlayPhaseStarting)
            return _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play && _resolutionStackOperations._resolutionStack[index - 1] is PlayPhaseStartingBoundaryFrame parent && parent.Id == c.ParentFrameId && parent.OwnerSeat == f.OwnerSeat && parent.ItemIndex >= 0 && parent.ItemIndex < parent.Items.Count && parent.Items[parent.ItemIndex].Candidate is { } candidate && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, candidate);
        if (_resolutionStackOperations._resolutionStack[index - 1] is not ProgramLifecycleTriggerWindowFrame lifecycle || lifecycle.Id != c.ParentFrameId || lifecycle.OwnerSeat != f.OwnerSeat || lifecycle.Window != c.Window || lifecycle.CandidateIndex < 0 || lifecycle.CandidateIndex >= lifecycle.Candidates.Count || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, lifecycle.Candidates[lifecycle.CandidateIndex]))
            return false;
        return c.Window switch
        {
            SkillProgramTriggerWindow.DrawPhaseStarting => _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Draw && lifecycle.Continuation == ProgramLifecycleContinuation.CompleteDrawPhase,
            SkillProgramTriggerWindow.DiscardPhaseStarting => _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Discard && lifecycle.Continuation == ProgramLifecycleContinuation.CompleteDiscardPhase,
            _ => false
        };
    }

    internal bool ExactTurnDefaultStatDamageParent(ProgramSkillFrame f, out DamageFrame damage, out DamageTriggerWindowFrame window, out GameEngine.IDamageAttempt attack)
    {
        damage = null !;
        window = null !;
        attack = null !;
        if (f.TriggerId is null || f.OwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || f.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied, DamageFrameId: { } damageId, Amount: > 0 } c || c.OwnerSeat != f.OwnerSeat || c.TargetSeat != f.OwnerSeat)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (index < 2 || _resolutionStackOperations._resolutionStack[index - 1] is not DamageTriggerWindowFrame w || w.Id != c.ParentFrameId || w.ParentFrameId != damageId || w.TriggerWindow != c.Window || w.TargetSeat != f.OwnerSeat || w.CandidateIndex < 0 || w.CandidateIndex >= w.Candidates.Count || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, w.Candidates[w.CandidateIndex].ToProgramCandidate()) || _resolutionStackOperations._resolutionStack[index - 2] is not DamageFrame d || d.Id != damageId || d.TargetSeat != f.OwnerSeat || d.Amount != c.Amount || d.Amount <= 0)
            return false;
        var a = _coreNativeDamageHandlesOperations.Value.GetDamageTriggerAttack(w);
        if (a.ResolutionId != d.ParentFrameId || !a.DamageWasApplied || a.TargetSeat != f.OwnerSeat || a.SourceSeat != d.SourceSeat || a.DamageAmount != d.Amount || _coreNativeCardClassificationsOperations.GetDamageNature(a) != d.Nature || c.SourceSeat != (a.IsSourceLess ? null : a.SourceSeat) || w.SourceSeat != d.SourceSeat)
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        if (!TurnDefaultStatDamageRequestMatches(history, d.Id, d.SourceSeat, f.OwnerSeat, d.Amount, d.Nature, a.IsSourceLess))
            return false;
        damage = d;
        window = w;
        attack = a;
        return true;
    }

    internal static bool TurnDefaultStatDamageRequestMatches(IReadOnlyList<IGameEvent> history, long damageId, int source, int target, int amount, DamageNature nature, bool sourceLess) => RuntimeSkills.RuntimeRules_Core_TurnDefaultStatsValidation.TurnDefaultStatDamageRequestMatches(history, damageId, source, target, amount, nature, sourceLess);

    internal bool IsTurnDefaultStatChoice(ProgramSkillFrame f, PendingDecision decision)
    {
        if (f.TurnDefaultStatAllocation is null || decision.Kind != DecisionKind.ProgramTrigger || !decision.IsPrivate || decision.PlayerSeat != f.OwnerSeat || decision.SourceSeat != f.OwnerSeat || decision.TargetSeat != f.OwnerSeat || decision.SkillPrompt?.SkillId != f.SkillId || decision.ValidCardIds.Count != 0 || decision.ValidTargetSeats.Count != 0 || decision.ValidContentIds.Count != 0 || decision.RequiredCardCount != 0)
            return false;
        return CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(decision.Choices, _coreTurnDefaultStatsOperations.TurnDefaultStatChoices(f));
    }
}
