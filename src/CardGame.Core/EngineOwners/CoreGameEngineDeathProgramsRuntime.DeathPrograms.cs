#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDeathProgramsRuntime
{
    internal void ChangeProgramAttributedMarker(ProgramSkillFrame frame, ProgramParticipantReference targetReference, PlayerMarkerKind marker, int amount)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (targetReference.Kind == ProgramParticipantRef.EventSource)
        {
            if (active.WindowContext is not { Window: SkillProgramTriggerWindow.DamageAppliedBeforeDying or SkillProgramTriggerWindow.AfterDamageApplied } || amount is < 1 or > 20)
                throw new InvalidOperationException("Attributed marker mutation requires an after-damage event source.");
        }
        else if (targetReference.Kind == ProgramParticipantRef.Owner)
        {
            if (active.WindowContext is not { Window: SkillProgramTriggerWindow.GameStarting or SkillProgramTriggerWindow.AfterDamageApplied or SkillProgramTriggerWindow.CardsMoved } || amount is < 1 or > 20)
                throw new InvalidOperationException("Owner-attributed marker mutation requires an after-damage or card-movement trigger.");
        }
        else
        {
            throw new InvalidOperationException("Attributed marker mutation requires an owner or event source.");
        }

        var targetSeat = _coreProgramCardPaymentsOperations.ResolveProgramParticipant(active, targetReference);
        var isHappyWuhunSelfDamage = targetReference.Kind == ProgramParticipantRef.EventSource && active.SkillId == HappyGuanYuComposition.Wuhun && active.TriggerId == "damage-nightmare" && marker == PlayerMarkerKind.Nightmare && amount == 1 && active.WindowContext is { Window: SkillProgramTriggerWindow.DamageAppliedBeforeDying } damage && damage.SourceSeat == active.OwnerSeat && damage.TargetSeat == active.OwnerSeat;
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(targetSeat) || targetSeat == active.OwnerSeat && targetReference.Kind != ProgramParticipantRef.Owner && !isHappyWuhunSelfDamage)
            return;
        var target = _coreMatchParticipantsPlayersAccess._players[targetSeat];
        var sourceKey = (marker, active.OwnerSeat);
        target.MarkerSourceCounts[sourceKey] = target.MarkerSourceCounts.GetValueOrDefault(sourceKey) + amount;
        var count = target.Markers.GetValueOrDefault(marker) + amount;
        target.Markers[marker] = count;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlayerMarkerChangedEvent(active.Id, targetSeat, marker, Delta: amount, Count: count, SkillOwnerSeat: active.OwnerSeat, Reason: $"skill-program.{(active.SkillId)}.{(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active))}.marker"));
    }

    internal SkillProgramStepOutcome CauseProgramDeathUnlessBoundCardKind(ProgramSkillFrame frame, string sourceBind, IReadOnlyList<CardKind> excludedCardKinds)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (active.WindowContext is not { Window: SkillProgramTriggerWindow.OwnerDied } || active.SelectedTargetSeats is not [var targetSeat])
            throw new InvalidOperationException("Configured direct death requires an owner-death selected target.");
        var binding = _coreProgramLifecycleOperations.GetProgramCardSet(active, sourceBind);
        if (binding.CardIds.Count == 0)
            return SkillProgramStepOutcome.Continue;
        if (binding.CardIds.Count != 1 || binding.SourceLocations.Count != 1)
            throw new InvalidOperationException("Configured direct death requires exactly one bound judgment card.");
        var location = binding.SourceLocations[0];
        if (_cardZonesOperations._cardZones.GetLocation(binding.CardIds[0]) != location)
            throw new InvalidOperationException("The bound judgment card left its frozen location.");
        var card = _cardZonesOperations._cardZones.CardsAt(location).Single(item => item.Id == binding.CardIds[0]);
        _corePhysicalMovementFacadesOperations.MoveCard(card, location, CardLocation.DiscardPile, new CardMoveReason($"skill-program.{(active.SkillId)}.{(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active))}.judgment-finish"));
        var target = _coreMatchParticipantsPlayersAccess._players[targetSeat];
        if (excludedCardKinds.Contains(card.Kind) || !target.IsAlive || _coreMatchOutcomeWinnerWinnerFactionIdWinnerTeamIdAccess._winner != Winner.None)
            return SkillProgramStepOutcome.Continue;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSkillCauseDeathDeclaredEvent(active.Id, active.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), active.OwnerSeat, targetSeat));
        _coreMatchLogOperations.AddLog("DirectDeath", $"{(target.Name)} 的判定结果不满足【{(_contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId).Name)}】的存活条件，其直接死亡。", active.OwnerSeat, targetSeat);
        _coreNativePlayerDeathOperations.BeginPlayerDeath(active.Id, target, killer: null, attack: null, dying: null, causingProgramSkillFrameId: active.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal DeathFrame GetCurrentDeathFrame(long expectedId)
    {
        var death = _resolutionStackOperations._resolutionStack.OfType<DeathFrame>().LastOrDefault();
        if (death is null || death.Id != expectedId)
            throw new InvalidOperationException("The continued death resolution is not current.");
        var index = _resolutionStackOperations._resolutionStack.FindLastIndex(frame => frame.Id == death.Id);
        if (index <= 0 || _resolutionStackOperations._resolutionStack[index - 1].Id != death.ParentFrameId || (death.ReturnKind switch
        {
            DeathReturnKind.Dying => _resolutionStackOperations._resolutionStack[index - 1] is not DyingFrame,
            DeathReturnKind.ProgramSkill => _resolutionStackOperations._resolutionStack[index - 1] is not ProgramSkillFrame,
            DeathReturnKind.CardDamageReplacement => !_coreCardDamageMaximumHpReplacementOperations.IsExactMaximumHpReplacementDeathParent(death, _resolutionStackOperations._resolutionStack[index - 1]),
            _ => true
        }))
            throw new InvalidOperationException("The death frame lost its typed return parent.");
        return death;
    }

    internal void UpdateDeathFrame(long frameId, Func<DeathFrame, DeathFrame> update)
    {
        var death = GetCurrentDeathFrame(frameId);
        var index = _resolutionStackOperations._resolutionStack.FindLastIndex(frame => frame.Id == death.Id);
        _coreRuntimeOperations.ReplaceRuntimeFrame(_resolutionStackOperations._resolutionStack[index].Id, update(death));
    }

    internal bool TryBeginOwnerDiedProgramWindow(DeathFrame death, CharacterState owner)
    {
        if (_coreMatchOutcomeWinnerWinnerFactionIdWinnerTeamIdAccess._winner != Winner.None || owner.IsAlive)
            return false;
        var facts = _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(owner);
        var candidates = _characterSimaShiMouOperations.IncludeMouSimaShiDeathCandidates(owner, _coreProgramLifecycleOperations.CollectEligibleProgramTriggerCandidates(owner, SkillProgramTriggerWindow.OwnerDied, facts));
        if (candidates.Count == 0)
            return false;
        var frame = new ProgramDeathTriggerWindowFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, death.Id, owner.Seat, death.KillerSeat, candidates, facts);
        _coreRuntimeOperations.PushRuntimeFrame(frame);
        _coreRuntimeOperations.AdvanceRuntimeTop<ProgramDeathTriggerWindowFrame>();
        return true;
    }

    internal ProgramSkillWindowContext CreateOwnerDiedProgramContext(ProgramDeathTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => new(SkillProgramTriggerWindow.OwnerDied, frame.Id, frame.OwnerSeat, SourceSeat: frame.KillerSeat, TargetSeat: frame.OwnerSeat, OccurrenceIndex: candidate.OccurrenceIndex, Facts: frame.Facts);
    [NativeFrameRoute(typeof(ProgramDeathTriggerWindowFrame), 420)]
    internal void ContinueOwnerDiedProgramWindowCore()
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramDeathTriggerWindowFrame frame)
        {
            var death = GetCurrentDeathFrame(frame.DeathFrameId);
            if (death.VictimSeat != frame.OwnerSeat)
                throw new InvalidOperationException("The owner-death program window lost its death parent.");
            if (frame.CandidateIndex >= frame.Candidates.Count)
            {
                _coreRuntimeOperations.PopResolutionFrame(frame.Id, ResolutionFrameKind.ProgramDeathTriggerWindow);
                UpdateDeathFrame(death.Id, current => current with { OwnerDiedProgramsResolved = true });
                ContinueDeathResolution(death.Id);
                return;
            }

            var candidate = frame.Candidates[frame.CandidateIndex];
            var context = CreateOwnerDiedProgramContext(frame, candidate);
            if (!_coreProgramLifecycleOperations.CanRunProgramTrigger(candidate, context))
            {
                AdvanceOwnerDiedProgramCandidate(frame, candidate, activated: false, completed: false);
                continue;
            }

            var trigger = _coreProgramLifecycleOperations.GetProgramTrigger2(candidate);
            if (trigger.Optional)
            {
                _coreProgramLifecycleOperations.ExposeProgramTriggerDecision(candidate, context);
                return;
            }

            _coreProgramLifecycleOperations.BeginProgramBinding(candidate, context);
            return;
        }
    }

    internal void AdvanceOwnerDiedProgramCandidate(ProgramDeathTriggerWindowFrame frame, ProgramTriggerCandidate candidate, bool activated, bool completed)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramDeathTriggerWindowFrame current || current.Id != frame.Id || current.Candidates[current.CandidateIndex] != candidate)
            throw new InvalidOperationException("The owner-death program cursor changed before it advanced.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramBindingResolvedEvent(current.Id, candidate.SkillId, candidate.BindingId, candidate.SkillInstanceId, candidate.OwnerSeat, SkillProgramTriggerWindow.OwnerDied, activated, completed));
        AdvanceOwnerDiedProgramCursor(current);
    }

    internal void AdvanceOwnerDiedProgramCursor(ProgramDeathTriggerWindowFrame frame) => _programWindowCursorOperations.Get().AdvanceOwnerDiedProgramCursor(frame);

    internal bool TryBeginKillDiedProgramWindow(DeathFrame death)
    {
        if (_coreMatchOutcomeWinnerWinnerFactionIdWinnerTeamIdAccess._winner != Winner.None)
            return false;
        var collected = new List<(ProgramTriggerCandidate Candidate, SkillProgramTriggerFacts Facts)>();
        foreach (var player in _coreMatchParticipantsPlayersAccess._players.Where(item => item.IsAlive))
        {
            var facts = _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(player)with
            {
                DeathKillerIsOwner = death.KillerSeat == player.Seat,
                DeathVictimCleanupCardCount = death.CleanedUpCardIds.Count,
                DeathExtinguishedFaction = _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(_coreMatchParticipantsPlayersAccess._players[death.VictimSeat])is { } faction && !_coreMatchParticipantsPlayersAccess._players.Any(other => other.IsAlive && _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(other) == faction)
            };
            foreach (var candidate in _coreProgramLifecycleOperations.CollectEligibleProgramTriggerCandidates(player, SkillProgramTriggerWindow.CharacterDied, facts))
                if (!_coreProgramLifecycleOperations.GetProgramTrigger2(candidate).Effects.Any(effect => effect.Op == SkillProgramEffectOp.ResolveAnniversaryYuanYinYunjiu))
                    collected.Add((candidate, facts));
        }

        if (collected.Count == 0)
            return false;
        var ordered = collected.OrderBy(item => _coreTableOrderOperations.RelativeTableOrder(_coreActualTurnProgressCurrentSeatPhaseAccess._currentSeat, item.Candidate.OwnerSeat)).ThenByDescending(item => item.Candidate.Priority).ThenBy(item => item.Candidate.SkillId, StringComparer.Ordinal).ThenBy(item => item.Candidate.BindingId, StringComparer.Ordinal).ThenBy(item => item.Candidate.SkillInstanceId, StringComparer.Ordinal).ThenBy(item => item.Candidate.OccurrenceIndex).ToArray();
        var frameId = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        var frame = new ProgramKillTriggerWindowFrame(frameId, death.Id, death.VictimSeat, death.KillerSeat, ordered.Select(item => item.Candidate).ToArray(), ordered.Select(item => new ProgramSkillWindowContext(SkillProgramTriggerWindow.CharacterDied, frameId, item.Candidate.OwnerSeat, SourceSeat: death.KillerSeat, TargetSeat: death.VictimSeat, OccurrenceIndex: item.Candidate.OccurrenceIndex, Facts: item.Facts)).ToArray());
        _coreRuntimeOperations.PushRuntimeFrame(frame);
        _coreRuntimeOperations.AdvanceRuntimeTop<ProgramKillTriggerWindowFrame>();
        return true;
    }

    [NativeFrameRoute(typeof(ProgramKillTriggerWindowFrame), 430)]
    internal void ContinueKillDiedProgramWindowCore()
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramKillTriggerWindowFrame frame)
        {
            var death = GetCurrentDeathFrame(frame.DeathFrameId);
            if (frame.CandidateIndex >= frame.Candidates.Count)
            {
                if (frame.BeforeDeathCleanup)
                {
                    _characterYuanYinAnniversaryOperations.CompleteAnniversaryYuanYinDeathBeforeCleanupWindow(frame, death);
                    _coreRuntimeOperations.PopResolutionFrame(frame.Id, ResolutionFrameKind.ProgramDeathTriggerWindow);
                    ContinuePlayerDeathCleanup(death.Id);
                    return;
                }

                _coreRuntimeOperations.PopResolutionFrame(frame.Id, ResolutionFrameKind.ProgramDeathTriggerWindow);
                UpdateDeathFrame(death.Id, current => current with { KillerProgramsResolved = true });
                ContinueDeathResolution(death.Id);
                return;
            }

            var candidate = frame.Candidates[frame.CandidateIndex];
            var context = frame.Contexts[frame.CandidateIndex];
            if (!_coreProgramLifecycleOperations.CanRunProgramTrigger(candidate, context))
            {
                AdvanceKillDiedProgramCandidate(frame, candidate, activated: false, completed: false);
                continue;
            }

            var trigger = _coreProgramLifecycleOperations.GetProgramTrigger2(candidate);
            if (trigger.Optional)
            {
                _coreProgramLifecycleOperations.ExposeProgramTriggerDecision(candidate, context);
                return;
            }

            _coreProgramLifecycleOperations.BeginProgramBinding(candidate, context);
            return;
        }
    }

    internal void AdvanceKillDiedProgramCandidate(ProgramKillTriggerWindowFrame frame, ProgramTriggerCandidate candidate, bool activated, bool completed)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramKillTriggerWindowFrame current || current.Id != frame.Id || current.Candidates[current.CandidateIndex] != candidate)
            throw new InvalidOperationException("The killer-death program cursor changed before it advanced.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramBindingResolvedEvent(current.Id, candidate.SkillId, candidate.BindingId, candidate.SkillInstanceId, candidate.OwnerSeat, SkillProgramTriggerWindow.CharacterDied, activated, completed));
        _coreRuntimeOperations.ReplaceRuntimeTop(current with { CandidateIndex = current.CandidateIndex + 1 });
    }

    internal void ContinueDeathOutcome(long deathFrameId)
    {
        var death = GetCurrentDeathFrame(deathFrameId);
        var victim = _coreMatchParticipantsPlayersAccess._players[death.VictimSeat];
        var killer = death.KillerSeat is { } killerSeat ? _coreMatchParticipantsPlayersAccess._players[killerSeat] : null;
        if (killer is { } killerState && killerState.Seat == _coreActualTurnProgressCurrentSeatPhaseAccess._currentSeat && _coreActualTurnProgressCurrentSeatPhaseAccess._phase == TurnPhase.Play)
            _coreCurrentTurnStatisticsPlayPhaseKillCountByCurrentPlayerAccess._playPhaseKillCountByCurrentPlayer++;
        _characterLuMengMouOperations.ResetHengyeAfterKill(killer);
        if (_modeRulesOperations._modeRules.Death.NotifyAiOnDeath)
        {
            _coreNativeAiDecisionsOperations.NotifyAiOfDeath(killer, victim);
        }

        if (_modeRulesOperations._modeRules.Death.UsesIdentityRewards && killer is { IsAlive: true })
        {
            if (victim.Role == Role.Rebel)
            {
                _corePhysicalDrawFacadesOperations.DrawCards(killer, 3, log: true);
                _coreMatchLogOperations.AddLog("KillReward", $"{(killer.Name)} 击杀反贼，摸三张牌。", killer.Seat, victim.Seat);
            }
            else if (killer.Role == Role.Lord && victim.Role == Role.Loyalist)
            {
                var penalty = _corePhysicalOwnedCardQueriesOperations.GetHand(killer).ToArray();
                _coreNativeLogicalDiscardsOperations.MoveLogicalDiscardCards(killer.Seat, killer.Seat, penalty, CardLocation.Hand(killer.Seat), CardMoveReasons.LordPenalty);
                _coreMatchLogOperations.AddLog("LordPenalty", $"主公误杀忠臣，弃置全部 {(penalty.Length)} 张手牌。", killer.Seat, victim.Seat);
            }
        }

        var previousWinner = _coreMatchOutcomeWinnerWinnerFactionIdWinnerTeamIdAccess._winner;
        _coreModeRulesOperations.ApplyModeDeathOutcome(deathFrameId, victim, killer);
        if (_coreMatchOutcomeWinnerWinnerFactionIdWinnerTeamIdAccess._winner != previousWinner && _coreMatchOutcomeWinnerWinnerFactionIdWinnerTeamIdAccess._winner != Winner.None)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WinnerDeterminedEvent(_coreMatchOutcomeWinnerWinnerFactionIdWinnerTeamIdAccess._winner, _coreNativeDamageHandlesOperations.IsTeamMode ? _coreMatchOutcomeWinnerWinnerFactionIdWinnerTeamIdAccess._winnerTeamId : null, _coreNativeDamageHandlesOperations.IsNationalWarMode ? _coreMatchOutcomeWinnerWinnerFactionIdWinnerTeamIdAccess._winnerFactionId : null));
        }

        ContinueDeathResolution(deathFrameId);
    }

    internal void ContinueDeathResolution(long deathFrameId)
    {
        var death = GetCurrentDeathFrame(deathFrameId);
        if (death.ShenPangTongOutcomeDeferred && death.OwnerDiedProgramsResolved)
        {
            _characterPangTongAnniversaryGodOperations.CompleteShenPangTongKunyuDeath(death);
            ContinueDeathOutcome(deathFrameId);
            return;
        }

        var owner = _coreMatchParticipantsPlayersAccess._players[death.VictimSeat];
        if (!death.OwnerDiedProgramsResolved && TryBeginOwnerDiedProgramWindow(death, owner))
            return;
        if (!death.OwnerDiedProgramsResolved)
        {
            UpdateDeathFrame(death.Id, current => current with { OwnerDiedProgramsResolved = true });
            death = GetCurrentDeathFrame(death.Id);
        }

        if (!death.KillerProgramsResolved && TryBeginKillDiedProgramWindow(death))
            return;
        if (!death.KillerProgramsResolved)
            UpdateDeathFrame(death.Id, current => current with { KillerProgramsResolved = true });
        CompleteDeathResolution(death.Id);
    }

    internal void CompleteDeathResolution(long deathFrameId)
    {
        var death = GetCurrentDeathFrame(deathFrameId);
        // Death-trigger skills remain available through their original native
        // windows. End the interrupted phase only after those children return.
        if (!_coreMatchParticipantsPlayersAccess._players[death.VictimSeat].IsAlive && death.VictimSeat == _coreActualTurnProgressCurrentSeatPhaseAccess._currentSeat && _coreActualTurnProgressCurrentSeatPhaseAccess._phase == TurnPhase.Play)
            _corePlayPhaseSkillGrantsOperations.CloseCurrentPlayPhaseSkillGrants("phase-actor-died");
        if (!death.HappyGuardMarkerCleanupCompleted)
        {
            ClearAttributedMarkerSources(_coreMatchParticipantsPlayersAccess._players[death.VictimSeat], death.Id);
            UpdateDeathFrame(death.Id, current => current with { HappyGuardMarkerCleanupCompleted = true });
            death = GetCurrentDeathFrame(death.Id);
        }

        if (_characterDianWeiHappyGodOperations.TryBeginHappyDianWeiDeathRemovals(death))
            return;
        _coreRuntimeOperations.PopResolutionFrame(death.Id, ResolutionFrameKind.Death);
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != death.ParentFrameId)
            throw new InvalidOperationException("A death resolution lost its return parent.");
        if (death.ReturnKind == DeathReturnKind.Dying)
        {
            if (_resolutionStackOperations._resolutionStack[^1] is not DyingFrame || _coreDyingCursorRuntimeOperations.ActiveDying is not { } dying || dying.FrameId != death.ParentFrameId)
                throw new InvalidOperationException("A death resolution lost its dying parent.");
            _coreNativeDyingOperations.CompleteDyingAfterDeath(dying, survived: _coreMatchParticipantsPlayersAccess._players[death.VictimSeat].IsAlive);
            return;
        }

        if (death.ReturnKind == DeathReturnKind.ProgramSkill)
        {
            if (_resolutionStackOperations._resolutionStack[^1] is not ProgramSkillFrame)
                throw new InvalidOperationException("A death resolution lost its program parent.");
            _coreRuntimeOperations.AdvanceRuntimeProgram(death.ParentFrameId);
            return;
        }

        if (death.ReturnKind == DeathReturnKind.CardDamageReplacement)
        {
            if (_coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not { } attack || attack.ResolutionId != death.ParentFrameId || !_coreCardDamageMaximumHpReplacementOperations.IsExactMaximumHpReplacementDeathParent(death, _resolutionStackOperations._resolutionStack[^1]))
                throw new InvalidOperationException("Maximum-HP death lost its exact card attack return.");
            if (_characterDaQiaoXiaoQiaoHappyGodOperations.TryBeginHappyQiaoCardMaximumHealth(attack.ResolutionId))
                return;
            _coreNativeAttackCompletionOperations.CompleteAttack(attack);
            return;
        }

        throw new InvalidOperationException("A death resolution has no continuation.");
    }

    internal int GetMarkerSourceCount(CharacterState player, PlayerMarkerKind marker, int skillOwnerSeat) => player.MarkerSourceCounts.GetValueOrDefault((marker, skillOwnerSeat));
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 86200, SkillProgramEffectOp.ClaimDeathCleanupCards)]
    internal bool HasInitialFilteredDeathClaimResources(ProgramTriggerLegalityContext eligibility)
    {
        // An explicit initial source filter is also its resource prerequisite.
        // Legacy unfiltered claims and later conditional branches keep their admission.
        if (eligibility.Plan.Instructions.FirstOrDefault()is not { Op: SkillProgramEffectOp.ClaimDeathCleanupCards, Condition.Kind: SkillProgramConditionKind.Always } effect || effect.Zones.Count == 0)
            return true;
        return FilteredDeathClaimParent(eligibility.OwnerSeat, eligibility.Context)is { } death && QueryProgramDeathCleanupCards(death, effect.Zones).Length > 0;
    }

    internal DeathFrame? FilteredDeathClaimParent(int ownerSeat, ProgramSkillWindowContext context)
    {
        if (context.Window != SkillProgramTriggerWindow.CharacterDied || context.OwnerSeat != ownerSeat || _resolutionStackOperations._resolutionStack.OfType<ProgramKillTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { } window || window.BeforeDeathCleanup || context.TargetSeat != window.VictimSeat || context.SourceSeat != window.KillerSeat)
            return null;
        var windowIndex = _resolutionStackOperations._resolutionStack.FindLastIndex(f => f.Id == window.Id);
        var deathIndex = _resolutionStackOperations._resolutionStack.FindLastIndex(f => f.Id == window.DeathFrameId);
        return deathIndex >= 0 && deathIndex < windowIndex && _resolutionStackOperations._resolutionStack[deathIndex] is DeathFrame death && death.VictimSeat == window.VictimSeat && death.KillerSeat == window.KillerSeat && death.VictimSeat != ownerSeat ? death : null;
    }

    internal void ClaimProgramDeathCleanupCards(ProgramSkillFrame frame)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (active.OwnerSeat != frame.OwnerSeat || active.SkillId != frame.SkillId)
            throw new InvalidOperationException("A death-claim requires the active program frame.");
        var context = active.WindowContext ?? throw new InvalidOperationException("A death-claim requires a trigger window context.");
        if (context.Window is not SkillProgramTriggerWindow.CharacterDied)
            throw new InvalidOperationException("A death-claim requires a characterDied window.");
        var window = _resolutionStackOperations._resolutionStack.OfType<ProgramKillTriggerWindowFrame>().LastOrDefault(item => item.Id == context.ParentFrameId) ?? throw new InvalidOperationException("A death-claim lost its killer-death window.");
        var windowIndex = _resolutionStackOperations._resolutionStack.FindLastIndex(item => item.Id == window.Id);
        var deathIndex = _resolutionStackOperations._resolutionStack.FindLastIndex(item => item.Id == window.DeathFrameId);
        if (deathIndex < 0 || deathIndex >= windowIndex || _resolutionStackOperations._resolutionStack[deathIndex] is not DeathFrame death)
            throw new InvalidOperationException("A death-claim lost its death parent.");
        if (frame.OwnerSeat == death.VictimSeat)
            throw new InvalidOperationException("A death-claim cannot target its own death.");
        var paused = ProgramInstructionResolver.Default.Resolve(active, _contentRegistryOperations._contentRegistry.GetSkill(active.SkillId).Program!).GetPausedInstruction(active.InstructionIndex).Effect;
        var claimable = QueryProgramDeathCleanupCards(death, paused.Zones);
        if (claimable.Length == 0)
            return;
        var reason = new CardMoveReason($"skill-program.{(frame.SkillId)}.{(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active))}.claim-death-cleanup");
        _corePhysicalMovementFacadesOperations.MoveCards(claimable, CardLocation.DiscardPile, CardLocation.Hand(frame.OwnerSeat), reason);
    }

    internal Card[] QueryProgramDeathCleanupCards(DeathFrame death, IReadOnlyList<CardZoneKind> sourceZones) => death.CleanedUpCardIds.Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.DiscardPile && (sourceZones.Count == 0 || _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault(m => m.CardId == id)is { } movement && movement.From.OwnerSeat == death.VictimSeat && sourceZones.Contains(movement.From.Zone) && movement.To == CardLocation.DiscardPile && (movement.From.Zone == CardZoneKind.Equipment ? movement.Reason == CardMoveReasons.DeathEquipmentDiscard : movement.From.Zone is CardZoneKind.Hand or CardZoneKind.Judgment && movement.Reason == CardMoveReasons.DeathDiscard))).Select(id => _cardZonesOperations._cardZones.CardsAt(CardLocation.DiscardPile).Single(c => c.Id == id)).ToArray();
    internal void ClearAttributedMarkerSources(CharacterState owner, long resolutionId)
    {
        foreach (var player in _coreMatchParticipantsPlayersAccess._players)
        {
            var attributed = player.MarkerSourceCounts.Where(item => item.Key.SkillOwnerSeat == owner.Seat && item.Key.Marker is not (PlayerMarkerKind.Kui or PlayerMarkerKind.DragonSeal or PlayerMarkerKind.PhoenixSeal or PlayerMarkerKind.Pingding or PlayerMarkerKind.ShencaiChi or PlayerMarkerKind.ShencaiZhang or PlayerMarkerKind.ShencaiTu or PlayerMarkerKind.ShencaiLiu or PlayerMarkerKind.ShencaiSi or PlayerMarkerKind.HappyXue)) // Issued Liushi hand-limit effects belong to the affected role;
            // their original source's death does not withdraw them.
            .Where(item => item.Key.Marker != PlayerMarkerKind.LiuShi || !_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ApplyLiushiEffect)).ToArray();
            foreach (var item in attributed)
            {
                var independent = _coreHpDamageShieldsOperations.IndependentMarkerCount(player, item.Key.Marker, owner.Seat) + _coreLeastHandMarkerDrawProgramsOperations.IndependentIssuedDrawMarkerCount(player, item.Key.Marker, owner.Seat) + _coreRecipientCategoryMarksOperations.IndependentRecipientCategoryMarkerCount(player, item.Key.Marker, owner.Seat);
                var removed = item.Value - independent;
                if (removed < 0)
                    throw new InvalidOperationException("Independent shields exceed their public attributed markers.");
                if (removed == 0)
                    continue;
                if (independent == 0)
                    player.MarkerSourceCounts.Remove(item.Key);
                else
                    player.MarkerSourceCounts[item.Key] = independent;
                var total = player.Markers.GetValueOrDefault(item.Key.Marker);
                if (total < removed)
                    throw new InvalidOperationException("An attributed marker source exceeds its public total.");
                var remaining = total - removed;
                if (remaining == 0)
                    player.Markers.Remove(item.Key.Marker);
                else
                    player.Markers[item.Key.Marker] = remaining;
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlayerMarkerChangedEvent(resolutionId, player.Seat, item.Key.Marker, Delta: -removed, Count: remaining, SkillOwnerSeat: owner.Seat, Reason: "program.attributed-marker.death-clear"));
            }
        }
    }

    internal void ContinuePlayerDeathCleanup(long deathFrameId)
    {
        var death = GetCurrentDeathFrame(deathFrameId);
        var victim = _coreMatchParticipantsPlayersAccess._players[death.VictimSeat];
        var killer = death.KillerSeat is { } killerSeat ? _coreMatchParticipantsPlayersAccess._players[killerSeat] : null;
        try
        {
            var deathCleanupCardIds = new List<int>();
            void MoveDiedCards(IEnumerable<Card> cards, CardLocation from, CardMoveReason reason)
            {
                var materialized = cards.ToArray();
                deathCleanupCardIds.AddRange(materialized.Select(item => item.Id));
                _corePhysicalMovementFacadesOperations.MoveCards(materialized, from, CardLocation.DiscardPile, reason);
            }

            MoveDiedCards(_corePhysicalOwnedCardQueriesOperations.GetHand(victim), CardLocation.Hand(victim.Seat), CardMoveReasons.DeathDiscard);
            MoveDiedCards(_corePhysicalOwnedCardQueriesOperations.GetEquipment(victim), CardLocation.Equipment(victim.Seat), CardMoveReasons.DeathEquipmentDiscard);
            MoveDiedCards(_corePhysicalOwnedCardQueriesOperations.GetJudgment(victim), CardLocation.Judgment(victim.Seat), CardMoveReasons.DeathDiscard);
            MoveDiedCards(_corePhysicalOwnedCardQueriesOperations.GetBuquWounds(victim), CardLocation.BuquWound(victim.Seat), CardMoveReasons.BuquDeathDiscard);
            MoveDiedCards(_corePhysicalOwnedCardQueriesOperations.GetAuthority(victim), CardLocation.Authority(victim.Seat), CardMoveReasons.AuthorityDeathDiscard);
            MoveDiedCards(_cardZonesOperations._cardZones.CardsAt(CardLocation.Chunlao(victim.Seat)), CardLocation.Chunlao(victim.Seat), CardMoveReasons.ChunlaoDeathDiscard);
            foreach (var curse in _coreGameEngineSourceCursesSourceCursesAccess._sourceCurses.Where(c => c.TargetSeat == victim.Seat).ToArray())
                MoveDiedCards(_cardZonesOperations._cardZones.CardsAt(curse.Location), curse.Location, new("program.source-curse.target-death"));
            var publicPileSources = _corePublicPileSourcesOperations.PublicPileSources(victim.Seat).ToArray();
            if (publicPileSources.Length == 0)
                MoveDiedCards(_cardZonesOperations._cardZones.CardsAt(global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileSourcesRuntime.PublicPileLocation(victim.Seat)), global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileSourcesRuntime.PublicPileLocation(victim.Seat), new("skill-program.public-pile.owner-death"));
            else
                foreach (var source in publicPileSources)
                    MoveDiedCards(_corePublicPileSourcesOperations.PublicPileCards(source), source.Location, new("skill-program.public-pile.owner-death"));
            MoveDiedCards(_cardZonesOperations._cardZones.CardsAt(new CardLocation(CardZoneKind.PublicDeferredPile, victim.Seat)), new CardLocation(CardZoneKind.PublicDeferredPile, victim.Seat), new CardMoveReason("skill-program.deferred-pile.owner-death"));
            _coreGameEngineDeferredCardsDeferredPublicPileDepositsAccess._deferredPublicPileDeposits.RemoveAll(item => item.OwnerSeat == victim.Seat);
            foreach (var location in _cardZonesOperations._cardZones.PrivateTurnHoldLocations.Where(l => l.OwnerSeat == victim.Seat || l.PrivateTurnHold?.DeferredOffer?.TargetSeat == victim.Seat))
                MoveDiedCards(_cardZonesOperations._cardZones.CardsAt(location), location, new("skill.private-turn-hold.death"));
            MoveDiedCards(_cardZonesOperations._cardZones.CardsAt(CardLocation.RoundHold(victim.Seat)), CardLocation.RoundHold(victim.Seat), new("skill-program.hanguo.death-discard"));
            MoveDiedCards(_cardZonesOperations._cardZones.CardsAt(CardLocation.PojunHold(victim.Seat)), CardLocation.PojunHold(victim.Seat), CardMoveReasons.PojunHoldDeathDiscard);
            MoveDiedCards(_cardZonesOperations._cardZones.CardsAt(new CardLocation(CardZoneKind.PrivateReserve, victim.Seat)), new CardLocation(CardZoneKind.PrivateReserve, victim.Seat), new CardMoveReason("program.private-reserve.death"));
            deathCleanupCardIds.AddRange(_characterGuoTuOlOperations.DiscardQushiHolderDeathSeals(_resolutionStackOperations._resolutionStack.OfType<DeathFrame>().Single(frame => frame.Id == deathFrameId)));
            deathCleanupCardIds.AddRange(_characterCaoZhenMobileShiOperations.DiscardShiCaoZHolderDeathPiles(_resolutionStackOperations._resolutionStack.OfType<DeathFrame>().Single(frame => frame.Id == deathFrameId)));
            deathCleanupCardIds.AddRange(_characterHuangQuanMobileShiOperations.DiscardShiQuanHolderDeathPiles(_resolutionStackOperations._resolutionStack.OfType<DeathFrame>().Single(frame => frame.Id == deathFrameId)));
            deathCleanupCardIds.AddRange(_characterZhouFeiBoundaryMobileOperations.DiscardBoundaryZhouFeiHolderDeathPiles(_resolutionStackOperations._resolutionStack.OfType<DeathFrame>().Single(frame => frame.Id == deathFrameId)));
            deathCleanupCardIds.AddRange(_characterSunQuanAnniversaryGodOperations.DiscardAnniversarySunQuanMingOnDeath(_resolutionStackOperations._resolutionStack.OfType<DeathFrame>().Single(frame => frame.Id == deathFrameId)));
            UpdateDeathFrame(deathFrameId, frame => frame with { CleanedUpCardIds = deathCleanupCardIds });
            _coreMatchLogOperations.AddLog("PlayerDied", _coreNativeDamageHandlesOperations.IsNationalWarMode ? $"{(victim.Name)} 阵亡，属于{(global::CardGame.Core.EngineOwners.CoreMatchPresentationRuntime.GetFactionName(victim.NationalFactionId!))}势力。" : $"{(victim.Name)} 阵亡，身份是【{(_coreMatchPresentationOperations.GetRoleName(victim.Role))}】。", killer?.Seat, victim.Seat);
            _coreConvertingGiftsOperations.MarkConvertingGiftExactDeath(deathFrameId, victim.Seat);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlayerDiedEvent(victim.Seat, killer?.Seat));
            if (_characterPangTongAnniversaryGodOperations.TryBeginShenPangTongKunyuDeath(GetCurrentDeathFrame(deathFrameId)))
                return;
            ContinueDeathOutcome(deathFrameId);
        }
        catch
        {
            if (_resolutionStackOperations._resolutionStack.LastOrDefault()is DeathFrame { Id: var currentId } && currentId == deathFrameId)
                _coreRuntimeOperations.PopResolutionFrame(deathFrameId, ResolutionFrameKind.Death);
            throw;
        }
    }
}
