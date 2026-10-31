#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineAssistedPhysicalCardsRuntime
{

    internal static bool AssistedChoicesEqual(IReadOnlyList<PromptChoice> actual, IReadOnlyList<PromptChoice> expected) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_AssistedPhysicalCards.AssistedChoicesEqual(actual, expected);
    internal int[] AssistedPhysicalSlashTargets(int actorSeat) => _coreMatchParticipantsPlayersAccess._players.Where(target => _coreMatchParticipantsPlayersAccess._players[actorSeat].IsAlive && target.IsAlive && target.Seat != actorSeat && (_characterGuanYuWumiaoOperations.HasPublicWuyouContributionPotential(_coreMatchParticipantsPlayersAccess._players[actorSeat]) || _characterXiaoQiaoLeOperations.HasWeiwanUseDistance(_coreMatchParticipantsPlayersAccess._players[actorSeat], target, CardKind.Slash) || (_characterCaiWenJiLeOperations.HasBeifenPreUsePrivilege(_coreMatchParticipantsPlayersAccess._players[actorSeat]) || _characterHuangGaiMouMobileOperations.HasMouZhaJiangPreUsePrivilege(_coreMatchParticipantsPlayersAccess._players[actorSeat])) || _coreProvenanceCardPoliciesOperations.HasPotentialProvenanceSlash(_coreMatchParticipantsPlayersAccess._players[actorSeat]) || _coreProgramDodgeResponsesOperations.IsWithinAttackRange(actorSeat, target.Seat) || _coreFinalTargetSlashOperations.HasPotentialRankSlashRange(_coreMatchParticipantsPlayersAccess._players[actorSeat], target, CardKind.Slash) || _coreLiYiNativeQueriesOperations.HasLiYiProspectiveWeaponSlashTarget(_coreMatchParticipantsPlayersAccess._players[actorSeat], target) || global::CardGame.Core.EngineOwners.CoreGameEngineDirectedCardPolicyQueriesDefinitions.SlashKinds.Any(k => _characterMiHengLeOperations.HasSiruiSlashDistance(_coreMatchParticipantsPlayersAccess._players[actorSeat], k)))).Select(target => target.Seat).ToArray();
    internal SkillProgramStepOutcome RequestProgramSlashAgainstChosenTarget(ProgramSkillFrame frame, int actorSeat, string bind, bool actorChoosesTarget)
    {
        if (_characterJiangWeiBoundaryMobileOperations.TryBeginMobileJiangWeiTiaoxin(frame, actorSeat, bind, actorChoosesTarget, out var mobileOutcome))
            return mobileOutcome;
        if (_characterPeiYuanShaoOlOperations.TryBeginFulueAssistedSlash(frame, actorSeat, bind, out var fulueOutcome))
            return fulueOutcome;
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (active.AssistedSlashRequest is not null || active.SelectedTargetSeats.Count != 1 || active.SelectedTargetSeats[0] != actorSeat || actorSeat == active.OwnerSeat)
            throw new InvalidOperationException("An assisted Slash requires one distinct selected actor.");
        if (!_coreMatchParticipantsPlayersAccess._players[actorSeat].IsAlive || _characterFaZhengBoundaryMobileOperations.FaZhengMobileAssistedTargets(active, actorSeat).Length == 0)
        {
            _coreProgramSlashRequestOperations.CommitProgramChoiceResult(active.Id, bind, "declined", actorSeat, "没有合法目标。");
            return SkillProgramStepOutcome.Continue;
        }

        active = active with
        {
            AssistedSlashRequest = new(actorSeat, ActorChoosesTarget: actorChoosesTarget)
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(active);
        PublishAssistedPhysicalSlashPrompt(active);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> AssistedPhysicalSlashChoices(ProgramSkillFrame frame)
    {
        var draft = frame.AssistedSlashRequest ?? throw new InvalidOperationException("Missing assisted Slash draft.");
        var actor = _coreMatchParticipantsPlayersAccess._players[draft.ActorSeat];
        Dictionary<string, string> Parameters(string option) => new()
        {
            ["program-action"] = "assisted-physical-slash",
            ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture),
            ["request-option"] = option
        };
        if (draft.TargetSeat is null)
            return _characterFaZhengBoundaryMobileOperations.FaZhengMobileAssistedTargets(frame, actor.Seat).Select(seat => new PromptChoice(new ChoiceId($"assisted-slash.{(frame.Id)}.target-{(seat)}"), $"指定 {(_coreMatchParticipantsPlayersAccess._players[seat].Name)} 为【杀】的目标", [], [seat], Parameters("target"))).ToArray();
        var targetSeat = draft.TargetSeat.Value;
        var choices = new List<PromptChoice>();
        foreach (var card in _coreActionQueriesOperations.GetSlashUseCards(actor))
        {
            var identity = _coreActionQueriesOperations.GetProgramCardIdentityMatches(actor, card).FirstOrDefault(item => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(item.Identity.OutputKind));
            var baseKind = identity?.Identity.OutputKind ?? _coreOwnPublicPileConversionsOperations.PhysicalSlashUseBaseKind(actor, card);
            foreach (var kind in _coreNativeTargetLegalityOperations.GetSlashUseKinds(actor, baseKind))
            {
                if (!AssistedSlashPaymentHasRange(actor, targetSeat, [card]) || !_coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, _coreMatchParticipantsPlayersAccess._players[targetSeat], card, effectiveKind: kind))
                    continue;
                var parameters = Parameters("use");
                parameters["effective-kind"] = kind.ToString();
                choices.AddRange(_coreCardConversionsOperations.CreateConversionChoiceVariants(actor, card, kind, false, $"assisted-slash.{(frame.Id)}.card-{(card.Id)}.{(kind)}", $"使用【{(CardCatalog.Get(kind).DisplayName)}】", [card.Id], [targetSeat], parameters));
            }
        }

        foreach (var card in _corePhysicalOwnedCardQueriesOperations.GetPlayableCards(actor).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(actor)).Concat(_coreOwnPublicPileConversionsOperations.GetOwnPublicPileConversionCards(actor)).DistinctBy(card => card.Id))
            foreach (var kind in global::CardGame.Core.EngineOwners.CoreGameEngineDirectedCardPolicyQueriesDefinitions.SlashKinds.Where(kind => kind != CardKind.Slash))
                foreach (var conversion in _coreActionQueriesOperations.GetProgramViewAsConversions(actor, card, kind, forResponse: false))
                {
                    if (_coreCardUseModulesOperations.IsTurnHandCardRestricted(actor, card) || !AssistedSlashPaymentHasRange(actor, targetSeat, [card], conversion) || !_coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, _coreMatchParticipantsPlayersAccess._players[targetSeat], card, conversion, kind))
                        continue;
                    var parameters = Parameters("use");
                    parameters["effective-kind"] = kind.ToString();
                    global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime.AddConversionParameters(parameters, conversion);
                    choices.Add(new PromptChoice(new ChoiceId($"assisted-slash.{(frame.Id)}.typed.{(kind)}.{(conversion.SkillId)}.{(conversion.BindingId)}.{(card.Id)}"), $"将牌当【{(CardCatalog.Get(kind).DisplayName)}】使用", [card.Id], [targetSeat], parameters));
                }

        foreach (var pair in _coreNativeResponseMaterialQueriesOperations.GetZhangbaSlashPairs(actor))
        {
            if (!_coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, _coreMatchParticipantsPlayersAccess._players[targetSeat], pair[0], noEffectiveRank: true, specificEffectiveRank: _coreFinalTargetSlashOperations.ZhangbaSpecificSlashRank(actor, pair), physicalCardIds: pair.Select(c => c.Id).ToArray()))
                continue;
            var parameters = Parameters("use");
            parameters["effective-kind"] = CardKind.Slash.ToString();
            parameters["equipment"] = CardKind.ZhangbaSerpentSpear.ToString();
            var ids = pair.Select(card => card.Id).ToArray();
            choices.Add(new PromptChoice(new ChoiceId($"assisted-slash.{(frame.Id)}.zhangba.{(string.Join('-', ids))}"), "使用丈八蛇矛：将两张手牌当【杀】使用", ids, [targetSeat], parameters));
        }

        foreach (var kind in global::CardGame.Core.EngineOwners.CoreGameEngineDirectedCardPolicyQueriesDefinitions.SlashKinds)
            foreach (var selection in _coreCardConversionsOperations.GetProgramMultiCardViewAsSelections(actor, kind, false).Where(item => _corePlayCardHistoryOperations.ViewAsRule(item.Source)is { RoundDistinctBasicUse: null, LeiluanBasicUse: null } || item.OutputKind == kind))
            {
                if (!AssistedSlashPaymentHasRange(actor, targetSeat, selection.Cards) || !_coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, _coreMatchParticipantsPlayersAccess._players[targetSeat], selection.Cards[0], selection.Source, kind, noEffectiveRank: selection.Cards.Count > 1, physicalCardIds: selection.Cards.Select(c => c.Id).ToArray()))
                    continue;
                var parameters = Parameters("use");
                parameters["effective-kind"] = kind.ToString();
                global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime.AddConversionParameters(parameters, selection.Source);
                var ids = selection.Cards.Select(card => card.Id).ToArray();
                choices.Add(new PromptChoice(new ChoiceId($"assisted-slash.{(frame.Id)}.multi.{(selection.Source.SkillId)}.{(selection.Source.BindingId)}.{(string.Join('-', ids))}"), "将所选牌当【杀】使用", ids, [targetSeat], parameters));
            }

        var factionPolicy = _coreProgramFactionPoliciesOperations.GetFactionResponsePolicy(actor, CardKind.Slash);
        if (factionPolicy is not null && _coreAssistedFactionSlashOperations.CanRequestAssistedProgramFactionSlash(actor.Seat, targetSeat))
        {
            var parameters = Parameters("faction");
            parameters["skill"] = factionPolicy.SkillId;
            choices.Add(new PromptChoice(new ChoiceId($"assisted-slash.{(frame.Id)}.faction"), "发动技能请求其他角色提供【杀】", [], [targetSeat], parameters));
        }

        choices.Add(new PromptChoice(new ChoiceId($"assisted-slash.{(frame.Id)}.decline"), "不使用【杀】", [], [], Parameters("decline")));
        return choices.Where(c => c.Parameters.GetValueOrDefault("request-option") != "use" || !_coreWuJingQueriesOperations.IsTurnSlashChoiceUseForbidden(actor.Seat, c) && !_coreProgramCardLegalityOperations.IsProgramMaterialUseForbidden(actor.Seat, Enum.Parse<CardKind>(c.Parameters["effective-kind"]), c.Cards)).ToArray();
    }

    internal bool AssistedSlashPaymentHasRange(CharacterState actor, int targetSeat, IReadOnlyList<Card> cards, CardConversionSource? conversionSource = null) => _characterHuangfuSongWumiaoOperations.HasWuHfOrWuyouMaterialPrivilege(actor, CardKind.Slash, cards.Select(c => c.Id).ToArray()) || _characterXiaoQiaoLeOperations.HasWeiwanUseDistance(actor, _coreMatchParticipantsPlayersAccess._players[targetSeat], CardKind.Slash) || (_characterCaiWenJiLeOperations.HasBeifenPreUsePrivilege(actor) || _characterHuangGaiMouMobileOperations.HasMouZhaJiangPreUsePrivilege(actor)) || global::CardGame.Core.EngineOwners.CoreGameEngineDirectedCardPolicyQueriesDefinitions.SlashKinds.Any(k => _characterMiHengLeOperations.HasSiruiSlashDistance(actor, k, cards.Select(c => c.Id).ToArray(), conversionSource)) || _characterSunYuanOlOperations.HasSayingRoundRange(actor.Seat, targetSeat) || (_coreProvenanceCardPoliciesOperations.HasProvenanceUseDistance2(actor, cards.Select(c => c.Id).ToArray()) || _coreGrantedEntityPhasesOperations.HasGrantedPhaseEntityDistance(actor, cards.Select(c => c.Id).ToArray())) || cards.Count == 1 && _coreFinalTargetSlashOperations.HasRankSlashRange(actor, CardKind.Slash) && cards[0].Rank > 0 || !cards.Any(card => _cardZonesOperations._cardZones.GetLocation(card.Id) == CardLocation.Equipment(actor.Seat) && _coreFieldEquipmentOperations.FieldEquipmentSlot(card) == EquipmentSlot.Weapon) || _coreLiYiNativeQueriesOperations.HasLiYiWeaponPaymentRange(actor, targetSeat, cards) || _coreNativeRuleQueriesOperations.GetCombatDistance(actor.Seat, targetSeat) <= 1;

    internal void PublishAssistedPhysicalSlashPrompt(ProgramSkillFrame frame)
    {
        var draft = frame.AssistedSlashRequest!;
        var chooserSeat = draft.TargetSeat is null && !draft.ActorChoosesTarget ? frame.OwnerSeat : draft.ActorSeat;
        var nativeChoices = AssistedPhysicalSlashChoices(frame);
        var choices = draft.TargetSeat is { } target ? Array.AsReadOnly(nativeChoices.Where(choice => choice.Parameters.GetValueOrDefault("request-option") != "decline").Concat(_coreRoundOnceCardlessProgramRequestsOperations.RoundOnceCardlessProgramSlashChoices(frame, draft.ActorSeat, [target], _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(frame)!.ResultBind).Concat(_characterMaXiuMaTieOlOperations.KenshangProgramSlashChoices(frame, draft.ActorSeat, [target], _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(frame)!.ResultBind)).Concat(_characterChengJiChengCuiOlOperations.TousuiProgramSlashChoices(frame, draft.ActorSeat, [target], _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(frame)!.ResultBind))).Concat(nativeChoices.Where(choice => choice.Parameters.GetValueOrDefault("request-option") == "decline")).ToArray()) : nativeChoices;
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, chooserSeat, draft.TargetSeat is null ? "请选择受令角色攻击范围内的目标。" : "使用一张【杀】，或允许技能拥有者获得你的牌。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), draft.TargetSeat is null ? _characterFaZhengBoundaryMobileOperations.FaZhengMobileAssistedTargets(frame, draft.ActorSeat) : [draft.TargetSeat.Value], frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = chooserSeat,
            Choices = Array.AsReadOnly(choices.ToArray()),
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[chooserSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("assisted-physical-slash")]
    internal void ResolveAssistedPhysicalSlashChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Assisted Slash frame disappeared.");
        selected = AssistedPhysicalSlashChoices(frame).SingleOrDefault(choice => choice.Id == selected.Id) ?? throw new InvalidOperationException("The assisted Slash option is no longer available.");
        var effect = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
        var draft = frame.AssistedSlashRequest ?? throw new InvalidOperationException("Assisted Slash draft disappeared.");
        if (effect.Op != SkillProgramEffectOp.RequestSlashAgainstChosenTarget || selected.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != (draft.TargetSeat is null && !draft.ActorChoosesTarget ? frame.OwnerSeat : draft.ActorSeat) || !AssistedPhysicalSlashChoices(frame).Any(choice => choice.Id == selected.Id))
            throw new InvalidOperationException("Assisted Slash choice changed its frozen instruction or participants.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (!_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[draft.ActorSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId) && !_characterPeiYuanShaoOlOperations.HasIssuedFulueRequest(frame))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "受令角色或技能实例已失效。");
            return;
        }

        if (draft.TargetSeat is null)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { AssistedSlashRequest = draft with { TargetSeat = selected.Targets.Single() } });
            PublishAssistedPhysicalSlashPrompt((ProgramSkillFrame)_resolutionStackOperations._resolutionStack[^1]);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { AssistedSlashRequest = null });
        if (selected.Parameters.GetValueOrDefault("request-option") == "decline")
        {
            _coreProgramSlashRequestOperations.CommitProgramChoiceResult(frame.Id, effect.ResultBind!, "declined", draft.ActorSeat, "不使用【杀】。");
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
            return;
        }

        if (selected.Parameters.GetValueOrDefault("request-option") == "faction")
        {
            _coreAssistedFactionSlashOperations.BeginAssistedProgramFactionSlashRequest(frame, draft.ActorSeat, draft.TargetSeat.Value, effect.ResultBind!);
            return;
        }

        var actor = _coreMatchParticipantsPlayersAccess._players[draft.ActorSeat];
        var(cards, kind, conversion) = _coreAssistedSlashPaymentOperations.ReadAssistedSlashPayment(actor, selected);
        if (!_characterFaZhengBoundaryMobileOperations.FaZhengMobileAssistedTargets(frame, actor.Seat).Contains(draft.TargetSeat.Value) || !AssistedSlashPaymentHasRange(actor, draft.TargetSeat.Value, cards, conversion) || !_coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, _coreMatchParticipantsPlayersAccess._players[draft.TargetSeat.Value], cards[0], conversion, kind, noEffectiveRank: cards.Count > 1, specificEffectiveRank: selected.Parameters.GetValueOrDefault("equipment") == CardKind.ZhangbaSerpentSpear.ToString() ? _coreFinalTargetSlashOperations.ZhangbaSpecificSlashRank(actor, cards) : null, physicalCardIds: cards.Select(c => c.Id).ToArray()))
            throw new InvalidOperationException("The assisted Slash target became illegal.");
        _coreProgramSlashRequestOperations.CommitProgramChoiceResult(frame.Id, effect.ResultBind!, "used-slash", actor.Seat, "使用【杀】。");
        _coreNativeSlashUseOperations.ResolveSlashCore(actor, _coreMatchParticipantsPlayersAccess._players[draft.TargetSeat.Value], cards[0], kind, actor.Seat, physicalCards: cards, countsTowardSlashLimit: false, usesZhuqueFan: kind == CardKind.FireSlash && (cards[0].Kind == CardKind.Slash || _coreOwnPublicPileConversionsOperations.IsOwnPublicPileSlashMaterial(actor, cards[0])) && _coreNativeTargetLegalityOperations.HasZhuqueFan(actor), conversionSource: conversion, programSkillCardUseFrameId: frame.Id);
    }

    internal SkillProgramStepOutcome TakeProgramSelectedTargetCards(ProgramSkillFrame frame, int sourceSeat, int count)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (sourceSeat == active.OwnerSeat || active.SelectedTargetSeats.Single() != sourceSeat || active.OtherCardSelection is not null || count is < 1 or > 2)
            throw new InvalidOperationException("Other-owned card selection requires a distinct selected source.");
        var available = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[sourceSeat]).Count + _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[sourceSeat]).Count;
        if (!_coreMatchParticipantsPlayersAccess._players[sourceSeat].IsAlive || available == 0)
            return SkillProgramStepOutcome.Continue;
        active = active with
        {
            OtherCardSelection = new(sourceSeat, Math.Min(count, available), [])
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(active);
        PublishOtherCardSelection(active);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> OtherCardSelectionChoices(ProgramSkillFrame frame)
    {
        var draft = frame.OtherCardSelection!;
        return _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(frame.Id, frame.OwnerSeat, draft.SourceSeat, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Obtain).Where(choice => !draft.SelectedSlots.Contains(new(Enum.Parse<CardZoneKind>(choice.Parameters["source-zone"]), int.Parse(choice.Parameters["slot-index"])))).Select(choice => choice with { Id = new ChoiceId($"take-other.{(frame.Id)}.{(draft.SelectedSlots.Count)}.{(choice.Id.Value)}"), Parameters = new Dictionary<string, string>(choice.Parameters) { ["program-action"] = "take-other-card", ["selection-index"] = draft.SelectedSlots.Count.ToString(System.Globalization.CultureInfo.InvariantCulture) } }).ToArray();
    }

    internal void PublishOtherCardSelection(ProgramSkillFrame frame)
    {
        var choices = OtherCardSelectionChoices(frame);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, frame.OwnerSeat, "请选择要获得的手牌或装备牌，选齐后同时移动。", choices.SelectMany(choice => choice.Cards).Distinct().ToArray(), [], frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = frame.SelectedTargetSeats.Single(),
            Choices = Array.AsReadOnly(choices.ToArray()),
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("take-other-card")]
    internal void ResolveOtherCardSelectionChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Other-card selection lost its frame.");
        selected = OtherCardSelectionChoices(frame).SingleOrDefault(choice => choice.Id == selected.Id) ?? throw new InvalidOperationException("The other-owned card slot is no longer available.");
        var effect = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
        var draft = frame.OtherCardSelection ?? throw new InvalidOperationException("Other-card selection lost its draft.");
        if (effect.Op != SkillProgramEffectOp.TakeSelectedTargetCards || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != frame.OwnerSeat || !OtherCardSelectionChoices(frame).Any(choice => choice.Id == selected.Id))
            throw new InvalidOperationException("Other-card selection changed its frozen source or slot.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (!_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[draft.SourceSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "获得牌的参与者或技能实例已失效。");
            return;
        }

        var slots = draft.SelectedSlots.Append(new ProgramOtherCardSlot(Enum.Parse<CardZoneKind>(selected.Parameters["source-zone"]), int.Parse(selected.Parameters["slot-index"]))).ToArray();
        if (slots.Length < draft.RequiredCount)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { OtherCardSelection = draft with { SelectedSlots = slots } });
            PublishOtherCardSelection((ProgramSkillFrame)_resolutionStackOperations._resolutionStack[^1]);
            return;
        }

        var reason = new CardMoveReason($"skill-program.{(frame.SkillId)}.take-selected-target-cards");
        var moves = slots.Select(slot =>
        {
            var from = new CardLocation(slot.Zone, draft.SourceSeat);
            var card = _cardZonesOperations._cardZones.CardsAt(from)[slot.SlotIndex];
            return (Card: card, From: from, To: _coreEphemeralCardMaterialsOperations.NormalizeEphemeralCardDestination(card, from, card.IsGeneralWeapon && slot.Zone == CardZoneKind.Equipment ? CardLocation.OutsideGame : CardLocation.Hand(frame.OwnerSeat)));
        }).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { OtherCardSelection = null, PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(moves.Select(move => move.Card.Id).ToArray(), CardLocation.Hand(frame.OwnerSeat), reason, destinationForCard: (card, _) => moves.Single(move => move.Card.Id == card.Id).To);
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreRuntimeOperations.ReturnRuntimeProgramMovement(frame.Id);
    }
}
