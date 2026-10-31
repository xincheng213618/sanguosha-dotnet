#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreSequentialTrickTargetsCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.DuelHandle? ActiveDuel { get; set; }
}

internal interface ICoreSequentialTrickTargetsCharacterMiHengLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSiruiOutsideGeneralWeaponMaterial(long useId, int cardId);
}

internal interface ICoreSequentialTrickTargetsCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ClearPendingDecision();
}

internal interface ICoreSequentialTrickTargetsCoreExtraCardEffectRoundsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasUnstartedExtraCardEffect(long id);
}

internal interface ICoreSequentialTrickTargetsCoreActiveNativeAttackQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get; set; }
}

internal interface ICoreSequentialTrickTargetsCharacterZhugeGuoLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool KeepLeChengyanBorrowedMaterial(long id);
}

internal interface ICoreSequentialTrickTargetsCharacterDiaoChanLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDiouZeroUse(long id);
}

internal interface ICoreSequentialTrickTargetsCharacterFanJiangZhangDaOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsJueshengDuelUse(long id);
}

internal interface ICoreSequentialTrickTargetsCoreDamageColorDuelProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetTrickRepresentation(long frameId, int cardId, bool requireProcessing = false);
}

internal interface ICoreSequentialTrickTargetsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreSequentialTrickTargetsCharacterCuiLieAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAnniversaryCuiLieDelayedTail(global::CardGame.Core.CardUseFrame use);
    bool ContinueAnniversaryCuiLieDelayedPlacement(global::CardGame.Core.CardUseFrame use);
    bool IsAnniversaryCuiLieDelayedMaterialPlaced(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreSequentialTrickTargetsCoreDamageTargetObtainProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsIssuedZeroEntityDuel(long id);
}

internal interface ICoreSequentialTrickTargetsCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
}

internal interface ICoreSequentialTrickTargetsCoreNativeNullificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginNullificationWindow(long resolutionId, global::CardGame.Core.Card effectCard, int sourceSeat, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, global::CardGame.Core.LegalActionKind actionKind, int? targetCardId = default, global::CardGame.Core.CardKind? requiredCardKind = default, global::CardGame.Core.CardKind? playedCardKind = default);
}

internal interface ICoreSequentialTrickTargetsCoreVirtualOrdinaryTrickUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsProgramVirtualOrdinaryTrickUse(long frameId);
}

internal interface ICoreSequentialTrickTargetsCoreTargetedOrdinaryTrickOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasTargetedOrdinaryTrickTail(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreSequentialTrickTargetsCoreResponseEntityExchangeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card EntityAtCurrentLocation(int id);
}

internal interface ICoreSequentialTrickTargetsCoreProgramMultiTargetDuelsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasProgramIronChainTargetExpansion(int sourceSeat);
}

internal interface ICoreSequentialTrickTargetsCoreEphemeralCardMaterialsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsEphemeralCardDestroyedMaterial(long useId, int cardId);
}

internal interface ICoreSequentialTrickTargetsCoreNextCardTargetAdjustmentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRemainingAdjustedBorrowedSwordTargets(long frameId);
}

internal interface ICoreSequentialTrickTargetsCoreMantianGuohaiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool KeepMantianGuohaiMaterial(long id);
}

internal interface ICoreSequentialTrickTargetsCharacterXuHuangMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouXuHuangZeroUse(long id);
}

internal interface ICoreSequentialTrickTargetsCharacterHuBanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsHuiyunOutsideGeneralWeaponMaterial(long useId, int cardId);
}

internal interface ICoreSequentialTrickTargetsCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next);
}

internal interface ICoreSequentialTrickTargetsCharacterZhugeLiangMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsZeroEntityUse(long id);
}

internal interface ICoreSequentialTrickTargetsCoreCurrentUsePhysicalClaimsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsClaimedUseCardEntity(long frameId, int cardId);
}

internal interface ICoreSequentialTrickTargetsCharacterWenChouMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouWenChouZeroDuel(long id);
}

internal interface ICoreSequentialTrickTargetsCoreTieredRoundZeroUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsTieredRoundZeroUse(long id);
    global::CardGame.Core.Card TieredRoundZeroRepresentation(long id);
}

internal interface ICoreSequentialTrickTargetsCoreHellSoulCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsHellDestroyedSoulMaterial(long useId, int cardId);
}

internal interface ICoreSequentialTrickTargetsCoreAdjacentRedTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAdjacentRedTargetTail(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreSequentialTrickTargetsCoreOriginalUseLightningOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsOriginalUseLightning(long id);
}

internal interface ICoreSequentialTrickTargetsCoreDamageTargetBudgetOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasDamageTargetBudgetTail(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreSequentialTrickTargetsCoreTieredRoundZeroReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool SkipTieredRoundZeroFinishedMovement(long id, global::CardGame.Core.Card card);
}

internal interface ICoreSequentialTrickTargetsCoreUniqueLeaderTrickTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasUniqueLeaderTrickTargetTail(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreSequentialTrickTargetsCharacterDongChengMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouDcZeroUse(long id);
}

internal interface ICoreSequentialTrickTargetsCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreSequentialTrickTargetsCoreGuanduEquipmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsGuanduDestroyedChariotMaterial(long useId, int cardId);
}

internal interface ICoreSequentialTrickTargetsCharacterXiaoQiaoMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouXqJueyanUse(long id);
}

internal interface ICoreSequentialTrickTargetsCorePhysicalMovementFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
}

internal interface ICoreSequentialTrickTargetsCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreSequentialTrickTargetsCharacterXunXuAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryFinishAnniversaryXunXuUseMaterials(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreSequentialTrickTargetsCoreSameTypeActualUseAidOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasSameTypeAidTargetTail(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreSequentialTrickTargetsCharacterZhouYuMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouZyZeroFireUse(long id);
}
