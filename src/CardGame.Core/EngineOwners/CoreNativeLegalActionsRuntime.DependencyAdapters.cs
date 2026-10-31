#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaoAngMouRuntime))]
internal sealed class CoreNativeLegalActionsCharacterCaoAngMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoAngMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCharacterCaoAngMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoAngMouRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card? MouCaQueryOriginalCard(global::CardGame.Core.CharacterState actor, int? id) => this._targetFactory().MouCaQueryOriginalCard(actor, id);
    public bool IsMouCaExactCardQuery(global::CardGame.Core.CharacterState actor, int? selected) => this._targetFactory().IsMouCaExactCardQuery(actor, selected);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreHellDizangProgramsRuntime))]
internal sealed class CoreNativeLegalActionsCoreHellDizangProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreHellDizangProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreHellDizangProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreHellDizangProgramsRuntime> _targetFactory = targetFactory;
    public bool CanHellDitingMountRecast(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card) => this._targetFactory().CanHellDitingMountRecast(actor, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDuanJiongOlRuntime))]
internal sealed class CoreNativeLegalActionsCharacterDuanJiongOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDuanJiongOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCharacterDuanJiongOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDuanJiongOlRuntime> _targetFactory = targetFactory;
    public bool IsDuanJiongSaoguDiscardQuery(global::CardGame.Core.CharacterState actor, int? id) => this._targetFactory().IsDuanJiongSaoguDiscardQuery(actor, id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuoYouZhiMouMobileRuntime))]
internal sealed class CoreNativeLegalActionsCharacterGuoYouZhiMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoYouZhiMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCharacterGuoYouZhiMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoYouZhiMouMobileRuntime> _targetFactory = targetFactory;
    public bool IsMouGyzExactCardQuery(global::CardGame.Core.CharacterState actor, int? selectedId) => this._targetFactory().IsMouGyzExactCardQuery(actor, selectedId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHellSoulCardsRuntime))]
internal sealed class CoreNativeLegalActionsCoreHellSoulCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHellSoulCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreHellSoulCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHellSoulCardsRuntime> _targetFactory = targetFactory;
    public void AddHellSoulActions(global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IEnumerable<global::CardGame.Core.Card> cards) => this._targetFactory().AddHellSoulActions(actions, actor, cards);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime))]
internal sealed class CoreNativeLegalActionsCoreNativeCardAppearanceAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreNativeCardAppearanceOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().EffectiveSuit(owner, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePaidColorConversionsRuntime))]
internal sealed class CoreNativeLegalActionsCorePaidColorConversionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePaidColorConversionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCorePaidColorConversionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePaidColorConversionsRuntime> _targetFactory = targetFactory;
    public bool IsPaidColorEquipmentDuelAction(global::CardGame.Core.CharacterState owner, global::CardGame.Core.LegalAction action, global::CardGame.Core.Card card) => this._targetFactory().IsPaidColorEquipmentDuelAction(owner, action, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWuJingOlRuntime))]
internal sealed class CoreNativeLegalActionsCharacterWuJingOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuJingOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCharacterWuJingOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuJingOlRuntime> _targetFactory = targetFactory;
    public bool HasWuJingLiubing { get => this._targetFactory().HasWuJingLiubing; }

    public global::CardGame.Core.Suit? WuJingLiubingUsedSuit(int actorSeat, global::CardGame.Core.Card card, global::CardGame.Core.CardKind effectiveKind, global::System.Collections.Generic.IReadOnlyList<int> ids, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? conversions, global::CardGame.Core.Suit? ordinary) => this._targetFactory().WuJingLiubingUsedSuit(actorSeat, card, effectiveKind, ids, conversions, ordinary);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime))]
internal sealed class CoreNativeLegalActionsCoreRuleQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreRuleQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime> _targetFactory = targetFactory;
    public int GetSlashUseLimit(global::CardGame.Core.CharacterState player, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default) => this._targetFactory().GetSlashUseLimit(player, excludedEquipmentId, excludedEquipmentSlot);
    public int GetCardUseDistanceLimit(global::CardGame.Core.CharacterState player, global::CardGame.Core.CardKind effectiveCardKind) => this._targetFactory().GetCardUseDistanceLimit(player, effectiveCardKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterJiangWeiMouMobileRuntime))]
internal sealed class CoreNativeLegalActionsCharacterJiangWeiMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterJiangWeiMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCharacterJiangWeiMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterJiangWeiMouMobileRuntime> _targetFactory = targetFactory;
    public bool IsMouJwSlashQuery(global::CardGame.Core.CharacterState actor) => this._targetFactory().IsMouJwSlashQuery(actor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime))]
internal sealed class CoreNativeLegalActionsCoreProgramCardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreProgramCardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> _targetFactory = targetFactory;
    public bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default) => this._targetFactory().HasCardPolicy(owner, kind, effectiveKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardUseRulesRuntime))]
internal sealed class CoreNativeLegalActionsCoreProgramCardUseRulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardUseRulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreProgramCardUseRulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardUseRulesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SlashUseVariant> GetSlashUseVariants(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card physicalCard, global::CardGame.Core.CardKind baseEffectiveKind, global::CardGame.Core.CardConversionSource? primaryConversionSource) => this._targetFactory().GetSlashUseVariants(actor, physicalCard, baseEffectiveKind, primaryConversionSource);
    public string DescribeAdditionalConversion(global::CardGame.Core.CardConversionSource source) => this._targetFactory().DescribeAdditionalConversion(source);
    public void AddProgramTargetCountSlashActions(global::System.Collections.Generic.ICollection<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card physicalCard, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CharacterState> legalTargets, string slashName, global::CardGame.Core.CardKind? playedCardKind, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default) => this._targetFactory().AddProgramTargetCountSlashActions(actions, actor, physicalCard, legalTargets, slashName, playedCardKind, conversionSource, additionalConversionSources);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SlashUseVariant> GetSlashUseVariants(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind baseEffectiveKind) => this._targetFactory().GetSlashUseVariants(actor, baseEffectiveKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeOwnedCardQueriesRuntime))]
internal sealed class CoreNativeLegalActionsCoreNativeOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreNativeOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card? FindOwnedPlayableCard(global::CardGame.Core.CharacterState actor, int? cardId) => this._targetFactory().FindOwnedPlayableCard(actor, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime))]
internal sealed class CoreNativeLegalActionsCorePhysicalOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCorePhysicalOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetPlayableCards(global::CardGame.Core.CharacterState player) => this._targetFactory().GetPlayableCards(player);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player) => this._targetFactory().GetEquipment(player);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player) => this._targetFactory().GetHand(player);
    public bool HasTargetCard(global::CardGame.Core.CharacterState player) => this._targetFactory().HasTargetCard(player);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetAuthority(global::CardGame.Core.CharacterState player) => this._targetFactory().GetAuthority(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuCheErMouRuntime))]
internal sealed class CoreNativeLegalActionsCharacterHuCheErMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuCheErMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCharacterHuCheErMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuCheErMouRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> KongwuEffectiveEquipment(global::CardGame.Core.CharacterState player) => this._targetFactory().KongwuEffectiveEquipment(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime))]
internal sealed class CoreNativeLegalActionsCoreEquipmentContentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreEquipmentContentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.EquipmentContentRegistry EquipmentRules { get => this._targetFactory().EquipmentRules; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime))]
internal sealed class CoreNativeLegalActionsCharacterDaQiaoXiaoQiaoHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCharacterDaQiaoXiaoQiaoHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime> _targetFactory = targetFactory;
    public bool IsHappyQiaoShutuCardQuery(global::CardGame.Core.CharacterState actor, int? selectedPhysicalCardId) => this._targetFactory().IsHappyQiaoShutuCardQuery(actor, selectedPhysicalCardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuoYuanMobileShiRuntime))]
internal sealed class CoreNativeLegalActionsCharacterGuoYuanMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoYuanMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCharacterGuoYuanMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoYuanMobileShiRuntime> _targetFactory = targetFactory;
    public bool IsShiGyHandQuery(global::CardGame.Core.CharacterState actor, int? id) => this._targetFactory().IsShiGyHandQuery(actor, id);
    public bool IsShiGyOffTurnQuery(global::CardGame.Core.CharacterState actor, int? card) => this._targetFactory().IsShiGyOffTurnQuery(actor, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuanYuRuntimeRuntime))]
internal sealed class CoreNativeLegalActionsCharacterGuanYuRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCharacterGuanYuRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuRuntimeRuntime> _targetFactory = targetFactory;
    public bool GuanFirstRedDistance(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, long? useId = default) => this._targetFactory().GuanFirstRedDistance(owner, card, useId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuangfuSongWumiaoRuntime))]
internal sealed class CoreNativeLegalActionsCharacterHuangfuSongWumiaoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangfuSongWumiaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCharacterHuangfuSongWumiaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangfuSongWumiaoRuntime> _targetFactory = targetFactory;
    public bool HasWuHfOrDixianPotential(int actor) => this._targetFactory().HasWuHfOrDixianPotential(actor);
    public bool HasWuHfOrDixianMaterialPrivilege(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int>? ids, global::CardGame.Core.CardConversionSource? source = default) => this._targetFactory().HasWuHfOrDixianMaterialPrivilege(actor, kind, ids, source);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuangQuanOlRuntime))]
internal sealed class CoreNativeLegalActionsCharacterHuangQuanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangQuanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCharacterHuangQuanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangQuanOlRuntime> _targetFactory = targetFactory;
    public bool IsJianjiExactCardQuery(global::CardGame.Core.CharacterState actor, int? selectedId) => this._targetFactory().IsJianjiExactCardQuery(actor, selectedId);
    public bool CanUseJianjiOrdinaryAlcohol(global::CardGame.Core.CharacterState actor, int? selectedId) => this._targetFactory().CanUseJianjiOrdinaryAlcohol(actor, selectedId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuBuMobileGodRuntime))]
internal sealed class CoreNativeLegalActionsCharacterLuBuMobileGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuBuMobileGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCharacterLuBuMobileGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuBuMobileGodRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> ProjectMobileWumouActions(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> allActions, int? selectedPhysicalCardId) => this._targetFactory().ProjectMobileWumouActions(actor, allActions, selectedPhysicalCardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRoundGainedSourceUsesRuntime))]
internal sealed class CoreNativeLegalActionsCoreRoundGainedSourceUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundGainedSourceUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreRoundGainedSourceUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundGainedSourceUsesRuntime> _targetFactory = targetFactory;
    public bool HasRoundGainedBasicBonus(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> ids) => this._targetFactory().HasRoundGainedBasicBonus(owner, kind, ids);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuBuOlMagicRuntime))]
internal sealed class CoreNativeLegalActionsCharacterLuBuOlMagicAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuBuOlMagicRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCharacterLuBuOlMagicOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuBuOlMagicRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> FilterMoGangquanActions(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IEnumerable<global::CardGame.Core.LegalAction> actions) => this._targetFactory().FilterMoGangquanActions(actor, actions);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramParticipantPhaseOperationsRuntime))]
internal sealed class CoreNativeLegalActionsCoreProgramParticipantPhaseOperationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramParticipantPhaseOperationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreProgramParticipantPhaseOperationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramParticipantPhaseOperationsRuntime> _targetFactory = targetFactory;
    public bool IsTurnSuitUseForbidden(int seat, global::CardGame.Core.Suit suit) => this._targetFactory().IsTurnSuitUseForbidden(seat, suit);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterYaDanOlRuntime))]
internal sealed class CoreNativeLegalActionsCharacterYaDanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYaDanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCharacterYaDanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYaDanOlRuntime> _targetFactory = targetFactory;
    public bool IsQingyaDiscardQuery(global::CardGame.Core.CharacterState actor, int? id) => this._targetFactory().IsQingyaDiscardQuery(actor, id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePublicSlashTraitsRuntime))]
internal sealed class CoreNativeLegalActionsCorePublicSlashTraitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicSlashTraitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCorePublicSlashTraitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicSlashTraitsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PublicSlashTraits CreatePublicSlashTraits(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action, global::CardGame.Core.CardKind kind, bool? declaredColor, global::CardGame.Core.GameSnapshot? ownerView) => this._targetFactory().CreatePublicSlashTraits(actor, action, kind, declaredColor, ownerView);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatCardsRuntime))]
internal sealed class CoreNativeLegalActionsCoreDragonBoatCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreDragonBoatCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatCardsRuntime> _targetFactory = targetFactory;
    public void AddDragonBoatCardActions(global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IEnumerable<global::CardGame.Core.Card> cards) => this._targetFactory().AddDragonBoatCardActions(actions, actor, cards);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineLastZoneConversionsRuntime))]
internal sealed class CoreNativeLegalActionsCoreLastZoneConversionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLastZoneConversionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreLastZoneConversionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLastZoneConversionsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetLastZoneJudgmentConversionCards(global::CardGame.Core.CharacterState owner, bool forResponse) => this._targetFactory().GetLastZoneJudgmentConversionCards(owner, forResponse);
    public bool IsLastZoneJudgmentCard(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().IsLastZoneJudgmentCard(owner, card);
    public global::CardGame.Core.Card? FindLastZoneJudgmentConversionCard(global::CardGame.Core.CharacterState owner, int cardId) => this._targetFactory().FindLastZoneJudgmentConversionCard(owner, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreNativeLegalActionsCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public void ValidatePlayerSeat(int seat, string parameterName) => this._targetFactory().ValidatePlayerSeat(seat, parameterName);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTurnTargetCardQuotaAllowancesRuntime))]
internal sealed class CoreNativeLegalActionsCoreTurnTargetCardQuotaAllowancesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTurnTargetCardQuotaAllowancesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreTurnTargetCardQuotaAllowancesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTurnTargetCardQuotaAllowancesRuntime> _targetFactory = targetFactory;
    public bool HasTargetCardQuotaAllowance(int actor, int target) => this._targetFactory().HasTargetCardQuotaAllowance(actor, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWenhePoisonRuntime))]
internal sealed class CoreNativeLegalActionsCoreWenhePoisonAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenhePoisonRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreWenhePoisonOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenhePoisonRuntime> _targetFactory = targetFactory;
    public bool IsWenhePhysicalPoison(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().IsWenhePhysicalPoison(owner, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramMultiTargetDuelsRuntime))]
internal sealed class CoreNativeLegalActionsCoreProgramMultiTargetDuelsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramMultiTargetDuelsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreProgramMultiTargetDuelsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramMultiTargetDuelsRuntime> _targetFactory = targetFactory;
    public void AddProgramMultiTargetDuelActions(global::System.Collections.Generic.ICollection<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor) => this._targetFactory().AddProgramMultiTargetDuelActions(actions, actor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWenheCardsRuntime))]
internal sealed class CoreNativeLegalActionsCoreWenheCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenheCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreWenheCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenheCardsRuntime> _targetFactory = targetFactory;
    public void AddWenheCardActions(global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IEnumerable<global::CardGame.Core.Card> cards) => this._targetFactory().AddWenheCardActions(actions, actor, cards);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHulaoCardsRuntime))]
internal sealed class CoreNativeLegalActionsCoreHulaoCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHulaoCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreHulaoCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHulaoCardsRuntime> _targetFactory = targetFactory;
    public void AddHulaoAllianceFeastActions(global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IEnumerable<global::CardGame.Core.Card> playableCards) => this._targetFactory().AddHulaoAllianceFeastActions(actions, actor, playableCards);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime))]
internal sealed class CoreNativeLegalActionsCoreNativeDamageHandlesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreNativeDamageHandlesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> _targetFactory = targetFactory;
    public bool IsNationalWarMode { get => this._targetFactory().IsNationalWarMode; }
    public bool UsesFormalBorrowedSword { get => this._targetFactory().UsesFormalBorrowedSword; }
    public bool UsesFormalWoodenOx { get => this._targetFactory().UsesFormalWoodenOx; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProvenanceCardPoliciesRuntime))]
internal sealed class CoreNativeLegalActionsCoreProvenanceCardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProvenanceCardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreProvenanceCardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProvenanceCardPoliciesRuntime> _targetFactory = targetFactory;
    public bool HasProvenanceUseDistance(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().HasProvenanceUseDistance(owner, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDirectedCardPolicyQueriesRuntime))]
internal sealed class CoreNativeLegalActionsCoreDirectedCardPolicyQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDirectedCardPolicyQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreDirectedCardPolicyQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDirectedCardPolicyQueriesRuntime> _targetFactory = targetFactory;
    public bool HasSlashAllowanceForAnyTarget(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IEnumerable<global::CardGame.Core.CardKind>? kinds = default) => this._targetFactory().HasSlashAllowanceForAnyTarget(actor, kinds);
    public bool CanUseGlobalCard(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind, bool excludeOwner = false) => this._targetFactory().CanUseGlobalCard(actor, kind, excludeOwner);
    public bool HasCardDistanceExemption(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind kind, long? cardUseFrameId = default, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default) => this._targetFactory().HasCardDistanceExemption(actor, target, kind, cardUseFrameId, excludedEquipmentId, excludedEquipmentSlot, physicalCardIds);
    public bool HasYuanchouCardPolicy { get => this._targetFactory().HasYuanchouCardPolicy; }

    public global::System.Collections.Generic.IEnumerable<int> GetDeclaredCardTargets(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalActionKind actionKind, global::System.Collections.Generic.IReadOnlyList<int> targets) => this._targetFactory().GetDeclaredCardTargets(actor, actionKind, targets);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNativePublicPileOriginalCardsRuntime))]
internal sealed class CoreNativeLegalActionsCoreNativePublicPileOriginalCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativePublicPileOriginalCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreNativePublicPileOriginalCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativePublicPileOriginalCardsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> NativePublicPileOriginalPlayActions(global::CardGame.Core.CharacterState actor, int? selectedId) => this._targetFactory().NativePublicPileOriginalPlayActions(actor, selectedId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGuanduCardsRuntime))]
internal sealed class CoreNativeLegalActionsCoreGuanduCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreGuanduCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduCardsRuntime> _targetFactory = targetFactory;
    public void AddGuanduCardActions(global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IEnumerable<global::CardGame.Core.Card> cards) => this._targetFactory().AddGuanduCardActions(actions, actor, cards);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWeiYanHappyGodRuntime))]
internal sealed class CoreNativeLegalActionsCharacterWeiYanHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWeiYanHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCharacterWeiYanHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWeiYanHappyGodRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card? HappyWeiYanBoneReturnCard(global::CardGame.Core.CharacterState actor, int? id) => this._targetFactory().HappyWeiYanBoneReturnCard(actor, id);
    public bool IsHappyWeiYanBoneReturnQuery(global::CardGame.Core.CharacterState actor, int? id) => this._targetFactory().IsHappyWeiYanBoneReturnQuery(actor, id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMaTieAnniversaryRuntime))]
internal sealed class CoreNativeLegalActionsCharacterMaTieAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaTieAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCharacterMaTieAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaTieAnniversaryRuntime> _targetFactory = targetFactory;
    public bool IsAnniversaryMaTieSlashQuery(global::CardGame.Core.CharacterState actor) => this._targetFactory().IsAnniversaryMaTieSlashQuery(actor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuanYuWumiaoRuntime))]
internal sealed class CoreNativeLegalActionsCharacterGuanYuWumiaoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuWumiaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCharacterGuanYuWumiaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuWumiaoRuntime> _targetFactory = targetFactory;
    public bool HasWuyouPotentialSlash(global::CardGame.Core.CharacterState actor) => this._targetFactory().HasWuyouPotentialSlash(actor);
    public bool IsJuewuSource(global::CardGame.Core.CardConversionSource? source) => this._targetFactory().IsJuewuSource(source);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineModeCardRecastsRuntime))]
internal sealed class CoreNativeLegalActionsCoreModeCardRecastsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineModeCardRecastsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreModeCardRecastsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineModeCardRecastsRuntime> _targetFactory = targetFactory;
    public bool CanModeHandEquipmentRecast(global::CardGame.Core.Card card) => this._targetFactory().CanModeHandEquipmentRecast(card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime))]
internal sealed class CoreNativeLegalActionsCoreSkillProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreSkillProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.LegalAction> BuildProgramActions(global::CardGame.Core.CharacterState owner) => this._targetFactory().BuildProgramActions(owner);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterJuShouMouRuntime))]
internal sealed class CoreNativeLegalActionsCharacterJuShouMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterJuShouMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCharacterJuShouMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterJuShouMouRuntime> _targetFactory = targetFactory;
    public bool IsMouJsExactCardQuery(global::CardGame.Core.CharacterState actor, int? selected) => this._targetFactory().IsMouJsExactCardQuery(actor, selected);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramIronChainTargetsRuntime))]
internal sealed class CoreNativeLegalActionsCoreProgramIronChainTargetsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramIronChainTargetsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreProgramIronChainTargetsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramIronChainTargetsRuntime> _targetFactory = targetFactory;
    public void AddProgramIronChainUseActions(global::System.Collections.Generic.ICollection<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card physicalCard, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CharacterState> candidates, global::CardGame.Core.CardKind? playedCardKind = default, global::CardGame.Core.CardConversionSource? conversionSource = default) => this._targetFactory().AddProgramIronChainUseActions(actions, actor, physicalCard, candidates, playedCardKind, conversionSource);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicConversionsRuntime))]
internal sealed class CoreNativeLegalActionsCoreChainedStateBasicConversionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicConversionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreChainedStateBasicConversionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicConversionsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> ChainedStateBasicPlayActions(global::CardGame.Core.CharacterState actor) => this._targetFactory().ChainedStateBasicPlayActions(actor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuangGaiMouMobileRuntime))]
internal sealed class CoreNativeLegalActionsCharacterHuangGaiMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangGaiMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCharacterHuangGaiMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangGaiMouMobileRuntime> _targetFactory = targetFactory;
    public bool HasMouZhaJiangPreUsePrivilege(global::CardGame.Core.CharacterState actor) => this._targetFactory().HasMouZhaJiangPreUsePrivilege(actor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterJiaXuMouRuntime))]
internal sealed class CoreNativeLegalActionsCharacterJiaXuMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterJiaXuMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCharacterJiaXuMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterJiaXuMouRuntime> _targetFactory = targetFactory;
    public bool IsMouJxExactCardQuery(global::CardGame.Core.CharacterState actor, int? selected) => this._targetFactory().IsMouJxExactCardQuery(actor, selected);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNationalGeneralRevealRuntime))]
internal sealed class CoreNativeLegalActionsCoreNationalGeneralRevealAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNationalGeneralRevealRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreNationalGeneralRevealOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNationalGeneralRevealRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> BuildNationalRevealActions(global::CardGame.Core.CharacterState actor) => this._targetFactory().BuildNationalRevealActions(actor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFieldEquipmentRuntime))]
internal sealed class CoreNativeLegalActionsCoreFieldEquipmentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFieldEquipmentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreFieldEquipmentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFieldEquipmentRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.EquipmentSlot FieldEquipmentSlot(global::CardGame.Core.Card card) => this._targetFactory().FieldEquipmentSlot(card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeBorrowedSwordRuntime))]
internal sealed class CoreNativeLegalActionsCoreNativeBorrowedSwordAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeBorrowedSwordRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreNativeBorrowedSwordOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeBorrowedSwordRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card? GetWeapon(global::CardGame.Core.CharacterState player) => this._targetFactory().GetWeapon(player);
    public bool IsLegalBorrowedSwordSlashTarget(global::CardGame.Core.CharacterState weaponOwner, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.Suit? physicalSuit = default, bool allowAnyPhysicalSuit = true, bool? effectiveColor = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, global::CardGame.Core.CardConversionSource? conversionSource = default) => this._targetFactory().IsLegalBorrowedSwordSlashTarget(weaponOwner, target, effectiveKind, physicalSuit, allowAnyPhysicalSuit, effectiveColor, effectiveRank, physicalCardIds, conversionSource);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramCardLegalityRuntime))]
internal sealed class CoreNativeLegalActionsCoreProgramCardLegalityAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramCardLegalityRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreProgramCardLegalityOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramCardLegalityRuntime> _targetFactory = targetFactory;
    public bool IsProgramEntityPhysicalRestricted(int actorSeat, int cardId) => this._targetFactory().IsProgramEntityPhysicalRestricted(actorSeat, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShenwuCardsRuntime))]
internal sealed class CoreNativeLegalActionsCoreShenwuCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShenwuCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreShenwuCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShenwuCardsRuntime> _targetFactory = targetFactory;
    public void AddShenwuTrickActions(global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IEnumerable<global::CardGame.Core.Card> playableCards) => this._targetFactory().AddShenwuTrickActions(actions, actor, playableCards);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime))]
internal sealed class CoreNativeLegalActionsCorePlayerViewProjectionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCorePlayerViewProjectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDeckEquipmentProgramsRuntime))]
internal sealed class CoreNativeLegalActionsCoreDeckEquipmentProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeckEquipmentProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreDeckEquipmentProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeckEquipmentProgramsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> FilterBeneficiarySuitShieldActions(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> actions) => this._targetFactory().FilterBeneficiarySuitShieldActions(actor, actions);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterBaiLingYunAnniversaryRuntime))]
internal sealed class CoreNativeLegalActionsCharacterBaiLingYunAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterBaiLingYunAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCharacterBaiLingYunAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterBaiLingYunAnniversaryRuntime> _targetFactory = targetFactory;
    public bool IsAnniversaryBaiLingYunExactCardQuery(global::CardGame.Core.CharacterState actor, int? selectedPhysicalCardId) => this._targetFactory().IsAnniversaryBaiLingYunExactCardQuery(actor, selectedPhysicalCardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime))]
internal sealed class CoreNativeLegalActionsCoreCardConversionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreCardConversionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime> _targetFactory = targetFactory;
    public bool HasProgramCardIdentity(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().HasProgramCardIdentity(owner, card);
    public bool IgnoresProgramSlashLimit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardConversionSource? source) => this._targetFactory().IgnoresProgramSlashLimit(owner, source);
    public string DescribeConversion(global::CardGame.Core.CardConversionSource? source, string description) => this._targetFactory().DescribeConversion(source, description);
    public void AddNuzhanUnlimitedTrickSlashActions(global::System.Collections.Generic.ICollection<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> playableCards) => this._targetFactory().AddNuzhanUnlimitedTrickSlashActions(actions, actor, playableCards);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSunYuanOlRuntime))]
internal sealed class CoreNativeLegalActionsCharacterSunYuanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunYuanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCharacterSunYuanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunYuanOlRuntime> _targetFactory = targetFactory;
    public bool HasDongxinEntitySlashQuota(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> ids) => this._targetFactory().HasDongxinEntitySlashQuota(actor, kind, ids);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterKeBiNengOlRuntime))]
internal sealed class CoreNativeLegalActionsCharacterKeBiNengOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterKeBiNengOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCharacterKeBiNengOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterKeBiNengOlRuntime> _targetFactory = targetFactory;
    public bool IsKeBiNengPingduanBasicQuery(global::CardGame.Core.CharacterState actor) => this._targetFactory().IsKeBiNengPingduanBasicQuery(actor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSunHuanAnniversaryRuntime))]
internal sealed class CoreNativeLegalActionsCharacterSunHuanAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunHuanAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCharacterSunHuanAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunHuanAnniversaryRuntime> _targetFactory = targetFactory;
    public bool IsAnniversarySunHuanExactCardQuery(global::CardGame.Core.CharacterState actor, int? selectedPhysicalCardId) => this._targetFactory().IsAnniversarySunHuanExactCardQuery(actor, selectedPhysicalCardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramParticipantPromptsRuntime))]
internal sealed class CoreNativeLegalActionsCoreProgramParticipantPromptsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramParticipantPromptsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreProgramParticipantPromptsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramParticipantPromptsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? GetPendingDecisionForActor(int actorSeat) => this._targetFactory().GetPendingDecisionForActor(actorSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterKongRongMouOlRuntime))]
internal sealed class CoreNativeLegalActionsCharacterKongRongMouOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterKongRongMouOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCharacterKongRongMouOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterKongRongMouOlRuntime> _targetFactory = targetFactory;
    public bool IsMouKrLiwenUseQuery(global::CardGame.Core.CharacterState actor) => this._targetFactory().IsMouKrLiwenUseQuery(actor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActionQueriesRuntime))]
internal sealed class CoreNativeLegalActionsCoreActionQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActionQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreActionQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActionQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.ActionQueryScope BeginActionQuery() => this._targetFactory().BeginActionQuery();
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GameEngine.ProgramCardIdentityMatch> GetProgramCardIdentityMatches(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().GetProgramCardIdentityMatches(owner, card);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource> GetProgramViewAsConversions(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind outputKind, bool forResponse, bool dyingUse = false) => this._targetFactory().GetProgramViewAsConversions(owner, card, outputKind, forResponse, dyingUse);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime))]
internal sealed class CoreNativeLegalActionsCoreCardUseModulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreCardUseModulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime> _targetFactory = targetFactory;
    public bool IsTurnHandCardRestricted(global::CardGame.Core.CharacterState player, global::CardGame.Core.Card card) => this._targetFactory().IsTurnHandCardRestricted(player, card);
    public bool IsCardUseForbidden(int actorSeat, global::CardGame.Core.CardKind effectiveKind, global::CardGame.Core.CardActionType actionType, bool ignoreIssuedPlayBan = false, long? existingUseFrameId = default) => this._targetFactory().IsCardUseForbidden(actorSeat, effectiveKind, actionType, ignoreIssuedPlayBan, existingUseFrameId);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> FilterTurnCardUseRestrictions(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IEnumerable<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.Card? nativeOriginalCard = default) => this._targetFactory().FilterTurnCardUseRestrictions(actor, actions, nativeOriginalCard);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramActionModulesRuntime))]
internal sealed class CoreNativeLegalActionsCoreProgramActionModulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramActionModulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreProgramActionModulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramActionModulesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramActionRouter ProgramActions { get => this._targetFactory().ProgramActions; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroUsesRuntime))]
internal sealed class CoreNativeLegalActionsCoreTieredRoundZeroUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreTieredRoundZeroUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroUsesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> TieredRoundZeroPlayActions(global::CardGame.Core.CharacterState actor) => this._targetFactory().TieredRoundZeroPlayActions(actor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineZhangbaSelectionsRuntime))]
internal sealed class CoreNativeLegalActionsCoreZhangbaSelectionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineZhangbaSelectionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreZhangbaSelectionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineZhangbaSelectionsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ActiveSkillCardGroupTarget> GetZhangbaSlashGroupTargets(global::CardGame.Core.CharacterState actor) => this._targetFactory().GetZhangbaSlashGroupTargets(actor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMaChaoAnniversaryGodRuntime))]
internal sealed class CoreNativeLegalActionsCharacterMaChaoAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaChaoAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCharacterMaChaoAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaChaoAnniversaryGodRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card? FindShouliFieldCard(global::CardGame.Core.CharacterState actor, int? id) => this._targetFactory().FindShouliFieldCard(actor, id);
    public bool IsAcceptedShouliMaterial(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.CardConversionSource? source, bool response) => this._targetFactory().IsAcceptedShouliMaterial(actor, card, source, response);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDirectedTurnCardPoliciesRuntime))]
internal sealed class CoreNativeLegalActionsCoreDirectedTurnCardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDirectedTurnCardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreDirectedTurnCardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDirectedTurnCardPoliciesRuntime> _targetFactory = targetFactory;
    public bool IsDirectedCardTargetProhibited(int actorSeat, int targetSeat, global::CardGame.Core.CardKind cardKind, long? cardUseFrameId = default) => this._targetFactory().IsDirectedCardTargetProhibited(actorSeat, targetSeat, cardKind, cardUseFrameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime))]
internal sealed class CoreNativeLegalActionsCoreNativeRuleQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreNativeRuleQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> _targetFactory = targetFactory;
    public int GetCombatDistance(int sourceSeat, int targetSeat) => this._targetFactory().GetCombatDistance(sourceSeat, targetSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDyingWindowSkillPoliciesRuntime))]
internal sealed class CoreNativeLegalActionsCoreDyingWindowSkillPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingWindowSkillPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreDyingWindowSkillPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingWindowSkillPoliciesRuntime> _targetFactory = targetFactory;
    public bool HasBlackTrickTargetPolicy { get => this._targetFactory().HasBlackTrickTargetPolicy; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHealthQueriesRuntime))]
internal sealed class CoreNativeLegalActionsCoreHealthQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHealthQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreHealthQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHealthQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> HappyQiaoActionQueryUnion(global::System.Func<global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction>> query) => this._targetFactory().HappyQiaoActionQueryUnion(query);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCurrentSlashFireOffersRuntime))]
internal sealed class CoreNativeLegalActionsCoreCurrentSlashFireOffersAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentSlashFireOffersRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreCurrentSlashFireOffersOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentSlashFireOffersRuntime> _targetFactory = targetFactory;
    public void AddCurrentNonFireConvertedTargetActions(global::System.Collections.Generic.ICollection<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, int? selectedPhysicalCardId = default) => this._targetFactory().AddCurrentNonFireConvertedTargetActions(actions, actor, selectedPhysicalCardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHuxinMirrorRuntime))]
internal sealed class CoreNativeLegalActionsCoreHuxinMirrorAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHuxinMirrorRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreHuxinMirrorOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHuxinMirrorRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.LegalAction> HuxinMirrorPlacementActions(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards) => this._targetFactory().HuxinMirrorPlacementActions(actor, cards);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineBoundDiscardSlashBenefitsRuntime))]
internal sealed class CoreNativeLegalActionsCoreBoundDiscardSlashBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBoundDiscardSlashBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreBoundDiscardSlashBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBoundDiscardSlashBenefitsRuntime> _targetFactory = targetFactory;
    public bool HasTurnAlcoholSuitAllowance(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card? material = default, global::CardGame.Core.CardConversionSource? conversion = default) => this._targetFactory().HasTurnAlcoholSuitAllowance(actor, material, conversion);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCuiYanOlRuntime))]
internal sealed class CoreNativeLegalActionsCharacterCuiYanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCuiYanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCharacterCuiYanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCuiYanOlRuntime> _targetFactory = targetFactory;
    public bool IsCuiYanNextUseForbidden(int actorSeat, global::System.Collections.Generic.IReadOnlyList<int> materialIds, global::CardGame.Core.Suit? nativeEffectiveSuit) => this._targetFactory().IsCuiYanNextUseForbidden(actorSeat, materialIds, nativeEffectiveSuit);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangHuanAnniversaryRuntime))]
internal sealed class CoreNativeLegalActionsCharacterZhangHuanAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangHuanAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCharacterZhangHuanAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangHuanAnniversaryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card? AnniversaryZhangHuanQueryOriginalCard(global::CardGame.Core.CharacterState actor, int? id) => this._targetFactory().AnniversaryZhangHuanQueryOriginalCard(actor, id);
    public bool IsAnniversaryZhangHuanExactCardQuery(global::CardGame.Core.CharacterState actor, int? selected) => this._targetFactory().IsAnniversaryZhangHuanExactCardQuery(actor, selected);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeTargetLegalityRuntime))]
internal sealed class CoreNativeLegalActionsCoreNativeTargetLegalityAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTargetLegalityRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreNativeTargetLegalityOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTargetLegalityRuntime> _targetFactory = targetFactory;
    public bool CanUseZhangbaSerpentSpear(global::CardGame.Core.CharacterState actor) => this._targetFactory().CanUseZhangbaSerpentSpear(actor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineUnexpectedAssaultActionsRuntime))]
internal sealed class CoreNativeLegalActionsCoreUnexpectedAssaultActionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnexpectedAssaultActionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreUnexpectedAssaultActionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnexpectedAssaultActionsRuntime> _targetFactory = targetFactory;
    public void AddUnexpectedAssaultActions(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> playableCards, global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions) => this._targetFactory().AddUnexpectedAssaultActions(actor, playableCards, actions);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreNativeLegalActionsCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaiWenJiLeRuntime))]
internal sealed class CoreNativeLegalActionsCharacterCaiWenJiLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaiWenJiLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCharacterCaiWenJiLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaiWenJiLeRuntime> _targetFactory = targetFactory;
    public bool HasBeifenPreUsePrivilege(global::CardGame.Core.CharacterState actor) => this._targetFactory().HasBeifenPreUsePrivilege(actor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTreatedWoundedRuntime))]
internal sealed class CoreNativeLegalActionsCoreTreatedWoundedAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTreatedWoundedRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreTreatedWoundedOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTreatedWoundedRuntime> _targetFactory = targetFactory;
    public bool IsRuleWounded(global::CardGame.Core.CharacterState player) => this._targetFactory().IsRuleWounded(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingSuitAndCompletedTopRuntime))]
internal sealed class CoreNativeLegalActionsCoreAlternatingSuitAndCompletedTopAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingSuitAndCompletedTopRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreAlternatingSuitAndCompletedTopOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingSuitAndCompletedTopRuntime> _targetFactory = targetFactory;
    public bool HasPhaseSuitAllowance(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().HasPhaseSuitAllowance(owner, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeMultiTargetActionsRuntime))]
internal sealed class CoreNativeLegalActionsCoreNativeMultiTargetActionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeMultiTargetActionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreNativeMultiTargetActionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeMultiTargetActionsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CharacterState> GetFangtianOrderedSlashTargets(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card slash, global::CardGame.Core.CardConversionSource? conversionSource = default, bool ignoresSlashLimit = false, global::CardGame.Core.CardKind effectiveKind = (global::CardGame.Core.CardKind)0) => this._targetFactory().GetFangtianOrderedSlashTargets(actor, slash, conversionSource, ignoresSlashLimit, effectiveKind);
    public void AddFangtianHalberdSlashActions(global::System.Collections.Generic.ICollection<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card physicalCard, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CharacterState> legalTargets, string slashName, global::CardGame.Core.CardKind? playedCardKind, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default) => this._targetFactory().AddFangtianHalberdSlashActions(actions, actor, physicalCard, legalTargets, slashName, playedCardKind, conversionSource, additionalConversionSources);
    public void AddTargetCardActions(global::System.Collections.Generic.ICollection<global::CardGame.Core.LegalAction> actions, int actorSeat, global::CardGame.Core.LegalActionKind actionKind, global::CardGame.Core.Card effectCard, global::CardGame.Core.CharacterState target, string effectName, global::CardGame.Core.CardKind? playedCardKind = default, global::CardGame.Core.CardConversionSource? conversionSource = default) => this._targetFactory().AddTargetCardActions(actions, actorSeat, actionKind, effectCard, target, effectName, playedCardKind, conversionSource);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineMantianGuohaiRuntime))]
internal sealed class CoreNativeLegalActionsCoreMantianGuohaiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMantianGuohaiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreMantianGuohaiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMantianGuohaiRuntime> _targetFactory = targetFactory;
    public void AddMantianGuohaiActions(global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IEnumerable<global::CardGame.Core.Card> playableCards) => this._targetFactory().AddMantianGuohaiActions(actions, actor, playableCards);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamagePoliciesRuntime))]
internal sealed class CoreNativeLegalActionsCoreNativeDamagePoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamagePoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreNativeDamagePoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamagePoliciesRuntime> _targetFactory = targetFactory;
    public bool IsCardTargetProhibited(global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind cardKind, global::CardGame.Core.Suit? suit = default, bool? effectiveIsRed = default) => this._targetFactory().IsCardTargetProhibited(target, cardKind, suit, effectiveIsRed);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuBanOlRuntime))]
internal sealed class CoreNativeLegalActionsCharacterHuBanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuBanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCharacterHuBanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuBanOlRuntime> _targetFactory = targetFactory;
    public bool IsHuiyunExactHandQuery(global::CardGame.Core.CharacterState actor, int? id) => this._targetFactory().IsHuiyunExactHandQuery(actor, id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhugeLiangMouRuntime))]
internal sealed class CoreNativeLegalActionsCharacterZhugeLiangMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeLiangMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCharacterZhugeLiangMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeLiangMouRuntime> _targetFactory = targetFactory;
    public bool HasMouWujingAllowance(global::CardGame.Core.CharacterState owner) => this._targetFactory().HasMouWujingAllowance(owner);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSunJunMobileShiRuntime))]
internal sealed class CoreNativeLegalActionsCharacterSunJunMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunJunMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCharacterSunJunMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunJunMobileShiRuntime> _targetFactory = targetFactory;
    public bool HasShiSunJunFirstSuitAllowance(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card? material = default, global::CardGame.Core.CardConversionSource? conversion = default) => this._targetFactory().HasShiSunJunFirstSuitAllowance(actor, material, conversion);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime))]
internal sealed class CoreNativeLegalActionsCoreActualTurnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreActualTurnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime> _targetFactory = targetFactory;
    public bool HasJudgmentEffectiveCard(global::CardGame.Core.CharacterState player, global::CardGame.Core.CardKind kind) => this._targetFactory().HasJudgmentEffectiveCard(player, kind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterJiangJiMouRuntime))]
internal sealed class CoreNativeLegalActionsCharacterJiangJiMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterJiangJiMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCharacterJiangJiMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterJiangJiMouRuntime> _targetFactory = targetFactory;
    public bool IsMouJjExactCardQuery(global::CardGame.Core.CharacterState actor, int? selected) => this._targetFactory().IsMouJjExactCardQuery(actor, selected);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicOffersRuntime))]
internal sealed class CoreNativeLegalActionsCoreDrawFundedDistinctBasicOffersAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicOffersRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreDrawFundedDistinctBasicOffersOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicOffersRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> DrawFundedDistinctBasicPlayActions(global::CardGame.Core.CharacterState actor) => this._targetFactory().DrawFundedDistinctBasicPlayActions(actor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOwnPublicPileConversionsRuntime))]
internal sealed class CoreNativeLegalActionsCoreOwnPublicPileConversionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnPublicPileConversionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreOwnPublicPileConversionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnPublicPileConversionsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetOwnPublicPileConversionCards(global::CardGame.Core.CharacterState owner) => this._targetFactory().GetOwnPublicPileConversionCards(owner);
    public bool IsOwnPublicPileCard(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().IsOwnPublicPileCard(owner, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhouFeiBoundaryMobileRuntime))]
internal sealed class CoreNativeLegalActionsCharacterZhouFeiBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouFeiBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCharacterZhouFeiBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouFeiBoundaryMobileRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card? FindBoundaryZhouFeiUseCard(global::CardGame.Core.CharacterState actor, int? id) => this._targetFactory().FindBoundaryZhouFeiUseCard(actor, id);
    public bool IsBoundaryZhouFeiUseQuery(global::CardGame.Core.CharacterState actor, int? selectedId) => this._targetFactory().IsBoundaryZhouFeiUseQuery(actor, selectedId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhouYuMouRuntime))]
internal sealed class CoreNativeLegalActionsCharacterZhouYuMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouYuMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCharacterZhouYuMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouYuMouRuntime> _targetFactory = targetFactory;
    public bool IsMouZyExactCardQuery(global::CardGame.Core.CharacterState actor, int? id) => this._targetFactory().IsMouZyExactCardQuery(actor, id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuangQuanMobileShiRuntime))]
internal sealed class CoreNativeLegalActionsCharacterHuangQuanMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangQuanMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCharacterHuangQuanMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangQuanMobileShiRuntime> _targetFactory = targetFactory;
    public bool ShiQuanGushiUnlimitedCard(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> ids) => this._targetFactory().ShiQuanGushiUnlimitedCard(actor, kind, ids);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime))]
internal sealed class CoreNativeLegalActionsCorePlayCardHistoryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCorePlayCardHistoryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card ApplyProgramUseAppearance(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardConversionSource? source) => this._targetFactory().ApplyProgramUseAppearance(owner, card, source);
    public global::CardGame.Core.SkillProgramViewAs? ViewAsRule(global::CardGame.Core.CardConversionSource source) => this._targetFactory().ViewAsRule(source);
    public global::CardGame.Core.Suit? PreviousPlaySuitForInheritance(int actorSeat) => this._targetFactory().PreviousPlaySuitForInheritance(actorSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterFuRongOlRuntime))]
internal sealed class CoreNativeLegalActionsCharacterFuRongOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterFuRongOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCharacterFuRongOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterFuRongOlRuntime> _targetFactory = targetFactory;
    public bool IsFuRongDiscardQuery(global::CardGame.Core.CharacterState actor, int? id) => this._targetFactory().IsFuRongDiscardQuery(actor, id);
    public bool HasFuRongDiscardDistance(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int>? ids) => this._targetFactory().HasFuRongDiscardDistance(actor, kind, ids);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineJudgedRankSlashPoliciesRuntime))]
internal sealed class CoreNativeLegalActionsCoreJudgedRankSlashPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJudgedRankSlashPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreJudgedRankSlashPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJudgedRankSlashPoliciesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.TurnJudgedRankSlashPolicy? CurrentJudgedRankSlashPolicy(int actor) => this._targetFactory().CurrentJudgedRankSlashPolicy(actor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedPoliciesRuntime))]
internal sealed class CoreNativeLegalActionsCoreAdvancedPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreAdvancedPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedPoliciesRuntime> _targetFactory = targetFactory;
    public bool HasNextUnlimitedCard(global::CardGame.Core.CharacterState owner) => this._targetFactory().HasNextUnlimitedCard(owner);
    public bool HasFriendQihuiAllowance(global::CardGame.Core.CharacterState owner) => this._targetFactory().HasFriendQihuiAllowance(owner);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWushuangSkillsBRuntime))]
internal sealed class CoreNativeLegalActionsCoreWushuangSkillsBAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangSkillsBRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreWushuangSkillsBOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangSkillsBRuntime> _targetFactory = targetFactory;
    public bool HasWushuangBianyuPotential(global::CardGame.Core.CharacterState owner) => this._targetFactory().HasWushuangBianyuPotential(owner);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessNativeUsesRuntime))]
internal sealed class CoreNativeLegalActionsCoreRoundOnceCardlessNativeUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessNativeUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCoreRoundOnceCardlessNativeUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessNativeUsesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> RoundOnceCardlessPlayActions(global::CardGame.Core.CharacterState actor) => this._targetFactory().RoundOnceCardlessPlayActions(actor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuSuMouRuntime))]
internal sealed class CoreNativeLegalActionsCharacterLuSuMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuSuMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeLegalActionsCharacterLuSuMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuSuMouRuntime> _targetFactory = targetFactory;
    public bool IsMouLuSuUseQuery(global::CardGame.Core.CharacterState actor) => this._targetFactory().IsMouLuSuUseQuery(actor);
}
