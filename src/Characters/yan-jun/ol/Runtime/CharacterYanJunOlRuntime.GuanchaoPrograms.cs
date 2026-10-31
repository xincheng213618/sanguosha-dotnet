#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYanJunOlRuntime
{
    internal (bool Ascending, int PhaseEventIndex)? CurrentGuanchaoPattern(int ownerSeat, string skillId)
    {
        var facts = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var phaseIndex = Array.FindLastIndex(facts, e => e is PhaseChangedEvent phase && phase.Phase == TurnPhase.Play && phase.ActorSeat == ownerSeat);
        var choice = facts.OfType<ProgramGuanchaoPatternChosenEvent>().LastOrDefault(e => e.OwnerSeat == ownerSeat && e.SkillId == skillId && e.PhaseEventIndex == phaseIndex);
        return choice is null ? null : (choice.Ascending, choice.PhaseEventIndex);
    }

    internal (bool Ascending, int PhaseEventIndex)? OwnTurnGuanchaoPattern(int ownerSeat, string skillId)
    {
        var facts = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var turn = Array.FindLastIndex(facts, e => e is TurnStartedEvent);
        var choice = facts.Skip(turn + 1).OfType<ProgramGuanchaoPatternChosenEvent>().LastOrDefault(e => e.OwnerSeat == ownerSeat && e.SkillId == skillId);
        var phase = Array.FindLastIndex(facts, e => e is PhaseChangedEvent);
        return choice is null || ownerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat ? null : (choice.Ascending, phase);
    }

    internal bool UsesOwnTurnGuanchaoLedger(ProgramSkillFrame frame) => _coreProgramLifecycleOperations.GetProgramTrigger(frame)is { Window: SkillProgramTriggerWindow.CardUseCommitted, IncludeResponseUses: true, OwnerRelation: SkillProgramCardActionOwnerRelation.Actor, Optional: false, Condition.Kind: SkillProgramTriggerConditionKind.CardActionActorIsCurrentTurn, Effects: [{ Op: SkillProgramEffectOp.GuanchaoRankDraw }] };
    internal IReadOnlyList<int?> GuanchaoPhaseUseRanks(int ownerSeat, int phaseEventIndex, long currentActionId, bool includeResponseUses = false)
    {
        var ranks = new List<int?>();
        var seen = new HashSet<long>();
        var responseUses = includeResponseUses ? _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ProgramGuanchaoRankDrawEvent>().Where(e => e.CardActionId > 0).Select(e => e.CardActionId).ToHashSet() : null;
        foreach (var fact in _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().Skip(phaseEventIndex + 1))
        {
            var action = fact switch
            {
                CardActionAcceptedEvent accepted => accepted.Action,
                CardUseAppearanceCapturedEvent appearance => appearance.Action,
                _ => null
            };
            if (action is null)
                continue;
            if ((action.Type != CardActionType.Use && responseUses?.Contains(action.ActionId) != true) || action.ActorSeat != ownerSeat || !seen.Add(action.ActionId) || action.ActionId == currentActionId)
                continue;
            var rank = action.EffectiveRank ?? (action.PhysicalCards.Count == 1 ? _coreAdvancedLifecycleOperations.GetAdvancedCard(action.PhysicalCards[0].CardId).Rank : (int? )null);
            ranks.Add(includeResponseUses && rank is <= 0 ? null : rank);
        }

        return ranks;
    }

    internal static bool MatchesGuanchaoPattern(IReadOnlyList<int?> ranks, bool ascending) => RuntimeSkills.RuntimeRules_yan_jun_ol_Guanchao_Programs.MatchesGuanchaoPattern(ranks, ascending);
    internal SkillProgramStepOutcome GuanchaoProgramChoosePattern(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var owner = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        if (!owner.IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || owner.Seat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat)
            return SkillProgramStepOutcome.Continue;
        var presentation = _contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId);
        PromptChoice Pattern(bool ascending) => new(new ChoiceId($"guanchao-pattern.frame-{(frame.Id)}.{((ascending ? "ascending" : "descending"))}"), ascending ? "严格递增。" : "严格递减。", [], [], new Dictionary<string, string> { ["program-action"] = "guanchao-pattern", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["ascending"] = ascending ? "true" : "false" });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, active.OwnerSeat, $"【{(presentation.Name)}】请选择本阶段使用牌点数的规律。", [], [], active.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = false,
            TargetSeat = active.OwnerSeat,
            SkillPrompt = new SkillPromptPresentation(active.SkillId, presentation.Name, $"{(presentation.Name)} · 选择规律", "本阶段你使用过的所有牌的点数均符合所选规律时，你每次使用牌摸一张牌。"),
            Choices = new[]
            {
                Pattern(true),
                Pattern(false)
            }.AsReadOnly()
        };
        _coreMatchOutcomeStatusWinnerAccess._status = owner.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    [ProgramAnswer("guanchao-pattern")]
    internal void ResolveGuanchaoPatternChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("The Guanchao pattern choice lost its program frame.");
        var paused = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex);
        if (paused.Effect is not { Op: SkillProgramEffectOp.GuanchaoChoosePattern } || selected.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture))
            throw new InvalidOperationException("The Guanchao pattern choice does not match its suspended instruction.");
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } decision || decision.PlayerSeat != active.OwnerSeat)
            throw new InvalidOperationException("The Guanchao pattern chooser changed while suspended.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var ascending = selected.Parameters.GetValueOrDefault("ascending") == "true";
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramGuanchaoPatternChosenEvent(active.Id, active.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), active.OwnerSeat, CurrentPlayPhaseEventIndex(active.OwnerSeat), ascending));
        _coreMatchLogOperations.AddLog("SkillEffect", $"{(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].Name)} 观潮：本阶段使用牌的点数按{((ascending ? "严格递增" : "严格递减"))}结算。", active.OwnerSeat);
        _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
    }

    internal SkillProgramStepOutcome GuanchaoProgramRankDraw(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var owner = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        if (!owner.IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            return SkillProgramStepOutcome.Continue;
        var ownTurnLedger = UsesOwnTurnGuanchaoLedger(active);
        var pattern = ownTurnLedger ? OwnTurnGuanchaoPattern(owner.Seat, active.SkillId) : CurrentGuanchaoPattern(owner.Seat, active.SkillId);
        if (pattern is null)
            return SkillProgramStepOutcome.Continue;
        var context = frame.WindowContext?.CardUse;
        var action = _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().LastOrDefault()?.Action ?? _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault(item => item.Action?.ActionId == context?.CardActionId)?.Action;
        if (action is null || action.ActorSeat != owner.Seat)
            return SkillProgramStepOutcome.Continue;
        var currentRank = action.EffectiveRank ?? (action.PhysicalCards.Count == 1 ? _coreAdvancedLifecycleOperations.GetAdvancedCard(action.PhysicalCards[0].CardId).Rank : (int? )null);
        if (ownTurnLedger && currentRank is <= 0)
            currentRank = null;
        var ranks = GuanchaoPhaseUseRanks(owner.Seat, pattern.Value.PhaseEventIndex, action.ActionId, ownTurnLedger).Append(currentRank).ToArray();
        var drew = MatchesGuanchaoPattern(ranks, pattern.Value.Ascending);
        if (drew)
            _corePhysicalDrawFacadesOperations.DrawCards(owner, 1, true, new($"skill-program.{(active.SkillId)}.guanchao-draw"));
        _coreMatchLogOperations.AddLog("SkillEffect", drew ? $"{(owner.Name)} 观潮：本阶段使用过的牌点数均符合所选规律，摸一张牌。" : $"{(owner.Name)} 观潮：本阶段使用过的牌点数不再符合所选规律。", owner.Seat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramGuanchaoRankDrawEvent(active.Id, active.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), owner.Seat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, currentRank ?? 0, drew) { CardActionId = action.ActionId });
        if (drew && !_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
        return drew ? SkillProgramStepOutcome.AwaitChild : SkillProgramStepOutcome.Continue;
    }
}
