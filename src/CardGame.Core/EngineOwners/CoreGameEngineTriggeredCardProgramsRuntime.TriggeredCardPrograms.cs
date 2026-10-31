#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineTriggeredCardProgramsRuntime
{
    internal bool IsProgramDiscardedDelayedTrickTarget(int ownerSeat, CharacterState target, ProgramSkillWindowContext? context)
    {
        if (context is not { Window: SkillProgramTriggerWindow.DiscardPileReceived, MovementBatch: { } batch, MovementIndex: { } index } || index < 0 || index >= batch.Movements.Count || !_coreMatchParticipantsPlayersAccess._players[ownerSeat].IsAlive || !target.IsAlive || target.Seat == ownerSeat || target.JudgmentAreaAbolished || _coreActualTurnsOperations.HasJudgmentEffectiveCard(target, CardKind.SupplyShortage) || _coreCardUseModulesOperations.IsCardUseForbidden(ownerSeat, CardKind.SupplyShortage, CardActionType.Use) || _coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(ownerSeat, target.Seat, CardKind.SupplyShortage))
            return false;
        var movement = batch.Movements[index];
        if (_coreProgramDiscardOriginsOperations.GetProgramDiscardSource(movement)?.OwnerSeat != ownerSeat || _cardZonesOperations._cardZones.GetLocation(movement.CardId) != CardLocation.DiscardPile)
            return false;
        var card = _cardZonesOperations._cardZones.CardsAt(CardLocation.DiscardPile).Single(item => item.Id == movement.CardId);
        return card.Suit is Suit.Spade or Suit.Club && ProgramCardSetFilter.Matches(card.Kind, card.Suit, [], [SkillProgramCardCategory.Basic], [], []) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(target, CardKind.SupplyShortage, card.Suit, CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[ownerSeat], card)));
    }

    internal SkillProgramStepOutcome UseTriggeredProgramVirtualSlash(ProgramSkillFrame frame, int targetSeat)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(targetSeat) || !_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[targetSeat].IsAlive || targetSeat == active.OwnerSeat || _coreCardUseModulesOperations.IsCardUseForbidden(active.OwnerSeat, CardKind.Slash, CardActionType.Use) || _coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(active.OwnerSeat, targetSeat, CardKind.Slash) || _coreNativeDamagePoliciesOperations.IsSlashProhibited(_coreMatchParticipantsPlayersAccess._players[targetSeat]))
            return SkillProgramStepOutcome.Continue;
        if (_coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not null || _coreNativeCardContinuationCursorsOperations.ActiveDuel is not null)
            throw new InvalidOperationException("A triggered virtual Slash cannot replace a pending attack.");
        var source = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[targetSeat];
        var resolutionId = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        var provenance = new CardConversionSource(active.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), active.OwnerSeat, active.SkillInstanceId);
        var action = _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(++_coreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence, _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault()?.Action?.ActionId, CardActionType.Use, source.Seat, source.Seat, null, null, null, CardKind.Slash, [targetSeat], [], [provenance], effectiveSuit: Suit.None, effectiveRank: 0));
        _coreRuntimeOperations.PushRuntimeFrame(new CardUseFrame(resolutionId, source.Seat, 0, CardKind.Slash, [targetSeat], PhysicalCardIds: []) { Action = action });
        _characterMengDaOlOperations.IssueGoudeSlashUse(resolutionId, active, action);
        _coreShanheS5ZhulingOperations.IssueShanheS5JixianSlashUse(resolutionId, active, action);
        _characterYangFuOlOperations.IssueSuyiSlashUse(resolutionId, active, action);
        if (_corePlayCardHistoryOperations.TracksPlayCardHistory)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseAppearanceCapturedEvent(action));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseDeclaredEvent(resolutionId, 0, CardKind.Slash, source.Seat));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TargetsConfirmedEvent(resolutionId, [targetSeat]));
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(resolutionId, source.Seat, targetSeat, card: null, damageAmount: 1 + source.AlcoholDamageBonus, playedCardKind: CardKind.Slash, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(source, target, CardKind.Slash), programSkillCardUseFrameId: active.Id);
        _coreAlcoholSlashSuppressionsOperations.CaptureProgramAlcoholConsumption(resolutionId, source);
        source.HasAlcoholEffect = false;
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        _corePhaseNamePredictionOperations.ObservePhaseNamePredictionUse(action);
        if (_corePhaseStartControlPoliciesOperations.HasPhaseStartCardUseParticipant(source.Seat))
            _coreActualPlayPhaseCardUsesOperations.RecordActualPlayPhaseUse(action);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, CardKind.Slash, source.Seat, targetSeat));
        _coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(resolutionId);
        if (!_coreCardActionsOperations.TryBeginProgramCardWindow(attack, action, SkillProgramTriggerWindow.CardUseCommitted, action.TargetSeats, ProgramCardContinuation.CommittedSlash))
            _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal SkillProgramStepOutcome UseProgramDiscardedCardAsDelayedTrick(ProgramSkillFrame frame, int targetSeat, CardKind kind)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (kind != CardKind.SupplyShortage || active.WindowContext is not { Window: SkillProgramTriggerWindow.DiscardPileReceived, MovementBatch: { } batch, MovementIndex: { } index } || index < 0 || index >= batch.Movements.Count)
            throw new InvalidOperationException("A discarded-card conversion lost its frozen discard occurrence.");
        var movement = batch.Movements[index];
        if (_coreProgramDiscardOriginsOperations.GetProgramDiscardSource(movement)?.OwnerSeat != active.OwnerSeat || movement.To != CardLocation.DiscardPile || movement.From == movement.To)
            throw new InvalidOperationException("A discarded-card conversion requires the owner's actual discarded card.");
        if (_cardZonesOperations._cardZones.GetLocation(movement.CardId) != CardLocation.DiscardPile || !_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].IsAlive)
            return SkillProgramStepOutcome.Continue;
        var card = _cardZonesOperations._cardZones.CardsAt(CardLocation.DiscardPile).Single(item => item.Id == movement.CardId);
        if (CoreNativeCardClassificationsRuntime.IsRedSuit(card.Suit) || card.Suit == Suit.None || !ProgramCardSetFilter.Matches(card.Kind, card.Suit, [], [SkillProgramCardCategory.Basic], [], []))
            throw new InvalidOperationException("The frozen discarded card is not a black basic card.");
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(targetSeat) || !_coreMatchParticipantsPlayersAccess._players[targetSeat].IsAlive || targetSeat == active.OwnerSeat || _coreMatchParticipantsPlayersAccess._players[targetSeat].JudgmentAreaAbolished || _coreActualTurnsOperations.HasJudgmentEffectiveCard(_coreMatchParticipantsPlayersAccess._players[targetSeat], kind) || _coreCardUseModulesOperations.IsCardUseForbidden(active.OwnerSeat, kind, CardActionType.Use) || _coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(active.OwnerSeat, targetSeat, kind) || _coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[targetSeat], kind, card.Suit, CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat], card))))
            return SkillProgramStepOutcome.Continue;
        var source = new CardConversionSource(active.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), active.OwnerSeat, active.SkillInstanceId);
        var resolutionId = _coreNativeCardUseOperations.BeginCardUse(card, active.OwnerSeat, [targetSeat], kind, conversionSource: source);
        _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.DiscardPile, CardLocation.Processing, CardMoveReasons.Use);
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(resolutionId, card, active.OwnerSeat, [targetSeat], LegalActionKind.SupplyShortage, playedCardKind: kind);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal Card[] EquipmentColorDiscardCandidates(int ownerSeat)
    {
        var owner = _coreMatchParticipantsPlayersAccess._players[ownerSeat];
        var colors = _cardZonesOperations._cardZones.CardsAt(CardLocation.Equipment(ownerSeat)).Select(card => CoreNativeCardClassificationsRuntime.IsRedSuit(_coreNativeCardAppearanceOperations.EffectiveSuit(owner, card))).Distinct().ToArray();
        return _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[ownerSeat]).Concat(_cardZonesOperations._cardZones.CardsAt(CardLocation.Equipment(ownerSeat))).Where(card => !ProgramCardSetFilter.Matches(card.Kind, card.Suit, [], [SkillProgramCardCategory.Basic], [], []) && _coreNativeCardAppearanceOperations.EffectiveSuit(owner, card) != Suit.None && colors.Contains(CoreNativeCardClassificationsRuntime.IsRedSuit(_coreNativeCardAppearanceOperations.EffectiveSuit(owner, card)))).OrderBy(card => card.Id).ToArray();
    }

    internal SkillProgramStepOutcome PayProgramEquipmentColorDiscard(ProgramSkillFrame frame, string resultBind)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (active.CardSetBindings.Any(binding => binding.Name == resultBind))
            return SkillProgramStepOutcome.Continue;
        var candidates = EquipmentColorDiscardCandidates(active.OwnerSeat);
        if (candidates.Length == 0)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(active, "没有可支付的与装备颜色相同的非基本牌。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        var choices = candidates.Select(card => new PromptChoice(new ChoiceId($"equipment-color-discard.frame-{(active.Id)}.card-{(card.Id)}"), $"弃置 {(card.DisplayName)}", [card.Id], [], new Dictionary<string, string> { ["program-action"] = "equipment-color-discard", ["frame-id"] = active.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["result-bind"] = resultBind })).ToArray();
        var skill = _contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, active.OwnerSeat, "弃置一张与装备区任意牌颜色相同的非基本牌。", candidates.Select(card => card.Id).ToArray(), [], active.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = Array.AsReadOnly(choices),
            SkillPrompt = new SkillPromptPresentation(active.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void AssertProgramEquipmentColorDiscardChoice(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        if (effect.Op != SkillProgramEffectOp.PayEquipmentColorDiscard || !ReferenceEquals(frame, _resolutionStackOperations._resolutionStack.LastOrDefault()))
            return;
        var eligible = EquipmentColorDiscardCandidates(frame.OwnerSeat).Select(card => card.Id).ToHashSet();
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } decision || decision.PlayerSeat != frame.OwnerSeat || decision.Choices.Count == 0 || decision.Choices.Any(choice => choice.Cards.Count != 1 || !eligible.Contains(choice.Cards[0]) || choice.Targets.Count != 0 || choice.Parameters.GetValueOrDefault("program-action") != "equipment-color-discard" || choice.Parameters.GetValueOrDefault("result-bind") != effect.ResultBind || choice.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture)))
            throw new InvalidOperationException("An equipment-color discard prompt changed its owner, candidates or binding.");
    }

    [ProgramAnswer("equipment-color-discard")]
    internal void ResolveProgramEquipmentColorDiscardChoice(PromptChoice choice)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("A discard payment lost its program frame.");
        var effect = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
        AssertProgramEquipmentColorDiscardChoice(frame, effect);
        if (effect.Op != SkillProgramEffectOp.PayEquipmentColorDiscard || choice.Cards.Count != 1 || choice.Targets.Count != 0 || choice.Parameters.GetValueOrDefault("result-bind") != effect.ResultBind || choice.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture))
            throw new InvalidOperationException("The discard payment changed its frozen instruction.");
        var card = EquipmentColorDiscardCandidates(frame.OwnerSeat).SingleOrDefault(card => card.Id == choice.Cards[0]) ?? throw new InvalidOperationException("The payment card is no longer eligible.");
        var location = _cardZonesOperations._cardZones.GetLocation(card.Id);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreProgramLifecycleOperations.SetProgramCardSet(frame.Id, effect.ResultBind!, [card.Id], SkillProgramCardSetVisibility.Public, [location], _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], card));
        _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(frame.OwnerSeat, frame.OwnerSeat, card, location, new CardMoveReason($"skill-program.{(frame.SkillId)}.{(effect.Op)}"));
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    SkillProgramStepOutcome ITriggeredCardProgramHost.UseTriggeredVirtualSlash(ProgramSkillFrame frame, int targetSeat) => UseTriggeredProgramVirtualSlash(frame, targetSeat);
    SkillProgramStepOutcome ITriggeredCardProgramHost.UseDiscardedCardAsDelayedTrick(ProgramSkillFrame frame, int targetSeat, CardKind kind) => UseProgramDiscardedCardAsDelayedTrick(frame, targetSeat, kind);
    SkillProgramStepOutcome ITriggeredCardProgramHost.PayEquipmentColorDiscard(ProgramSkillFrame frame, string resultBind) => PayProgramEquipmentColorDiscard(frame, resultBind);
    void ITriggeredCardProgramHost.GrantPlayPhaseColorRestriction(ProgramSkillFrame frame, string sourceBind, int targetSeat) => _coreProgramPhaseColorRestrictionsOperations.GrantProgramPlayPhaseColorRestriction(frame, sourceBind, targetSeat);
}
