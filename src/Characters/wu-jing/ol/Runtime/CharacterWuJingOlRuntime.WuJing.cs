#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWuJingOlRuntime
{
    internal const string WuJingRewardReason = "skill-program.wu-jing.heji.red", WuJingClaimReason = "skill-program.wu-jing.liubing.claim";
    internal CardConversionSource WuJingSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    SkillProgramStepOutcome global::CardGame.Core.IWuJingHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginWuJing(f, op);
    internal CardUseFrame? WuJingCompletedUse(ProgramSkillWindowContext context)
    {
        if (context is not { Window: SkillProgramTriggerWindow.CardUseCompleted, CardUse: { } card })
            return null;
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(card.ParentCardUseFrameId);
        return use is { Action: { Type: CardActionType.Use } a } && a.ActionId == card.CardActionId && a.ActorSeat == card.ActorSeat && a.EffectiveKind == use.CardKind ? use : null;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 27130)]
    internal bool QualifyWuJing(ProgramTriggerCandidate c, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (trigger.Effects is not [var e] || !WuJingComposition.Handles(e.Op))
            return true;
        if (e.Op == SkillProgramEffectOp.ClaimLiubingUndamagingSlash)
            return CanRunWuJingLiubingCandidate(c, context);
        if (e.Op == SkillProgramEffectOp.RewardHejiUnconvertedUse)
            return IsIssuedWuJingHejiReward(c, context);
        return _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive && WuJingCompletedUse(context)is { } use && WuJingHejiIncoming(use) && use.TargetSeats[0] != c.OwnerSeat;
    }

    internal SkillProgramStepOutcome BeginWuJing(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.WuJing is not null || f.InstructionIndex != 1 || f.TriggerId is null || f.WindowContext is not { } context)
            throw new InvalidOperationException("Wu Jing requires his exact unissued native trigger instruction.");
        var c = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex);
        if (!QualifyWuJing(c, _coreProgramLifecycleOperations.GetProgramTrigger(f), context))
            throw new InvalidOperationException("Wu Jing lost his completed use or genuine discard invoice.");
        var use = op == SkillProgramEffectOp.ClaimLiubingUndamagingSlash ? null : WuJingCompletedUse(context)!;
        var claim = op == SkillProgramEffectOp.ClaimLiubingUndamagingSlash ? WuJingClaimFacts(context.MovementBatch!, c) : [];
        var entities = claim.Where(e => _cardZonesOperations._cardZones.GetLocation(e.CardId) == CardLocation.DiscardPile && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Last(m => m.CardId == e.CardId).Sequence == e.MovementSequence).Select(e => new WuJingMaterial(e.CardId, _coreAdvancedLifecycleOperations.GetAdvancedCard(e.CardId).Kind, CardLocation.DiscardPile, e.MovementSequence)).ToArray();
        var r = new WuJingReceipt(WuJingSource(f), f.GameplayHash, op, context.ParentFrameId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, op == SkillProgramEffectOp.UseHejiAtCompletedSingleTarget ? WuJingStage.UseChoice : op == SkillProgramEffectOp.RewardHejiUnconvertedUse ? WuJingStage.RewardChildren : WuJingStage.ClaimChildren, use?.Id ?? claim[0].UseId, use?.Action!.ActionId ?? claim[0].ActionId, op == SkillProgramEffectOp.RewardHejiUnconvertedUse ? use!.WuJingHeji!.Origin.TargetSeat : use?.TargetSeats.Single() ?? context.SourceSeat!.Value, NativeOrigin: op == SkillProgramEffectOp.RewardHejiUnconvertedUse ? use!.WuJingHeji!.Origin : null, Before: _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After: _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
        {
            Entities = entities
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuJing = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuJingStartedEvent(f.Id, r.Source, r.GameplayHash, op, r.WindowId, r.ActualTurn, r.UseId, r.ActionId, r.TargetSeat));
        if (op == SkillProgramEffectOp.UseHejiAtCompletedSingleTarget)
        {
            PublishWuJing(f);
            return SkillProgramStepOutcome.AwaitChoice;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        if (op == SkillProgramEffectOp.RewardHejiUnconvertedUse)
        {
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(r.UseId, u => u with { WuJingHeji = u.WuJingHeji!with { Rewarded = true } });
            var cards = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Where(card => card.Suit is Suit.Heart or Suit.Diamond).ToArray();
            var red = cards.Length == 0 ? null : cards[_randomOperations._random.Next(cards.Length)];
            if (red is not null)
                _corePhysicalMovementFacadesOperations.MoveCard(red, CardLocation.DrawPile, CardLocation.Hand(f.OwnerSeat), new(WuJingRewardReason));
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuJing = f.WuJing!with { RewardCardId = red?.Id, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuJingRewardIssuedEvent(f.Id, r.NativeOrigin!.ProgramFrameId, r.UseId, red?.Id, r.Before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        }
        else
        {
            foreach (var entity in entities)
                _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(entity.CardId), CardLocation.DiscardPile, CardLocation.Hand(f.OwnerSeat), new(WuJingClaimReason));
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuJing = f.WuJing!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuJingClaimIssuedEvent(f.Id, r.UseId, r.ActionId, context.MovementBatch!.Id, entities.Length, r.Before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        }

        ContinueWuJing(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal bool WuJingCanUse(int owner, int target, Card card, CardKind kind, CardConversionSource? conversion, IReadOnlyList<int> ids)
    {
        var actor = _coreMatchParticipantsPlayersAccess._players[owner];
        var victim = _coreMatchParticipantsPlayersAccess._players[target];
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !actor.IsAlive || !victim.IsAlive || owner == target || _coreCardUseModulesOperations.IsCardUseForbidden(owner, kind, CardActionType.Use) || (global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind) ? _coreTurnHandCategoryRestrictionsOperations.IsHandCategoryMaterialRestricted(ids) || WuJingLiubingUsedSuit(owner, card, kind, ids, conversion is null ? [] : [conversion], _corePlayCardHistoryOperations.CaptureUsedCardSuit(owner, ids, card))is { } usedSuit && _coreProgramParticipantPhaseOperationsOperations.IsTurnSuitUseForbidden(owner, usedSuit) : _coreProgramParticipantPhaseOperationsOperations.IsTurnPhysicalUseForbidden(owner, ids)) || _coreCardUseModulesOperations.HasTurnCardTargetRestriction(owner, SkillProgramCardTargetRestriction.SelfOnly) || ids.Any(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Hand(owner) && _coreCardUseModulesOperations.IsTurnHandCardRestricted(actor, _coreAdvancedLifecycleOperations.GetAdvancedCard(id))))
            return false;
        if (global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind))
            return _coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, victim, card, conversion, kind, ignoreDistance: true, physicalCardIds: ids);
        var appearance = _corePlayCardHistoryOperations.ApplyProgramUseAppearance(actor, card, conversion);
        return kind == CardKind.Duel && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(owner, target, kind) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(victim, kind, appearance.Suit, global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(actor, appearance)));
    }

    internal IReadOnlyList<PromptChoice> WuJingChoices(ProgramSkillFrame f)
    {
        if (f.WuJing is not { Stage: WuJingStage.UseChoice } r || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
            return[];
        var actor = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var result = new List<PromptChoice>();
        Dictionary<string, string> P(CardKind kind) => new()
        {
            ["program-action"] = "wu-jing",
            ["kind"] = kind.ToString()
        };
        void Add(Card card, CardKind kind, CardConversionSource? source, IReadOnlyList<int> ids, bool zhangba = false)
        {
            if (!WuJingCanUse(f.OwnerSeat, r.TargetSeat, card, kind, source, ids))
                return;
            var parameters = P(kind);
            if (source is not null)
                global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime.AddConversionParameters(parameters, source);
            if (zhangba)
                parameters["equipment"] = CardKind.ZhangbaSerpentSpear.ToString();
            result.Add(new(new($"wu-jing.{(f.Id)}.{(kind)}.{(source?.SkillInstanceId)}.{(source?.BindingId)}.{(string.Join('-', ids))}.{(zhangba)}"), $"对{(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].Name)}使用【{(CardCatalog.Get(kind).DisplayName)}】", ids, [r.TargetSeat], parameters));
        }

        foreach (var card in _corePhysicalOwnedCardQueriesOperations.GetPlayableCards(actor).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(actor)).Concat(_coreNativePublicPileOriginalCardsOperations.NativePublicPileOriginalUseCards(actor)).DistinctBy(c => c.Id))
        {
            foreach (var kind in global::CardGame.Core.EngineOwners.CoreGameEngineDirectedCardPolicyQueriesDefinitions.SlashKinds.Append(CardKind.Duel))
                foreach (var conversion in _coreActionQueriesOperations.GetProgramViewAsConversions(actor, card, kind, false))
                    Add(card, kind, conversion, [card.Id]);
            if (card.Kind == CardKind.Duel && !_coreCardConversionsOperations.HasProgramCardIdentity(actor, card))
                Add(card, CardKind.Duel, null, [card.Id]);
            foreach (var identity in _coreActionQueriesOperations.GetProgramCardIdentityMatches(actor, card).Where(i => i.Identity.OutputKind == CardKind.Duel))
                Add(card, CardKind.Duel, identity.Source, [card.Id]);
        }

        foreach (var card in _coreActionQueriesOperations.GetSlashUseCards(actor))
        {
            var identity = _coreActionQueriesOperations.GetProgramCardIdentityMatches(actor, card).FirstOrDefault(i => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(i.Identity.OutputKind));
            var baseKind = identity?.Identity.OutputKind ?? (global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(card.Kind) ? card.Kind : CardKind.Slash);
            foreach (var kind in _coreNativeTargetLegalityOperations.GetSlashUseKinds(actor, baseKind))
                foreach (var variant in _coreCardConversionsOperations.CreateConversionChoiceVariants(actor, card, kind, false, $"wu-jing.{(f.Id)}.base.{(kind)}.{(card.Id)}", $"使用【{(CardCatalog.Get(kind).DisplayName)}】", [card.Id], [r.TargetSeat], P(kind)))
                {
                    global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime.TryReadConversionSource(variant.Parameters, out var conversion);
                    if (WuJingCanUse(f.OwnerSeat, r.TargetSeat, card, kind, conversion, [card.Id]))
                        result.Add(variant);
                }
        }

        foreach (var pair in _coreNativeResponseMaterialQueriesOperations.GetZhangbaSlashPairs(actor))
            Add(pair[0], CardKind.Slash, null, pair.Select(c => c.Id).ToArray(), true);
        foreach (var kind in global::CardGame.Core.EngineOwners.CoreGameEngineDirectedCardPolicyQueriesDefinitions.SlashKinds.Append(CardKind.Duel))
            foreach (var selection in _coreCardConversionsOperations.GetProgramMultiCardViewAsSelections(actor, kind, false).Where(s => _corePlayCardHistoryOperations.ViewAsRule(s.Source)is { RoundDistinctBasicUse: null, LeiluanBasicUse: null } || s.OutputKind == kind))
                Add(selection.Cards[0], kind, selection.Source, selection.Cards.Select(c => c.Id).ToArray());
        result.AddRange(_characterMaXiuMaTieOlOperations.KenshangProgramSlashChoices(f, f.OwnerSeat, [r.TargetSeat]));
        foreach (var(source, _)in _coreRoundOnceCardlessNativeUsesOperations.RoundOnceCardlessRules(actor, CardKind.Slash).Where(entry => _coreRoundOnceCardlessProgramRequestsOperations.RoundOnceCardlessProgramTargetLegal(actor, r.TargetSeat, entry.Source, true)))
        {
            var p = new Dictionary<string, string>
            {
                ["program-action"] = "wu-jing",
                ["kind"] = "round-once"
            };
            global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime.AddConversionParameters(p, source);
            result.Add(new(new($"wu-jing.{(f.Id)}.round-once.{(source.SkillInstanceId)}.{(source.BindingId)}"), "支付独立回合额度，视为对原目标使用杀", [], [r.TargetSeat], p));
        }

        if (_coreProgramFactionPoliciesOperations.GetFactionResponsePolicy(actor, CardKind.Slash)is not null && WuJingFactionTarget(f))
            result.Add(new(new($"wu-jing.{(f.Id)}.faction"), "请求同势力角色提供杀", [], [r.TargetSeat], new Dictionary<string, string> { ["program-action"] = "wu-jing", ["kind"] = "faction" }));
        result.Add(new(new($"wu-jing.{(f.Id)}.decline"), "不使用牌", [], [], new Dictionary<string, string> { ["program-action"] = "wu-jing", ["kind"] = "decline" }));
        return Array.AsReadOnly(result.DistinctBy(c => c.Id).ToArray());
    }

    internal void PublishWuJing(ProgramSkillFrame f, bool keepCurrentRequest = false)
    {
        var choices = WuJingChoices(f);
        if (choices.Count == 0 || !keepCurrentRequest && choices.All(c => c.Cards.Count == 0 && c.Parameters.GetValueOrDefault("kind") == "decline") && !HasWuJingZeroSupply(f))
        {
            FinishWuJing(f, false);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "合击：对原牌的唯一目标使用一张杀或决斗。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), [f.WuJing!.TargetSeat], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = f.OwnerSeat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("wu-jing")]
    internal void AnswerWuJing(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { WuJing: { Stage: WuJingStage.UseChoice } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], WuJingChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Heji lost its exact private original-actor card choices.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        if (choice.Parameters["kind"] == "decline")
        {
            FinishWuJing(f, false);
            return;
        }

        if (choice.Parameters["kind"] == "round-once")
        {
            BeginWuJingRoundOnce(f, global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime.RequireConversionSource(choice));
            return;
        }

        if (choice.Parameters["kind"] == "faction")
        {
            BeginWuJingFaction(f);
            return;
        }

        var kind = Enum.Parse<CardKind>(choice.Parameters["kind"]);
        global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime.TryReadConversionSource(choice.Parameters, out var source);
        var zhangba = choice.Parameters.GetValueOrDefault("equipment") == CardKind.ZhangbaSerpentSpear.ToString();
        var cards = choice.Cards.Count == 1 ? new[]
        {
            _corePhysicalOwnedCardQueriesOperations.GetPlayableCards(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(f.OwnerSeat)).Concat(_coreNativePublicPileOriginalCardsOperations.NativePublicPileOriginalUseCards(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat])).Single(c => c.Id == choice.Cards[0])
        }

        : zhangba ? _coreNativeResponseMaterialQueriesOperations.GetZhangbaSlashPairs(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Single(p => p.Select(c => c.Id).SequenceEqual(choice.Cards)).ToArray() : _coreCardConversionsOperations.FindProgramMultiCardViewAsSelection(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], choice.Cards, kind, false, source!)?.Cards.ToArray() ?? throw new InvalidOperationException("Heji lost its real multi-material conversion.");
        var selection = new WuJingUseSelection(kind, zhangba, kind == CardKind.FireSlash && cards[0].Kind == CardKind.Slash && _coreNativeTargetLegalityOperations.HasZhuqueFan(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]))
        {
            CardIds = choice.Cards,
            Conversions = source is null ? [] : [source]
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuJing = r with { Stage = WuJingStage.IssuingUse, Selection = selection, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _coreGameEngineCardConversionsHasSelectedUseConversionChoiceSelectedUseConversionAccess._selectedUseConversion = source;
        _coreGameEngineCardConversionsHasSelectedUseConversionChoiceSelectedUseConversionAccess._hasSelectedUseConversionChoice = true;
        if (global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind))
            _coreNativeSlashUseOperations.ResolveSlashCore(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[r.TargetSeat], cards[0], kind, f.OwnerSeat, physicalCards: cards, countsTowardSlashLimit: false, usesZhuqueFan: selection.UsesFan, conversionSource: source, programSkillCardUseFrameId: f.Id);
        else
        {
            var id = _coreNativeCardUseOperations.BeginCardUse(cards[0], f.OwnerSeat, [r.TargetSeat], CardKind.Duel, physicalCardIds: cards.Select(c => c.Id).ToArray(), conversionSource: source);
            _coreCardUsePaymentsOperations.PaySlashUseMaterials(cards, f.OwnerSeat, null);
            CompleteWuJingHejiPayment(id);
            _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(id, cards[0], f.OwnerSeat, [r.TargetSeat], LegalActionKind.Duel, playedCardKind: CardKind.Duel);
        }
    }

    internal bool DrainWuJing(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    internal void ContinueWuJing(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { WuJing: { } r } f || f.Id != id)
            return;
        AssertWuJing(f);
        if (r.Stage is WuJingStage.IssuingUse or WuJingStage.NativeUse or WuJingStage.FactionRequest)
            return;
        if (r.Stage == WuJingStage.UseChoice)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishWuJing(f);
            return;
        }

        if (!DrainWuJing(f))
            FinishWuJing(_coreProgramLifecycleOperations.GetActiveProgramFrame(id), true);
    }

    internal void FinishWuJing(ProgramSkillFrame f, bool applied)
    {
        if (f.HuanpeiReturn is { } huanpei && (f.WuJing is not { Stage: WuJingStage.NativeChildren, NativeCompleted: true, NativeOrigin: { } huanpeiNative } || huanpeiNative.UseId != huanpei.UseId || !huanpei.Returned))
            throw new InvalidOperationException("Heji cannot release Huanpei before all native children returned.");
        if (f.ZonghuReturn is { } zonghu && (f.WuJing is not { Stage: WuJingStage.NativeChildren, NativeCompleted: true, NativeOrigin: { } origin } || origin.UseId != zonghu.UseId || !zonghu.Returned))
            throw new InvalidOperationException("Heji cannot release its exact paid Zonghu lease before all native children returned.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuJingCompletedEvent(f.Id, applied));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuJing = null, ZonghuReturn = null, HuanpeiReturn = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, applied);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 27130)]
    internal bool ResumeWuJing(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { WuJing: not null } f || f.Id != id)
            return false;
        ContinueWuJing(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 27130)]
    internal bool ReturnWuJingMovement(ProgramSkillFrame f)
    {
        if (f.WuJing is null)
            return false;
        ContinueWuJing(f.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27130)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27130)]
    internal bool ContinuePaidWuJing(ProgramSkillFrame f) => f.WuJing is { } r && (r.Stage != WuJingStage.UseChoice || WuJingHasPaidAdapter(f)) && _coreProgramEventHistoryOperations.ProgramEventHistory<WuJingStartedEvent>().Count(e => e.FrameId == f.Id && e.Source == r.Source && e.GameplayHash == r.GameplayHash) == 1;
    [ProgramAiChoice(SkillProgramEffectOp.UseHejiAtCompletedSingleTarget, SkillProgramEffectOp.ClaimLiubingUndamagingSlash, SkillProgramEffectOp.RewardHejiUnconvertedUse)]
    internal PromptChoice AiWuJing(PendingDecision p, ProgramSkillFrame f) => global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[f.WuJing!.TargetSeat]) ? p.Choices.Single(c => c.Parameters.GetValueOrDefault("kind") == "decline") : p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("kind") != "decline") ?? p.Choices.First();
}
