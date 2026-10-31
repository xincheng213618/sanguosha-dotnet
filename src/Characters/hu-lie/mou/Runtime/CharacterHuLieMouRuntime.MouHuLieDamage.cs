#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuLieMouRuntime
{
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 37500)]
    internal void ObserveMouHuLieYitouDamage(IGameEvent payload)
    {
        if (!HasMouHuLie || payload is not DamageAppliedEvent { Amount: > 0, SourceLess: false } applied || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { } attack || attack.IsSourceLess || _resolutionStackOperations._resolutionStack.LastOrDefault()is not DamageFrame d || d.ParentFrameId != attack.ResolutionId || d.SourceSeat != applied.SourceSeat || d.TargetSeat != applied.TargetSeat || d.Amount != applied.Amount)
            return;
        foreach (var lease in _coreProgramEventHistoryOperations.ProgramEventHistory<MouHuLieYitouIssuedEvent>().Where(e => e.Turn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && e.Recipient == applied.SourceSeat && _contentRegistryOperations._contentRegistry.GetSkill(e.Source.SkillId).Program?.GameplayHash == e.GameplayHash))
            if (!_coreProgramEventHistoryOperations.ProgramEventHistory<MouHuLieDamageEvent>().Any(e => e.DamageId == d.Id && e.LeaseId == lease.FrameId))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouHuLieDamageEvent(d.Id, lease.FrameId, lease.Source.OwnerSeat, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, applied.SourceSeat, applied.TargetSeat, applied.Amount));
    }

    internal void AppendMouHuLieDamageCandidates(GameEngine.IDamageAttempt attack, SkillProgramTriggerWindow window, List<DamageTriggerCandidate> candidates)
    {
        if (!HasMouHuLie || window != SkillProgramTriggerWindow.AfterDamageApplied || attack.IsSourceLess)
            return;
        foreach (var fact in _coreProgramEventHistoryOperations.ProgramEventHistory<MouHuLieDamageEvent>().Where(e => e.Turn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && e.SourceSeat == attack.SourceSeat && e.TargetSeat == attack.TargetSeat && _coreMatchParticipantsPlayersAccess._players[e.Owner].IsAlive))
        {
            if (!_resolutionStackOperations._resolutionStack.OfType<DamageFrame>().Any(d => d.Id == fact.DamageId && d.ParentFrameId == attack.ResolutionId))
                continue;
            var lease = _coreProgramEventHistoryOperations.ProgramEventHistory<MouHuLieYitouIssuedEvent>().Single(e => e.FrameId == fact.LeaseId);
            var trigger = _contentRegistryOperations._contentRegistry.GetSkill(MouHuLieRuntime.Yitou).Program!.Triggers.Single(t => t.Id == "issued-recipient-damage");
            candidates.RemoveAll(c => c.OwnerSeat == fact.Owner && c.ProgramId == MouHuLieRuntime.Yitou && c.ProgramTriggerId == trigger.Id && c.SkillInstanceId == lease.Source.SkillInstanceId);
            candidates.Add(new(fact.Owner, $"program:{(MouHuLieRuntime.Yitou)}:{(trigger.Id)}:{(lease.Source.SkillInstanceId)}:occurrence-0", trigger.Priority, false, MouHuLieRuntime.Yitou, trigger.Id, lease.Source.SkillInstanceId, lease.GameplayHash, 0));
        }
    }

    internal MouHuLieDamageEvent? MouHuLieDamageReceipt(ProgramTriggerCandidate c, ProgramSkillWindowContext w)
    {
        if (c.SkillId != MouHuLieRuntime.Yitou || c.BindingId != "issued-recipient-damage" || w.Window != SkillProgramTriggerWindow.AfterDamageApplied || w.OwnerSeat != c.OwnerSeat || w.DamageFrameId is not { } damage || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == w.ParentFrameId)is not DamageTriggerWindowFrame window || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || window.Candidates[window.CandidateIndex].ToProgramCandidate() != c || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == window.ParentFrameId)is not DamageFrame actual || actual.Id != damage || actual.SourceSeat != w.SourceSeat || actual.TargetSeat != w.TargetSeat || actual.Amount != w.Amount)
            return null;
        var fact = _coreProgramEventHistoryOperations.ProgramEventHistory<MouHuLieDamageEvent>().LastOrDefault(e => e.DamageId == damage && e.Owner == c.OwnerSeat && e.Turn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && e.SourceSeat == w.SourceSeat && e.TargetSeat == w.TargetSeat && _coreProgramEventHistoryOperations.ProgramEventHistory<MouHuLieYitouIssuedEvent>().Any(l => l.FrameId == e.LeaseId && l.Source.SkillInstanceId == c.SkillInstanceId && l.GameplayHash == c.GameplayHash));
        return fact is not null && !_coreProgramEventHistoryOperations.ProgramEventHistory<MouHuLieStartedEvent>().Any(s => s.Operation == SkillProgramEffectOp.DrawMouHuLieYitou && s.DamageId == damage && s.Source.OwnerSeat == c.OwnerSeat && s.ParentId != w.ParentFrameId) ? fact : null;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.IssuedTriggerCandidate, 37500)]
    internal bool IssuedMouHuLieDamage(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => MouHuLieDamageReceipt(c, w)is not null || c.SkillId == MouHuLieRuntime.Chuanyu && c.BindingId == "round-clear-and-slashes" && CurrentMouHuLieYu(c.OwnerSeat, c.SkillInstanceId)is { } yu && yu.GameplayHash == c.GameplayHash && _coreRoundEndingOperations.IsExactRoundEndingCandidate(c, w);
}
