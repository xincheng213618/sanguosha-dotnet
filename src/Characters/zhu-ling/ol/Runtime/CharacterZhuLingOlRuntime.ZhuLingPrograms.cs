#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhuLingOlRuntime
{
    internal static string ZhanyiCategoryName(SkillProgramCardCategory category) => global::CardGame.Core.RuntimeSkills.RuntimeRules_zhu_ling_ol_ZhuLingPrograms.ZhanyiCategoryName(category);
    internal IReadOnlyList<Card> ZhanyiOwnedCategoryCards(int ownerSeat, SkillProgramCardCategory category) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[ownerSeat]).Concat(_cardZonesOperations._cardZones.CardsAt(CardLocation.Equipment(ownerSeat))).Where(card => global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(card.Kind) == category).OrderBy(card => card.Id).ToArray();
    internal SkillProgramStepOutcome ZhanyiProgramChooseCategory(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var owner = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        if (!owner.IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || _coreActualTurnProgressCurrentSeatPhaseAccess._phase != TurnPhase.Play || owner.Seat != _coreActualTurnProgressCurrentSeatPhaseAccess._currentSeat || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, active.SkillId, active.SkillInstanceId))
            return SkillProgramStepOutcome.Continue;
        var options = new[]
        {
            SkillProgramCardCategory.Basic,
            SkillProgramCardCategory.Trick,
            SkillProgramCardCategory.Equipment
        }.Select(category => (Category: category, Cards: ZhanyiOwnedCategoryCards(owner.Seat, category))).Where(item => item.Cards.Count > 0).OrderBy(item => item.Category).ToArray();
        if (options.Length == 0)
        {
            _coreMatchLogOperations.AddLog("SkillEffect", $"{(owner.Name)} 战意：没有可弃置的牌，技能落空。", owner.Seat);
            return SkillProgramStepOutcome.Continue;
        }

        var presentation = _contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId);
        var choices = options.Select(item => new PromptChoice(new ChoiceId($"zhanyi-launch.frame-{(frame.Id)}.{(item.Category.ToString().ToLowerInvariant())}"), $"弃置所有【{(ZhanyiCategoryName(item.Category))}】（{(item.Cards.Count)} 张）。", item.Cards.Select(card => card.Id).ToArray(), [], new Dictionary<string, string> { ["program-action"] = "zhanyi-launch", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["category"] = item.Category.ToString() })).ToArray();
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, active.OwnerSeat, $"【{(presentation.Name)}】请选择弃置一种类别的所有牌。", choices.SelectMany(choice => choice.Cards).Distinct().ToArray(), [], active.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = false,
            TargetSeat = active.OwnerSeat,
            SkillPrompt = new SkillPromptPresentation(active.SkillId, presentation.Name, $"{(presentation.Name)} · 选择类别", "弃置一种类别的所有牌，直到你的下个回合开始，另外两种类别的牌获得战意加成。"),
            Choices = choices.AsReadOnly()
        };
        _coreMatchOutcomeStatusWinnerAccess._status = owner.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    [ProgramAnswer("zhanyi-launch")]
    internal void ResolveZhanyiCategoryChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("The Zhanyi category choice lost its program frame.");
        var paused = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex);
        if (paused.Effect is not { Op: SkillProgramEffectOp.ZhuLingZhanyiChooseCategory } || selected.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) || !Enum.TryParse(selected.Parameters.GetValueOrDefault("category"), out SkillProgramCardCategory category))
            throw new InvalidOperationException("The Zhanyi category choice does not match its suspended instruction.");
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } decision || decision.PlayerSeat != active.OwnerSeat)
            throw new InvalidOperationException("The Zhanyi category chooser changed while suspended.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var owner = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        var discarded = ZhanyiOwnedCategoryCards(active.OwnerSeat, category);
        if (!owner.IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || discarded.Count == 0)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(active, "战意的弃置类别已无牌可弃，剩余结算取消。");
            return;
        }

        var reason = new CardMoveReason($"skill-program.{(active.SkillId)}.launch");
        var handDiscards = discarded.Where(card => _cardZonesOperations._cardZones.GetLocation(card.Id).Zone == CardZoneKind.Hand).ToArray();
        var equipmentDiscards = discarded.Except(handDiscards).ToArray();
        var logicalDiscard = _coreNativeLogicalDiscardsOperations.CaptureNativeLogicalDiscardInvocation(active.OwnerSeat, active.OwnerSeat, discarded.Select(card => card.Id).ToArray(), reason, active.Id);
        if (handDiscards.Length > 0)
            _corePhysicalMovementFacadesOperations.MoveCards(handDiscards, CardLocation.Hand(active.OwnerSeat), CardLocation.DiscardPile, reason);
        if (equipmentDiscards.Length > 0)
            _corePhysicalMovementFacadesOperations.MoveCards(equipmentDiscards, CardLocation.Equipment(active.OwnerSeat), CardLocation.DiscardPile, reason);
        _coreNativeLogicalDiscardsOperations.CompleteNativeLogicalDiscardInvocation(logicalDiscard);
        if (category != SkillProgramCardCategory.Basic)
        {
            _coreCardUseModulesOperations.GrantProgramTurnCardDamageModifier(active, global::CardGame.Core.EngineOwners.CharacterZhuLingOlDefinitions.ZhanyiBasicDamageKinds, 1, SkillProgramDamageModifierExpiration.NextOwnerTurnStart, SkillProgramDamageModifierSourceScope.OwnerUsed);
            _coreCardUseModulesOperations.GrantProgramTurnRuleModifier(active, SkillRuleQuery.CardUseDistanceLimit, SkillRuleOperation.Unlimited, 0, global::CardGame.Core.EngineOwners.CharacterZhuLingOlDefinitions.ZhanyiBasicDamageKinds, expiration: paused.Effect.CardUseBenefitPolicy?.Expiration ?? SkillProgramDamageModifierExpiration.CurrentTurnEnd);
            if (paused.Effect.CardUseBenefitPolicy is { RecoveryBonus: > 0 } policy)
                _coreCardRecoveryModifiersOperations.GrantProgramCardRecoveryModifier(active, [CardKind.Peach, CardKind.Alcohol], policy.RecoveryBonus, policy.Expiration);
        }

        if (category != SkillProgramCardCategory.Trick)
            _coreTurnHandLimitKindExemptionsOperations.GrantProgramTurnHandLimitCardKindExemption(active, global::CardGame.Core.EngineOwners.CharacterZhuLingOlDefinitions.ZhanyiTrickKinds, paused.Effect.CardUseBenefitPolicy?.Expiration ?? SkillProgramDamageModifierExpiration.CurrentTurnEnd);
        _coreProgramSkillChangesOperations.SetProgramBooleanState(active, "zhanyi-basic", category != SkillProgramCardCategory.Basic);
        _coreProgramSkillChangesOperations.SetProgramBooleanState(active, "zhanyi-trick", category != SkillProgramCardCategory.Trick);
        _coreProgramSkillChangesOperations.SetProgramBooleanState(active, "zhanyi-equipment", category != SkillProgramCardCategory.Equipment);
        if (paused.Effect.CardUseBenefitPolicy?.ExpireBooleanStatesAtOwnerTurnStart == true)
            _coreProgramBooleanStateExpirationOperations.ScheduleProgramBooleanStateExpiration(active, ["zhanyi-basic", "zhanyi-trick", "zhanyi-equipment"], SkillProgramDamageModifierExpiration.NextOwnerTurnStart);
        _coreMatchLogOperations.AddLog("SkillEffect", $"{(owner.Name)} 战意：弃置 {(discarded.Count)} 张【{(ZhanyiCategoryName(category))}】，另外两种类别的牌直到下个回合开始获得加成。", active.OwnerSeat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramZhanyiCategoryChosenEvent(active.Id, active.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), active.OwnerSeat, category.ToString(), Array.AsReadOnly(discarded.Select(card => card.Id).ToArray())));
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
    }

    internal SkillProgramStepOutcome ZhanyiProgramEquipmentPunish(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var owner = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        if (active.WindowContext is not { Window: SkillProgramTriggerWindow.CardsGained, MovementBatch: { } batch, MovementIndex: { } index } || index < 0 || index >= batch.Movements.Count || batch.Movements[index].To != CardLocation.Equipment(active.OwnerSeat) || batch.Movements[index].From == batch.Movements[index].To)
            throw new InvalidOperationException("Zhanyi equipment punishment lost its exact actual equipment entry.");
        if (!owner.IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, active.SkillId, active.SkillInstanceId) || !_coreProgramPersistentStateOperations.GetProgramBooleanState(active, "zhanyi-equipment"))
            return SkillProgramStepOutcome.Continue;
        var candidates = _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != active.OwnerSeat && (_corePhysicalOwnedCardQueriesOperations.GetHand(player).Count > 0 || _cardZonesOperations._cardZones.CardsAt(CardLocation.Equipment(player.Seat)).Any(card => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(active.OwnerSeat, card, CardLocation.Equipment(player.Seat), OwnedCardMoveIntent.Discard)))).Select(player => player.Seat).ToArray();
        if (candidates.Length == 0)
        {
            _coreMatchLogOperations.AddLog("SkillEffect", $"{(owner.Name)} 战意：没有可弃牌的其他角色，装备牌的战意加成落空。", active.OwnerSeat);
            return SkillProgramStepOutcome.Continue;
        }

        var presentation = _contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId);
        var choices = candidates.Select(seat => new PromptChoice(new ChoiceId($"zhanyi-punish-target.frame-{(frame.Id)}.seat-{(seat)}"), $"选择 {(_coreMatchParticipantsPlayersAccess._players[seat].Name)}。", [], [seat], new Dictionary<string, string> { ["program-action"] = "zhanyi-punish-target", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["target"] = seat.ToString(System.Globalization.CultureInfo.InvariantCulture) })).ToArray();
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, active.OwnerSeat, $"【{(presentation.Name)}】请选择一名其他角色弃置其一张牌。", [], candidates, active.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = false,
            TargetSeat = active.OwnerSeat,
            SkillPrompt = new SkillPromptPresentation(active.SkillId, presentation.Name, $"{(presentation.Name)} · 选择角色", "装备牌置入你的装备区时，令一名其他角色弃置一张牌。"),
            Choices = choices.AsReadOnly()
        };
        _coreMatchOutcomeStatusWinnerAccess._status = owner.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    [ProgramAnswer("zhanyi-punish-target")]
    internal void ResolveZhanyiPunishTargetChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("The Zhanyi punish target choice lost its program frame.");
        var paused = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex);
        if (paused.Effect is not { Op: SkillProgramEffectOp.ZhuLingZhanyiEquipmentPunish } || selected.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) || !int.TryParse(selected.Parameters.GetValueOrDefault("target"), System.Globalization.CultureInfo.InvariantCulture, out var target))
            throw new InvalidOperationException("The Zhanyi punish target choice does not match its suspended instruction.");
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } decision || decision.PlayerSeat != active.OwnerSeat)
            throw new InvalidOperationException("The Zhanyi punish chooser changed while suspended.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || target == active.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].IsAlive)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(active, "战意的弃牌目标已失效，剩余结算取消。");
            return;
        }

        var owner = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        var victim = _coreMatchParticipantsPlayersAccess._players[target];
        var handCount = _corePhysicalOwnedCardQueriesOperations.GetHand(victim).Count;
        var equipment = _cardZonesOperations._cardZones.CardsAt(CardLocation.Equipment(target)).Where(card => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(active.OwnerSeat, card, CardLocation.Equipment(target), OwnedCardMoveIntent.Discard)).OrderBy(card => card.Id).ToArray();
        var choices = new List<PromptChoice>();
        foreach (var card in equipment)
            choices.Add(new PromptChoice(new ChoiceId($"zhanyi-punish-card.frame-{(frame.Id)}.card-{(card.Id)}"), $"弃置 {(_coreMatchParticipantsPlayersAccess._players[target].Name)} 的【{(card.DisplayName)}】。", [card.Id], [target], new Dictionary<string, string> { ["program-action"] = "zhanyi-punish-card", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["target"] = target.ToString(System.Globalization.CultureInfo.InvariantCulture), ["card-id"] = card.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) }));
        for (var slot = 0; slot < handCount; slot++)
            choices.Add(new PromptChoice(new ChoiceId($"zhanyi-punish-card.frame-{(frame.Id)}.hand-{(slot)}"), $"弃置 {(_coreMatchParticipantsPlayersAccess._players[target].Name)} 的一张暗牌（牌位 {(slot + 1)}）。", [], [target], new Dictionary<string, string> { ["program-action"] = "zhanyi-punish-card", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["target"] = target.ToString(System.Globalization.CultureInfo.InvariantCulture), ["hand-slot"] = slot.ToString(System.Globalization.CultureInfo.InvariantCulture) }));
        if (choices.Count == 0)
        {
            _coreMatchLogOperations.AddLog("SkillEffect", $"{(owner.Name)} 战意：{(_coreMatchParticipantsPlayersAccess._players[target].Name)} 没有可弃置的牌，剩余结算取消。", active.OwnerSeat);
            _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
            return;
        }

        var presentation = _contentRegistryOperations._contentRegistry.GetSkill(active.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, active.OwnerSeat, $"【{(presentation.Name)}】请选择 {(_coreMatchParticipantsPlayersAccess._players[target].Name)} 的一张牌弃置。", choices.SelectMany(choice => choice.Cards).Distinct().ToArray(), [target], active.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = target,
            SkillPrompt = new SkillPromptPresentation(active.SkillId, presentation.Name, $"{(presentation.Name)} · 选择牌", "令其弃置一张牌；暗牌按牌位盲选。"),
            Choices = choices.AsReadOnly()
        };
        _coreMatchOutcomeStatusWinnerAccess._status = owner.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("zhanyi-punish-card")]
    internal void ResolveZhanyiPunishCardChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("The Zhanyi punish card choice lost its program frame.");
        var paused = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex);
        if (paused.Effect is not { Op: SkillProgramEffectOp.ZhuLingZhanyiEquipmentPunish } || selected.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) || !int.TryParse(selected.Parameters.GetValueOrDefault("target"), System.Globalization.CultureInfo.InvariantCulture, out var target))
            throw new InvalidOperationException("The Zhanyi punish card choice does not match its suspended instruction.");
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } decision || decision.PlayerSeat != active.OwnerSeat)
            throw new InvalidOperationException("The Zhanyi punish chooser changed while suspended.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var owner = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || target == active.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !owner.IsAlive)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(active, "战意的弃牌目标已失效，剩余结算取消。");
            return;
        }

        var reason = new CardMoveReason($"skill-program.{(active.SkillId)}.equipment-punish");
        Card card;
        var fromHand = selected.Parameters.ContainsKey("hand-slot");
        if (fromHand)
        {
            var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).OrderBy(item => item.Id).ToArray();
            if (!int.TryParse(selected.Parameters.GetValueOrDefault("hand-slot"), System.Globalization.CultureInfo.InvariantCulture, out var slot) || slot < 0 || slot >= hand.Length)
            {
                _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(active, "战意的暗牌牌位已失效，剩余结算取消。");
                return;
            }

            card = hand[slot];
        }
        else
        {
            var cardId = selected.Parameters.TryGetValue("card-id", out var cardText) && int.TryParse(cardText, System.Globalization.CultureInfo.InvariantCulture, out var parsed) ? parsed : -1;
            if (!_cardZonesOperations._cardZones.CardsAt(CardLocation.Equipment(target)).Any(item => item.Id == cardId))
            {
                _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(active, "战意的装备牌已离开原处，剩余结算取消。");
                return;
            }

            card = _cardZonesOperations._cardZones.CardsAt(CardLocation.Equipment(target)).Single(item => item.Id == cardId);
        }

        var logicalDiscard = _coreNativeLogicalDiscardsOperations.CaptureNativeLogicalDiscardInvocation(active.OwnerSeat, active.OwnerSeat, [card.Id], reason, active.Id);
        _corePhysicalMovementFacadesOperations.MoveCards([card], _cardZonesOperations._cardZones.GetLocation(card.Id), CardLocation.DiscardPile, reason, movement => _characterBianFuRenOlOperations.RecordBianFuRenForeignDiscardProducer(movement, active.OwnerSeat, active.Id));
        _coreNativeLogicalDiscardsOperations.CompleteNativeLogicalDiscardInvocation(logicalDiscard);
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(owner.Name)} 发动【{(_contentRegistryOperations._contentRegistry.GetSkill(active.SkillId).Name)}】，弃置 {(_coreMatchParticipantsPlayersAccess._players[target].Name)} 的【{(card.DisplayName)}】。", active.OwnerSeat, target);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramZhanyiEquipmentPunishEvent(active.Id, active.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), active.OwnerSeat, target, card.Id, fromHand));
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
    }

    SkillProgramStepOutcome global::CardGame.Core.IZhuLingProgramHost.ZhuLingZhanyiChooseCategory(ProgramSkillFrame frame, SkillProgramEffect effect) => ZhanyiProgramChooseCategory(frame, effect);
    SkillProgramStepOutcome global::CardGame.Core.IZhuLingProgramHost.ZhuLingZhanyiEquipmentPunish(ProgramSkillFrame frame, SkillProgramEffect effect) => ZhanyiProgramEquipmentPunish(frame, effect);
}
