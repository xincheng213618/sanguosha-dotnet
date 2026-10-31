#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineSlashTargetPenaltyWindowsRuntime
{
    internal bool HasSlashTargetPenaltyPrograms => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.RequireTargetDiscardOrEquipmentRecast);

    internal bool MatchesSlashTargetPenaltyUse(SlashTargetPenaltyIdentity r)
    {
        if (r.FrozenDistance != 1 || r.Source.OwnerSeat != r.ActorSeat || string.IsNullOrWhiteSpace(r.Source.SkillId) || string.IsNullOrWhiteSpace(r.Source.BindingId) || string.IsNullOrWhiteSpace(r.Source.SkillInstanceId) || r.ActualTurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || r.ActualTurnOwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.TargetSeat) || _coreCardUseLifecycleOperations.LifecycleCardUse(r.CardUseFrameId)is not { } use || !CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || use.CardKind != r.EffectiveKind)
            return false;
        // Reuse the mature same-use actor-replacement and legacy producer proof.
        var target = r.TargetSeat;
        if (!use.TargetSeats.Contains(target))
        {
            if (use.TargetSeats.Count == 0 || !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<SlashTargetPenaltyOfferedEvent>().Any(e => e.Identity == r))
                return false;
            target = use.TargetSeats[0];
        }

        return _coreSlashTargetBenefitWindowsOperations.MatchesSlashTargetBenefitUse(new(use.Id, r.ActionId, r.ActorSeat, r.ProviderSeat, r.EffectiveKind, target, r.ActualTurnNumber, r.ActualTurnOwnerSeat, r.LegacyProducerProgramId));
    }

    internal bool TryBeginSlashTargetPenalties(NativeOwners.CardAttackHandle attack, bool legacy = false, bool afterActualTargets = false)
    {
        if (!HasSlashTargetPenaltyPrograms || _coreMatchOutcomeWinnerAccess._winner != Winner.None || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { } use || !CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || attack.IsSourceLess || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(use.SourceSeat) || !_coreMatchParticipantsPlayersAccess._players[use.SourceSeat].IsAlive)
            return false;
        var owner = _coreMatchParticipantsPlayersAccess._players[use.SourceSeat];
        var facts = use.Action is { } action ? _coreProgramLifecycleOperations.CaptureProgramTriggerFacts2(owner, action) : _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(owner);
        var candidates = _coreProgramLifecycleOperations.CollectEligibleProgramTriggerCandidates(owner, SkillProgramTriggerWindow.ActualSlashTargetPenalty, facts).Where(c => _coreProgramLifecycleOperations.GetProgramTrigger2(c).Effects is [{ Op: SkillProgramEffectOp.RequireTargetDiscardOrEquipmentRecast }]).ToArray();
        if (candidates.Length == 0)
            return false;
        var visited = (use.SlashTargetPenaltyVisits ?? []).ToList();
        var entries = new List<SlashTargetPenaltyEntry>();
        foreach (var target in use.TargetSeats.Distinct())
        {
            if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || target == owner.Seat || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive)
                continue;
            long? producer = null;
            if (use.Action is null)
                producer = _coreSlashTargetBenefitWindowsOperations.SlashBenefitLegacyProducer(use, target);
            else if (use.Action.Type != CardActionType.Use || use.Action.ActorSeat != use.SourceSeat || use.Action.EffectiveKind != use.CardKind)
                continue;
            if (use.Action is null && producer is null)
                continue;
            foreach (var c in candidates)
            {
                var source = new CardConversionSource(c.SkillId, c.BindingId, c.OwnerSeat, c.SkillInstanceId);
                if (visited.Any(v => v.ActorSeat == owner.Seat && v.TargetSeat == target && v.Source.SkillId == c.SkillId && v.Source.BindingId == c.BindingId))
                    continue;
                // Announced targets are visited even when their distance is not one.
                // Later HP/equipment changes cannot manufacture a second opportunity.
                visited.Add(new(owner.Seat, target, source, c.GameplayHash));
                var distance = _coreNativeRuleQueriesOperations.GetCombatDistance(owner.Seat, target);
                if (distance != 1)
                    continue;
                var identity = new SlashTargetPenaltyIdentity(use.Id, use.Action?.ActionId, owner.Seat, use.Action?.ProviderSeat ?? owner.Seat, use.CardKind, target, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, producer, source, c.GameplayHash, distance);
                entries.Add(new(c, identity));
            }
        }

        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { SlashTargetPenaltyVisits = visited });
        if (entries.Count == 0)
            return false;
        var id = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        for (var i = 0; i < entries.Count; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SlashTargetPenaltyOfferedEvent(id, i, entries[i].Identity));
        _coreRuntimeOperations.PushRuntimeFrame(new SlashTargetPenaltyWindowFrame(id, use.Id, afterActualTargets ? legacy ? SlashTargetPenaltyReturn.AfterActualTargetsLegacy : SlashTargetPenaltyReturn.AfterActualTargetsSlash : legacy ? SlashTargetPenaltyReturn.LegacySlash : SlashTargetPenaltyReturn.FinalizedSlash, entries));
        _coreRuntimeOperations.AdvanceRuntimeTop<SlashTargetPenaltyWindowFrame>();
        return true;
    }

    internal bool SlashTargetPenaltyWindowMatches(SlashTargetPenaltyWindowFrame w)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == w.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not CardUseFrame use || use.Id != w.ParentFrameId || !Enum.IsDefined(w.ReturnKind) || w.Entries.Count == 0 || w.CandidateIndex < 0 || w.CandidateIndex > w.Entries.Count)
            return false;
        for (var i = 0; i < w.Entries.Count; i++)
        {
            var e = w.Entries[i];
            var r = e.Identity;
            var c = e.Candidate;
            if (r.CardUseFrameId != use.Id || r.Source.SkillId != c.SkillId || r.Source.BindingId != c.BindingId || r.Source.OwnerSeat != c.OwnerSeat || r.Source.SkillInstanceId != c.SkillInstanceId || r.GameplayHash != c.GameplayHash || !MatchesSlashTargetPenaltyUse(r) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<SlashTargetPenaltyOfferedEvent>().Count(f => f.WindowId == w.Id && f.CandidateIndex == i && f.Identity == r) != 1 || !(use.SlashTargetPenaltyVisits ?? []).Any(v => v.ActorSeat == r.ActorSeat && v.TargetSeat == r.TargetSeat && v.Source == r.Source && v.GameplayHash == r.GameplayHash))
                return false;
        }

        return true;
    }

    internal bool SlashTargetPenaltyProgramMatches(ProgramSkillFrame f)
    {
        if (f.WindowContext is not { Window: SkillProgramTriggerWindow.ActualSlashTargetPenalty } context || _resolutionStackOperations._resolutionStack.OfType<SlashTargetPenaltyWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { } parent || !SlashTargetPenaltyWindowMatches(parent) || parent.CandidateIndex >= parent.Entries.Count)
            return false;
        var e = parent.Entries[parent.CandidateIndex];
        var r = e.Identity;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        return index > 0 && _resolutionStackOperations._resolutionStack[index - 1].Id == parent.Id && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, e.Candidate) && context.OwnerSeat == f.OwnerSeat && context.SourceSeat == r.ActorSeat && context.TargetSeat == r.TargetSeat && context.OccurrenceIndex == e.Candidate.OccurrenceIndex && context.SlashTargetPenalty == r && f.InstructionIndex is 0 or 1;
    }

    internal bool CanRunSlashTargetPenalty(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => context.SlashTargetPenalty is { } r && context.Window == SkillProgramTriggerWindow.ActualSlashTargetPenalty && r.Source.SkillId == c.SkillId && r.Source.BindingId == c.BindingId && r.Source.OwnerSeat == c.OwnerSeat && r.Source.SkillInstanceId == c.SkillInstanceId && r.GameplayHash == c.GameplayHash && MatchesSlashTargetPenaltyUse(r) && _coreMatchOutcomeWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive && _coreCardUseLifecycleOperations.LifecycleCardUse(r.CardUseFrameId)!.TargetSeats.Contains(r.TargetSeat) && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat], c.SkillId, c.SkillInstanceId);
    [NativeFrameAutomaticStep(NativeFrameAutomaticPhase.AfterOpening, 100, NativeFrameAutomaticPending.RequireNoneBeforeFrame, NativeFrameAutomaticPublish.AfterAdvance)]
    [NativeFrameRoute(typeof(SlashTargetPenaltyWindowFrame), 290)]
    internal void ContinueSlashTargetPenaltyWindow()
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is SlashTargetPenaltyWindowFrame w)
        {
            if (!SlashTargetPenaltyWindowMatches(w))
                throw new InvalidOperationException("The target penalty lost its original same-use window.");
            if (w.CandidateIndex >= w.Entries.Count || _coreMatchOutcomeWinnerAccess._winner != Winner.None)
            {
                _coreRuntimeOperations.PopResolutionFrame(w.Id, ResolutionFrameKind.SlashTargetPenaltyWindow);
                var attack = _coreActiveNativeAttackQueriesOperations.ActiveCardAttack;
                if (_coreMatchOutcomeWinnerAccess._winner != Winner.None)
                {
                    _coreNativeCardUseFramesOperations.SetCardUseStep(attack.ResolutionId, ResolutionFrameStep.ResolvingEffect);
                    _coreNativeAttackCompletionOperations.CompleteAttack(attack);
                }
                else if (w.ReturnKind is SlashTargetPenaltyReturn.AfterActualTargetsSlash or SlashTargetPenaltyReturn.AfterActualTargetsLegacy)
                    _coreSlashTargetBenefitWindowsOperations.ContinueSlashAfterTargetBenefitAdditions(attack, w.ReturnKind == SlashTargetPenaltyReturn.AfterActualTargetsLegacy);
                else if (w.ReturnKind == SlashTargetPenaltyReturn.LegacySlash)
                {
                    if (TryBeginSlashTargetPenalties(attack, true) || _coreSlashTargetBenefitWindowsOperations.TryBeginSlashTargetBenefits(attack, true) || _coreActualUseTargetProgramsOperations.TryBeginActualUseTargetPrograms(attack, ActualUseTargetReturnKind.LegacyVirtualSlash))
                        return;
                    _coreNativeSlashUseOperations.ContinueSlashAfterResponsePrograms(attack);
                }
                else
                    _coreNativeSlashUseOperations.ContinueSlashAfterFinalizedTargets(attack);
                return;
            }

            var e = w.Entries[w.CandidateIndex];
            var r = e.Identity;
            var context = new ProgramSkillWindowContext(SkillProgramTriggerWindow.ActualSlashTargetPenalty, w.Id, e.Candidate.OwnerSeat, SourceSeat: r.ActorSeat, TargetSeat: r.TargetSeat, OccurrenceIndex: e.Candidate.OccurrenceIndex)
            {
                SlashTargetPenalty = r
            };
            if (!CanRunSlashTargetPenalty(e.Candidate, context))
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(w with { CandidateIndex = w.CandidateIndex + 1 });
                continue;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(w with { Step = ResolutionFrameStep.AwaitingResponse });
            _coreProgramLifecycleOperations.BeginProgramBinding(e.Candidate, context);
            return;
        }
    }

    internal void CompleteSlashTargetPenaltyBinding(ProgramSkillFrame f)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not SlashTargetPenaltyWindowFrame w || w.Id != f.WindowContext?.ParentFrameId || w.CandidateIndex >= w.Entries.Count || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, w.Entries[w.CandidateIndex].Candidate))
            throw new InvalidOperationException("The target penalty binding lost its exact parent candidate.");
        _coreRuntimeOperations.ReplaceRuntimeTop(w with { CandidateIndex = w.CandidateIndex + 1, Step = ResolutionFrameStep.ResolvingEffect });
        _coreRuntimeOperations.AdvanceRuntimeTop<SlashTargetPenaltyWindowFrame>();
    }

    [ProgramIssuedTrigger(710, ProgramIssuedTriggerMatch.Decision)]
    internal bool? DecideIssuedTrigger710(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => context.Window == SkillProgramTriggerWindow.ActualSlashTargetPenalty ? CanRunSlashTargetPenalty(candidate, context) : null;
}
