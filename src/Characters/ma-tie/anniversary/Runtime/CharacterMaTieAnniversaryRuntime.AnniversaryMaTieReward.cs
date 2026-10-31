#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterMaTieAnniversaryRuntime
{
    [ProgramPipelineHook(ProgramPipelinePhase.CollectIssuedCardTrigger, 46700)]
    internal void CollectMaTieIssuedReward(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result)
    {
        if (window != SkillProgramTriggerWindow.CardUseCommitted || action.Type != CardActionType.Use || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(u => u.Action?.ActionId == action.ActionId)is not { AnniversaryMaTieOrigin: { } n, ProgramUseCommitted: true } use || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == n.FrameId)is not { } parent || !_coreMatchParticipantsPlayersAccess._players[parent.OwnerSeat].IsAlive || !MaTieNativeMatches(use, parent) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversaryMaTieRewardIssuedEvent>().Any(e => e.UseId == n.UseId))
            return;
        if (!result.Any(c => c.OwnerSeat == parent.OwnerSeat && c.SkillId == AnniversaryMaTieComposition.Quxian && c.TriggerId == "issued-slash-reward" && c.SkillInstanceId == n.Source.SkillInstanceId))
        {
            var facts = _coreProgramLifecycleOperations.CaptureProgramTriggerFacts2(_coreMatchParticipantsPlayersAccess._players[parent.OwnerSeat], action)with
            {
                CardUseCausedDamage = use.CausedDamage
            };
            var context = _coreCardActionsOperations.CreateCardActionProgramContext(action, window, 0, parent.OwnerSeat, action.ActorSeat, facts);
            result.Add(new(parent.OwnerSeat, action.ActorSeat, AnniversaryMaTieComposition.Quxian, "issued-slash-reward", n.Hash, n.Source.SkillInstanceId, FrozenContext: context));
        }
    }

    [ProgramPipelineHook(ProgramPipelinePhase.IssuedTriggerCandidate, 46700)]
    internal bool IsIssuedMaTieReward(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => c.SkillId == AnniversaryMaTieComposition.Quxian && c.BindingId == "issued-slash-reward" && MaTieRewardUse(c.OwnerSeat, w)is { AnniversaryMaTieOrigin: { } n } && c.SkillInstanceId == n.Source.SkillInstanceId && c.GameplayHash == n.Hash;
    internal CardUseFrame? MaTieCardWindow(ProgramSkillWindowContext c, SkillProgramTriggerWindow window)
    {
        if (c.Window != window || c.CardUse is not { } card || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == c.ParentFrameId)is not { } parent || parent.ParentFrameId != card.ParentCardUseFrameId || parent.Action.ActionId != card.CardActionId || _coreCardUseLifecycleOperations.LifecycleCardUse(parent.ParentFrameId)is not { Action: { } action } use || action.ActionId != card.CardActionId)
            return null;
        return use;
    }

    internal CardUseFrame? MaTieRewardUse(int owner, ProgramSkillWindowContext c)
    {
        if (MaTieCardWindow(c, SkillProgramTriggerWindow.CardUseCommitted)is not { ProgramUseCommitted: true, AnniversaryMaTieOrigin: { } n } use || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == n.FrameId)is not { } f || f.OwnerSeat != owner || !MaTieNativeMatches(use, f) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversaryMaTieRewardIssuedEvent>().Any(e => e.UseId == n.UseId))
            return null;
        return use;
    }

    internal bool MaTieCardParent(ProgramSkillFrame f, SkillProgramTriggerWindow window)
    {
        if (f.ActivationId != f.TriggerId || f.WindowContext is not { } c || MaTieCardWindow(c, window)is null || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == c.ParentFrameId)is not { } parent || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, CoreGameEngineCardActionsRuntime.ToSharedCandidate(parent.Candidates[parent.CandidateIndex])) || f.WindowContext != _coreCardActionsOperations.CreateCardActionProgramContext2(parent, parent.Candidates[parent.CandidateIndex]))
            return false;
        return true;
    }

    internal SkillProgramStepOutcome BeginMaTieReward(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.SkillId != AnniversaryMaTieComposition.Quxian || f.TriggerId != "issued-slash-reward" || f.InstructionIndex != 1 || f.AnniversaryMaTie is not null || !MaTieCardParent(f, SkillProgramTriggerWindow.CardUseCommitted) || MaTieRewardUse(f.OwnerSeat, f.WindowContext!)is not { AnniversaryMaTieOrigin: { } n } use)
            throw new InvalidOperationException("Quxian reward requires this exact once-committed issued actual Slash.");
        var parent = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Single(p => p.Id == n.FrameId);
        var original = parent.AnniversaryMaTie!;
        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { AnniversaryMaTie = original with { Used = original.Used.Contains(n.Actor) ? original.Used : original.Used.Append(n.Actor).ToArray() } });
        var r = new AnniversaryMaTieReceipt(MaTieSource(f), f.GameplayHash, f.WindowContext!.ParentFrameId, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, AnniversaryMaTieStage.RewardDrawing)
        {
            RewardParent = parent.Id,
            UseId = use.Id,
            ActionId = n.ActionId,
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryMaTie = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryMaTieStartedEvent(f.Id, r.Source, r.Hash, r.ParentId, r.Turn, r.TurnOwner));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryMaTieRewardIssuedEvent(f.Id, parent.Id, n.Index, n.UseId, n.ActionId));
        _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, 1, new("skill-program.anniversary:quxian.used-slash-draw"));
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveAnniversaryMaTieQuxianReward)]
    internal void ReturnMaTieRewardDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        if (!ValidMaTie(f) || f.AnniversaryMaTie is not { Stage: AnniversaryMaTieStage.RewardDrawing, Draw: null } r || outcome.RecipientSeat != f.OwnerSeat || outcome.RequestedCount != 1 || outcome.SequenceAfter != _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
            throw new InvalidOperationException("Quxian cannot substitute or repeat its issued one-card native draw.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryMaTie = r with { Stage = AnniversaryMaTieStage.RewardChildren, Draw = outcome, After = outcome.SequenceAfter }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryMaTieRewardReturnedEvent(f.Id, r.RewardParent, r.UseId, outcome.ActualDrawIds.Count, outcome.SequenceBefore, outcome.SequenceAfter));
        ContinueMaTie(f.Id);
    }

    internal SkillProgramStepOutcome RecordMaTieZhuiji(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.SkillId != AnniversaryMaTieComposition.Zhuiji || f.TriggerId != "completed-other-targets" || f.InstructionIndex != 1 || !MaTieCardParent(f, SkillProgramTriggerWindow.CardUseCompleted) || MaTieCardWindow(f.WindowContext!, SkillProgramTriggerWindow.CardUseCompleted)is not { Action: { Type: CardActionType.Use } action } use || action.ActorSeat != f.OwnerSeat)
            throw new InvalidOperationException("Zhuiji needs its exact completed actual use and final other character targets.");
        foreach (var target in action.EffectiveDesignatedTargetSeats.Where(t => t != f.OwnerSeat).Distinct())
        {
            if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversaryMaTieDistanceEvent>().Any(e => e.ActionId == action.ActionId && e.Source.OwnerSeat == f.OwnerSeat && e.Target == target))
                continue;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryMaTieDistanceEvent(f.Id, MaTieSource(f), f.GameplayHash, use.Id, action.ActionId, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, target));
        }

        return SkillProgramStepOutcome.Continue;
    }

    [ProgramDistanceRule(ProgramDistanceStage.DistanceProjection, 46700)]
    internal RuleQueryEvaluation ApplyMaTieZhuiji(CharacterState source, CharacterState target, RuleQueryEvaluation original)
    {
        if (source.Seat == target.Seat || !_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.RecordAnniversaryMaTieZhuiji) || _corePlayCardHistoryOperations.EventsSinceLastBoundary(e => e is TurnStartedEvent).OfType<TurnEndedEvent>().Any(e => e.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber && e.ActorSeat == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat))
            return original;
        var grants = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversaryMaTieDistanceEvent>().Where(e => e.Source.OwnerSeat == source.Seat && e.Target == target.Seat && e.Turn == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber && e.TurnOwner == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && e.Source.SkillId == AnniversaryMaTieComposition.Zhuiji && _contentRegistryOperations._contentRegistry.GetSkill(e.Source.SkillId).Program?.GameplayHash == e.Hash && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ProgramBindingStartedEvent>().Count(b => b.FrameId == e.FrameId && b.OwnerSeat == e.Source.OwnerSeat && b.SkillId == e.Source.SkillId && b.SkillInstanceId == e.Source.SkillInstanceId && b.BindingId == e.Source.BindingId && b.Window == SkillProgramTriggerWindow.CardUseCompleted) == 1).ToArray();
        if (grants.Length == 0)
            return original;
        return RuleQueryService.Evaluate(SkillRuleQuery.OutgoingDistance, new(1, int.MaxValue), [new RuleQueryBaseTerm("distance:before-anniversary-zhuiji", CoreGameEngineRuleQueriesRuntime.ConvertRuleValue(original))], grants.Select(g => (RuleQueryContribution)new FiniteRuleQueryContribution($"anniversary-zhuiji:{(g.FrameId)}:{(g.Target)}", SkillRuleOperation.Set, 1)).ToArray());
    }
}
