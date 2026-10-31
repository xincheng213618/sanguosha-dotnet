#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreNativeDyingKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
    void AdvanceRulesAndPublishState();
}

internal interface ICoreNativeDyingCoreCardUseModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsTurnHandCardRestricted(global::CardGame.Core.CharacterState player, global::CardGame.Core.Card card);
    bool IsCardUseForbidden(int actorSeat, global::CardGame.Core.CardKind effectiveKind, global::CardGame.Core.CardActionType actionType, bool ignoreIssuedPlayBan = false, long? existingUseFrameId = default);
}

internal interface ICoreNativeDyingCoreDyingEntryProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginDyingEntryProgramWindow(global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreNativeDyingCoreNativeRecoveryCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolvePeach(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card peach, bool allowDying, global::CardGame.Core.CardConversionSource? conversionSource = default, global::CardGame.Core.DyingResponseEvent? dyingResponse = default);
    void ResolveDyingAlcohol(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card alcohol, global::CardGame.Core.CardConversionSource? conversionSource = default, global::CardGame.Core.DyingResponseEvent? dyingResponse = default);
}

internal interface ICoreNativeDyingCharacterHuanShuJunOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasHuanpeiDyingResponse(global::CardGame.Core.CharacterState actor);
}

internal interface ICoreNativeDyingCharacterZhaoTongZhaoGuangAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsAnniversaryYizanSelection(global::CardGame.Core.ProgramMultiCardViewAsSelection s);
}

internal interface ICoreNativeDyingCoreDrawFundedDistinctBasicResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryResolveDrawFundedDistinctBasicAiDying();
}

internal interface ICoreNativeDyingContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreNativeDyingCoreTieredRoundZeroResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasTieredRoundZeroDyingResponse(global::CardGame.Core.CharacterState responder);
    bool TryResolveTieredRoundZeroAiDying();
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> TieredRoundZeroResponseChoices(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind, bool dyingUse = false);
}

internal interface ICoreNativeDyingCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreNativeDyingCharacterHuaTuoAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReturnSkillInvocationDying(global::CardGame.Core.GameEngine.DyingCompletionReceipt dying);
}

internal interface ICoreNativeDyingCharacterYanPuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginYanPuDyingExit(global::CardGame.Core.GameEngine.DyingCompletionReceipt dying, bool survived);
}

internal interface ICoreNativeDyingCoreNativeDyingFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetDyingFrameStep(long frameId, global::CardGame.Core.ResolutionFrameStep step);
    global::CardGame.Core.DyingFrame UpdateDyingFrame(long frameId, global::System.Func<global::CardGame.Core.DyingFrame, global::CardGame.Core.DyingFrame> update);
}

internal interface ICoreNativeDyingCoreNativeDamageWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginDamageTriggerWindow(global::CardGame.Core.GameEngine.IDamageAttempt attack, long damageFrameId, global::CardGame.Core.SkillProgramTriggerWindow window);
}

internal interface ICoreNativeDyingCharacterHouChengAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsAnniversaryHouChengWineCard(int cardId);
}

internal interface ICoreNativeDyingCoreRequestedDeckBasicsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.Card> SelectedRequestedDeckBasicCards(global::CardGame.Core.CharacterState actor);
    bool HasRequestedDeckBasicSource(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind required);
}

internal interface ICoreNativeDyingCoreCuiYanNativeQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCuiYanSingleUseForbidden(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.CardConversionSource? conversion = default);
}

internal interface ICoreNativeDyingCoreDeferredBenefitsProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginFactionRecoveryDebts(global::CardGame.Core.GameEngine.DyingCompletionReceipt dying, bool survived);
}

internal interface ICoreNativeDyingCoreHandComparisonOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasSelfCardTargetProhibition(int seat);
}

internal interface ICoreNativeDyingCorePlayCardHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card ApplyProgramUseAppearance(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardConversionSource? source);
    global::CardGame.Core.SkillProgramViewAs? ViewAsRule(global::CardGame.Core.CardConversionSource source);
}

internal interface ICoreNativeDyingCoreResolutionIdentityBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long _resolutionSequence { get; set; }
}

internal interface ICoreNativeDyingCoreDynamicDiscardDamagePaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureDynamicDiscardDamageDying(long dyingId, long damageId, int victim);
    void CaptureDynamicDiscardDamageSurvival(global::CardGame.Core.GameEngine.DyingCompletionReceipt completion, bool survived);
}

internal interface ICoreNativeDyingCorePlayerViewProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICoreNativeDyingCoreCardConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasProgramCardIdentity(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramMultiCardViewAsSelection> GetProgramMultiCardViewAsSelections(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind outputKind, bool forResponse, bool ignoreSuitUseProhibition = false, int? requestedUseTarget = default);
    string ProgramConversionName(global::CardGame.Core.CardConversionSource source);
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.PromptChoice> CreateConversionChoiceVariants(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind effectiveKind, bool forResponse, string baseChoiceId, string description, global::System.Collections.Generic.IReadOnlyList<int> cards, global::System.Collections.Generic.IReadOnlyList<int> targets, global::System.Collections.Generic.IReadOnlyDictionary<string, string> baseParameters, bool dyingUse = false);
}

internal interface ICoreNativeDyingCharacterDaQiaoXiaoQiaoHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.HappyQiaoPool? CurrentHappyQiaoPool(int seat, long? parentFrameId = default, bool healthWrite = false);
}

internal interface ICoreNativeDyingCharacterXunXuAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsAnniversaryXunXuBoqiaSource(global::CardGame.Core.CardConversionSource? s);
}

internal interface ICoreNativeDyingCharacterGuanYuWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWuyouIdentityKind(global::CardGame.Core.CharacterState holder, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource> WuyouIdentitySources(global::CardGame.Core.CharacterState holder, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind);
}

internal interface ICoreNativeDyingCoreNativeAiDecisionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddThought(global::CardGame.Core.AiThoughtRecord thought);
}

internal interface ICoreNativeDyingCoreNativePlayerDeathOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinalizePlayerDeath(global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreNativeDyingCoreTableOrderOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int TableStepSeat(int from, int offset);
}

internal interface ICoreNativeDyingCharacterGuoTuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWeijieRequiredBasic(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind required);
}

internal interface ICoreNativeDyingCharacterPangJiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsBiguoCategoryForbidden(int seat, global::CardGame.Core.CardKind effectiveKind);
}

internal interface ICoreNativeDyingCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
}

internal interface ICoreNativeDyingCoreNativeDamageHandlesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameEngine.IDamageAttempt? GetCompletedDyingAttack(global::CardGame.Core.GameEngine.DyingCompletionReceipt receipt);
}

internal interface ICoreNativeDyingCoreNativePublicPileOriginalCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> NativePublicPileResponseCards(global::CardGame.Core.CharacterState provider, global::CardGame.Core.CardKind kind);
    bool IsNativePublicPileOriginalChoice(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, bool response);
}

internal interface ICoreNativeDyingCoreActionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource> GetProgramViewAsConversions(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind outputKind, bool forResponse, bool dyingUse = false);
}

internal interface ICoreNativeDyingCoreNativeCardAppearanceOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreNativeDyingCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame);
    void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind);
}

internal interface ICoreNativeDyingCharacterCaoZhiLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.Card> FuleDyingAlcoholGrainCards(global::CardGame.Core.CharacterState owner);
}

internal interface ICoreNativeDyingCoreDrawFundedDistinctBasicOffersOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasDrawFundedDistinctBasicDyingResponse(global::CardGame.Core.CharacterState actor);
}

internal interface ICoreNativeDyingCoreExtendedViewAsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ExtendedViewAsResponseChoices(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind kind);
    void ResolveExtendedViewAsResponse(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramMultiCardViewAsSelection selection);
}

internal interface ICoreNativeDyingCoreProgramCardLegalityOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsProgramMaterialUseForbidden(int actorSeat, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IEnumerable<int> cardIds, bool response = false);
    bool IsProgramNativeResponseAllowed(global::CardGame.Core.CharacterState provider, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> cardIds, global::CardGame.Core.CardConversionSource? source = default, bool dyingUse = false);
}

internal interface ICoreNativeDyingCoreNativeCardClassificationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DamageNature GetDamageNature(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreNativeDyingCharacterXiaoQiaoMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMouXqJueyanDyingResponse(global::CardGame.Core.CharacterState actor);
    bool TryMouXqJueyanAiResponse(global::CardGame.Core.CardKind kind, bool dying = false);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MouXqJueyanResponseChoices(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind, bool dying = false);
}

internal interface ICoreNativeDyingCoreProgramAlcoholConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ValidateAlcoholConversion(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardConversionSource? source, bool forResponse);
}

internal interface ICoreNativeDyingCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginMandatorySelfDyingProgram(global::CardGame.Core.DyingFrame dying);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> GetDyingProgramCandidates(global::CardGame.Core.CharacterState responder, global::CardGame.Core.DyingFrame dying);
    global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate);
    void BeginDyingProgramBinding(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreNativeDyingCoreSkillProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteProgramSkillAfterDying(global::CardGame.Core.GameEngine.DyingCompletionReceipt dying);
}

internal interface ICoreNativeDyingCoreNativeDamageCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteDamageAttack(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreNativeDyingCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetPlayableCards(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
}

internal interface ICoreNativeDyingCoreWushuangJinzhiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWushuangJinzhiSelection(global::CardGame.Core.ProgramMultiCardViewAsSelection selection);
}

internal interface ICoreNativeDyingCoreProgramAllHandBonusOperationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanUsePeachToRescue(int responderSeat, int victimSeat);
}

internal interface ICoreNativeDyingCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreNativeDyingCoreDeckEquipmentProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasBeneficiarySuitShield(int actor, int target, global::CardGame.Core.Suit? suit);
    bool IsShieldedRescueSelection(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramMultiCardViewAsSelection selection);
}

internal interface ICoreNativeDyingCoreWenhePoisonOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWenhePhysicalPoison(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
    void ReturnWenhePoisonDying(global::CardGame.Core.GameEngine.DyingCompletionReceipt returned);
}
