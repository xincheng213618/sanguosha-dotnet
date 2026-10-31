#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhugeLiangMouRuntime
{
    internal bool MouZhijueYang(int seat) => _coreProgramPersistentStateOperations.GetProgramBooleanState(seat, "mou:zhijue", _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(_coreMatchParticipantsPlayersAccess._players[seat], "mou:zhijue"), "yang");
    internal IReadOnlyList<global::CardGame.Core.GameEngine.ProgramOrdinaryTrickUseOption> MouYangOptions(CharacterState owner)
    {
        var top = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).LastOrDefault();
        return top is null ? [] : _coreProgramOrdinaryTrickUsesOperations.BuildProgramOrdinaryTrickUseOptions(owner, CardKind.FireAttack, _coreNativeCardAppearanceOperations.EffectiveSuit(owner, top), enforceUsePermission: true, beneficiaryShieldSuit: _coreNativeCardAppearanceOperations.EffectiveSuit(owner, top), actualEffectiveColor: global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsRedSuit(_coreNativeCardAppearanceOperations.EffectiveSuit(owner, top)), hasActualColor: true, physicalCardIds: [top.Id], includeNextActualUseAdjustment: true);
    }

    internal IReadOnlyList<LegalAction> MouYinActions(ProgramSkillFrame frame)
    {
        var actor = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        var result = new List<LegalAction>();
        foreach (var kind in frame.MouZhijue!.CopiedKinds)
        {
            if (_coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, kind, CardActionType.Use))
                continue;
            if (global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind))
                foreach (var target in _coreMatchParticipantsPlayersAccess._players.Where(target => _coreNativeTargetLegalityOperations.CanUseVirtualSlashTarget(actor, target, kind, Suit.None, effectiveColor: null, physicalCardIds: [])))
                    result.Add(new(LegalActionKind.Slash, 0, target.Seat, $"视为对 {(target.Name)} 使用【{(CardCatalog.Get(kind).DisplayName)}】", kind, TargetSeats: [target.Seat]));
            else if (kind == CardKind.Peach && _coreTreatedWoundedOperations.IsRuleWounded(actor) && !_coreHandComparisonOperations.HasSelfCardTargetProhibition(actor.Seat) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(actor, kind, Suit.None))
                result.Add(new(LegalActionKind.Peach, 0, null, "视为使用【桃】", kind));
            else if (kind == CardKind.Alcohol && !actor.HasAlcoholEffect && !_coreHandComparisonOperations.HasSelfCardTargetProhibition(actor.Seat) && (!actor.UsedPlayPhaseAlcoholThisTurn || HasMouWujingAllowance(actor) || _coreTurnTargetCardQuotaAllowancesOperations.HasTargetCardQuotaAllowance(actor.Seat, actor.Seat) || _coreAdvancedPoliciesOperations.HasNextUnlimitedCard(actor) || _coreProgramCardPoliciesOperations.HasCardPolicy(actor, SkillProgramCardPolicyKind.UnlimitedAlcoholUse, kind)) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(actor, kind, Suit.None))
                result.Add(new(LegalActionKind.Alcohol, 0, null, "视为使用【酒】", kind));
            else if (global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(kind) && kind != CardKind.Nullification)
                foreach (var option in _coreProgramOrdinaryTrickUsesOperations.BuildProgramOrdinaryTrickUseOptions(actor, kind, Suit.None, enforceUsePermission: true, beneficiaryShieldSuit: Suit.None, actualEffectiveColor: null, hasActualColor: true, physicalCardIds: [], includeNextActualUseAdjustment: true))
                    result.Add(new(option.ActionKind, 0, option.TargetSeats.Count == 1 ? option.TargetSeats[0] : null, option.Description, kind, option.TargetCardId, option.TargetSeats));
        }

        return result;
    }

    internal IReadOnlyList<PromptChoice> MouZhijueChoices(ProgramSkillFrame frame)
    {
        var draft = frame.MouZhijue!;
        var actor = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        var choices = new List<PromptChoice>();
        Dictionary<string, string> Params(string option) => new()
        {
            ["program-action"] = "mou-zhijue",
            ["frame-id"] = frame.Id.ToString(CultureInfo.InvariantCulture),
            ["option"] = option
        };
        if (draft.Stage == "color")
        {
            foreach (var red in new[]
            {
                true,
                false
            }

            )
            {
                var ids = _corePhysicalOwnedCardQueriesOperations.GetHand(actor).Where(card => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsRedSuit(_coreNativeCardAppearanceOperations.EffectiveSuit(actor, card)) == red).Select(card => card.Id).Order().ToArray();
                if (ids.Length > 0 && _skillRuntimeStateOperations._skillRuntimeState.GetUsage(actor.Seat, frame.SkillId, "color:" + red, SkillUsageScope.Turn) == 0)
                    choices.Add(new(new($"mou-color.{(frame.Id)}.{(red)}"), $"将所有{((red ? "红" : "黑"))}色手牌（{(ids.Length)}张）置入弃牌堆", ids, [], Params(red.ToString())));
            }
        }
        else if (draft.Stage == "yang")
            foreach (var(option, index)in MouYangOptions(actor).Select((option, index) => (option, index)))
                choices.Add(new(new($"mou-yang.{(frame.Id)}.{(index)}"), option.Description, [_cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile)[^1].Id], option.TargetSeats, Params(index.ToString(CultureInfo.InvariantCulture))));
        else if (draft.Stage == "copy")
        {
            foreach (var(action, index)in MouYinActions(frame).Select((action, index) => (action, index)))
                choices.Add(new(new($"mou-copy.{(frame.Id)}.{(index)}"), action.Description, [], action.TargetSeats, Params(index.ToString(CultureInfo.InvariantCulture))));
            choices.Add(new(new($"mou-copy.{(frame.Id)}.decline"), "不视为使用牌", [], [], Params("decline")));
        }

        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishMouPrompt(ProgramSkillFrame frame, IReadOnlyList<PromptChoice> choices, string text)
    {
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, frame.OwnerSeat, text, choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = frame.OwnerSeat,
            Choices = choices,
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 40)]
    internal bool ResumeMouZhijue(ProgramSkillFrame frame)
    {
        if (frame.MouZhijue is not { } draft)
            return false;
        if (frame.PendingMovementContinuation is not null || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || draft.Stage == "use")
            return true;
        if (!_coreDeckProgramsOperations.DeckProgramSourceValid(frame) || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            _coreSkillProgramsOperations.FinishProgramSkill(frame, true);
            return true;
        }

        if (draft.Stage == "returned")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { MouZhijue = draft with { Stage = "done" } });
            if (!draft.CausedDamage)
            {
                _characterZhugeLiangMouMouWujingAllowancesMouZhitianCountsAccess._mouZhitianCounts[frame.OwnerSeat] = Math.Max(1, MouZhitianCount(frame.OwnerSeat) - 1);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZhijueUndamagedEvent(frame.OwnerSeat, MouZhitianCount(frame.OwnerSeat)));
                _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], 2, log: true, reason: new("mou.zhijue.undamaged"));
                _coreDeckProgramsOperations.AwaitDeckProgramMovement(frame.Id);
                return true;
            }
        }

        if (frame.MouZhijue!.Stage == "done")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { MouZhijue = null });
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
            return true;
        }

        var choices = MouZhijueChoices(frame);
        if (choices.Count == 0)
        {
            _coreSkillProgramsOperations.FinishProgramSkill(frame, true);
            return true;
        }

        PublishMouPrompt(frame, choices, draft.Stage switch
        {
            "yang" => "智绝·阳：将牌堆顶的一张牌当【火攻】使用，选择目标。",
            "color" => "智绝·阴：选择一种颜色，置入该颜色的全部手牌。",
            _ => "智绝·阴：可视为使用刚置入弃牌堆的一张基本或普通锦囊牌。"
        });
        return true;
    }

    [ProgramAnswer("mou-zhijue", "mou-zhitian-confirm")]
    internal void ResolveMouZhugeChoice(PromptChoice choice)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Mou Zhuge lost its program.");
        if (!_coreDeckProgramsOperations.DeckProgramSourceValid(frame))
            throw new InvalidOperationException("Mou Zhuge lost its source instance.");
        if (choice.Parameters.GetValueOrDefault("program-action") == "mou-zhitian-confirm")
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
            return;
        }

        var draft = frame.MouZhijue ?? throw new InvalidOperationException("Zhijue lost its typed draft.");
        var canonical = MouZhijueChoices(frame).SingleOrDefault(item => item.Id == choice.Id);
        if (canonical is null || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([canonical], [choice]))
            throw new InvalidOperationException("Zhijue changed its exact cards or legal targets.");
        var option = choice.Parameters["option"];
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (draft.Stage == "color")
        {
            var red = bool.Parse(option);
            var actor = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
            var cards = _corePhysicalOwnedCardQueriesOperations.GetHand(actor).Where(card => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsRedSuit(_coreNativeCardAppearanceOperations.EffectiveSuit(actor, card)) == red).OrderBy(c => c.Id).ToArray();
            if (!_skillRuntimeStateOperations._skillRuntimeState.TryConsumeUsage(frame.OwnerSeat, frame.SkillId, "color:" + red, SkillUsageScope.Turn, 1))
                throw new InvalidOperationException("Zhijue repeated its color this actual turn.");
            _coreProgramSkillChangesOperations.SetProgramBooleanState(frame, "yang", true);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZhijuePolarityChangedEvent(frame.OwnerSeat, true));
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { MouZhijue = draft with { Stage = "copy", PaidCardIds = Array.AsReadOnly(cards.Select(c => c.Id).ToArray()), CopiedKinds = Array.AsReadOnly(cards.Where(c => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(c.Kind) || global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.MatchesProgramCardCategory(c.Kind, [SkillProgramCardCategory.Basic])).Select(c => c.Kind).Distinct().Order().ToArray()) } });
            _corePhysicalMovementFacadesOperations.MoveCards(cards, CardLocation.Hand(frame.OwnerSeat), CardLocation.DiscardPile, new("mou.zhijue.place"));
            _coreDeckProgramsOperations.AwaitDeckProgramMovement(frame.Id);
            return;
        }

        if (option == "decline")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { MouZhijue = draft with { Stage = "returned" } });
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
            return;
        }

        var index = int.Parse(option, CultureInfo.InvariantCulture);
        LegalAction action;
        global::CardGame.Core.GameEngine.ProgramOrdinaryTrickUseOption? trick;
        Card card;
        IReadOnlyList<int> physicalIds;
        if (draft.Yang)
        {
            trick = MouYangOptions(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat])[index];
            card = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile)[^1];
            action = new(trick.ActionKind, card.Id, null, trick.Description, trick.EffectiveCardKind, trick.TargetCardId, trick.TargetSeats);
            physicalIds = [card.Id];
            _coreProgramSkillChangesOperations.SetProgramBooleanState(frame, "yang", false);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZhijuePolarityChangedEvent(frame.OwnerSeat, false));
        }
        else
        {
            action = MouYinActions(frame)[index];
            card = new(0, action.PlayedCardKind!.Value, Suit.None, 0);
            physicalIds = [];
            trick = global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(card.Kind) ? _coreProgramOrdinaryTrickUsesOperations.BuildProgramOrdinaryTrickUseOptions(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], card.Kind, Suit.None, enforceUsePermission: true, beneficiaryShieldSuit: Suit.None, actualEffectiveColor: null, hasActualColor: true, physicalCardIds: [], includeNextActualUseAdjustment: true).Single(item => item.ActionKind == action.Kind && item.TargetCardId == action.TargetCardId && item.TargetSeats.SequenceEqual(action.TargetSeats)) : null;
        }

        IssueMouZhijueUse(frame, action, trick, card, physicalIds);
    }

    internal void IssueMouZhijueUse(ProgramSkillFrame frame, LegalAction action, global::CardGame.Core.GameEngine.ProgramOrdinaryTrickUseOption? trick, Card card, IReadOnlyList<int> physicalIds)
    {
        var actor = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        var kind = action.PlayedCardKind!.Value;
        if (trick?.NextActualUseAdjusted == true)
            _coreGameEngineNextCardTargetAdjustmentsSelectedNextCardTargetSeatsAccess._selectedNextCardTargetSeats = action.TargetSeats;
        var id = _coreNativeCardUseOperations.BeginCardUse(card, actor.Seat, action.TargetSeats, kind, physicalCardIds: physicalIds, conversionSource: frame.MouZhijue!.Yang ? new(frame.SkillId, frame.ActivationId, frame.OwnerSeat, frame.SkillInstanceId) : null, designatedTargetSeats: kind == CardKind.BorrowedSword ? action.TargetSeats.Take(1).ToArray() : null);
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
        var ret = new MouZhijueUseReturn(frame.Id, frame.InstructionIndex, frame.OwnerSeat, frame.SkillId, frame.SkillInstanceId, frame.GameplayHash, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, frame.MouZhijue!.Yang, kind, card.Id, use.Action!.ActionId);
        _coreRuntimeOperations.ReplaceRuntimeFrame(frame.Id, frame with { MouZhijue = frame.MouZhijue with { Stage = "use", UseId = id } });
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, current => current with { MouZhijueReturn = ret });
        if (physicalIds.Count > 0)
            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.DrawPile, CardLocation.Processing, CardMoveReasons.Use);
        if (kind == CardKind.Peach)
        {
            _coreCardUseLifecycleOperations.BeginSimpleCardUse(id, new(0, SimpleCardUseEffect.Recovery));
            return;
        }

        if (kind == CardKind.Alcohol)
        {
            _coreCardUseLifecycleOperations.BeginSimpleCardUse(id, new(0, SimpleCardUseEffect.Alcohol));
            return;
        }

        if (global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind))
        {
            var target = _coreMatchParticipantsPlayersAccess._players[action.TargetSeats[0]];
            var damage = 1 + actor.AlcoholDamageBonus;
            _coreAlcoholSlashSuppressionsOperations.CaptureProgramAlcoholConsumption(id, actor);
            actor.HasAlcoholEffect = false;
            if (_coreCardUseLifecycleOperations.LifecycleCardUse(id)?.UnlimitedUse != true)
                _coreCardUseDebitsOperations.RecordSlashUseDebit(id, actor.Seat);
            var attack = _kernelCoreMainOperations.CreateCardAttackHandle(id, actor.Seat, target.Seat, card: null, damageAmount: damage, playedCardKind: kind, programSkillCardUseFrameId: frame.Id, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, target, kind));
            _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
            _coreCurrentCardEnhancementsOperations.CaptureProgramAdjustedSlashBaseDamage(attack);
            _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actor.Seat, kind);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, kind, actor.Seat, target.Seat));
            if (!_coreCardActionsOperations.TryBeginProgramCardWindow(attack, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.Action!, SkillProgramTriggerWindow.CardUseCommitted, action.TargetSeats, ProgramCardContinuation.CommittedSlash))
                _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
            return;
        }

        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(id, card, actor.Seat, use.TargetSeats, action.Kind, action.TargetCardId, trick?.RequiredCardKind, kind);
    }

    internal bool IsMouZhijueUse(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { MouZhijueReturn: { } ret } use && ValidMouZhijueUse(use, ret);
    internal bool IsMouZhijueZeroUse(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { CardId: 0, MouZhijueReturn: { Yang: false } } && IsMouZhijueUse(id);
    internal bool ValidMouZhijueUse(CardUseFrame use, MouZhijueUseReturn ret)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(item => item.Id == use.Id);
        return index > 0 && _resolutionStackOperations._resolutionStack[index - 1] is ProgramSkillFrame frame && frame.Id == ret.ProgramFrameId && frame.InstructionIndex == ret.InstructionIndex && frame.OwnerSeat == ret.OwnerSeat && frame.SkillId == ret.SkillId && frame.SkillInstanceId == ret.SkillInstanceId && frame.GameplayHash == ret.GameplayHash && frame.MouZhijue is { Stage: "use" } draft && draft.UseId == use.Id && draft.Yang == ret.Yang && use.CardId == ret.PhysicalCardId && use.CardKind == ret.Kind && use.Action is { Type: CardActionType.Use } action && action.ActionId == ret.ActionId && action.ActorSeat == ret.OwnerSeat && action.ProviderSeat == ret.OwnerSeat && (ret.Yang ? use.PhysicalCardIds is [var paid] && paid == ret.PhysicalCardId && action.PhysicalCards is [var cost] && cost.CardId == paid && cost.From == CardLocation.DrawPile && ret.Kind == CardKind.FireAttack : use.PhysicalCardIds is { Count: 0 } && action.PhysicalCards.Count == 0 && draft.CopiedKinds.Contains(ret.Kind));
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterCleanup, 20)]
    internal bool CompleteMouZhijueNativeAttackReturn()
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouZhijue.Stage: "returned" } parent)
            return false;
        _coreRuntimeOperations.AdvanceRuntimeProgram(parent.Id);
        return true;
    }

    [NativeUseReturn(300)]
    internal void ReturnMouZhijueUse(CardUseFrame use)
    {
        if (use.MouZhijueReturn is not { } ret)
            return;
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(ret.ProgramFrameId);
        if (frame.MouZhijue is not { Stage: "use" } draft || draft.UseId != use.Id || frame.InstructionIndex != ret.InstructionIndex)
            throw new InvalidOperationException("Zhijue lost its one-shot typed card-use return.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(frame.Id, frame with { MouZhijue = draft with { Stage = "returned", CausedDamage = use.CausedDamage } });
        if (use.CardAttack is not { Active: true })
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    internal bool ContinueMouZhijueSimpleUse(long id, ProgramSimpleCardContinuation continuation)
    {
        if (!IsMouZhijueZeroUse(id))
            return false;
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
        var actor = _coreMatchParticipantsPlayersAccess._players[use.SourceSeat];
        var card = new Card(0, use.CardKind, Suit.None, 0);
        _coreNativeCardUseFramesOperations.SetCardUseStep(id, ResolutionFrameStep.ResolvingEffect);
        if (continuation.Effect == SimpleCardUseEffect.Recovery)
            _coreNativeRecoveryCardsOperations.CompleteRecoveryCardUse(actor, actor, card, id, use.CardKind, continuation.RecoveryAmount, continuation.RecoveryPolicySources ?? []);
        else
            _coreNativeTrickUseOperations.CompleteAlcoholUse(actor, card, id);
        return true;
    }

    internal void AssertMouZhijueProgram(ProgramSkillFrame frame, SkillProgramEffect paused)
    {
        if (frame.MouZhijue is not { } draft)
            return;
        if (paused.Op != SkillProgramEffectOp.MouZhijueUse || draft.Stage is not ("yang" or "color" or "copy" or "use" or "returned" or "done") || draft.Stage == "use" != (draft.UseId is not null && _coreCardUseLifecycleOperations.LifecycleCardUse(draft.UseId.Value)is not null) || draft.PaidCardIds.Distinct().Count() != draft.PaidCardIds.Count)
            throw new InvalidOperationException("Zhijue lost its exact staged payment/use draft.");
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == frame.Id && frame.PendingMovementContinuation is null && draft.Stage is "yang" or "color" or "copy" && (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } prompt || prompt.PlayerSeat != frame.OwnerSeat || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(prompt.Choices, MouZhijueChoices(frame))))
            throw new InvalidOperationException("Zhijue lost its exact private choice.");
    }
}
