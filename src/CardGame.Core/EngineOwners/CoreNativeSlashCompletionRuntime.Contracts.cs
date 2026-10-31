#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreNativeSlashCompletionCharacterLuMengMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginYingboGift(long resolutionId, int sourceSeat, global::CardGame.Core.Card card, global::CardGame.Core.CardKind cardKind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> physicalCards, global::CardGame.Core.YingboGiftContinuation continuation, global::CardGame.Core.NativeOwners.CardAttackHandle? attack = default, global::CardGame.Core.NativeOwners.GroupCardHandle? group = default);
}

internal interface ICoreNativeSlashCompletionCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next);
    void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind);
}

internal interface ICoreNativeSlashCompletionCharacterXinPiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsXinPiPhysicalClaim(long useId, int cardId);
}

internal interface ICoreNativeSlashCompletionCharacterGuanHaiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsXieweiRemovedMaterial(long useId, int cardId);
}

internal interface ICoreNativeSlashCompletionCoreCardActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginProgramCardWindow(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<int> opponents, global::CardGame.Core.ProgramCardContinuation? continuation, bool? cardUseCausedDamage = default, global::CardGame.Core.ProgramTrickContinuation? trickContinuation = default, global::CardGame.Core.ProgramSimpleCardContinuation? simpleContinuation = default, global::CardGame.Core.ProgramCompletedResponseReturn? completedResponseReturn = default, global::CardGame.Core.CaoChunHandUsePayment? caoChunPayment = default, bool caoChunDebtOnly = false);
}

internal interface ICoreNativeSlashCompletionCoreForeignSelectedCardSlashReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSelectedForeignCardSlashRemovedMaterial(long useId, int cardId);
}

internal interface ICoreNativeSlashCompletionResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreNativeSlashCompletionCoreShanheS5XuncanOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsShanheS5YunshenUse(long useId);
}

internal interface ICoreNativeSlashCompletionCoreCurrentUsePhysicalClaimsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCurrentUsePhysicalCardClaim(long frameId, int cardId);
}

internal interface ICoreNativeSlashCompletionCharacterMiHengLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSiruiOutsideGeneralWeaponMaterial(long useId, int cardId);
}

internal interface ICoreNativeSlashCompletionCoreOriginalUseLightningOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool FinishOriginalUseLightningAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreNativeSlashCompletionCharacterYangFuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSuyiSlashUse(long id);
}

internal interface ICoreNativeSlashCompletionCoreNativeSlashTargetDuelOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsNativeSlashTargetDuelAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreNativeSlashCompletionCharacterXunXuAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryFinishAnniversaryXunXuUseMaterials(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNativeSlashCompletionCoreForeignPublicPileSlashesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsForeignPublicPileSlashUse(long resolutionId);
}

internal interface ICoreNativeSlashCompletionCoreNativeCardUseFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetCardUseStep(long frameId, global::CardGame.Core.ResolutionFrameStep step);
}

internal interface ICoreNativeSlashCompletionCharacterZhangYanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsZhangYanSujiRemovedMaterial(long useId, int cardId);
}

internal interface ICoreNativeSlashCompletionCoreDamageTargetObtainProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsIssuedZeroEntityDuel(long id);
}

internal interface ICoreNativeSlashCompletionCoreKenshangPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsKenshangRemovedMaterial(long useId, int cardId);
}

internal interface ICoreNativeSlashCompletionCoreGuanduEquipmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsGuanduDestroyedChariotMaterial(long useId, int cardId);
}

internal interface ICoreNativeSlashCompletionCharacterDianWeiMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouDianWeiSlashUse(long id);
}

internal interface ICoreNativeSlashCompletionCharacterYaDanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsIssuedQingyaUse(long id);
}

internal interface ICoreNativeSlashCompletionCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreNativeSlashCompletionCoreActualHandGainProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame NormalizeCompletedCategoryVirtualUse(global::CardGame.Core.CardUseFrame use, long? programParentFrameId);
}

internal interface ICoreNativeSlashCompletionCharacterWangChangAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWangChangSlash(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNativeSlashCompletionCoreHellSoulCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsHellDestroyedSoulMaterial(long useId, int cardId);
}

internal interface ICoreNativeSlashCompletionCharacterWuJingOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWuJingHejiZeroUse(global::CardGame.Core.CardUseFrame use);
    bool IsWuJingHejiRemovedMaterial(long useId, int cardId);
}

internal interface ICoreNativeSlashCompletionKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreNativeSlashCompletionCoreActualTurnLegacyCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame NormalizeActualTurnLegacyCompletedUse(global::CardGame.Core.CardUseFrame use, long? programParentId);
}

internal interface ICoreNativeSlashCompletionCharacterMengDaOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsGoudeSlashUse(long useId);
}

internal interface ICoreNativeSlashCompletionCharacterLiYiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasIssuedChanshuangSlash(long? useId, int actor);
}

internal interface ICoreNativeSlashCompletionCoreNativeCardUseCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PopFinishedCardUse(long frameId);
    void CompleteFinishedAttackCardUse(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
    void QueueCardUseFinishedWithoutPop(long frameId, global::CardGame.Core.Card card, global::CardGame.Core.CardKind? playedCardKind);
}

internal interface ICoreNativeSlashCompletionCharacterHuBanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsHuiyunOutsideGeneralWeaponMaterial(long useId, int cardId);
}

internal interface ICoreNativeSlashCompletionCoreCurrentSlashFireOffersOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCurrentSlashFireChangedUse(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNativeSlashCompletionCoreEphemeralCardMaterialsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsEphemeralCardDestroyedMaterial(int cardId, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to);
    bool IsEphemeralCardDestroyedMaterial(long useId, int cardId);
}

internal interface ICoreNativeSlashCompletionCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
}

internal interface ICoreNativeSlashCompletionCoreProgramDamageHooksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramDamageHooks DamageHooks { get; }
}

internal interface ICoreNativeSlashCompletionCharacterSunYuanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSayingRemovedMaterial(long useId, int cardId);
}

internal interface ICoreNativeSlashCompletionCharacterXuRongOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsXuRongCausalEntityClaim(long nativeOwnerId, int cardId);
}

internal interface ICoreNativeSlashCompletionCorePhysicalMovementFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
}

internal interface ICoreNativeSlashCompletionKernelCoreMainOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteGame();
}

internal interface ICoreNativeSlashCompletionCharacterHuaManOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsHuaManClaimedEntity(long useId, int cardId);
}

internal interface ICoreNativeSlashCompletionCoreOriginalTargetAdditionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryGetOriginalTargetVirtualSlashReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramSkillFrame parent, out global::System.Collections.Generic.IReadOnlyList<int> primaryTarget);
}
