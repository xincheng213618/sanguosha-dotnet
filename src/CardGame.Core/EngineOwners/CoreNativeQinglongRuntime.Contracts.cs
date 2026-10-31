#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreNativeQinglongCoreCardConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.PromptChoice> CreateConversionChoiceVariants(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind effectiveKind, bool forResponse, string baseChoiceId, string description, global::System.Collections.Generic.IReadOnlyList<int> cards, global::System.Collections.Generic.IReadOnlyList<int> targets, global::System.Collections.Generic.IReadOnlyDictionary<string, string> baseParameters, bool dyingUse = false);
    void CaptureSelectedResponseConversion(global::CardGame.Core.PromptChoice choice);
}

internal interface ICoreNativeQinglongCoreRoundOnceCardlessNativeUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> RoundOnceCardlessForcedSlashChoices(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, long originalId, bool qinglong);
    bool TryResolveRoundOnceCardlessForcedSlash(global::CardGame.Core.PromptChoice selected, bool advance);
}

internal interface ICoreNativeQinglongCharacterHuanShuJunOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasHuanpeiForcedSlash(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle pending);
}

internal interface ICoreNativeQinglongCharacterMiHengLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SlashUseVariant> SiruiPhysicalSlashVariants(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card);
    bool HasSiruiForcedSlash(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle pending);
    global::CardGame.Core.CardKind SiruiSelectedSlashKind(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.CardKind fallback, global::CardGame.Core.CardConversionSource? explicitSource = default);
}

internal interface ICoreNativeQinglongCoreActionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetSlashUseCards(global::CardGame.Core.CharacterState owner, bool ignoreSuitUseProhibition = false);
}

internal interface ICoreNativeQinglongCoreCardUseModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardUseForbidden(int actorSeat, global::CardGame.Core.CardKind effectiveKind, global::CardGame.Core.CardActionType actionType, bool ignoreIssuedPlayBan = false, long? existingUseFrameId = default);
}

internal interface ICoreNativeQinglongCharacterLiuFengBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasBoundaryLiuFengForcedSlash(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle pending);
}

internal interface ICoreNativeQinglongCoreNativeAttackCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreNativeQinglongCoreProgramFactionPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.FactionResponsePolicySource? GetFactionResponsePolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind requiredKind);
    global::System.Collections.Generic.IReadOnlyList<int> GetFactionSlashCandidateSeats(int ownerSeat);
}

internal interface ICoreNativeQinglongCoreNativeEquipmentInvocationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginQinglongEquipmentInvocation(global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle pending, global::CardGame.Core.Card slash, global::CardGame.Core.CardKind effectiveSlashKind, int physicalOwnerSeat, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> physicalCards, global::CardGame.Core.NativeOwners.FactionCardRequestHandle? jijiang = default, global::CardGame.Core.CardConversionSource? conversionSource = default);
}

internal interface ICoreNativeQinglongCharacterGuoTuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWeijieForcedSlash(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle pending);
}

internal interface ICoreNativeQinglongCoreOwnPublicPileConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardKind PhysicalSlashUseBaseKind(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreNativeQinglongCoreTieredRoundZeroUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.CardConversionSource Source, global::CardGame.Core.SkillProgramViewAs Rule, string Hash)> TieredRoundZeroForcedSlashRules(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, bool qinglong);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> TieredRoundZeroForcedSlashChoices(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, long originalId, bool qinglong);
    bool TryResolveTieredRoundZeroForcedSlash(global::CardGame.Core.PromptChoice selected, bool advance);
}

internal interface ICoreNativeQinglongCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.FactionCardRequestHandle? ActiveFactionCardRequest { get; set; }

    global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle? ActiveQinglongCrescentBlade { get; set; }
}

internal interface ICoreNativeQinglongCoreNativeCardUseFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetCardUseStep(long frameId, global::CardGame.Core.ResolutionFrameStep step);
}

internal interface ICoreNativeQinglongCoreProgramCardUseRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SlashUseVariant> GetSlashUseVariants(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind baseEffectiveKind);
}

internal interface ICoreNativeQinglongCharacterMaXiuMaTieOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> KenshangQinglongSlashChoices(global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle pending);
    bool TryBeginKenshangQinglongSlash(global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle pending, global::CardGame.Core.PromptChoice selected);
}

internal interface ICoreNativeQinglongCoreActiveNativeAttackQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get; set; }
}

internal interface ICoreNativeQinglongCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreNativeQinglongCoreNativeFactionSlashOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginQinglongCrescentBladeFactionSlashRequest(global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle qinglong);
}

internal interface ICoreNativeQinglongCharacterChengJiChengCuiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> TousuiQinglongSlashChoices(global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle pending);
    bool TryBeginTousuiQinglongSlash(global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle pending, global::CardGame.Core.PromptChoice selected);
}

internal interface ICoreNativeQinglongCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICoreNativeQinglongCoreNativeDamagePoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSlashProhibited(global::CardGame.Core.CharacterState target);
}

internal interface ICoreNativeQinglongCoreFinalTargetSlashOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int? SpecificSlashRank(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, long? useId = default);
}

internal interface ICoreNativeQinglongCoreNativeSlashUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveSlashCore(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card slash, global::CardGame.Core.CardKind playedCardKind, int physicalOwnerSeat, global::CardGame.Core.NativeOwners.FactionCardRequestHandle? factionRequest = default, global::CardGame.Core.NativeOwners.BorrowedSwordHandle? borrowedSword = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? physicalCards = default, bool countsTowardSlashLimit = true, bool usesZhuqueFan = false, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default, long? programSkillCardUseFrameId = default, global::CardGame.Core.CardLocation? physicalSourceLocation = default);
}

internal interface ICoreNativeQinglongCoreRequestedDeckBasicsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRequestedDeckBasicSource(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind required);
}

internal interface ICoreNativeQinglongCharacterCaoZhiLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SlashUseVariant> FulePhysicalSlashVariants(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
    global::CardGame.Core.CardKind FuleSelectedSlashKind(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind fallback, global::CardGame.Core.CardConversionSource? explicitSource = default);
}

internal interface ICoreNativeQinglongKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreNativeQinglongCharacterLiuZhangOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasZonghuForcedSlash(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle pending);
}

internal interface ICoreNativeQinglongCoreProgramCardLegalityOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramCardLegality CardLegality { get; }
}

internal interface ICoreNativeQinglongCoreChainedStateBasicConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.CardConversionSource Source, global::CardGame.Core.SkillProgramViewAs Rule, string Hash)> ChainedStateBasicForcedSlashRules(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, bool qinglong);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ChainedStateBasicForcedSlashChoices(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, long originalId, bool qinglong);
}

internal interface ICoreNativeQinglongCoreAdvancedWeaponsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWeaponAbility(global::CardGame.Core.CharacterState player, global::CardGame.Core.CardKind kind);
}

internal interface ICoreNativeQinglongCoreNativeCardAppearanceOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreNativeQinglongCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
    void ClearPendingDecision();
}

internal interface ICoreNativeQinglongCoreDirectedTurnCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDirectedCardTargetProhibited(int actorSeat, int targetSeat, global::CardGame.Core.CardKind cardKind, long? cardUseFrameId = default);
}

internal interface ICoreNativeQinglongKernelCoreMainOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle CreateQinglongCrescentBladeHandle(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreNativeQinglongCoreQinglongInsertionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SuspendFactionRequestForQinglongChoice(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
    void RestoreFactionRequestAfterQinglongChoice(long ownerId);
    void SuspendContinuationForQinglongFollowup(long outerResolutionId, global::CardGame.Core.NativeOwners.FactionCardRequestHandle? followupRequest = default);
}

internal interface ICoreNativeQinglongCoreNativeDamageHandlesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool UsesFormalQinglongCrescentBlade { get; }
}

internal interface ICoreNativeQinglongCharacterGuanYuWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SlashUseVariant> JuewuPhysicalSlashVariants(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
    global::CardGame.Core.CardKind JuewuSelectedSlashKind(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind fallback, global::CardGame.Core.CardConversionSource? explicitSource = default);
}

internal interface ICoreNativeQinglongCoreDrawFundedDistinctBasicOffersOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.CardConversionSource Source, global::CardGame.Core.SkillProgramViewAs Rule, string Hash)> DrawFundedDistinctBasicForcedSlashRules(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, bool qinglong);
}

internal interface ICoreNativeQinglongCharacterXiaoQiaoMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasProspectiveMouXqQinglong(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.NativeOwners.CardAttackHandle attack);
    bool BeginMouXqForcedSlash(global::CardGame.Core.PromptChoice choice);
}
