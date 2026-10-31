#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreNativeBorrowedSwordCoreAdvancedWeaponsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MoveProcessingCardUnlessDestroyed(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation destination, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default);
}

internal interface ICoreNativeBorrowedSwordCharacterCaoZhiLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SlashUseVariant> FulePhysicalSlashVariants(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
    bool IsFulePhysicalSlashKind(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind);
    global::CardGame.Core.CardKind FuleSelectedSlashKind(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind fallback, global::CardGame.Core.CardConversionSource? explicitSource = default);
}

internal interface ICoreNativeBorrowedSwordCharacterHuangfuSongWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWuHfOrWuyouMaterialPrivilege(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int>? ids, long? useId = default);
}

internal interface ICoreNativeBorrowedSwordCoreNativeResponseMaterialQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>> GetZhangbaSlashPairs(global::CardGame.Core.CharacterState responder);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? FindZhangbaSlashPair(global::CardGame.Core.CharacterState responder, global::System.Collections.Generic.IReadOnlyList<int> requestedCardIds);
}

internal interface ICoreNativeBorrowedSwordCoreNativeResponseWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PushResponseWindow(long parentFrameId, int sourceSeat, int responderSeat, global::CardGame.Core.CardKind incomingCard, global::CardGame.Core.CardKind requiredCardKind);
    void PopResponseWindow(long parentFrameId);
}

internal interface ICoreNativeBorrowedSwordCharacterGuoTuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWeijieForcedSlash(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending);
}

internal interface ICoreNativeBorrowedSwordCharacterHuCheErMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> KongwuEffectiveEquipment(global::CardGame.Core.CharacterState player);
}

internal interface ICoreNativeBorrowedSwordCharacterZhangLiaoHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HappyZlCannotRespond(long useId, int responderSeat);
}

internal interface ICoreNativeBorrowedSwordCoreProgramDistanceRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramDistanceRules DistanceRules { get; }
}

internal interface ICoreNativeBorrowedSwordCorePhysicalMovementFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
}

internal interface ICoreNativeBorrowedSwordCoreDirectedTurnCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDirectedCardTargetProhibited(int actorSeat, int targetSeat, global::CardGame.Core.CardKind cardKind, long? cardUseFrameId = default);
}

internal interface ICoreNativeBorrowedSwordCoreDirectedCardPolicyQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardDistanceExemption(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind kind, long? cardUseFrameId = default, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default);
}

internal interface ICoreNativeBorrowedSwordCoreNativeTargetLegalityOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardKind> GetSlashUseKinds(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind baseEffectiveKind);
    bool IsZhuqueFanConversion(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind baseEffectiveKind, global::CardGame.Core.CardKind requestedEffectiveKind);
}

internal interface ICoreNativeBorrowedSwordCoreNativeCardUseCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinishCardUse(long frameId, global::CardGame.Core.Card card, global::CardGame.Core.CardKind? playedCardKind = default);
}

internal interface ICoreNativeBorrowedSwordCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.BorrowedSwordHandle? ActiveBorrowedSword { get; set; }

    global::CardGame.Core.NativeOwners.FactionCardRequestHandle? ActiveFactionCardRequest { get; set; }
}

internal interface ICoreNativeBorrowedSwordCoreRequestedDeckBasicsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRequestedDeckBasicSource(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind required);
}

internal interface ICoreNativeBorrowedSwordCoreOwnPublicPileConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardKind PhysicalSlashUseBaseKind(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
    bool IsOwnPublicPileSlashMaterial(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreNativeBorrowedSwordCharacterLiuZhangOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasZonghuForcedSlash(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending);
}

internal interface ICoreNativeBorrowedSwordCoreNativeCardAppearanceOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreNativeBorrowedSwordResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreNativeBorrowedSwordCoreNextCardTargetAdjustmentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRemainingAdjustedBorrowedSwordTargets(long frameId);
}

internal interface ICoreNativeBorrowedSwordCoreProgramFactionPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.FactionResponsePolicySource? GetFactionResponsePolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind requiredKind);
    global::System.Collections.Generic.IReadOnlyList<int> GetFactionSlashCandidateSeats(int ownerSeat);
}

internal interface ICoreNativeBorrowedSwordCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player);
}

internal interface ICoreNativeBorrowedSwordCoreDrawFundedDistinctBasicOffersOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.CardConversionSource Source, global::CardGame.Core.SkillProgramViewAs Rule, string Hash)> DrawFundedDistinctBasicForcedSlashRules(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, bool qinglong);
}

internal interface ICoreNativeBorrowedSwordCoreChainedStateBasicConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.CardConversionSource Source, global::CardGame.Core.SkillProgramViewAs Rule, string Hash)> ChainedStateBasicForcedSlashRules(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, bool qinglong);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ChainedStateBasicForcedSlashChoices(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, long originalId, bool qinglong);
}

internal interface ICoreNativeBorrowedSwordCoreFinalTargetSlashOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int? SpecificSlashRank(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, long? useId = default);
    int? ZhangbaSpecificSlashRank(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CharacterState? provider = default);
    bool HasRankSlashRange(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind);
    bool IsWithinSpecificSlashRange(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind kind, int? rank, long? existingUseFrameId = default, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default);
}

internal interface ICoreNativeBorrowedSwordCharacterXiaoQiaoMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MouXqForcedSlashChoices(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, long parentId, bool qinglong);
}

internal interface ICoreNativeBorrowedSwordCoreGuanHaiBridgesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit? ProgramMultiCardSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramMultiCardViewAsSelection selection);
    bool? ProgramMultiCardColor(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramMultiCardViewAsSelection selection);
}

internal interface ICoreNativeBorrowedSwordCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
    void ClearPendingDecision();
}

internal interface ICoreNativeBorrowedSwordCoreNativeNullificationFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetCardUseTargetIndex(long frameId, int targetIndex);
}

internal interface ICoreNativeBorrowedSwordCharacterLuBuOlMagicOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MoGangquanBorrowedDistance(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int>? ids, global::CardGame.Core.CardConversionSource? source = default);
}

internal interface ICoreNativeBorrowedSwordCoreIssuedPlayUsePoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsIssuedCardUnrespondable(long frameId);
}

internal interface ICoreNativeBorrowedSwordCharacterHuangQuanMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Action<global::CardGame.Core.CardMovementRecord> NativeGiveCompletion(long producer, int giver, int recipient, global::System.Collections.Generic.IReadOnlyList<int> ids, long before, global::System.Action<global::CardGame.Core.CardMovementRecord>? original = default);
}

internal interface ICoreNativeBorrowedSwordCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreNativeBorrowedSwordCoreRoundGainedSourceUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRoundGainedBasicBonus(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> ids);
}

internal interface ICoreNativeBorrowedSwordCharacterChengJiChengCuiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> TousuiBorrowedSlashChoices(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending);
}

internal interface ICoreNativeBorrowedSwordCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICoreNativeBorrowedSwordCharacterLiuFengBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasBoundaryLiuFengForcedSlash(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending);
}

internal interface ICoreNativeBorrowedSwordCharacterHuanShuJunOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasHuanpeiForcedSlash(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending);
}

internal interface ICoreNativeBorrowedSwordCoreCardConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramMultiCardViewAsSelection> GetProgramMultiCardViewAsSelections(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind outputKind, bool forResponse, bool ignoreSuitUseProhibition = false, int? requestedUseTarget = default);
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.PromptChoice> CreateConversionChoiceVariants(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind effectiveKind, bool forResponse, string baseChoiceId, string description, global::System.Collections.Generic.IReadOnlyList<int> cards, global::System.Collections.Generic.IReadOnlyList<int> targets, global::System.Collections.Generic.IReadOnlyDictionary<string, string> baseParameters, bool dyingUse = false);
    string ProgramConversionName(global::CardGame.Core.CardConversionSource source);
    global::CardGame.Core.ProgramMultiCardViewAsSelection? FindProgramMultiCardViewAsSelection(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int> cardIds, global::CardGame.Core.CardKind outputKind, bool forResponse, global::CardGame.Core.CardConversionSource source);
    void ResolveProgramMultiCardSlash(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.ProgramMultiCardViewAsSelection selection, global::CardGame.Core.NativeOwners.BorrowedSwordHandle? borrowedSword = default, bool enforceOwnTurnSlashLimit = true);
}

internal interface ICoreNativeBorrowedSwordCoreActionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetSlashUseCards(global::CardGame.Core.CharacterState owner, bool ignoreSuitUseProhibition = false);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource> GetProgramViewAsConversions(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind outputKind, bool forResponse, bool dyingUse = false);
}

internal interface ICoreNativeBorrowedSwordCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreNativeBorrowedSwordCoreNativeSlashUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveSlashCore(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card slash, global::CardGame.Core.CardKind playedCardKind, int physicalOwnerSeat, global::CardGame.Core.NativeOwners.FactionCardRequestHandle? factionRequest = default, global::CardGame.Core.NativeOwners.BorrowedSwordHandle? borrowedSword = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? physicalCards = default, bool countsTowardSlashLimit = true, bool usesZhuqueFan = false, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default, long? programSkillCardUseFrameId = default, global::CardGame.Core.CardLocation? physicalSourceLocation = default);
}

internal interface ICoreNativeBorrowedSwordKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreNativeBorrowedSwordCoreTieredRoundZeroUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> TieredRoundZeroForcedSlashChoices(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, long originalId, bool qinglong);
}

internal interface ICoreNativeBorrowedSwordCharacterBianFuRenOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordBianFuRenOwnedTransferProducer(global::CardGame.Core.CardMovementRecord movement, int recipientSeat, long producerId);
}

internal interface ICoreNativeBorrowedSwordCharacterGuanYuWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SlashUseVariant> JuewuPhysicalSlashVariants(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
    global::CardGame.Core.CardKind JuewuSelectedSlashKind(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind fallback, global::CardGame.Core.CardConversionSource? explicitSource = default);
}

internal interface ICoreNativeBorrowedSwordCharacterZhugeGuoLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool FinishLeChengyanBorrowedNative(global::CardGame.Core.NativeOwners.BorrowedSwordHandle borrowed);
}

internal interface ICoreNativeBorrowedSwordCoreRoundOnceCardlessNativeUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> RoundOnceCardlessForcedSlashChoices(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, long originalId, bool qinglong);
}

internal interface ICoreNativeBorrowedSwordCoreCardDeclarationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCardDeclaration(global::CardGame.Core.CharacterState provider, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardConversionSource? source, global::CardGame.Core.CardDeclarationParentReturn parent, global::System.Collections.Generic.IReadOnlyList<int> targets);
    global::CardGame.Core.CardConversionSource? PeekDeclarationConversion(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, bool use, global::CardGame.Core.CardConversionSource? explicitSource = default);
    global::CardGame.Core.CardDeclarationParentReturn DeclarationReturn(global::CardGame.Core.CardDeclarationPurpose purpose, int actor, global::CardGame.Core.LegalAction? action = default, int? target = default, int recovery = 1, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramRecoveryPolicySource>? policies = default, global::CardGame.Core.DyingResponseEvent? dying = default, bool fan = false, global::CardGame.Core.CardKind? finalKind = default);
}

internal interface ICoreNativeBorrowedSwordCoreProgramTargetRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramTargetRules TargetRules { get; }
}

internal interface ICoreNativeBorrowedSwordCoreTurnRedSlashBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool? PhysicalGroupColor(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards);
    bool HasTurnRedSlashPolicy(int actor, global::CardGame.Core.CardKind kind, global::CardGame.Core.Suit? suit);
}

internal interface ICoreNativeBorrowedSwordCharacterMiHengLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SlashUseVariant> SiruiPhysicalSlashVariants(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card);
    bool IsSiruiSource(global::CardGame.Core.CardConversionSource? source);
    bool IsSiruiPhysicalSlashKind(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind);
    bool SiruiBorrowedChoiceAllowed(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card card, global::CardGame.Core.PromptChoice choice);
    global::CardGame.Core.CardKind SiruiSelectedSlashKind(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.CardKind fallback, global::CardGame.Core.CardConversionSource? explicitSource = default);
    bool HasSiruiSlashDistance(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int>? materialIds = default, global::CardGame.Core.CardConversionSource? conversionSource = default, long? existingUseFrameId = default);
}

internal interface ICoreNativeBorrowedSwordCoreCurrentTurnShownCardBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasSlashUseDistanceBySuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind kind, global::CardGame.Core.Suit? suit);
}

internal interface ICoreNativeBorrowedSwordCoreProgramCardLegalityOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramCardLegality CardLegality { get; }
}

internal interface ICoreNativeBorrowedSwordCharacterSimaYiMobileGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginMobileSimaRenjieBorrowedSword(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending);
}

internal interface ICoreNativeBorrowedSwordCharacterMaXiuMaTieOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> KenshangBorrowedSlashChoices(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending);
}

internal interface ICoreNativeBorrowedSwordCoreNativeDamagePoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSlashProhibited(global::CardGame.Core.CharacterState target);
}

internal interface ICoreNativeBorrowedSwordCoreProgramCardUseRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SlashUseVariant> GetSlashUseVariants(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind baseEffectiveKind);
}

internal interface ICoreNativeBorrowedSwordCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default);
}

internal interface ICoreNativeBorrowedSwordCoreNativeCardUseFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetCardUseStep(long frameId, global::CardGame.Core.ResolutionFrameStep step);
}

internal interface ICoreNativeBorrowedSwordCoreFieldEquipmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.EquipmentSlot FieldEquipmentSlot(global::CardGame.Core.Card card);
}

internal interface ICoreNativeBorrowedSwordCoreSequentialTrickTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveFinishedTrickCard(long frameId, global::CardGame.Core.Card card);
}

internal interface ICoreNativeBorrowedSwordKernelCoreMainOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteGame();
}

internal interface ICoreNativeBorrowedSwordCoreAlternatingSuitAndCompletedTopOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit? PhysicalGroupSuit(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards);
    bool HasPhaseSuitAllowance(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreNativeBorrowedSwordCoreCardUseModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardUseForbidden(int actorSeat, global::CardGame.Core.CardKind effectiveKind, global::CardGame.Core.CardActionType actionType, bool ignoreIssuedPlayBan = false, long? existingUseFrameId = default);
}

internal interface ICoreNativeBorrowedSwordCoreDeckEquipmentProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasBeneficiarySuitShield(int actor, int target, global::CardGame.Core.Suit? suit);
}
