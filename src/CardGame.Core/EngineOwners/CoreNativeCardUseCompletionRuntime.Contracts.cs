#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreNativeCardUseCompletionCorePhysicalMovementFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
}

internal interface ICoreNativeCardUseCompletionCharacterXunXuAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryFinishAnniversaryXunXuUseMaterials(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNativeCardUseCompletionCoreMantianGuohaiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMantianGuohaiUse(long id);
    bool TryContinueMantianGuohaiGifts(long id);
}

internal interface ICoreNativeCardUseCompletionCoreNextCardTargetAdjustmentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryContinueAdjustedSimpleCardUse(long frameId);
    bool TryContinueAdjustedBorrowedSwordUse(long frameId);
}

internal interface ICoreNativeCardUseCompletionCoreOriginalUseLightningOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryContinueOriginalUseLightningPlacement(long id);
}

internal interface ICoreNativeCardUseCompletionCoreCardActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginProgramCardWindow(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<int> opponents, global::CardGame.Core.ProgramCardContinuation? continuation, bool? cardUseCausedDamage = default, global::CardGame.Core.ProgramTrickContinuation? trickContinuation = default, global::CardGame.Core.ProgramSimpleCardContinuation? simpleContinuation = default, global::CardGame.Core.ProgramCompletedResponseReturn? completedResponseReturn = default, global::CardGame.Core.CaoChunHandUsePayment? caoChunPayment = default, bool caoChunDebtOnly = false);
}

internal interface ICoreNativeCardUseCompletionCoreSequentialTrickTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryContinueSequentialTrick(long frameId, bool afterAttack = false);
}

internal interface ICoreNativeCardUseCompletionCharacterHuaManOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsHuaManClaimedEntity(long useId, int cardId);
}

internal interface ICoreNativeCardUseCompletionKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreNativeCardUseCompletionCoreNativeFactionSlashCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteProgramFactionCardRequest(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending);
}

internal interface ICoreNativeCardUseCompletionCoreCharacterStateProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCharacterStateProgramWindow(long? resumeFrameId = default, global::CardGame.Core.CharacterStateContinuation continuation = (global::CardGame.Core.CharacterStateContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICoreNativeCardUseCompletionCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreNativeCardUseCompletionCoreSoleIncomingRangeDrawOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginSoleIncomingRangeDraw(long id);
}

internal interface ICoreNativeCardUseCompletionCharacterZhugeGuoLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryContinueLeChengyanBorrowed(long id);
    bool ValidLeChengyanEffect(global::CardGame.Core.CardUseFrame u);
}

internal interface ICoreNativeCardUseCompletionCoreNativeFactionSlashOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteQinglongCrescentBladeFactionSlash(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending);
}

internal interface ICoreNativeCardUseCompletionCharacterXinPiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsXinPiPhysicalClaim(long useId, int cardId);
}

internal interface ICoreNativeCardUseCompletionCoreHpChangeProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginHpChangedProgramWindow(long? resumeFrameId = default, global::CardGame.Core.PostEventContinuation continuation = (global::CardGame.Core.PostEventContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICoreNativeCardUseCompletionCoreHpLossSlashMaterialsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasExactHpLossMaterialReturn(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNativeCardUseCompletionCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind);
}

internal interface ICoreNativeCardUseCompletionCoreProgramNativeReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramNativeReturns NativeReturns { get; }
}

internal interface ICoreNativeCardUseCompletionCoreCurrentUsePhysicalClaimsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCurrentUsePhysicalCardClaim(long frameId, int cardId);
}

internal interface ICoreNativeCardUseCompletionCharacterDengAiAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RetireUnplacedShenDaCarrier(long id);
}

internal interface ICoreNativeCardUseCompletionCharacterWuAnGuoOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWuAnGuoLiyongCompletedUseParent(long useId);
}

internal interface ICoreNativeCardUseCompletionCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
}

internal interface ICoreNativeCardUseCompletionResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreNativeCardUseCompletionCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.FactionCardRequestHandle? ActiveFactionCardRequest { get; set; }

    global::CardGame.Core.NativeOwners.QinglongFollowupHandle? ActiveQinglongFollowup { get; set; }

    global::CardGame.Core.NativeOwners.BorrowedSwordHandle? ActiveBorrowedSword { get; set; }
}

internal interface ICoreNativeCardUseCompletionCoreCurrentUseTargetCandidateRefreshOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame NormalizeImplicitDrawTwoUseTarget(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNativeCardUseCompletionCoreExtraCardEffectRoundsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginExtraCardEffectRound(long id, bool afterAttack = false);
}

internal interface ICoreNativeCardUseCompletionCharacterMaChengOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteChenglieTerminalCleanup(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNativeCardUseCompletionCharacterDiaoChanLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RetireUnplacedDiouCarrier(long useId);
}

internal interface ICoreNativeCardUseCompletionCoreNoDamageSkillDisableOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SettleNoDamageSkillDisable(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNativeCardUseCompletionKernelCoreMainOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteGame();
}

internal interface ICoreNativeCardUseCompletionCoreNativeCardUseFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetCardUseStep(long frameId, global::CardGame.Core.ResolutionFrameStep step);
}

internal interface ICoreNativeCardUseCompletionCoreNativeSlashTargetDuelOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidNativeSlashTargetDuelPlan(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.NativeSlashTargetDuelPlan plan);
}
