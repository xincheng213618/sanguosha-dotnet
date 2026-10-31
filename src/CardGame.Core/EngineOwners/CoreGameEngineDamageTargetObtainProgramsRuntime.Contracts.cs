#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreDamageTargetObtainProgramsCoreCardUseModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardUseForbidden(int actorSeat, global::CardGame.Core.CardKind effectiveKind, global::CardGame.Core.CardActionType actionType, bool ignoreIssuedPlayBan = false, long? existingUseFrameId = default);
    bool HasTurnCardTargetRestriction(int actorSeat, global::CardGame.Core.SkillProgramCardTargetRestriction restriction, int? targetSeat = default);
}

internal interface ICoreDamageTargetObtainProgramsCorePhysicalDrawFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> DrawCards(global::CardGame.Core.CharacterState player, int count, bool log, global::CardGame.Core.CardMoveReason? reason = default, global::CardGame.Core.CardConversionSource? directProducer = default);
}

internal interface ICoreDamageTargetObtainProgramsCoreDamageTargetRegionObtainOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool UsesDamageTargetRegionObtain(global::CardGame.Core.ProgramSkillFrame frame);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> DamageTargetRegionObtainChoices(global::CardGame.Core.ProgramSkillFrame frame);
    void IssueDamageTargetRegionObtain(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.PromptChoice choice);
    bool IsValidDamageTargetRegionObtain(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramDamageTargetObtain draft);
    bool DamageTargetDuelBaseMatches(global::CardGame.Core.ProgramDamageTargetObtain receipt, global::CardGame.Core.ProgramDamageTargetDuelOrigin origin);
    global::CardGame.Core.PromptChoice SelectAiDamageTargetRegion(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreDamageTargetObtainProgramsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreDamageTargetObtainProgramsCoreEquipmentContentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.EquipmentContentRegistry EquipmentRules { get; }
}

internal interface ICoreDamageTargetObtainProgramsCoreDamageColorDuelProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSelectedActorDuelUse(long id);
}

internal interface ICoreDamageTargetObtainProgramsCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
}

internal interface ICoreDamageTargetObtainProgramsCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreDamageTargetObtainProgramsCoreDirectedTurnCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDirectedCardTargetProhibited(int actorSeat, int targetSeat, global::CardGame.Core.CardKind cardKind, long? cardUseFrameId = default);
}

internal interface ICoreDamageTargetObtainProgramsCoreConditionalDiscardDuelReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsConditionalDiscardDuelUse(long id);
}

internal interface ICoreDamageTargetObtainProgramsCharacterHaoPuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsZhenyingDuelUse(long id);
}

internal interface ICoreDamageTargetObtainProgramsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
    void CancelProgramBindingAndCleanup(global::CardGame.Core.ProgramSkillFrame frame, string reason);
}

internal interface ICoreDamageTargetObtainProgramsCorePlayerViewProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICoreDamageTargetObtainProgramsCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreDamageTargetObtainProgramsCoreNativeNullificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginJizhiOrNullificationWindow(long resolutionId, global::CardGame.Core.Card effectCard, int sourceSeat, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, global::CardGame.Core.LegalActionKind actionKind, int? targetCardId = default, global::CardGame.Core.CardKind? requiredCardKind = default, global::CardGame.Core.CardKind? playedCardKind = default);
}

internal interface ICoreDamageTargetObtainProgramsCorePhysicalMovementFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
}

internal interface ICoreDamageTargetObtainProgramsCoreProgramOtherOwnedCardDiscardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptChoice SelectAiProgramOtherOwnedCardDiscard(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreDamageTargetObtainProgramsCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreDamageTargetObtainProgramsCharacterLuBuMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> MouLbDuelOuterProcessing(global::CardGame.Core.CardUseFrame use);
    bool MatchesMouLbDuelParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramDamageTargetDuelOrigin o);
    void AssertMouLbDuelUse(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreDamageTargetObtainProgramsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreDamageTargetObtainProgramsCoreCardMovementProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCardsMovedProgramWindow(long? instructionFrameId = default, int? subjectSeat = default);
}

internal interface ICoreDamageTargetObtainProgramsCharacterHuangQuanMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsShiQuanDuelUse(long id);
}

internal interface ICoreDamageTargetObtainProgramsCharacterDongChengMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouDcZeroUse(long id);
}

internal interface ICoreDamageTargetObtainProgramsCoreNativeCardUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long BeginCardUse(global::CardGame.Core.Card card, int sourceSeat, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, global::CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default, global::System.Collections.Generic.IReadOnlyList<int>? designatedTargetSeats = default, bool isTrueZhangbaSlash = false, global::CardGame.Core.ProgramSelectedActorDuelOrigin? selectedActorDuelOrigin = default, global::CardGame.Core.ProgramDamageTargetDuelOrigin? damageTargetDuelOrigin = default, global::CardGame.Core.ProgramDualColorDuelOrigin? dualColorDuelOrigin = default, global::CardGame.Core.ConditionalDiscardDuelOrigin? conditionalDiscardDuelOrigin = default, global::CardGame.Core.ShiQuanDuelOrigin? shiQuanDuelOrigin = default, global::CardGame.Core.ProgramVirtualOrdinaryTrickOrigin? virtualOrdinaryTrickOrigin = default, global::CardGame.Core.DrawFundedDistinctBasicPayment? drawFundedPayment = default, global::CardGame.Core.ChainedStateBasicPayment? chainedStatePayment = default, global::CardGame.Core.FriendRecastDuelOrigin? friendRecastDuel = default, global::CardGame.Core.ChosenAssaultReturn? chosenAssault = default, global::CardGame.Core.ZhenyingDuelOrigin? zhenyingDuel = default, global::CardGame.Core.HeJinMouzhuUseOrigin? heJinMouzhu = default, global::CardGame.Core.WuAnGuoLiyongDuelOrigin? wuAnGuoLiyong = default, global::CardGame.Core.TianChouShandaoUseOrigin? tianChouShandao = default, global::CardGame.Core.QinLangXianyingUseOrigin? qinLangXianying = default, global::CardGame.Core.AHuiNanJuemanUseOrigin? aHuiNanJueman = default, global::CardGame.Core.LiuYanLimuUseOrigin? liuYanLimu = default, global::CardGame.Core.JueshengDuelOrigin? jueshengDuel = default, global::CardGame.Core.QuHuangNishouUseOrigin? quHuangNishou = default, global::CardGame.Core.WeijiePaidOrigin? weijieOrigin = default, global::CardGame.Core.ZonghuPaidOrigin? zonghuOrigin = default, global::CardGame.Core.HuanpeiPaidOrigin? huanpeiOrigin = default);
}

internal interface ICoreDamageTargetObtainProgramsCoreCardAttackStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void UpdateCardAttackState(long id, global::System.Func<global::CardGame.Core.CardAttackState?, global::CardGame.Core.CardAttackState> update);
}

internal interface ICoreDamageTargetObtainProgramsCharacterHeJinOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsHeJinMouzhuDuelUse(long id);
}

internal interface ICoreDamageTargetObtainProgramsCharacterWuAnGuoOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWuAnGuoLiyongDuelUse(long id);
}

internal interface ICoreDamageTargetObtainProgramsCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
    void ClearPendingDecision();
}

internal interface ICoreDamageTargetObtainProgramsCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void ReturnRuntimeProgramMovement(long frameId);
    void AdvanceRuntimeProgram(long frameId);
}

internal interface ICoreDamageTargetObtainProgramsCoreNativeDamagePoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardTargetProhibited(global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind cardKind, global::CardGame.Core.Suit? suit = default, bool? effectiveIsRed = default);
}

internal interface ICoreDamageTargetObtainProgramsCharacterXuHuangMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouXuHuangZeroUse(long id);
}

internal interface ICoreDamageTargetObtainProgramsCharacterWenChouMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouWenChouZeroDuel(long id);
}

internal interface ICoreDamageTargetObtainProgramsCoreDamageTargetMountProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? ActualDamageTargetMountUse(int ownerSeat, global::CardGame.Core.ProgramSkillWindowContext context);
}

internal interface ICoreDamageTargetObtainProgramsCoreProgramCardPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> BuildOwnedCardPaymentChoices(long frameId, int chooserSeat, int cardOwnerSeat, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardZoneKind> zones, global::CardGame.Core.OwnedCardMoveIntent intent, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SkillProgramCardCategory>? cardCategories = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardKind>? cardKinds = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Suit>? suits = default, global::System.Func<global::CardGame.Core.CardZoneKind, global::CardGame.Core.Card, bool>? canSelect = default, global::CardGame.Core.SkillProgramCardDestination destination = (global::CardGame.Core.SkillProgramCardDestination)1, int? destinationSeat = default, bool prohibitReplacingEquipment = false, int? correspondingZoneSeat = default);
}

internal interface ICoreDamageTargetObtainProgramsCoreDualColorDuelProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDualColorDuelUse(long id);
}

internal interface ICoreDamageTargetObtainProgramsCoreHappyWeiYanGainDestinationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsHappyWeiYanRedirectedGain(global::CardGame.Core.CardMovementRecord movement, global::CardGame.Core.CardLocation expectedTo);
}

internal interface ICoreDamageTargetObtainProgramsCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId);
}

internal interface ICoreDamageTargetObtainProgramsCoreEphemeralCardMaterialsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsEphemeralCardDestroyedMaterial(global::CardGame.Core.CardMovementRecord movement);
}

internal interface ICoreDamageTargetObtainProgramsCoreFriendEventsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsFriendRecastDuelUse(long id);
}

internal interface ICoreDamageTargetObtainProgramsCharacterFanJiangZhangDaOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsJueshengDuelUse(long id);
}
