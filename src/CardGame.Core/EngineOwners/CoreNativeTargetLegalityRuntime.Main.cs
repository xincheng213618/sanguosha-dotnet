#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeTargetLegalityRuntime
{
    internal bool CanUseSlashTarget(CharacterState source, CharacterState target, Card slashCard, CardConversionSource? conversionSource = null, CardKind effectiveKind = CardKind.Slash, bool ignoreDistance = false, long? existingUseFrameId = null, bool noEffectiveRank = false, int? specificEffectiveRank = null, IReadOnlyList<int>? physicalCardIds = null, int? excludedEquipmentId = null, EquipmentSlot? excludedEquipmentSlot = null)
    {
        if (!_coreProgramTargetRulesOperations.TargetRules.AllowsSlash(new(source, target, effectiveKind, conversionSource, physicalCardIds ?? [slashCard.Id])))
            return false;
        ignoreDistance |= global::CardGame.Core.EngineOwners.CharacterLuBuOlMagicRuntime.IsMoGangquanSource(conversionSource);
        slashCard = _characterWuJingOlOperations.WuJingLiubingUseCard(source, slashCard, effectiveKind, conversionSource, physicalCardIds, existingUseFrameId);
        if (existingUseFrameId is null && excludedEquipmentId is null && excludedEquipmentSlot is null)
            excludedEquipmentId = _coreLiYiNativeQueriesOperations.LiYiPaidSlashWeapon(source, physicalCardIds ?? [slashCard.Id]);
        if (existingUseFrameId is null && _characterCuiYanOlOperations.HasCuiYanCapability && _characterCuiYanOlOperations.IsCuiYanNextUseForbidden(source.Seat, physicalCardIds ?? [slashCard.Id], _coreNativeCardAppearanceOperations.EffectiveSuit(source, _corePlayCardHistoryOperations.ApplyProgramUseAppearance(source, slashCard, conversionSource))))
            return false;
        return _coreProgramTargetRulesOperations.TargetRules.AllowsNativeUseMaterial(new(source.Seat, effectiveKind, physicalCardIds ?? (slashCard.Id > 0 ? [slashCard.Id] : []), conversionSource, existingUseFrameId)) && !_coreCardUseModulesOperations.IsCardUseForbidden(source.Seat, effectiveKind, CardActionType.Use, ignoreIssuedPlayBan: existingUseFrameId is { } id && _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Any(f => f.Id == id && f.Action?.ActorSeat == source.Seat), existingUseFrameId: existingUseFrameId) && target.IsAlive && target.Seat != source.Seat && (ignoreDistance || _coreProgramDistanceRulesOperations.DistanceRules.AllowsSlashDistance(ProgramDistanceStage.PhysicalSlash, new(source, target, effectiveKind, slashCard, conversionSource, existingUseFrameId, physicalCardIds, NoEffectiveRank: noEffectiveRank, SpecificEffectiveRank: specificEffectiveRank, ExcludedEquipmentId: excludedEquipmentId, ExcludedEquipmentSlot: excludedEquipmentSlot))) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(source.Seat, target.Seat, effectiveKind, existingUseFrameId) && !_coreNativeDamagePoliciesOperations.IsSlashProhibited2(source, target, slashCard);
    }

    internal bool CanUseVirtualSlashTarget(CharacterState source, CharacterState target, CardKind effectiveKind = CardKind.Slash, Suit? physicalSuit = null, bool? effectiveColor = null, int? effectiveRank = null, IReadOnlyList<int>? physicalCardIds = null, bool ignoreDistance = false, CardConversionSource? conversionSource = null, bool actualUnconvertedPhysical = false, bool countsTowardLimit = true)
    {
        if (!target.IsAlive || target.Seat == source.Seat)
            return false;
        if (!_coreProgramTargetRulesOperations.TargetRules.AllowsSlash(new(source, target, effectiveKind, conversionSource, physicalCardIds)))
            return false;
        if (_coreProgramCardLegalityOperations.IsProgramMaterialUseForbidden(source.Seat, effectiveKind, physicalCardIds ?? []))
            return false;
        ignoreDistance |= global::CardGame.Core.EngineOwners.CharacterLuBuOlMagicRuntime.IsMoGangquanSource(conversionSource);
        if (actualUnconvertedPhysical && conversionSource is null && physicalCardIds is [var physicalId])
        {
            var physical = _coreAdvancedLifecycleOperations.GetAdvancedCard(physicalId);
            var appearance = _characterWuJingOlOperations.WuJingLiubingUseCard(source, physical, effectiveKind, ids: physicalCardIds);
            if (appearance.Suit != physical.Suit)
            {
                physicalSuit = _coreNativeCardAppearanceOperations.EffectiveSuit(source, appearance);
                effectiveColor = global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileColorsRuntime.SuitColor(physicalSuit);
            }
        }

        return _coreProgramTargetRulesOperations.TargetRules.AllowsNativeUseMaterial(new(source.Seat, effectiveKind, physicalCardIds ?? [], conversionSource)) && !_coreCardUseModulesOperations.IsCardUseForbidden(source.Seat, effectiveKind, CardActionType.Use) && _coreRuleQueriesOperations.CanSpendSlashUse(source, target, ignoresCount: !countsTowardLimit || _coreAlternatingSuitAndCompletedTopOperations.HasPhaseSuitAllowance2(source.Seat, physicalSuit), effectiveKind, effectiveRank: effectiveRank, physicalCardIds: physicalCardIds, effectiveSuit: physicalSuit) && (ignoreDistance || _coreProgramDistanceRulesOperations.DistanceRules.AllowsSlashDistance(ProgramDistanceStage.VirtualSlash, new(source, target, effectiveKind, Conversion: conversionSource, PhysicalCardIds: physicalCardIds, PhysicalSuit: physicalSuit, EffectiveColor: effectiveColor, EffectiveRank: effectiveRank))) && !_coreNativeDamagePoliciesOperations.IsSlashProhibited(target);
    }

    internal bool CanUseZhangbaSerpentSpear(CharacterState actor) => global::CardGame.Core.EngineOwners.CoreGameEngineDirectedCardPolicyQueriesDefinitions.SlashKinds.Any(kind => !_coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, kind, CardActionType.Use)) && _coreNativeDamageHandlesOperations.UsesFormalZhangbaSerpentSpear && _coreAdvancedWeaponsOperations.HasWeaponAbility(actor, CardKind.ZhangbaSerpentSpear) && _corePhysicalOwnedCardQueriesOperations.GetPlayableCards(actor).Count(card => !_coreCardUseModulesOperations.IsTurnHandCardRestricted(actor, card) && !_characterZhugeLiangWumiaoOperations.IsWuZglCopyBlocked(actor.Seat, card.Id)) >= 2;
    internal bool HasZhuqueFan(CharacterState actor) => _coreNativeDamageHandlesOperations.UsesFormalZhuqueFan && _coreAdvancedWeaponsOperations.HasWeaponAbility(actor, CardKind.ZhuqueFan);
    internal IReadOnlyList<CardKind> GetSlashUseKinds(CharacterState actor, CardKind baseEffectiveKind) => _coreProgramCardUseRulesOperations.GetSlashUseVariants2(actor, baseEffectiveKind).Select(variant => variant.EffectiveKind).Distinct().ToArray();
    internal bool IsZhuqueFanConversion(CharacterState actor, CardKind baseEffectiveKind, CardKind requestedEffectiveKind) => baseEffectiveKind == CardKind.Slash && requestedEffectiveKind == CardKind.FireSlash && HasZhuqueFan(actor);
}
