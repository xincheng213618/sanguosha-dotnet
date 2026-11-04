#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeTargetLegalityRuntime
{
    internal bool CanUseSlashTarget(CharacterState source, CharacterState target, Card slashCard, CardConversionSource? conversionSource = null, CardKind effectiveKind = CardKind.Slash, bool ignoreDistance = false, long? existingUseFrameId = null, bool noEffectiveRank = false, int? specificEffectiveRank = null, IReadOnlyList<int>? physicalCardIds = null, int? excludedEquipmentId = null, EquipmentSlot? excludedEquipmentSlot = null)
    {
        if (!_coreProgramTargetRulesOperations.TargetRules.AllowsSlash(new(source, target, effectiveKind, conversionSource, physicalCardIds ?? [slashCard.Id])))
            return false;
        ignoreDistance |= CharacterLuBuOlMagicRuntime.IsMoGangquanSource(conversionSource);
        slashCard = _characterWuJingOlOperations.WuJingLiubingUseCard(source, slashCard, effectiveKind, conversionSource, physicalCardIds, existingUseFrameId);
        if (existingUseFrameId is null && excludedEquipmentId is null && excludedEquipmentSlot is null)
            excludedEquipmentId = _coreLiYiNativeQueriesOperations.LiYiPaidSlashWeapon(source, physicalCardIds ?? [slashCard.Id]);
        if (existingUseFrameId is null && _appliedCardActionRestrictions.IsMaterialUseForbidden(source.Seat, physicalCardIds ?? [slashCard.Id], _coreNativeCardAppearanceOperations.EffectiveSuit(source, _corePlayCardHistoryOperations.ApplyProgramUseAppearance(source, slashCard, conversionSource))))
            return false;
        return _coreProgramTargetRulesOperations.TargetRules.AllowsNativeUseMaterial(new(source.Seat, effectiveKind, physicalCardIds ?? (slashCard.Id > 0 ? [slashCard.Id] : []), conversionSource, existingUseFrameId)) && !_coreCardUseModulesOperations.IsCardUseForbidden(source.Seat, effectiveKind, CardActionType.Use, ignoreIssuedPlayBan: existingUseFrameId is { } id && _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Any(f => f.Id == id && f.Action?.ActorSeat == source.Seat), existingUseFrameId: existingUseFrameId) && target.IsAlive && target.Seat != source.Seat && (ignoreDistance || _coreProgramDistanceRulesOperations.DistanceRules.AllowsSlashDistance(ProgramDistanceStage.PhysicalSlash, new(source, target, effectiveKind, slashCard, conversionSource, existingUseFrameId, physicalCardIds, NoEffectiveRank: noEffectiveRank, SpecificEffectiveRank: specificEffectiveRank, ExcludedEquipmentId: excludedEquipmentId, ExcludedEquipmentSlot: excludedEquipmentSlot))) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(source.Seat, target.Seat, effectiveKind, existingUseFrameId) && !_coreNativeDamagePoliciesOperations.IsSlashProhibited(target);
    }

    internal bool CanUseVirtualSlashTarget(CharacterState source, CharacterState target, CardKind effectiveKind = CardKind.Slash, Suit? physicalSuit = null, bool? effectiveColor = null, int? effectiveRank = null, IReadOnlyList<int>? physicalCardIds = null, bool ignoreDistance = false, CardConversionSource? conversionSource = null, bool actualUnconvertedPhysical = false, bool countsTowardLimit = true)
    {
        if (!IsVirtualSlashCandidate(source, target, effectiveKind, conversionSource, physicalCardIds))
            return false;
        var material = EvaluateVirtualSlashMaterials(source, effectiveKind, physicalSuit, effectiveColor,
            physicalCardIds, ignoreDistance, conversionSource, actualUnconvertedPhysical);
        return material.Allowed && EvaluateVirtualSlashTargetTail(source, target, effectiveKind,
            effectiveRank, physicalCardIds, conversionSource, countsTowardLimit, material);
    }

    internal IReadOnlyList<int> GetVirtualSlashTargetSeats(CharacterState source, IReadOnlyList<CharacterState> candidates, CardKind effectiveKind = CardKind.Slash, Suit? physicalSuit = null, bool? effectiveColor = null, int? effectiveRank = null, IReadOnlyList<int>? physicalCardIds = null, bool ignoreDistance = false, CardConversionSource? conversionSource = null, bool actualUnconvertedPhysical = false, bool countsTowardLimit = true)
    {
        var materialInitialized = false;
        VirtualSlashMaterialEvaluation material = default;
        var targets = new List<int>();
        foreach (var target in candidates)
        {
            if (!IsVirtualSlashCandidate(source, target, effectiveKind, conversionSource, physicalCardIds))
                continue;
            // This target-independent result stays local to one read-only query.
            if (!materialInitialized)
            {
                material = EvaluateVirtualSlashMaterials(source, effectiveKind, physicalSuit, effectiveColor,
                    physicalCardIds, ignoreDistance, conversionSource, actualUnconvertedPhysical);
                materialInitialized = true;
            }
            if (material.Allowed && EvaluateVirtualSlashTargetTail(source, target, effectiveKind,
                effectiveRank, physicalCardIds, conversionSource, countsTowardLimit, material))
                targets.Add(target.Seat);
        }
        return targets.AsReadOnly();
    }

    private readonly record struct VirtualSlashMaterialEvaluation(bool Allowed, Suit? Suit, bool? Color, bool IgnoreDistance);

    private bool IsVirtualSlashCandidate(CharacterState source, CharacterState target, CardKind effectiveKind,
        CardConversionSource? conversionSource, IReadOnlyList<int>? physicalCardIds) =>
        target.IsAlive && target.Seat != source.Seat &&
        _coreProgramTargetRulesOperations.TargetRules.AllowsSlash(new(source, target, effectiveKind, conversionSource, physicalCardIds));

    private bool EvaluateVirtualSlashTargetTail(CharacterState source, CharacterState target, CardKind effectiveKind,
        int? effectiveRank, IReadOnlyList<int>? physicalCardIds, CardConversionSource? conversionSource,
        bool countsTowardLimit, VirtualSlashMaterialEvaluation material) =>
        _coreRuleQueriesOperations.CanSpendSlashUse(source, target, ignoresCount: !countsTowardLimit || _coreAlternatingSuitAndCompletedTopOperations.HasPhaseSuitAllowance2(source.Seat, material.Suit), effectiveKind, effectiveRank: effectiveRank, physicalCardIds: physicalCardIds, effectiveSuit: material.Suit) && (material.IgnoreDistance || _coreFinalTargetSlashOperations.Value.IsWithinSpecificSlashRange(source, target, effectiveKind, effectiveRank) || _coreProgramDistanceRulesOperations.DistanceRules.AllowsSlashDistance(ProgramDistanceStage.VirtualSlash, new(source, target, effectiveKind, Conversion: conversionSource, PhysicalCardIds: physicalCardIds, PhysicalSuit: material.Suit, EffectiveColor: material.Color, EffectiveRank: effectiveRank))) && !_coreCardUseModulesOperations.HasTurnCardTargetRestriction(source.Seat, SkillProgramCardTargetRestriction.SelfOnly) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(source.Seat, target.Seat, effectiveKind) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(target, effectiveKind, material.Suit, material.Color) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(source.Seat, target.Seat, material.Suit);

    private VirtualSlashMaterialEvaluation EvaluateVirtualSlashMaterials(CharacterState source, CardKind effectiveKind,
        Suit? physicalSuit, bool? effectiveColor, IReadOnlyList<int>? physicalCardIds, bool ignoreDistance,
        CardConversionSource? conversionSource, bool actualUnconvertedPhysical)
    {
        if (_coreProgramCardLegalityOperations.IsProgramMaterialUseForbidden(source.Seat, effectiveKind, physicalCardIds ?? []))
            return default;
        ignoreDistance |= CharacterLuBuOlMagicRuntime.IsMoGangquanSource(conversionSource);
        if (actualUnconvertedPhysical && conversionSource is null && physicalCardIds is [var physicalId])
        {
            var physical = _coreAdvancedLifecycleOperations.GetAdvancedCard(physicalId);
            var appearance = _characterWuJingOlOperations.WuJingLiubingUseCard(source, physical, effectiveKind, ids: physicalCardIds);
            if (appearance.Suit != physical.Suit)
            {
                physicalSuit = _coreNativeCardAppearanceOperations.EffectiveSuit(source, appearance);
                effectiveColor = CoreGameEnginePublicPileColorsRuntime.SuitColor(physicalSuit);
            }
        }

        var allowed = _coreProgramTargetRulesOperations.TargetRules.AllowsNativeUseMaterial(new(source.Seat, effectiveKind, physicalCardIds ?? [], conversionSource)) && !_coreCardUseModulesOperations.IsCardUseForbidden(source.Seat, effectiveKind, CardActionType.Use);
        return new(allowed, physicalSuit, effectiveColor, ignoreDistance);
    }

    internal bool CanUseZhangbaSerpentSpear(CharacterState actor) => CoreGameEngineDirectedCardPolicyQueriesDefinitions.SlashKinds.Any(kind => !_coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, kind, CardActionType.Use)) && _coreNativeDamageHandlesOperations.Value.UsesFormalZhangbaSerpentSpear && _coreAdvancedWeaponsOperations.HasWeaponAbility(actor, CardKind.ZhangbaSerpentSpear) && _zhangbaMaterials.HasZhangbaSlashMaterials(actor);
    internal bool HasZhuqueFan(CharacterState actor) => _coreNativeDamageHandlesOperations.Value.UsesFormalZhuqueFan && _coreAdvancedWeaponsOperations.HasWeaponAbility(actor, CardKind.ZhuqueFan);
    internal IReadOnlyList<CardKind> GetSlashUseKinds(CharacterState actor, CardKind baseEffectiveKind) => _coreProgramCardUseRulesOperations.GetSlashUseVariants2(actor, baseEffectiveKind).Select(variant => variant.EffectiveKind).Distinct().ToArray();
    internal bool IsZhuqueFanConversion(CharacterState actor, CardKind baseEffectiveKind, CardKind requestedEffectiveKind) => baseEffectiveKind == CardKind.Slash && requestedEffectiveKind == CardKind.FireSlash && HasZhuqueFan(actor);
}
