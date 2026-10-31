#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeTrickUseRuntime
{
    internal void ResolveDrawTwo(CharacterState source, Card drawTwo)
    {
        var stillLegal = _coreNativeLegalActionsOperations.BuildLegalActions(source, selectedPhysicalCardId: drawTwo.Id).Any(action => action.Kind == LegalActionKind.DrawTwo && action.CardId == drawTwo.Id && action.TargetSeat is null);
        if (!stillLegal)
        {
            throw new InvalidOperationException("DrawTwo became illegal before resolution.");
        }

        var resolutionId = _coreNativeCardUseOperations.BeginCardUse(drawTwo, source.Seat, []);
        _corePhysicalMovementFacadesOperations.MoveCard(drawTwo, _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(source, drawTwo), CardLocation.Processing, CardMoveReasons.Use, disclosure: CardMovementDisclosure.FaceUp);
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(resolutionId, drawTwo, source.Seat, [], LegalActionKind.DrawTwo);
    }

    internal void ResolveAlcohol(CharacterState source, Card alcohol, CardConversionSource? conversionSource = null)
    {
        var stillLegal = _coreNativeLegalActionsOperations.BuildLegalActions(source, selectedPhysicalCardId: alcohol.Id).Any(action => action.Kind == LegalActionKind.Alcohol && action.CardId == alcohol.Id && action.ConversionSource == conversionSource && action.TargetSeat is null);
        if (!stillLegal)
        {
            throw new InvalidOperationException("Alcohol became illegal before resolution.");
        }

        _coreProgramAlcoholConversionsOperations.Value.ValidateAlcoholConversion(source, alcohol, conversionSource, forResponse: false);
        var resolutionId = _coreNativeCardUseOperations.BeginCardUse(alcohol, source.Seat, [], playedCardKind: CardKind.Alcohol, conversionSource: conversionSource);
        _corePhysicalMovementFacadesOperations.MoveCard(alcohol, _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(source, alcohol), CardLocation.Processing, CardMoveReasons.Use, disclosure: CardMovementDisclosure.FaceUp);
        // Spend the allowance at commitment, before an optional trigger can pause.
        if (!_characterFuRongOlOperations.HasIssuedFuRongUnlimitedUse(resolutionId, source.Seat) && !_coreRoundGainedSourceUsesOperations.HasIssuedRoundGainedBasicBonus(resolutionId, source.Seat) && !_characterMiHengLeOperations.HasIssuedSiruiUse(resolutionId, source.Seat))
            _characterHuangGaiMouMobileOperations.RecordMouZhaJiangAwareAlcoholDebit(resolutionId, source);
        _coreCardUseLifecycleOperations.BeginSimpleCardUse(resolutionId, new(alcohol.Id, SimpleCardUseEffect.Alcohol));
    }

    internal void CompleteAlcoholUse(CharacterState source, Card alcohol, long resolutionId)
    {
        var use = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(frame => frame.Id == resolutionId);
        var target = _characterZhouYuOlMouOperations.MouZuifengAlcoholBeneficiary(use, (_coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)?.AdjustedSimpleContinuation is not null) ? _coreMatchParticipantsPlayersAccess._players[use.TargetSeats[use.TargetIndex]] : source);
        if (target.IsAlive && !_coreProgramCardTargetEffectsOperations.IsCardEffectIneffective(resolutionId, target.Seat))
        {
            _coreExtraCardEffectRoundsOperations.ApplyExtraCapableAlcoholEffect(resolutionId, target);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AlcoholAppliedEvent(resolutionId, target.Seat, DamageBonus: 1));
            _coreMatchLogOperations.AddLog("CardEffect", $"{(source.Name)} 使用【酒】，{(target.Name)} 的下一张直接杀造成的伤害 +1。", source.Seat, target.Seat);
        }

        if (!_coreNextCardTargetAdjustmentsOperations.HasRemainingAdjustedSimpleTargets(resolutionId))
            _coreAlternativeViewAsCostsOperations.FinishSingleBasicCardUseCost(use, alcohol);
        _coreNativeCardUseCompletionOperations.FinishCardUse(resolutionId, alcohol, use.CardKind);
    }

    internal void ResolveGroupCard(CharacterState source, Card groupCard, LegalActionKind actionKind, CardKind requiredCardKind)
    {
        var stillLegal = _coreNativeLegalActionsOperations.BuildLegalActions(source, selectedPhysicalCardId: groupCard.Id).Any(action => action.Kind == actionKind && action.CardId == groupCard.Id && action.TargetSeat is null);
        if (!stillLegal)
        {
            throw new InvalidOperationException($"{(groupCard.Kind)} became illegal before resolution.");
        }

        var targets = Enumerable.Range(1, _coreMatchDefinitionPlayerCountAccess._playerCount - 1).Select(offset => _coreMatchParticipantsPlayersAccess._players[_coreTableOrderOperations.TableStepSeat(source.Seat, offset)]).Where(player => player.IsAlive && !_coreProgramCardPoliciesOperations.HasCardPolicy(player, SkillProgramCardPolicyKind.ExcludeGlobalTarget, groupCard.Kind) && !_coreProgramTargetRulesOperations.TargetRules.IsDeclaredGlobalProhibited(new(source.Seat, player.Seat, groupCard.Kind)) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(player, groupCard.Kind, groupCard.Suit, CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(source, groupCard))) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(source.Seat, player.Seat, _coreNativeCardAppearanceOperations.EffectiveSuit(source, groupCard))).Select(player => player.Seat).ToArray();
        var resolutionId = _coreNativeCardUseOperations.BeginCardUse(groupCard, source.Seat, targets);
        _corePhysicalMovementFacadesOperations.MoveCard(groupCard, _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(source, groupCard), CardLocation.Processing, CardMoveReasons.Use, disclosure: CardMovementDisclosure.FaceUp);
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(resolutionId, groupCard, source.Seat, targets, actionKind, requiredCardKind: requiredCardKind);
    }

    internal IReadOnlyList<Card> GetCardUsePhysicalCards(long resolutionId)
    {
        var frame = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(cardUse => cardUse.Id == resolutionId);
        var processing = _cardZonesOperations._cardZones.CardsAt(CardLocation.Processing);
        return (frame.PhysicalCardIds ?? [frame.CardId]).Select(cardId => (_coreCurrentUsePhysicalClaimsOperations.IsClaimedUseCardEntity(resolutionId, cardId) || _characterSunYuanOlOperations.IsSayingRemovedMaterial(resolutionId, cardId) || (_coreHellSoulCardsOperations.IsHellDestroyedSoulMaterial(resolutionId, cardId) || (_characterHuBanOlOperations.IsHuiyunOutsideGeneralWeaponMaterial(resolutionId, cardId) || _characterMiHengLeOperations.IsSiruiOutsideGeneralWeaponMaterial(resolutionId, cardId)) || (_coreGuanduEquipmentOperations.IsGuanduDestroyedChariotMaterial(resolutionId, cardId) || _coreEphemeralCardMaterialsOperations.IsEphemeralCardDestroyedMaterial3(resolutionId, cardId)))) ? _coreResponseEntityExchangeOperations.EntityAtCurrentLocation(cardId) : processing.Single(card => card.Id == cardId)).ToArray();
    }

    internal void ResolvePeachGarden(CharacterState source, Card peachGarden)
    {
        var stillLegal = _coreNativeLegalActionsOperations.BuildLegalActions(source, selectedPhysicalCardId: peachGarden.Id).Any(action => action.Kind == LegalActionKind.PeachGarden && action.CardId == peachGarden.Id && action.TargetSeat is null);
        if (!stillLegal)
        {
            throw new InvalidOperationException("PeachGarden became illegal before resolution.");
        }

        var targets = _coreCardUseModulesOperations.ApplyTurnCardGroupTargetRestrictions(source, Enumerable.Range(0, _coreMatchDefinitionPlayerCountAccess._playerCount).Select(offset => _coreMatchParticipantsPlayersAccess._players[_coreTableOrderOperations.TableStepSeat(source.Seat, offset)]).Where(player => player.IsAlive && !_coreProgramTargetRulesOperations.TargetRules.IsDeclaredGlobalProhibited(new(source.Seat, player.Seat, CardKind.PeachGarden)) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(player, CardKind.PeachGarden, peachGarden.Suit, CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(source, peachGarden))) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(source.Seat, player.Seat, _coreNativeCardAppearanceOperations.EffectiveSuit(source, peachGarden))).Select(player => player.Seat).ToArray());
        var resolutionId = _coreNativeCardUseOperations.BeginCardUse(peachGarden, source.Seat, targets);
        _corePhysicalMovementFacadesOperations.MoveCard(peachGarden, _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(source, peachGarden), CardLocation.Processing, CardMoveReasons.Use, disclosure: CardMovementDisclosure.FaceUp);
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(resolutionId, peachGarden, source.Seat, targets, LegalActionKind.PeachGarden);
    }

    internal void ResolveFiveGrains(CharacterState source, Card fiveGrains)
    {
        var stillLegal = _coreNativeLegalActionsOperations.BuildLegalActions(source, selectedPhysicalCardId: fiveGrains.Id).Any(action => action.Kind == LegalActionKind.FiveGrains && action.CardId == fiveGrains.Id && action.TargetSeat is null);
        if (!stillLegal)
        {
            throw new InvalidOperationException("FiveGrains became illegal before resolution.");
        }

        var aliveSeats = _coreCardUseModulesOperations.ApplyTurnCardGroupTargetRestrictions(source, Enumerable.Range(0, _coreMatchDefinitionPlayerCountAccess._playerCount).Select(offset => _coreMatchParticipantsPlayersAccess._players[_coreTableOrderOperations.TableStepSeat(source.Seat, offset)]).Where(player => player.IsAlive && !_coreProgramTargetRulesOperations.TargetRules.IsDeclaredGlobalProhibited(new(source.Seat, player.Seat, CardKind.FiveGrains)) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(player, CardKind.FiveGrains, fiveGrains.Suit, CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(source, fiveGrains))) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(source.Seat, player.Seat, _coreNativeCardAppearanceOperations.EffectiveSuit(source, fiveGrains))).Select(player => player.Seat).ToArray());
        var availableCards = _cardZonesOperations._cardZones.Count(CardLocation.DrawPile) + _cardZonesOperations._cardZones.Count(CardLocation.DiscardPile);
        var targets = aliveSeats.Take(availableCards).ToArray();
        var resolutionId = _coreNativeCardUseOperations.BeginCardUse(fiveGrains, source.Seat, targets);
        _corePhysicalMovementFacadesOperations.MoveCard(fiveGrains, _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(source, fiveGrains), CardLocation.Processing, CardMoveReasons.Use, disclosure: CardMovementDisclosure.FaceUp);
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(resolutionId, fiveGrains, source.Seat, targets, LegalActionKind.FiveGrains);
    }

    internal void ResolveIronChain(CharacterState source, Card ironChain, IReadOnlyList<int> targetSeats, CardKind? playedCardKind = null)
    {
        var targets = targetSeats.ToArray();
        var stillLegal = _coreNativeLegalActionsOperations.BuildLegalActions(source, selectedPhysicalCardId: ironChain.Id).Any(action => action.Kind == LegalActionKind.IronChain && action.CardId == ironChain.Id && action.TargetSeats.SequenceEqual(targets) && action.PlayedCardKind == playedCardKind);
        if (!stillLegal)
        {
            throw new InvalidOperationException("IronChain became illegal before resolution.");
        }

        var resolutionId = _coreNativeCardUseOperations.BeginCardUse(ironChain, source.Seat, targets, playedCardKind);
        _corePhysicalMovementFacadesOperations.MoveCard(ironChain, _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(source, ironChain), CardLocation.Processing, CardMoveReasons.IronChainUse);
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(resolutionId, ironChain, source.Seat, targets, LegalActionKind.IronChain, playedCardKind: playedCardKind);
    }

    internal void ResolveDelayedCard(CharacterState source, CharacterState target, Card delayedCard, LegalActionKind actionKind, CardKind? playedCardKind = null)
    {
        var effectiveCardKind = playedCardKind ?? delayedCard.Kind;
        var stillLegal = _coreNativeLegalActionsOperations.BuildLegalActions(source, selectedPhysicalCardId: delayedCard.Id).Any(action => action.Kind == actionKind && action.CardId == delayedCard.Id && action.TargetSeat == target.Seat && (action.PlayedCardKind ?? delayedCard.Kind) == effectiveCardKind);
        if (!stillLegal)
        {
            throw new InvalidOperationException($"{(CardCatalog.Get(effectiveCardKind).DisplayName)} became illegal before resolution.");
        }

        var resolutionId = _coreNativeCardUseOperations.BeginCardUse(delayedCard, source.Seat, [target.Seat], effectiveCardKind);
        _corePhysicalMovementFacadesOperations.MoveCard(delayedCard, _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(source, delayedCard), CardLocation.Processing, CardMoveReasons.Use, disclosure: CardMovementDisclosure.FaceUp);
        if (effectiveCardKind == CardKind.Lightning && !_coreSoleTargetProtectionOperations.Value.HasSoleTargetProtectionCapability && _coreCardActionsOperations.TryBeginDelayedCardUsePrograms(resolutionId))
        {
            return;
        }

        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(resolutionId, delayedCard, source.Seat, [target.Seat], actionKind, playedCardKind: effectiveCardKind);
    }

    internal void ResolveDelayedCardEffect(NullificationWindowFrame pending)
    {
        if (_coreOriginalUseLightningOperations.PlaceOriginalUseLightning(pending))
            return;
        if (_characterCuiLieAnniversaryOperations.TrySkipAnniversaryCuiLieDelayedTarget(pending))
            return;
        if (_coreCurrentUsePhysicalClaimsOperations.IsClaimedUseCardEntity(pending.ParentFrameId, pending.EffectCardId) || _cardZonesOperations._cardZones.GetLocation(pending.EffectCardId) == CardLocation.OutsideGame && _coreEphemeralCardMaterialsOperations.IsEphemeralCardDestroyedMaterial3(pending.ParentFrameId, pending.EffectCardId))
        {
            _coreNativeCardUseCompletionOperations.FinishCardUse(pending.ParentFrameId, _coreNativeNullificationFramesOperations.GetNullificationEffectCard(pending), pending.EffectCardKind);
            _coreMatchLogOperations.AddLog("CardEffect", "延时锦囊的原实体已离开处理区，不能置入判定区。", pending.SourceSeat);
            return;
        }

        var source = _coreMatchParticipantsPlayersAccess._players[pending.SourceSeat];
        var targetSeat = CoreNativeNullificationFramesRuntime.GetNullificationTargetSeat(pending) ?? throw new InvalidOperationException("A delayed card must retain one target.");
        var target = _coreMatchParticipantsPlayersAccess._players[targetSeat];
        var expectedActionKind = pending.EffectCardKind switch
        {
            CardKind.Indulgence => LegalActionKind.Indulgence,
            CardKind.SupplyShortage => LegalActionKind.SupplyShortage,
            CardKind.Lightning => LegalActionKind.Lightning,
            _ => throw new InvalidOperationException($"Card {(_coreNativeNullificationFramesOperations.GetNullificationEffectCard(pending).Kind)} is not a delayed card.")};
        if (_characterLiuYanOlOperations.TrySkipUnavailableLiuYanLimuEffect(pending))
            return;
        if (_characterQuHuangOlOperations.Value.TrySkipUnavailableQuHuangLightning(pending))
            return;
        var canTargetSelf = pending.EffectCardKind == CardKind.Lightning || _characterLiuYanOlOperations.IsLiuYanLimuSelfDelayedUse(pending);
        if (pending.EffectCardKind == CardKind.SupplyShortage && false && _corePhysicalOwnedCardQueriesOperations.GetHand(target).Count == 0)
        {
            _coreNativeTrickEffectsOperations.SkipUnavailableTargetCardEffect(pending, target, CardEffectSkipReason.TargetHandEmpty);
            return;
        }

        if (_coreProgramCardPoliciesOperations.HasCardPolicy(target, SkillProgramCardPolicyKind.ProhibitDelayedTrickTarget))
        {
            _coreNativeTrickEffectsOperations.SkipUnavailableTargetCardEffect(pending, target, CardEffectSkipReason.SkillNullified);
            return;
        }

        if (!target.IsAlive || target.JudgmentAreaAbolished || (!canTargetSelf && target.Seat == source.Seat) || pending.ActionKind != expectedActionKind || _coreActualTurnsOperations.HasJudgmentEffectiveCard(target, pending.EffectCardKind))
        {
            throw new InvalidOperationException($"{(CardCatalog.Get(pending.EffectCardKind).DisplayName)} target is no longer legal.");
        }

        _coreNativeCardUseFramesOperations.SetCardUseStep(pending.ParentFrameId, ResolutionFrameStep.ResolvingEffect);
        _corePhysicalMovementFacadesOperations.MoveCard(_coreNativeNullificationFramesOperations.GetNullificationEffectCard(pending), CardLocation.Processing, CardLocation.Judgment(target.Seat), CardMoveReasons.DelayedCardPlace);
        if (pending.EffectCardKind != _coreNativeNullificationFramesOperations.GetNullificationEffectCard(pending).Kind)
        {
            _coreJudgmentEffectiveCardsJudgmentEffectiveCardKindsAccess._judgmentEffectiveCardKinds[pending.EffectCardId] = pending.EffectCardKind;
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DelayedCardPlacedEvent(pending.ParentFrameId, pending.EffectCardId, pending.EffectCardKind, source.Seat, target.Seat));
        if (!_coreExtraCardEffectRoundsOperations.IsExtraCardEffectRound(pending.ParentFrameId))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(pending.EffectCardId, pending.EffectCardKind, source.Seat, target.Seat));
        _coreMatchLogOperations.AddLog("CardEffect", $"{(source.Name)} 对 {(target.Name)} 使用【{(CardCatalog.Get(pending.EffectCardKind).DisplayName)}】，置入其判定区。", source.Seat, target.Seat);
        _coreNativeCardUseCompletionOperations.FinishCardUse(pending.ParentFrameId, _coreNativeNullificationFramesOperations.GetNullificationEffectCard(pending), pending.EffectCardKind);
    }

    internal void ResolveDismantlement(CharacterState source, CharacterState target, Card dismantlement, int? targetCardId, CardKind? playedCardKind = null, TargetHandCardPreselection? preselection = null) => BeginTargetCardEffect(source, target, dismantlement, LegalActionKind.Dismantlement, GameEngine.TargetCardEffect.Discard, targetCardId, playedCardKind, preselection);
    internal void ResolveSnatch(CharacterState source, CharacterState target, Card snatch, int? targetCardId, CardKind? playedCardKind = null, TargetHandCardPreselection? preselection = null) => BeginTargetCardEffect(source, target, snatch, LegalActionKind.Snatch, GameEngine.TargetCardEffect.Take, targetCardId, playedCardKind, preselection);
    internal void BeginTargetCardEffect(CharacterState source, CharacterState target, Card effectCard, LegalActionKind actionKind, GameEngine.TargetCardEffect effect, int? targetCardId, CardKind? playedCardKind = null, TargetHandCardPreselection? preselection = null)
    {
        var effectiveCardKind = playedCardKind ?? effectCard.Kind;
        var stillLegal = _coreNativeLegalActionsOperations.BuildLegalActions(source, selectedPhysicalCardId: effectCard.Id).Any(action => action.Kind == actionKind && action.CardId == effectCard.Id && action.TargetSeat == target.Seat && action.TargetCardId == targetCardId && (action.PlayedCardKind ?? effectCard.Kind) == effectiveCardKind);
        if (!stillLegal)
        {
            throw new InvalidOperationException($"{(effectCard.Kind)} became illegal before resolution.");
        }

        if (targetCardId is { } requestedTargetCardId && _corePhysicalOwnedCardQueriesOperations.FindPublicTargetCard(target, requestedTargetCardId)is null)
        {
            throw new InvalidOperationException($"{(effectCard.Kind)} cannot resolve against the requested public equipment or judgment card.");
        }

        if (targetCardId is null)
        {
            var targetHand = _corePhysicalOwnedCardQueriesOperations.GetHand(target);
            if (targetHand.Count == 0)
            {
                throw new InvalidOperationException($"{(effectCard.Kind)} cannot resolve against an empty hand.");
            }
        }

        var resolutionId = _coreNativeCardUseOperations.BeginCardUse(effectCard, source.Seat, [target.Seat], effectiveCardKind);
        var sourceLocation = _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(source, effectCard);
        if (preselection is not null)
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(resolutionId, frame => frame with { TargetHandPreselection = preselection });
        _corePhysicalMovementFacadesOperations.MoveCard(effectCard, sourceLocation, CardLocation.Processing, CardMoveReasons.Use, disclosure: CardMovementDisclosure.FaceUp);
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(resolutionId, effectCard, source.Seat, [target.Seat], actionKind, targetCardId, playedCardKind: effectiveCardKind);
    }

    internal void ResolveFireAttack(CharacterState source, CharacterState target, Card fireAttack, CardKind? playedCardKind = null)
    {
        var stillLegal = _coreNativeLegalActionsOperations.BuildLegalActions(source, selectedPhysicalCardId: fireAttack.Id).Any(action => action.Kind == LegalActionKind.FireAttack && action.CardId == fireAttack.Id && action.TargetSeat == target.Seat && action.PlayedCardKind == playedCardKind);
        if (!stillLegal)
        {
            throw new InvalidOperationException("FireAttack became illegal before resolution.");
        }

        var resolutionId = _coreNativeCardUseOperations.BeginCardUse(fireAttack, source.Seat, [target.Seat], playedCardKind);
        _corePhysicalMovementFacadesOperations.MoveCard(fireAttack, _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(source, fireAttack), CardLocation.Processing, CardMoveReasons.Use, disclosure: CardMovementDisclosure.FaceUp);
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(resolutionId, fireAttack, source.Seat, [target.Seat], LegalActionKind.FireAttack, playedCardKind: playedCardKind);
    }

    internal void ResolveBorrowedSword(CharacterState source, CharacterState weaponOwner, CharacterState slashTarget, Card borrowedSword)
    {
        var targets = new[]
        {
            weaponOwner.Seat,
            slashTarget.Seat
        };
        var stillLegal = _coreNativeLegalActionsOperations.BuildLegalActions(source, selectedPhysicalCardId: borrowedSword.Id).Any(action => action.Kind == LegalActionKind.BorrowedSword && action.CardId == borrowedSword.Id && action.TargetSeats.SequenceEqual(targets));
        if (!stillLegal)
        {
            throw new InvalidOperationException("Borrowed Sword became illegal before resolution.");
        }

        var resolutionId = _coreNativeCardUseOperations.BeginCardUse(borrowedSword, source.Seat, targets, designatedTargetSeats: _coreGameEngineNextCardTargetAdjustmentsSelectedNextCardTargetSeatsAccess._selectedNextCardTargetSeats is { Count: >= 4 } selectedTargets ? selectedTargets.Where((_, index) => index % 2 == 0).ToArray() : [targets[0]]);
        _corePhysicalMovementFacadesOperations.MoveCard(borrowedSword, _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(source, borrowedSword), CardLocation.Processing, CardMoveReasons.Use, disclosure: CardMovementDisclosure.FaceUp);
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(resolutionId, borrowedSword, source.Seat, targets, LegalActionKind.BorrowedSword);
    }
}
