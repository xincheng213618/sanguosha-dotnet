#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreAssistedFactionSlashCoreDirectedTurnCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDirectedCardTargetProhibited(int actorSeat, int targetSeat, global::CardGame.Core.CardKind cardKind, long? cardUseFrameId = default);
}

internal interface ICoreAssistedFactionSlashCharacterPangTongHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsHappyPangTongFactionOrigin(global::CardGame.Core.ProgramSkillFrame frame, int actor, int target, string? bind);
    bool ValidateHappyPangTongFactionParent(global::CardGame.Core.NativeOwners.FactionCardRequestHandle request, global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreAssistedFactionSlashCoreCardConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasProgramCardIdentity(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource> GetProgramCardIdentitySources(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind effectiveKind, bool forResponse);
    string ProgramConversionName(global::CardGame.Core.CardConversionSource source);
    void CaptureSelectedResponseConversion(global::CardGame.Core.PromptChoice choice);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramMultiCardViewAsSelection> GetProgramMultiCardViewAsSelections(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind outputKind, bool forResponse, bool ignoreSuitUseProhibition = false, int? requestedUseTarget = default);
}

internal interface ICoreAssistedFactionSlashCoreNativeTargetLegalityOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasZhuqueFan(global::CardGame.Core.CharacterState actor);
}

internal interface ICoreAssistedFactionSlashCoreProgramFactionPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.FactionResponsePolicySource? GetFactionResponsePolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind requiredKind);
}

internal interface ICoreAssistedFactionSlashCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetPlayableCards(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player);
}

internal interface ICoreAssistedFactionSlashCoreLastZoneConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetLastZoneJudgmentConversionCards(global::CardGame.Core.CharacterState owner, bool forResponse);
    bool IsLastZoneJudgmentCard(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreAssistedFactionSlashCoreCardUseModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsTurnHandCardRestricted(global::CardGame.Core.CharacterState player, global::CardGame.Core.Card card);
    bool IsCardUseForbidden(int actorSeat, global::CardGame.Core.CardKind effectiveKind, global::CardGame.Core.CardActionType actionType, bool ignoreIssuedPlayBan = false, long? existingUseFrameId = default);
}

internal interface ICoreAssistedFactionSlashCharacterCaiWenJiLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasBeifenPreUsePrivilege(global::CardGame.Core.CharacterState actor);
}

internal interface ICoreAssistedFactionSlashCharacterWuJingOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWuJingFactionDistance(int actor, int target);
    bool ValidateWuJingFactionParent(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreAssistedFactionSlashCoreNativeResponseMaterialQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? FindZhangbaSlashPair(global::CardGame.Core.CharacterState responder, global::System.Collections.Generic.IReadOnlyList<int> requestedCardIds);
    global::System.Collections.Generic.IReadOnlyList<global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>> GetZhangbaSlashPairs(global::CardGame.Core.CharacterState responder);
}

internal interface ICoreAssistedFactionSlashCharacterHuangfuSongWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWuHfOrWuyouMaterialPrivilege(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int>? ids, long? useId = default);
}

internal interface ICoreAssistedFactionSlashCharacterSunYuanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSayingFactionOrigin(global::CardGame.Core.ProgramSkillFrame f, int actor, int target, string resultBind);
    bool ValidateSayingFactionSlashParent(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreAssistedFactionSlashKernelCoreMainOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.FactionCardRequestHandle CreateFactionCardRequestHandle(long resolutionId, global::CardGame.Core.FactionCardRequestPurpose purpose, int ownerSeat, global::System.Collections.Generic.IReadOnlyList<int> candidateSeats, string providerFactionId, global::CardGame.Core.NativeOwners.CardAttackHandle? responseAttack = default, int? targetSeat = default, long? programSkillFrameId = default, global::CardGame.Core.CardKind requiredKind = (global::CardGame.Core.CardKind)0, global::CardGame.Core.NativeOwners.BorrowedSwordHandle? borrowedSword = default, global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle? qinglongCrescentBlade = default, global::CardGame.Core.FactionResponsePolicySource? policySource = default, string? assistedResultBind = default);
}

internal interface ICoreAssistedFactionSlashCoreActionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource> GetProgramViewAsConversions(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind outputKind, bool forResponse, bool dyingUse = false);
}

internal interface ICoreAssistedFactionSlashCoreFixedSlashAndDeclaredDeckProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsFixedTargetFactionSlashLegal(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.CardKind kind, int? effectiveRank = default, bool allowPotentialRank = true);
    bool ValidateFixedTargetFactionSlashParent(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreAssistedFactionSlashCharacterHuangGaiMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMouZhaJiangPreUsePrivilege(global::CardGame.Core.CharacterState actor);
}

internal interface ICoreAssistedFactionSlashCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.FactionCardRequestHandle? ActiveFactionCardRequest { get; set; }
}

internal interface ICoreAssistedFactionSlashKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceRulesAndPublishState();
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreAssistedFactionSlashCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreAssistedFactionSlashCharacterCaoChunOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidateCaoChunFactionSlashParent(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreAssistedFactionSlashCorePlayCardHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramViewAs? ViewAsRule(global::CardGame.Core.CardConversionSource source);
}

internal interface ICoreAssistedFactionSlashCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreAssistedFactionSlashCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ClearPendingDecision();
}

internal interface ICoreAssistedFactionSlashCharacterMaTieAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool AnniversaryMaTieFactionSupplyLegal(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.CardKind kind);
    bool ValidateAnniversaryMaTieFactionParent(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreAssistedFactionSlashCoreNativeFactionSlashOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    string GetFactionRequestSkillId(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending);
    string GetFactionRequestDisplayName(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending);
    void ResolveFactionSlashCandidateResponse(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, bool useSlash, int? requestedCardId, global::CardGame.Core.CardKind? requestedEffectiveKind = default);
    void ResolveFactionSlashZhangbaCandidateResponse(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.CharacterState provider, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> pair);
    bool TryBeginFactionSlashZhuqueFanChoice(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.CharacterState provider, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> physicalCards, global::CardGame.Core.CardKind baseEffectiveKind);
    void BeginProvidedFactionSlashSlash(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.CharacterState provider, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> physicalCards, global::CardGame.Core.CardKind effectiveKind, bool usesZhuqueFan, global::CardGame.Core.CardConversionSource? conversionSource = default);
    void AdvanceFactionSlashCandidate(bool keepCurrentRequest = false);
}

internal interface ICoreAssistedFactionSlashCoreTurnRedSlashBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsRedSlashProviderPaymentLegal(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, bool isTrueZhangba = false);
}

internal interface ICoreAssistedFactionSlashCoreCompletedCardGiftsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int[] CompletedGiftSlashTargets(global::CardGame.Core.ProgramSkillFrame frame, int actorSeat);
}

internal interface ICoreAssistedFactionSlashCoreNativeEquipmentAndFactionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> GetFactionProviderSeats(int ownerSeat, string factionId);
}

internal interface ICoreAssistedFactionSlashCharacterJiangWeiMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MouJwFactionSupplyLegal(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.CardKind kind);
    bool HasMouJwFactionDistance(int actor, int target);
    bool ValidateMouJwFactionParent(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreAssistedFactionSlashCoreNearestLegalSlashRequestsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsNearestLegalFactionOrigin(global::CardGame.Core.ProgramSkillFrame frame, int actor, int target, string? bind);
    bool ValidateNearestLegalFactionParent(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreAssistedFactionSlashCoreProgramParticipantPhaseOperationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsTurnPhysicalUseForbidden(int seat, global::System.Collections.Generic.IReadOnlyList<int> ids);
}

internal interface ICoreAssistedFactionSlashCoreCuiYanNativeQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanSupplyCuiYanFactionSlashVariant(global::CardGame.Core.NativeOwners.FactionCardRequestHandle request, global::CardGame.Core.CharacterState provider, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> materials, global::CardGame.Core.CardConversionSource? source = default);
    bool IsCuiYanFactionSlashUseForbidden(global::CardGame.Core.NativeOwners.FactionCardRequestHandle request, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> materials, global::CardGame.Core.CardConversionSource? conversion = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversions = default);
}

internal interface ICoreAssistedFactionSlashContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreAssistedFactionSlashCharacterXiaoQiaoLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWeiwanUseDistance(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind kind, long? useId = default);
}

internal interface ICoreAssistedFactionSlashCharacterLiYiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsChanshuangFactionOrigin(global::CardGame.Core.ProgramSkillFrame f, int actor, int target, string resultBind);
    bool ValidateChanshuangFactionSlashParent(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreAssistedFactionSlashResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreAssistedFactionSlashCoreRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanSpendSlashUse(global::CardGame.Core.CharacterState player, global::CardGame.Core.CharacterState target, bool ignoresCount, global::CardGame.Core.CardKind effectiveKind = (global::CardGame.Core.CardKind)0, global::CardGame.Core.Card? physicalCard = default, int? effectiveRank = default, int? excludedEquipmentId = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default, bool ignoreMoQuanyu = false, global::CardGame.Core.Suit? effectiveSuit = default);
}

internal interface ICoreAssistedFactionSlashCoreFinalTargetSlashOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRankSlashRange(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind);
    bool IsWithinSpecificSlashRange(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind kind, int? rank, long? existingUseFrameId = default, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default);
    bool HasPotentialRankSlashRange(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind kind);
}

internal interface ICoreAssistedFactionSlashCharacterLvKuangLvXiangOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool QigongAdapterActor(global::CardGame.Core.ProgramSkillFrame f, int actor, int? target, int cursor, long? parent, long? request);
    bool ValidateQigongFactionSlashParent(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreAssistedFactionSlashCoreNativeDamagePoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSlashProhibited(global::CardGame.Core.CharacterState target);
}

internal interface ICoreAssistedFactionSlashCoreOwnPublicPileConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetOwnPublicPileConversionCards(global::CardGame.Core.CharacterState owner);
    bool IsOwnPublicPileCard(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
    bool IsOwnPublicPileSlashMaterial(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreAssistedFactionSlashKernelCoreGameEngineRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceToHumanBoundary();
}

internal interface ICoreAssistedFactionSlashCharacterGuanYuWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWuyouIdentityKind(global::CardGame.Core.CharacterState holder, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind);
    bool HasPotentialWuyouFactionSlash(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind);
}
