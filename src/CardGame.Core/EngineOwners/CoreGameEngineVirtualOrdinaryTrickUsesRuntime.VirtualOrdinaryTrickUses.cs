#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineVirtualOrdinaryTrickUsesRuntime
{
    SkillProgramStepOutcome IVirtualOrdinaryTrickProgramHost.UseVirtualOrdinaryTrick(ProgramSkillFrame frame, SkillProgramEffect effect) => BeginProgramVirtualOrdinaryTrick(frame, effect);
    internal bool IsExactOwnPlayEndingProgram(ProgramSkillFrame frame) => _closedProgramPhaseFrameQueriesSource.Get().IsExactOwnPlayEndingProgram(frame);
    internal bool IsExactOwnVirtualOrdinaryTrickProgram(ProgramSkillFrame frame)
    {
        if (frame.WindowContext?.Window == SkillProgramTriggerWindow.TurnEnding)
            return _characterZhaoYunMobileShiOperations.Value.IsExactMobileShiCuifengProgram(frame);
        if (frame.WindowContext?.Window == SkillProgramTriggerWindow.GameStarting)
            return _coreHellActivitiesOperations.IsExactHellArrivalProgram(frame);
        if (frame.WindowContext?.Window == SkillProgramTriggerWindow.PlayEnding)
            return IsExactOwnPlayEndingProgram(frame);
        if (frame.WindowContext?.Window is SkillProgramTriggerWindow.TurnStartBeforeNormalFlow or SkillProgramTriggerWindow.AfterTurnEnded)
            return IsExactOwnVirtualTrickTurnBoundary(frame);
        var index = _resolutionStackOperations._resolutionStack.FindIndex(item => item.Id == frame.Id);
        if (index < 1 || frame.TriggerId is null || frame.SelectedCardIds.Count != 0 || frame.WindowContext is not { } context || context.OwnerSeat != frame.OwnerSeat || frame.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase != TurnPhase.Play || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.TurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber)
            return false;
        if (!(context.Window == SkillProgramTriggerWindow.PlayPhaseStarting && context.SourceSeat == frame.OwnerSeat && context.TargetSeat == frame.OwnerSeat && _resolutionStackOperations._resolutionStack[index - 1] is PlayPhaseStartingBoundaryFrame starting && starting.Id == context.ParentFrameId && starting.OwnerSeat == frame.OwnerSeat && starting.ItemIndex >= 0 && starting.ItemIndex < starting.Items.Count && starting.Items[starting.ItemIndex] is { Kind: TurnEndingBoundaryItemKind.Program, Candidate: { } candidate }))
            return false;
        return candidate.OwnerSeat == frame.OwnerSeat && candidate.SkillId == frame.SkillId && candidate.BindingId == frame.TriggerId && frame.ActivationId == frame.TriggerId && candidate.SkillInstanceId == frame.SkillInstanceId && candidate.GameplayHash == frame.GameplayHash && candidate.OccurrenceIndex == context.OccurrenceIndex;
    }

    internal SkillProgramEffect ValidateVirtualOrdinaryTrickInstruction(ProgramSkillFrame frame)
    {
        if (_characterLuXunMouMobileOperations.Value.IsMouLuXunVirtualParent(frame))
            return _characterLuXunMouMobileOperations.Value.MouLuXunVirtualInstruction(frame);
        if (!IsExactOwnVirtualOrdinaryTrickProgram(frame) || frame.InstructionIndex <= 0)
            throw new InvalidOperationException("A virtual ordinary trick lost its exact owning phase program.");
        var effect = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
        var choulue = _coreGuanduGeneralNativeCardsOperations.IsGuanduChoulueVirtualOrdinaryProgram(frame, effect);
        if (effect.Op != SkillProgramEffectOp.UseVirtualOrdinaryTrick || effect.Target != SkillProgramEffectTarget.Owner || !choulue && (!UseVirtualOrdinaryTrickProgramOperationDescriptor.HasValidReferences(effect.OutputKind, effect.ActorReference?.Kind, effect.TargetReference?.Kind) || !UseVirtualOrdinaryTrickProgramOperationDescriptor.AllowsWindow(effect.OutputKind, frame.WindowContext!.Window) || UseVirtualOrdinaryTrickProgramOperationDescriptor.IsGroupOutput(effect.OutputKind) && effect.Condition.Kind != SkillProgramConditionKind.Always && !_coreHellKingProgramsOperations.Value.IsHellRandomAssaultBranch(frame, effect) || effect.Condition.Kind is not (SkillProgramConditionKind.Always or SkillProgramConditionKind.PindianWon or SkillProgramConditionKind.PindianNotWon) && !(effect.OutputKind == CardKind.Duel && effect.Condition.Kind == SkillProgramConditionKind.ChoiceIs) && !_coreHellKingProgramsOperations.Value.IsHellRandomAssaultBranch(frame, effect) || (effect.ActorReference?.Kind == ProgramParticipantRef.SelectedTarget || effect.TargetReference?.Kind == ProgramParticipantRef.SelectedTarget) && frame.SelectedTargetSeats.Count != 1))
            throw new InvalidOperationException("A virtual ordinary trick lost its exact references or output instruction.");
        if (effect.Condition.Kind is not (SkillProgramConditionKind.Always or SkillProgramConditionKind.ChoiceIs) && !_coreHellKingProgramsOperations.Value.IsHellRandomAssaultBranch(frame, effect))
        {
            var result = frame.PindianResultBindings.SingleOrDefault(item => item.Name == effect.Condition.SourceBind);
            if (result is null || result.SourceSeat != frame.OwnerSeat || frame.SelectedTargetSeats is not [var opponent] || result.OpponentSeat != opponent || result.SourceWon != (effect.Condition.Kind == SkillProgramConditionKind.PindianWon))
                throw new InvalidOperationException("A virtual ordinary trick lost its frozen named Pindian branch.");
        }

        return effect;
    }

    internal bool IsExactOwnVirtualTrickTurnBoundary(ProgramSkillFrame frame)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(item => item.Id == frame.Id);
        if (index < 1 || frame.TriggerId is null || frame.SelectedCardIds.Count != 0 || frame.WindowContext is not { } context || context.OwnerSeat != frame.OwnerSeat || context.SourceSeat != frame.OwnerSeat || context.TargetSeat != frame.OwnerSeat || frame.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.TurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber)
            return false;
        ProgramTriggerCandidate? candidate = null;
        if (context.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Draw && _resolutionStackOperations._resolutionStack[index - 1] is ProgramLifecycleTriggerWindowFrame start && start.Id == context.ParentFrameId && start.Window == context.Window && start.OwnerSeat == frame.OwnerSeat && start.CandidateIndex >= 0 && start.CandidateIndex < start.Candidates.Count)
            candidate = start.Candidates[start.CandidateIndex];
        if (context.Window == SkillProgramTriggerWindow.AfterTurnEnded && _resolutionStackOperations._resolutionStack[index - 1] is DeferredTurnEndFrame ending && ending.OwnerSeat == frame.OwnerSeat && _coreAfterTurnEndedProgramsOperations.MatchesAfterTurnEndedChild(ending, frame, context))
            candidate = _coreAfterTurnEndedProgramsOperations.AfterTurnEndedCandidate(ending);
        return candidate is not null && candidate.OwnerSeat == frame.OwnerSeat && candidate.SkillId == frame.SkillId && candidate.BindingId == frame.TriggerId && frame.ActivationId == frame.TriggerId && candidate.SkillInstanceId == frame.SkillInstanceId && candidate.GameplayHash == frame.GameplayHash && candidate.OccurrenceIndex == context.OccurrenceIndex;
    }

    internal IReadOnlyList<GameEngine.ProgramOrdinaryTrickUseOption> VirtualOrdinaryTrickOptions(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var actorSeat = _coreProgramCardPaymentsOperations.ResolveProgramParticipant(frame, effect.ActorReference!);
        var targetSeat = _coreProgramCardPaymentsOperations.ResolveProgramParticipant(frame, effect.TargetReference!);
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(actorSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(targetSeat) || !_coreMatchParticipantsPlayersAccess._players[actorSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[targetSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            return[];
        var actor = _coreMatchParticipantsPlayersAccess._players[actorSeat];
        var options = _coreProgramOrdinaryTrickUsesOperations.BuildProgramOrdinaryTrickUseOptions(actor, effect.OutputKind, Suit.None, enforceUsePermission: true, beneficiaryShieldSuit: Suit.None, actualEffectiveColor: null, hasActualColor: true, physicalCardIds: []).Where(option => _coreGuanduGeneralNativeCardsOperations.IsGuanduChoulueVirtualOrdinaryProgram(frame, effect) || (UseVirtualOrdinaryTrickProgramOperationDescriptor.IsGroupOutput(effect.OutputKind) ? actorSeat == targetSeat && option.TargetSeats.Count > 0 : effect.OutputKind == CardKind.DrawTwo ? actorSeat == targetSeat && option.TargetSeats.Count == 0 : option.TargetSeats.SequenceEqual([targetSeat]))).ToArray();
        return _coreNextActualUseTargetAdjustmentsOperations.NextActualUseOrdinaryTrickOptions(actor, Array.AsReadOnly(options), Suit.None, null, Suit.None, false);
    }

    internal SkillProgramStepOutcome BeginProgramVirtualOrdinaryTrick(ProgramSkillFrame input, SkillProgramEffect supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        var effect = ValidateVirtualOrdinaryTrickInstruction(frame);
        if (frame.OwnerSeat != input.OwnerSeat || frame.SkillId != input.SkillId || frame.SkillInstanceId != input.SkillInstanceId || effect != supplied || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != frame.Id || _coreProgramLifecycleOperations.HasPendingProgramBoundCards || _cardZonesOperations._cardZones.Count(CardLocation.Processing) != 0)
            throw new InvalidOperationException("A virtual ordinary trick requires its unpaid active program instruction.");
        var options = VirtualOrdinaryTrickOptions(frame, effect);
        if (options.Count == 0)
            return SkillProgramStepOutcome.Continue;
        if (options.Count == 1)
        {
            IssueProgramVirtualOrdinaryTrick(frame, effect, options[0]);
            return SkillProgramStepOutcome.AwaitChild;
        }

        var actorSeat = _coreProgramCardPaymentsOperations.ResolveProgramParticipant(frame, effect.ActorReference!);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, actorSeat, $"【{(skill.Name)}】请选择【{(CardCatalog.Get(effect.OutputKind!.Value).DisplayName)}】的合法用法。", [], options.SelectMany(option => option.TargetSeats).Distinct().ToArray(), SourceSeat: actorSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            SkillPrompt = new(frame.SkillId, skill.Name, $"{(skill.Name)} · 使用锦囊", skill.Description),
            Choices = options.Select(option => VirtualOrdinaryTrickChoice(frame, option)).ToArray()
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[actorSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal static PromptChoice VirtualOrdinaryTrickChoice(ProgramSkillFrame frame, GameEngine.ProgramOrdinaryTrickUseOption option) => new(option.Id, option.Description, [], option.TargetSeats, new Dictionary<string, string> { ["program-action"] = "use-virtual-ordinary-trick", ["frame-id"] = frame.Id.ToString(CultureInfo.InvariantCulture) });
    [ProgramAnswer("use-virtual-ordinary-trick")]
    internal void ResolveProgramVirtualOrdinaryTrickChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("A virtual ordinary-trick choice lost its owning program.");
        var effect = ValidateVirtualOrdinaryTrickInstruction(frame);
        var actorSeat = _coreProgramCardPaymentsOperations.ResolveProgramParticipant(frame, effect.ActorReference!);
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } decision || decision.PlayerSeat != actorSeat || selected.Parameters.GetValueOrDefault("program-action") != "use-virtual-ordinary-trick" || selected.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(CultureInfo.InvariantCulture))
            throw new InvalidOperationException("A virtual ordinary-trick choice lost its exact actor or instruction.");
        var option = VirtualOrdinaryTrickOptions(frame, effect).SingleOrDefault(option => option.Id == selected.Id);
        if (option is null || !option.TargetSeats.SequenceEqual(selected.Targets))
            throw new InvalidOperationException("A virtual ordinary-trick choice is no longer legal.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        IssueProgramVirtualOrdinaryTrick(frame, effect, option);
    }

    internal void IssueProgramVirtualOrdinaryTrick(ProgramSkillFrame frame, SkillProgramEffect effect, GameEngine.ProgramOrdinaryTrickUseOption option)
    {
        var actorSeat = _coreProgramCardPaymentsOperations.ResolveProgramParticipant(frame, effect.ActorReference!);
        var targetSeat = _coreProgramCardPaymentsOperations.ResolveProgramParticipant(frame, effect.TargetReference!);
        var frozen = effect.Condition.Kind is SkillProgramConditionKind.Always or SkillProgramConditionKind.ChoiceIs ? (bool? )null : frame.PindianResultBindings.Single(item => item.Name == effect.Condition.SourceBind).SourceWon;
        var origin = new ProgramVirtualOrdinaryTrickOrigin(frame.Id, frame.InstructionIndex, frame.WindowContext!.ParentFrameId, frame.OwnerSeat, actorSeat, targetSeat, frame.SkillId, frame.TriggerId!, frame.SkillInstanceId, frame.GameplayHash, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, effect.OutputKind!.Value, effect.Condition.Kind, effect.Condition.SourceBind, frozen);
        var card = new Card(0, effect.OutputKind.Value, Suit.None, 0);
        if (option.NextActualUseAdjusted)
            _coreGameEngineNextCardTargetAdjustmentsSelectedNextCardTargetSeatsAccess._selectedNextCardTargetSeats = option.TargetSeats;
        var targets = effect.OutputKind == CardKind.DrawTwo && option.TargetSeats.Count == 0 ? new[]
        {
            targetSeat
        }

        : option.TargetSeats;
        var id = _coreNativeCardUseOperations.BeginCardUse(card, actorSeat, targets, effect.OutputKind, physicalCardIds: [], virtualOrdinaryTrickOrigin: origin);
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(id, card, actorSeat, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.TargetSeats, option.ActionKind, option.TargetCardId, option.RequiredCardKind, effect.OutputKind);
    }

    [NativeUseValidation(100)]
    internal void ValidateVirtualOrdinaryTrickReturn(CardUseFrame use)
    {
        if (use.VirtualOrdinaryTrickOrigin is not null && !ValidProgramVirtualOrdinaryTrickUse(use))
            throw new InvalidOperationException("A virtual ordinary trick lost its issuance before completion.");
    }

    internal bool ValidProgramVirtualOrdinaryTrickUse(CardUseFrame use)
    {
        if (use.VirtualOrdinaryTrickOrigin?.SkillId == "shanhe:zhaogu-mirror-6018")
            return _coreZhaoguMirrorOperations.Value.IsZhaoguVirtualOrdinaryTrickUse(use);
        if (use.VirtualOrdinaryTrickOrigin?.SkillId == "mobile:shi-xiaoge")
            return _characterZhangYanMobileShiOperations.Value.ValidShiZyVirtualDuelUse(use);
        if (use.VirtualOrdinaryTrickOrigin?.SkillId == "mobile:shi-jixi")
            return _characterDengAiMobileShiOperations.ValidShiDaVirtualSnatchUse(use);
        if (use.VirtualOrdinaryTrickOrigin?.SkillId == "anniversary:zhuning")
            return _characterLiuYongAnniversaryOperations.Value.IsAnniversaryLiuYongVirtualUse(use);
        if (use.VirtualOrdinaryTrickOrigin?.SkillId == "anniversary:dehua")
            return _characterLiuLiAnniversaryOperations.IsAnniversaryLiuLiVirtualUse(use);
        if (use.VirtualOrdinaryTrickOrigin?.SkillId == "anniversary:juee")
            return _characterYangBiaoAnniversaryOperations.IsAnniversaryYangBiaoVirtualUse(use);
        if (use.VirtualOrdinaryTrickOrigin?.SkillId == "happy:zuoxing")
            return _characterGuoJiaHappyGodOperations.Value.IsHappyGuoJiaVirtualUse(use);
        if (use.VirtualOrdinaryTrickOrigin?.SkillId == "mobile:zuoxing")
            return _characterGuoJiaMobileGodOperations.IsShenGuoJiaVirtualUse(use);
        if (use.VirtualOrdinaryTrickOrigin?.SkillId == MouXunYuAnniversaryRuntime.Shimou)
            return _characterXunYuMouOperations.IsMouXyVirtualUse(use);
        if (use.VirtualOrdinaryTrickOrigin is not { } origin || use.CardId != 0 || use.CardKind != origin.OutputKind || use.PhysicalCardIds is not { Count: 0 } || use.SelectedActorDuelOrigin is not null || use.DamageTargetDuelOrigin is not null || use.DualColorDuelOrigin is not null || use.ConditionalDiscardDuelOrigin is not null || use.TieredRoundConversionUse is not null || use.VirtualBasicReturn is not null || use.Action is not { Type: CardActionType.Use, PhysicalCards.Count: 0, ConversionChain.Count: 0, EffectiveSuit: Suit.None, EffectiveRank: 0, EffectiveIsRed: not true, RequesterSeat: null, ResponderSeat: null, OpponentSeat: null } action || action.EffectiveKind != origin.OutputKind || action.ActionId != origin.CardActionId || !action.TargetSeats.SequenceEqual(use.TargetSeats) || use.TargetSeats.Count == 0 && !(_characterXunYuMobileGodOperations.HasDinghanCancellation(use) || _characterHuaManOlOperations.HasHuaManSouyingCancellation(use) || _coreOverflowTargetCancellationOperations.HasOverflowTargetCancellation(use)) || use.TargetSeats.Distinct().Count() != use.TargetSeats.Count || use.TargetSeats.Any(seat => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(seat)) || action.ProviderSeat != origin.InitialActorSeat || origin.TurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(origin.InitialActorSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(origin.InitialTargetSeat))
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(item => item.Id == use.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramSkillFrame frame || frame.Id != origin.ParentProgramFrameId || frame.InstructionIndex != origin.InstructionIndex || frame.OwnerSeat != origin.OwnerSeat || frame.SkillId != origin.SkillId || frame.TriggerId != origin.BindingId || frame.SkillInstanceId != origin.SkillInstanceId || frame.GameplayHash != origin.GameplayHash || frame.WindowContext?.ParentFrameId != origin.LifecycleFrameId || !IsExactOwnVirtualOrdinaryTrickProgram(frame))
            return false;
        var effect = ValidateVirtualOrdinaryTrickInstruction(frame);
        if (effect.OutputKind != origin.OutputKind || effect.Condition.Kind != origin.ConditionKind || effect.Condition.SourceBind != origin.PindianBind || _coreProgramCardPaymentsOperations.ResolveProgramParticipant(frame, effect.ActorReference!) != origin.InitialActorSeat || _coreProgramCardPaymentsOperations.ResolveProgramParticipant(frame, effect.TargetReference!) != origin.InitialTargetSeat || (origin.ConditionKind is SkillProgramConditionKind.Always or SkillProgramConditionKind.ChoiceIs ? origin.FrozenSourceWon is not null : frame.PindianResultBindings.Single(item => item.Name == origin.PindianBind).SourceWon != origin.FrozenSourceWon))
            return false;
        // Provider and issuance remain frozen. The actual source may only move
        // along the existing typed, accepted actor-replacement producer chain.
        return _coreActualTurnDamageClaimsOperations.MatchesDeclaredActualDamageUse(use, action.ActorSeat, origin.InitialActorSeat);
    }

    internal bool IsProgramVirtualOrdinaryTrickUse(long frameId) => _characterDengAiAnniversaryGodOperations.IsShenDaCuixinTrick(frameId) || _characterChengYuMouOlOperations.IsMouDanchiDrawTwo(frameId) || _coreCardUseLifecycleOperations.LifecycleCardUse(frameId)is { } use && ValidProgramVirtualOrdinaryTrickUse(use);
    internal Card ReadVirtualOrdinaryTrickUseAppearance(long ownerId, CardAppearanceReference appearance)
    {
        if (_characterDengAiAnniversaryGodOperations.IsShenDaCuixinUse(ownerId))
            return _characterDengAiAnniversaryGodOperations.ReadShenDaCuixinAppearance(ownerId, appearance);
        if (_characterChengYuMouOlOperations.IsMouDanchiDrawTwo(ownerId))
            return _characterChengYuMouOlOperations.ReadMouDanchiAppearance(ownerId, appearance);
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(ownerId)is not { VirtualOrdinaryTrickOrigin: not null } use)
            return _characterDongChengMouOperations.ReadMouDcUseAppearance(ownerId, appearance);
        if (!ValidProgramVirtualOrdinaryTrickUse(use) || appearance.Id != 0 || appearance.Kind != use.CardKind || appearance.Suit != Suit.None || appearance.Rank != 0)
            throw new InvalidOperationException("A virtual ordinary trick appearance lost its exact issued use.");
        return new Card(0, use.CardKind, Suit.None, 0);
    }
    [ProgramDamageHook(ProgramDamagePhase.IssuedAttackCompletion, 160)]
    internal bool FinishProgramVirtualOrdinaryAttack(NativeOwners.CardAttackHandle attack, out bool paused)
    {
        paused = false;
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { } use || !ValidProgramVirtualOrdinaryTrickUse(use) || use.CardKind is not (CardKind.Duel or CardKind.FireAttack or CardKind.UnexpectedAssault or CardKind.QizhengXiangsheng) || attack.EffectiveCardKind != use.CardKind || attack.PhysicalCards.Count != 0 || _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not { } current || !CoreNativeDamageHandlesRuntime.SameAttackOwner(current, attack))
            return false;
        _coreNativeCardUseFramesOperations.SetCardUseStep(use.Id, ResolutionFrameStep.Completed);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseFinishedEvent(use.Id, 0, use.CardKind));
        if (_coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreCardActionsOperations.TryBeginProgramCardWindow(attack, use.Action!, SkillProgramTriggerWindow.CardUseCompleted, use.TargetSeats, ProgramCardContinuation.CompletedSlash, cardUseCausedDamage: attack.CardUseCausedDamage))
            paused = true;
        else
        {
            _coreNativeCardUseCompletionOperations.PopFinishedCardUse(use.Id);
            _coreNativeCardUseCompletionOperations.CompleteFinishedAttackCardUse(attack);
        }

        return true;
    }

    internal bool MatchesVirtualOrdinaryTrickUseAction(long frameId, CardActionContext action, int cardId) => cardId == 0 && IsProgramVirtualOrdinaryTrickUse(frameId) && _coreCardUseLifecycleOperations.LifecycleCardUse(frameId)?.Action?.ActionId == action.ActionId && action.Type == CardActionType.Use && action.EffectiveKind == _coreCardUseLifecycleOperations.LifecycleCardUse(frameId)!.CardKind && action.PhysicalCards.Count == 0 && action.ConversionChain.Count == 0;
    [NativeUseReturn(210)]
    internal void ReturnProgramVirtualOrdinaryTrick(CardUseFrame use)
    {
        if (use.VirtualOrdinaryTrickOrigin?.SkillId == "shanhe:zhaogu-mirror-6018")
        {
            _coreZhaoguMirrorOperations.Value.ReturnZhaoguVirtualOrdinaryTrickUse(use);
            return;
        }

        if (use.VirtualOrdinaryTrickOrigin is not { } origin)
            return;
        if (_characterZhangYanMobileShiOperations.Value.TryReturnShiZyVirtualDuel(use))
            return;
        if (_characterDengAiMobileShiOperations.TryReturnShiDaVirtualSnatch(use))
            return;
        if (_characterGuoJiaHappyGodOperations.Value.TryReturnHappyGuoJiaVirtualUse(use))
            return;
        if (_characterGuoJiaMobileGodOperations.TryReturnShenGuoJiaVirtualUse(use))
            return;
        if (_characterLiuYongAnniversaryOperations.Value.TryReturnAnniversaryLiuYongVirtualUse(use))
            return;
        if (_characterLiuLiAnniversaryOperations.TryReturnAnniversaryLiuLiVirtualUse(use))
            return;
        if (_characterYangBiaoAnniversaryOperations.TryReturnAnniversaryYangBiaoVirtualUse(use))
            return;
        if (_characterXunYuMouOperations.ReturnMouXyVirtualUse(use))
            return;
        if (_characterLuXunMouMobileOperations.Value.ReturnMouLuXunNativeUse(use))
            return;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame frame || frame.Id != origin.ParentProgramFrameId || frame.InstructionIndex != origin.InstructionIndex || frame.OwnerSeat != origin.OwnerSeat || frame.SkillId != origin.SkillId || frame.TriggerId != origin.BindingId || frame.SkillInstanceId != origin.SkillInstanceId || frame.GameplayHash != origin.GameplayHash || use.Step != ResolutionFrameStep.Completed || frame.WindowContext?.ParentFrameId != origin.LifecycleFrameId || ValidateVirtualOrdinaryTrickInstruction(frame).OutputKind != origin.OutputKind)
            throw new InvalidOperationException("A completed virtual ordinary trick lost its typed program return.");
        if (use.CardKind is CardKind.Duel or CardKind.FireAttack or CardKind.UnexpectedAssault or CardKind.QizhengXiangsheng && use.CardAttack is { Active: true })
        {
            if (_coreProgramEventHistoryOperations.Value.ProgramEventHistory<ProgramVirtualOrdinaryAttackReturnedEvent>().Any(e => e.UseId == use.Id))
                throw new InvalidOperationException("A virtual ordinary attack cannot return twice.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramVirtualOrdinaryAttackReturnedEvent(use.Id, origin));
            return;
        }

        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterCleanup, 44001)]
    internal bool ResumeProgramVirtualOrdinaryAttack()
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame frame || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(frame)?.Op != SkillProgramEffectOp.UseVirtualOrdinaryTrick || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ProgramVirtualOrdinaryAttackReturnedEvent>().LastOrDefault()is not { Origin: { } origin } returned || origin.ParentProgramFrameId != frame.Id || origin.InstructionIndex != frame.InstructionIndex || _resolutionStackOperations._resolutionStack.Any(f => f.Id == returned.UseId) || frame.SkillId != origin.SkillId || frame.SkillInstanceId != origin.SkillInstanceId || ValidateVirtualOrdinaryTrickInstruction(frame).OutputKind != origin.OutputKind)
            return false;
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
        return true;
    }
}
