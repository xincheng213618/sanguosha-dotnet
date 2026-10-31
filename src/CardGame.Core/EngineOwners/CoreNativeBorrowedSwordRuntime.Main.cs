#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeBorrowedSwordRuntime
{
    internal Card? GetWeapon(CharacterState player) => _characterHuCheErMouOperations.KongwuEffectiveEquipment(player).FirstOrDefault(card => _coreFieldEquipmentOperations.FieldEquipmentSlot(card) == EquipmentSlot.Weapon);
    internal bool IsLegalBorrowedSwordSlashTarget(CharacterState weaponOwner, CharacterState target, CardKind? effectiveKind = null, Suit? physicalSuit = null, bool allowAnyPhysicalSuit = true, bool? effectiveColor = null, int? effectiveRank = null, IReadOnlyList<int>? physicalCardIds = null, CardConversionSource? conversionSource = null) => effectiveKind is { } kind ? target.IsAlive && target.Seat != weaponOwner.Seat && _coreProgramTargetRulesOperations.TargetRules.AllowsSlash(new(weaponOwner, target, kind, conversionSource, physicalCardIds)) && !_coreCardUseModulesOperations.IsCardUseForbidden(weaponOwner.Seat, kind, CardActionType.Use) && (_coreProgramDistanceRulesOperations.DistanceRules.AllowsSlashDistance(ProgramDistanceStage.BorrowedSlash, new(weaponOwner, target, kind, Conversion: conversionSource, PhysicalCardIds: physicalCardIds, PhysicalSuit: physicalSuit, EffectiveColor: effectiveColor, EffectiveRank: effectiveRank, AllowAnyPhysicalSuit: allowAnyPhysicalSuit))) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(weaponOwner.Seat, target.Seat, kind) && !_coreNativeDamagePoliciesOperations.IsSlashProhibited(target) : global::CardGame.Core.EngineOwners.CoreGameEngineDirectedCardPolicyQueriesDefinitions.SlashKinds.Any(candidate => IsLegalBorrowedSwordSlashTarget(weaponOwner, target, candidate, physicalSuit, allowAnyPhysicalSuit, effectiveColor, effectiveRank, physicalCardIds, conversionSource));
    internal IReadOnlyList<Card> GetBorrowedSwordSlashCards(CharacterState weaponOwner, CharacterState slashTarget)
    {
        if (!IsLegalBorrowedSwordSlashTarget(weaponOwner, slashTarget))
        {
            return[];
        }

        return _coreActionQueriesOperations.GetSlashUseCards(weaponOwner).Where(card => !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(weaponOwner.Seat, slashTarget.Seat, _coreNativeCardAppearanceOperations.EffectiveSuit(weaponOwner, card))).Where(card =>
        {
            var baseKind = _coreOwnPublicPileConversionsOperations.PhysicalSlashUseBaseKind(weaponOwner, card);
            if (!_coreProgramCardUseRulesOperations.GetSlashUseVariants2(weaponOwner, baseKind).Concat(_characterMiHengLeOperations.SiruiPhysicalSlashVariants(weaponOwner, card).Concat(_characterGuanYuWumiaoOperations.JuewuPhysicalSlashVariants(weaponOwner, card)).Concat(_characterCaoZhiLeOperations.FulePhysicalSlashVariants(weaponOwner, card).Concat(_coreProgramCardLegalityOperations.CardLegality.NativeSlashVariants(new(weaponOwner, card))))).DistinctBy(v => v.EffectiveKind).Any(variant => (_coreProgramDistanceRulesOperations.DistanceRules.AllowsSlashDistance(ProgramDistanceStage.BorrowedPhysicalSlash, new(weaponOwner, slashTarget, variant.EffectiveKind, card))) && IsLegalBorrowedSwordSlashTarget(weaponOwner, slashTarget, variant.EffectiveKind, _coreNativeCardAppearanceOperations.EffectiveSuit(weaponOwner, card), false, global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(weaponOwner, card)), _coreFinalTargetSlashOperations.SpecificSlashRank(weaponOwner, card, variant.EffectiveKind), [card.Id], _coreActionQueriesOperations.GetProgramViewAsConversions(weaponOwner, card, variant.EffectiveKind, false).FirstOrDefault(_characterMiHengLeOperations.IsSiruiSource))))
                return false;
            var location = _cardZonesOperations._cardZones.GetLocation(card.Id);
            if (location != CardLocation.Equipment(weaponOwner.Seat) || _coreFieldEquipmentOperations.FieldEquipmentSlot(card) != EquipmentSlot.Weapon)
            {
                return true;
            }

            // When Wusheng spends the equipped weapon itself, that weapon
            // cannot also provide the range needed for the forced Slash.
            return _coreProgramDistanceRulesOperations.DistanceRules.AllowsSlashDistance(ProgramDistanceStage.BorrowedPaidWeaponSlash, new(weaponOwner, slashTarget, CardKind.Slash, card));
        }).ToArray();
    }

    internal bool CanRequestBorrowedSwordFactionSlash(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending)
    {
        var owner = _coreMatchParticipantsPlayersAccess._players[pending.WeaponOwnerSeat];
        return !pending.FactionSlashAttempted && owner.IsAlive && _coreProgramFactionPoliciesOperations.GetFactionResponsePolicy(owner, CardKind.Slash)is not null && IsLegalBorrowedSwordSlashTarget(owner, _coreMatchParticipantsPlayersAccess._players[pending.SlashTargetSeat]) && _coreProgramFactionPoliciesOperations.GetFactionSlashCandidateSeats(owner.Seat).Count > 0;
    }

    internal void BeginBorrowedSwordSlashChoice(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending, bool includeFactionSlash, bool keepCurrentRequest = false)
    {
        if (!global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword, pending) || pending.ActiveAttack is not null)
        {
            throw new InvalidOperationException("Borrowed Sword is not awaiting its forced Slash choice.");
        }

        var source = _coreMatchParticipantsPlayersAccess._players[pending.SourceSeat];
        var weaponOwner = _coreMatchParticipantsPlayersAccess._players[pending.WeaponOwnerSeat];
        var slashTarget = _coreMatchParticipantsPlayersAccess._players[pending.SlashTargetSeat];
        var weapon = GetWeapon(weaponOwner);
        if (!weaponOwner.IsAlive || weapon is null)
        {
            CompleteBorrowedSwordWithoutSlash(pending, transferWeapon: false);
            return;
        }

        if (_coreIssuedPlayUsePoliciesOperations.IsIssuedCardUnrespondable(pending.ResolutionId) || _characterZhangLiaoHappyGodOperations.HappyZlCannotRespond2(pending.ResolutionId, pending.WeaponOwnerSeat) || !IsLegalBorrowedSwordSlashTarget(weaponOwner, slashTarget))
        {
            CompleteBorrowedSwordWithoutSlash(pending, transferWeapon: true);
            return;
        }

        var slashes = GetBorrowedSwordSlashCards(weaponOwner, slashTarget);
        var zhangbaPairs = _coreNativeResponseMaterialQueriesOperations.GetZhangbaSlashPairs(weaponOwner).Where(cards => _coreProgramCardPoliciesOperations.HasCardPolicy(weaponOwner, SkillProgramCardPolicyKind.RoundGainedOtherSourceUse) ? IsLegalBorrowedSwordSlashTarget(weaponOwner, slashTarget, CardKind.Slash, _coreAlternatingSuitAndCompletedTopOperations.PhysicalGroupSuit(weaponOwner, cards), false, _coreTurnRedSlashBenefitsOperations.PhysicalGroupColor(weaponOwner, cards), _coreFinalTargetSlashOperations.ZhangbaSpecificSlashRank(weaponOwner, cards), cards.Select(c => c.Id).ToArray()) : !_coreFinalTargetSlashOperations.HasRankSlashRange(weaponOwner, CardKind.Slash) || IsLegalBorrowedSwordSlashTarget(weaponOwner, slashTarget, CardKind.Slash, _coreAlternatingSuitAndCompletedTopOperations.PhysicalGroupSuit(weaponOwner, cards), false, _coreTurnRedSlashBenefitsOperations.PhysicalGroupColor(weaponOwner, cards), _coreFinalTargetSlashOperations.ZhangbaSpecificSlashRank(weaponOwner, cards))).Where(cards => !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(weaponOwner.Seat, slashTarget.Seat, cards.Select(card => _coreNativeCardAppearanceOperations.EffectiveSuit(weaponOwner, card)).Distinct().ToArray()is [var suit] ? suit : null)).ToArray();
        var programMultiCardUses = _coreCardConversionsOperations.GetProgramMultiCardViewAsSelections(weaponOwner, CardKind.Slash, forResponse: false).Where(selection => !_coreProgramCardPoliciesOperations.HasCardPolicy(weaponOwner, SkillProgramCardPolicyKind.RoundGainedOtherSourceUse) || IsLegalBorrowedSwordSlashTarget(weaponOwner, slashTarget, selection.OutputKind, _coreGuanHaiBridgesOperations.ProgramMultiCardSuit(weaponOwner, selection), false, _coreGuanHaiBridgesOperations.ProgramMultiCardColor(weaponOwner, selection), physicalCardIds: selection.Cards.Select(c => c.Id).ToArray())).Where(selection => !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(weaponOwner.Seat, slashTarget.Seat, selection.Cards.Select(card => _coreNativeCardAppearanceOperations.EffectiveSuit(weaponOwner, card)).Distinct().ToArray()is [var suit] ? suit : null)).ToArray();
        var canRequestFactionSlash = includeFactionSlash && CanRequestBorrowedSwordFactionSlash(pending);
        if (!keepCurrentRequest && slashes.Count == 0 && zhangbaPairs.Length == 0 && programMultiCardUses.Length == 0 && !canRequestFactionSlash && _coreTieredRoundZeroUsesOperations.TieredRoundZeroForcedSlashChoices(weaponOwner, slashTarget, pending.ResolutionId, false).Count == 0 && _characterXiaoQiaoMouOlOperations.MouXqForcedSlashChoices(weaponOwner, slashTarget, pending.ResolutionId, false).Count == 0 && _characterMaXiuMaTieOlOperations.KenshangBorrowedSlashChoices(pending).Count == 0 && _characterChengJiChengCuiOlOperations.TousuiBorrowedSlashChoices(pending).Count == 0 && _coreRoundOnceCardlessNativeUsesOperations.RoundOnceCardlessForcedSlashChoices(weaponOwner, slashTarget, pending.ResolutionId, false).Count == 0 && !_coreChainedStateBasicConversionsOperations.ChainedStateBasicForcedSlashRules(weaponOwner, slashTarget, false).Any() && !_coreDrawFundedDistinctBasicOffersOperations.DrawFundedDistinctBasicForcedSlashRules(weaponOwner, slashTarget, false).Any() && !_coreRequestedDeckBasicsOperations.HasRequestedDeckBasicSource(weaponOwner, CardKind.Slash) && !_characterLiuFengBoundaryOperations.HasBoundaryLiuFengForcedSlash(weaponOwner, slashTarget, pending) && !(_characterGuoTuOlOperations.HasWeijieForcedSlash(weaponOwner, slashTarget, pending) || (_characterLiuZhangOlOperations.HasZonghuForcedSlash(weaponOwner, slashTarget, pending) || _characterHuanShuJunOlOperations.HasHuanpeiForcedSlash(weaponOwner, slashTarget, pending))))
        {
            CompleteBorrowedSwordWithoutSlash(pending, transferWeapon: true);
            return;
        }

        _coreNativeResponseWindowsOperations.PushResponseWindow(pending.ResolutionId, source.Seat, weaponOwner.Seat, CardKind.BorrowedSword, CardKind.Slash);
        pending.AwaitingSlashChoice = true;
        var choices = slashes.SelectMany(card =>
        {
            var baseEffectiveKind = _coreOwnPublicPileConversionsOperations.PhysicalSlashUseBaseKind(weaponOwner, card);
            return _coreNativeTargetLegalityOperations.GetSlashUseKinds(weaponOwner, baseEffectiveKind).Concat(_characterMiHengLeOperations.SiruiPhysicalSlashVariants(weaponOwner, card).Concat(_characterGuanYuWumiaoOperations.JuewuPhysicalSlashVariants(weaponOwner, card)).Concat(_characterCaoZhiLeOperations.FulePhysicalSlashVariants(weaponOwner, card).Concat(_coreProgramCardLegalityOperations.CardLegality.NativeSlashVariants(new(weaponOwner, card)))).Select(v => v.EffectiveKind)).Distinct().Where(effectiveKind => effectiveKind == baseEffectiveKind || global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsNativeResponseCard(card, CardKind.Slash) || _coreOwnPublicPileConversionsOperations.IsOwnPublicPileSlashMaterial(weaponOwner, card) || (_characterMiHengLeOperations.IsSiruiPhysicalSlashKind(weaponOwner, card, effectiveKind) || _characterCaoZhiLeOperations.IsFulePhysicalSlashKind(weaponOwner, card, effectiveKind))).SelectMany(effectiveKind =>
            {
                var usesZhuqueFan = effectiveKind != baseEffectiveKind && !(_characterMiHengLeOperations.IsSiruiPhysicalSlashKind(weaponOwner, card, effectiveKind) || _characterCaoZhiLeOperations.IsFulePhysicalSlashKind(weaponOwner, card, effectiveKind));
                var cardDescription = usesZhuqueFan ? $"发动【朱雀羽扇】，将【杀】改为【火杀】使用" : global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsNativeResponseCard(card, CardKind.Slash) ? $"使用【{(CardCatalog.Get(effectiveKind).DisplayName)}】" : $"将【{(card.DisplayName)}】当作【杀】使用";
                var suffix = usesZhuqueFan ? ".as-FireSlash" : string.Empty;
                return _coreCardConversionsOperations.CreateConversionChoiceVariants(weaponOwner, card, usesZhuqueFan ? baseEffectiveKind : effectiveKind, forResponse: false, $"borrowed-sword.slash.card-{(card.Id)}{(suffix)}", $"{(cardDescription)}攻击 {(slashTarget.Name)}。", [card.Id], [slashTarget.Seat], new Dictionary<string, string> { ["response"] = "borrowed-sword-slash", ["response-card-kind"] = effectiveKind.ToString(), ["target-seat"] = slashTarget.Seat.ToString(System.Globalization.CultureInfo.InvariantCulture) }).Where(choice => _characterMiHengLeOperations.SiruiBorrowedChoiceAllowed(weaponOwner, slashTarget, card, choice));
            });
        }).ToList();
        foreach (var pair in zhangbaPairs)
        {
            choices.Add(new PromptChoice(new ChoiceId($"borrowed-sword.zhangba-slash.cards-{(pair[0].Id)}-{(pair[1].Id)}"), $"发动【丈八蛇矛】，将两张手牌当【杀】攻击 {(slashTarget.Name)}。", [pair[0].Id, pair[1].Id], [slashTarget.Seat], new Dictionary<string, string> { ["response"] = "zhangba-slash", ["response-card-kind"] = CardKind.Slash.ToString(), ["equipment"] = CardKind.ZhangbaSerpentSpear.ToString(), ["target-seat"] = slashTarget.Seat.ToString(System.Globalization.CultureInfo.InvariantCulture) }));
        }

        foreach (var selection in programMultiCardUses)
        {
            var cardIds = selection.Cards.Select(card => card.Id).ToArray();
            var parameters = new Dictionary<string, string>
            {
                ["response"] = "program-view-as-slash",
                ["response-card-kind"] = selection.OutputKind.ToString(),
                ["target-seat"] = slashTarget.Seat.ToString(System.Globalization.CultureInfo.InvariantCulture)
            };
            global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime.AddConversionParameters(parameters, selection.Source);
            choices.Add(new PromptChoice(new ChoiceId($"borrowed-sword.program-view-as-slash.{(selection.Source.SkillId.Length)}:" + $"{(selection.Source.SkillId)}.{(selection.Source.BindingId)}.cards-{(string.Join('-', cardIds))}"), $"发动【{(_coreCardConversionsOperations.ProgramConversionName(selection.Source))}】，将 {(cardIds.Length)} 张手牌当【{(CardCatalog.Get(selection.OutputKind).DisplayName)}】攻击 {(slashTarget.Name)}。", cardIds, [slashTarget.Seat], parameters));
        }

        if (canRequestFactionSlash)
        {
            choices.Add(new PromptChoice(new ChoiceId("borrowed-sword.jijiang"), $"发动主公技【激将】，请求其他蜀势力角色提供【杀】攻击 {(slashTarget.Name)}。", [], [slashTarget.Seat], new Dictionary<string, string> { ["response"] = "faction-slash-request", ["skill"] = _coreProgramFactionPoliciesOperations.GetFactionResponsePolicy(weaponOwner, CardKind.Slash)!.SkillId }));
        }

        choices.AddRange(_coreChainedStateBasicConversionsOperations.ChainedStateBasicForcedSlashChoices(weaponOwner, slashTarget, pending.ResolutionId, false));
        choices.AddRange(_coreRoundOnceCardlessNativeUsesOperations.RoundOnceCardlessForcedSlashChoices(weaponOwner, slashTarget, pending.ResolutionId, false));
        choices.AddRange(_characterMaXiuMaTieOlOperations.KenshangBorrowedSlashChoices(pending));
        choices.AddRange(_characterChengJiChengCuiOlOperations.TousuiBorrowedSlashChoices(pending));
        choices.AddRange(_coreTieredRoundZeroUsesOperations.TieredRoundZeroForcedSlashChoices(weaponOwner, slashTarget, pending.ResolutionId, false));
        choices.Add(new PromptChoice(new ChoiceId("borrowed-sword.give-weapon"), $"不使用【杀】，将【{(weapon.DisplayName)}】交给 {(source.Name)}。", [], [source.Seat], new Dictionary<string, string> { ["response"] = "borrowed-sword-give-weapon", ["weapon-card-id"] = weapon.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) }));
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.RespondSlash, weaponOwner.Seat, $"{(source.Name)} 对你使用【借刀杀人】：请对 {(slashTarget.Name)} 使用【杀】，否则交出武器。", slashes.Select(card => card.Id).Concat(zhangbaPairs.SelectMany(pair => pair.Select(card => card.Id))).Concat(programMultiCardUses.SelectMany(item => item.Cards.Select(card => card.Id))).Distinct().ToArray(), [slashTarget.Seat], source.Seat, CardKind.BorrowedSword)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            TargetSeat = slashTarget.Seat,
            RequiredCardKind = CardKind.Slash,
            Choices = choices
        };
        _coreMatchOutcomeStatusWinnerAccess._status = weaponOwner.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ResponseRequestedEvent(source.Seat, weaponOwner.Seat, CardKind.BorrowedSword, CardKind.Slash));
    }

    internal void ResolveBorrowedSwordSlashChoice(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending, Card slash, CardKind? requestedEffectiveKind = null)
    {
        var declaredOwner = _coreMatchParticipantsPlayersAccess._players[pending.WeaponOwnerSeat];
        if (_coreCardDeclarationsOperations.TryBeginCardDeclaration(declaredOwner, slash, requestedEffectiveKind ?? CardKind.Slash, _coreCardDeclarationsOperations.PeekDeclarationConversion(declaredOwner, slash, requestedEffectiveKind ?? CardKind.Slash, true), _coreCardDeclarationsOperations.DeclarationReturn(CardDeclarationPurpose.BorrowedSword, declaredOwner.Seat, finalKind: requestedEffectiveKind ?? CardKind.Slash), [pending.SlashTargetSeat]))
            return;
        var weaponOwner = _coreMatchParticipantsPlayersAccess._players[pending.WeaponOwnerSeat];
        var slashTarget = _coreMatchParticipantsPlayersAccess._players[pending.SlashTargetSeat];
        var selected = GetBorrowedSwordSlashCards(weaponOwner, slashTarget).SingleOrDefault(card => card.Id == slash.Id) ?? throw new InvalidOperationException("The selected Borrowed Sword Slash is no longer legal.");
        var baseEffectiveKind = _coreProgramCardLegalityOperations.CardLegality.SelectNativeSlashKind(new(weaponOwner, selected, _characterCaoZhiLeOperations.FuleSelectedSlashKind(weaponOwner, selected, _characterGuanYuWumiaoOperations.JuewuSelectedSlashKind(weaponOwner, selected, _characterMiHengLeOperations.SiruiSelectedSlashKind(weaponOwner, selected, _coreOwnPublicPileConversionsOperations.PhysicalSlashUseBaseKind(weaponOwner, selected)))), null));
        var effectiveKind = requestedEffectiveKind ?? baseEffectiveKind;
        var usesZhuqueFan = _coreNativeTargetLegalityOperations.IsZhuqueFanConversion(weaponOwner, baseEffectiveKind, effectiveKind);
        if (effectiveKind != baseEffectiveKind && !usesZhuqueFan)
        {
            throw new InvalidOperationException("The selected Borrowed Sword Slash conversion is no longer legal.");
        }

        if (!IsLegalBorrowedSwordSlashTarget(weaponOwner, slashTarget, effectiveKind, _coreNativeCardAppearanceOperations.EffectiveSuit(weaponOwner, selected), false, global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(weaponOwner, selected)), _coreFinalTargetSlashOperations.SpecificSlashRank(weaponOwner, selected, effectiveKind), [selected.Id], _coreGameEngineCardConversionsSelectedResponseConversionSelectedUseConversionAccess._selectedUseConversion ?? _coreGameEngineCardConversionsSelectedResponseConversionSelectedUseConversionAccess._selectedResponseConversion) || !(_characterLuBuOlMagicOperations.MoGangquanBorrowedDistance(weaponOwner, slashTarget, effectiveKind, [selected.Id], _coreGameEngineCardConversionsSelectedResponseConversionSelectedUseConversionAccess._selectedUseConversion ?? _coreGameEngineCardConversionsSelectedResponseConversionSelectedUseConversionAccess._selectedResponseConversion) || _characterHuangfuSongWumiaoOperations.HasWuHfOrWuyouMaterialPrivilege(weaponOwner, effectiveKind, [selected.Id]) || _characterMiHengLeOperations.HasSiruiSlashDistance(weaponOwner, effectiveKind, [selected.Id], _coreGameEngineCardConversionsSelectedResponseConversionSelectedUseConversionAccess._selectedUseConversion ?? _coreGameEngineCardConversionsSelectedResponseConversionSelectedUseConversionAccess._selectedResponseConversion) || _coreRoundGainedSourceUsesOperations.HasRoundGainedBasicBonus(weaponOwner, effectiveKind, [selected.Id]) || _coreCurrentTurnShownCardBenefitsOperations.HasSlashUseDistanceBySuit(weaponOwner, effectiveKind, _coreNativeCardAppearanceOperations.EffectiveSuit(weaponOwner, selected)) || _coreTurnRedSlashBenefitsOperations.HasTurnRedSlashPolicy(weaponOwner.Seat, effectiveKind, _coreNativeCardAppearanceOperations.EffectiveSuit(weaponOwner, selected)) || _coreAlternatingSuitAndCompletedTopOperations.HasPhaseSuitAllowance(weaponOwner, selected) || _coreDirectedCardPolicyQueriesOperations.HasCardDistanceExemption(weaponOwner, slashTarget, effectiveKind, physicalCardIds: [selected.Id]) || _coreFinalTargetSlashOperations.IsWithinSpecificSlashRange(weaponOwner, slashTarget, effectiveKind, _coreFinalTargetSlashOperations.SpecificSlashRank(weaponOwner, selected, effectiveKind))))
        {
            throw new InvalidOperationException("The selected Borrowed Sword Slash target is no longer legal.");
        }

        _coreNativeResponseWindowsOperations.PopResponseWindow(pending.ResolutionId);
        _coreNativeCardUseFramesOperations.SetCardUseStep(pending.ResolutionId, ResolutionFrameStep.ResolvingEffect);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        pending.AwaitingSlashChoice = false;
        pending.SlashCardId = selected.Id;
        pending.EffectiveSlashKind = effectiveKind;
        _coreNativeSlashUseOperations.ResolveSlashCore(weaponOwner, slashTarget, selected, effectiveKind, weaponOwner.Seat, borrowedSword: pending, usesZhuqueFan: usesZhuqueFan);
    }

    internal void ResolveBorrowedSwordZhangbaSlashChoice(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending, IReadOnlyList<Card> pair)
    {
        var weaponOwner = _coreMatchParticipantsPlayersAccess._players[pending.WeaponOwnerSeat];
        var slashTarget = _coreMatchParticipantsPlayersAccess._players[pending.SlashTargetSeat];
        if (!global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword, pending) || !pending.AwaitingSlashChoice || !IsLegalBorrowedSwordSlashTarget(weaponOwner, slashTarget, physicalSuit: _coreAlternatingSuitAndCompletedTopOperations.PhysicalGroupSuit(weaponOwner, pair), allowAnyPhysicalSuit: false, effectiveColor: _coreTurnRedSlashBenefitsOperations.PhysicalGroupColor(weaponOwner, pair), effectiveRank: _coreFinalTargetSlashOperations.ZhangbaSpecificSlashRank(weaponOwner, pair), physicalCardIds: _coreProgramCardPoliciesOperations.HasCardPolicy(weaponOwner, SkillProgramCardPolicyKind.RoundGainedOtherSourceUse) ? pair.Select(c => c.Id).ToArray() : null) || _coreNativeResponseMaterialQueriesOperations.FindZhangbaSlashPair(weaponOwner, pair.Select(card => card.Id).ToArray())is null || _coreFinalTargetSlashOperations.HasRankSlashRange(weaponOwner, CardKind.Slash) && !IsLegalBorrowedSwordSlashTarget(weaponOwner, slashTarget, CardKind.Slash, _coreAlternatingSuitAndCompletedTopOperations.PhysicalGroupSuit(weaponOwner, pair), false, _coreTurnRedSlashBenefitsOperations.PhysicalGroupColor(weaponOwner, pair), _coreFinalTargetSlashOperations.ZhangbaSpecificSlashRank(weaponOwner, pair), _coreProgramCardPoliciesOperations.HasCardPolicy(weaponOwner, SkillProgramCardPolicyKind.RoundGainedOtherSourceUse) ? pair.Select(c => c.Id).ToArray() : null))
        {
            throw new InvalidOperationException("The selected Borrowed Sword Zhangba Slash is no longer legal.");
        }

        _coreNativeResponseWindowsOperations.PopResponseWindow(pending.ResolutionId);
        _coreNativeCardUseFramesOperations.SetCardUseStep(pending.ResolutionId, ResolutionFrameStep.ResolvingEffect);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        pending.AwaitingSlashChoice = false;
        pending.SlashCardId = pair[0].Id;
        pending.EffectiveSlashKind = CardKind.Slash;
        _coreNativeSlashUseOperations.ResolveSlashCore(weaponOwner, slashTarget, pair[0], CardKind.Slash, weaponOwner.Seat, borrowedSword: pending, physicalCards: pair);
    }

    internal void ResolveBorrowedSwordProgramMultiCardSlashChoice(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending, ProgramMultiCardViewAsSelection selection)
    {
        var weaponOwner = _coreMatchParticipantsPlayersAccess._players[pending.WeaponOwnerSeat];
        var slashTarget = _coreMatchParticipantsPlayersAccess._players[pending.SlashTargetSeat];
        if (!global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword, pending) || !pending.AwaitingSlashChoice || !IsLegalBorrowedSwordSlashTarget(weaponOwner, slashTarget, selection.OutputKind, _coreGuanHaiBridgesOperations.ProgramMultiCardSuit(weaponOwner, selection), false, _coreGuanHaiBridgesOperations.ProgramMultiCardColor(weaponOwner, selection), physicalCardIds: selection.Cards.Select(card => card.Id).ToArray()) || _coreCardConversionsOperations.FindProgramMultiCardViewAsSelection(weaponOwner, selection.Cards.Select(card => card.Id).ToArray(), selection.OutputKind, forResponse: false, selection.Source)is null)
        {
            throw new InvalidOperationException("The selected Borrowed Sword configured Slash is no longer legal.");
        }

        _coreNativeResponseWindowsOperations.PopResponseWindow(pending.ResolutionId);
        _coreNativeCardUseFramesOperations.SetCardUseStep(pending.ResolutionId, ResolutionFrameStep.ResolvingEffect);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        pending.AwaitingSlashChoice = false;
        pending.SlashCardId = selection.Cards[0].Id;
        pending.EffectiveSlashKind = selection.OutputKind;
        _coreCardConversionsOperations.ResolveProgramMultiCardSlash(weaponOwner, slashTarget, selection, borrowedSword: pending, enforceOwnTurnSlashLimit: false);
    }

    internal void CompleteBorrowedSwordWithoutSlash(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending, bool transferWeapon)
    {
        if (!global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword, pending) || pending.ActiveAttack is not null)
        {
            throw new InvalidOperationException("Borrowed Sword fallback is not current.");
        }

        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is ResponseWindowFrame response && response.ParentFrameId == pending.ResolutionId)
        {
            _coreNativeResponseWindowsOperations.PopResponseWindow(pending.ResolutionId);
        }

        _coreNativeCardUseFramesOperations.SetCardUseStep(pending.ResolutionId, ResolutionFrameStep.ResolvingEffect);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        pending.AwaitingSlashChoice = false;
        if (transferWeapon && _characterSimaYiMobileGodOperations.TryBeginMobileSimaRenjieBorrowedSword(pending))
            return;
        var source = _coreMatchParticipantsPlayersAccess._players[pending.SourceSeat];
        var weaponOwner = _coreMatchParticipantsPlayersAccess._players[pending.WeaponOwnerSeat];
        var weapons = transferWeapon ? _corePhysicalOwnedCardQueriesOperations.GetEquipment(weaponOwner).Where(card => _coreFieldEquipmentOperations.FieldEquipmentSlot(card) == EquipmentSlot.Weapon).ToArray() : [];
        var transferred = new List<Card>();
        if (weapons.Length > 0)
        {
            foreach (var weapon in weapons)
            {
                _corePhysicalMovementFacadesOperations.MoveCard(weapon, CardLocation.Equipment(weaponOwner.Seat), CardLocation.Processing, CardMoveReasons.BorrowedSwordGive, movement => _characterBianFuRenOlOperations.RecordBianFuRenOwnedTransferProducer(movement, source.Seat, pending.ResolutionId));
                var obtained = _coreAdvancedWeaponsOperations.MoveProcessingCardUnlessDestroyed(weapon, CardLocation.Hand(source.Seat), CardMoveReasons.BorrowedSwordGive, _characterHuangQuanMouOperations.NativeGiveCompletion(pending.ResolutionId, weaponOwner.Seat, source.Seat, [weapon.Id], _coreCardMovementJournalMovementSequenceAccess._movementSequence, null));
                if (obtained)
                    transferred.Add(weapon);
                _coreMatchLogOperations.AddLog("CardEffect", obtained ? $"{(weaponOwner.Name)} 未使用【杀】，将【{(weapon.DisplayName)}】交给 {(source.Name)}。" : $"{(weaponOwner.Name)} 未使用【杀】，【{(weapon.DisplayName)}】离开装备区后销毁。", weaponOwner.Seat, source.Seat);
            }
        }
        else
        {
            _coreMatchLogOperations.AddLog("CardEffect", $"{(weaponOwner.Name)} 已没有可交出的武器，【借刀杀人】结束。", pending.SourceSeat, weaponOwner.Seat);
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BorrowedSwordResolvedEvent(pending.ResolutionId, pending.SourceSeat, pending.WeaponOwnerSeat, pending.SlashTargetSeat, UsedSlash: false, TransferredWeaponCardId: transferred.FirstOrDefault()?.Id, TransferredWeaponKind: transferred.FirstOrDefault()?.Kind));
        FinishBorrowedSword(pending);
    }

    internal void CompleteBorrowedSwordAfterSlash(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending)
    {
        if (!global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword, pending) || pending.ActiveAttack is null || pending.SlashCardId is null || pending.EffectiveSlashKind is null)
        {
            throw new InvalidOperationException("Borrowed Sword Slash completion is not current.");
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BorrowedSwordResolvedEvent(pending.ResolutionId, pending.SourceSeat, pending.WeaponOwnerSeat, pending.SlashTargetSeat, UsedSlash: true, SlashCardId: pending.SlashCardId, EffectiveSlashKind: pending.EffectiveSlashKind));
        _coreMatchLogOperations.AddLog("CardEffect", $"{(_coreMatchParticipantsPlayersAccess._players[pending.WeaponOwnerSeat].Name)} 已按【借刀杀人】要求对 {(_coreMatchParticipantsPlayersAccess._players[pending.SlashTargetSeat].Name)} 使用【{(CardCatalog.Get(pending.EffectiveSlashKind.Value).DisplayName)}】。", pending.WeaponOwnerSeat, pending.SlashTargetSeat);
        FinishBorrowedSword(pending);
    }

    internal void FinishBorrowedSword(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending)
    {
        if (_characterZhugeGuoLeOperations.FinishLeChengyanBorrowedNative(pending))
            return;
        if (!_coreNextCardTargetAdjustmentsOperations.HasRemainingAdjustedBorrowedSwordTargets(pending.ResolutionId))
            _coreNativeNullificationFramesOperations.SetCardUseTargetIndex(pending.ResolutionId, _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(frame => frame.Id == pending.ResolutionId).TargetSeats.Count);
        _coreSequentialTrickTargetsOperations.MoveFinishedTrickCard(pending.ResolutionId, pending.Card);
        _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword = null;
        if (global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest?.BorrowedSword, pending))
        {
            _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest = null;
        }

        _coreNativeCardUseCompletionOperations.FinishCardUse(pending.ResolutionId, pending.Card, CardKind.BorrowedSword);
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None && _coreMatchOutcomeStatusWinnerAccess._status != EngineStatus.Completed)
        {
            _kernelCoreMainOperations.CompleteGame();
        }
    }
}
