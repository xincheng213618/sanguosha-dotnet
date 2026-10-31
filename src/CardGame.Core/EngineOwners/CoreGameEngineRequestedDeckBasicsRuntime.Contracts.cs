#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreRequestedDeckBasicsCharacterCaoChunOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCaoChunInitialRequest(global::CardGame.Core.ProgramSkillFrame f);
    void DispatchCaoChunRequestedDeckSlash(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.Card card, global::System.Collections.Generic.IReadOnlyList<int> targets);
}

internal interface ICoreRequestedDeckBasicsCoreFinalTargetSlashOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int? SpecificSlashRank(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, long? useId = default);
}

internal interface ICoreRequestedDeckBasicsCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreRequestedDeckBasicsCoreCardUseModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardUseForbidden(int actorSeat, global::CardGame.Core.CardKind effectiveKind, global::CardGame.Core.CardActionType actionType, bool ignoreIssuedPlayBan = false, long? existingUseFrameId = default);
}

internal interface ICoreRequestedDeckBasicsCharacterHanDangMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouHanDangNativeResponseAllowed(global::CardGame.Core.CharacterState provider, global::CardGame.Core.Card card, bool dyingUse = false);
}

internal interface ICoreRequestedDeckBasicsCoreNativeFactionDefenseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveFactionDefenseCandidateResponse(global::CardGame.Core.NativeOwners.FactionDefenseHandle pending, bool useDodge, bool useBagua, int? requestedCardId);
}

internal interface ICoreRequestedDeckBasicsCharacterHuanShuJunOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWenyiPeachRequest(global::CardGame.Core.ProgramSkillFrame f);
    bool TryGetWenyiPeachRequest(global::CardGame.Core.ProgramSkillFrame f, out int actor, out int target);
}

internal interface ICoreRequestedDeckBasicsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceRulesAndPublishState();
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreRequestedDeckBasicsCoreActiveNativeAttackQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get; set; }
}

internal interface ICoreRequestedDeckBasicsCoreNativeCardUseFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetCardUseStep(long frameId, global::CardGame.Core.ResolutionFrameStep step);
}

internal interface ICoreRequestedDeckBasicsCoreRecordedSuitSlashOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsRecordedSuitResponseAllowed(global::CardGame.Core.CharacterState responder, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardKind kind);
}

internal interface ICoreRequestedDeckBasicsCharacterMaTieAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool AnniversaryMaTieRequestActor(global::CardGame.Core.ProgramSkillFrame f, int actor);
    bool TryDispatchAnniversaryMaTieDeckSlash(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.Card card, global::System.Collections.Generic.IReadOnlyList<int> targets);
}

internal interface ICoreRequestedDeckBasicsCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
}

internal interface ICoreRequestedDeckBasicsCoreNativeQinglongOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanUseQinglongCrescentBladeTarget(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind? effectiveKind = default, bool? effectiveColor = default, bool allowAnyColor = true, global::CardGame.Core.Suit? physicalSuit = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default);
    void ResolveQinglongCrescentBladeChoice(global::CardGame.Core.PromptChoice selected);
}

internal interface ICoreRequestedDeckBasicsCorePhysicalDrawFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool EnsureDrawPile();
}

internal interface ICoreRequestedDeckBasicsCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind);
    void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next);
    void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame);
}

internal interface ICoreRequestedDeckBasicsCoreNativeSlashUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveSlashCore(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card slash, global::CardGame.Core.CardKind playedCardKind, int physicalOwnerSeat, global::CardGame.Core.NativeOwners.FactionCardRequestHandle? factionRequest = default, global::CardGame.Core.NativeOwners.BorrowedSwordHandle? borrowedSword = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? physicalCards = default, bool countsTowardSlashLimit = true, bool usesZhuqueFan = false, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default, long? programSkillCardUseFrameId = default, global::CardGame.Core.CardLocation? physicalSourceLocation = default);
}

internal interface ICoreRequestedDeckBasicsCharacterLvKuangLvXiangOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsQigongRequest(global::CardGame.Core.ProgramSkillFrame f);
    bool QigongAdapterActor(global::CardGame.Core.ProgramSkillFrame f, int actor, int? target, int cursor, long? parent, long? request);
    void DispatchQigongRequestedDeckSlash(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.Card card, global::System.Collections.Generic.IReadOnlyList<int> targets);
}

internal interface ICoreRequestedDeckBasicsCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreRequestedDeckBasicsCoreSnapshotValueProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardSnapshot ToSnapshot(global::CardGame.Core.Card card);
}

internal interface ICoreRequestedDeckBasicsCoreNativeDyingOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ApplyDyingResponse(global::CardGame.Core.CharacterState responder, bool usePeach, int? peachCardId, bool useAlcohol, int? alcoholCardId, global::CardGame.Core.CardConversionSource? peachConversionSource = default, global::CardGame.Core.CardConversionSource? alcoholConversionSource = default);
    void RunOneDyingStep(bool skipRequestedDeckBasic = false);
}

internal interface ICoreRequestedDeckBasicsCoreNativeTargetLegalityOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanUseSlashTarget(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card slashCard, global::CardGame.Core.CardConversionSource? conversionSource = default, global::CardGame.Core.CardKind effectiveKind = (global::CardGame.Core.CardKind)0, bool ignoreDistance = false, long? existingUseFrameId = default, bool noEffectiveRank = false, int? specificEffectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default);
}

internal interface ICoreRequestedDeckBasicsCharacterLiYiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsChanshuangSlashRequest(global::CardGame.Core.ProgramSkillFrame f);
    bool TryGetChanshuangSlashRequest(global::CardGame.Core.ProgramSkillFrame f, out int actorSeat, out global::System.Collections.Generic.IReadOnlyList<int> targets);
    void PrepareChanshuangSuppliedSlash(global::CardGame.Core.ProgramSkillFrame supplied, int actorSeat, int providerSeat, int targetSeat, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> materials, global::CardGame.Core.CardConversionSource? conversionSource, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default);
}

internal interface ICoreRequestedDeckBasicsCoreRequestedDeckBasicOwnershipOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardActionType RequestedDeckBasicActionType(global::CardGame.Core.RequestedDeckBasicIntent intent);
    void AssertRequestedDeckBasicFrame(global::CardGame.Core.RequestedDeckBasicFrame view);
    bool HasValidRequestedDeckBasicMaterial(global::CardGame.Core.ResolutionFrame owner);
}

internal interface ICoreRequestedDeckBasicsCoreDeckEquipmentProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasBeneficiarySuitShield(int actor, int target, global::CardGame.Core.Suit? suit);
}

internal interface ICoreRequestedDeckBasicsCoreAssistedPhysicalCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> AssistedPhysicalSlashChoices(global::CardGame.Core.ProgramSkillFrame frame);
    void ResolveAssistedPhysicalSlashChoice(global::CardGame.Core.PromptChoice selected);
}

internal interface ICoreRequestedDeckBasicsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreRequestedDeckBasicsCoreWenyiPeachSuppliesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int[] WenyiRequestedDeckPeachTargets(global::CardGame.Core.RequestedDeckBasicFrame view, global::CardGame.Core.Card card);
    void ExecuteWenyiRequestedDeckPeach(global::CardGame.Core.Card card, global::System.Collections.Generic.IReadOnlyList<int> targets);
}

internal interface ICoreRequestedDeckBasicsCoreNearestLegalSlashRequestsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> NearestLegalSlashChoices(global::CardGame.Core.ProgramSkillFrame frame);
    void ResolveNearestLegalSlashChoice(global::CardGame.Core.PromptChoice selected);
}

internal interface ICoreRequestedDeckBasicsCoreProgramCardLegalityOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsProgramNativeResponseAllowed(global::CardGame.Core.CharacterState provider, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> cardIds, global::CardGame.Core.CardConversionSource? source = default, bool dyingUse = false);
}

internal interface ICoreRequestedDeckBasicsCoreNativeGroupResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveGroupResponse(global::CardGame.Core.NativeOwners.GroupCardHandle group, global::CardGame.Core.CharacterState responder, global::CardGame.Core.Card? selectedResponse);
}

internal interface ICoreRequestedDeckBasicsCoreResolutionIdentityBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long _resolutionSequence { get; set; }
}

internal interface ICoreRequestedDeckBasicsCoreProgramTargetRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramTargetRules TargetRules { get; }
}

internal interface ICoreRequestedDeckBasicsCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.IndexedSkillProgramInstance Source, global::CardGame.Core.SkillProgramCardPolicy Policy)> CardPolicies(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.CardKind? requiredKind = default, bool? eventTargetIsFemale = default, bool? eventSourceIsFemale = default);
}

internal interface ICoreRequestedDeckBasicsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreRequestedDeckBasicsCoreProgramSlashRequestOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveProgramRequestSlashChoice(global::CardGame.Core.PromptChoice selected);
    void ResolveProgramRequestSlashByNearestChoice(global::CardGame.Core.PromptChoice selected);
}

internal interface ICoreRequestedDeckBasicsCoreNativeDuelOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveDuelResponse(global::CardGame.Core.NativeOwners.DuelHandle duel, global::CardGame.Core.CharacterState responder, global::CardGame.Core.Card? selectedSlash);
}

internal interface ICoreRequestedDeckBasicsCoreNativeResponseWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PopResponseWindow(long parentFrameId);
    void SetResponseParentStep(long frameId, global::CardGame.Core.ResolutionFrameStep step);
}

internal interface ICoreRequestedDeckBasicsCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.FactionDefenseHandle? ActiveFactionDefense { get; set; }

    global::CardGame.Core.NativeOwners.GroupCardHandle? ActiveGroupCard { get; set; }

    global::CardGame.Core.NativeOwners.FactionCardRequestHandle? ActiveFactionCardRequest { get; set; }

    global::CardGame.Core.NativeOwners.BorrowedSwordHandle? ActiveBorrowedSword { get; set; }

    global::CardGame.Core.NativeOwners.DuelHandle? ActiveDuel { get; set; }

    global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle? ActiveQinglongCrescentBlade { get; set; }
}

internal interface ICoreRequestedDeckBasicsCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
    void ClearPendingDecision();
}

internal interface ICoreRequestedDeckBasicsCoreProgramAllHandBonusOperationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanUsePeachToRescue(int responderSeat, int victimSeat);
}

internal interface ICoreRequestedDeckBasicsCoreTurnRedSlashBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsRedSlashProviderPaymentLegal(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, bool isTrueZhangba = false);
}

internal interface ICoreRequestedDeckBasicsCoreNativeCardAppearanceOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreRequestedDeckBasicsKernelCoreGameEngineRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceToHumanBoundary();
}

internal interface ICoreRequestedDeckBasicsKernelCoreMainOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CommandResult Reject(global::CardGame.Core.CommandErrorCode code, string message);
    global::CardGame.Core.CommandResult Accept(global::System.Action operation);
}

internal interface ICoreRequestedDeckBasicsCharacterSunYuanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSayingSlashRequest(global::CardGame.Core.ProgramSkillFrame f);
    bool TryGetSayingSlashRequest(global::CardGame.Core.ProgramSkillFrame f, out int actorSeat, out global::System.Collections.Generic.IReadOnlyList<int> targetSeats);
    void PrepareSayingSuppliedSlash(global::CardGame.Core.ProgramSkillFrame supplied, int actorSeat, int providerSeat, int targetSeat, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> materials, global::CardGame.Core.CardConversionSource? conversionSource, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default);
}

internal interface ICoreRequestedDeckBasicsCoreHandComparisonOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasSelfCardTargetProhibition(int seat);
}

internal interface ICoreRequestedDeckBasicsCoreNativeBorrowedSwordOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveBorrowedSwordSlashChoice(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending, global::CardGame.Core.Card slash, global::CardGame.Core.CardKind? requestedEffectiveKind = default);
}

internal interface ICoreRequestedDeckBasicsCoreNativeFactionSlashOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveFactionSlashCandidateResponse(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, bool useSlash, int? requestedCardId, global::CardGame.Core.CardKind? requestedEffectiveKind = default);
}

internal interface ICoreRequestedDeckBasicsCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreRequestedDeckBasicsCoreCardConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureSelectedResponseConversion(global::CardGame.Core.PromptChoice choice);
}

internal interface ICoreRequestedDeckBasicsCoreNativeDodgeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveDodgeResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.CharacterState defender, global::CardGame.Core.Card responseCard);
}
