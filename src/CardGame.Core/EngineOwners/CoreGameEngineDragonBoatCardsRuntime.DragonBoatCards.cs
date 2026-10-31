#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDragonBoatCardsRuntime
{
    internal static bool IsDragonBoatCard(CardKind kind) => RuntimeSkills.RuntimeRules_Core_DragonBoatCards.IsDragonBoatCard(kind);
    internal static bool IsDragonBoatTrick(CardKind kind) => CoreGameEngineDragonBoatCardsDefinitions.DragonBoatOrdinaryTrickKinds.Contains(kind);
    internal static LegalActionKind DragonBoatCardAction(CardKind kind) => RuntimeSkills.RuntimeRules_Core_DragonBoatCards.DragonBoatCardAction(kind);
    internal bool DragonBoatCardAllies(int first, int second) => _coreMatchParticipantsPlayersAccess._players[first].TeamId is { } team && team == _coreMatchParticipantsPlayersAccess._players[second].TeamId;
    internal bool IsDragonBoatRealgarWineQuotaUse(CardUseFrame use, CardActionContext action) => _coreDragonBoatActivitiesOperations.IsDragonBoatActivity && use.CardKind == CardKind.RealgarWine && action.EffectiveKind == CardKind.RealgarWine && action.Type == CardActionType.Use && action.ActorSeat == use.SourceSeat && action.ProviderSeat == use.SourceSeat && use.TargetSeats is [var target] && target == use.SourceSeat && action.TargetSeats.SequenceEqual(use.TargetSeats);
    internal int DragonBoatCardTeamScore(int seat) => _coreDragonBoatActivitiesOperations.DragonBoatTeamScore(_coreMatchParticipantsPlayersAccess._players[seat].TeamId ?? throw new InvalidOperationException("A Dragon Boat card requires its actual public team."));
    internal (bool Most, bool Least) DragonBoatCardRank(int seat)
    {
        var scores = _coreMatchParticipantsPlayersAccess._players.Select(p => p.TeamId).Where(id => id is not null).Distinct(StringComparer.Ordinal).Select(id => _coreDragonBoatActivitiesOperations.DragonBoatTeamScore(id!)).ToArray();
        var score = DragonBoatCardTeamScore(seat);
        return (score == scores.Max(), score == scores.Min());
    }

    internal bool CanBeDragonBoatAdditionalTarget(CardUseFrame use, CharacterState target) => target.IsAlive && (use.CardKind switch
    {
        CardKind.SameBoat => DragonBoatCardAllies(use.SourceSeat, target.Seat),
        CardKind.AgainstCurrent => !DragonBoatCardAllies(use.SourceSeat, target.Seat),
        CardKind.Zong => DragonBoatCardAllies(use.SourceSeat, target.Seat) && _coreTreatedWoundedOperations.IsRuleWounded(target),
        _ => false
    });
    internal IReadOnlyList<int> DragonBoatGlobalTargets(CharacterState actor, Card card) => _coreCardUseModulesOperations.ApplyTurnCardGroupTargetRestrictions(actor, Enumerable.Range(0, _coreMatchDefinitionPlayerCountAccess._playerCount).Select(offset => _coreMatchParticipantsPlayersAccess._players[_coreTableOrderOperations.TableStepSeat(actor.Seat, offset)]).Where(target => target.IsAlive && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, target.Seat, CardKind.StriveUpstream) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(target, CardKind.StriveUpstream, card.Suit, CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(actor, card))) && !_coreShenwuEquipmentOperations.IsShenwuRobeTargetProhibited(actor.Seat, target.Seat, CardKind.StriveUpstream) && !_coreProgramCardPoliciesOperations.HasCardPolicy(target, SkillProgramCardPolicyKind.ExcludeGlobalTarget, CardKind.StriveUpstream) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, target.Seat, _coreNativeCardAppearanceOperations.EffectiveSuit(actor, card))).Select(target => target.Seat).ToArray());
    internal IReadOnlyList<int>? DragonBoatExpectedNativeTargets(CharacterState actor, Card card, LegalAction action) => (action.PlayedCardKind ?? card.Kind) switch
    {
        CardKind.StriveUpstream => DragonBoatGlobalTargets(actor, card),
        CardKind.RealgarWine => [actor.Seat],
        CardKind.Zong or CardKind.SameBoat or CardKind.AgainstCurrent => action.TargetSeats,
        _ => null
    };
    internal void AddDragonBoatCardActions(List<LegalAction> actions, CharacterState actor, IEnumerable<Card> cards)
    {
        foreach (var card in cards.Where(card => IsDragonBoatCard(card.Kind)))
            AddDragonBoatPhysicalOrConvertedActions(actions, actor, card, card.Kind, null);
    }

    internal void AddDragonBoatPhysicalOrConvertedActions(List<LegalAction> actions, CharacterState actor, Card card, CardKind kind, CardConversionSource? conversion)
    {
        if (_coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, kind, CardActionType.Use))
            return;
        if (kind == CardKind.StriveUpstream)
        {
            var targets = DragonBoatGlobalTargets(actor, card);
            if (targets.Count > 0)
                actions.Add(new(DragonBoatCardAction(kind), card.Id, null, "使用【力争上游】", conversion is null ? null : kind, TargetSeats: targets) { ConversionSource = conversion });
            return;
        }

        if (kind == CardKind.RealgarWine && (actor.HasAlcoholEffect || !(_characterHuangfuSongWumiaoOperations.HasWuHfOrDixianMaterialPrivilege(actor, CardKind.Alcohol, [card.Id], conversion) || _characterMiHengLeOperations.SiruiIgnoresAlcoholLimit(actor, conversion) || _characterHuangQuanOlOperations.CanUseJianjiOrdinaryAlcohol(actor, card.Id) || _coreTurnTargetCardQuotaAllowancesOperations.HasTargetCardQuotaAllowance(actor.Seat, actor.Seat) || _coreAdvancedPoliciesOperations.Value.HasNextUnlimitedCard(actor) || _characterZhugeLiangMouOperations.HasMouWujingAllowance(actor) || _coreAdvancedPoliciesOperations.Value.HasFriendQihuiAllowance(actor) || _coreProgramCardPoliciesOperations.HasCardPolicy(actor, SkillProgramCardPolicyKind.UnlimitedAlcoholUse, CardKind.Alcohol))))
            return;
        foreach (var target in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && (kind switch
        {
            CardKind.Zong => DragonBoatCardAllies(actor.Seat, p.Seat) && _coreTreatedWoundedOperations.IsRuleWounded(p),
            CardKind.RealgarWine => p.Seat == actor.Seat,
            CardKind.SameBoat => DragonBoatCardAllies(actor.Seat, p.Seat),
            CardKind.AgainstCurrent => !DragonBoatCardAllies(actor.Seat, p.Seat),
            _ => false
        })))
        {
            if (_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, target.Seat, kind) || _coreNativeDamagePoliciesOperations.IsCardTargetProhibited(target, kind, card.Suit, CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(actor, card))) || _coreShenwuEquipmentOperations.IsShenwuRobeTargetProhibited(actor.Seat, target.Seat, kind) || _coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, target.Seat, _coreNativeCardAppearanceOperations.EffectiveSuit(actor, card)))
                continue;
            actions.Add(new(DragonBoatCardAction(kind), card.Id, kind == CardKind.RealgarWine ? null : target.Seat, _coreCardConversionsOperations.DescribeConversion(conversion, $"对 {(target.Name)} 使用【{(CardCatalog.Get(kind).DisplayName)}】"), conversion is null ? null : kind) { ConversionSource = conversion });
        }
    }

    internal void ResolveDragonBoatCard(CharacterState actor, Card card, LegalAction action)
    {
        var kind = action.PlayedCardKind ?? card.Kind;
        if (!IsDragonBoatCard(kind))
            throw new InvalidOperationException("A Dragon Boat action lost its effective card name.");
        if (kind == CardKind.Zong)
        {
            var target = _coreMatchParticipantsPlayersAccess._players[action.TargetSeat ?? throw new InvalidOperationException("Zong needs its selected allied target.")];
            if (!DragonBoatCardAllies(actor.Seat, target.Seat) || !_coreTreatedWoundedOperations.IsRuleWounded(target))
                throw new InvalidOperationException("Zong lost its living wounded public ally.");
            _coreNativeRecoveryCardsOperations.ResolveRecoveryCard(actor, target, card, "粽", CardKind.Zong, conversionSource: action.ConversionSource, physicalCards: card.Id == 0 ? [] : null);
            return;
        }

        var targets = kind == CardKind.StriveUpstream ? DragonBoatGlobalTargets(actor, card) : kind == CardKind.RealgarWine ? Array.AsReadOnly(new[] { actor.Seat }) : Array.AsReadOnly(new[] { action.TargetSeat ?? throw new InvalidOperationException("A Dragon Boat trick needs its selected target.") });
        var id = _coreNativeCardUseOperations.BeginCardUse(card, actor.Seat, targets, kind, conversionSource: action.ConversionSource, physicalCardIds: card.Id == 0 ? [] : null);
        if (card.Id != 0)
            _corePhysicalMovementFacadesOperations.MoveCard(card, _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(actor, card), CardLocation.Processing, CardMoveReasons.Use, disclosure: CardMovementDisclosure.FaceUp);
        if (kind == CardKind.RealgarWine)
        {
            if (!_characterFuRongOlOperations.HasIssuedFuRongUnlimitedUse(id, actor.Seat) && !_coreRoundGainedSourceUsesOperations.HasIssuedRoundGainedBasicBonus(id, actor.Seat) && !_characterMiHengLeOperations.HasIssuedSiruiUse(id, actor.Seat))
                _characterHuangGaiMouMobileOperations.RecordMouZhaJiangAwareAlcoholDebit(id, actor);
            _coreCardUseLifecycleOperations.BeginSimpleCardUse(id, new(card.Id, SimpleCardUseEffect.Alcohol));
            return;
        }

        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(id, card, actor.Seat, targets, DragonBoatCardAction(kind), playedCardKind: kind);
    }

    internal void ResolveDragonBoatTrickEffect(NullificationWindowFrame pending)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame use || use.Id != pending.ParentFrameId || !IsDragonBoatTrick(CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use)) || pending.EffectCardKind != CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use) || pending.ActionKind != DragonBoatCardAction(CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use)) || use.DragonBoatCardEffect is not null || use.Action is not { Type: CardActionType.Use } action || use.SourceSeat != pending.SourceSeat || use.TargetIndex < 0 || use.TargetIndex >= CharacterZhugeGuoLeRuntime.LeChengyanNativeTargets(use).Count || CoreNativeNullificationFramesRuntime.GetNullificationTargetSeat(pending)is not { } target || CharacterZhugeGuoLeRuntime.LeChengyanNativeTargets(use)[use.TargetIndex] != target)
            throw new InvalidOperationException("A Dragon Boat trick lost its exact native effect and target cursor.");
        if (!_coreMatchParticipantsPlayersAccess._players[target].IsAlive)
        {
            _coreNativeCardUseCompletionOperations.FinishCardUse(use.Id, _coreNativeNullificationFramesOperations.GetNullificationEffectCard(pending), CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use));
            return;
        }

        var rank = DragonBoatCardRank(use.SourceSeat);
        var recipients = CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use) == CardKind.SameBoat ? _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && DragonBoatCardAllies(target, p.Seat)).OrderBy(p => _coreTableOrderOperations.RelativeTableOrder(target, p.Seat)).Select(p => p.Seat).ToArray() : CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use) == CardKind.StriveUpstream ? [target] : Array.Empty<int>();
        var count = CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use) == CardKind.SameBoat ? rank.Most ? 2 : 1 : CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use) == CardKind.StriveUpstream ? target == use.SourceSeat ? Math.Min(5, DragonBoatCardTeamScore(use.SourceSeat)) : 1 : 0;
        var receipt = new DragonBoatCardEffectReceipt(action.ActionId, pending.Id, CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use), target, use.TargetIndex, CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use) == CardKind.AgainstCurrent ? DragonBoatCardEffectStage.AwaitingDamage : DragonBoatCardEffectStage.Drawing)
        {
            Recipients = recipients,
            RequestedCount = count,
            AwaitingProgramFrameId = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().LastOrDefault()is { } parent && CoreGameEngineSkillProgramsRuntime.IsAwaitingProgramMovement(parent) ? parent.Id : null
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(use with { Step = ResolutionFrameStep.ResolvingEffect, DragonBoatCardEffect = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DragonBoatCardEffectStartedEvent(use.Id, action.ActionId, pending.Id, CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use), target, use.TargetIndex, recipients.Length, count));
        if (CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use) == CardKind.AgainstCurrent)
        {
            var effectCard = _coreNativeNullificationFramesOperations.GetNullificationEffectCard(pending);
            var attack = _kernelCoreMainOperations.CreateCardAttackHandle(use.Id, use.SourceSeat, target, effectCard.Id == 0 ? null : effectCard, playedCardKind: CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use), damageNatureOverride: DamageNature.Normal, physicalCards: _coreNativeTrickUseOperations.GetCardUsePhysicalCards(use.Id));
            _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
            if (!_coreNativeDamageApplicationOperations.ApplyAttackDamage(attack))
                _coreNativeAttackCompletionOperations.CompleteAttack(attack);
            return;
        }

        ContinueDragonBoatCardEffect(use.Id);
    }

    internal bool TryCompleteDragonBoatAgainstCurrentAttack(NativeOwners.CardAttackHandle attack)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { DragonBoatCardEffect: { Kind: CardKind.AgainstCurrent } r } use)
            return false;
        if (r.Stage == DragonBoatCardEffectStage.FinishingAttack)
        {
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, current => current with { DragonBoatCardEffect = null });
            return false;
        }

        if (r.Stage != DragonBoatCardEffectStage.AwaitingDamage || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != use.Id)
            throw new InvalidOperationException("Against Current repeated its already returned native damage.");
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = null;
        _coreNativeCardContinuationCursorsOperations.ActiveDuel = null;
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        // ActiveCardAttack is a frame projection: clearing it replaced the
        // stored CardAttackState. Preserve that new inactive state instead of
        // restoring the captured pre-return frame while its tail owns prompts.
        use = _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)!;
        var rank = DragonBoatCardRank(use.SourceSeat);
        _coreRuntimeOperations.ReplaceRuntimeTop(use with { DragonBoatCardEffect = r with { Stage = DragonBoatCardEffectStage.Ranking, MostTreasures = rank.Most, LeastTreasures = rank.Least } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DragonBoatAgainstCurrentRankingEvent(use.Id, r.NullificationFrameId, DragonBoatCardTeamScore(use.SourceSeat), rank.Most, rank.Least));
        ContinueDragonBoatCardEffect(use.Id);
        return true;
    }
    internal IReadOnlyList<Card> DragonBoatDiscardableCards(int seat) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[seat])).Where(card => !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(seat, card, _cardZonesOperations._cardZones.GetLocation(card.Id), OwnedCardMoveIntent.Discard)).ToArray();
    internal void BeginDragonBoatDiscard(CardUseFrame use)
    {
        var choices = DragonBoatDiscardableCards(use.SourceSeat).Select(card => new PromptChoice(new ChoiceId($"dragon-boat.against-current.{(use.Id)}.{(use.TargetIndex)}.discard-{(card.Id)}"), $"弃置【{(card.DisplayName)}】", [card.Id], [], new Dictionary<string, string> { ["program-action"] = "dragon-boat-discard", ["frame-id"] = use.Id.ToString(CultureInfo.InvariantCulture) })).ToArray();
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, use.SourceSeat, "【逆水行舟】己队龙船至宝并列最多，须弃置一张牌。", choices.SelectMany(c => c.Cards).ToArray(), [], use.SourceSeat, CardKind.AgainstCurrent)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = Array.AsReadOnly(choices)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[use.SourceSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    internal bool HasDragonBoatDiscardPrompt() => _resolutionStackOperations._resolutionStack.LastOrDefault()is CardUseFrame { DragonBoatCardEffect.Stage: DragonBoatCardEffectStage.ChoosingDiscard } use && _corePendingDecisionPolicyPipelineOperations._pendingDecision is { Kind: DecisionKind.ProgramTrigger, IncomingCard: CardKind.AgainstCurrent } p && p.PlayerSeat == use.SourceSeat;
    [ProgramAnswer("dragon-boat-discard")]
    internal void ResolveDragonBoatDiscard(PromptChoice selected)
    {
        if (!HasDragonBoatDiscardPrompt() || _resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame { DragonBoatCardEffect: { } r } use || selected.Cards is not [var cardId] || selected.Targets.Count != 0 || selected.Parameters.GetValueOrDefault("frame-id") != use.Id.ToString(CultureInfo.InvariantCulture) || !_corePendingDecisionPolicyPipelineOperations._pendingDecision!.Choices.Any(choice => choice.Id == selected.Id) || DragonBoatDiscardableCards(use.SourceSeat).SingleOrDefault(card => card.Id == cardId)is not { } card)
            throw new InvalidOperationException("Against Current changed its exact mandatory self-discard choice.");
        var from = _cardZonesOperations._cardZones.GetLocation(card.Id);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(use with { DragonBoatCardEffect = r with { Stage = DragonBoatCardEffectStage.DiscardChildren, DiscardCardId = card.Id, DiscardFrom = from } });
        _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(use.SourceSeat, use.SourceSeat, card, from, new("card.effect.dragon-boat.against-current"), movement => _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, current => current with { DragonBoatCardEffect = current.DragonBoatCardEffect!with { DiscardSequence = movement.Sequence } }));
        ContinueDragonBoatCardEffect(use.Id);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.BeforeDeclaration, 43000)]
    internal bool TryAdvanceDragonBoatDiscardAi()
    {
        if (!HasDragonBoatDiscardPrompt() || _coreMatchParticipantsPlayersAccess._players[_corePendingDecisionPolicyPipelineOperations._pendingDecision!.PlayerSeat].IsHuman)
            return false;
        ResolveDragonBoatDiscard(_corePendingDecisionPolicyPipelineOperations._pendingDecision.Choices.OrderBy(choice => CardCatalog.Get(_coreAdvancedLifecycleOperations.GetAdvancedCard(choice.Cards.Single()).Kind).HandKeepValue).ThenBy(choice => choice.Cards.Single()).First());
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        return true;
    }

    [NativeFrameRoute(typeof(CardUseFrame), 60, typeof(NativeFrameRouteConditions), "DragonBoatCardEffect")]
    internal void ContinueDragonBoatCardEffect(long id)
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is CardUseFrame { DragonBoatCardEffect: { } r } use && use.Id == id)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
                return;
            if (r.Stage is DragonBoatCardEffectStage.AwaitingDamage or DragonBoatCardEffectStage.FinishingAttack)
                return;
            if (r.Stage == DragonBoatCardEffectStage.Ranking)
            {
                if (_coreMatchParticipantsPlayersAccess._players[use.SourceSeat].IsAlive && r.MostTreasures && DragonBoatDiscardableCards(use.SourceSeat).Count > 0)
                {
                    _coreRuntimeOperations.ReplaceRuntimeTop(use = use with { DragonBoatCardEffect = r with { Stage = DragonBoatCardEffectStage.ChoosingDiscard } });
                    BeginDragonBoatDiscard(use);
                    return;
                }

                _coreRuntimeOperations.ReplaceRuntimeTop(use with { DragonBoatCardEffect = r with { Stage = DragonBoatCardEffectStage.DiscardChildren } });
                continue;
            }

            if (r.Stage == DragonBoatCardEffectStage.ChoosingDiscard)
            {
                BeginDragonBoatDiscard(use);
                return;
            }

            if (r.Stage is DragonBoatCardEffectStage.DiscardChildren or DragonBoatCardEffectStage.DrawingChildren)
            {
                if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.DragonBoatCardEffect) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.DragonBoatCardEffect) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.DragonBoatCardEffect) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id))
                    return;
                use = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
                r = use.DragonBoatCardEffect!;
                _coreRuntimeOperations.ReplaceRuntimeTop(use with { DragonBoatCardEffect = r.Stage == DragonBoatCardEffectStage.DiscardChildren ? r with { Stage = DragonBoatCardEffectStage.Drawing, Recipients = r.LeastTreasures ? [use.SourceSeat] : [], RequestedCount = 1 } : r with { Stage = DragonBoatCardEffectStage.Drawing, RecipientIndex = r.RecipientIndex + 1, DrawOutcome = null } });
                continue;
            }

            if (r.Stage != DragonBoatCardEffectStage.Drawing)
                throw new InvalidOperationException("A Dragon Boat card lost its native effect stage.");
            if (r.RecipientIndex >= r.Recipients.Count)
            {
                if (CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use) == CardKind.AgainstCurrent)
                {
                    _coreRuntimeOperations.ReplaceRuntimeTop(use with { DragonBoatCardEffect = r with { Stage = DragonBoatCardEffectStage.FinishingAttack } });
                    var attack = _kernelCoreMainOperations.CreateCardAttackHandle2(id);
                    _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
                    _coreNativeAttackCompletionOperations.CompleteAttack(attack);
                    return;
                }

                _coreRuntimeOperations.ReplaceRuntimeTop(use with { DragonBoatCardEffect = null, NativeDraw = null });
                _coreNativeCardUseCompletionOperations.FinishCardUse(id, _coreDamageColorDuelProgramsOperations.GetTrickRepresentation(id, use.CardId, requireProcessing: true), CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use));
                return;
            }

            var recipient = r.Recipients[r.RecipientIndex];
            if (use.NativeDraw is null)
                _coreRuntimeOperations.ReplaceRuntimeTop(use = use with { NativeDraw = new NativeDrawProducerReceipt(0, r.RequestedCount, null, SkillProgramCardSetVisibility.Private, CardMoveReasons.Draw) { Recipients = [recipient], ReturnKind = NativeDrawAttemptReturnKind.DragonBoatCardEffect } });
            var paid = use.NativeDraw!;
            if (paid.ActiveAttemptId is not null && !paid.AttemptReturned)
                throw new InvalidOperationException("A Dragon Boat draw resumed before its request returned.");
            if (!paid.AttemptReturned && _coreNativeDrawAttemptsOperations.TryBeginNativeDrawAttempt(use))
                return;
            var before = _coreOutsidePhaseDrawDiscardOperations.OutsidePhaseMovementSequence;
            var ids = paid.Replacement is null && _coreMatchParticipantsPlayersAccess._players[recipient].IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None ? _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[recipient], r.RequestedCount, true, CardMoveReasons.Draw) : Array.AsReadOnly(Array.Empty<int>());
            var outcome = new NativeDrawOutcome(recipient, r.RequestedCount, paid.ActiveAttemptId, before, _coreOutsidePhaseDrawDiscardOperations.OutsidePhaseMovementSequence, paid.Replacement)
            {
                ActualDrawIds = ids
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(_coreCardUseLifecycleOperations.LifecycleCardUse(id)!with { NativeDraw = null, DragonBoatCardEffect = r with { Stage = DragonBoatCardEffectStage.DrawingChildren, DrawOutcome = outcome } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DragonBoatCardDrawAppliedEvent(id, r.NullificationFrameId, r.RecipientIndex, recipient, r.RequestedCount, ids.Count, before, outcome.SequenceAfter));
        }
    }

    internal bool IsDragonBoatCardEffectMovement(CardUseFrame use, CardMovementBatchContext batch) => use.DragonBoatCardEffect is { } r && batch.ParentFrameId == use.Id && batch.AwaitingProgramFrameId == r.AwaitingProgramFrameId && batch.Movements.Count > 0 && (r.Stage == DragonBoatCardEffectStage.DrawingChildren && r.DrawOutcome is { } outcome && batch.Movements.All(m => m.Sequence > outcome.SequenceBefore && m.Sequence <= outcome.SequenceAfter && (outcome.ActualDrawIds.Contains(m.CardId) && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(outcome.RecipientSeat) && m.Reason == CardMoveReasons.Draw || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle)) || r.Stage == DragonBoatCardEffectStage.DiscardChildren && r.DiscardSequence > 0 && batch.Movements is [var move] && move.Sequence == r.DiscardSequence && move.CardId == r.DiscardCardId && move.From == r.DiscardFrom && move.To == CardLocation.DiscardPile && move.Reason == new CardMoveReason("card.effect.dragon-boat.against-current"));
    internal bool IsDragonBoatCardEffectMovementReturn(CardUseFrame use, CardsMovedTriggerWindowFrame frame) => frame.ResumeDragonBoatCardEffectFrameId == use.Id && frame.Id == frame.Batch.Id && frame.ResumeProgramFrameId is null && !CoreDamageObserverCursorQueriesRuntime.HasAlternateProgramMovementReturn(frame with { ResumeDragonBoatCardEffectFrameId = null }) && IsDragonBoatCardEffectMovement(use, frame.Batch);
    internal bool IsDragonBoatCardEffectHpReturn(CardUseFrame use, HpChangedTriggerWindowFrame hp) => use.DragonBoatCardEffect is { Stage: DragonBoatCardEffectStage.DrawingChildren or DragonBoatCardEffectStage.DiscardChildren } && hp.Continuation == PostEventContinuation.DragonBoatCardEffect && hp.ResumeFrameId == use.Id && hp.Change.ParentFrameId == use.Id && hp.CardId is null && hp.CardKind is null;
    internal bool IsDragonBoatCardEffectRecoveryReturn(CardUseFrame use, RecoveryReplacementFrame recovery) => recovery.ParentFrameId == use.Id && recovery.Return == new RecoveryReplacementReturn(PostEventContinuation.DragonBoatCardEffect, use.Id) && IsDragonBoatCardRecoveryAttempt(use, recovery.Attempt);
    internal bool IsDragonBoatCardRecoveryAttempt(CardUseFrame use, RecoveryAttempt attempt) => use.DragonBoatCardEffect is { Stage: DragonBoatCardEffectStage.DiscardChildren, DiscardCardId: { } card, DiscardFrom: { Zone: CardZoneKind.Equipment } from, DiscardSequence: > 0 } r && from.OwnerSeat == use.SourceSeat && attempt.SourceSeat == use.SourceSeat && attempt.TargetSeat == use.SourceSeat && attempt.Amount == 1 && attempt.Completion == new RecoveryAttemptCompletion(RecoveryAttemptProducer.SilverLion, MoveReason: new("card.effect.dragon-boat.against-current")) && _coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m.Sequence == r.DiscardSequence && m.CardId == card && m.CardKind == CardKind.SilverLion && m.From == from && m.To == CardLocation.DiscardPile && m.Reason == new CardMoveReason("card.effect.dragon-boat.against-current"));
}
