#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYuanShaoMouMobileRuntime
{
    internal bool HasMouYuanShaoCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ReserveMouYuanShaoReward);

    internal static bool IsMouYuanShaoTargetTrigger(string skill, SkillProgramTrigger trigger) => RuntimeSkills.RuntimeRules_yuan_shao_mou_mobile_MouYuanShao.IsMouYuanShaoTargetTrigger(skill, trigger);
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 30100)]
    internal void CaptureMouYuanShaoTurn(TurnStartedEvent turn)
    {
        if (!HasMouYuanShaoCapability)
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouYuanShaoTurnStartedEvent(turn.TurnNumber, turn.ActorSeat, _coreProgramEventHistoryOperations.Value.ProgramEventHistory<TurnStartedEvent>().Count(), _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
    }

    internal MouYuanShaoTurnStartedEvent MouYuanShaoTurn() => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouYuanShaoTurnStartedEvent>().LastOrDefault() ?? new(0, _coreActualTurnProgressCurrentSeatAccess._currentSeat, 0, 0);
    internal int MouYuanShaoActualRewards(int owner, string skill)
    {
        var start = MouYuanShaoTurn();
        var reason = $"skill-program.{(skill)}.{(SkillProgramEffectOp.Draw)}";
        return _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > start.MovementBefore && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(owner) && m.Reason.Value == reason);
    }

    internal bool CanMouYuanShaoReward(ProgramTriggerCandidate candidate, ProgramSkillWindowContext c)
    {
        if (!_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None || c.CardUse is not { } action || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == c.ParentFrameId)is not { } w || w.Action.ActionId != action.CardActionId || w.CandidateIndex < 0 || w.CandidateIndex >= w.Candidates.Count)
            return false;
        if (candidate.SkillId == MouYuanShaoComposition.Luanji)
        {
            if (c.Window != SkillProgramTriggerWindow.CardResponseCompleted || w.ResponseCompletion is not { CostsDrained: true, OriginalContinuation: ProgramCardContinuation.GroupResponse or ProgramCardContinuation.FactionDefenseDodge } || w.Action.EffectiveKind != CardKind.Dodge || w.Action.ProviderSeat == candidate.OwnerSeat || w.Action.ParentActionId is not { } parentAction || _coreCardUseLifecycleOperations.LifecycleCardUse(w.ParentFrameId)is not { Action: { Type: CardActionType.Use } parent } use || parent.ActionId != parentAction || parent.EffectiveKind != CardKind.ArrowBarrage || parent.ActorSeat != candidate.OwnerSeat || use.SourceSeat != candidate.OwnerSeat || MouYuanShaoActualRewards(candidate.OwnerSeat, candidate.SkillId) >= 3)
                return false;
            return !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouYuanShaoRewardReservedEvent>().Any(e => e.Source.OwnerSeat == candidate.OwnerSeat && e.Source.SkillId == candidate.SkillId && e.ActionId == action.CardActionId);
        }

        if (candidate.SkillId != MouYuanShaoComposition.Xueyi || c.Window != SkillProgramTriggerWindow.CardUseTargetsFinalized || c.TargetSeat is not { } target || w.Action.Type != CardActionType.Use || w.Action.ActorSeat != candidate.OwnerSeat || !_coreLordSkillProjectionsOperations.HasSkillRoleQualification(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat], candidate.SkillId, candidate.SkillInstanceId, Role.Lord) || MouYuanShaoActualRewards(candidate.OwnerSeat, candidate.SkillId) >= 2)
            return false;
        var source = new CardConversionSource(candidate.SkillId, candidate.BindingId, candidate.OwnerSeat, candidate.SkillInstanceId);
        return _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouYuanShaoDesignationEvent>().Any(e => e.UseId == w.ParentFrameId && e.ActionId == action.CardActionId && e.TargetSeat == target && e.Source == source && e.GameplayHash == candidate.GameplayHash && e.OtherQun) && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouYuanShaoRewardReservedEvent>().Any(e => e.Source.OwnerSeat == candidate.OwnerSeat && e.Source.SkillId == candidate.SkillId && e.ActionId == action.CardActionId && e.TargetSeat == target);
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 30100, SkillProgramEffectOp.ReserveMouYuanShaoReward)]
    internal bool CanOfferMouYuanShaoReward(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => CanMouYuanShaoReward(candidate, context);
    internal SkillProgramStepOutcome ReserveMouYuanShaoReward(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.InstructionIndex != 1 || f.TriggerId is null || f.ActivationId != f.TriggerId || f.WindowContext is not { CardUse: { } action } c)
            throw new InvalidOperationException("Yuan Shao's reward requires its exact native card-action candidate.");
        var candidate = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority);
        if (!CanMouYuanShaoReward(candidate, c))
            throw new InvalidOperationException("Yuan Shao's native response/designation reward is no longer current.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouYuanShaoRewardReservedEvent(f.Id, new(f.SkillId, f.TriggerId, f.OwnerSeat, f.SkillInstanceId), c.ParentFrameId, action.ParentCardUseFrameId, action.CardActionId, c.TargetSeat ?? -1, MouYuanShaoTurn()));
        return SkillProgramStepOutcome.Continue;
    }

    SkillProgramStepOutcome IMouYuanShaoHost.Reserve(ProgramSkillFrame frame) => ReserveMouYuanShaoReward(frame);
    internal void CaptureMouYuanShaoDesignation(CardActionContext action, SkillProgramTriggerWindow window)
    {
        if (!HasMouYuanShaoCapability || window != SkillProgramTriggerWindow.CardUseTargetsFinalized || action.Type != CardActionType.Use || _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(u => u.Action?.ActionId == action.ActionId)is not { } use)
            return;
        var targets = action.EffectiveKind == CardKind.BorrowedSword ? action.TargetSeats.Where((_, i) => i % 2 == 0) : action.EffectiveDesignatedTargetSeats;
        foreach (var target in targets.Distinct().Where(t => _coreTableSeatQueriesOperations.IsValidPlayerSeat(t) && _coreMatchParticipantsPlayersAccess._players[t].IsAlive))
        {
            if (_coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouYuanShaoDesignationEvent>().Any(e => e.UseId == use.Id && e.TargetSeat == target))
                continue;
            var bindings = _coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[action.ActorSeat]).GetInstanceTriggers(window).Where(b => IsMouYuanShaoTargetTrigger(b.SkillId, b.Trigger) && _coreLordSkillProjectionsOperations.HasSkillRoleQualification(_coreMatchParticipantsPlayersAccess._players[action.ActorSeat], b.SkillId, b.SkillInstanceId, Role.Lord)).ToArray();
            var otherQun = target != action.ActorSeat && _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(_coreMatchParticipantsPlayersAccess._players[target]) == "qun";
            if (bindings.Length == 0)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouYuanShaoDesignationEvent(use.Id, action.ActionId, target, null, null, otherQun));
            foreach (var binding in bindings)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouYuanShaoDesignationEvent(use.Id, action.ActionId, target, new(binding.SkillId, binding.Trigger.Id, action.ActorSeat, binding.SkillInstanceId), binding.Program.GameplayHash, otherQun));
        }
    }

    internal ProgramCardTriggerWindowFrame AppendMouYuanShaoTargets(ProgramCardTriggerWindowFrame frame)
    {
        if (!HasMouYuanShaoCapability || CoreGameEngineCardActionsRuntime.GetCardActionWindow(frame) != SkillProgramTriggerWindow.CardUseTargetsFinalized || _coreCardUseLifecycleOperations.LifecycleCardUse(frame.ParentFrameId)is not { Action: { Type: CardActionType.Use } } use || _coreMatchOutcomeWinnerAccess._winner != Winner.None || MouYuanShaoActualRewards(use.SourceSeat, MouYuanShaoComposition.Xueyi) >= 2)
            return frame;
        var action = CoreGameEngineActorHandLimitTargetProgramsRuntime.ActorHandLimitTargetCollectionAction(use);
        CaptureMouYuanShaoDesignation(action, SkillProgramTriggerWindow.CardUseTargetsFinalized);
        var claims = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouYuanShaoDesignationEvent>().Where(e => e.UseId == use.Id && e.Source is not null && e.OtherQun).ToArray();
        var added = _coreCardActionsOperations.CollectSharedCardActionCandidates(action, SkillProgramTriggerWindow.CardUseTargetsFinalized, use.TargetSeats, null, frame.Continuation).Where(c => c.SkillId == MouYuanShaoComposition.Xueyi && claims.Any(e => e.TargetSeat == c.OpponentSeat && e.Source!.SkillInstanceId == c.SkillInstanceId) && !frame.Candidates.Any(old => old.SkillId == c.SkillId && old.OwnerSeat == c.OwnerSeat && old.OpponentSeat == c.OpponentSeat && old.SkillInstanceId == c.SkillInstanceId) && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouYuanShaoRewardReservedEvent>().Any(e => e.Source.OwnerSeat == c.OwnerSeat && e.Source.SkillId == c.SkillId && e.ActionId == action.ActionId && e.TargetSeat == c.OpponentSeat)).Select(c => c with { FrozenContext = c.FrozenContext!with { ParentFrameId = frame.Id, CardUse = c.FrozenContext.CardUse!with { ParentCardUseFrameId = use.Id } } }).ToArray();
        if (added.Length != 0)
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { Candidates = Array.AsReadOnly(frame.Candidates.Concat(added).ToArray()) });
        return frame;
    }

    internal bool TryBeginMouYuanShaoLateSlashTarget(NativeOwners.CardAttackHandle attack)
    {
        if (!HasMouYuanShaoCapability || _coreMatchOutcomeWinnerAccess._winner != Winner.None || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { ProgramUseAccepted: true, Action: { Type: CardActionType.Use } } use || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != use.Id || !CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || MouYuanShaoActualRewards(use.SourceSeat, MouYuanShaoComposition.Xueyi) >= 2)
            return false;
        var action = CoreGameEngineActorHandLimitTargetProgramsRuntime.ActorHandLimitTargetCollectionAction(use);
        CaptureMouYuanShaoDesignation(action, SkillProgramTriggerWindow.CardUseTargetsFinalized);
        var claims = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouYuanShaoDesignationEvent>().Where(e => e.UseId == use.Id && e.Source is not null && e.OtherQun).ToArray();
        var windowId = _coreResolutionIdentityBoundaryOperations._resolutionSequence + 1;
        var added = _coreCardActionsOperations.CollectSharedCardActionCandidates(action, SkillProgramTriggerWindow.CardUseTargetsFinalized, use.TargetSeats, null, ProgramCardContinuation.Slash).Where(c => c.SkillId == MouYuanShaoComposition.Xueyi && claims.Any(e => e.TargetSeat == c.OpponentSeat && e.Source!.SkillInstanceId == c.SkillInstanceId) && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouYuanShaoRewardReservedEvent>().Any(e => e.Source.OwnerSeat == c.OwnerSeat && e.Source.SkillId == c.SkillId && e.ActionId == action.ActionId && e.TargetSeat == c.OpponentSeat)).OrderByDescending(c => c.Priority).ThenBy(c => c.OwnerSeat).ThenBy(c => c.SkillId, StringComparer.Ordinal).Select(c => c with { FrozenContext = c.FrozenContext!with { ParentFrameId = windowId, CardUse = c.FrozenContext.CardUse!with { ParentCardUseFrameId = use.Id } } }).ToArray();
        if (added.Length == 0)
            return false;
        _coreResolutionIdentityBoundaryOperations._resolutionSequence++;
        _coreRuntimeOperations.PushRuntimeFrame(new ProgramCardTriggerWindowFrame(windowId, use.Id, action, ProgramCardContinuation.Slash, Array.AsReadOnly(added), AttackOwnerFrameId: use.Id));
        _coreRuntimeOperations.AdvanceRuntimeTop<ProgramCardTriggerWindowFrame>();
        return true;
    }
}
