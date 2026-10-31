#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangXunAnniversaryRuntime
{
    internal bool HasAnniversaryZhangXunCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ArmAnniversaryZhangXunSuizheng);

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 45200)]
    internal void ObserveAnniversaryZhangXunLease(ZhangXunSuizhengLeaseEvent fact) => _characterZhangXunAnniversaryHasIssuedAnniversaryZhangXunAccess._hasIssuedAnniversaryZhangXun = true;
    internal bool ValidZhangXunLease(ZhangXunSuizhengLeaseEvent lease, IReadOnlyList<IGameEvent> history)
    {
        if (lease.FrameId <= 0 || lease.EndingWindowId <= 0 || lease.IssuedTurn < 1 || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(lease.RecipientSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(lease.Source.OwnerSeat) || lease.Source.SkillId != AnniversaryZhangXunComposition.Skill || lease.Source.BindingId != AnniversaryZhangXunComposition.Arm || !_contentRegistryOperations._contentRegistry.Skills.TryGetValue(lease.Source.SkillId, out var skill) || skill.Program?.GameplayHash != lease.Hash || history.OfType<ZhangXunSuizhengLeaseEvent>().Count(e => e.FrameId == lease.FrameId) != 1)
            return false;
        return history.OfType<ProgramBindingStartedEvent>().Count(e => e.FrameId == lease.FrameId && e.OwnerSeat == lease.Source.OwnerSeat && e.SkillId == lease.Source.SkillId && e.BindingId == lease.Source.BindingId && e.SkillInstanceId == lease.Source.SkillInstanceId && e.Window == SkillProgramTriggerWindow.TurnEnding) == 1;
    }

    internal IReadOnlyList<GameEngine.ZhangXunDueLease> ActiveZhangXunLeases()
    {
        if (!HasAnniversaryZhangXunCapability || !_characterZhangXunAnniversaryHasIssuedAnniversaryZhangXunAccess._hasIssuedAnniversaryZhangXun)
            return[];
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var result = new List<GameEngine.ZhangXunDueLease>();
        var ordinal = 0;
        for (var i = 0; i < history.Length; i++)
        {
            if (history[i] is not ZhangXunSuizhengLeaseEvent lease)
                continue;
            var index = ordinal++;
            if (!ValidZhangXunLease(lease, history) || !_coreMatchParticipantsPlayersAccess._players[lease.RecipientSeat].IsAlive || history.Skip(i + 1).OfType<PlayerDiedEvent>().Any(e => e.VictimSeat == lease.RecipientSeat))
                continue;
            var start = Array.FindIndex(history, i + 1, e => e is TurnStartedEvent t && t.ActorSeat == lease.RecipientSeat && t.TurnNumber > lease.IssuedTurn);
            if (start < 0 || history[start] is not TurnStartedEvent due || due.TurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber || due.ActorSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || history.Skip(start + 1).OfType<TurnEndedEvent>().Any(t => t.TurnNumber == due.TurnNumber && t.ActorSeat == due.ActorSeat))
                continue;
            result.Add(new(lease, index, start, due.TurnNumber));
        }

        return Array.AsReadOnly(result.ToArray());
    }

    internal bool HasZhangXunSlashBenefit(int seat) => HasAnniversaryZhangXunCapability && _characterZhangXunAnniversaryHasIssuedAnniversaryZhangXunAccess._hasIssuedAnniversaryZhangXun && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat == seat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat == seat && ActiveZhangXunLeases().Any(d => d.Lease.RecipientSeat == seat);
    internal IEnumerable<RuleQueryContribution> AnniversaryZhangXunSlashContributions(CharacterState player, SkillRuleQuery query)
    {
        if (query != SkillRuleQuery.SlashLimit || !HasAnniversaryZhangXunCapability || !_characterZhangXunAnniversaryHasIssuedAnniversaryZhangXunAccess._hasIssuedAnniversaryZhangXun || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase != TurnPhase.Play || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat != player.Seat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat != player.Seat)
            return[];
        return ActiveZhangXunLeases().Where(d => d.Lease.RecipientSeat == player.Seat).Select(d => (RuleQueryContribution)new FiniteRuleQueryContribution($"anniversary:suizheng:{(d.Lease.FrameId)}", SkillRuleOperation.Add, 1)).ToArray();
    }

    [ProgramDistanceRule(ProgramDistanceStage.CardUseExemption, 45200)]
    internal bool ZhangXunCardDistance(ProgramCardDistanceQuery q) => CoreNativeCardClassificationsRuntime.IsSlashCard(q.Kind) && HasZhangXunSlashBenefit(q.Actor.Seat);
    [ProgramDistanceRule(ProgramDistanceStage.PhysicalSlash, 45200)]
    [ProgramDistanceRule(ProgramDistanceStage.VirtualSlash, 45200)]
    [ProgramDistanceRule(ProgramDistanceStage.ProvidedSlash, 45200)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedSlash, 45200)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedPhysicalSlash, 45200)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedPaidWeaponSlash, 45200)]
    [ProgramDistanceRule(ProgramDistanceStage.SpecificSlashExemption, 45200)]
    internal bool ZhangXunSlashDistance(ProgramSlashDistanceQuery q) => CoreNativeCardClassificationsRuntime.IsSlashCard(q.Kind) && HasZhangXunSlashBenefit(q.Actor.Seat);
    internal bool LegalZhangXunFollowupTarget(int owner, int target) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(owner) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(target) && _coreMatchParticipantsPlayersAccess._players[owner].IsAlive && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && owner != target && !_coreCardUseModulesOperations.IsCardUseForbidden(owner, CardKind.Slash, CardActionType.Use) && _coreNativeTargetLegalityOperations.CanUseVirtualSlashTarget(_coreMatchParticipantsPlayersAccess._players[owner], _coreMatchParticipantsPlayersAccess._players[target], CardKind.Slash, ignoreDistance: true, countsTowardLimit: false) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(owner, target, CardKind.Slash) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[target], CardKind.Slash, Suit.None);
    internal IReadOnlyList<ZhangXunSuizhengDamageWitness> ZhangXunVictims(GameEngine.ZhangXunDueLease due, int cutoff)
    {
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        if (cutoff < due.StartIndex || cutoff > history.Length)
            throw new InvalidOperationException("Suizheng's damage cutoff must belong to its original due actual turn.");
        return Array.AsReadOnly(history.Take(cutoff).Select((fact, index) => (fact, index)).Where(x => x.index > due.StartIndex && x.fact is DamageAppliedEvent { Amount: > 0, SourceLess: false } damage && damage.SourceSeat == due.Lease.RecipientSeat && LegalZhangXunFollowupTarget(due.Lease.Source.OwnerSeat, damage.TargetSeat)).Select(x => new ZhangXunSuizhengDamageWitness(x.index, ((DamageAppliedEvent)x.fact).TargetSeat)).DistinctBy(x => x.TargetSeat).ToArray());
    }

    internal bool ZhangXunPhaseUsed(long leaseId, int turn, int phase) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ZhangXunSuizhengFollowupStartedEvent>().Any(e => e.LeaseId == leaseId && e.ActualTurn == turn && e.PhaseInstance == phase) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ZhangXunSuizhengPhaseClosedEvent>().Any(e => e.LeaseId == leaseId && e.ActualTurn == turn && e.PhaseInstance == phase);
    internal IEnumerable<ProgramTriggerCandidate> IssuedAnniversaryZhangXunPlayEndingCandidates(CharacterState phaseActor)
    {
        if (!HasAnniversaryZhangXunCapability || !_characterZhangXunAnniversaryHasIssuedAnniversaryZhangXunAccess._hasIssuedAnniversaryZhangXun || !phaseActor.IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase != TurnPhase.Play || phaseActor.Seat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat || phaseActor.Seat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat)
            yield break;
        foreach (var due in ActiveZhangXunLeases().Where(d => d.Lease.RecipientSeat == phaseActor.Seat && _coreMatchParticipantsPlayersAccess._players[d.Lease.Source.OwnerSeat].IsAlive))
        {
            if (ZhangXunPhaseUsed(due.Lease.FrameId, due.DueTurn, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId) || ZhangXunVictims(due, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().Count()).Count == 0)
                continue;
            var trigger = _contentRegistryOperations._contentRegistry.GetSkill(due.Lease.Source.SkillId).Program!.Triggers.Single(t => t.Id == AnniversaryZhangXunComposition.Followup);
            yield return new(due.Lease.Source.OwnerSeat, due.Lease.Source.SkillId, trigger.Id, due.Lease.Source.SkillInstanceId, due.Lease.Hash, trigger.Priority, due.Ordinal);
        }
    }

    internal GameEngine.ZhangXunDueLease? ExactZhangXunCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext w)
    {
        if (!HasAnniversaryZhangXunCapability || !_characterZhangXunAnniversaryHasIssuedAnniversaryZhangXunAccess._hasIssuedAnniversaryZhangXun || c.SkillId != AnniversaryZhangXunComposition.Skill || c.BindingId != AnniversaryZhangXunComposition.Followup || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(c.OwnerSeat) || !_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive || w.Window != SkillProgramTriggerWindow.PlayEnding || w.OwnerSeat != c.OwnerSeat || w.SourceSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat || w.OccurrenceIndex != c.OccurrenceIndex || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase != TurnPhase.Play || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(f => f.Id == w.ParentFrameId)is not { Window: SkillProgramTriggerWindow.PlayEnding } parent || parent.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || parent.Candidates[parent.CandidateIndex] != c)
            return null;
        return ActiveZhangXunLeases().SingleOrDefault(d => d.Ordinal == c.OccurrenceIndex && d.Lease.Source.OwnerSeat == c.OwnerSeat && d.Lease.Source.SkillId == c.SkillId && d.Lease.Source.SkillInstanceId == c.SkillInstanceId && d.Lease.Hash == c.GameplayHash && d.Lease.RecipientSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat);
    }

    [ProgramIssuedTrigger(400)]
    internal bool IsIssuedAnniversaryZhangXunCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext w)
    {
        var due = ExactZhangXunCandidate(c, w);
        if (due is null || _coreMatchOutcomeWinnerAccess._winner != Winner.None)
            return false;
        if (ZhangXunPhaseUsed(due.Lease.FrameId, due.DueTurn, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId))
            return _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(f => f.ZhangXunSuizheng is { } r && r.LeaseId == due.Lease.FrameId && r.ParentWindowId == w.ParentFrameId && r.ActualTurn == due.DueTurn && r.PhaseInstance == _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, c));
        return ZhangXunVictims(due, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().Count()).Count > 0;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 45200, SkillProgramEffectOp.ChooseAnniversaryZhangXunSuizhengSlash)]
    internal bool CanOfferZhangXunFollowup(ProgramTriggerCandidate c, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => IsIssuedAnniversaryZhangXunCandidate(c, context);
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 45201)]
    internal void CloseZhangXunPhase(ProgramBindingResolvedEvent fact)
    {
        if (!HasAnniversaryZhangXunCapability || !_characterZhangXunAnniversaryHasIssuedAnniversaryZhangXunAccess._hasIssuedAnniversaryZhangXun || fact.SkillId != AnniversaryZhangXunComposition.Skill || fact.BindingId != AnniversaryZhangXunComposition.Followup || fact.Window != SkillProgramTriggerWindow.PlayEnding || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.PlayEnding } parent || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count)
            return;
        var c = parent.Candidates[parent.CandidateIndex];
        if (c.OwnerSeat != fact.OwnerSeat || c.SkillId != fact.SkillId || c.BindingId != fact.BindingId || c.SkillInstanceId != fact.SkillInstanceId)
            return;
        var due = ActiveZhangXunLeases().SingleOrDefault(d => d.Ordinal == c.OccurrenceIndex && d.Lease.Source.OwnerSeat == c.OwnerSeat && d.Lease.Source.SkillInstanceId == c.SkillInstanceId && d.Lease.Hash == c.GameplayHash && d.Lease.RecipientSeat == parent.OwnerSeat);
        if (due is null || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ZhangXunSuizhengPhaseClosedEvent>().Any(e => e.LeaseId == due.Lease.FrameId && e.WindowId == parent.Id))
            return;
        if (fact.Activated)
        {
            // Completion pops the original Program child before publishing its
            // own ID; refusal instead publishes the still-active parent ID.
            if (fact.FrameId == parent.Id || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ZhangXunSuizhengFollowupStartedEvent>().Where(e => e.FrameId == fact.FrameId).ToArray()is not [var started] || started.ParentWindowId != parent.Id || started.LeaseId != due.Lease.FrameId || started.ActualTurn != due.DueTurn || started.RecipientSeat != parent.OwnerSeat || started.PhaseInstance != _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ProgramBindingStartedEvent>().Count(e => e.FrameId == fact.FrameId && e.OwnerSeat == c.OwnerSeat && e.SkillId == c.SkillId && e.BindingId == c.BindingId && e.SkillInstanceId == c.SkillInstanceId && e.Window == SkillProgramTriggerWindow.PlayEnding) != 1)
                return;
        }
        else if (fact.FrameId != parent.Id)
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhangXunSuizhengPhaseClosedEvent(due.Lease.FrameId, parent.Id, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, fact.Activated));
    }
}
