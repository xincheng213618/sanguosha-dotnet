#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineCompletedCardGiftsRuntime
{
    internal const string CompletedGiftFactionResultBind = "completed-card-gift-slash";
    internal CardActionContext CompletedGiftAction(ProgramSkillFrame frame)
    {
        var context = frame.WindowContext;
        if (context is not { Window: SkillProgramTriggerWindow.CardUseCompleted, CardUse: { } use } || use.ActorSeat != frame.OwnerSeat)
            throw new InvalidOperationException("A completed-card gift requires the owner's completed use.");
        return _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(frame => frame.Id == use.ParentCardUseFrameId && frame.Action?.ActionId == use.CardActionId).Action!;
    }

    internal IReadOnlyList<CardActionContext> CompletedGiftSources(ProgramSkillFrame frame)
    {
        var use = CompletedGiftAction(frame);
        var sources = new List<CardActionContext>
        {
            use
        };
        sources.AddRange(_coreCommandJournalStorageEventsPendingEventsAccess._events.Select(envelope => envelope.Payload).Concat(_coreCommandJournalStorageEventsPendingEventsAccess._pendingEvents).OfType<CardActionAcceptedEvent>().Select(item => item.Action).Where(response => response.Type == CardActionType.Response && response.ParentActionId == use.ActionId && response.EffectiveKind == CardKind.Dodge && response.ResponderSeat is { } responder && use.EffectiveDesignatedTargetSeats.Contains(responder)));
        return sources.Where(action => action.PhysicalCards.Count > 0 && action.PhysicalCards.All(cost => _cardZonesOperations._cardZones.GetLocation(cost.CardId)is { Zone: CardZoneKind.Processing or CardZoneKind.DiscardPile })).ToArray();
    }

    internal int[] CompletedGiftSlashTargets(ProgramSkillFrame frame, int actorSeat) => _coreMatchParticipantsPlayersAccess._players.Where(target => _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[actorSeat].IsAlive && target.IsAlive && target.Seat != actorSeat && target.Seat != frame.OwnerSeat && _coreProgramDodgeResponsesOperations.IsWithinAttackRange(frame.OwnerSeat, target.Seat)).Select(target => target.Seat).ToArray();
    internal SkillProgramStepOutcome OfferProgramCompletedCardGift(ProgramSkillFrame frame)
    {
        var action = CompletedGiftAction(frame);
        if (frame.CompletedCardGiftDraft is not null)
            throw new InvalidOperationException("A completed gift was offered twice.");
        if (!global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind) || CompletedGiftSources(frame).Count == 0 || !_coreMatchParticipantsPlayersAccess._players.Any(player => player.IsAlive && player.Seat != frame.OwnerSeat))
            return SkillProgramStepOutcome.Continue;
        frame = frame with
        {
            CompletedCardGiftDraft = new(action.ActionId)
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame);
        PublishCompletedCardGiftPrompt(frame);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> CompletedCardGiftChoices(ProgramSkillFrame frame)
    {
        var draft = frame.CompletedCardGiftDraft ?? throw new InvalidOperationException("Completed gift lost its draft.");
        Dictionary<string, string> Parameters(string option) => new()
        {
            ["program-action"] = "completed-card-gift",
            ["gift-option"] = option,
            ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture)
        };
        var choices = new List<PromptChoice>();
        if (draft.RecipientSeat is null)
        {
            foreach (var source in CompletedGiftSources(frame))
                foreach (var recipient in _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != frame.OwnerSeat))
                    choices.Add(new(new ChoiceId($"completed-gift.{(frame.Id)}.{(source.ActionId)}.{(recipient.Seat)}"), $"交给 {(recipient.Name)}【{(CardCatalog.Get(source.EffectiveKind).DisplayName)}】", source.PhysicalCards.Select(card => card.CardId).ToArray(), [recipient.Seat], Parameters("give")));
        }
        else
        {
            var actor = _coreMatchParticipantsPlayersAccess._players[draft.RecipientSeat.Value];
            if (_coreProgramFactionPoliciesOperations.GetFactionResponsePolicy(actor, CardKind.Slash)is { } policy)
                foreach (var target in CompletedGiftSlashTargets(frame, actor.Seat))
                {
                    var parameters = Parameters("faction");
                    parameters["skill"] = policy.SkillId;
                    choices.Add(new(new ChoiceId($"completed-gift.{(frame.Id)}.faction.{(target)}"), $"请求同势力角色提供杀，对 {(_coreMatchParticipantsPlayersAccess._players[target].Name)} 使用", [], [target], parameters));
                }

            foreach (var target in CompletedGiftSlashTargets(frame, actor.Seat))
                foreach (var card in _corePhysicalOwnedCardQueriesOperations.GetPlayableCards(actor).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(actor)).Concat(_coreOwnPublicPileConversionsOperations.GetOwnPublicPileConversionCards(actor)).DistinctBy(card => card.Id))
                    foreach (var kind in global::CardGame.Core.EngineOwners.CoreGameEngineDirectedCardPolicyQueriesDefinitions.SlashKinds)
                        foreach (var conversion in _coreActionQueriesOperations.GetProgramViewAsConversions(actor, card, kind, forResponse: false))
                        {
                            if (_coreCardUseModulesOperations.IsTurnHandCardRestricted(actor, card) || !_coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, _coreMatchParticipantsPlayersAccess._players[target], card, conversion, kind, ignoreDistance: true))
                                continue;
                            var parameters = Parameters("slash");
                            parameters["effective-kind"] = kind.ToString();
                            global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime.AddConversionParameters(parameters, conversion);
                            choices.Add(new(new ChoiceId($"completed-gift.{(frame.Id)}.typed.{(kind)}.{(conversion.SkillId)}.{(conversion.BindingId)}.{(card.Id)}.{(target)}"), $"将牌当【{(CardCatalog.Get(kind).DisplayName)}】对 {(_coreMatchParticipantsPlayersAccess._players[target].Name)} 使用", [card.Id], [target], parameters));
                        }

            foreach (var target in CompletedGiftSlashTargets(frame, actor.Seat))
                foreach (var pair in _coreNativeResponseMaterialQueriesOperations.GetZhangbaSlashPairs(actor))
                {
                    if (!_coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, _coreMatchParticipantsPlayersAccess._players[target], pair[0], effectiveKind: CardKind.Slash, ignoreDistance: true))
                        continue;
                    var parameters = Parameters("slash");
                    parameters["effective-kind"] = CardKind.Slash.ToString();
                    parameters["equipment"] = CardKind.ZhangbaSerpentSpear.ToString();
                    var ids = pair.Select(card => card.Id).ToArray();
                    choices.Add(new(new ChoiceId($"completed-gift.{(frame.Id)}.zhangba.{(string.Join('-', ids))}.{(target)}"), $"使用丈八蛇矛对 {(_coreMatchParticipantsPlayersAccess._players[target].Name)} 使用杀", ids, [target], parameters));
                }

            foreach (var target in CompletedGiftSlashTargets(frame, actor.Seat))
                foreach (var card in _coreActionQueriesOperations.GetSlashUseCards(actor))
                {
                    var identity = _coreActionQueriesOperations.GetProgramCardIdentityMatches(actor, card).FirstOrDefault(item => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(item.Identity.OutputKind));
                    var baseKind = identity?.Identity.OutputKind ?? _coreOwnPublicPileConversionsOperations.PhysicalSlashUseBaseKind(actor, card);
                    foreach (var kind in _coreNativeTargetLegalityOperations.GetSlashUseKinds(actor, baseKind))
                    {
                        if (!_coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, _coreMatchParticipantsPlayersAccess._players[target], card, effectiveKind: kind, ignoreDistance: true))
                            continue;
                        var parameters = Parameters("slash");
                        parameters["effective-kind"] = kind.ToString();
                        choices.AddRange(_coreCardConversionsOperations.CreateConversionChoiceVariants(actor, card, kind, false, $"completed-gift.{(frame.Id)}.slash.{(card.Id)}.{(kind)}.{(target)}", $"对 {(_coreMatchParticipantsPlayersAccess._players[target].Name)} 使用【{(CardCatalog.Get(kind).DisplayName)}】", [card.Id], [target], parameters));
                    }
                }

            foreach (var target in CompletedGiftSlashTargets(frame, actor.Seat))
                foreach (var kind in global::CardGame.Core.EngineOwners.CoreGameEngineDirectedCardPolicyQueriesDefinitions.SlashKinds)
                    foreach (var selection in _coreCardConversionsOperations.GetProgramMultiCardViewAsSelections(actor, kind, false).Where(item => _corePlayCardHistoryOperations.ViewAsRule(item.Source)is { RoundDistinctBasicUse: null, LeiluanBasicUse: null } || item.OutputKind == kind))
                    {
                        if (!_coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, _coreMatchParticipantsPlayersAccess._players[target], selection.Cards[0], selection.Source, kind, ignoreDistance: true))
                            continue;
                        var parameters = Parameters("slash");
                        parameters["effective-kind"] = kind.ToString();
                        global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime.AddConversionParameters(parameters, selection.Source);
                        var ids = selection.Cards.Select(card => card.Id).ToArray();
                        choices.Add(new(new ChoiceId($"completed-gift.{(frame.Id)}.multi.{(selection.Source.SkillId)}.{(selection.Source.BindingId)}.{(string.Join('-', ids))}.{(target)}"), $"对 {(_coreMatchParticipantsPlayersAccess._players[target].Name)} 使用【{(CardCatalog.Get(kind).DisplayName)}】", ids, [target], parameters));
                    }
        }

        choices.Add(new(new ChoiceId($"completed-gift.{(frame.Id)}.decline"), draft.RecipientSeat is null ? "不交出牌" : "不使用杀", [], [], Parameters("decline")));
        return choices.Where(c => draft.RecipientSeat is not { } actorSeat || c.Parameters.GetValueOrDefault("gift-option") != "slash" || !_coreWuJingQueriesOperations.IsTurnSlashChoiceUseForbidden(actorSeat, c)).ToArray();
    }

    internal void PublishCompletedCardGiftPrompt(ProgramSkillFrame frame)
    {
        var choices = CompletedCardGiftChoices(frame);
        var chooser = frame.CompletedCardGiftDraft!.RecipientSeat ?? frame.OwnerSeat;
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, chooser == frame.OwnerSeat ? "将本次杀或目标响应的闪交给一名其他角色。" : "可以对技能拥有者攻击范围内的角色使用一张杀。", choices.SelectMany(choice => choice.Cards).Distinct().ToArray(), choices.SelectMany(choice => choice.Targets).Distinct().ToArray(), frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = chooser,
            Choices = choices,
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("completed-card-gift")]
    internal void ResolveCompletedCardGiftChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Completed gift lost its frame.");
        var draft = frame.CompletedCardGiftDraft ?? throw new InvalidOperationException("Completed gift lost its draft.");
        selected = CompletedCardGiftChoices(frame).SingleOrDefault(choice => choice.Id == selected.Id) ?? throw new InvalidOperationException("Completed gift option is no longer available.");
        var instruction = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
        if (instruction.Op != SkillProgramEffectOp.OfferCompletedCardGift || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != (draft.RecipientSeat ?? frame.OwnerSeat))
            throw new InvalidOperationException("Completed gift lost its frozen instruction or chooser.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (selected.Parameters["gift-option"] == "decline" || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId))
        {
            CompleteProgramCardGift(frame);
            return;
        }

        if (draft.RecipientSeat is null)
        {
            var recipient = selected.Targets.Single();
            var cards = selected.Cards.Select(id =>
            {
                var from = _cardZonesOperations._cardZones.GetLocation(id);
                return _cardZonesOperations._cardZones.CardsAt(from).Single(card => card.Id == id);
            }).ToArray();
            var red = cards.All(card => card.Suit is Suit.Heart or Suit.Diamond);
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(cards.Select(card => card.Id).ToArray(), CardLocation.Hand(recipient), new($"skill-program.{(frame.SkillId)}.completed-card-gift"));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CompletedCardGiftedEvent(frame.Id, draft.CardActionId, frame.OwnerSeat, recipient, selected.Cards, red));
            if (!red || !_coreMatchParticipantsPlayersAccess._players[recipient].IsAlive)
            {
                CompleteProgramCardGift(frame);
                return;
            }

            frame = frame with
            {
                CompletedCardGiftDraft = draft with
                {
                    RecipientSeat = recipient,
                    GiftWasRed = true
                },
                SelectedTargetSeats = [recipient]
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(frame);
            PublishCompletedCardGiftPrompt(frame);
            return;
        }

        var actor = _coreMatchParticipantsPlayersAccess._players[draft.RecipientSeat.Value];
        var target = selected.Targets.Single();
        if (selected.Parameters["gift-option"] == "faction")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { CompletedCardGiftDraft = draft with { RequestTargetSeat = target } });
            _coreAssistedFactionSlashOperations.BeginAssistedProgramFactionSlashRequest(frame, actor.Seat, target, CompletedGiftFactionResultBind);
            return;
        }

        var kind = Enum.Parse<CardKind>(selected.Parameters["effective-kind"]);
        global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime.TryReadConversionSource(selected.Parameters, out var conversion);
        IReadOnlyList<Card> paid = selected.Cards.Count == 1 && !_characterKongShuOlOperations.IsLeiluanSource(conversion) ? [_corePhysicalOwnedCardQueriesOperations.GetPlayableCards(actor).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(actor)).Concat(_coreOwnPublicPileConversionsOperations.GetOwnPublicPileConversionCards(actor)).Single(card => card.Id == selected.Cards[0])] : selected.Parameters.GetValueOrDefault("equipment") == CardKind.ZhangbaSerpentSpear.ToString() ? _coreNativeResponseMaterialQueriesOperations.GetZhangbaSlashPairs(actor).Single(pair => pair.Select(card => card.Id).SequenceEqual(selected.Cards)) : _coreCardConversionsOperations.FindProgramMultiCardViewAsSelection(actor, selected.Cards, kind, false, conversion!)?.Cards ?? throw new InvalidOperationException("Completed gift Slash lost its physical payment.");
        if (!CompletedGiftSlashTargets(frame, actor.Seat).Contains(target) || !_coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, _coreMatchParticipantsPlayersAccess._players[target], paid[0], conversion, kind, ignoreDistance: true))
            throw new InvalidOperationException("Completed gift Slash target is no longer legal.");
        _coreNativeSlashUseOperations.ResolveSlashCore(actor, _coreMatchParticipantsPlayersAccess._players[target], paid[0], kind, actor.Seat, physicalCards: paid, countsTowardSlashLimit: false, usesZhuqueFan: kind == CardKind.FireSlash && (paid[0].Kind == CardKind.Slash || _coreOwnPublicPileConversionsOperations.IsOwnPublicPileSlashMaterial(actor, paid[0])) && _coreNativeTargetLegalityOperations.HasZhuqueFan(actor), conversionSource: conversion, programSkillCardUseFrameId: frame.Id);
    }

    internal void CompleteProgramCardGift(ProgramSkillFrame frame)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { CompletedCardGiftDraft = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    SkillProgramStepOutcome global::CardGame.Core.ICompletedCardGiftProgramHost.OfferCompletedCardGift(ProgramSkillFrame frame) => OfferProgramCompletedCardGift(frame);
}
