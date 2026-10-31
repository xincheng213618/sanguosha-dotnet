#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreNativeLegalActionsCharacterCaoAngMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card? MouCaQueryOriginalCard(global::CardGame.Core.CharacterState actor, int? id);
    bool IsMouCaExactCardQuery(global::CardGame.Core.CharacterState actor, int? selected);
}

internal interface ICoreNativeLegalActionsCoreHellDizangProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanHellDitingMountRecast(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card);
}

internal interface ICoreNativeLegalActionsCharacterDuanJiongOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDuanJiongSaoguDiscardQuery(global::CardGame.Core.CharacterState actor, int? id);
}

internal interface ICoreNativeLegalActionsCharacterGuoYouZhiMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouGyzExactCardQuery(global::CardGame.Core.CharacterState actor, int? selectedId);
}

internal interface ICoreNativeLegalActionsCoreHellSoulCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddHellSoulActions(global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IEnumerable<global::CardGame.Core.Card> cards);
}

internal interface ICoreNativeLegalActionsCoreNativeCardAppearanceOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreNativeLegalActionsCorePaidColorConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPaidColorEquipmentDuelAction(global::CardGame.Core.CharacterState owner, global::CardGame.Core.LegalAction action, global::CardGame.Core.Card card);
}

internal interface ICoreNativeLegalActionsCharacterWuJingOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWuJingLiubing { get; }

    global::CardGame.Core.Suit? WuJingLiubingUsedSuit(int actorSeat, global::CardGame.Core.Card card, global::CardGame.Core.CardKind effectiveKind, global::System.Collections.Generic.IReadOnlyList<int> ids, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? conversions, global::CardGame.Core.Suit? ordinary);
}

internal interface ICoreNativeLegalActionsCoreRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetSlashUseLimit(global::CardGame.Core.CharacterState player, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default);
    int GetCardUseDistanceLimit(global::CardGame.Core.CharacterState player, global::CardGame.Core.CardKind effectiveCardKind);
}

internal interface ICoreNativeLegalActionsCharacterJiangWeiMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouJwSlashQuery(global::CardGame.Core.CharacterState actor);
}

internal interface ICoreNativeLegalActionsCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default);
}

internal interface ICoreNativeLegalActionsCoreProgramCardUseRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SlashUseVariant> GetSlashUseVariants(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card physicalCard, global::CardGame.Core.CardKind baseEffectiveKind, global::CardGame.Core.CardConversionSource? primaryConversionSource);
    string DescribeAdditionalConversion(global::CardGame.Core.CardConversionSource source);
    void AddProgramTargetCountSlashActions(global::System.Collections.Generic.ICollection<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card physicalCard, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CharacterState> legalTargets, string slashName, global::CardGame.Core.CardKind? playedCardKind, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SlashUseVariant> GetSlashUseVariants(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind baseEffectiveKind);
}

internal interface ICoreNativeLegalActionsCoreNativeOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card? FindOwnedPlayableCard(global::CardGame.Core.CharacterState actor, int? cardId);
}

internal interface ICoreNativeLegalActionsCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetPlayableCards(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
    bool HasTargetCard(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetAuthority(global::CardGame.Core.CharacterState player);
}

internal interface ICoreNativeLegalActionsCharacterHuCheErMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> KongwuEffectiveEquipment(global::CardGame.Core.CharacterState player);
}

internal interface ICoreNativeLegalActionsCoreEquipmentContentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.EquipmentContentRegistry EquipmentRules { get; }
}

internal interface ICoreNativeLegalActionsCharacterDaQiaoXiaoQiaoHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsHappyQiaoShutuCardQuery(global::CardGame.Core.CharacterState actor, int? selectedPhysicalCardId);
}

internal interface ICoreNativeLegalActionsCharacterGuoYuanMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsShiGyHandQuery(global::CardGame.Core.CharacterState actor, int? id);
    bool IsShiGyOffTurnQuery(global::CardGame.Core.CharacterState actor, int? card);
}

internal interface ICoreNativeLegalActionsCharacterGuanYuRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool GuanFirstRedDistance(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, long? useId = default);
}

internal interface ICoreNativeLegalActionsCharacterHuangfuSongWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWuHfOrDixianPotential(int actor);
    bool HasWuHfOrDixianMaterialPrivilege(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int>? ids, global::CardGame.Core.CardConversionSource? source = default);
}

internal interface ICoreNativeLegalActionsCharacterHuangQuanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsJianjiExactCardQuery(global::CardGame.Core.CharacterState actor, int? selectedId);
    bool CanUseJianjiOrdinaryAlcohol(global::CardGame.Core.CharacterState actor, int? selectedId);
}

internal interface ICoreNativeLegalActionsCharacterLuBuMobileGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> ProjectMobileWumouActions(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> allActions, int? selectedPhysicalCardId);
}

internal interface ICoreNativeLegalActionsCoreRoundGainedSourceUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRoundGainedBasicBonus(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> ids);
}

internal interface ICoreNativeLegalActionsCharacterLuBuOlMagicOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> FilterMoGangquanActions(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IEnumerable<global::CardGame.Core.LegalAction> actions);
}

internal interface ICoreNativeLegalActionsCoreProgramParticipantPhaseOperationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsTurnSuitUseForbidden(int seat, global::CardGame.Core.Suit suit);
}

internal interface ICoreNativeLegalActionsCharacterYaDanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsQingyaDiscardQuery(global::CardGame.Core.CharacterState actor, int? id);
}

internal interface ICoreNativeLegalActionsCorePublicSlashTraitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PublicSlashTraits CreatePublicSlashTraits(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action, global::CardGame.Core.CardKind kind, bool? declaredColor, global::CardGame.Core.GameSnapshot? ownerView);
}

internal interface ICoreNativeLegalActionsCoreDragonBoatCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddDragonBoatCardActions(global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IEnumerable<global::CardGame.Core.Card> cards);
}

internal interface ICoreNativeLegalActionsCoreLastZoneConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetLastZoneJudgmentConversionCards(global::CardGame.Core.CharacterState owner, bool forResponse);
    bool IsLastZoneJudgmentCard(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
    global::CardGame.Core.Card? FindLastZoneJudgmentConversionCard(global::CardGame.Core.CharacterState owner, int cardId);
}

internal interface ICoreNativeLegalActionsCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ValidatePlayerSeat(int seat, string parameterName);
}

internal interface ICoreNativeLegalActionsCoreTurnTargetCardQuotaAllowancesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasTargetCardQuotaAllowance(int actor, int target);
}

internal interface ICoreNativeLegalActionsCoreWenhePoisonOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWenhePhysicalPoison(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreNativeLegalActionsCoreProgramMultiTargetDuelsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddProgramMultiTargetDuelActions(global::System.Collections.Generic.ICollection<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor);
}

internal interface ICoreNativeLegalActionsCoreWenheCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddWenheCardActions(global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IEnumerable<global::CardGame.Core.Card> cards);
}

internal interface ICoreNativeLegalActionsCoreHulaoCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddHulaoAllianceFeastActions(global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IEnumerable<global::CardGame.Core.Card> playableCards);
}

internal interface ICoreNativeLegalActionsCoreNativeDamageHandlesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsNationalWarMode { get; }

    bool UsesFormalBorrowedSword { get; }

    bool UsesFormalWoodenOx { get; }
}

internal interface ICoreNativeLegalActionsCoreProvenanceCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasProvenanceUseDistance(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreNativeLegalActionsCoreDirectedCardPolicyQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasSlashAllowanceForAnyTarget(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IEnumerable<global::CardGame.Core.CardKind>? kinds = default);
    bool CanUseGlobalCard(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind, bool excludeOwner = false);
    bool HasCardDistanceExemption(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind kind, long? cardUseFrameId = default, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default);
    bool HasYuanchouCardPolicy { get; }

    global::System.Collections.Generic.IEnumerable<int> GetDeclaredCardTargets(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalActionKind actionKind, global::System.Collections.Generic.IReadOnlyList<int> targets);
}

internal interface ICoreNativeLegalActionsCoreNativePublicPileOriginalCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> NativePublicPileOriginalPlayActions(global::CardGame.Core.CharacterState actor, int? selectedId);
}

internal interface ICoreNativeLegalActionsProgramPipelineHooksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ProgramPipelineHooks _programPipelineHooks { get; }
}

internal interface ICoreNativeLegalActionsCoreGuanduCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddGuanduCardActions(global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IEnumerable<global::CardGame.Core.Card> cards);
}

internal interface ICoreNativeLegalActionsCharacterWeiYanHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card? HappyWeiYanBoneReturnCard(global::CardGame.Core.CharacterState actor, int? id);
    bool IsHappyWeiYanBoneReturnQuery(global::CardGame.Core.CharacterState actor, int? id);
}

internal interface ICoreNativeLegalActionsCharacterMaTieAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsAnniversaryMaTieSlashQuery(global::CardGame.Core.CharacterState actor);
}

internal interface ICoreNativeLegalActionsCharacterGuanYuWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWuyouPotentialSlash(global::CardGame.Core.CharacterState actor);
    bool IsJuewuSource(global::CardGame.Core.CardConversionSource? source);
}

internal interface ICoreNativeLegalActionsCoreModeCardRecastsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanModeHandEquipmentRecast(global::CardGame.Core.Card card);
}

internal interface ICoreNativeLegalActionsCoreSkillProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.LegalAction> BuildProgramActions(global::CardGame.Core.CharacterState owner);
}

internal interface ICoreNativeLegalActionsCharacterJuShouMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouJsExactCardQuery(global::CardGame.Core.CharacterState actor, int? selected);
}

internal interface ICoreNativeLegalActionsCoreProgramIronChainTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddProgramIronChainUseActions(global::System.Collections.Generic.ICollection<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card physicalCard, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CharacterState> candidates, global::CardGame.Core.CardKind? playedCardKind = default, global::CardGame.Core.CardConversionSource? conversionSource = default);
}

internal interface ICoreNativeLegalActionsCoreChainedStateBasicConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> ChainedStateBasicPlayActions(global::CardGame.Core.CharacterState actor);
}

internal interface ICoreNativeLegalActionsCharacterHuangGaiMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMouZhaJiangPreUsePrivilege(global::CardGame.Core.CharacterState actor);
}

internal interface ICoreNativeLegalActionsCharacterJiaXuMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouJxExactCardQuery(global::CardGame.Core.CharacterState actor, int? selected);
}

internal interface ICoreNativeLegalActionsCoreNationalGeneralRevealOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> BuildNationalRevealActions(global::CardGame.Core.CharacterState actor);
}

internal interface ICoreNativeLegalActionsCoreFieldEquipmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.EquipmentSlot FieldEquipmentSlot(global::CardGame.Core.Card card);
}

internal interface ICoreNativeLegalActionsCoreNativeBorrowedSwordOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card? GetWeapon(global::CardGame.Core.CharacterState player);
    bool IsLegalBorrowedSwordSlashTarget(global::CardGame.Core.CharacterState weaponOwner, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.Suit? physicalSuit = default, bool allowAnyPhysicalSuit = true, bool? effectiveColor = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, global::CardGame.Core.CardConversionSource? conversionSource = default);
}

internal interface ICoreNativeLegalActionsCoreProgramCardLegalityOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsProgramEntityPhysicalRestricted(int actorSeat, int cardId);
}

internal interface ICoreNativeLegalActionsCoreShenwuCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddShenwuTrickActions(global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IEnumerable<global::CardGame.Core.Card> playableCards);
}

internal interface ICoreNativeLegalActionsCorePlayerViewProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICoreNativeLegalActionsCoreDeckEquipmentProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> FilterBeneficiarySuitShieldActions(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> actions);
}

internal interface ICoreNativeLegalActionsCharacterBaiLingYunAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsAnniversaryBaiLingYunExactCardQuery(global::CardGame.Core.CharacterState actor, int? selectedPhysicalCardId);
}

internal interface ICoreNativeLegalActionsCoreCardConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasProgramCardIdentity(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
    bool IgnoresProgramSlashLimit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardConversionSource? source);
    string DescribeConversion(global::CardGame.Core.CardConversionSource? source, string description);
    void AddNuzhanUnlimitedTrickSlashActions(global::System.Collections.Generic.ICollection<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> playableCards);
}

internal interface ICoreNativeLegalActionsCharacterSunYuanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasDongxinEntitySlashQuota(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> ids);
}

internal interface ICoreNativeLegalActionsCharacterKeBiNengOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsKeBiNengPingduanBasicQuery(global::CardGame.Core.CharacterState actor);
}

internal interface ICoreNativeLegalActionsCharacterSunHuanAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsAnniversarySunHuanExactCardQuery(global::CardGame.Core.CharacterState actor, int? selectedPhysicalCardId);
}

internal interface ICoreNativeLegalActionsCoreProgramParticipantPromptsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? GetPendingDecisionForActor(int actorSeat);
}

internal interface ICoreNativeLegalActionsCharacterKongRongMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouKrLiwenUseQuery(global::CardGame.Core.CharacterState actor);
}

internal interface ICoreNativeLegalActionsCoreActionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.ActionQueryScope BeginActionQuery();
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GameEngine.ProgramCardIdentityMatch> GetProgramCardIdentityMatches(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource> GetProgramViewAsConversions(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind outputKind, bool forResponse, bool dyingUse = false);
}

internal interface ICoreNativeLegalActionsCoreCardUseModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsTurnHandCardRestricted(global::CardGame.Core.CharacterState player, global::CardGame.Core.Card card);
    bool IsCardUseForbidden(int actorSeat, global::CardGame.Core.CardKind effectiveKind, global::CardGame.Core.CardActionType actionType, bool ignoreIssuedPlayBan = false, long? existingUseFrameId = default);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> FilterTurnCardUseRestrictions(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IEnumerable<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.Card? nativeOriginalCard = default);
}

internal interface ICoreNativeLegalActionsCoreProgramActionModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramActionRouter ProgramActions { get; }
}

internal interface ICoreNativeLegalActionsCoreTieredRoundZeroUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> TieredRoundZeroPlayActions(global::CardGame.Core.CharacterState actor);
}

internal interface ICoreNativeLegalActionsCoreZhangbaSelectionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ActiveSkillCardGroupTarget> GetZhangbaSlashGroupTargets(global::CardGame.Core.CharacterState actor);
}

internal interface ICoreNativeLegalActionsCharacterMaChaoAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card? FindShouliFieldCard(global::CardGame.Core.CharacterState actor, int? id);
    bool IsAcceptedShouliMaterial(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.CardConversionSource? source, bool response);
}

internal interface ICoreNativeLegalActionsCoreDirectedTurnCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDirectedCardTargetProhibited(int actorSeat, int targetSeat, global::CardGame.Core.CardKind cardKind, long? cardUseFrameId = default);
}

internal interface ICoreNativeLegalActionsCoreNativeRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
}

internal interface ICoreNativeLegalActionsCoreDyingWindowSkillPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasBlackTrickTargetPolicy { get; }
}

internal interface ICoreNativeLegalActionsCoreHealthQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> HappyQiaoActionQueryUnion(global::System.Func<global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction>> query);
}

internal interface ICoreNativeLegalActionsCoreCurrentSlashFireOffersOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddCurrentNonFireConvertedTargetActions(global::System.Collections.Generic.ICollection<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, int? selectedPhysicalCardId = default);
}

internal interface ICoreNativeLegalActionsCoreHuxinMirrorOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.LegalAction> HuxinMirrorPlacementActions(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards);
}

internal interface ICoreNativeLegalActionsCoreBoundDiscardSlashBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasTurnAlcoholSuitAllowance(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card? material = default, global::CardGame.Core.CardConversionSource? conversion = default);
}

internal interface ICoreNativeLegalActionsCharacterCuiYanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCuiYanNextUseForbidden(int actorSeat, global::System.Collections.Generic.IReadOnlyList<int> materialIds, global::CardGame.Core.Suit? nativeEffectiveSuit);
}

internal interface ICoreNativeLegalActionsCharacterZhangHuanAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card? AnniversaryZhangHuanQueryOriginalCard(global::CardGame.Core.CharacterState actor, int? id);
    bool IsAnniversaryZhangHuanExactCardQuery(global::CardGame.Core.CharacterState actor, int? selected);
}

internal interface ICoreNativeLegalActionsCoreNativeTargetLegalityOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanUseZhangbaSerpentSpear(global::CardGame.Core.CharacterState actor);
}

internal interface ICoreNativeLegalActionsCoreUnexpectedAssaultActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddUnexpectedAssaultActions(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> playableCards, global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions);
}

internal interface ICoreNativeLegalActionsCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreNativeLegalActionsCharacterCaiWenJiLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasBeifenPreUsePrivilege(global::CardGame.Core.CharacterState actor);
}

internal interface ICoreNativeLegalActionsCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreNativeLegalActionsCoreTreatedWoundedOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsRuleWounded(global::CardGame.Core.CharacterState player);
}

internal interface ICoreNativeLegalActionsCoreAlternatingSuitAndCompletedTopOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasPhaseSuitAllowance(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreNativeLegalActionsCoreNativeMultiTargetActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CharacterState> GetFangtianOrderedSlashTargets(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card slash, global::CardGame.Core.CardConversionSource? conversionSource = default, bool ignoresSlashLimit = false, global::CardGame.Core.CardKind effectiveKind = (global::CardGame.Core.CardKind)0);
    void AddFangtianHalberdSlashActions(global::System.Collections.Generic.ICollection<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card physicalCard, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CharacterState> legalTargets, string slashName, global::CardGame.Core.CardKind? playedCardKind, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default);
    void AddTargetCardActions(global::System.Collections.Generic.ICollection<global::CardGame.Core.LegalAction> actions, int actorSeat, global::CardGame.Core.LegalActionKind actionKind, global::CardGame.Core.Card effectCard, global::CardGame.Core.CharacterState target, string effectName, global::CardGame.Core.CardKind? playedCardKind = default, global::CardGame.Core.CardConversionSource? conversionSource = default);
}

internal interface ICoreNativeLegalActionsCoreMantianGuohaiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddMantianGuohaiActions(global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IEnumerable<global::CardGame.Core.Card> playableCards);
}

internal interface ICoreNativeLegalActionsCoreNativeDamagePoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardTargetProhibited(global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind cardKind, global::CardGame.Core.Suit? suit = default, bool? effectiveIsRed = default);
}

internal interface ICoreNativeLegalActionsCharacterHuBanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsHuiyunExactHandQuery(global::CardGame.Core.CharacterState actor, int? id);
}

internal interface ICoreNativeLegalActionsCharacterZhugeLiangMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMouWujingAllowance(global::CardGame.Core.CharacterState owner);
}

internal interface ICoreNativeLegalActionsCharacterSunJunMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasShiSunJunFirstSuitAllowance(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card? material = default, global::CardGame.Core.CardConversionSource? conversion = default);
}

internal interface ICoreNativeLegalActionsCoreActualTurnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasJudgmentEffectiveCard(global::CardGame.Core.CharacterState player, global::CardGame.Core.CardKind kind);
}

internal interface ICoreNativeLegalActionsCharacterJiangJiMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouJjExactCardQuery(global::CardGame.Core.CharacterState actor, int? selected);
}

internal interface ICoreNativeLegalActionsCoreDrawFundedDistinctBasicOffersOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> DrawFundedDistinctBasicPlayActions(global::CardGame.Core.CharacterState actor);
}

internal interface ICoreNativeLegalActionsCoreOwnPublicPileConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetOwnPublicPileConversionCards(global::CardGame.Core.CharacterState owner);
    bool IsOwnPublicPileCard(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreNativeLegalActionsCharacterZhouFeiBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card? FindBoundaryZhouFeiUseCard(global::CardGame.Core.CharacterState actor, int? id);
    bool IsBoundaryZhouFeiUseQuery(global::CardGame.Core.CharacterState actor, int? selectedId);
}

internal interface ICoreNativeLegalActionsCharacterZhouYuMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouZyExactCardQuery(global::CardGame.Core.CharacterState actor, int? id);
}

internal interface ICoreNativeLegalActionsCharacterHuangQuanMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ShiQuanGushiUnlimitedCard(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> ids);
}

internal interface ICoreNativeLegalActionsCorePlayCardHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card ApplyProgramUseAppearance(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardConversionSource? source);
    global::CardGame.Core.SkillProgramViewAs? ViewAsRule(global::CardGame.Core.CardConversionSource source);
    global::CardGame.Core.Suit? PreviousPlaySuitForInheritance(int actorSeat);
}

internal interface ICoreNativeLegalActionsCharacterFuRongOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsFuRongDiscardQuery(global::CardGame.Core.CharacterState actor, int? id);
    bool HasFuRongDiscardDistance(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int>? ids);
}

internal interface ICoreNativeLegalActionsCoreJudgedRankSlashPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.TurnJudgedRankSlashPolicy? CurrentJudgedRankSlashPolicy(int actor);
}

internal interface ICoreNativeLegalActionsCoreAdvancedPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasNextUnlimitedCard(global::CardGame.Core.CharacterState owner);
    bool HasFriendQihuiAllowance(global::CardGame.Core.CharacterState owner);
}

internal interface ICoreNativeLegalActionsCoreWushuangSkillsBOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWushuangBianyuPotential(global::CardGame.Core.CharacterState owner);
}

internal interface ICoreNativeLegalActionsCoreRoundOnceCardlessNativeUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> RoundOnceCardlessPlayActions(global::CardGame.Core.CharacterState actor);
}

internal interface ICoreNativeLegalActionsCharacterLuSuMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouLuSuUseQuery(global::CardGame.Core.CharacterState actor);
}
