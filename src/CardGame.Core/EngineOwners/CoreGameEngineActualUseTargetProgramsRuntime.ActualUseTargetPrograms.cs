#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineActualUseTargetProgramsRuntime
{
    internal bool TracksPaidOwnTargets => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.PayHpThenNullifyOwnActualUseTarget) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.OfferHalfHandRecipientSupport) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.DiscardDrawAndOfferUniqueHpPeer) || _coreSameTypeActualUseAidOperations.HasSameTypeActualUseAid || _coreOwnTrickDrawTargetsOperations.HasOwnMultiTargetTrickDraw || _coreShortRangeSlashTargetsOperations.HasShortRangeSlashCapability;

    internal ActualUseTargetIdentity? FreezeActualUseTarget(CardUseFrame use, int target)
    {
        if (!TracksPaidOwnTargets || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || target == use.SourceSeat && !_coreHalfHandTargetSupportOperations.HasHalfHandSelfTargetSupport(target) && !_coreOwnTrickDrawTargetsOperations.HasOwnMultiTargetTrickSelfTarget(use, target) || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || !use.TargetSeats.Contains(target) && !_coreHalfHandTargetSupportOperations.IsHalfHandImplicitSelfTarget(use, target) || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) && !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(use.CardKind))
            return null;
        if (use.CardKind == CardKind.BorrowedSword && !use.TargetSeats.Where((_, i) => i % 2 == 0).Contains(target))
            return null;
        if (use.Action is { Type: CardActionType.Use } action && action.ActorSeat == use.SourceSeat && action.EffectiveKind == use.CardKind)
            return new(use.Id, action.ActionId, action.ActorSeat, action.ProviderSeat, action.EffectiveKind, target, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, null);
        return use.Action is null && _coreDamageJudgmentSuitPaymentsOperations.LegacyDamageJudgmentVirtualProducer(use, target)is { } producer ? new(use.Id, null, use.SourceSeat, use.SourceSeat, use.CardKind, target, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, producer) : null;
    }

    internal bool MatchesActualUseTarget(ActualUseTargetIdentity identity)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(identity.CardUseFrameId)is not { } use || use.SourceSeat != identity.ActorSeat || use.CardKind != identity.EffectiveKind || !use.TargetSeats.Contains(identity.TargetSeat) && !_coreHalfHandTargetSupportOperations.IsHalfHandImplicitSelfTarget(use, identity.TargetSeat) || identity.ActualTurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || identity.ActualTurnOwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat)
            return false;
        if (identity.ActionId is { } actionId)
            return identity.LegacyProducerProgramId is null && use.Action is { Type: CardActionType.Use } action && action.ActionId == actionId && action.ActorSeat == identity.ActorSeat && action.ProviderSeat == identity.ProviderSeat && action.EffectiveKind == identity.EffectiveKind;
        return identity.LegacyProducerProgramId is { } producer && use.Action is null && (_coreDamageJudgmentSuitPaymentsOperations.LegacyDamageJudgmentVirtualProducer(use, identity.TargetSeat) == producer || _coreUniqueHpTargetAnnouncementsOperations.MatchesAnnouncedUniqueHpLegacyTarget(identity));
    }

    internal bool TryBeginActualUseTargetPrograms(global::CardGame.Core.NativeOwners.CardAttackHandle attack, ActualUseTargetReturnKind kind)
    {
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId);
        return use is not null && StartActualUseTargetWindow(use, [attack.TargetSeat], kind, null);
    }

    internal bool TryBeginActualUseTargetPrograms(global::CardGame.Core.NativeOwners.JizhiResolution pending)
    {
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(pending.ResolutionId);
        return use is not null && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(use.CardKind) && StartActualUseTargetWindow(use, use.CardKind == CardKind.BorrowedSword ? pending.TargetSeats.Take(1).ToArray() : _coreHalfHandTargetSupportOperations.HalfHandActualTrickTargets(use, pending.TargetSeats), ActualUseTargetReturnKind.OrdinaryTrick, new(pending.Card.Id, pending.ActionKind, pending.TargetCardId, pending.RequiredCardKind));
    }

    internal bool StartActualUseTargetWindow(CardUseFrame use, IReadOnlyList<int> targets, ActualUseTargetReturnKind kind, ActualUseTargetTrickReturn? trick)
    {
        if (!TracksPaidOwnTargets || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            return false;
        var id = _coreResolutionIdentityBoundaryOperations._resolutionSequence + 1;
        var entries = new List<(ProgramTriggerCandidate Candidate, ProgramSkillWindowContext Context)>();
        foreach (var target in targets.Distinct())
        {
            var identity = FreezeActualUseTarget(use, target);
            if (identity is null || _coreProgramCardTargetEffectsOperations.IsCardEffectIneffective(use.Id, target))
                continue;
            var facts = use.Action is { } action ? _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[target], action) : _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[target]);
            foreach (var candidate in _coreProgramLifecycleOperations.CollectEligibleProgramTriggerCandidates(_coreMatchParticipantsPlayersAccess._players[target], SkillProgramTriggerWindow.OtherActualUseTargeted, facts))
            {
                if (!_coreProgramLifecycleOperations.GetProgramTrigger(candidate).Effects.Any(e => e.Op is SkillProgramEffectOp.PayHpThenNullifyOwnActualUseTarget or SkillProgramEffectOp.OfferHalfHandRecipientSupport or SkillProgramEffectOp.DrawThenNullifyOwnMultiTargetTrick || e.Op == SkillProgramEffectOp.OfferSameTypeDifferentNameOrExtraTarget && identity.ActionId is null))
                    continue;
                entries.Add((candidate, new(SkillProgramTriggerWindow.OtherActualUseTargeted, id, target, SourceSeat: identity.ActorSeat, TargetSeat: target, OccurrenceIndex: candidate.OccurrenceIndex, Facts: facts) { ActualUseTarget = identity }));
            }
        }

        _coreShortRangeSlashTargetsOperations.AppendShortRangeLegacyCandidate(use, id, entries);
        var previouslyAnnounced = use.UniqueHpAnnouncedTargets?.Count ?? 0;
        _coreUniqueHpTargetAnnouncementsOperations.AppendUnannouncedUniqueHpTargets(use, id, entries);
        if (entries.Count == 0 && (_coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)?.UniqueHpAnnouncedTargets?.Count ?? 0) == previouslyAnnounced)
            return false;
        entries = entries.OrderByDescending(e => e.Candidate.Priority).ThenBy(e => _coreTableOrderOperations.RelativeTableOrder(0, e.Candidate.OwnerSeat)).ThenBy(e => e.Candidate.SkillId, StringComparer.Ordinal).ToList();
        _coreResolutionIdentityBoundaryOperations._resolutionSequence = id;
        _coreRuntimeOperations.PushRuntimeFrame(new ActualUseTargetWindowFrame(id, use.Id, kind, Array.AsReadOnly(entries.Select(e => e.Candidate).ToArray()), Array.AsReadOnly(entries.Select(e => e.Context).ToArray()), trick));
        _coreRuntimeOperations.AdvanceRuntimeTop<ActualUseTargetWindowFrame>();
        return true;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 790)]
    [ProgramTriggerProof(ProgramTriggerProofPhase.NativeWindow, 20, SkillProgramTriggerWindow.OtherActualUseTargeted)]
    internal bool CanRunActualUseTarget(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (context.Window != SkillProgramTriggerWindow.OtherActualUseTargeted)
            return true;
        if (_coreShortRangeSlashTargetsOperations.IsShortRangeLegacyCandidate(candidate, context))
            return true;
        return context.ActualUseTarget is { } use && use.TargetSeat == candidate.OwnerSeat && context.TargetSeat == use.TargetSeat && context.SourceSeat == use.ActorSeat && (candidate.OwnerSeat != use.ActorSeat || _coreHalfHandTargetSupportOperations.IsHalfHandSupportCandidate(candidate) || _coreOwnTrickDrawTargetsOperations.OwnMultiTargetTrickCandidate(candidate)) && _coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].Hp > 0 && MatchesActualUseTarget(use) && !_coreProgramCardTargetEffectsOperations.IsCardEffectIneffective(use.CardUseFrameId, use.TargetSeat) && _resolutionStackOperations._resolutionStack.OfType<ActualUseTargetWindowFrame>().LastOrDefault()is { } parent && parent.Id == context.ParentFrameId && parent.ParentFrameId == use.CardUseFrameId && parent.CandidateIndex >= 0 && parent.CandidateIndex < parent.Candidates.Count && parent.Candidates[parent.CandidateIndex] == candidate && parent.Contexts[parent.CandidateIndex].ActualUseTarget == use;
    }

    [NativeFrameAutomaticStep(NativeFrameAutomaticPhase.AfterOpening, 120, NativeFrameAutomaticPending.RequireNoneBeforeFrame, NativeFrameAutomaticPublish.AfterAdvance)]
    [NativeFrameRoute(typeof(ActualUseTargetWindowFrame), 230)]
    internal void ContinueActualUseTargetWindowCore()
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is ActualUseTargetWindowFrame parent)
        {
            parent = _coreUniqueHpTargetAnnouncementsOperations.AppendUniqueHpTargetsToCurrentWindow(parent);
            if (parent.CandidateIndex >= parent.Candidates.Count || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                _coreRuntimeOperations.PopResolutionFrame(parent.Id, ResolutionFrameKind.ActualUseTargetWindow);
                ContinueAfterActualUseTargets(parent);
                return;
            }

            var candidate = parent.Candidates[parent.CandidateIndex];
            var context = parent.Contexts[parent.CandidateIndex];
            if (!_coreProgramLifecycleOperations.CanRunProgramTrigger(candidate, context))
            {
                AdvanceActualUseTargetCandidate(parent, false, false);
                continue;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(parent with { Step = ResolutionFrameStep.AwaitingResponse });
            if (_coreProgramLifecycleOperations.GetProgramTrigger(candidate).Optional)
                _coreProgramLifecycleOperations.ExposeProgramTriggerDecision(candidate, context);
            else
                _coreProgramLifecycleOperations.BeginProgramBinding(candidate, context);
            return;
        }
    }

    internal void AdvanceActualUseTargetCandidate(ActualUseTargetWindowFrame parent, bool activated, bool completed)
    {
        var c = parent.Candidates[parent.CandidateIndex];
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramBindingResolvedEvent(parent.Id, c.SkillId, c.BindingId, c.SkillInstanceId, c.OwnerSeat, SkillProgramTriggerWindow.OtherActualUseTargeted, activated, completed));
        _coreRuntimeOperations.ReplaceRuntimeTop(parent with { CandidateIndex = parent.CandidateIndex + 1, Step = ResolutionFrameStep.ResolvingEffect });
    }

    internal void CompleteActualUseTargetBinding(ProgramSkillFrame child, bool completed)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ActualUseTargetWindowFrame parent || parent.Id != child.WindowContext?.ParentFrameId || parent.CandidateIndex >= parent.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(child, parent.Candidates[parent.CandidateIndex]))
            throw new InvalidOperationException("The paid target binding lost its exact owning candidate.");
        // The binding fact has already been committed by the standard completion path.
        _coreRuntimeOperations.ReplaceRuntimeTop(parent with { CandidateIndex = parent.CandidateIndex + 1, Step = ResolutionFrameStep.ResolvingEffect });
        _coreRuntimeOperations.AdvanceRuntimeTop<ActualUseTargetWindowFrame>();
    }

    internal void ContinueAfterActualUseTargets(ActualUseTargetWindowFrame parent)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(parent.ParentFrameId)is not { } use)
            throw new InvalidOperationException("The target window lost its actual use.");
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            if (parent.ReturnKind != ActualUseTargetReturnKind.OrdinaryTrick)
            {
                var finishedAttack = _coreActiveNativeAttackQueriesOperations.ActiveCardAttack;
                if (finishedAttack is null || finishedAttack.ResolutionId != use.Id)
                    throw new InvalidOperationException("Winner cleanup lost its exact Slash.");
                _coreNativeCardUseFramesOperations.SetCardUseStep(use.Id, ResolutionFrameStep.ResolvingEffect);
                _coreNativeAttackCompletionOperations.CompleteAttack(finishedAttack);
                return;
            }

            // No new effect/card-response window after a winner. Clean only this use's paid physical entities.
            foreach (var id in use.PhysicalCardIds ?? [use.CardId])
                if (id != 0 && !_coreCurrentUsePhysicalClaimsOperations.IsCurrentUsePhysicalCardClaim(use.Id, id) && _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Processing)
                    _corePhysicalMovementFacadesOperations.MoveCard(_cardZonesOperations._cardZones.CardsAt(CardLocation.Processing).Single(c => c.Id == id), CardLocation.Processing, CardLocation.DiscardPile, CardMoveReasons.UseFinished);
            _coreNativeCardUseCompletionOperations.QueueCardUseFinishedWithoutPop(use.Id, new Card(use.CardId, use.CardKind, Suit.None, 0), use.CardKind);
            _coreNativeCardUseCompletionOperations.PopFinishedCardUse(use.Id);
            if (_resolutionStackOperations._resolutionStack.Count == 0 && _coreMatchOutcomeStatusWinnerAccess._status != EngineStatus.Completed)
                _kernelCoreMainOperations.CompleteGame();
            return;
        }

        if (parent.ReturnKind == ActualUseTargetReturnKind.OrdinaryTrick)
        {
            var ret = parent.TrickReturn ?? throw new InvalidOperationException("Missing ordinary-trick return.");
            var action = use.Action ?? throw new InvalidOperationException("An ordinary-trick target window requires an actual action.");
            var pending = new global::CardGame.Core.NativeOwners.JizhiResolution(action.ActorSeat, use.Id, _coreDamageColorDuelProgramsOperations.GetTrickRepresentation(use.Id, ret.EffectCardId, true), use.CardKind, use.CardKind == CardKind.BorrowedSword ? use.TargetSeats.Skip(use.TargetIndex).Take(2).ToArray() : use.TargetSeats, ret.ActionKind, ret.TargetCardId, ret.RequiredCardKind);
            if (!_coreCardActionsOperations.TryBeginProgramCardWindow(null, action, SkillProgramTriggerWindow.CardUseBeforeTargetEffects, use.TargetSeats, ProgramCardContinuation.BeforeTrickTargetEffects, trickContinuation: new(ret.EffectCardId, ret.ActionKind, ret.TargetCardId, ret.RequiredCardKind)))
                _coreNativeNullificationOperations.ContinueJizhiOrNullificationAfterTargetTriggers(pending);
            return;
        }

        var attack = _coreActiveNativeAttackQueriesOperations.ActiveCardAttack;
        if (attack is null || attack.ResolutionId != use.Id)
            throw new InvalidOperationException("The target return lost its actual Slash.");
        if (_coreSlashTargetPenaltyWindowsOperations.TryBeginSlashTargetPenalties(attack, parent.ReturnKind == ActualUseTargetReturnKind.LegacyVirtualSlash, afterActualTargets: true))
            return;
        if (_coreSlashTargetBenefitWindowsOperations.TryBeginSlashTargetBenefits(attack, parent.ReturnKind == ActualUseTargetReturnKind.LegacyVirtualSlash, afterActualTargets: true))
            return;
        if (parent.ReturnKind == ActualUseTargetReturnKind.LegacyVirtualSlash)
        {
            if (_coreProgramCardTargetEffectsOperations.IsCardEffectIneffective(use.Id, attack.TargetSeat))
            {
                _coreNativeCardUseFramesOperations.SetCardUseStep(use.Id, ResolutionFrameStep.ResolvingEffect);
                _coreNativeAttackCompletionOperations.CompleteAttack(attack);
            }
            else
                _coreNativeSlashUseOperations.ContinueSlashAfterResponsePrograms(attack);
        }
        else if (!_coreCardActionsOperations.TryBeginProgramCardUseBeforeTargetEffects(attack))
            _coreNativeSlashUseOperations.ContinueSlashAfterProgramTargetEffects(attack);
    }
}
