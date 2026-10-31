#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDianWeiHappyGodRuntime
{
    internal bool BeginHappyGuardRemovedWindow(HappyGuardRemovalReturn ret)
    {
        var participantFacts = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).ToDictionary(p => p.Seat, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts);
        var candidates = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).SelectMany(p => _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(p, SkillProgramTriggerWindow.HappyGuardRemoved)).Where(c => c.SkillId == HappyDianWeiComposition.Elai && c.BindingId == "removed-guard").OrderByDescending(c => c.Priority).ThenBy(c => _coreTableOrderOperations.RelativeTableOrder(_coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, c.OwnerSeat)).ThenBy(c => c.SkillInstanceId, StringComparer.Ordinal).ToArray();
        if (candidates.Length == 0)
            return false;
        _coreRuntimeOperations.PushRuntimeFrame(new ProgramLifecycleTriggerWindowFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, ret.RemovedSeat, SkillProgramTriggerWindow.HappyGuardRemoved, Array.AsReadOnly(candidates), ProgramLifecycleContinuation.HappyGuardRemoved, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[ret.RemovedSeat])) { HappyGuardRemoval = ret, ResumeProgramFrameId = ret.ParentId, ParticipantFacts = participantFacts });
        _coreRuntimeOperations.AdvanceRuntimeTop<ProgramLifecycleTriggerWindowFrame>();
        return true;
    }

    internal bool HappyGuardRemovalIsPaid(HappyGuardRemovalReturn r)
    {
        if (r.DeathParent)
            return _resolutionStackOperations._resolutionStack.OfType<DeathFrame>().SingleOrDefault(d => d.Id == r.ParentId)is { HappyGuardMarkerCleanupCompleted: true, HappyGuardRemovals: { } removals } death && death.HappyGuardRemovalIndex >= 0 && death.HappyGuardRemovalIndex < removals.Count && removals[death.HappyGuardRemovalIndex] is { } entry && entry.RemovedSeat == r.RemovedSeat && entry.GuardOwner == r.GuardOwnerSeat && entry.ResolutionId == r.RemovalId && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<PlayerMarkerChangedEvent>().Count(e => e.ResolutionId == r.RemovalId && e.PlayerSeat == r.RemovedSeat && e.SkillOwnerSeat == r.GuardOwnerSeat && e.Marker == PlayerMarkerKind.HappyGuard && e.Delta == -1 && e.Count == 0 && e.Reason == "program.attributed-marker.death-clear") == 1;
        return _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == r.ParentId)is { HappyGuard: { Stage: HappyGuardStage.RemovedChildren } guard } parent && parent.Id == r.RemovalId && parent.OwnerSeat == r.GuardOwnerSeat && guard.GuardedSeat == r.RemovedSeat && ValidHappyGuardFrame(parent);
    }

    [ProgramTriggerProof(ProgramTriggerProofPhase.NativeWindow, 50, SkillProgramTriggerWindow.HappyGuardRemoved)]
    internal bool IsHappyDianWeiElaiCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => c.SkillId == HappyDianWeiComposition.Elai && c.BindingId == "removed-guard" && context.Window == SkillProgramTriggerWindow.HappyGuardRemoved && context.OwnerSeat == c.OwnerSeat && _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is { Window: SkillProgramTriggerWindow.HappyGuardRemoved, Continuation: ProgramLifecycleContinuation.HappyGuardRemoved, HappyGuardRemoval: { } r } window && context.SourceSeat == r.RemovedSeat && context.TargetSeat == r.RemovedSeat && window.OwnerSeat == r.RemovedSeat && window.ResumeProgramFrameId == r.ParentId && window.CandidateIndex >= 0 && window.CandidateIndex < window.Candidates.Count && window.Candidates[window.CandidateIndex] == c && HappyGuardRemovalIsPaid(r) && _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat], c.SkillId, c.SkillInstanceId) && _contentRegistryOperations._contentRegistry.GetSkill(c.SkillId).Program?.GameplayHash == c.GameplayHash;
    internal bool IsHappyDianWeiElaiContext(ProgramSkillFrame f) => f.WindowContext is { } context && _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is { } window && window.CandidateIndex >= 0 && window.CandidateIndex < window.Candidates.Count && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, window.Candidates[window.CandidateIndex]) && IsHappyDianWeiElaiCandidate(window.Candidates[window.CandidateIndex], context);
    [ProgramWindowReturn(SkillProgramTriggerWindow.HappyGuardRemoved)]
    internal void ReturnHappyGuardElai(ProgramSkillFrame f, bool completed)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramLifecycleTriggerWindowFrame w || f.WindowContext?.ParentFrameId != w.Id || w.HappyGuardRemoval is not { } r || !HappyGuardRemovalIsPaid(r) || w.CandidateIndex < 0 || w.CandidateIndex >= w.Candidates.Count || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, w.Candidates[w.CandidateIndex]))
            throw new InvalidOperationException("Elai lost its original paid removed-guard window.");
        _programLifecycleWindowOperations.Get().AdvanceCandidate(w, true, completed);
        _coreRuntimeOperations.AdvanceRuntimeTop<ProgramLifecycleTriggerWindowFrame>();
    }

    [ProgramPipelineHook(ProgramPipelinePhase.LifecycleWindowCompleted, 41980)]
    internal bool CompleteHappyGuardRemovedWindow(ProgramLifecycleTriggerWindowFrame w)
    {
        if (w.HappyGuardRemoval is not { } r)
            return false;
        if (w.Window != SkillProgramTriggerWindow.HappyGuardRemoved || w.Continuation != ProgramLifecycleContinuation.HappyGuardRemoved || w.CandidateIndex != w.Candidates.Count || !HappyGuardRemovalIsPaid(r) || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != r.ParentId)
            throw new InvalidOperationException("Removed guard returned to a different native host or payment.");
        if (r.DeathParent)
        {
            var death = _coreDeathProgramsOperations.GetCurrentDeathFrame(r.ParentId);
            _coreDeathProgramsOperations.UpdateDeathFrame(death.Id, d => d with { HappyGuardRemovalIndex = d.HappyGuardRemovalIndex + 1 });
            if (!TryBeginHappyDianWeiDeathRemovals(_coreDeathProgramsOperations.GetCurrentDeathFrame(death.Id)))
                _coreDeathProgramsOperations.CompleteDeathResolution(death.Id);
        }
        else
            FinishHappyGuardTransfer(r.ParentId);
        return true;
    }

    internal void FinishHappyGuardTransfer(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var r = f.HappyGuard!;
        if (r.Stage != HappyGuardStage.RemovedChildren || !ValidHappyGuardFrame(f) || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not BeforeDamageProgramWindowFrame window || window.Id != r.ParentId || HappyGuardAttack(r.AttackId)is not { } attack || window.SourceSeat != r.DamageSource || window.TargetSeat != r.GuardedSeat || window.Amount != r.Amount || attack.SourceSeat != r.DamageSource || attack.TargetSeat != r.GuardedSeat || attack.DamageAmount != r.Amount)
            throw new InvalidOperationException("Paid guard lost its exact suspended original damage occurrence.");
        if (_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && r.GuardedSeat != f.OwnerSeat)
        {
            if (attack.DamageRedirected)
                throw new InvalidOperationException("The guard cannot replace a recipient after a different frozen redirection.");
            attack.RedirectFinalizedDamageTarget(f.OwnerSeat, new GameEngine.ProgramDamageTransferFollowup(f.SkillId, f.OwnerSeat, f.OwnerSeat, false));
            _coreRecipientScopedDamageOperations.Value.UpdateRecipientScopedDamageRedirect(attack);
            _coreRuntimeOperations.ReplaceRuntimeFrame(window.Id, window with { RedirectedTargetSeat = f.OwnerSeat });
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyGuardTransferredEvent(f.Id, r.AttackId, r.GuardedSeat, attack.TargetSeat, r.Amount));
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { HappyGuard = r with { Stage = HappyGuardStage.Transferred } });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 41980)]
    internal void CaptureHappyGuardDeathRemoval(PlayerMarkerChangedEvent fact)
    {
        if (fact.Marker != PlayerMarkerKind.HappyGuard || fact.Delta != -1 || fact.Reason != "program.attributed-marker.death-clear" || _resolutionStackOperations._resolutionStack.LastOrDefault()is not DeathFrame death || death.Id != fact.ResolutionId || fact.SkillOwnerSeat is not { } source)
            return;
        var list = death.HappyGuardRemovals ?? [];
        _coreDeathProgramsOperations.UpdateDeathFrame(death.Id, d => d with { HappyGuardRemovals = list.Append(new(list.Count, fact.PlayerSeat, source, fact.ResolutionId)).ToArray() });
    }

    internal bool TryBeginHappyDianWeiDeathRemovals(DeathFrame supplied)
    {
        var death = _coreDeathProgramsOperations.GetCurrentDeathFrame(supplied.Id);
        while (death.HappyGuardRemovals is { } list && death.HappyGuardRemovalIndex < list.Count)
        {
            var entry = list[death.HappyGuardRemovalIndex];
            if (BeginHappyGuardRemovedWindow(new(death.Id, entry.RemovedSeat, entry.GuardOwner, entry.ResolutionId, true)))
                return true;
            _coreDeathProgramsOperations.UpdateDeathFrame(death.Id, d => d with { HappyGuardRemovalIndex = d.HappyGuardRemovalIndex + 1 });
            death = _coreDeathProgramsOperations.GetCurrentDeathFrame(death.Id);
        }

        return false;
    }
}
