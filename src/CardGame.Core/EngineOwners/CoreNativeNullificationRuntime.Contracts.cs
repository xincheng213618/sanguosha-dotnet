#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreNativeNullificationCharacterLuMengMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsYingboUnrespondable(long resolutionId);
}

internal interface ICoreNativeNullificationCoreCardConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.PromptChoice> CreateConversionChoiceVariants(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind effectiveKind, bool forResponse, string baseChoiceId, string description, global::System.Collections.Generic.IReadOnlyList<int> cards, global::System.Collections.Generic.IReadOnlyList<int> targets, global::System.Collections.Generic.IReadOnlyDictionary<string, string> baseParameters, bool dyingUse = false);
    bool HasProgramCardIdentity(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramMultiCardViewAsSelection> GetProgramMultiCardViewAsSelections(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind outputKind, bool forResponse, bool ignoreSuitUseProhibition = false, int? requestedUseTarget = default);
}

internal interface ICoreNativeNullificationCoreNativeTrickEffectsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveDrawTwoEffect(global::CardGame.Core.NullificationWindowFrame pending);
    void ResolveGroupCardEffect(global::CardGame.Core.NullificationWindowFrame pending);
    void ResolvePeachGardenEffect(global::CardGame.Core.NullificationWindowFrame pending);
    void ResolveFiveGrainsEffect(global::CardGame.Core.NullificationWindowFrame pending);
    void ResolveTargetCardEffect(global::CardGame.Core.NullificationWindowFrame pending);
    void ResolveFireAttackEffect(global::CardGame.Core.NullificationWindowFrame pending);
    void ResolveBorrowedSwordEffect(global::CardGame.Core.NullificationWindowFrame pending);
    void ResolveDuelEffect(global::CardGame.Core.NullificationWindowFrame pending);
    void ResolveIronChainEffect(global::CardGame.Core.NullificationWindowFrame pending);
}

internal interface ICoreNativeNullificationCoreNativeCardUseFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetCardUseStep(long frameId, global::CardGame.Core.ResolutionFrameStep step);
}

internal interface ICoreNativeNullificationKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreNativeNullificationCoreSequentialTrickTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> FreezeSequentialTrickTarget(long frameId, global::System.Collections.Generic.IReadOnlyList<int> targets, global::CardGame.Core.LegalActionKind kind, int? firstTargetCardId, global::CardGame.Core.CardKind? requiredCardKind);
    void MoveFinishedTrickCard(long frameId, global::CardGame.Core.Card card);
}

internal interface ICoreNativeNullificationCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICoreNativeNullificationCharacterChengYuMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMouCyMatchingName(int actor, global::CardGame.Core.CardKind kind);
}

internal interface ICoreNativeNullificationCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreNativeNullificationContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreNativeNullificationCharacterHuangZhongAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAnniversaryHzPendingInvalidation(int actor);
}

internal interface ICoreNativeNullificationCoreCurrentCardEnhancementsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCurrentCardEnhancement(long frameId, global::CardGame.Core.CurrentCardEnhancement flag);
}

internal interface ICoreNativeNullificationCharacterSimaYiMobileGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginMobileSimaRenjieNullification(global::CardGame.Core.NullificationWindowFrame supplied, bool explicitPass);
}

internal interface ICoreNativeNullificationCoreCardUseModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardUseForbidden(int actorSeat, global::CardGame.Core.CardKind effectiveKind, global::CardGame.Core.CardActionType actionType, bool ignoreIssuedPlayBan = false, long? existingUseFrameId = default);
    bool IsTurnHandCardRestricted(global::CardGame.Core.CharacterState player, global::CardGame.Core.Card card);
}

internal interface ICoreNativeNullificationCharacterRuanYuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRuanYuCapability { get; }
}

internal interface ICoreNativeNullificationCoreProgramCardUseNullificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsProgramUseIneffective(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNativeNullificationCoreCommittedResponseUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCommittedResponseUsePrograms(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation);
}

internal interface ICoreNativeNullificationCoreShanheS5QuyiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ShanheS5FuqiNullificationBanned(int responderSeat);
}

internal interface ICoreNativeNullificationCoreFieldEquipmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasHappyWeiYanPotentialCancellation(int seat);
}

internal interface ICoreNativeNullificationCoreWushuangTargetCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void WushuangClaimNullifiedTrick(global::CardGame.Core.NullificationWindowFrame window, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> finishedPhysicalCards, int beforeFinishMovement);
}

internal interface ICoreNativeNullificationCoreWenheCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveWenheCardEffect(global::CardGame.Core.NullificationWindowFrame pending);
}

internal interface ICoreNativeNullificationCharacterLiuQiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWenjiNullificationResponseForbidden(global::CardGame.Core.NullificationWindowFrame window, int responderSeat);
}

internal interface ICoreNativeNullificationCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
    void ClearPendingDecision();
}

internal interface ICoreNativeNullificationCoreProgramCardLegalityOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsProgramMaterialUseForbidden(int actorSeat, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IEnumerable<int> cardIds, bool response = false);
    bool IsProgramNativeResponseAllowed(global::CardGame.Core.CharacterState provider, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> cardIds, global::CardGame.Core.CardConversionSource? source = default, bool dyingUse = false);
}

internal interface ICoreNativeNullificationCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetPlayableCards(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player);
}

internal interface ICoreNativeNullificationCharacterLuXunWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int? CaptureWuLuXunResponseRank(global::CardGame.Core.CharacterState provider, global::CardGame.Core.CardKind effectiveKind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardActionCost> costs, global::CardGame.Core.CardConversionSource? source);
}

internal interface ICoreNativeNullificationCoreActionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource> GetProgramViewAsConversions(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind outputKind, bool forResponse, bool dyingUse = false);
}

internal interface ICoreNativeNullificationCoreTableOrderOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int TableStepSeat(int from, int offset);
}

internal interface ICoreNativeNullificationCoreGroupTargetNullificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    string NullificationEffectDescription(global::CardGame.Core.NullificationWindowFrame window);
    void CompleteGroupTargetNullification(global::CardGame.Core.NullificationWindowFrame window);
}

internal interface ICoreNativeNullificationCorePhysicalMovementFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
}

internal interface ICoreNativeNullificationCoreOriginalUseLightningOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> OriginalUseLightningPlacementTargets(global::CardGame.Core.NativeOwners.JizhiResolution pending);
}

internal interface ICoreNativeNullificationCoreNativePublicPileOriginalCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> NativePublicPileResponseCards(global::CardGame.Core.CharacterState provider, global::CardGame.Core.CardKind kind);
    void PrepareNativePublicPileResponsePayment(global::CardGame.Core.CharacterState provider, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardConversionSource? conversion);
}

internal interface ICoreNativeNullificationCoreHellSoulCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveHellReturnSoulEffect(global::CardGame.Core.NullificationWindowFrame pending);
}

internal interface ICoreNativeNullificationResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreNativeNullificationCoreDragonBoatCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveDragonBoatTrickEffect(global::CardGame.Core.NullificationWindowFrame pending);
}

internal interface ICoreNativeNullificationCoreCardDeclarationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCardDeclaration(global::CardGame.Core.CharacterState provider, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardConversionSource? source, global::CardGame.Core.CardDeclarationParentReturn parent, global::System.Collections.Generic.IReadOnlyList<int> targets);
    global::CardGame.Core.CardConversionSource? PeekDeclarationConversion(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, bool use, global::CardGame.Core.CardConversionSource? explicitSource = default);
    global::CardGame.Core.CardDeclarationParentReturn DeclarationReturn(global::CardGame.Core.CardDeclarationPurpose purpose, int actor, global::CardGame.Core.LegalAction? action = default, int? target = default, int recovery = 1, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramRecoveryPolicySource>? policies = default, global::CardGame.Core.DyingResponseEvent? dying = default, bool fan = false, global::CardGame.Core.CardKind? finalKind = default);
}

internal interface ICoreNativeNullificationCharacterXunYuMobileGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveQizhengXiangshengEffect(global::CardGame.Core.NullificationWindowFrame pending);
}

internal interface ICoreNativeNullificationCoreDelayedNullificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteDelayedNullification(global::CardGame.Core.NullificationWindowFrame window, global::CardGame.Core.DelayedEffectContinuation delayed);
}

internal interface ICoreNativeNullificationCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
    bool TryBeginCommittedCardUse(long frameId, global::CardGame.Core.ProgramCardContinuation continuation, global::CardGame.Core.ProgramTrickContinuation? trick = default, global::CardGame.Core.ProgramSimpleCardContinuation? simple = default);
}

internal interface ICoreNativeNullificationCoreProgramFactionActionRewardOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardActionContext CaptureFactionAction(global::CardGame.Core.CardActionContext action);
}

internal interface ICoreNativeNullificationCoreIssuedPlayUsePoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsIssuedCardUnrespondable(long frameId);
}

internal interface ICoreNativeNullificationCorePlayCardHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramViewAs? ViewAsRule(global::CardGame.Core.CardConversionSource source);
    void ConsumeProgramViewAsUsage(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource> sources);
}

internal interface ICoreNativeNullificationCoreResponseUseCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit? FreezeCompletedResponseUseSuit(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardKind kind);
    void ContinueNullificationAfterResponseUse(global::CardGame.Core.CardActionContext action);
}

internal interface ICoreNativeNullificationCoreNativeOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardLocation FindOwnedCardLocation(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card);
}

internal interface ICoreNativeNullificationCharacterMaMiDiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMaMiDiCapability { get; }
}

internal interface ICoreNativeNullificationProgramPipelineHooksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ProgramPipelineHooks _programPipelineHooks { get; }
}

internal interface ICoreNativeNullificationCoreRecoveryReplacementOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryPauseRecoveryPaidCardUse(long useId, global::CardGame.Core.RecoveryPaidCardUseContinuation continuation);
}

internal interface ICoreNativeNullificationCoreResolutionIdentityBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long _resolutionSequence { get; set; }
}

internal interface ICoreNativeNullificationCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NullificationWindowFrame? ActiveNullificationWindow { get; }
}

internal interface ICoreNativeNullificationCoreShanheTrickResponseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ShanheTrickNullificationBanned(int responderSeat);
}

internal interface ICoreNativeNullificationCharacterLiFengOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasLiFengCapability { get; }
}

internal interface ICoreNativeNullificationCharacterCuiLieAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> AnniversaryCuiLieDelayedPlacementTargets(global::CardGame.Core.NativeOwners.JizhiResolution pending, global::System.Collections.Generic.IReadOnlyList<int> original);
}

internal interface ICoreNativeNullificationCoreProgramCardTargetEffectsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteIneffectiveTrickTarget(global::CardGame.Core.NullificationWindowFrame pending, int targetSeat);
    bool IsCardEffectIneffective(long resolutionId, int targetSeat);
}

internal interface ICoreNativeNullificationCoreCardActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginFinalizedTrickPrograms(global::CardGame.Core.NativeOwners.JizhiResolution pending);
    bool TryBeginProgramCardUseBeforeTargetEffects(global::CardGame.Core.NativeOwners.JizhiResolution pending);
    bool TryBeginProgramCardWindow(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<int> opponents, global::CardGame.Core.ProgramCardContinuation? continuation, bool? cardUseCausedDamage = default, global::CardGame.Core.ProgramTrickContinuation? trickContinuation = default, global::CardGame.Core.ProgramSimpleCardContinuation? simpleContinuation = default, global::CardGame.Core.ProgramCompletedResponseReturn? completedResponseReturn = default, global::CardGame.Core.CaoChunHandUsePayment? caoChunPayment = default, bool caoChunDebtOnly = false);
}

internal interface ICoreNativeNullificationCoreNativeNullificationFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NullificationWindowFrame ReplaceNullificationWindowFrame(global::CardGame.Core.NullificationWindowFrame next);
    global::CardGame.Core.Card GetNullificationEffectCard(global::CardGame.Core.NullificationWindowFrame frame);
}

internal interface ICoreNativeNullificationCoreUnexpectedAssaultOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveUnexpectedAssaultEffect(global::CardGame.Core.NullificationWindowFrame pending);
}

internal interface ICoreNativeNullificationCoreNativeCardUseCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinishCardUse(long frameId, global::CardGame.Core.Card card, global::CardGame.Core.CardKind? playedCardKind = default);
}

internal interface ICoreNativeNullificationCharacterCaoZhiLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PrepareLeCaoZhiWenlanResponse(global::CardGame.Core.CharacterState provider, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards);
}

internal interface ICoreNativeNullificationCharacterMiHengLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PrepareSiruiResponsePayment(global::CardGame.Core.CharacterState provider, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardConversionSource? source);
    bool IsSiruiOutsideResponseMaterial(long parentId, int cardId);
    bool IsSiruiSource(global::CardGame.Core.CardConversionSource? source);
    void BindSiruiResponseAction(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.CardActionContext action);
}

internal interface ICoreNativeNullificationCharacterZhugeGuoLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBypassLeChengyanNullification(long id, global::CardGame.Core.Card card, global::System.Collections.Generic.IReadOnlyList<int> targets, global::CardGame.Core.LegalActionKind action, int? targetCard, global::CardGame.Core.CardKind? required);
    bool TryResolveLeChengyanNativeEffect(global::CardGame.Core.NullificationWindowFrame pending);
}

internal interface ICoreNativeNullificationCoreDelayedPlacementTargetProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginFinalizedDelayedPlacementPrograms(long frameId, global::CardGame.Core.Card card, global::CardGame.Core.LegalActionKind actionKind);
}

internal interface ICoreNativeNullificationCharacterCuiYanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCuiYanNativeResponseForbidden(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind required, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards);
}

internal interface ICoreNativeNullificationCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame);
    void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind);
}

internal interface ICoreNativeNullificationCoreNativeTrickUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetCardUsePhysicalCards(long resolutionId);
    void ResolveDelayedCardEffect(global::CardGame.Core.NullificationWindowFrame pending);
}

internal interface ICoreNativeNullificationCharacterCaoXiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureCaoXiNativeCounterspellOffset(global::CardGame.Core.NullificationWindowFrame pending, global::CardGame.Core.CardActionContext action, bool invalidated);
}

internal interface ICoreNativeNullificationCoreWushuangShopCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWushuangShopDestroyedNullificationPayment(long windowId, int provider, global::CardGame.Core.CardActionCost material, int afterSequence = -1);
}

internal interface ICoreNativeNullificationCoreFriendEventsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureFriendMinimumHandUse(global::CardGame.Core.CardActionContext? action);
}

internal interface ICoreNativeNullificationCharacterZhangFeiWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PrepareWuZhangFeiResponseRanks(global::CardGame.Core.CharacterState provider, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardConversionSource? source, int? actorSeat = default);
    int? CaptureWuZhangFeiResponseRank(global::CardGame.Core.CharacterState provider, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardActionCost> costs, global::CardGame.Core.CardConversionSource? source);
    void BindWuZhangFeiResponseRanks(global::CardGame.Core.CardActionContext action);
}

internal interface ICoreNativeNullificationCoreOffTurnUsedCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureOffTurnResponseUseOrigin(long owningUseId, global::CardGame.Core.CardActionContext action);
    void CaptureOffTurnResponseUsedCards(global::CardGame.Core.CardActionContext action);
}

internal interface ICoreNativeNullificationCharacterGuoJiaMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardConversionSource? MouGjLunshiSource(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int> cards, global::CardGame.Core.CardConversionSource? selected);
}

internal interface ICoreNativeNullificationCoreShenwuCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveShenwuSowBeansEffect(global::CardGame.Core.NullificationWindowFrame pending);
    void ResolveShenwuTransplantWoodEffect(global::CardGame.Core.NullificationWindowFrame pending);
}

internal interface ICoreNativeNullificationCoreOrderedSelectedActorDuelsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsOrderedSelectedActorDuelCannotNullify(long useId);
}

internal interface ICoreNativeNullificationCharacterXiaoQiaoMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MouXqJueyanResponseChoices(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind, bool dying = false);
}

internal interface ICoreNativeNullificationCoreExtendedViewAsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ExtendedViewAsResponseChoices(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind kind);
    void ResolveExtendedViewAsResponse(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramMultiCardViewAsSelection selection);
}

internal interface ICoreNativeNullificationCoreTieredRoundZeroResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> TieredRoundZeroResponseChoices(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind, bool dyingUse = false);
    void TryIssueTieredRoundResponse(global::CardGame.Core.ResolutionFrame owner, global::CardGame.Core.CardActionContext action);
}

internal interface ICoreNativeNullificationCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsNearbyTargetResponseProhibited(int sourceSeat, int responderSeat, global::CardGame.Core.CardKind incomingKind, global::System.Collections.Generic.IReadOnlyList<int> targetSeats);
}

internal interface ICoreNativeNullificationCoreUnrespondableCounterspellsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryFinishUnrespondableCounterspell(global::CardGame.Core.NullificationWindowFrame window);
    global::CardGame.Core.CardConversionSource? FreezeUnrespondableCounterspellSource(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int> physicalCardIds);
    bool TryBeginPolicyCounterspellPayment(global::CardGame.Core.NullificationWindowFrame window, global::CardGame.Core.CardActionContext action, global::CardGame.Core.CardConversionSource? frozenSource, global::System.Collections.Generic.IReadOnlyList<int> frozenMaterialIds);
}

internal interface ICoreNativeNullificationCoreActionDiscardColorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool? CapturePhysicalCardColor(int seat, global::CardGame.Core.Card card);
    bool TracksActionDiscardColor { get; }
}

internal interface ICoreNativeNullificationCoreGuanduCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveGuanduTrickEffect(global::CardGame.Core.NullificationWindowFrame pending);
}

internal interface ICoreNativeNullificationCoreCardEffectProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginOrdinaryCardEffectPrograms(global::CardGame.Core.NullificationWindowFrame pending);
}

internal interface ICoreNativeNullificationCharacterHanDangMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouHanDangNativeResponseAllowed(global::CardGame.Core.CharacterState provider, global::CardGame.Core.Card card, bool dyingUse = false);
}

internal interface ICoreNativeNullificationCoreHulaoCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveAllianceFeastEffect(global::CardGame.Core.NullificationWindowFrame pending);
}

internal interface ICoreNativeNullificationCoreNamedAmbushInvalidationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NullificationWindowFrame CompleteNamedAmbushCounterspell(global::CardGame.Core.NullificationWindowFrame pending);
    bool HasEnabledMatchingNamedAmbush(int actor, global::CardGame.Core.CardKind kind);
    global::CardGame.Core.NullificationWindowFrame PrepareNamedAmbushCounterspell(global::CardGame.Core.NullificationWindowFrame pending, int responder, int cardId);
    void BindNamedAmbushCounterspell(long id, global::CardGame.Core.CardActionContext action);
    bool IsNamedAmbushUseIneffective(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNativeNullificationCoreWushuangSkillsBAuditOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool WushuangFuqiNullificationBanned(int responder);
}

internal interface ICoreNativeNullificationCharacterFeiYiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BindFeiYiRankEffects(long id, global::CardGame.Core.ProgramTrickContinuation trick);
}

internal interface ICoreNativeNullificationCoreMantianGuohaiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PrepareMantianGuohaiEffectRound(long id);
    void ResolveMantianGuohaiEffect(global::CardGame.Core.NullificationWindowFrame pending);
}

internal interface ICoreNativeNullificationCoreNativeSlashTargetDuelOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryCompleteCanceledNativeSlashTargetDuel(global::CardGame.Core.NullificationWindowFrame pending);
}

internal interface ICoreNativeNullificationCoreUnnullifiableOrdinaryTricksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsOriginalOrdinaryTrickCannotNullify(long useId);
}

internal interface ICoreNativeNullificationCoreActualPlayPhaseCardUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordActualPlayPhaseUse(global::CardGame.Core.CardActionContext action);
}

internal interface ICoreNativeNullificationCharacterGuanYuWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWuyouIdentityKind(global::CardGame.Core.CharacterState holder, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind);
    bool IsWuyouIdentityMaterial(global::CardGame.Core.CharacterState holder, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardConversionSource? source);
    int? CaptureJuewuResponseRank(global::CardGame.Core.CharacterState provider, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardActionCost> costs);
    void ResolveWumiaoFloodEffect(global::CardGame.Core.NullificationWindowFrame pending);
}
