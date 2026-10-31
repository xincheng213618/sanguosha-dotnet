#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterJiangGanOlRuntime
{
    // 盗书 stays offerable only while some other living character still holds a
    // hand card; the activation target kind re-checks the same set.
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 100, SkillProgramEffectOp.DaoshuGuessAndTake)]
    internal bool CanStartDaoshuGuess(CharacterState owner) => _coreMatchParticipantsPlayersAccess._players.Any(player => player.IsAlive && player.Seat != owner.Seat && _corePhysicalOwnedCardQueriesOperations.GetHand(player).Count > 0);
    internal SkillProgramStepOutcome DaoshuProgramGuessAndTake(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (active.SelectedTargetSeats is not [var target] || target == active.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).Count == 0)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(active, "盗书的目标已失效或没有手牌，技能结算取消。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (!_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].IsAlive || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(active))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(active, "盗书的技能拥有者已失效，技能结算取消。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        PresentDaoshuSuitPrompt(active);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void PresentDaoshuSuitPrompt(ProgramSkillFrame frame)
    {
        var presentation = _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId);
        var suits = new[]
        {
            Suit.Spade,
            Suit.Heart,
            Suit.Club,
            Suit.Diamond
        };
        var choices = suits.Select(suit => new PromptChoice(new ChoiceId($"daoshu-suit.frame-{(frame.Id)}.{(suit)}"), $"选择{(CoreNativeCardClassificationsRuntime.GetSuitDisplayName(suit))}。", [], [frame.SelectedTargetSeats is [var target] ? target : frame.OwnerSeat], new Dictionary<string, string> { ["program-action"] = "daoshu-suit", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["suit"] = suit.ToString() })).ToArray();
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, frame.OwnerSeat, $"【{(presentation.Name)}】请选择一种花色。", [], [], frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = false,
            TargetSeat = frame.OwnerSeat,
            SkillPrompt = new SkillPromptPresentation(frame.SkillId, presentation.Name, $"{(presentation.Name)} · 选择花色", "选择一种花色并获得该角色的一张手牌：花色相同，你对其造成1点伤害且此技能视为未发动过；花色不同，你交给其一张其他花色的手牌（若没有需展示所有手牌）。"),
            Choices = Array.AsReadOnly(choices)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("daoshu-suit")]
    internal void ResolveDaoshuSuitChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("The Daoshu suit choice lost its program frame.");
        var paused = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex);
        if (paused.Effect is not { Op: SkillProgramEffectOp.DaoshuGuessAndTake } || selected.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture))
            throw new InvalidOperationException("The Daoshu suit choice does not match its suspended instruction.");
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (active.SelectedTargetSeats is not [var target] || !Enum.TryParse<Suit>(selected.Parameters.GetValueOrDefault("suit"), out var chosenSuit))
            throw new InvalidOperationException("The Daoshu suit choice lost its target or suit.");
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } decision || decision.PlayerSeat != frame.OwnerSeat)
            throw new InvalidOperationException("The Daoshu guesser changed while suspended.");
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        if (!owner.IsAlive || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(frame))
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(active, "盗书的技能拥有者已失效，剩余结算取消。");
            return;
        }

        var targetState = _coreMatchParticipantsPlayersAccess._players[target];
        if (!targetState.IsAlive || _corePhysicalOwnedCardQueriesOperations.GetHand(targetState).Count == 0)
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(active, "盗书的目标已失效或没有手牌，剩余结算取消。");
            return;
        }

        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        // The take rides the established blind-hand privacy shape: the concealed
        // entity hops through the processing zone and only seats stay public.
        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(targetState);
        var taken = hand[_randomOperations._random.Next(hand.Count)];
        _corePhysicalMovementFacadesOperations.MoveCard(taken, CardLocation.Hand(target), CardLocation.Processing, new($"skill-program.{(frame.SkillId)}.daoshu-take"), movement => _characterBianFuRenOlOperations.RecordBianFuRenOwnedTransferProducer(movement, owner.Seat, frame.Id));
        _corePhysicalMovementFacadesOperations.MoveCard(taken, CardLocation.Processing, CardLocation.Hand(owner.Seat), new($"skill-program.{(frame.SkillId)}.daoshu-take"));
        var takenSuit = _coreProgramCardPoliciesOperations.GetProgramEffectiveSuit(owner, taken);
        _coreMatchLogOperations.AddLog("SkillEffect", $"{(owner.Name)} 发动【盗书】，选择{(CoreNativeCardClassificationsRuntime.GetSuitDisplayName(chosenSuit))}，获得 {(targetState.Name)} 的一张手牌。", owner.Seat, target);
        if (takenSuit == chosenSuit)
        {
            // Correct guess: the activation is refunded so the skill may
            // fire again this phase, then the damage settles through the program
            // damage pipeline which resumes and completes this frame afterwards.
            var plan = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!);
            var usageKey = (frame.OwnerSeat, frame.SkillId, plan.Activation?.UsageGroup ?? frame.ActivationId);
            if (_coreGameEngineSkillProgramsProgramPhaseUsesProgramUsesAccess._programUses.TryGetValue(usageKey, out var used) && used > 0)
                _coreGameEngineSkillProgramsProgramPhaseUsesProgramUsesAccess._programUses[usageKey] = used - 1;
            if (_coreGameEngineSkillProgramsProgramPhaseUsesProgramUsesAccess._programPhaseUses.TryGetValue(usageKey, out var phaseUsed) && phaseUsed > 0)
                _coreGameEngineSkillProgramsProgramPhaseUsesProgramUsesAccess._programPhaseUses[usageKey] = phaseUsed - 1;
            _coreMatchLogOperations.AddLog("SkillEffect", $"{(owner.Name)} 的【盗书】猜中花色，对 {(targetState.Name)} 造成1点伤害，此技能视为未发动过。", owner.Seat, target);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramDaoshuEvent(active.Id, active.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), owner.Seat, target, chosenSuit, true, true, null, Array.Empty<int>()));
            _coreSkillProgramsOperations.BeginProgramSkillDamage(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), target, 1);
            return;
        }

        // Wrong guess: one hand card of a suit other than the taken card's suit
        // goes back; without one the whole hand is shown instead.
        var eligible = _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Where(card => card.Id != taken.Id && _coreProgramCardPoliciesOperations.GetProgramEffectiveSuit(owner, card) != takenSuit).ToArray();
        if (eligible.Length == 0)
        {
            var handCards = _corePhysicalOwnedCardQueriesOperations.GetHand(owner).ToArray();
            _coreProgramLifecycleOperations.SetProgramCardSet(frame.Id, "daoshu-reveal", handCards.Select(card => card.Id).ToArray(), SkillProgramCardSetVisibility.Private, handCards.Select(card => _cardZonesOperations._cardZones.GetLocation(card.Id)).ToArray());
            _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)with { DaoshuGuess = null });
            _coreProgramCategoryChallengesOperations.RevealProgramBoundCards(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), "daoshu-reveal");
            _coreMatchLogOperations.AddLog("SkillEffect", $"{(owner.Name)} 的【盗书】未猜中花色，没有其他花色的手牌，展示了所有手牌。", owner.Seat, target);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramDaoshuEvent(active.Id, active.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), owner.Seat, target, chosenSuit, true, false, null, Array.AsReadOnly(handCards.Select(card => card.Id).ToArray())));
            if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
                _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(active with { DaoshuGuess = new DaoshuGuessState(target, chosenSuit, taken.Id, takenSuit, true) });
        PresentDaoshuGiveBackPrompt(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), eligible);
    }

    internal void PresentDaoshuGiveBackPrompt(ProgramSkillFrame frame, Card[] eligible)
    {
        var state = frame.DaoshuGuess ?? throw new InvalidOperationException("The Daoshu give-back lost its guess state.");
        var presentation = _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId);
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        var choices = eligible.Select(card => new PromptChoice(new ChoiceId($"daoshu-give.frame-{(frame.Id)}.card-{(card.Id)}"), $"交给 {(_coreMatchParticipantsPlayersAccess._players[state.TargetSeat].Name)} 【{(card.DisplayName)}】。", [card.Id], [state.TargetSeat], new Dictionary<string, string> { ["program-action"] = "daoshu-give-back", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["card-id"] = card.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) })).ToArray();
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, frame.OwnerSeat, $"【{(presentation.Name)}】请交给 {(_coreMatchParticipantsPlayersAccess._players[state.TargetSeat].Name)} 一张其他花色的手牌。", eligible.Select(card => card.Id).ToArray(), [state.TargetSeat], frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = frame.OwnerSeat,
            SkillPrompt = new SkillPromptPresentation(frame.SkillId, presentation.Name, $"{(presentation.Name)} · 交还手牌", "交给该角色一张与其所获牌花色不同的手牌，否则需展示所有手牌。"),
            Choices = Array.AsReadOnly(choices)
        };
        _coreMatchOutcomeStatusAccess._status = owner.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("daoshu-give-back")]
    internal void ResolveDaoshuGiveBackChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("The Daoshu give-back choice lost its program frame.");
        var paused = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex);
        if (paused.Effect is not { Op: SkillProgramEffectOp.DaoshuGuessAndTake } || selected.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture))
            throw new InvalidOperationException("The Daoshu give-back choice does not match its suspended instruction.");
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var state = active.DaoshuGuess is { GiveBackPending: true } guess ? guess : throw new InvalidOperationException("The Daoshu give-back choice lost its guess state.");
        if (!int.TryParse(selected.Parameters.GetValueOrDefault("card-id"), System.Globalization.CultureInfo.InvariantCulture, out var cardId))
            throw new InvalidOperationException("The Daoshu give-back choice carries no card.");
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } decision || decision.PlayerSeat != frame.OwnerSeat)
            throw new InvalidOperationException("The Daoshu giver changed while suspended.");
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[state.TargetSeat];
        if (!owner.IsAlive || !target.IsAlive || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(frame) || _cardZonesOperations._cardZones.GetLocation(cardId) != CardLocation.Hand(owner.Seat))
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(active, "盗书的参与者或所选手牌已失效，剩余结算取消。");
            return;
        }

        var card = _cardZonesOperations._cardZones.CardsAt(CardLocation.Hand(owner.Seat)).Single(item => item.Id == cardId);
        if (_coreProgramCardPoliciesOperations.GetProgramEffectiveSuit(owner, card) == state.TakenEffectiveSuit)
            throw new InvalidOperationException("The Daoshu give-back card must differ from the taken card's suit.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)with { DaoshuGuess = null });
        _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Hand(owner.Seat), CardLocation.Hand(state.TargetSeat), new($"skill-program.{(frame.SkillId)}.daoshu-give"), _characterHuangQuanMouOperations.NativeGiveCompletion(frame.Id, owner.Seat, state.TargetSeat, [card.Id], _coreCardMovementJournalMovementSequenceAccess._movementSequence, null));
        _coreMatchLogOperations.AddLog("SkillEffect", $"{(owner.Name)} 的【盗书】未猜中花色，交给 {(target.Name)} 【{(card.DisplayName)}】。", owner.Seat, state.TargetSeat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramDaoshuEvent(active.Id, active.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), owner.Seat, state.TargetSeat, state.ChosenSuit, true, false, card.Id, Array.Empty<int>()));
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
    }

    SkillProgramStepOutcome IJiangGanProgramHost.DaoshuGuessAndTake(ProgramSkillFrame frame, SkillProgramEffect effect) => DaoshuProgramGuessAndTake(frame, effect);
}
