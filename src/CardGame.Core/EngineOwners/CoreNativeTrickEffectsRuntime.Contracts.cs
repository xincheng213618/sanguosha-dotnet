#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreNativeTrickEffectsCoreNativeNullificationFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetNullificationEffectCard(global::CardGame.Core.NullificationWindowFrame frame);
}

internal interface ICoreNativeTrickEffectsCoreNativeTrickUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetCardUsePhysicalCards(long resolutionId);
}

internal interface ICoreNativeTrickEffectsCharacterWuJingOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long? WuJingHejiProgramParent(long id);
}

internal interface ICoreNativeTrickEffectsCharacterDiaoChanLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool SkipDiouFinishedMovement(long id, global::CardGame.Core.Card c);
}

internal interface ICoreNativeTrickEffectsCharacterLuXunMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ResolveMouLuXunMultiIronChain(global::CardGame.Core.NullificationWindowFrame pending);
}

internal interface ICoreNativeTrickEffectsCoreExtraEffectForeignSelectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginExtraForeignSelection(global::CardGame.Core.NullificationWindowFrame pending, global::CardGame.Core.CharacterState target);
    bool TryResolveExtraForeignSelection(global::CardGame.Core.TargetCardSelectionFrame pending, int slot);
}

internal interface ICoreNativeTrickEffectsCoreNativeGroupAttackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginGroupAttackResponse(global::CardGame.Core.NativeOwners.GroupCardHandle group);
}

internal interface ICoreNativeTrickEffectsCharacterZhugeGuoLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsLeChengyanEffectUse(long id);
}

internal interface ICoreNativeTrickEffectsCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.GroupCardHandle? ActiveGroupCard { get; set; }

    global::CardGame.Core.TargetCardSelectionFrame? ActiveTargetCardSelection { get; }

    global::CardGame.Core.CardUseFrame? ActiveFireAttack { get; }

    global::CardGame.Core.NativeOwners.BorrowedSwordHandle? ActiveBorrowedSword { get; set; }

    global::CardGame.Core.NativeOwners.DuelHandle? ActiveDuel { get; set; }
}

internal interface ICoreNativeTrickEffectsCorePhysicalMovementFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
}

internal interface ICoreNativeTrickEffectsCoreNativeLogicalDiscardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeLogicalDiscardInvocationDraft? CaptureNativeLogicalDiscardInvocation(int initiatorSeat, int executorSeat, global::System.Collections.Generic.IReadOnlyList<int> cardIds, global::CardGame.Core.CardMoveReason reason, long? parentFrameId = default);
    void CompleteNativeLogicalDiscardInvocation(global::CardGame.Core.NativeLogicalDiscardInvocationDraft? draft, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord>? prepaidMaterials = default);
}

internal interface ICoreNativeTrickEffectsCoreExtraCardEffectRoundsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RestoreExtraGroupDamageSource(global::CardGame.Core.NativeOwners.GroupCardHandle group);
    void CaptureExtraGroupDamageSource(global::CardGame.Core.NativeOwners.GroupCardHandle group);
    bool IsExtraCardEffectRound(long id);
    bool HasUnstartedExtraCardEffect(long id);
}

internal interface ICoreNativeTrickEffectsCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICoreNativeTrickEffectsCoreEquipmentSlotGroupProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ResolveEnteringChain(global::CardGame.Core.CharacterState player, bool requested);
}

internal interface ICoreNativeTrickEffectsCoreNativeBorrowedSwordOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginBorrowedSwordSlashChoice(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending, bool includeFactionSlash, bool keepCurrentRequest = false);
}

internal interface ICoreNativeTrickEffectsCharacterWuAnGuoOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWuAnGuoLiyongDuelUse(long id);
}

internal interface ICoreNativeTrickEffectsCharacterFanJiangZhangDaOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsJueshengDuelUse(long id);
    long? JueshengDuelProgramParent(long id);
}

internal interface ICoreNativeTrickEffectsCharacterXiaoQiaoMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool SkipMouXqJueyanFinishedMovement(long id, global::CardGame.Core.Card c);
}

internal interface ICoreNativeTrickEffectsCoreNativeCardEffectDrawsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginNativeDrawTwo(global::CardGame.Core.NullificationWindowFrame pending);
}

internal interface ICoreNativeTrickEffectsCoreNativeCardUseCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinishCardUse(long frameId, global::CardGame.Core.Card card, global::CardGame.Core.CardKind? playedCardKind = default);
}

internal interface ICoreNativeTrickEffectsCorePhysicalDrawFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> DrawCards(global::CardGame.Core.CharacterState player, int count, bool log, global::CardGame.Core.CardMoveReason? reason = default, global::CardGame.Core.CardConversionSource? directProducer = default);
    global::CardGame.Core.Card? DrawOneToProcessing(global::CardGame.Core.CardMoveReason reason);
}

internal interface ICoreNativeTrickEffectsCoreSnapshotValueProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardSnapshot ToSnapshot(global::CardGame.Core.Card card);
}

internal interface ICoreNativeTrickEffectsCharacterMengHuoMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RestoreMouMengHuoDamageSource(global::CardGame.Core.NativeOwners.GroupCardHandle group);
}

internal interface ICoreNativeTrickEffectsCoreActiveNativeAttackQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get; set; }
}

internal interface ICoreNativeTrickEffectsCoreTargetedOrdinaryTrickOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasTargetedOrdinaryTrickAdjustment { get; }
}

internal interface ICoreNativeTrickEffectsCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreNativeTrickEffectsCharacterQinLangOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsQinLangXianyingUse(long id);
}

internal interface ICoreNativeTrickEffectsCharacterMiHengLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSiruiOutsideGeneralWeaponMaterial(long useId, int cardId);
}

internal interface ICoreNativeTrickEffectsCoreNativeSlashTargetDuelOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryResolveNativeSlashTargetDuel(global::CardGame.Core.NullificationWindowFrame pending);
}

internal interface ICoreNativeTrickEffectsCoreProgramCardTargetEffectsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardEffectIneffective(long resolutionId, int targetSeat);
    void CompleteIneffectiveTrickTarget(global::CardGame.Core.NullificationWindowFrame pending, int targetSeat);
}

internal interface ICoreNativeTrickEffectsCoreCharacterStateProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordCharacterStateChange(int targetSeat, global::CardGame.Core.SkillProgramTriggerWindow window, long? parentFrameId = default, global::CardGame.Core.CharacterTurnedOverState? turnedOver = default);
}

internal interface ICoreNativeTrickEffectsCharacterHaoPuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsZhenyingDuelUse(long id);
}

internal interface ICoreNativeTrickEffectsCoreExtraEffectMaterialClaimsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RestoreExtraEffectGroupMaterialClaims(global::CardGame.Core.NativeOwners.GroupCardHandle group);
}

internal interface ICoreNativeTrickEffectsKernelCoreMainOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.GroupCardHandle CreateGroupCardHandle(long resolutionId, int sourceSeat, global::CardGame.Core.Card card, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, global::CardGame.Core.GroupCardEffect effect, global::CardGame.Core.CardKind? requiredCardKind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? physicalCards = default);
    global::CardGame.Core.NativeOwners.BorrowedSwordHandle CreateBorrowedSwordHandle(long resolutionId, int sourceSeat, int weaponOwnerSeat, int slashTargetSeat, global::CardGame.Core.Card card);
    global::CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long resolutionId, int sourceSeat, int targetSeat, global::CardGame.Core.Card? card, int damageAmount = 1, global::CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, bool isDelayedJudgmentDamage = false, int? delayedJudgmentSeat = default, global::CardGame.Core.DamageNature? damageNatureOverride = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? physicalCards = default, long? programJudgmentFrameId = default, long? programSkillFrameId = default, global::CardGame.Core.CardConversionSource? conversionSource = default, long? programSkillCardUseFrameId = default);
    global::CardGame.Core.NativeOwners.DuelHandle CreateDuelHandle(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreNativeTrickEffectsCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
    void UpdateLifecycleCardUse(long frameId, global::System.Func<global::CardGame.Core.CardUseFrame, global::CardGame.Core.CardUseFrame> update);
}

internal interface ICoreNativeTrickEffectsCoreNativeDuelOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginDuelResponse(global::CardGame.Core.NativeOwners.DuelHandle duel, bool keepCurrentRequest = false);
}

internal interface ICoreNativeTrickEffectsCoreFriendEventsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsFriendRecastDuelUse(long id);
}

internal interface ICoreNativeTrickEffectsCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame);
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind);
}

internal interface ICoreNativeTrickEffectsCharacterXuHuangMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long? MouXuHuangDuelProgramParent(long id);
}

internal interface ICoreNativeTrickEffectsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreNativeTrickEffectsCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    (global::CardGame.Core.Card Card, global::CardGame.Core.CardLocation Location)? FindPublicTargetCard(global::CardGame.Core.CharacterState target, int cardId);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
}

internal interface ICoreNativeTrickEffectsCoreActualTurnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardKind GetJudgmentEffectiveCardKind(global::CardGame.Core.Card card);
}

internal interface ICoreNativeTrickEffectsCharacterHeJinOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsHeJinMouzhuDuelUse(long id);
}

internal interface ICoreNativeTrickEffectsCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreNativeTrickEffectsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreNativeTrickEffectsCoreTargetCardPreselectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long TargetHandMovementSequence(int seat);
}

internal interface ICoreNativeTrickEffectsCoreNativeAiDecisionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void NotifyAiOfGroupAttack(global::CardGame.Core.CharacterState source, global::System.Collections.Generic.IReadOnlyList<int> targets);
    void NotifyAiOfFireAttack(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target);
    void NotifyAiOfDuel(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target);
}

internal interface ICoreNativeTrickEffectsCoreUnexpectedAssaultOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryApplyUnexpectedAssaultTargetCardEffect(long resolutionId, global::CardGame.Core.Card effectCard, global::CardGame.Core.CardKind effectiveCardKind, int sourceSeat, int targetSeat, global::CardGame.Core.LegalActionKind actionKind, global::CardGame.Core.Card targetCard, global::CardGame.Core.CardLocation targetCardFromZone);
}

internal interface ICoreNativeTrickEffectsCoreTieredRoundZeroReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool SkipTieredRoundZeroFinishedMovement(long id, global::CardGame.Core.Card card);
}

internal interface ICoreNativeTrickEffectsCoreWushuangTargetCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BindWushuangTargetCards(global::CardGame.Core.NullificationWindowFrame pending, int targetSeat);
    bool TryContinueWushuangTargetCards(long id, global::CardGame.Core.Card effectCard, global::CardGame.Core.CardKind effectiveKind, int source, int target, global::CardGame.Core.LegalActionKind kind);
}

internal interface ICoreNativeTrickEffectsCoreSequentialTrickTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveFinishedTrickCard(long frameId, global::CardGame.Core.Card card);
    bool HasRemainingSequentialTrickTargets(long frameId);
}

internal interface ICoreNativeTrickEffectsCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default);
}

internal interface ICoreNativeTrickEffectsCoreCurrentUsePhysicalClaimsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsClaimedUseCardEntity(long frameId, int cardId);
}

internal interface ICoreNativeTrickEffectsCoreProgramDrawTwoBonusOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int NativeDrawTwoCount(int sourceSeat);
}

internal interface ICoreNativeTrickEffectsCoreAdvancedWeaponsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MoveProcessingCardUnlessDestroyed(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation destination, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default);
}

internal interface ICoreNativeTrickEffectsCoreResolutionIdentityBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long _resolutionSequence { get; set; }
}

internal interface ICoreNativeTrickEffectsCoreNativeFireAttackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame SetFireAttackSelection(long frameId, global::CardGame.Core.FireAttackSelectionState? selection);
    void BeginFireAttackReveal(global::CardGame.Core.CardUseFrame pending);
}

internal interface ICoreNativeTrickEffectsCharacterZhugeLiangMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsZeroEntityUse(long id);
    bool IsMouZhijueZeroUse(long id);
}

internal interface ICoreNativeTrickEffectsCharacterHuangYueyingOlMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouLixianRemovedMaterial(long useId, int cardId);
}

internal interface ICoreNativeTrickEffectsCoreWushuangCombatTacticsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool WushuangHasVisibleTargetHand(int source, global::CardGame.Core.CardKind kind);
}

internal interface ICoreNativeTrickEffectsCoreNativeCardUseFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetCardUseStep(long frameId, global::CardGame.Core.ResolutionFrameStep step);
}

internal interface ICoreNativeTrickEffectsCoreVirtualOrdinaryTrickUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsProgramVirtualOrdinaryTrickUse(long frameId);
}

internal interface ICoreNativeTrickEffectsCoreForeignEquipmentDiscardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsForeignEquipmentDiscardPrevented(int actorSeat, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.OwnedCardMoveIntent intent);
}

internal interface ICoreNativeTrickEffectsCharacterYangBiaoAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsAnniversaryYangBiaoFiveGrainsUse(long id);
}

internal interface ICoreNativeTrickEffectsCharacterTianChouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsTianChouShandaoUse(long id);
}

internal interface ICoreNativeTrickEffectsCharacterWenChouMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long? MouWenChouDuelProgramParent(long id);
}

internal interface ICoreNativeTrickEffectsCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
}

internal interface ICoreNativeTrickEffectsCoreDamageTargetRegionObtainOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int PrepareDamageTargetDuelDamage(long useId, int targetSeat);
}

internal interface ICoreNativeTrickEffectsCoreNativeHarvestOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinishFiveGrains(global::CardGame.Core.NativeOwners.GroupCardHandle group);
    void BeginHarvestSelection(global::CardGame.Core.NativeOwners.GroupCardHandle group);
}

internal interface ICoreNativeTrickEffectsCoreDamageColorDuelProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetTrickRepresentation(long frameId, int cardId, bool requireProcessing = false);
}
