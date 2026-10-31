#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreSkillProgramsCoreDiscardedProvenanceOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertProvenanceClaim(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreSkillProgramsCoreComparedBlackSlashPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertComparedBlackSlashReceipt(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreSkillProgramsCoreShanheS5XuncanOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertShanheS5XuncanNativeUse(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreSkillProgramsCoreLordSkillProjectionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasSkillRoleQualification(global::CardGame.Core.CharacterState owner, string skillId, string? instance, global::CardGame.Core.Role role);
}

internal interface ICoreSkillProgramsCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreSkillProgramsCoreNativeCardAppearanceOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreSkillProgramsCorePrepDiscardReceiptsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertPrepDiscardReceipts(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreSkillProgramsCoreProvenanceVirtualAlcoholOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertProvenanceAlcohol(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreSkillProgramsCoreFireTargetBenefitObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertFireTargetBenefit(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreSkillProgramsCharacterDaQiaoXiaoQiaoHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.HappyQiaoPool? CurrentHappyQiaoPool(int seat, long? parentFrameId = default, bool healthWrite = false);
}

internal interface ICoreSkillProgramsCorePhysicalMovementFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
}

internal interface ICoreSkillProgramsCoreSlashSuitDiscardOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertProgramSlashSuitDiscard(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused);
}

internal interface ICoreSkillProgramsCoreQuotaTopProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertQuotaTop(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused);
}

internal interface ICoreSkillProgramsCoreConfiguredConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertConfiguredCardDeclaration(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused);
}

internal interface ICoreSkillProgramsCharacterWangChangMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMobileShiWangChangChoiceResult(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramChoiceResultBinding b);
}

internal interface ICoreSkillProgramsCharacterManChongBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsBoundaryManChongChoiceResult(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramChoiceResultBinding binding);
}

internal interface ICoreSkillProgramsCoreRoundOnceCardlessNativeResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertRoundOnceCardlessNativeResponses();
}

internal interface ICoreSkillProgramsCoreEquipmentSlotGroupProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramLegalityParticipant CreateProgramLegalityParticipant(int seat);
    bool CanBePindianTarget(int sourceSeat, int targetSeat);
}

internal interface ICoreSkillProgramsCharacterChenQunMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMobileShiChenQunFaenChoiceResult(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramChoiceResultBinding binding);
}

internal interface ICoreSkillProgramsCoreTreatedWoundedOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsRuleWounded(global::CardGame.Core.CharacterState player);
}

internal interface ICoreSkillProgramsCoreDyingEntryProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginDyingEntryProgramWindow(global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreSkillProgramsCoreNamedDefenseAndPublicDraftOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertNamedDefenseAndPublicDraft(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused);
}

internal interface ICoreSkillProgramsCharacterHanFuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasPaidShuziInitialSelection(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreSkillProgramsCoreUnnullifiableOrdinaryTricksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertOrdinaryTrickCannotNullify(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreSkillProgramsCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player);
    string GetRuntimeSkillInstanceId(global::CardGame.Core.CharacterState player, string skillId);
}

internal interface ICoreSkillProgramsCoreProgramSelectedGlobalCardUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramStepOutcome UseProgramSelectedCardsAsGlobal(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramMultiCardViewAsSelection selection);
}

internal interface ICoreSkillProgramsCoreForeignEquipmentDiscardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasDiscardableHeBy(int actorSeat, int ownerSeat);
    bool HasForeignDiscardCapability { get; }
}

internal interface ICoreSkillProgramsCoreAssistedPhysicalCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertAssistedPhysicalCardDrafts(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused);
}

internal interface ICoreSkillProgramsCoreTurnHandCategoryRestrictionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSelfHandCategoryDiscardForbidden(int actor, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.OwnedCardMoveIntent intent);
}

internal interface ICoreSkillProgramsCharacterZhangXunAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasExactAnniversaryZhangXunFollowupSelection(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreSkillProgramsCharacterPeiYuanShaoOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasIssuedFulueRequest(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreSkillProgramsCoreCompletedUsePaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertCompletedUsePayment(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreSkillProgramsCoreProgramAttackAttemptOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameEngine.IDamageAttempt? CurrentDamageAttempt { get; }
}

internal interface ICoreSkillProgramsCoreNativeSkillContextQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PlayerSkillContext CreateSkillContext(global::CardGame.Core.CharacterState player, bool includeHandLimit = false);
}

internal interface ICoreSkillProgramsCoreAppliedDamageBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    string? DistinctTurnTargetUsage(global::CardGame.Core.SkillProgramActivation activation);
    bool CanActivateDistinctTurnTarget(int ownerSeat, string skillId, string usageId, int targetSeat);
}

internal interface ICoreSkillProgramsCoreShanheS5ZhangliangOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasExactOwnTurnEnemyHandGainSelection(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreSkillProgramsCharacterXiahouShangMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasExactMobileShiTanfengParticipantSelection(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreSkillProgramsCoreNearestLegalSlashRequestsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidNearestSlashProgramSelection(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SkillProgramEffect> instructions);
    bool IsNearestFactionChoiceResult(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramChoiceResultBinding binding);
    void AssertNearestLegalSlashState(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused);
}

internal interface ICoreSkillProgramsCharacterHeQiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame CaptureEquipmentSuitProgramSource(global::CardGame.Core.ProgramSkillFrame frame);
    bool ValidateEquipmentSuitProgramSource(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreSkillProgramsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreSkillProgramsCoreForeignContestAidObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertForeignContestAidPrograms();
}

internal interface ICoreSkillProgramsCoreProgramRevealsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertProgramRevealCardSelection(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused);
}

internal interface ICoreSkillProgramsCoreResolutionIdentityBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long _resolutionSequence { get; set; }
}

internal interface ICoreSkillProgramsCoreProgramSelectionModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSelectionRegistry ProgramSelections { get; }
}

internal interface ICoreSkillProgramsCoreAlternatingSuitAndCompletedTopOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertAlternatingSuitTop(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect paused);
}

internal interface ICoreSkillProgramsCoreSharedPhaseSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryExecuteSharedProgramActivation(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action, global::System.Collections.Generic.IReadOnlyList<int> cards, global::System.Collections.Generic.IReadOnlyList<int> targets);
}

internal interface ICoreSkillProgramsCoreProgramRuntimeHooksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramRuntimeHooks RuntimeHooks { get; }
}

internal interface ICoreSkillProgramsCorePhaseHandSeizureReceiptsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertPhaseHandSeizure(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreSkillProgramsCoreActionCategoryGiftsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertDifferentActionCategoryGift(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused);
}

internal interface ICoreSkillProgramsCoreSequentialDiscardReceiptsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidSequentialDiscard(global::CardGame.Core.ProgramSkillFrame f);
    void AssertSequentialDiscard(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreSkillProgramsCoreProgramDodgeResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWithinAttackRange(int actorSeat, int targetSeat);
}

internal interface ICoreSkillProgramsCharacterYangBiaoAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasExactAnniversaryYangBiaoParticipantSelection(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreSkillProgramsCoreConvertingGiftsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertConvertingGift(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect paused);
}

internal interface ICoreSkillProgramsCharacterHuangQuanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertHuangQuanNativeUses();
}

internal interface ICoreSkillProgramsCoreTieredRoundZeroUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertTieredRoundConversionUses();
}

internal interface ICoreSkillProgramsCoreDiscardBudgetAndFactionGiftsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertBudgetGiftDraft(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect paused);
}

internal interface ICoreSkillProgramsCoreProgramCompoundCardTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertProgramCompoundCardTarget(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused);
}

internal interface ICoreSkillProgramsKernelCoreMainOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CommandResult Reject(global::CardGame.Core.CommandErrorCode code, string message);
    global::CardGame.Core.CommandResult Accept(global::System.Action operation);
}

internal interface ICoreSkillProgramsCharacterZhangJiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidZhangJiNativeUse(global::CardGame.Core.CardUseFrame u);
}

internal interface ICoreSkillProgramsCoreMatchSetupDefinitionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    string? GetEffectiveFactionId(global::CardGame.Core.CharacterState player);
}

internal interface ICoreSkillProgramsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> GetProgramTargetSeats(int ownerSeat, global::CardGame.Core.SkillProgramTargetKind targetKind, global::CardGame.Core.ProgramSkillWindowContext? windowContext = default, global::CardGame.Core.PlayerMarkerKind? marker = default, global::CardGame.Core.ProgramParticipantReference? actorReference = default);
    bool IsProgramTargetEligible(int ownerSeat, global::CardGame.Core.SkillProgramTargetKind targetKind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardZoneKind> zones, int targetSeat, global::CardGame.Core.PlayerMarkerKind? marker = default, global::CardGame.Core.ProgramSkillWindowContext? windowContext = default, global::CardGame.Core.ProgramParticipantReference? actorReference = default);
    global::CardGame.Core.ProgramAiPublicContext CreateProgramAiPublicContext(global::CardGame.Core.CharacterState owner);
    bool TryBeginMandatorySelfDyingProgram(global::CardGame.Core.DyingFrame dying);
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
    void CancelProgramBindingAndCleanup(global::CardGame.Core.ProgramSkillFrame frame, string reason);
}

internal interface ICoreSkillProgramsCoreZhaoguMirrorOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertZhaoguMirror(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect? paused);
}

internal interface ICoreSkillProgramsCoreNativeSlashUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveSlashCore(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card slash, global::CardGame.Core.CardKind playedCardKind, int physicalOwnerSeat, global::CardGame.Core.NativeOwners.FactionCardRequestHandle? factionRequest = default, global::CardGame.Core.NativeOwners.BorrowedSwordHandle? borrowedSword = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? physicalCards = default, bool countsTowardSlashLimit = true, bool usesZhuqueFan = false, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default, long? programSkillCardUseFrameId = default, global::CardGame.Core.CardLocation? physicalSourceLocation = default);
}

internal interface ICoreSkillProgramsCharacterZhugeLiangMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertMouZhijueProgram(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused);
}

internal interface ICoreSkillProgramsCoreProgramChoiceProofsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasDeclaredProgramChoiceProof(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramExecutionPlan plan, global::CardGame.Core.ProgramChoiceResultBinding binding);
}

internal interface ICoreSkillProgramsCoreRedDiscardRecoveryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertRedDiscardRecoveryDraft(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect paused);
}

internal interface ICoreSkillProgramsCoreFixedRecipientBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidFixedRecipientReceipt(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreSkillProgramsCoreHandComparisonOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertHandComparison(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? op);
}

internal interface ICoreSkillProgramsCoreRevealRecastProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertPairedHandRevealChoice(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused);
}

internal interface ICoreSkillProgramsCoreNativeEquipmentUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteEquipmentUse(global::CardGame.Core.CharacterState source, global::CardGame.Core.Card equipment, long resolutionId);
}

internal interface ICoreSkillProgramsCoreAdjacentDiscardStorageOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertPendingAdjacentDiscardOrigins();
    void AssertAdjacentDiscardStorage(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreSkillProgramsCharacterBuLianShiBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidBoundaryBuLianShiAnxuLeastSelection(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreSkillProgramsCharacterCaoZhenBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsBoundarySidiChoiceResult(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramChoiceResultBinding binding);
}

internal interface ICoreSkillProgramsCharacterHuangQuanMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Action<global::CardGame.Core.CardMovementRecord> NativeGiveCompletion(long producer, int giver, int recipient, global::System.Collections.Generic.IReadOnlyList<int> ids, long before, global::System.Action<global::CardGame.Core.CardMovementRecord>? original = default);
}

internal interface ICoreSkillProgramsSkillRuntimeStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.SkillRuntimeStateStore _skillRuntimeState { get; }
}

internal interface ICoreSkillProgramsCoreTieredRoundZeroResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertTieredRoundResponseUses();
}

internal interface ICoreSkillProgramsCoreAdvancedWeaponsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWeaponAbility(global::CardGame.Core.CharacterState player, global::CardGame.Core.CardKind kind);
}

internal interface ICoreSkillProgramsCharacterXiahouShiMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasPaidMouXiahouShiSelection(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreSkillProgramsCharacterXiaoQiaoLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasPaidWeiwanSelection(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreSkillProgramsNativeCardGameCoreProgramLegalityRouterOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramLegalityRouter CreateProgramLegalityRouter(global::CardGame.Core.ProgramLegalityCatalog catalog, global::System.Func<global::System.Reflection.MethodInfo, object?>? resolveTarget = default);
}

internal interface ICoreSkillProgramsCoreCappedHandRefreshProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertCappedHandRefresh(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused);
}

internal interface ICoreSkillProgramsCoreOffTurnUsedCardObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertOffTurnUsedCardGiftObservers();
}

internal interface ICoreSkillProgramsCharacterLiuHongOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertYujuePending(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused);
}

internal interface ICoreSkillProgramsCharacterZhuZhiBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasExactBoundaryAnguoRescueTarget(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreSkillProgramsCharacterZhuZhiAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsAnniversaryZhuZhiChoiceResult(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramChoiceResultBinding binding);
}

internal interface ICoreSkillProgramsKernelCoreGameEngineProgramAttackAttemptOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.ProgramAttackHandle CreateProgramAttackHandle(long ownerFrameId);
}

internal interface ICoreSkillProgramsCharacterZhangFeiWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsOriginalDyingSuspendedByWuZishengMovement(global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreSkillProgramsCharacterGuYongBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsBoundaryGuYongChoiceResult(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramChoiceResultBinding binding);
}

internal interface ICoreSkillProgramsCoreProgramOwnedCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertProgramOwnedCardSelection(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused);
}

internal interface ICoreSkillProgramsCoreFieldEquipmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.EquipmentSlot FieldEquipmentSlot(global::CardGame.Core.Card card);
}

internal interface ICoreSkillProgramsCoreUniqueHpTargetAnnouncementsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertUniqueHpTargetAnnouncements();
}

internal interface ICoreSkillProgramsCoreShenwuExpandedPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasExactShenwuYanyuParticipant(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreSkillProgramsKernelCoreGameEngineRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceToHumanBoundary();
}

internal interface ICoreSkillProgramsCoreCurrentTurnHandExchangeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCurrentTurnHandExchangeMovement(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending);
}

internal interface ICoreSkillProgramsCoreLostHpOwnedGiftsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame? LostHpOwnedGiftObserverRoot();
}

internal interface ICoreSkillProgramsCoreFinalizedCardTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginEquipmentTargetPrograms(global::CardGame.Core.CharacterState source, global::CardGame.Core.Card equipment, long frameId);
}

internal interface ICoreSkillProgramsCoreEquipmentContentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.EquipmentContentRegistry EquipmentRules { get; }
}

internal interface ICoreSkillProgramsCharacterQianZhaoOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasPaidWeifuInitialSelection(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreSkillProgramsCoreOwnedDeathBenefitReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ExactOwnedDeathBenefitReturn(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreSkillProgramsCoreHpConditionalSkillGrantsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame CaptureHpConditionalProgramSource(global::CardGame.Core.ProgramSkillFrame frame);
    bool CanContinueHpConditionalProgram(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreSkillProgramsCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreSkillProgramsCharacterZhangRenMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasExactShiZrFuanStrikeTarget(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreSkillProgramsCoreDeckProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertDeckProgramDrafts(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused);
}

internal interface ICoreSkillProgramsProgramCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramCompletionRuntime Get();
}

internal interface ICoreSkillProgramsCoreNextActualUseTargetAdjustmentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramNextActualUseAdjustment? FreezeNextActualUseProgramSelection(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramExecutionPlan plan, global::System.Collections.Generic.IReadOnlyList<int> targets);
    void PrepareNextActualUseSelectedRecovery(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramMultiCardViewAsSelection selection);
    global::CardGame.Core.SkillProgramStepOutcome BeginNextActualUseSelectedSlash(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramMultiCardViewAsSelection selection);
    bool HasExactNextActualUseAdjustedProgramReturn(global::CardGame.Core.CardUseFrame use);
    bool HasExactNextActualUseProgramSelection(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramExecutionPlan plan);
}

internal interface ICoreSkillProgramsCharacterCaoPiBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsBoundaryCaoPiChoiceResult(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramChoiceResultBinding b);
}

internal interface ICoreSkillProgramsCoreProgramLostHpChainedPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertLostHpChainedPayment(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreSkillProgramsPendingCardsMovedBatchesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.CardMovementBatchContext> _pendingCardsMovedBatches { get; }
}

internal interface ICoreSkillProgramsCoreTriggeredCardProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertProgramEquipmentColorDiscardChoice(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect);
}

internal interface ICoreSkillProgramsCoreProgramParticipantPhaseOperationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanActivateTargetPhaseLedger(int owner, string skill, string usage, int target);
}

internal interface ICoreSkillProgramsCoreSlashTargetBenefitObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertSlashTargetBenefitProgram(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreSkillProgramsCoreNativeDrawThresholdOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasExactNativeDrawThresholdSelection(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreSkillProgramsCoreHealthQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> HappyQiaoActionQueryUnion(global::System.Func<global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction>> query);
}

internal interface ICoreSkillProgramsCoreCompletedCardGiftsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidCompletedGiftTargetSelection(global::CardGame.Core.ProgramSkillFrame frame);
    void AssertCompletedCardGiftDraft(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused);
}

internal interface ICoreSkillProgramsProgramLegalityOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref global::CardGame.Core.ProgramLegalityRouter? _programLegality { get; }
}

internal interface ICoreSkillProgramsCoreProgramDiscardChallengesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ValidateProgramDiscardChallengeState(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreSkillProgramsCoreNativeEquipmentAndFactionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanUseProvidedSlashTarget(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.Suit? physicalSuit = default, bool allowAnyPhysicalSuit = true, bool? effectiveColor = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default);
}

internal interface ICoreSkillProgramsCharacterChenDaoMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMobileShiChenDaoChoiceResult(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramChoiceResultBinding b);
}

internal interface ICoreSkillProgramsCoreNativeTargetLegalityOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanUseVirtualSlashTarget(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind effectiveKind = (global::CardGame.Core.CardKind)0, global::CardGame.Core.Suit? physicalSuit = default, bool? effectiveColor = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, bool ignoreDistance = false, global::CardGame.Core.CardConversionSource? conversionSource = default, bool actualUnconvertedPhysical = false, bool countsTowardLimit = true);
}

internal interface ICoreSkillProgramsCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreSkillProgramsCoreAppliedDamageBenefitObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertAppliedDamageBenefitReceipt(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreSkillProgramsCharacterZhaoYunOlGod2016Operations : global::CardGame.Core.IProgramRuntimeOperations
{
    void IssueHpScaledProgramSelection(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramActivation a);
    bool HpScaledProgramSelectionIsExact(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreSkillProgramsCoreNativeRecoveryCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveRecoveryCard(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card card, string cardName, global::CardGame.Core.CardKind? playedCardKind = default, int recoveryAmount = 1, global::System.Collections.Generic.IReadOnlyList<(string SkillId, string PolicyId)>? recoveryPolicySources = default, global::CardGame.Core.CardConversionSource? conversionSource = default, global::CardGame.Core.DyingResponseEvent? dyingResponse = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? physicalCards = default, global::CardGame.Core.DrawFundedDistinctBasicPayment? drawFundedPayment = default, global::CardGame.Core.HuanpeiPaidOrigin? huanpeiOrigin = default);
}

internal interface ICoreSkillProgramsCoreNativeDamageApplicationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ApplyAttackDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreSkillProgramsCoreProgramActionModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramActionRouter ProgramActions { get; }
}

internal interface ICoreSkillProgramsCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame);
    void AdvanceRuntimeProgram(long frameId);
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void ReturnRuntimeProgramMovement(long frameId);
}

internal interface ICoreSkillProgramsCoreNativeLogicalDiscardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TracksNativeLogicalDiscards { get; }
}

internal interface ICoreSkillProgramsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreSkillProgramsCoreCurrentSlashFireOffersOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertCurrentSlashFirePolicy(global::CardGame.Core.CardUseFrame use);
    void AssertCurrentSlashFireDraft(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreSkillProgramsCharacterWangCanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDenglouNativeUse(global::CardGame.Core.CardUseFrame use);
    void AssertDenglouNativeUse(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreSkillProgramsCharacterLeiBoAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsAnniversaryLeiBoFactionChoiceResult(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramChoiceResultBinding b);
}

internal interface ICoreSkillProgramsCoreNativeDamageCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteDamageAttack(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreSkillProgramsCoreCardMovementProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCardsMovedProgramWindow(long? instructionFrameId = default, int? subjectSeat = default);
}

internal interface ICoreSkillProgramsCoreEquipmentPairPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidEquipmentPairProgramSelection(global::CardGame.Core.ProgramSkillFrame frame, bool awaitingSelection);
}

internal interface ICoreSkillProgramsCoreParticipantHandProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ValidateParticipantHandDrafts(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused);
}

internal interface ICoreSkillProgramsCharacterHuaTuoHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasExactHappyHuaTuoParticipantSelection(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreSkillProgramsCoreDistinctFactionDiscardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertDistinctFactionDiscardDraft(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect paused);
}

internal interface ICoreSkillProgramsCoreProgramTargetCommitOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertProgramTargetDraft(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused);
}

internal interface ICoreSkillProgramsCoreStrategicProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertStrategicProgramSelection(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused);
}

internal interface ICoreSkillProgramsCorePopulationTopReorderProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ValidatePopulationTopReorder(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreSkillProgramsCoreNativeRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
}

internal interface ICoreSkillProgramsCharacterBianFuRenOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordBianFuRenOwnedTransferProducer(global::CardGame.Core.CardMovementRecord movement, int recipientSeat, long producerId);
}

internal interface ICoreSkillProgramsCoreActualPhaseEndsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasActualPhaseEndBoundaryFrame();
}

internal interface ICoreSkillProgramsCoreRoundPricedPileAlcoholOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertRoundPileAlcoholUse(global::CardGame.Core.CardUseFrame use);
    void AssertRoundPileAlcohol(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreSkillProgramsCharacterChengPuMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMobileShiChengPuChoiceResult(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramChoiceResultBinding binding);
}

internal interface ICoreSkillProgramsCoreDamageCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DamageTriggerWindowFrame? ActiveDamageTrigger { get; }
}

internal interface ICoreSkillProgramsCoreProgramMarkerPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PayProgramMarkerCost(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramMarkerCost? cost, string skillId, string bindingId);
}

internal interface ICoreSkillProgramsCoreFixedSlashAndDeclaredDeckProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertFixedSlashAndDeclaredDeckDrafts(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused);
}

internal interface ICoreSkillProgramsCoreRecipientContestReceiptsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertPindianWinnerUses();
}

internal interface ICoreSkillProgramsCharacterDianWeiHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasExactHappyDianWeiElaiSelection(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreSkillProgramsCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player);
}

internal interface ICoreSkillProgramsCoreCardConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramMultiCardViewAsSelection? FindProgramMultiCardViewAsSelection(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int> cardIds, global::CardGame.Core.CardKind outputKind, bool forResponse, global::CardGame.Core.CardConversionSource source);
}

internal interface ICoreSkillProgramsCoreProgramAttackRangeAidOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertProgramAttackRangeAid(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused);
}

internal interface ICoreSkillProgramsCharacterZhouFeiLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasPaidLeZhouFeiXianshuSelection(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreSkillProgramsCoreNativeDrawThresholdDyingOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryDescribeNativeDrawThresholdDyingDamage(global::CardGame.Core.ProgramSkillFrame frame, int target, int amount, global::CardGame.Core.ProgramParticipantReference? source, global::CardGame.Core.DamageNature? nature, bool sourceLess, int? explicitSource, out global::CardGame.Core.NativeDrawThresholdDyingOrigin? origin);
}

internal interface ICoreSkillProgramsCharacterLiuYanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasPaidLiuYanInitialSelection(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreSkillProgramsCoreExactTopReorderProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ValidateExactTopReorder(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreSkillProgramsCoreDeferredBenefitsProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ValidateFactionRecoveryDraft(global::CardGame.Core.ProgramSkillFrame frame);
    void ValidateWeaponDamageDraft(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreSkillProgramsCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICoreSkillProgramsCoreHpLossSlashMaterialsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.HpLossSlashSelection? FreezeHpLossSlashSelection(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramExecutionPlan plan, global::System.Collections.Generic.IReadOnlyList<int> targets);
    global::CardGame.Core.SkillProgramStepOutcome BeginHpLossSelectedSlash(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramMultiCardViewAsSelection selection);
    bool HasExactHpLossSlashSelection(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramExecutionPlan plan);
}

internal interface ICoreSkillProgramsCoreCurrentCardEnhancementsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertCurrentCardEnhancementDraft(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused);
}

internal interface ICoreSkillProgramsCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreSkillProgramsCoreTurnDefaultStatsValidationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertTurnDefaultStatState();
}

internal interface ICoreSkillProgramsCoreLiejieSourceDiscardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertLiejieSourceDiscard(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused);
}

internal interface ICoreSkillProgramsCorePublicPersistentPilesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertPublicPileDraft(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused);
}

internal interface ICoreSkillProgramsCoreProgramPrivateDamageOfferOperationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertPrivateOfferDrafts(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused);
}

internal interface ICoreSkillProgramsCharacterHuaTuoAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsProgramWaitingForSkillInvocation(global::CardGame.Core.ProgramSkillFrame program);
}

internal interface ICoreSkillProgramsCoreActiveNativeAttackQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get; set; }
}

internal interface ICoreSkillProgramsCoreOrderedPrintedSkillLossObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertOrderedPrintedSkillLossState();
    void AssertOrderedPrintedSkillLossObservers();
}

internal interface ICoreSkillProgramsCharacterZhangNiBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsBoundaryWurongChoiceResult(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramChoiceResultBinding b);
}

internal interface ICoreSkillProgramsCorePindianOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginSharedPindian(long parentId, global::CardGame.Core.SkillPromptPresentation presentation, int sourceSeat, int? opponentSeat = default, int? sourceCardId = default, string? programResultBind = default, global::CardGame.Core.SkillProgramCardSetVisibility programResultVisibility = (global::CardGame.Core.SkillProgramCardSetVisibility)1, global::CardGame.Core.PindianOwnedPileSource? ownedPileSource = default);
}

internal interface ICoreSkillProgramsCoreProgramSelectionValidationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSelectionValidationRegistry ProgramSelectionValidators { get; }
}

internal interface ICoreSkillProgramsCoreDeferredCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertParticipantTopView(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused);
}

internal interface ICoreSkillProgramsCoreHalfHandPhaseDebtReceiptsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidPhaseHandDebtPayment(global::CardGame.Core.ProgramSkillFrame f);
    void AssertHalfHandPhaseDebt(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreSkillProgramsCoreDyingSuitsAndHistoricalUseObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertHistoricalEndingUse(global::CardGame.Core.CardUseFrame use);
    bool IsOriginalDyingSuspendedByDyingSuits(global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreSkillProgramsCoreRoundOnceCardlessNativeUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsRoundOnceCardlessBasicUse(long id);
}

internal interface ICoreSkillProgramsCoreProgramVirtualSlashOffersOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertProgramVirtualSlashOffer(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect);
}

internal interface ICoreSkillProgramsCoreCardUseModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardUseForbidden(int actorSeat, global::CardGame.Core.CardKind effectiveKind, global::CardGame.Core.CardActionType actionType, bool ignoreIssuedPlayBan = false, long? existingUseFrameId = default);
    bool HasTurnCardTargetRestriction(int actorSeat, global::CardGame.Core.SkillProgramCardTargetRestriction restriction, int? targetSeat = default);
}

internal interface ICoreSkillProgramsCharacterDaQiaoLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasPaidZixiPlacement(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreSkillProgramsCoreKuangfuReceiptsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasExactKuangfuProgramSelection(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramExecutionPlan plan);
}

internal interface ICoreSkillProgramsCharacterChenQunBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasPaidBoundaryChenQunInitialSelection(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreSkillProgramsCharacterLuYuShengMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMobileShiLuYuShengChoiceResult(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramChoiceResultBinding binding);
}

internal interface ICoreSkillProgramsCharacterBuLianShiMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMobileBuLianShiAnxuEqualityResult(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramChoiceResultBinding binding);
}

internal interface ICoreSkillProgramsCoreRecipientCategoryMarkObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertRecipientCategoryMarkState();
}

internal interface ICoreSkillProgramsCoreNativeDyingOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> BuildDyingResponderSeats(int victimSeat);
    void ExposeHumanDyingPrompt();
}

internal interface ICoreSkillProgramsCharacterHuaXiongMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouHuaXiongChoiceResult(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramChoiceResultBinding binding);
}

internal interface ICoreSkillProgramsCoreNativeDamageHandlesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? GetJudgmentAttack(global::CardGame.Core.JudgmentFrame frame);
}

internal interface ICoreSkillProgramsCoreDyingOwnedCardSelectionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidDyingOwnedCardProgramSelection(global::CardGame.Core.ProgramSkillFrame frame, bool awaitingSelection);
}

internal interface ICoreSkillProgramsCoreNamedCardAcquisitionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertNamedCardAcquisition(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreSkillProgramsCoreDeferredHandAlignmentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsExactDeferredChild(global::CardGame.Core.ProgramSkillFrame child);
}

internal interface ICoreSkillProgramsCharacterLuKangWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool WuLuKangActivationTargetHasPindianSource(global::CardGame.Core.ProgramExecutionPlan plan, global::CardGame.Core.CharacterState target);
    bool WuLuKangCanOmitPindianSource(int owner, global::CardGame.Core.SkillProgramActivation activation);
    global::CardGame.Core.ProgramSkillFrame CaptureWuLuKangPindianSelection(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramActivation activation);
    void IssueWuLuKangPindianSelection(global::CardGame.Core.ProgramSkillFrame f);
    bool HasWuLuKangPindianSelection(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramActivation a);
    bool HasWuLuKangPindianInput(global::CardGame.Core.CharacterState owner);
}

internal interface ICoreSkillProgramsCharacterKongShuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertLeiluanPayments(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreSkillProgramsCoreJudgmentCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.JudgmentFrame? ActiveJudgment { get; }
}

internal interface ICoreSkillProgramsCoreNativeCardUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long BeginCardUse(global::CardGame.Core.Card card, int sourceSeat, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, global::CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default, global::System.Collections.Generic.IReadOnlyList<int>? designatedTargetSeats = default, bool isTrueZhangbaSlash = false, global::CardGame.Core.ProgramSelectedActorDuelOrigin? selectedActorDuelOrigin = default, global::CardGame.Core.ProgramDamageTargetDuelOrigin? damageTargetDuelOrigin = default, global::CardGame.Core.ProgramDualColorDuelOrigin? dualColorDuelOrigin = default, global::CardGame.Core.ConditionalDiscardDuelOrigin? conditionalDiscardDuelOrigin = default, global::CardGame.Core.ShiQuanDuelOrigin? shiQuanDuelOrigin = default, global::CardGame.Core.ProgramVirtualOrdinaryTrickOrigin? virtualOrdinaryTrickOrigin = default, global::CardGame.Core.DrawFundedDistinctBasicPayment? drawFundedPayment = default, global::CardGame.Core.ChainedStateBasicPayment? chainedStatePayment = default, global::CardGame.Core.FriendRecastDuelOrigin? friendRecastDuel = default, global::CardGame.Core.ChosenAssaultReturn? chosenAssault = default, global::CardGame.Core.ZhenyingDuelOrigin? zhenyingDuel = default, global::CardGame.Core.HeJinMouzhuUseOrigin? heJinMouzhu = default, global::CardGame.Core.WuAnGuoLiyongDuelOrigin? wuAnGuoLiyong = default, global::CardGame.Core.TianChouShandaoUseOrigin? tianChouShandao = default, global::CardGame.Core.QinLangXianyingUseOrigin? qinLangXianying = default, global::CardGame.Core.AHuiNanJuemanUseOrigin? aHuiNanJueman = default, global::CardGame.Core.LiuYanLimuUseOrigin? liuYanLimu = default, global::CardGame.Core.JueshengDuelOrigin? jueshengDuel = default, global::CardGame.Core.QuHuangNishouUseOrigin? quHuangNishou = default, global::CardGame.Core.WeijiePaidOrigin? weijieOrigin = default, global::CardGame.Core.ZonghuPaidOrigin? zonghuOrigin = default, global::CardGame.Core.HuanpeiPaidOrigin? huanpeiOrigin = default);
}

internal interface ICoreSkillProgramsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceRulesAndPublishState();
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreSkillProgramsCoreResponseEntityExchangeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertResponseEntityExchange(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused);
}

internal interface ICoreSkillProgramsCharacterGuanHaiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsXieweiSelection(global::CardGame.Core.ProgramMultiCardViewAsSelection selection);
}

internal interface ICoreSkillProgramsCharacterLingTongBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidBoundaryLingTongEquipmentSelection(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreSkillProgramsCoreFriendTrioOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool FriendXuanjianIgnoresDistance(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardConversionSource? source);
}

internal interface ICoreSkillProgramsCoreParticipantAndReserveProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertParticipantReserveDraft(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused);
}

internal interface ICoreSkillProgramsCharacterLuMengHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasExactHappyGongxinParticipantSelection(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreSkillProgramsCorePlayCardHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card ApplyProgramUseAppearance(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardConversionSource? source);
}

internal interface ICoreSkillProgramsCoreProgramCardPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int ResolveProgramParticipant(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramParticipantReference reference);
}

internal interface ICoreSkillProgramsCoreGuanduGeneralNativeCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsGuanduChoiceResult(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramChoiceResultBinding binding);
}

internal interface ICoreSkillProgramsCoreFinalTargetGiftsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertFinalTargetGift(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect);
}

internal interface ICoreSkillProgramsCoreRecoveryDamageEdgesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryDescribeRecoveryDamage(global::CardGame.Core.ProgramSkillFrame frame, int target, int amount, global::CardGame.Core.ProgramParticipantReference? sourceReference, global::CardGame.Core.DamageNature? nature, bool sourceLess, int? explicitSourceSeat, out global::CardGame.Core.RecoveryDamageOrigin? origin, out long? damageWindowId);
}

internal interface ICoreSkillProgramsCoreWushuangModeProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWushuangMode { get; }
}

internal interface ICoreSkillProgramsCoreProgramHoldCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertProgramHoldCardSelection(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused);
}

internal interface ICoreSkillProgramsCoreOriginalHandEntityObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertOriginalHandEntityState();
}

internal interface ICoreSkillProgramsCoreVirtualBasicOffersOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertVirtualBasicDraft(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect);
}

internal interface ICoreSkillProgramsCoreRecipientCategoryMarkContinuationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsOriginalDyingSuspendedByRecipientCategoryMark(global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreSkillProgramsCoreRoundDistinctBasicUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertRoundDistinctBasicUses(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreSkillProgramsCoreProgramHandControlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertProgramHandControlDraft(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused);
}

internal interface ICoreSkillProgramsCoreGuanHaiBridgesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit? ProgramMultiCardSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramMultiCardViewAsSelection selection);
    bool? ProgramMultiCardColor(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramMultiCardViewAsSelection selection);
}

internal interface ICoreSkillProgramsCharacterWangYunOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidWangYunNativeUse(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreSkillProgramsCharacterCaoChunOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertCaoChunNativeUse(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreSkillProgramsCoreHellKingProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsHellKingChoiceResult(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramChoiceResultBinding binding);
    bool HasExactHellKingParticipantSelection(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreSkillProgramsCoreOwnedExactCountAndNamedTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertProgramNamedBoundTargetSelection(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused);
}

internal interface ICoreSkillProgramsCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CommandError? ValidateHumanPrompt(int actorSeat, global::CardGame.Core.DecisionKind expectedKind, global::CardGame.Core.PromptId? promptId, global::CardGame.Core.CommandErrorCode inactiveCode);
    void ClearPendingDecision();
}

internal interface ICoreSkillProgramsCoreShenwuNpcProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsShenwuChoiceResult(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramChoiceResultBinding binding);
    bool HasExactShenwuBoundParticipantSelection(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreSkillProgramsCharacterSunLuBanBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsBoundarySunLuBanChoiceResult(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramChoiceResultBinding binding);
}

internal interface ICoreSkillProgramsCoreProgramOwnedCardDistributionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertProgramOwnedCardDistribution(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused);
}

internal interface ICoreSkillProgramsCoreProgramDiscardTopPlacementOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertProgramDiscardTopPlacement(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreSkillProgramsCoreTeamActivityProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsExactPublicTeamTargetSelection(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect selection, global::CardGame.Core.ProgramExecutionPlan plan);
}

internal interface ICoreSkillProgramsCoreDiamondDelayedProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertDiamondDelayed(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect? paused);
}

internal interface ICoreSkillProgramsCoreRoundOnceCardlessProgramRequestsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertRoundOnceCardlessProgramRequests();
}

internal interface ICoreSkillProgramsCoreOriginalTargetAdditionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertOriginalTargetAdditionDraft(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused);
}

internal interface ICoreSkillProgramsCoreProgramDamageInsertionRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramDamageInsertionRuntime DamageInsertions { get; }
}

internal interface ICoreSkillProgramsCoreProgramCategoryTargetLedgerOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanActivateCategoryTargetLedger(int ownerSeat, string skillId, string usageId, global::System.Collections.Generic.IReadOnlyList<int> cardIds, global::System.Collections.Generic.IReadOnlyList<int> targetSeats);
}

internal interface ICoreSkillProgramsCoreFinalTargetTurnCountsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertNamedTurnFlow(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect);
}

internal interface ICoreSkillProgramsCoreGhostActivityProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidRedJudgmentFireSelection(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect, bool awaiting);
}

internal interface ICoreSkillProgramsKernelCoreProgramRuntimeModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    object? ResolveRuntimeHandlerTarget(global::System.Reflection.MethodInfo method);
}

internal interface ICoreSkillProgramsCharacterGuoSiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsGuoSiNativeUse(global::CardGame.Core.CardUseFrame use);
    void AssertGuoSiNativeUse(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreSkillProgramsCoreProgramPersistentStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool GetProgramBooleanState(int ownerSeat, string skillId, string skillInstanceId, string stateId);
}

internal interface ICoreSkillProgramsCoreTieredRoundZeroReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertCappedConversionBenefits();
    bool IsCappedConversionAwaitedMovement(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramMovementContinuation pending);
}
