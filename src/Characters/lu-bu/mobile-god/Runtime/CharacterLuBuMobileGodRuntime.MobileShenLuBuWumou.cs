#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuBuMobileGodRuntime
{
    internal const string MobileLbWumou = "mobile-god:wumou";
    internal CardConversionSource? MobileWumouSource(CharacterState actor, Card card) => _coreActionQueriesOperations.GetProgramCardIdentityMatches(actor, card).FirstOrDefault(m => m.Source.SkillId == MobileLbWumou && m.Identity.OutputKind == CardKind.Slash)?.Source;
    internal IReadOnlyList<int> MobileWumouTemplateTargets(CharacterState actor, Card card, LegalAction template)
    {
        // Some native producers intentionally omit implicit targets from their
        // action. Retain the same targets their actual resolvers would declare.
        IReadOnlyList<int> targets = template.Kind switch
        {
            LegalActionKind.DrawTwo or LegalActionKind.SowBeans or LegalActionKind.BeamSwap or LegalActionKind.WenheChaos => [actor.Seat],
            LegalActionKind.ReturnSoul => _coreHellSoulCardsOperations.HellReturnSoulUseTargets(actor, card),
            LegalActionKind.AllianceFeast => _coreCardUseModulesOperations.ApplyTurnCardGroupTargetRestrictions(actor, Enumerable.Range(0, _coreMatchDefinitionPlayerCountAccess._playerCount).Select(offset => _coreMatchParticipantsPlayersAccess._players[_coreTableOrderOperations.TableStepSeat(actor.Seat, offset)]).Where(target => target.IsAlive && !_coreShenwuEquipmentOperations.IsShenwuRobeTargetProhibited(actor.Seat, target.Seat, card.Kind) && !_coreProgramCardPoliciesOperations.HasCardPolicy(target, SkillProgramCardPolicyKind.ExcludeGlobalTarget, card.Kind) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(target, card.Kind, card.Suit, global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(actor, card))) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, target.Seat, _coreNativeCardAppearanceOperations.EffectiveSuit(actor, card))).Select(target => target.Seat).ToArray()),
            _ => _coreDragonBoatCardsOperations.DragonBoatExpectedNativeTargets(actor, card, template) ?? _coreGuanduCardsOperations.GuanduExpectedNativeTargets(actor, card, template) ?? _coreDirectedCardPolicyQueriesOperations.GetDeclaredCardTargets(actor, template.Kind, template.TargetSeats).ToArray()};
        return Array.AsReadOnly(targets.Distinct().OrderBy(s => _coreTableOrderOperations.RelativeTableOrder(actor.Seat, s)).ToArray());
    }

    internal IReadOnlyList<LegalAction> ProjectMobileWumouActions(CharacterState actor, IReadOnlyList<LegalAction> allActions, int? selectedPhysicalCardId)
    {
        var materials = _corePhysicalOwnedCardQueriesOperations.GetPlayableCards(actor).Where(c => selectedPhysicalCardId is null || c.Id == selectedPhysicalCardId).Select(c => (Card: c, Source: MobileWumouSource(actor, c))).Where(p => p.Source is not null).ToArray();
        if (materials.Length == 0)
            return allActions;
        var ids = materials.Select(p => p.Card.Id).ToHashSet();
        var result = allActions.Where(a => a.CardId is not { } id || !ids.Contains(id)).ToList();
        foreach (var(card, source)in materials)
            foreach (var template in _coreActionQueriesOperations.BuildMobileWumouPrintedTargetActions(actor, card))
                foreach (var variant in _coreProgramCardUseRulesOperations.GetSlashUseVariants(actor, card, CardKind.Slash, source))
                {
                    var targets = MobileWumouTemplateTargets(actor, card, template).Where(s => _coreMatchParticipantsPlayersAccess._players[s].IsAlive && !_coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, variant.EffectiveKind, CardActionType.Use) && !_coreProgramCardLegalityOperations.CardLegality.IsMaterialForbidden(new(actor.Seat, variant.EffectiveKind, [card.Id], false)) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, s, variant.EffectiveKind) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[s], variant.EffectiveKind, _coreNativeCardAppearanceOperations.EffectiveSuit(actor, card), global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(actor, card))) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, s, _coreNativeCardAppearanceOperations.EffectiveSuit(actor, card)) && _coreRuleQueriesOperations.CanSpendSlashUse(actor, _coreMatchParticipantsPlayersAccess._players[s], _coreCardConversionsOperations.IgnoresProgramSlashLimit(actor, source), variant.EffectiveKind, card)).ToArray();
                    if (targets.Length == 0)
                        continue;
                    var action = new LegalAction(LegalActionKind.Slash, card.Id, targets[0], $"【无谋】将【{(CardCatalog.Get(card.Kind).DisplayName)}】当【{(CardCatalog.Get(variant.EffectiveKind).DisplayName)}】使用，保留原锦囊目标", PlayedCardKind: variant.EffectiveKind, TargetSeats: targets)
                    {
                        ConversionSource = source,
                        AdditionalConversionSources = variant.AdditionalConversionSource is { } extra ? [extra] : null
                    };
                    if (!result.Any(a => SameMobileWumouAction(a, action)))
                        result.Add(action);
                }

        return _coreDeckEquipmentProgramsOperations.FilterBeneficiarySuitShieldActions(actor, _coreCardUseModulesOperations.FilterTurnCardUseRestrictions(actor, result, nativeOriginalCard: null));
    }

    internal static bool SameMobileWumouAction(LegalAction a, LegalAction b) => a.Kind == b.Kind && a.CardId == b.CardId && a.PlayedCardKind == b.PlayedCardKind && a.TargetSeats.SequenceEqual(b.TargetSeats) && a.ConversionSource == b.ConversionSource && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.ConversionSourcesEqual(a.AdditionalConversionSources, b.AdditionalConversionSources);
    internal bool TryResolveMobileWumouUse(CharacterState actor, LegalAction action)
    {
        if (action.ConversionSource?.SkillId != MobileLbWumou)
            return false;
        var card = _coreNativeOwnedCardQueriesOperations.FindOwnedPlayableCard(actor, action.CardId) ?? throw new InvalidOperationException("Wumou lost its real ordinary-trick material.");
        if (action.Kind != LegalActionKind.Slash || MobileWumouSource(actor, card) != action.ConversionSource || action.PlayedCardKind is not { } kind || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind) || !ProjectMobileWumouActions(actor, [], card.Id).Any(a => SameMobileWumouAction(a, action)))
            throw new InvalidOperationException("Wumou must retain its exact native original-trick target template, Slash quota and conversion source.");
        if (action.TargetSeats.Count == 1)
        {
            var fan = kind == CardKind.FireSlash && (action.AdditionalConversionSources is null || action.AdditionalConversionSources.Count == 0) && !_coreCardConversionsOperations.IsDirectProgramFireSlashConversion(actor, card, action.ConversionSource);
            _coreNativeSlashUseOperations.ResolveSlashCore(actor, _coreMatchParticipantsPlayersAccess._players[action.TargetSeats[0]], card, kind, actor.Seat, usesZhuqueFan: fan, conversionSource: action.ConversionSource, additionalConversionSources: action.AdditionalConversionSources);
            return true;
        }

        var id = _coreNativeCardUseOperations.BeginCardUse(card, actor.Seat, action.TargetSeats, kind, _coreNativeEquipmentAndFactionQueriesOperations.HasArmorBypass(actor), conversionSource: action.ConversionSource, additionalConversionSources: action.AdditionalConversionSources);
        var nuzhan = _coreCardConversionsOperations.GetNuzhanModifiers(id, actor);
        _coreCardUsePaymentsOperations.PaySlashUseMaterials([card], actor.Seat, null);
        _coreProgramCardUsePaymentsOperations.CardUsePayments.CompleteSlashPayment(id);
        var counted = !(_coreCardUseLifecycleOperations.LifecycleCardUse(id)?.UnlimitedUse == true) && !_coreProgramCardUsePaymentsOperations.CardUsePayments.ExemptsSlashDebit(ProgramCardUsePaymentStage.SlashCountExemption, id, actor.Seat) && !nuzhan.IgnoresSlashLimit && !_coreCardConversionsOperations.IgnoresProgramSlashLimit(actor, action.ConversionSource) && _coreActualTurnProgressCurrentSeatPhaseAccess._phase == TurnPhase.Play && actor.Seat == _coreActualTurnProgressCurrentSeatPhaseAccess._currentSeat;
        if (counted)
            _coreCardUseDebitsOperations.RecordSlashUseDebit(id, actor.Seat);
        _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actor.Seat, kind);
        var amount = 1 + actor.AlcoholDamageBonus + nuzhan.DamageBonus;
        _coreAlcoholSlashSuppressionsOperations.CaptureProgramAlcoholConsumption(id, actor);
        actor.HasAlcoholEffect = false;
        var pending = _kernelCoreMainOperations.CreateFangtianHalberdHandle(id, actor.Seat, card, kind, _coreNativeEquipmentAndFactionQueriesOperations.HasArmorBypass(actor), amount, action.TargetSeats, usesFangtian: false, countedTowardSlashLimit: counted, conversionSource: action.ConversionSource);
        _coreNativeCardContinuationCursorsOperations.ActiveFangtianHalberd = pending;
        if (kind == CardKind.FireSlash && (action.AdditionalConversionSources is null || action.AdditionalConversionSources.Count == 0) && !_coreCardConversionsOperations.IsDirectProgramFireSlashConversion(actor, card, action.ConversionSource))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhuqueFanConvertedEvent(id, actor.Seat, Array.AsReadOnly(new[] { card.Id }), action.TargetSeats));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(card.Id, kind, actor.Seat, action.TargetSeats[0], IgnoresArmor: _coreNativeEquipmentAndFactionQueriesOperations.HasArmorBypass(actor)));
        foreach (var target in action.TargetSeats)
            _coreNativeAiDecisionsOperations.NotifyAiOfSlash(actor, _coreMatchParticipantsPlayersAccess._players[target]);
        _coreNativeSlashUseOperations.BeginNextFangtianHalberdTarget(pending);
        return true;
    }
}
