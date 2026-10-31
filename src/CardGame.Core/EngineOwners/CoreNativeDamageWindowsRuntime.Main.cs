#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeDamageWindowsRuntime
{
    internal bool TryBeginDamageTriggerWindow(GameEngine.IDamageAttempt attack, long damageFrameId, SkillProgramTriggerWindow window)
    {
        var source = _coreMatchParticipantsPlayersAccess._players[attack.SourceSeat];
        var triggerCandidates = DamageTriggerOrdering.Order(CollectDamageTriggerCandidates(attack, window), source.Seat, _coreMatchDefinitionPlayerCountAccess._playerCount, _coreTableOrderOperations.CurrentTableOrder());
        if (triggerCandidates.Count == 0)
            return false;
        BeginDamageTriggerWindow(attack, damageFrameId, window, triggerCandidates);
        // Expose an optional trigger in the command that caused the damage.
        // Automatic candidates remain one deterministic engine step each.
        if (triggerCandidates[0].IsOptional)
        {
            RunOneDamageTriggerStep();
        }

        return true;
    }

    internal void BeginDamageTriggerWindow(GameEngine.IDamageAttempt attack, long damageFrameId, SkillProgramTriggerWindow window, IReadOnlyList<DamageTriggerCandidate> candidates)
    {
        if (_coreDamageCursorRuntimeOperations.ActiveDamageTrigger is not null)
        {
            throw new InvalidOperationException("The engine cannot resolve two damage trigger windows at once.");
        }

        var frozenCandidates = Array.AsReadOnly(candidates.ToArray());
        var frameId = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        var damageWindow = new DamageTriggerWindowFrame(frameId, damageFrameId, attack.SourceSeat, attack.TargetSeat, attack.Card?.Id, attack.EffectiveCardKind, frozenCandidates, window)
        {
            EventTargetDamageInstancesTakenThisTurn = window == SkillProgramTriggerWindow.AfterDamageApplied && _contentRegistryOperations._contentRegistry.ProgramDependencies.UsesTriggerValue(SkillProgramTriggerValueKind.EventTargetDamageInstancesTakenThisTurn) ? _corePlayCardHistoryOperations.DamageInstancesTakenThisTurn(attack.TargetSeat) : null
        };
        _coreRuntimeOperations.PushRuntimeFrame(_characterBaoXinOlOperations.CaptureBaoXinYimouWindow(damageWindow, attack));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DamageTriggerWindowOpenedEvent(frameId, damageFrameId, attack.SourceSeat, attack.TargetSeat, attack.Card?.Id, attack.EffectiveCardKind, frozenCandidates));
    }

    internal void RunOneDamageTriggerStep()
    {
        var window = _coreDamageCursorRuntimeOperations.ActiveDamageTrigger ?? throw new InvalidOperationException("A damage trigger step has no pending window.");
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
        {
            return;
        }

        if (window.CandidateIndex >= window.Candidates.Count)
        {
            CompleteDamageTriggerWindow(window);
            return;
        }

        var candidate = window.Candidates[window.CandidateIndex];
        if (candidate.ToProgramCandidate()is { } programCandidate)
        {
            var programContext = _coreProgramLifecycleOperations.CreateAfterDamageProgramContext(window, programCandidate);
            if (!_coreProgramLifecycleOperations.CanRunProgramTrigger(programCandidate, programContext))
            {
                AdvanceDamageTriggerCandidate(window);
                return;
            }

            if (candidate.IsOptional)
            {
                _coreProgramLifecycleOperations.ExposeProgramTriggerDecision(programCandidate, programContext);
                return;
            }

            _coreProgramLifecycleOperations.BeginProgramBinding(programCandidate, programContext);
            return;
        }

        throw new InvalidOperationException("A damage trigger candidate has no configured program.");
    }

    internal void AdvanceDamageTriggerCandidate(DamageTriggerWindowFrame window) => _coreDamageAdjacencyServicesOperations.DamageAdjacency.AdvanceDamageTriggerCandidate(window);
    internal void CompleteDamageTriggerWindow(DamageTriggerWindowFrame window) => _coreDamageAdjacencyServicesOperations.DamageAdjacency.CompleteDamageTriggerWindow(window);
    internal IReadOnlyList<DamageTriggerCandidate> CollectDamageTriggerCandidates(GameEngine.IDamageAttempt attack, SkillProgramTriggerWindow window)
    {
        var candidates = new List<DamageTriggerCandidate>();
        foreach (var programOwnerSeat in _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive).Select(player => player.Seat))
            foreach (var candidate in _programPipelineHooksOperations._programPipelineHooks.IncludeTriggerCandidate(_coreMatchParticipantsPlayersAccess._players[programOwnerSeat], window, _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(_coreMatchParticipantsPlayersAccess._players[programOwnerSeat], window)))
            {
                var trigger = _contentRegistryOperations._contentRegistry!.GetSkill(candidate.SkillId).Program!.Triggers.Single(item => item.Id == candidate.BindingId);
                if (trigger.Effects.Any(e => e.Op is SkillProgramEffectOp.ResolveYouqueIssuedSlashDamageBenefit or SkillProgramEffectOp.RecoverMouLuSuMengmouDamage))
                    continue;
                var ownsTiming = trigger.Subject switch
                {
                    SkillProgramTriggerSubject.Owner => programOwnerSeat == attack.TargetSeat,
                    SkillProgramTriggerSubject.DamageTarget => window == SkillProgramTriggerWindow.DamageAppliedBeforeDying && trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.DrawByDamageCardColor) && programOwnerSeat == attack.TargetSeat,
                    SkillProgramTriggerSubject.Source => !attack.IsSourceLess && programOwnerSeat == attack.SourceSeat && MatchesAfterDamageProgramSource(attack, trigger),
                    SkillProgramTriggerSubject.DamageSource => !attack.IsSourceLess && programOwnerSeat == attack.SourceSeat,
                    SkillProgramTriggerSubject.Any => true,
                    _ => false
                };
                if (ProgramInstructionResolver.Default.Features(trigger).HasOperation(SkillProgramEffectOp.ApplyLiushiEffect) && (attack.ConversionSource is not { } liushiSource || liushiSource.OwnerSeat != candidate.OwnerSeat || liushiSource.SkillInstanceId != candidate.SkillInstanceId))
                    continue;
                if (!ownsTiming || !_characterBeiMiHuOlOperations.CanDrawAfterKuiDamage(programOwnerSeat, trigger, attack.TargetSeat) || !_characterSimaHuiOlOperations.CanOfferSimaHuiDamage(programOwnerSeat, trigger, attack) || !_characterZhaoXiangOlOperations.CanGainMeiYingFromDamage(programOwnerSeat, trigger, attack.SourceSeat, attack.TargetSeat, attack.IsSourceLess) || !_characterXiZhiCaiOlOperations.CanOfferXianfuDamage(programOwnerSeat, trigger, attack) || !_characterHuangQuanOlOperations.CanOfferDianhuDamage(programOwnerSeat, trigger, attack) || !_coreLastDamageSourceReciprocityOperations.CanOfferLastDamageSourceReciprocity(programOwnerSeat, candidate.SkillId, trigger, attack) || !_coreOrderedPrintedSkillLossOperations.CanOfferOrderedPrintedSkillLoss(programOwnerSeat, trigger, attack) || !_coreDamageCounterpartAcquisitionOperations.CanOfferDamageCounterpartAcquisition2(programOwnerSeat, trigger, attack))
                    continue;
                var occurrenceIndexes = trigger.DamageOccurrence switch
                {
                    null or SkillProgramDamageOccurrence.PerDamage => [0],
                    SkillProgramDamageOccurrence.PerDamagePoint => Enumerable.Range(0, Math.Max(0, attack.DamageAmount)).ToArray(),
                    _ => throw new InvalidOperationException("An after-damage trigger lost its occurrence policy.")};
                foreach (var occurrenceIndex in occurrenceIndexes)
                {
                    candidates.Add(new DamageTriggerCandidate(candidate.OwnerSeat, $"program:{(candidate.SkillId)}:{(candidate.BindingId)}:{(candidate.SkillInstanceId)}:occurrence-{(occurrenceIndex)}", candidate.Priority, IsOptional: trigger.Optional, ProgramId: candidate.SkillId, ProgramTriggerId: candidate.BindingId, SkillInstanceId: candidate.SkillInstanceId, GameplayHash: candidate.GameplayHash, OccurrenceIndex: occurrenceIndex));
                }
            }

        _characterZhangYanOlOperations.AppendZhangYanSujiDamageCandidates(attack, window, candidates);
        _characterGuanHaiOlOperations.AppendGuanHaiYouqueDamageCandidates(attack, window, candidates);
        _characterLuSuMouOperations.AppendMouLuSuIssuedDamageCandidates(attack, window, candidates);
        _characterKanZeAnniversaryOperations.AppendAnniversaryKanZeDamageCandidates(attack, window, candidates);
        _characterMangYaChangAnniversaryOperations.AppendAnniversaryJiedaoDamageCandidates(attack, window, candidates);
        _characterZhangFenAnniversaryOperations.AppendDagongCheIssuedDamageCandidates(attack, window, candidates);
        _characterHuLieMouOperations.AppendMouHuLieDamageCandidates(attack, window, candidates);
        _characterTaoQianOlOperations.AppendZhaohuoDamageCandidates(attack, window, candidates);
        return candidates;
    }

    internal static bool MatchesAfterDamageProgramSource(GameEngine.IDamageAttempt attack, SkillProgramTrigger trigger) => attack.ConversionSource is { } source && string.Equals(source.SkillId, trigger.SourceSkillId, StringComparison.Ordinal) && (trigger.SourceViewAsId is null || string.Equals(source.BindingId, trigger.SourceViewAsId, StringComparison.Ordinal));
}
