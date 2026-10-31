#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICharacterDiaoChanLeCoreDamageObserverCursorQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TypedDamagePostEventEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICharacterDiaoChanLeResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack();
}

internal interface ICharacterDiaoChanLeCharacterHuangQuanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanUseJianjiOrdinaryAlcohol(global::CardGame.Core.CharacterState actor, int? selectedId);
}

internal interface ICharacterDiaoChanLeCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICharacterDiaoChanLeCoreDesignatedExtraTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool DesignatedExtraTargetCandidateMatches(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context, global::CardGame.Core.ProgramCardTriggerWindowFrame window);
}

internal interface ICharacterDiaoChanLeCoreActiveNativeAttackQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get; set; }
}

internal interface ICharacterDiaoChanLeCoreProgramContinuationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginProgramChildWindows(long frameId, global::CardGame.Core.PostEventContinuation continuation);
}

internal interface ICharacterDiaoChanLeCoreRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetCardUseDistanceLimit(global::CardGame.Core.CharacterState player, global::CardGame.Core.CardKind effectiveCardKind);
}

internal interface ICharacterDiaoChanLeCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICharacterDiaoChanLeCoreNativeEquipmentUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteEquipmentUse(global::CardGame.Core.CharacterState source, global::CardGame.Core.Card equipment, long resolutionId);
}

internal interface ICharacterDiaoChanLeCoreRecoveryReplacementDamageRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RecoveryReplacementFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath);
}

internal interface ICharacterDiaoChanLeCoreProgramNativeDrawOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramStepOutcome BeginOperationProgramDraw(global::CardGame.Core.ProgramSkillFrame frame, int recipientSeat, int requestedCount, global::CardGame.Core.CardMoveReason reason);
}

internal interface ICharacterDiaoChanLeCorePlayerViewProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICharacterDiaoChanLeCoreAdvancedLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetAdvancedCard(int id);
    global::CardGame.Core.CardKind AdvancedEffectiveHandKind(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICharacterDiaoChanLeCoreParticipantHandProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PublishParticipantHandChoice(global::CardGame.Core.ProgramSkillFrame frame, int chooser, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> choices, string text, int target);
}

internal interface ICharacterDiaoChanLeCoreProgramOrdinaryTrickUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GameEngine.ProgramOrdinaryTrickUseOption> BuildProgramOrdinaryTrickUseOptions(global::CardGame.Core.CharacterState source, global::CardGame.Core.CardKind? outputKind = default, global::CardGame.Core.Suit? physicalSuit = default, bool excludeOwner = false, bool enforceUsePermission = false, global::CardGame.Core.Suit? beneficiaryShieldSuit = default, bool? actualEffectiveColor = default, bool hasActualColor = false, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, bool includeNextActualUseAdjustment = false, global::CardGame.Core.CardConversionSource? conversionSource = default);
}

internal interface ICharacterDiaoChanLeCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramSkillFrame frame);
    global::CardGame.Core.SkillProgramTriggerFacts CaptureProgramTriggerFacts(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardActionContext action);
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
    global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate);
}

internal interface ICharacterDiaoChanLeCoreCardAttackStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void UpdateCardAttackState(long id, global::System.Func<global::CardGame.Core.CardAttackState?, global::CardGame.Core.CardAttackState> update);
}

internal interface ICharacterDiaoChanLeCharacterZhugeLiangMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMouWujingAllowance(global::CardGame.Core.CharacterState owner);
}

internal interface ICharacterDiaoChanLeCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
    void BeginSimpleCardUse(long frameId, global::CardGame.Core.ProgramSimpleCardContinuation continuation);
    bool TryMarkProgramUseCommitted(long frameId);
    void UpdateLifecycleCardUse(long frameId, global::System.Func<global::CardGame.Core.CardUseFrame, global::CardGame.Core.CardUseFrame> update);
}

internal interface ICharacterDiaoChanLeCoreNativeRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
}

internal interface ICharacterDiaoChanLeCoreDirectedTurnCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDirectedCardTargetProhibited(int actorSeat, int targetSeat, global::CardGame.Core.CardKind cardKind, long? cardUseFrameId = default);
}

internal interface ICharacterDiaoChanLeCoreOutsidePhaseDrawDiscardOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidNativeDrawInvocation(global::CardGame.Core.NativeDrawInvocationProof proof);
}

internal interface ICharacterDiaoChanLeCoreAdvancedPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasNextUnlimitedCard(global::CardGame.Core.CharacterState owner);
    bool HasFriendQihuiAllowance(global::CardGame.Core.CharacterState owner);
}

internal interface ICharacterDiaoChanLeCharacterCuiYanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCuiYanNextUseForbidden(int actorSeat, global::System.Collections.Generic.IReadOnlyList<int> materialIds, global::CardGame.Core.Suit? nativeEffectiveSuit);
}

internal interface ICharacterDiaoChanLeCoreProgramCardUseRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddProgramTargetCountSlashActions(global::System.Collections.Generic.ICollection<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card physicalCard, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CharacterState> legalTargets, string slashName, global::CardGame.Core.CardKind? playedCardKind, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default);
}

internal interface ICharacterDiaoChanLeCoreRedAdditionalTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsTargetAdjustmentAction(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action);
}

internal interface ICharacterDiaoChanLeCoreNativeTrickUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteAlcoholUse(global::CardGame.Core.CharacterState source, global::CardGame.Core.Card alcohol, long resolutionId);
}

internal interface ICharacterDiaoChanLeCoreOriginalHandEntityObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ExactOriginalHandLifecycleParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramTriggerWindow window);
    bool OriginalHandBindingStarted(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramTriggerWindow window);
}

internal interface ICharacterDiaoChanLeCharacterZhouYuOlMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MouGuquActorLineageMatches(long useId, long actionId, int originalActor, int currentActor, int provider, global::CardGame.Core.CardActionType type);
}

internal interface ICharacterDiaoChanLeCoreRecoveryReplacementDamageRidesOperations2 : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RecoveryReplacementFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath);
}

internal interface ICharacterDiaoChanLeCoreHandComparisonOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasSelfCardTargetProhibition(int seat);
}

internal interface ICharacterDiaoChanLeCoreGuanduCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddGuanduPhysicalOrConvertedActions(global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardConversionSource? conversion);
    void ResolveGuanduCard(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.LegalAction action);
}

internal interface ICharacterDiaoChanLeCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NullificationWindowFrame? ActiveNullificationWindow { get; }
}

internal interface ICharacterDiaoChanLeCoreProgramFactionActionRewardOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardActionContext CaptureFactionAction(global::CardGame.Core.CardActionContext action);
}

internal interface ICharacterDiaoChanLeCharacterSunHongOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ZenrunFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICharacterDiaoChanLeKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
    void AdvanceRulesAndPublishState();
}

internal interface ICharacterDiaoChanLeCoreNativeTargetLegalityOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanUseVirtualSlashTarget(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind effectiveKind = (global::CardGame.Core.CardKind)0, global::CardGame.Core.Suit? physicalSuit = default, bool? effectiveColor = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, bool ignoreDistance = false, global::CardGame.Core.CardConversionSource? conversionSource = default, bool actualUnconvertedPhysical = false, bool countsTowardLimit = true);
}

internal interface ICharacterDiaoChanLeCoreDirectedCardPolicyQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardDistanceExemption(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind kind, long? cardUseFrameId = default, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default);
    bool HasCardArmorBypass(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind kind);
}

internal interface ICharacterDiaoChanLeCoreDrawFundedDistinctBasicReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card ReadDrawFundedDistinctBasicUseAppearance(long ownerFrameId, global::CardGame.Core.CardAppearanceReference appearance);
}

internal interface ICharacterDiaoChanLeCoreNativeNullificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginJizhiOrNullificationWindow(long resolutionId, global::CardGame.Core.Card effectCard, int sourceSeat, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, global::CardGame.Core.LegalActionKind actionKind, int? targetCardId = default, global::CardGame.Core.CardKind? requiredCardKind = default, global::CardGame.Core.CardKind? playedCardKind = default);
}

internal interface ICharacterDiaoChanLeCoreFinalizedCardTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginEquipmentTargetPrograms(global::CardGame.Core.CharacterState source, global::CardGame.Core.Card equipment, long frameId);
    global::System.Collections.Generic.IReadOnlyList<int> GetImplicitSelfCardUseTargets(global::CardGame.Core.CardKind kind, int sourceSeat, global::System.Collections.Generic.IReadOnlyList<int> targets);
}

internal interface ICharacterDiaoChanLeCoreDeckEquipmentProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasBeneficiarySuitShield(int actor, int target, global::CardGame.Core.Suit? suit);
}

internal interface ICharacterDiaoChanLeCoreShenwuEquipmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsShenwuRobeTargetProhibited(int actorSeat, int targetSeat, global::CardGame.Core.CardKind kind);
}

internal interface ICharacterDiaoChanLeCoreNativeOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MarkSlashUsedOrPlayedDuringCurrentPlayPhase(int actorSeat, global::CardGame.Core.CardKind effectiveCardKind);
}

internal interface ICharacterDiaoChanLeCoreNativeCardUseCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinishCardUse(long frameId, global::CardGame.Core.Card card, global::CardGame.Core.CardKind? playedCardKind = default);
    void PopFinishedCardUse(long frameId);
    void CompleteFinishedAttackCardUse(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICharacterDiaoChanLeCharacterWangGuanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<int> MiuyanDraftHandIds(global::CardGame.Core.ProgramHandDraftRevealedEvent e);
    global::System.Collections.Generic.IEnumerable<int> MiuyanQingliangHandIds(global::CardGame.Core.ProgramQingliangRevealEvent e);
    global::System.Collections.Generic.IEnumerable<int> MiuyanJuanxiaHandIds(global::CardGame.Core.ProgramJuanxiaTrickUsedEvent e);
}

internal interface ICharacterDiaoChanLeCoreSkillProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinishProgramSkill(global::CardGame.Core.ProgramSkillFrame frame, bool completed);
}

internal interface ICharacterDiaoChanLeCoreNextActualUseTargetAdjustmentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddNextActualUseAdjustmentActions(global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor);
}

internal interface ICharacterDiaoChanLeCoreNativeCardUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long BeginCardUse(global::CardGame.Core.Card card, int sourceSeat, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, global::CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default, global::System.Collections.Generic.IReadOnlyList<int>? designatedTargetSeats = default, bool isTrueZhangbaSlash = false, global::CardGame.Core.ProgramSelectedActorDuelOrigin? selectedActorDuelOrigin = default, global::CardGame.Core.ProgramDamageTargetDuelOrigin? damageTargetDuelOrigin = default, global::CardGame.Core.ProgramDualColorDuelOrigin? dualColorDuelOrigin = default, global::CardGame.Core.ConditionalDiscardDuelOrigin? conditionalDiscardDuelOrigin = default, global::CardGame.Core.ShiQuanDuelOrigin? shiQuanDuelOrigin = default, global::CardGame.Core.ProgramVirtualOrdinaryTrickOrigin? virtualOrdinaryTrickOrigin = default, global::CardGame.Core.DrawFundedDistinctBasicPayment? drawFundedPayment = default, global::CardGame.Core.ChainedStateBasicPayment? chainedStatePayment = default, global::CardGame.Core.FriendRecastDuelOrigin? friendRecastDuel = default, global::CardGame.Core.ChosenAssaultReturn? chosenAssault = default, global::CardGame.Core.ZhenyingDuelOrigin? zhenyingDuel = default, global::CardGame.Core.HeJinMouzhuUseOrigin? heJinMouzhu = default, global::CardGame.Core.WuAnGuoLiyongDuelOrigin? wuAnGuoLiyong = default, global::CardGame.Core.TianChouShandaoUseOrigin? tianChouShandao = default, global::CardGame.Core.QinLangXianyingUseOrigin? qinLangXianying = default, global::CardGame.Core.AHuiNanJuemanUseOrigin? aHuiNanJueman = default, global::CardGame.Core.LiuYanLimuUseOrigin? liuYanLimu = default, global::CardGame.Core.JueshengDuelOrigin? jueshengDuel = default, global::CardGame.Core.QuHuangNishouUseOrigin? quHuangNishou = default, global::CardGame.Core.WeijiePaidOrigin? weijieOrigin = default, global::CardGame.Core.ZonghuPaidOrigin? zonghuOrigin = default, global::CardGame.Core.HuanpeiPaidOrigin? huanpeiOrigin = default);
}

internal interface ICharacterDiaoChanLeCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next);
}

internal interface ICharacterDiaoChanLeResolutionStackOperations2 : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICharacterDiaoChanLeCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default);
}

internal interface ICharacterDiaoChanLeCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
}

internal interface ICharacterDiaoChanLeCoreCardUseModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardUseForbidden(int actorSeat, global::CardGame.Core.CardKind effectiveKind, global::CardGame.Core.CardActionType actionType, bool ignoreIssuedPlayBan = false, long? existingUseFrameId = default);
    global::System.Collections.Generic.IReadOnlyList<int> ApplyTurnCardGroupTargetRestrictions(global::CardGame.Core.CharacterState source, global::System.Collections.Generic.IReadOnlyList<int> targets);
}

internal interface ICharacterDiaoChanLeCoreSameNameHandDiscardOrDamageObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSameNameHandObserverEdge(int child);
}

internal interface ICharacterDiaoChanLeCoreEquipmentContentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.EquipmentContentRegistry EquipmentRules { get; }
}

internal interface ICharacterDiaoChanLeCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICharacterDiaoChanLeCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player);
    bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId);
    bool HasRuntimeSkill(global::CardGame.Core.CharacterState player, string skillId);
}

internal interface ICharacterDiaoChanLeCoreSnapshotValueProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardSnapshot ToSnapshot(global::CardGame.Core.Card card);
}

internal interface ICharacterDiaoChanLeCoreTreatedWoundedOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsRuleWounded(global::CardGame.Core.CharacterState player);
}

internal interface ICharacterDiaoChanLePendingCardsMovedBatchesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.CardMovementBatchContext> _pendingCardsMovedBatches { get; }
}

internal interface ICharacterDiaoChanLeCharacterXiaoQiaoMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginPublicHandRevealWindow(long parentId, string key);
}

internal interface ICharacterDiaoChanLeCoreResponseUseCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCompletedResponseUse(global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation);
}

internal interface ICharacterDiaoChanLeCoreCardActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginProgramCardWindow(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<int> opponents, global::CardGame.Core.ProgramCardContinuation? continuation, bool? cardUseCausedDamage = default, global::CardGame.Core.ProgramTrickContinuation? trickContinuation = default, global::CardGame.Core.ProgramSimpleCardContinuation? simpleContinuation = default, global::CardGame.Core.ProgramCompletedResponseReturn? completedResponseReturn = default, global::CardGame.Core.CaoChunHandUsePayment? caoChunPayment = default, bool caoChunDebtOnly = false);
    global::CardGame.Core.ProgramSkillWindowContext CreateCardActionProgramContext(global::CardGame.Core.ProgramCardTriggerWindowFrame frame, global::CardGame.Core.ProgramCardTriggerCandidate candidate);
    global::CardGame.Core.ProgramSkillWindowContext CreateCardActionProgramContext(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, long parentFrameId, int ownerSeat, int eventTargetSeat, global::CardGame.Core.SkillProgramTriggerFacts facts);
}

internal interface ICharacterDiaoChanLeCoreNativeCardUseFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetCardUseStep(long frameId, global::CardGame.Core.ResolutionFrameStep step);
}

internal interface ICharacterDiaoChanLeCoreCardUseDebitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordSlashUseDebit(long cardUseFrameId, int actorSeat);
}

internal interface ICharacterDiaoChanLeCorePrivateOfferReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramEffect? GetPausedPrivateOfferEffect(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICharacterDiaoChanLeCoreObserverSubtreesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMovementObserverEdge(int child);
    bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying);
}

internal interface ICharacterDiaoChanLeCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICharacterDiaoChanLeCorePhysicalMovementFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
}

internal interface ICharacterDiaoChanLeContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICharacterDiaoChanLeCoreNativeRecoveryCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveRecoveryCard(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card card, string cardName, global::CardGame.Core.CardKind? playedCardKind = default, int recoveryAmount = 1, global::System.Collections.Generic.IReadOnlyList<(string SkillId, string PolicyId)>? recoveryPolicySources = default, global::CardGame.Core.CardConversionSource? conversionSource = default, global::CardGame.Core.DyingResponseEvent? dyingResponse = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? physicalCards = default, global::CardGame.Core.DrawFundedDistinctBasicPayment? drawFundedPayment = default, global::CardGame.Core.HuanpeiPaidOrigin? huanpeiOrigin = default);
    void CompleteRecoveryCardUse(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card card, long resolutionId, global::CardGame.Core.CardKind playedCardKind, int recoveryAmount, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramRecoveryPolicySource> recoveryPolicySources);
}

internal interface ICharacterDiaoChanLeCoreTrustedCardZoneDiagnosticsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardZoneDiagnostic> CreateCardZoneDiagnostics();
}

internal interface ICharacterDiaoChanLeCoreProgramTargetRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramTargetRules TargetRules { get; }
}

internal interface ICharacterDiaoChanLeCoreObserverSubtreesOperations2 : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasObserverSuffix(int rootIndex, global::System.Func<int, bool> edge, global::System.Func<int, global::CardGame.Core.DyingFrame, bool>? completedDyingSuffix);
    global::CardGame.Core.ProgramSkillFrame? FindProgramObserverRoot(global::System.Func<global::CardGame.Core.ProgramSkillFrame, global::CardGame.Core.ResolutionFrame, bool> firstChild, global::System.Func<int, bool> suffix, long? requiredRootId = default);
    bool TryAdvanceObserverSubtree(bool advanceRecoveryReplacement, string? incompleteAttackError = default, bool routeUnreturnedAttackThroughRuntime = false);
}

internal interface ICharacterDiaoChanLeCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICharacterDiaoChanLeCoreCardConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IgnoresProgramSlashLimit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardConversionSource? source);
}

internal interface ICharacterDiaoChanLeKernelCoreMainOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long resolutionId, int sourceSeat, int targetSeat, global::CardGame.Core.Card? card, int damageAmount = 1, global::CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, bool isDelayedJudgmentDamage = false, int? delayedJudgmentSeat = default, global::CardGame.Core.DamageNature? damageNatureOverride = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? physicalCards = default, long? programJudgmentFrameId = default, long? programSkillFrameId = default, global::CardGame.Core.CardConversionSource? conversionSource = default, long? programSkillCardUseFrameId = default);
    global::CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle2(long ownerId);
}

internal interface ICharacterDiaoChanLeCoreNativeSlashUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginSlashTargetResolution(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICharacterDiaoChanLeCharacterDiaoChanLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool DiouNativeWindowEdge(int index);
    bool DiouFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child);
    bool DiouObserverSuffix(int index);
}

internal interface ICharacterDiaoChanLeCoreDragonBoatCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddDragonBoatPhysicalOrConvertedActions(global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardConversionSource? conversion);
    void ResolveDragonBoatCard(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.LegalAction action);
}

internal interface ICharacterDiaoChanLeCoreCurrentCardEnhancementsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureProgramAdjustedSlashBaseDamage(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICharacterDiaoChanLeCoreTurnTargetCardQuotaAllowancesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasTargetCardQuotaAllowance(int actor, int target);
}

internal interface ICharacterDiaoChanLeCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ClearPendingDecision();
}

internal interface ICharacterDiaoChanLeCoreAlcoholSlashSuppressionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureProgramAlcoholConsumption(long cardUseFrameId, global::CardGame.Core.CharacterState actor);
}

internal interface ICharacterDiaoChanLeCharacterYanPuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsYanPuDyingExitEdge(int index);
}

internal interface ICharacterDiaoChanLeCoreActualTurnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasJudgmentEffectiveCard(global::CardGame.Core.CharacterState player, global::CardGame.Core.CardKind kind);
}

internal interface ICharacterDiaoChanLeCoreNextCardTargetAdjustmentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddNextCardTargetAdjustmentActions(global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor);
}

internal interface ICharacterDiaoChanLeCharacterHuangGaiMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordMouZhaJiangAwareAlcoholDebit(long useId, global::CardGame.Core.CharacterState actor);
}

internal interface ICharacterDiaoChanLeCoreNativeDamagePoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardTargetProhibited(global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind cardKind, global::CardGame.Core.Suit? suit = default, bool? effectiveIsRed = default);
}
