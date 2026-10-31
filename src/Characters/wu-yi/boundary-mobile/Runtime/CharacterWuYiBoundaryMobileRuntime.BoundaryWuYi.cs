#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWuYiBoundaryMobileRuntime
{
    internal bool HasBoundaryWuYi => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ArmBoundaryWuYiBenxi);

    internal static CardConversionSource BoundaryWuYiSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    SkillProgramStepOutcome IBoundaryWuYiHost.Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => ExecuteBoundaryWuYi(f, op);
    internal SkillProgramStepOutcome ExecuteBoundaryWuYi(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (op == SkillProgramEffectOp.PrepareBoundaryWuYiBenxi)
        {
            if (f.BoundaryWuYiPhase is not null || f.WindowContext?.Window != SkillProgramTriggerWindow.PlayPhaseStarting || _coreActualTurnProgressCurrentSeatPhaseAccess._phase != TurnPhase.Play || _coreActualTurnProgressCurrentSeatPhaseAccess._currentSeat != f.OwnerSeat || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { Kind: ActualDiscardRecoveryPhaseKind.Play } phase || phase.ActorSeat != f.OwnerSeat)
                throw new InvalidOperationException("Benxi requires its original actual own Play-start window.");
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { BoundaryWuYiPhase = new(BoundaryWuYiSource(f), f.GameplayHash, phase, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence) });
            return SkillProgramStepOutcome.Continue;
        }

        if (op == SkillProgramEffectOp.ArmBoundaryWuYiBenxi)
        {
            if (f.BoundaryWuYiPhase is not { } r || r.Source != BoundaryWuYiSource(f) || r.Hash != f.GameplayHash || !BoundaryWuYiCostPaid(f))
                throw new InvalidOperationException("Benxi requires its original completed native whole-card discard.");
            if (_coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase() != r.Phase || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
                return SkillProgramStepOutcome.Continue;
            if (_coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryWuYiBenxiGrantedEvent>().Any(e => e.FrameId == f.Id))
                throw new InvalidOperationException("Benxi cannot issue the same paid phase twice.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryWuYiBenxiGrantedEvent(f.Id, r.Source, r.Hash, r.Phase, _coreProgramLifecycleOperations.GetProgramCardSet(f, BoundaryWuYiComposition.Cost).CardIds.Count, r.Before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
            return SkillProgramStepOutcome.Continue;
        }

        if (op == SkillProgramEffectOp.ResolveBoundaryWuYiTargets)
            return BeginBoundaryWuYiTargets(f);
        if (!CanOfferBoundaryWuYiReward(new(f.OwnerSeat, f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, 0), f.WindowContext!))
            throw new InvalidOperationException("Benxi reward requires its exact once-completed issued card's actual damage.");
        var identity = f.WindowContext!.CardUse!;
        var grant = BoundaryWuYiIssuedGrant(identity.ParentCardUseFrameId, identity.CardActionId, f.OwnerSeat, f.SkillInstanceId)!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryWuYiRewardIssuedEvent(f.Id, grant.FrameId, identity.ParentCardUseFrameId, identity.CardActionId, f.OwnerSeat));
        return SkillProgramStepOutcome.Continue;
    }

    internal bool BoundaryWuYiCostPaid(ProgramSkillFrame f)
    {
        if (f.BoundaryWuYiPhase is not { } r || f.CardSetBindings.SingleOrDefault(b => b.Name == BoundaryWuYiComposition.Cost)is not { CardIds.Count: > 0 } cost)
            return false;
        return cost.CardIds.All(id => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > r.Before && m.CardId == id && m.From.OwnerSeat == f.OwnerSeat && m.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && m.To == CardLocation.DiscardPile && m.Reason.Value == "skill-program." + f.SkillId + ".MoveBoundCards") == 1);
    }

    internal bool ValidBoundaryWuYiGrant(BoundaryWuYiBenxiGrantedEvent grant) => grant.Source.SkillId == BoundaryWuYiComposition.Skill && grant.Source.BindingId == BoundaryWuYiComposition.Start && grant.X > 0 && grant.Before <= grant.After && grant.Phase.Kind == ActualDiscardRecoveryPhaseKind.Play && grant.Phase.ActorSeat == grant.Source.OwnerSeat && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(grant.Phase) && _contentRegistryOperations._contentRegistry.GetSkill(grant.Source.SkillId).Program?.GameplayHash == grant.Hash && _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryWuYiBenxiGrantedEvent>().Count(e => e == grant) == 1;
    internal IEnumerable<BoundaryWuYiBenxiGrantedEvent> ActiveBoundaryWuYiGrants(int owner)
    {
        if (!HasBoundaryWuYi || _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryWuYiBenxiGrantedEvent>().Count == 0 || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { Kind: ActualDiscardRecoveryPhaseKind.Play } phase || phase.ActorSeat != owner)
            return[];
        return _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryWuYiBenxiGrantedEvent>().Where(g => g.Source.OwnerSeat == owner && g.Phase == phase && ValidBoundaryWuYiGrant(g));
    }

    [ProgramDistanceRule(ProgramDistanceStage.DistanceBaseTerms, 53100)]
    internal void BoundaryWuYiDistance(ProgramDistanceQuery query, List<RuleQueryBaseTerm> terms)
    {
        if (query.Source.Seat == query.Target.Seat)
            return;
        foreach (var grant in ActiveBoundaryWuYiGrants(query.Source.Seat))
            terms.Add(new("issued:boundary-wu-yi:" + grant.FrameId, -grant.X));
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 53100)]
    internal void ConsumeBoundaryWuYiNextUse(IGameEvent payload)
    {
        if (!HasBoundaryWuYi)
            return;
        CardActionContext? action = null;
        long useId = 0;
        if (payload is CardUseDeclaredEvent declared && _coreCardUseLifecycleOperations.LifecycleCardUse(declared.ResolutionId)?.Action is { } used)
        {
            action = used;
            useId = declared.ResolutionId;
        }
        else if (payload is CardActionAcceptedEvent { Action.Type: CardActionType.Response } accepted)
            action = accepted.Action;
        // The shared ledger is entered only for genuine uses; Slash played to
        // a Duel or a supplied faction response never gains an entry here.
        if (action is null || !_coreGameEngineActualPlayPhaseCardUsesActualPlayPhaseUseActionsAccess._actualPlayPhaseUseActions.Contains(action.ActionId) || CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(action.EffectiveKind) != SkillProgramCardCategory.Basic && !CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(action.EffectiveKind))
            return;
        foreach (var grant in ActiveBoundaryWuYiGrants(action.ActorSeat).Where(g => !_coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryWuYiBenxiConsumedEvent>().Any(e => e.GrantFrameId == g.FrameId)).ToArray())
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryWuYiBenxiConsumedEvent(grant.FrameId, action.ActionId, useId, action.ActorSeat, action.EffectiveKind, grant.Phase));
    }

    internal BoundaryWuYiBenxiGrantedEvent? BoundaryWuYiIssuedGrant(long useId, long actionId, int owner, string instance) => _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryWuYiBenxiGrantedEvent>().SingleOrDefault(g => g.Source.OwnerSeat == owner && g.Source.SkillInstanceId == instance && ValidBoundaryWuYiGrant(g) && _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryWuYiBenxiConsumedEvent>().Count(e => e.GrantFrameId == g.FrameId && e.UseId == useId && e.ActionId == actionId && e.Actor == owner && e.Phase == g.Phase) == 1);
    internal bool CanOfferBoundaryWuYiTargets(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => c.SkillId == BoundaryWuYiComposition.Skill && c.BindingId == BoundaryWuYiComposition.Targets && context.CardUse is { } identity && BoundaryWuYiIssuedGrant(identity.ParentCardUseFrameId, identity.CardActionId, c.OwnerSeat, c.SkillInstanceId)is not null && _coreDesignatedExtraTargetsOperations.TryGetDesignatedExtraTargetUse(c.OwnerSeat, context, out var use, out var window) && _coreDesignatedExtraTargetsOperations.DesignatedExtraTargetCandidateMatches(c, context, window) && !_coreProgramEventHistoryOperations.ProgramEventHistory<DesignatedExtraTargetOfferedEvent>().Any(e => e.CardUseFrameId == use.Id && e.Source.SkillId == c.SkillId && e.Source.SkillInstanceId == c.SkillInstanceId);
    internal bool CanOfferBoundaryWuYiReward(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => c.SkillId == BoundaryWuYiComposition.Skill && c.BindingId == BoundaryWuYiComposition.Reward && _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive && context.Window == SkillProgramTriggerWindow.CardUseCompleted && context.CardUse is { } identity && BoundaryWuYiIssuedGrant(identity.ParentCardUseFrameId, identity.CardActionId, c.OwnerSeat, c.SkillInstanceId)is { } grant && _coreCardUseLifecycleOperations.LifecycleCardUse(identity.ParentCardUseFrameId)is { Step: ResolutionFrameStep.Completed, Action: { } action } use && action.ActionId == identity.CardActionId && action.ActorSeat == c.OwnerSeat && _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<DamageAppliedEvent>().Any(e => e.SourceCardUseResolutionId == use.Id && e.Amount > 0) && !_coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryWuYiRewardIssuedEvent>().Any(e => e.GrantFrameId == grant.FrameId);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 53100, SkillProgramEffectOp.ResolveBoundaryWuYiTargets, SkillProgramEffectOp.RewardBoundaryWuYiUse)]
    internal bool QualifyBoundaryWuYi(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => c.BindingId == BoundaryWuYiComposition.Targets ? CanOfferBoundaryWuYiTargets(c, context) : CanOfferBoundaryWuYiReward(c, context);
    [ProgramTargetWindow(ProgramTargetWindowStage.FinalizedInstantTrickRequirement, 53100)]
    internal bool BoundaryWuYiNeedsFinalizedTrickWindow(ProgramTargetCardWindowQuery query) => HasBoundaryWuYi;
    [ProgramPipelineHook(ProgramPipelinePhase.CollectIssuedCardTrigger, 53100)]
    internal void CollectIssuedBoundaryWuYi(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result)
    {
        if (!HasBoundaryWuYi || window is not (SkillProgramTriggerWindow.CardUseTargetsFinalized or SkillProgramTriggerWindow.CardUseCompleted) || _resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame use || use.Action?.ActionId != action.ActionId)
            return;
        var binding = window == SkillProgramTriggerWindow.CardUseTargetsFinalized ? BoundaryWuYiComposition.Targets : BoundaryWuYiComposition.Reward;
        foreach (var grant in _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryWuYiBenxiGrantedEvent>().Where(g => BoundaryWuYiIssuedGrant(use.Id, action.ActionId, action.ActorSeat, g.Source.SkillInstanceId) == g).ToArray())
        {
            if (result.Any(c => c.SkillId == grant.Source.SkillId && c.TriggerId == binding && c.OwnerSeat == grant.Source.OwnerSeat && c.SkillInstanceId == grant.Source.SkillInstanceId))
                continue;
            var trigger = _contentRegistryOperations._contentRegistry.GetSkill(grant.Source.SkillId).Program!.Triggers.Single(t => t.Id == binding);
            var context = _coreCardActionsOperations.CreateCardActionProgramContext(action, window, 0, grant.Source.OwnerSeat, grant.Source.OwnerSeat, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts2(_coreMatchParticipantsPlayersAccess._players[grant.Source.OwnerSeat], action));
            result.Add(new(grant.Source.OwnerSeat, grant.Source.OwnerSeat, grant.Source.SkillId, binding, grant.Hash, grant.Source.SkillInstanceId, trigger.Priority, context));
        }
    }

    [ProgramPipelineHook(ProgramPipelinePhase.IssuedTriggerCandidate, 53100)]
    internal bool AdmitIssuedBoundaryWuYi(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => c.SkillId == BoundaryWuYiComposition.Skill && (CanOfferBoundaryWuYiTargets(c, context) || CanOfferBoundaryWuYiReward(c, context));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 53100)]
    internal bool KeepBoundaryWuYiIssuedFrame(ProgramSkillFrame f) => f.BoundaryWuYiPhase is not null && BoundaryWuYiCostPaid(f) || f.BoundaryWuYiTargets is not null || f.SkillId == BoundaryWuYiComposition.Skill && f.TriggerId is BoundaryWuYiComposition.Targets or BoundaryWuYiComposition.Reward && f.WindowContext?.CardUse is { } identity && BoundaryWuYiIssuedGrant(identity.ParentCardUseFrameId, identity.CardActionId, f.OwnerSeat, f.SkillInstanceId)is not null;
}
