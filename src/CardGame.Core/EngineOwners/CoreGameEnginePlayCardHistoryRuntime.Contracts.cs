#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICorePlayCardHistoryCoreCardUseModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsTurnHandCardRestricted(global::CardGame.Core.CharacterState player, global::CardGame.Core.Card card);
}

internal interface ICorePlayCardHistoryCoreProgramCardUseRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddProgramTargetCountSlashActions(global::System.Collections.Generic.ICollection<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card physicalCard, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CharacterState> legalTargets, string slashName, global::CardGame.Core.CardKind? playedCardKind, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default);
}

internal interface ICorePlayCardHistorySkillRuntimeStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.SkillRuntimeStateStore _skillRuntimeState { get; }
}

internal interface ICorePlayCardHistoryCoreCardConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    string DescribeConversion(global::CardGame.Core.CardConversionSource? source, string description);
}

internal interface ICorePlayCardHistoryCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default);
}

internal interface ICorePlayCardHistoryContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICorePlayCardHistoryCoreProgramTieredRoundConversionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ConsumeTieredRoundConversion(global::CardGame.Core.CardConversionSource source);
}

internal interface ICorePlayCardHistoryCoreRoundGainedSourceUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRoundGainedBasicBonus(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> ids);
}

internal interface ICorePlayCardHistoryCoreTurnTargetCardQuotaAllowancesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasTargetCardQuotaAllowance(int actor, int target);
}

internal interface ICorePlayCardHistoryCoreNativeCardAppearanceOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICorePlayCardHistoryCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player);
}

internal interface ICorePlayCardHistoryCoreTreatedWoundedOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsRuleWounded(global::CardGame.Core.CharacterState player);
}

internal interface ICorePlayCardHistoryCoreNativeMultiTargetActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CharacterState> GetFangtianOrderedSlashTargets(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card slash, global::CardGame.Core.CardConversionSource? conversionSource = default, bool ignoresSlashLimit = false, global::CardGame.Core.CardKind effectiveKind = (global::CardGame.Core.CardKind)0);
    void AddFangtianHalberdSlashActions(global::System.Collections.Generic.ICollection<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card physicalCard, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CharacterState> legalTargets, string slashName, global::CardGame.Core.CardKind? playedCardKind, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default);
}

internal interface ICorePlayCardHistoryKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICorePlayCardHistoryCharacterSunQuanAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsAnniversaryChouxiSource(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardConversionSource? source);
}

internal interface ICorePlayCardHistoryCoreGuanduEventProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ConsumeGuanduRoundConversion(global::CardGame.Core.CardConversionSource source);
}

internal interface ICorePlayCardHistoryCharacterMiHengLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanUseSiruiPhase(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardConversionSource source);
    bool IsSiruiSource(global::CardGame.Core.CardConversionSource? source);
    bool SiruiIgnoresAlcoholLimit(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardConversionSource? source);
}

internal interface ICorePlayCardHistoryCoreActualTurnViewAsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ConsumeActualTurnLimitedViewAs(global::CardGame.Core.CardConversionSource source);
}

internal interface ICorePlayCardHistoryCharacterZhangFeiAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsShenZfIdentitySource(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardConversionSource? source);
}

internal interface ICorePlayCardHistoryCoreActivityOrdinaryTricksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgram ActivityRuntimeProgram(global::CardGame.Core.SkillProgram original);
}

internal interface ICorePlayCardHistoryCharacterGuanHaiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsXieweiSource(global::CardGame.Core.CardConversionSource source);
}

internal interface ICorePlayCardHistoryCoreBoundDiscardSlashBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasTurnAlcoholSuitAllowance(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card? material = default, global::CardGame.Core.CardConversionSource? conversion = default);
}

internal interface ICorePlayCardHistoryCoreWushuangJinzhiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWushuangJinzhiSource(global::CardGame.Core.CardConversionSource source);
}

internal interface ICorePlayCardHistoryCoreActionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource> GetProgramViewAsConversions(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind outputKind, bool forResponse, bool dyingUse = false);
}

internal interface ICorePlayCardHistoryCharacterGuanYuWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card ApplyJuewuOwnedAppearance(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICorePlayCardHistoryCharacterHuangfuSongWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWuHfOrDixianMaterialPrivilege(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int>? ids, global::CardGame.Core.CardConversionSource? source = default);
}

internal interface ICorePlayCardHistoryCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICorePlayCardHistoryCharacterWeiYanMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMobileShiZhuangshiQuota(int seat);
}

internal interface ICorePlayCardHistoryCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player);
}

internal interface ICorePlayCardHistoryCoreNamedUseConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ConsumeNamedUseConversion(global::CardGame.Core.CardConversionSource source);
}

internal interface ICorePlayCardHistoryCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramEventHistoryIndex ProgramHistory { get; }
}
