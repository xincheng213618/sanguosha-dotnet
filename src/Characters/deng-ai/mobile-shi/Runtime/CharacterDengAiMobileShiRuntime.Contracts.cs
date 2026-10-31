#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICharacterDengAiMobileShiCharacterDengAiMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ShiDaChildEdge(int i);
    bool ShiDaFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child);
    bool ShiDaChildSuffix(int i);
}

internal interface ICharacterDengAiMobileShiCoreProgramOrdinaryTrickUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GameEngine.ProgramOrdinaryTrickUseOption> BuildProgramOrdinaryTrickUseOptions(global::CardGame.Core.CharacterState source, global::CardGame.Core.CardKind? outputKind = default, global::CardGame.Core.Suit? physicalSuit = default, bool excludeOwner = false, bool enforceUsePermission = false, global::CardGame.Core.Suit? beneficiaryShieldSuit = default, bool? actualEffectiveColor = default, bool hasActualColor = false, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, bool includeNextActualUseAdjustment = false, global::CardGame.Core.CardConversionSource? conversionSource = default);
}

internal interface ICharacterDengAiMobileShiCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICharacterDengAiMobileShiCoreProgramCardPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> BuildOwnedCardPaymentChoices(long frameId, int chooserSeat, int cardOwnerSeat, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardZoneKind> zones, global::CardGame.Core.OwnedCardMoveIntent intent, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SkillProgramCardCategory>? cardCategories = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardKind>? cardKinds = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Suit>? suits = default, global::System.Func<global::CardGame.Core.CardZoneKind, global::CardGame.Core.Card, bool>? canSelect = default, global::CardGame.Core.SkillProgramCardDestination destination = (global::CardGame.Core.SkillProgramCardDestination)1, int? destinationSeat = default, bool prohibitReplacingEquipment = false, int? correspondingZoneSeat = default);
}

internal interface ICharacterDengAiMobileShiKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
    void AdvanceRulesAndPublishState();
}

internal interface ICharacterDengAiMobileShiCoreRecoveryReplacementDamageRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RecoveryReplacementFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath);
}

internal interface ICharacterDengAiMobileShiCoreEquipmentContentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.EquipmentContentRegistry EquipmentRules { get; }
}

internal interface ICharacterDengAiMobileShiCoreNativeDamagePoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardTargetProhibited(global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind cardKind, global::CardGame.Core.Suit? suit = default, bool? effectiveIsRed = default);
}

internal interface ICharacterDengAiMobileShiCharacterYanPuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsYanPuDyingExitEdge(int index);
}

internal interface ICharacterDengAiMobileShiCoreDamageObserverCursorQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TypedDamagePostEventEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICharacterDengAiMobileShiCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICharacterDengAiMobileShiCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICharacterDengAiMobileShiCharacterHuaTuoAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool SkillInvocationStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICharacterDengAiMobileShiCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICharacterDengAiMobileShiCoreSkillProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinishProgramSkill(global::CardGame.Core.ProgramSkillFrame frame, bool completed);
}

internal interface ICharacterDengAiMobileShiCoreCardActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillWindowContext CreateCardActionProgramContext(global::CardGame.Core.ProgramCardTriggerWindowFrame frame, global::CardGame.Core.ProgramCardTriggerCandidate candidate);
}

internal interface ICharacterDengAiMobileShiCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next);
}

internal interface ICharacterDengAiMobileShiCoreForeignEquipmentDiscardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsForeignEquipmentDiscardPrevented(int actorSeat, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.OwnedCardMoveIntent intent);
}

internal interface ICharacterDengAiMobileShiCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
    void ClearPendingDecision();
}

internal interface ICharacterDengAiMobileShiCoreProgramContinuationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginProgramChildWindows(long frameId, global::CardGame.Core.PostEventContinuation continuation);
}

internal interface ICharacterDengAiMobileShiCoreNativeNullificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginJizhiOrNullificationWindow(long resolutionId, global::CardGame.Core.Card effectCard, int sourceSeat, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, global::CardGame.Core.LegalActionKind actionKind, int? targetCardId = default, global::CardGame.Core.CardKind? requiredCardKind = default, global::CardGame.Core.CardKind? playedCardKind = default);
}

internal interface ICharacterDengAiMobileShiPendingCardsMovedBatchesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.CardMovementBatchContext> _pendingCardsMovedBatches { get; }
}

internal interface ICharacterDengAiMobileShiCoreAdvancedLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetAdvancedCard(int id);
}

internal interface ICharacterDengAiMobileShiCoreNativeCardUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long BeginCardUse(global::CardGame.Core.Card card, int sourceSeat, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, global::CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default, global::System.Collections.Generic.IReadOnlyList<int>? designatedTargetSeats = default, bool isTrueZhangbaSlash = false, global::CardGame.Core.ProgramSelectedActorDuelOrigin? selectedActorDuelOrigin = default, global::CardGame.Core.ProgramDamageTargetDuelOrigin? damageTargetDuelOrigin = default, global::CardGame.Core.ProgramDualColorDuelOrigin? dualColorDuelOrigin = default, global::CardGame.Core.ConditionalDiscardDuelOrigin? conditionalDiscardDuelOrigin = default, global::CardGame.Core.ShiQuanDuelOrigin? shiQuanDuelOrigin = default, global::CardGame.Core.ProgramVirtualOrdinaryTrickOrigin? virtualOrdinaryTrickOrigin = default, global::CardGame.Core.DrawFundedDistinctBasicPayment? drawFundedPayment = default, global::CardGame.Core.ChainedStateBasicPayment? chainedStatePayment = default, global::CardGame.Core.FriendRecastDuelOrigin? friendRecastDuel = default, global::CardGame.Core.ChosenAssaultReturn? chosenAssault = default, global::CardGame.Core.ZhenyingDuelOrigin? zhenyingDuel = default, global::CardGame.Core.HeJinMouzhuUseOrigin? heJinMouzhu = default, global::CardGame.Core.WuAnGuoLiyongDuelOrigin? wuAnGuoLiyong = default, global::CardGame.Core.TianChouShandaoUseOrigin? tianChouShandao = default, global::CardGame.Core.QinLangXianyingUseOrigin? qinLangXianying = default, global::CardGame.Core.AHuiNanJuemanUseOrigin? aHuiNanJueman = default, global::CardGame.Core.LiuYanLimuUseOrigin? liuYanLimu = default, global::CardGame.Core.JueshengDuelOrigin? jueshengDuel = default, global::CardGame.Core.QuHuangNishouUseOrigin? quHuangNishou = default, global::CardGame.Core.WeijiePaidOrigin? weijieOrigin = default, global::CardGame.Core.ZonghuPaidOrigin? zonghuOrigin = default, global::CardGame.Core.HuanpeiPaidOrigin? huanpeiOrigin = default);
}

internal interface ICharacterDengAiMobileShiCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.SkillGrant> EnabledRuntimeSkillGrants(global::CardGame.Core.CharacterState player);
}

internal interface ICharacterDengAiMobileShiCoreSameNameHandDiscardOrDamageObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSameNameHandObserverEdge(int child);
}

internal interface ICharacterDengAiMobileShiRandomOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.DeterministicRandom _random { get; }
}

internal interface ICharacterDengAiMobileShiCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void UpdateLifecycleCardUse(long frameId, global::System.Func<global::CardGame.Core.CardUseFrame, global::CardGame.Core.CardUseFrame> update);
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
}

internal interface ICharacterDengAiMobileShiCoreObserverSubtreesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMovementObserverEdge(int child);
    bool HasObserverSuffix(int rootIndex, global::System.Func<int, bool> edge, global::System.Func<int, global::CardGame.Core.DyingFrame, bool>? completedDyingSuffix);
    global::CardGame.Core.ProgramSkillFrame? FindProgramObserverRoot(global::System.Func<global::CardGame.Core.ProgramSkillFrame, global::CardGame.Core.ResolutionFrame, bool> firstChild, global::System.Func<int, bool> suffix, long? requiredRootId = default);
    bool TryAdvanceObserverSubtree(bool advanceRecoveryReplacement, string? incompleteAttackError = default, bool routeUnreturnedAttackThroughRuntime = false);
}

internal interface ICharacterDengAiMobileShiCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasTargetCard(global::CardGame.Core.CharacterState player);
}

internal interface ICharacterDengAiMobileShiResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICharacterDengAiMobileShiCorePhysicalMovementFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
}

internal interface ICharacterDengAiMobileShiCoreShownEntityTurnPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ShownEntityUseActorMatches(global::CardGame.Core.CardUseFrame use, int originalActor, int provider);
}

internal interface ICharacterDengAiMobileShiCoreNativeLogicalDiscardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveProgramLogicalDiscard(global::CardGame.Core.ProgramSkillFrame frame, int executorSeat, global::System.Collections.Generic.IReadOnlyList<int> ids, global::CardGame.Core.CardMoveReason reason, global::System.Action<long, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord>>? afterPhysicalRecords = default);
}

internal interface ICharacterDengAiMobileShiCoreObserverSubtreesOperations2 : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying);
}

internal interface ICharacterDengAiMobileShiCoreProgramNativeDrawOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramStepOutcome BeginOperationProgramDraw(global::CardGame.Core.ProgramSkillFrame frame, int recipientSeat, int requestedCount, global::CardGame.Core.CardMoveReason reason);
}

internal interface ICharacterDengAiMobileShiCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICharacterDengAiMobileShiCoreCardUseModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardUseForbidden(int actorSeat, global::CardGame.Core.CardKind effectiveKind, global::CardGame.Core.CardActionType actionType, bool ignoreIssuedPlayBan = false, long? existingUseFrameId = default);
}

internal interface ICharacterDengAiMobileShiCoreAfterTurnEndedProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsActualAfterTurnEndedParent(global::CardGame.Core.DeferredTurnEndFrame parent);
    bool MatchesAfterTurnEndedChild(global::CardGame.Core.DeferredTurnEndFrame parent, global::CardGame.Core.ProgramSkillFrame child, global::CardGame.Core.ProgramSkillWindowContext context);
}

internal interface ICharacterDengAiMobileShiCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
}

internal interface ICharacterDengAiMobileShiCorePrivateOfferReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramEffect? GetPausedPrivateOfferEffect(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICharacterDengAiMobileShiCoreResolutionIdentityBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long _resolutionSequence { get; set; }
}

internal interface ICharacterDengAiMobileShiCoreDirectedTurnCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDirectedCardTargetProhibited(int actorSeat, int targetSeat, global::CardGame.Core.CardKind cardKind, long? cardUseFrameId = default);
}

internal interface ICharacterDengAiMobileShiCoreEquipmentSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsActiveProgramSourceEquipmentCard(int ownerSeat, string skillId, string skillInstanceId, global::CardGame.Core.Card card);
}

internal interface ICharacterDengAiMobileShiContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}
