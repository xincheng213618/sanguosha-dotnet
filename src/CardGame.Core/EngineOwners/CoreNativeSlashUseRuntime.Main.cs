#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeSlashUseRuntime
{
    internal void ResolveSlash(CharacterState source, CharacterState target, Card slash, CardKind playedCardKind, CardConversionSource? conversionSource = null, IReadOnlyList<CardConversionSource>? additionalConversionSources = null)
    {
        var stillLegal = _coreNativeLegalActionsOperations.BuildLegalActions(source, selectedPhysicalCardId: slash.Id).Any(action => action.Kind == LegalActionKind.Slash && action.CardId == slash.Id && action.TargetSeat == target.Seat && (action.PlayedCardKind ?? slash.Kind) == playedCardKind && action.ConversionSource == conversionSource && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.ConversionSourcesEqual(action.AdditionalConversionSources, additionalConversionSources));
        if (!stillLegal)
        {
            throw new InvalidOperationException("Slash became illegal before resolution.");
        }

        ResolveSlashCore(source, target, slash, playedCardKind, source.Seat, usesZhuqueFan: playedCardKind == CardKind.FireSlash && (additionalConversionSources is null || additionalConversionSources.Count == 0) && !_coreCardConversionsOperations.IsDirectProgramFireSlashConversion(source, slash, conversionSource) && (slash.Kind == CardKind.Slash || _coreActionQueriesOperations.GetProgramCardIdentityMatches(source, slash).Any(match => match.Identity.OutputKind == CardKind.Slash)), conversionSource: conversionSource, additionalConversionSources: additionalConversionSources);
    }

    internal void ResolveFangtianHalberdSlash(CharacterState source, IReadOnlyList<CharacterState> targets, Card slash, CardKind playedCardKind, CardConversionSource? conversionSource = null, IReadOnlyList<CardConversionSource>? additionalConversionSources = null)
    {
        var targetSeats = targets.Select(target => target.Seat).ToArray();
        // The same current legal-action projection owns the target limit for
        // equipment and program modifiers, including every other living seat.
        var stillLegal = targetSeats.Length >= 2 && targetSeats.Length < _coreMatchParticipantsPlayersAccess._players.Count(p => p.IsAlive) && _coreNativeLegalActionsOperations.BuildLegalActions(source, selectedPhysicalCardId: slash.Id).Any(action => action.Kind == LegalActionKind.Slash && action.CardId == slash.Id && action.TargetSeats.SequenceEqual(targetSeats) && (action.PlayedCardKind ?? slash.Kind) == playedCardKind && action.ConversionSource == conversionSource && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.ConversionSourcesEqual(action.AdditionalConversionSources, additionalConversionSources));
        var usesFangtian = _coreNativeDamageHandlesOperations.UsesFormalFangtianHalberd && _corePhysicalOwnedCardQueriesOperations.GetHand(source).Count == 1 && _corePhysicalOwnedCardQueriesOperations.GetHand(source)[0].Id == slash.Id && _coreAdvancedWeaponsOperations.HasWeaponAbility(source, CardKind.FangtianHalberd);
        var usesProgramTargetCount = _coreRedAdditionalTargetsOperations.HasNextCardTargetAdjustment(source) || _coreProgramCardUseRulesOperations.UsesProgramCardTargetCount(source, slash, playedCardKind, targetSeats.Length) || _characterLuBuOlMagicOperations.IsMoGangquanMultiSlash(source, slash, playedCardKind, conversionSource, targetSeats);
        if (!stillLegal || (!usesFangtian && !usesProgramTargetCount))
        {
            throw new InvalidOperationException("Fangtian Halberd Slash became illegal before resolution.");
        }

        var ignoresArmor = _coreNativeEquipmentAndFactionQueriesOperations.HasArmorBypass(source);
        var resolutionId = _coreNativeCardUseOperations.BeginCardUse(slash, source.Seat, targetSeats, playedCardKind, ignoresArmor, conversionSource: conversionSource, additionalConversionSources: additionalConversionSources);
        var nuzhan = _coreCardConversionsOperations.GetNuzhanModifiers(resolutionId, source);
        _corePhysicalMovementFacadesOperations.MoveCard(slash, _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(source, slash), CardLocation.Processing, CardMoveReasons.Use, disclosure: CardMovementDisclosure.FaceUp);
        var countedTowardSlashLimit = !(_coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)?.UnlimitedUse == true) && !_coreProgramCardUsePaymentsOperations.CardUsePayments.ExemptsSlashDebit(ProgramCardUsePaymentStage.SlashCountExemption, resolutionId, source.Seat) && !nuzhan.IgnoresSlashLimit && !_coreCardConversionsOperations.IgnoresProgramSlashLimit(source, conversionSource) && _coreActualTurnProgressCurrentSeatPhaseAccess._phase == TurnPhase.Play && source.Seat == _coreActualTurnProgressCurrentSeatPhaseAccess._currentSeat;
        if (countedTowardSlashLimit)
        {
            _coreCardUseDebitsOperations.RecordSlashUseDebit(resolutionId, source.Seat);
        }

        _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(source.Seat, playedCardKind);
        var damageAmount = (1 + source.AlcoholDamageBonus) + nuzhan.DamageBonus;
        _coreAlcoholSlashSuppressionsOperations.CaptureProgramAlcoholConsumption(resolutionId, source);
        source.HasAlcoholEffect = false;
        var pending = _kernelCoreMainOperations.CreateFangtianHalberdHandle(resolutionId, source.Seat, slash, playedCardKind, ignoresArmor, damageAmount, targetSeats, usesFangtian, countedTowardSlashLimit, conversionSource);
        _coreNativeCardContinuationCursorsOperations.ActiveFangtianHalberd = pending;
        if (usesFangtian)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FangtianHalberdUsedEvent(resolutionId, source.Seat, slash.Id, playedCardKind, Array.AsReadOnly(targetSeats)));
        }

        var usesZhuqueFan = playedCardKind == CardKind.FireSlash && (additionalConversionSources is null || additionalConversionSources.Count == 0) && !_coreCardConversionsOperations.IsDirectProgramFireSlashConversion(source, slash, conversionSource) && (slash.Kind == CardKind.Slash || _coreActionQueriesOperations.GetProgramCardIdentityMatches(source, slash).Any(match => match.Identity.OutputKind == CardKind.Slash));
        if (usesZhuqueFan)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhuqueFanConvertedEvent(resolutionId, source.Seat, Array.AsReadOnly(new[] { slash.Id }), Array.AsReadOnly(targetSeats)));
        }

        if (usesProgramTargetCount)
        {
            var targetCountRule = _coreRuleQueriesOperations.EvaluateCardTargetCount(source, playedCardKind, resolutionId);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramCardTargetCountAppliedEvent(resolutionId, source.Seat, playedCardKind, Array.AsReadOnly(targetSeats), Array.AsReadOnly(targetCountRule.Value.Contributions.Select(contribution => contribution.SourceId).ToArray())));
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(slash.Id, playedCardKind, source.Seat, targetSeats[0], IgnoresArmor: ignoresArmor));
        var slashName = CardCatalog.Get(playedCardKind).DisplayName;
        _coreMatchLogOperations.AddLog(usesFangtian && !usesProgramTargetCount ? "EquipmentEffect" : "SkillTriggered", $"{(source.Name)} 以【{(slashName)}】指定 {(string.Join("、", targets.Select(target => target.Name)))}。", source.Seat, targetSeats[0]);
        foreach (var target in targets)
        {
            _coreNativeAiDecisionsOperations.NotifyAiOfSlash(source, target);
        }

        BeginNextFangtianHalberdTarget(pending);
    }

    internal void BeginNextFangtianHalberdTarget(global::CardGame.Core.NativeOwners.FangtianHalberdHandle pending)
    {
        if (!global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveFangtianHalberd, pending))
        {
            throw new InvalidOperationException("The Fangtian Halberd continuation is no longer current.");
        }

        while (pending.TargetIndex < pending.TargetSeats.Count && !_coreMatchParticipantsPlayersAccess._players[_coreCardActionsOperations.GetFangtianEffectiveTarget(pending, pending.TargetIndex)].IsAlive)
        {
            pending.TargetIndex++;
        }

        if (pending.TargetIndex >= pending.TargetSeats.Count)
        {
            throw new InvalidOperationException("A Fangtian Halberd use must finish through its final target attack.");
        }

        _coreNativeNullificationFramesOperations.SetCardUseTargetIndex(pending.ResolutionId, pending.TargetIndex);
        _coreNativeCardUseFramesOperations.SetCardUseStep(pending.ResolutionId, ResolutionFrameStep.Declared);
        var targetSeat = _coreCardActionsOperations.GetFangtianEffectiveTarget(pending, pending.TargetIndex);
        var targetIgnoresArmor = pending.IgnoresArmor || _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(_coreMatchParticipantsPlayersAccess._players[pending.SourceSeat], _coreMatchParticipantsPlayersAccess._players[targetSeat], pending.EffectiveCardKind);
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(pending.ResolutionId, pending.SourceSeat, targetSeat, pending.Card, pending.DamageAmount, pending.EffectiveCardKind, targetIgnoresArmor, conversionSource: pending.ConversionSource);
        if ((_coreCardUseLifecycleOperations.LifecycleCardUse(pending.ResolutionId)?.ProgramUseAccepted == true) && _coreCardActionsOperations.PreparedTargetAttacks(pending.ResolutionId)is { } prepared)
            _coreCardAttackStateOperations.UpdateCardAttackState(pending.ResolutionId, _ => prepared[pending.TargetIndex]);
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(pending.ResolutionId)?.AdjustedSlashReturn is { } adjustedReturn)
            _coreCardAttackStateOperations.UpdateCardAttackState(pending.ResolutionId, state => state!with { AdjustedSlashReturn = adjustedReturn, PhysicalCardIds = _coreCardUseLifecycleOperations.LifecycleCardUse(pending.ResolutionId)!.PhysicalCardIds! });
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(pending.ResolutionId)is { NativeMaterialSlashExtraTargets: not null, PhysicalCardIds: { } nativeMaterials })
            _coreCardAttackStateOperations.UpdateCardAttackState(pending.ResolutionId, state => state!with { PhysicalCardIds = nativeMaterials });
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(pending.ResolutionId)is { HpLossMaterialSlashReturn: not null, PhysicalCardIds: { } hpMaterials })
            _coreCardAttackStateOperations.UpdateCardAttackState(pending.ResolutionId, state => state!with { PhysicalCardIds = hpMaterials });
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(pending.ResolutionId)is { PhysicalCardIds: { } baitMaterials } baitUse && _characterGuanHaiOlOperations.IsXieweiMultiTargetUse(baitUse))
            _coreCardAttackStateOperations.UpdateCardAttackState(pending.ResolutionId, state => state!with { PhysicalCardIds = baitMaterials });
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(pending.ResolutionId)is { PhysicalCardIds: { } haoMengMaterials } haoMengUse && _characterHaoMengAnniversaryOperations.IsHaoMengNativeUse(haoMengUse))
            _coreCardAttackStateOperations.UpdateCardAttackState(pending.ResolutionId, state => state!with { PhysicalCardIds = haoMengMaterials });
        pending.CurrentAttack = attack;
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        _coreMatchLogOperations.AddLog("CardEffect", $"【{((pending.UsesFangtian ? "方天画戟" : "天义"))}】的【{(CardCatalog.Get(pending.EffectiveCardKind).DisplayName)}】开始结算 {(_coreMatchParticipantsPlayersAccess._players[targetSeat].Name)}。", pending.SourceSeat, targetSeat);
        var committedAction = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(frame => frame.Id == attack.ResolutionId).Action;
        if (_coreRecoveryReplacementOperations.TryPauseRecoveryPaidCardUse(attack.ResolutionId, new(RecoveryPaidCardUseKind.CommittedSlash, attack.SourceSeat)))
            return;
        if (committedAction is not null && _coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(attack.ResolutionId) && _coreCardActionsOperations.TryBeginProgramCardWindow(attack, committedAction, SkillProgramTriggerWindow.CardUseCommitted, committedAction.TargetSeats, ProgramCardContinuation.CommittedSlash))
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return;
        }

        BeginSlashTargetResolution(attack);
    }

    internal void ResolveZhangbaSlash(CharacterState source, CharacterState target, IReadOnlyList<Card> physicalCards)
    {
        if (!_coreNativeTargetLegalityOperations.CanUseZhangbaSerpentSpear(source) || physicalCards.Count != 2 || physicalCards.Select(card => card.Id).Distinct().Count() != 2 || physicalCards.Any(card => !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsOwnedPlayableLocation(source, _cardZonesOperations._cardZones.GetLocation(card.Id)) && !_characterGongsunZanMouMobileOperations.IsMouGszHuCard(source, card.Id) && !_characterLuSuMobileShiOperations.IsShiLsHandAccess2(source.Seat, card.Id)) || !_coreNativeTargetLegalityOperations.CanUseVirtualSlashTarget(source, target, physicalSuit: _coreAlternatingSuitAndCompletedTopOperations.PhysicalGroupSuit(source, physicalCards), effectiveColor: _coreTurnRedSlashBenefitsOperations.PhysicalGroupColor(source, physicalCards), effectiveRank: _coreFinalTargetSlashOperations.ZhangbaSpecificSlashRank(source, physicalCards), physicalCardIds: physicalCards.Select(c => c.Id).ToArray()))
        {
            throw new InvalidOperationException("Zhangba Serpent Spear became illegal before resolution.");
        }

        ResolveSlashCore(source, target, physicalCards[0], CardKind.Slash, source.Seat, physicalCards: physicalCards);
    }

    internal void ResolveSlashCore(CharacterState source, CharacterState target, Card slash, CardKind playedCardKind, int physicalOwnerSeat, global::CardGame.Core.NativeOwners.FactionCardRequestHandle? factionRequest = null, global::CardGame.Core.NativeOwners.BorrowedSwordHandle? borrowedSword = null, IReadOnlyList<Card>? physicalCards = null, bool countsTowardSlashLimit = true, bool usesZhuqueFan = false, CardConversionSource? conversionSource = null, IReadOnlyList<CardConversionSource>? additionalConversionSources = null, long? programSkillCardUseFrameId = null, CardLocation? physicalSourceLocation = null)
    {
        if (physicalSourceLocation is { } deckSource && !_coreForeignSelectedCardSlashOperations.IsSelectedForeignCardSlashSource(programSkillCardUseFrameId, source.Seat, target.Seat, slash, playedCardKind, physicalOwnerSeat, deckSource, physicalCards, countsTowardSlashLimit) && !_characterJiangWeiMobileGodOperations.IsMobileJwXinghunSlashSource(programSkillCardUseFrameId, source.Seat, target.Seat, slash, playedCardKind, physicalOwnerSeat, deckSource, physicalCards, countsTowardSlashLimit) && !_characterDuKuiAnniversaryOperations.IsDuKuiFanyinSlashSource(programSkillCardUseFrameId, source.Seat, target.Seat, slash, playedCardKind, physicalOwnerSeat, deckSource, physicalCards, countsTowardSlashLimit) && !_characterBaiLingYunAnniversaryOperations.IsAnniversaryBaiLingYunSlashSource(programSkillCardUseFrameId, source.Seat, target.Seat, slash, playedCardKind, physicalOwnerSeat, deckSource, physicalCards, countsTowardSlashLimit) && (deckSource != CardLocation.DrawPile || physicalCards is not null || programSkillCardUseFrameId is not { } deckParent || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { DeckSlashSequence: { } sequence } deckFrame || deckFrame.Id != deckParent || deckFrame.OwnerSeat != source.Seat || sequence.TargetSeat != target.Seat || sequence.ActiveCardId != slash.Id || sequence.DeclaredIds.LastOrDefault() != slash.Id || slash.Kind != playedCardKind || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(playedCardKind) || countsTowardSlashLimit || _cardZonesOperations._cardZones.GetLocation(slash.Id) != deckSource))
            throw new InvalidOperationException("An external Slash source requires its exact active deck sequence.");
        var slashCards = physicalCards?.ToArray() ?? [slash];
        if (slashCards.Length == 0 || slashCards[0].Id != slash.Id)
        {
            throw new InvalidOperationException("A Slash use must retain its primary physical card.");
        }

        if (usesZhuqueFan && (!_coreNativeTargetLegalityOperations.HasZhuqueFan(source) || playedCardKind != CardKind.FireSlash))
        {
            throw new InvalidOperationException("Zhuque Fan is no longer available for this Fire Slash conversion.");
        }

        target = _characterLuBuOlMagicOperations.MoGangquanFirstTarget(source, target, slash, playedCardKind, conversionSource);
        var ignoresArmor = _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(source, target, playedCardKind);
        var resolutionId = _coreNativeCardUseOperations.BeginCardUse(slash, source.Seat, [target.Seat], playedCardKind, ignoresArmor, slashCards.Select(card => card.Id).ToArray(), conversionSource, additionalConversionSources, isTrueZhangbaSlash: slashCards.Length == 2 && conversionSource is null && additionalConversionSources is null && _coreAdvancedWeaponsOperations.HasWeaponAbility(_coreMatchParticipantsPlayersAccess._players[physicalOwnerSeat], CardKind.ZhangbaSerpentSpear));
        _coreForeignSelectedCardSlashOperations.BindSelectedForeignCardSlashUse(resolutionId, programSkillCardUseFrameId);
        _characterJiangWeiMobileGodOperations.BindMobileJwXinghunSlashUse(resolutionId, programSkillCardUseFrameId);
        var nuzhan = _coreCardConversionsOperations.GetNuzhanModifiers(resolutionId, source);
        if (usesZhuqueFan)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhuqueFanConvertedEvent(resolutionId, source.Seat, Array.AsReadOnly(slashCards.Select(card => card.Id).ToArray()), Array.AsReadOnly(new[] { target.Seat })));
            _coreMatchLogOperations.AddLog("EquipmentEffect", $"{(source.Name)} 发动【朱雀羽扇】，将本次普通【杀】改为【火杀】使用。", source.Seat, target.Seat);
        }

        _coreCardUsePaymentsOperations.PaySlashUseMaterials(slashCards, physicalOwnerSeat, physicalSourceLocation);
        _coreProgramCardUsePaymentsOperations.CardUsePayments.CompleteSlashPayment(resolutionId);
        var countedTowardSlashLimit = countsTowardSlashLimit && !(_coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)?.UnlimitedUse == true) && !_coreProgramCardUsePaymentsOperations.CardUsePayments.ExemptsSlashDebit(ProgramCardUsePaymentStage.SlashCountExemption, resolutionId, source.Seat) && !nuzhan.IgnoresSlashLimit && !_coreCardConversionsOperations.IgnoresProgramSlashLimit(source, conversionSource) && _coreActualTurnProgressCurrentSeatPhaseAccess._phase == TurnPhase.Play && source.Seat == _coreActualTurnProgressCurrentSeatPhaseAccess._currentSeat;
        if (countedTowardSlashLimit)
        {
            _coreCardUseDebitsOperations.RecordSlashUseDebit(resolutionId, source.Seat);
        }

        _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(source.Seat, playedCardKind);
        var damageAmount = (1 + source.AlcoholDamageBonus) + nuzhan.DamageBonus;
        _coreAlcoholSlashSuppressionsOperations.CaptureProgramAlcoholConsumption(resolutionId, source);
        source.HasAlcoholEffect = false;
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(resolutionId, source.Seat, target.Seat, slash, damageAmount, playedCardKind, ignoresArmor, physicalCards: slashCards, conversionSource: conversionSource, programSkillCardUseFrameId: factionRequest?.ProgramSkillFrameId ?? programSkillCardUseFrameId);
        if (factionRequest is not null)
        {
            factionRequest.ActiveAttack = attack;
        }

        if (borrowedSword is not null)
        {
            borrowedSword.ActiveAttack = attack;
        }

        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        var slashName = CardCatalog.Get(playedCardKind).DisplayName;
        var useDescription = _coreCardDeclarationsOperations.PublicDeclarationDescription(slash, conversionSource is not null && slashCards.Length > 1 ? $"发动【{(_coreCardConversionsOperations.ProgramConversionName(conversionSource))}】，将 {(slashCards.Length)} 张手牌当作【杀】使用" : slashCards.Length == 2 ? "发动【丈八蛇矛】，将两张手牌当作【杀】使用" : playedCardKind == slash.Kind ? $"使用【{(slashName)}】" : $"将【{(CardCatalog.Get(slash.Kind).DisplayName)}】当作【{(slashName)}】使用");
        _coreMatchLogOperations.AddLog("CardUsed", $"{(source.Name)} 对 {(target.Name)}{(useDescription)}。", source.Seat, target.Seat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(slash.Id, playedCardKind, source.Seat, target.Seat, IgnoresArmor: ignoresArmor));
        if (conversionSource is not null && slashCards.Length > 1)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramViewAsConvertedEvent(resolutionId, conversionSource.SkillId, conversionSource.BindingId, source.Seat, Array.AsReadOnly(slashCards.Select(card => card.Id).ToArray()), playedCardKind, IsUse: true, [target.Seat]));
        }
        else if (slashCards.Length == 2)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhangbaSerpentSpearConvertedEvent(resolutionId, source.Seat, Array.AsReadOnly(slashCards.Select(card => card.Id).ToArray()), IsUse: true, target.Seat));
        }

        _coreNativeAiDecisionsOperations.NotifyAiOfSlash(source, target);
        if (_coreCardSupplyCompletionOperations.TryBeginProvidedCardSupplyCompletion(attack, factionRequest))
            return;
        var committedAction = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(frame => frame.Id == attack.ResolutionId).Action;
        if (_coreRecoveryReplacementOperations.TryPauseRecoveryPaidCardUse(attack.ResolutionId, new(RecoveryPaidCardUseKind.CommittedSlash, attack.SourceSeat)))
            return;
        if (committedAction is not null && _coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(attack.ResolutionId) && _coreCardActionsOperations.TryBeginProgramCardWindow(attack, committedAction, SkillProgramTriggerWindow.CardUseCommitted, committedAction.TargetSeats, ProgramCardContinuation.CommittedSlash))
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return;
        }

        BeginSlashTargetResolution(attack);
    }

    internal void BeginSlashTargetResolution(global::CardGame.Core.NativeOwners.CardAttackHandle attack)
    {
        if (_characterSimaYiMouOperations.TryBeginMouSmPrivateWindow(attack, MouSmPrivateCall.TargetSlash))
            return;
        if (_coreExtraCardEffectRoundsOperations.ContinueExtraCardEffectFangtian(attack))
            return;
        if (_coreRecoveryReplacementOperations.TryPauseRecoveryPaidCardUse(attack.ResolutionId, new(RecoveryPaidCardUseKind.SlashTarget, attack.SourceSeat)))
            return;
        _coreCurrentCardEnhancementsOperations.CaptureProgramAdjustedSlashBaseDamage(attack);
        if (_coreCardActionsOperations.TryBeginProgramSlashStage(attack, SkillProgramTriggerWindow.SlashTargetRedirecting, ProgramCardContinuation.SlashTargetRedirecting))
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return;
        }

        ContinueSlashAfterRedirectPrograms(attack);
    }

    internal void ContinueSlashAfterRedirectPrograms(global::CardGame.Core.NativeOwners.CardAttackHandle attack)
    {
        if (_coreCardActionsOperations.PrepareProgramSlashTargets(attack))
            return;
        if (_coreActorHandLimitTargetProgramsOperations.TryBeginAcceptedSlashActorHandLimitTargetPrograms(attack))
            return;
        if (_characterLiuChengMouMobileOperations.TryBeginMouLiuChengLateSlashTargets(attack))
            return;
        if (_characterYuanShaoMouMobileOperations.TryBeginMouYuanShaoLateSlashTarget(attack))
            return;
        ContinueSlashAfterFinalizedTargets(attack);
    }

    internal void ContinueSlashAfterFinalizedTargets(global::CardGame.Core.NativeOwners.CardAttackHandle attack)
    {
        _coreNextSlashProgramsOperations.FreezeNextSlashDamage(attack.ResolutionId);
        var currentUse = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(frame => frame.Id == attack.ResolutionId);
        if (currentUse.SourceSeat != attack.CardUserSeat)
            attack.ReplaceCardUser(currentUse.SourceSeat);
        if (_coreCurrentCardEnhancementsOperations.HasCurrentCardEnhancement(attack.ResolutionId, CurrentCardEnhancement.IgnoreArmor))
            attack.SetIgnoresArmor(true);
        _coreNativeSlashTargetDuelOperations.FreezeNativeSlashTargetDuelPlan(currentUse);
        if (_coreSlashTargetPenaltyWindowsOperations.TryBeginSlashTargetPenalties(attack))
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return;
        }

        if (_coreSlashTargetBenefitWindowsOperations.TryBeginSlashTargetBenefits(attack))
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return;
        }

        if (_coreActualUseTargetProgramsOperations.TryBeginActualUseTargetPrograms(attack, ActualUseTargetReturnKind.Slash))
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return;
        }

        if (_coreCardActionsOperations.TryBeginProgramCardUseBeforeTargetEffects(attack))
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return;
        }

        ContinueSlashAfterProgramTargetEffects(attack);
    }

    internal void ContinueSlashAfterProgramTargetEffects(global::CardGame.Core.NativeOwners.CardAttackHandle attack)
    {
        if (_coreDirectedCardPolicyQueriesOperations.HasDirectedCardArmorBypass(attack.ResolutionId, attack.TargetSeat))
            attack.SetIgnoresArmor(true);
        if (_coreShenwuEquipmentOperations.TryApplyShenwuQimen(attack))
            return;
        if (_coreProgramCardTargetEffectsOperations.IsCardEffectIneffective(attack.ResolutionId, attack.TargetSeat))
        {
            _coreNativeCardUseFramesOperations.SetCardUseStep(attack.ResolutionId, ResolutionFrameStep.ResolvingEffect);
            _coreNativeAttackCompletionOperations.CompleteAttack(attack);
            return;
        }

        if (_coreNativeSlashTargetDuelOperations.TryBeginNativeSlashTargetDuel(attack))
            return;
        if (_coreCardActionsOperations.TryBeginProgramSlashStage(attack, SkillProgramTriggerWindow.SlashBeforeResponse, ProgramCardContinuation.SlashBeforeResponse))
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return;
        }

        ContinueSlashAfterResponsePrograms(attack);
    }

    internal void ContinueSlashAfterResponsePrograms(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => ContinueSlashDefense(attack);
    internal void ContinueSlashDefense(global::CardGame.Core.NativeOwners.CardAttackHandle attack)
    {
        if (_coreNativeSlashTargetDuelOperations.TryBeginNativeSlashTargetDuel(attack))
            return;
        _characterSunQuanOlMagicOperations.ApplyMoQuanyuAttackPolicy(attack);
        _coreExtraCardEffectRoundsOperations.CaptureExtraCardEffectAttack(attack);
        if (!global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, attack))
        {
            throw new InvalidOperationException("The response-stage continuation does not own the current Slash.");
        }

        if (_coreAdvancedWeaponsOperations.HasWeaponAbility(_coreMatchParticipantsPlayersAccess._players[attack.CardUserSeat], CardKind.ScarletBloodSword))
            attack.ProhibitTargetHandResponses();
        if (_coreNativeWeaponResponsesOperations.TryBeginCixiongDoubleSwordsChoice(attack))
        {
            return;
        }

        var source = _coreMatchParticipantsPlayersAccess._players[attack.SourceSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat];
        var slash = attack.Card;
        var playedCardKind = attack.EffectiveCardKind ?? slash?.Kind ?? throw new InvalidOperationException("A Slash continuation must retain an effective card kind.");
        var ignoresArmor = attack.IgnoresArmor;
        var resolutionId = attack.ResolutionId;
        var slashName = CardCatalog.Get(playedCardKind).DisplayName;
        if (_coreComparedBlackSlashPoliciesOperations.ApplyComparedBlackSlashPolicies(attack))
        {
            _coreNativeCardUseFramesOperations.SetCardUseStep(resolutionId, ResolutionFrameStep.ResolvingEffect);
            _coreNativeAttackCompletionOperations.CompleteAttack(attack);
            return;
        }

        if (_coreHellActivitiesOperations.HasHellRemenImmunity(target, playedCardKind))
        {
            _coreNativeCardUseFramesOperations.SetCardUseStep(resolutionId, ResolutionFrameStep.ResolvingEffect);
            _coreNativeAttackCompletionOperations.CompleteAttack(attack);
            return;
        }

        if (attack.SuccessfulDodgeResponses == 0)
        {
            attack.SetRequiredDodgeResponses(_characterChengJiChengCuiOlOperations.TousuiRequiredDodge(resolutionId, _coreShortRangeSlashTargetsOperations.FreezeShortRangeSlashResponseRequirement(resolutionId, source.Seat, target.Seat, playedCardKind, _coreNativeEquipmentAndFactionQueriesOperations.GetRequiredResponseCount(source, source.Seat, target.Seat, playedCardKind, CardKind.Dodge))));
        }

        if (!ignoresArmor && !attack.IsTwoCardVirtualSlash && !_characterZhangFeiAnniversaryGodOperations.ShenZfAttackIsColorless(attack) && slash?.Suit is Suit.Spade or Suit.Club && _coreNativeEquipmentAndFactionQueriesOperations.HasBlackSlashBarrier(target))
        {
            _coreMatchLogOperations.AddLog("ArmorEffect", $"{(target.Name)} 的【仁王盾】令这次黑色【{(slashName)}】无效。", target.Seat, source.Seat);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ArmorEffectAppliedEvent(resolutionId, CardKind.RenwangShield, source.Seat, target.Seat, playedCardKind));
            _coreNativeCardUseFramesOperations.SetCardUseStep(resolutionId, ResolutionFrameStep.ResolvingEffect);
            _coreNativeAttackCompletionOperations.CompleteAttack(attack);
            return;
        }

        if (_coreProgramCardPoliciesOperations.HasCardPolicy(target, SkillProgramCardPolicyKind.NullifyBlackSlashWithoutArmor, playedCardKind) && !_characterZhangFeiAnniversaryGodOperations.ShenZfAttackIsColorless(attack) && global::CardGame.Core.EngineOwners.CoreNativeDamagePoliciesRuntime.CanYizhongNullify(attack.PhysicalCards, _coreNativeEquipmentAndFactionQueriesOperations.HasArmor(target)))
        {
            _coreMatchLogOperations.AddLog("SkillTriggered", $"{(target.Name)} 的【毅重】令这次黑色【{(slashName)}】无效。", target.Seat, source.Seat);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YizhongNullifiedEvent(resolutionId, source.Seat, target.Seat, playedCardKind));
            _coreNativeCardUseFramesOperations.SetCardUseStep(resolutionId, ResolutionFrameStep.ResolvingEffect);
            _coreNativeAttackCompletionOperations.CompleteAttack(attack);
            return;
        }

        if (_coreNativeDamageHandlesOperations.UsesFormalTengjia && !ignoresArmor && playedCardKind == CardKind.Slash && _coreNativeEquipmentAndFactionQueriesOperations.HasTengjia(target))
        {
            _coreMatchLogOperations.AddLog("ArmorEffect", $"{(target.Name)} 的【藤甲】令这次普通【杀】无效。", target.Seat, source.Seat);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ArmorEffectAppliedEvent(resolutionId, CardKind.Tengjia, source.Seat, target.Seat, playedCardKind));
            _coreNativeCardUseFramesOperations.SetCardUseStep(resolutionId, ResolutionFrameStep.ResolvingEffect);
            _coreNativeAttackCompletionOperations.CompleteAttack(attack);
            return;
        }

        if (_resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(frame => frame.Id == resolutionId).TurnRedSlashSkipsDodge)
        {
            _coreMatchLogOperations.AddLog("SkillTriggered", $"{(source.Name)} 使用的红色【{(slashName)}】不能被抵消，{(target.Name)} 跳过【闪】响应。", source.Seat, target.Seat);
            _coreNativeCardUseFramesOperations.SetCardUseStep(resolutionId, ResolutionFrameStep.ResolvingEffect);
            if (!_coreNativeDamageApplicationOperations.ApplyAttackDamage(attack))
                _coreNativeAttackCompletionOperations.CompleteAttack(attack);
            return;
        }

        if (_characterLuMengMouOperations.IsYingboUnrespondable(resolutionId) || _coreIssuedPlayUsePoliciesOperations.IsIssuedCardUnrespondable(resolutionId) || _characterLiuQiOlOperations.IsWenjiResponseForbidden2(resolutionId, target.Seat))
        {
            _coreNativeCardUseFramesOperations.SetCardUseStep(resolutionId, ResolutionFrameStep.ResolvingEffect);
            if (!_coreNativeDamageApplicationOperations.ApplyAttackDamage(attack))
            {
                _coreNativeAttackCompletionOperations.CompleteAttack(attack);
            }

            return;
        }

        var nearbyResponseProhibited = _coreProgramCardPoliciesOperations.IsNearbyTargetResponseProhibited(attack.CardUserSeat, target.Seat, playedCardKind, [target.Seat]);
        var suitResponseProhibited = _coreProgramCardPoliciesOperations.IsSuitSlashResponseProhibited(attack);
        var convertedColorlessProhibited = _coreConvertedSlashResponseOperations.HasConvertedSlashSameColorResponseRestriction(attack) && _coreConvertedSlashResponseOperations.ConvertedSlashSameColorResponseColor(attack)is null;
        var ghostBladeProhibited = _coreAdvancedWeaponsOperations.HasWeaponAbility(_coreMatchParticipantsPlayersAccess._players[attack.CardUserSeat], CardKind.GhostDragonCrescentBlade) && attack.PhysicalCards.Count > 0 && attack.PhysicalCards.All(card => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsRedSuit(_coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[attack.CardUserSeat], card)));
        if (attack.ProhibitsDodge || _coreWushuangCombatTacticsOperations.WushuangDrunkenSlashUnrespondable(attack) || nearbyResponseProhibited || suitResponseProhibited || ghostBladeProhibited || convertedColorlessProhibited)
        {
            var prohibitingSkills = attack.ResponseProhibitingSkillNames.Concat(nearbyResponseProhibited ? ["伏骑"] : []).Concat(suitResponseProhibited ? ["武神"] : []).Concat(convertedColorlessProhibited ? ["父魂"] : []).Concat(ghostBladeProhibited ? ["鬼龙斩月刀"] : []).Distinct(StringComparer.Ordinal).Select(name => $"【{(name)}】");
            _coreMatchLogOperations.AddLog("SkillTriggered", $"{(source.Name)} 的{(string.Join("、", prohibitingSkills))}令 {(target.Name)} 不能使用【闪】响应此【{(slashName)}】。", source.Seat, target.Seat);
            _coreNativeCardUseFramesOperations.SetCardUseStep(resolutionId, ResolutionFrameStep.ResolvingEffect);
            if (!_coreNativeDamageApplicationOperations.ApplyAttackDamage(attack))
            {
                _coreNativeAttackCompletionOperations.CompleteAttack(attack);
            }

            return;
        }

        _characterSimaYiMobileGodOperations.MarkMobileSimaRenjieResponse(attack, target.Seat, CardKind.Dodge);
        var dodges = _coreNativeResponseMaterialQueriesOperations.GetResponseCards(target, CardKind.Dodge);
        if (_coreConvertedSlashResponseOperations.ConvertedSlashSameColorResponseColor(attack)is { } requireRed)
            dodges = dodges.Where(card => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsRedSuit(_coreNativeCardAppearanceOperations.EffectiveSuit(target, card)) == requireRed).ToList();
        var hasBagua = !ignoresArmor && _coreNativeEquipmentAndFactionQueriesOperations.HasBagua(target) && !_coreIssuedPlayUsePoliciesOperations.HasIssuedPlayPhaseUseBan(target.Seat);
        var dodge = dodges.FirstOrDefault();
        var canRequestFactionDefense = _coreNativeEquipmentAndFactionQueriesOperations.CanRequestFactionDefense(target, attack);
        var responseOrdinal = attack.SuccessfulDodgeResponses + 1;
        var responsePrompt = attack.RequiredDodgeResponses > 1 ? $"{(source.Name)} 的【无双】要求你打出第 {(responseOrdinal)} 张【闪】，是否响应？" : $"{(source.Name)} 对你使用了【{(slashName)}】，是否打出【闪】？";
        if ((dodges.Count > 0 || hasBagua || canRequestFactionDefense || _coreProgramDodgeResponsesOperations.ProgramDodgeResponseChoices(target).Any() || _coreChainedStateBasicConversionsOperations.HasChainedStateBasicDodge(target) || _coreDrawFundedDistinctBasicOffersOperations.HasDrawFundedDistinctBasicDodge(target) || _coreRequestedDeckBasicsOperations.HasRequestedDeckBasicSource(target, CardKind.Dodge) || (_characterGuoTuOlOperations.HasWeijieRequiredBasic(target, CardKind.Dodge) || (_characterLiuZhangOlOperations.HasZonghuRequiredBasic(target, CardKind.Dodge) || _characterHuanShuJunOlOperations.HasHuanpeiRequiredBasic(target, CardKind.Dodge)))) && (target.IsHuman || (_characterGuoTuOlOperations.HasWeijieRequiredBasic(target, CardKind.Dodge) || (_characterLiuZhangOlOperations.HasZonghuRequiredBasic(target, CardKind.Dodge) || _characterHuanShuJunOlOperations.HasHuanpeiRequiredBasic(target, CardKind.Dodge)))))
        {
            _coreNativeResponseWindowsOperations.PushResponseWindow(resolutionId, source.Seat, target.Seat, playedCardKind, CardKind.Dodge);
            _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.RespondDodge, target.Seat, responsePrompt, dodges.Select(card => card.Id).ToArray(), [], source.Seat, playedCardKind)
            {
                PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
                Choices = _coreNativePromptBoundaryOperations.CreateResponseChoices(dodges, CardKind.Dodge, playedCardKind, card => _coreNativeResponseMaterialQueriesOperations.GetEffectiveResponseKind(target, card, CardKind.Dodge), includeBagua: hasBagua, includeFactionDefense: canRequestFactionDefense, responder: target),
                RequiredCardKind = CardKind.Dodge
            };
            _coreMatchOutcomeStatusAccess._status = EngineStatus.AwaitingHumanResponse;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ResponseRequestedEvent(source.Seat, target.Seat, playedCardKind, CardKind.Dodge));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return;
        }

        if (dodge is not null || hasBagua || canRequestFactionDefense || _coreProgramDodgeResponsesOperations.ProgramDodgeResponseChoices(target).Any() || _coreChainedStateBasicConversionsOperations.HasChainedStateBasicDodge(target) || _coreDrawFundedDistinctBasicOffersOperations.HasDrawFundedDistinctBasicDodge(target) || _coreRequestedDeckBasicsOperations.HasRequestedDeckBasicSource(target, CardKind.Dodge) || (_characterGuoTuOlOperations.HasWeijieRequiredBasic(target, CardKind.Dodge) || (_characterLiuZhangOlOperations.HasZonghuRequiredBasic(target, CardKind.Dodge) || _characterHuanShuJunOlOperations.HasHuanpeiRequiredBasic(target, CardKind.Dodge))))
        {
            // Choosing to respond is a separate continuation. Advance() consumes it
            // immediately, while AdvanceOneStep() exposes it as the next AI decision.
            _coreNativeResponseWindowsOperations.PushResponseWindow(resolutionId, source.Seat, target.Seat, playedCardKind, CardKind.Dodge);
            _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.RespondDodge, target.Seat, attack.RequiredDodgeResponses > 1 ? $"{(source.Name)} 的【无双】要求 {(target.Name)} 打出第 {(responseOrdinal)} 张【闪】，AI 将选择是否响应。" : $"{(source.Name)} 对 {(target.Name)} 使用了【{(slashName)}】，AI 将选择是否打出【闪】。", dodges.Select(card => card.Id).ToArray(), [], source.Seat, playedCardKind)
            {
                PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
                Choices = _coreNativePromptBoundaryOperations.CreateResponseChoices(dodges, CardKind.Dodge, playedCardKind, card => _coreNativeResponseMaterialQueriesOperations.GetEffectiveResponseKind(target, card, CardKind.Dodge), includeBagua: hasBagua, includeFactionDefense: canRequestFactionDefense, responder: target),
                RequiredCardKind = CardKind.Dodge
            };
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ResponseRequestedEvent(source.Seat, target.Seat, playedCardKind, CardKind.Dodge));
            return;
        }

        _coreNativeCardUseFramesOperations.SetCardUseStep(resolutionId, ResolutionFrameStep.ResolvingEffect);
        if (!_coreNativeDamageApplicationOperations.ApplyAttackDamage(attack))
        {
            _coreNativeAttackCompletionOperations.CompleteAttack(attack);
        }
    }
}
