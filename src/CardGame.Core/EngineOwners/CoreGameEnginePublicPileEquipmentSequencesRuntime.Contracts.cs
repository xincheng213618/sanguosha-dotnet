#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICorePublicPileEquipmentSequencesCoreEquipmentContentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.EquipmentContentRegistry EquipmentRules { get; }
}

internal interface ICorePublicPileEquipmentSequencesResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICorePublicPileEquipmentSequencesCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player);
}

internal interface ICorePublicPileEquipmentSequencesCoreCardMovementProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCardsMovedProgramWindow(long? instructionFrameId = default, int? subjectSeat = default);
}

internal interface ICorePublicPileEquipmentSequencesCoreGameDomainsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.FirstGameDomainCrossingEvent? FirstDomainFact(global::CardGame.Core.ProgramSkillWindowContext context);
}

internal interface ICorePublicPileEquipmentSequencesContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICorePublicPileEquipmentSequencesCoreNativeCardUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long BeginCardUse(global::CardGame.Core.Card card, int sourceSeat, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, global::CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default, global::System.Collections.Generic.IReadOnlyList<int>? designatedTargetSeats = default, bool isTrueZhangbaSlash = false, global::CardGame.Core.ProgramSelectedActorDuelOrigin? selectedActorDuelOrigin = default, global::CardGame.Core.ProgramDamageTargetDuelOrigin? damageTargetDuelOrigin = default, global::CardGame.Core.ProgramDualColorDuelOrigin? dualColorDuelOrigin = default, global::CardGame.Core.ConditionalDiscardDuelOrigin? conditionalDiscardDuelOrigin = default, global::CardGame.Core.ShiQuanDuelOrigin? shiQuanDuelOrigin = default, global::CardGame.Core.ProgramVirtualOrdinaryTrickOrigin? virtualOrdinaryTrickOrigin = default, global::CardGame.Core.DrawFundedDistinctBasicPayment? drawFundedPayment = default, global::CardGame.Core.ChainedStateBasicPayment? chainedStatePayment = default, global::CardGame.Core.FriendRecastDuelOrigin? friendRecastDuel = default, global::CardGame.Core.ChosenAssaultReturn? chosenAssault = default, global::CardGame.Core.ZhenyingDuelOrigin? zhenyingDuel = default, global::CardGame.Core.HeJinMouzhuUseOrigin? heJinMouzhu = default, global::CardGame.Core.WuAnGuoLiyongDuelOrigin? wuAnGuoLiyong = default, global::CardGame.Core.TianChouShandaoUseOrigin? tianChouShandao = default, global::CardGame.Core.QinLangXianyingUseOrigin? qinLangXianying = default, global::CardGame.Core.AHuiNanJuemanUseOrigin? aHuiNanJueman = default, global::CardGame.Core.LiuYanLimuUseOrigin? liuYanLimu = default, global::CardGame.Core.JueshengDuelOrigin? jueshengDuel = default, global::CardGame.Core.QuHuangNishouUseOrigin? quHuangNishou = default, global::CardGame.Core.WeijiePaidOrigin? weijieOrigin = default, global::CardGame.Core.ZonghuPaidOrigin? zonghuOrigin = default, global::CardGame.Core.HuanpeiPaidOrigin? huanpeiOrigin = default);
}

internal interface ICorePublicPileEquipmentSequencesCoreEquipmentSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsActiveProgramSourceEquipmentCard(int ownerSeat, string skillId, string skillInstanceId, global::CardGame.Core.Card card);
}

internal interface ICorePublicPileEquipmentSequencesKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceRulesAndPublishState();
}

internal interface ICorePublicPileEquipmentSequencesKernelCoreGameEngineAdvancedLifecycleHostOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.ProgramSkillHost CreateProgramSkillHost();
}

internal interface ICorePublicPileEquipmentSequencesCorePhysicalMovementFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveCards(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
    void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
}

internal interface ICorePublicPileEquipmentSequencesCoreCardUseModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardUseForbidden(int actorSeat, global::CardGame.Core.CardKind effectiveKind, global::CardGame.Core.CardActionType actionType, bool ignoreIssuedPlayBan = false, long? existingUseFrameId = default);
}

internal interface ICorePublicPileEquipmentSequencesCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICorePublicPileEquipmentSequencesCoreBoundCardMovementContinuationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramStepOutcome AwaitProgramBoundCardMovements(long frameId, int ownerSeat);
}

internal interface ICorePublicPileEquipmentSequencesCoreProgramAtomicCardMovesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveProgramCardsFromMultipleSources(global::System.Collections.Generic.IReadOnlyList<int> ids, global::CardGame.Core.CardLocation destination, global::CardGame.Core.CardMoveReason reason, global::System.Action<long, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord>>? afterPhysicalRecords = default, global::System.Func<global::CardGame.Core.Card, global::CardGame.Core.CardLocation, global::CardGame.Core.CardLocation>? destinationForCard = default, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0, global::System.Action<global::CardGame.Core.HappyWeiYanGainRequestSettlement>? afterRequestSettled = default, long? producerFrameId = default);
}

internal interface ICorePublicPileEquipmentSequencesCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
    void DrawProgramCards(long frameId, int targetSeat, int amount, global::CardGame.Core.SkillProgramNumberExpression? numberExpression, string? resultBind, global::CardGame.Core.SkillProgramCardSetVisibility visibility, global::CardGame.Core.CardMoveReason reason);
}

internal interface ICorePublicPileEquipmentSequencesCoreNativeEquipmentUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteEquipmentUse(global::CardGame.Core.CharacterState source, global::CardGame.Core.Card equipment, long resolutionId);
}

internal interface ICorePublicPileEquipmentSequencesCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
    void ClearPendingDecision();
}

internal interface ICorePublicPileEquipmentSequencesCorePublicPileSourcesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.PublicPersistentPileSource> PublicPileSources(int seat);
    global::CardGame.Core.PublicPersistentPileSource EnsurePublicPileSource(global::CardGame.Core.ProgramSkillFrame frame, int capacity);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> PublicPileCards(global::CardGame.Core.PublicPersistentPileSource source);
}

internal interface ICorePublicPileEquipmentSequencesCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void ReturnRuntimeProgramMovement(long frameId);
    void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next);
    void AdvanceRuntimeProgram(long frameId);
}

internal interface ICorePublicPileEquipmentSequencesCoreFinalizedCardTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginEquipmentTargetPrograms(global::CardGame.Core.CharacterState source, global::CardGame.Core.Card equipment, long frameId);
}

internal interface ICorePublicPileEquipmentSequencesCoreHandComparisonOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSelfTargetForbiddenAction(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> targets);
}

internal interface ICorePublicPileEquipmentSequencesCoreResponseEntityExchangeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsResponseEntityRestricted(int seat, int id);
}

internal interface ICorePublicPileEquipmentSequencesCoreNativeLogicalDiscardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveProgramLogicalDiscard(global::CardGame.Core.ProgramSkillFrame frame, int executorSeat, global::System.Collections.Generic.IReadOnlyList<int> ids, global::CardGame.Core.CardMoveReason reason, global::System.Action<long, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord>>? afterPhysicalRecords = default);
}

internal interface ICorePublicPileEquipmentSequencesCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICorePublicPileEquipmentSequencesCoreProgramPhaseColorRestrictionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPlayPhasePhysicalCardRestricted(global::CardGame.Core.CharacterState player, global::CardGame.Core.Card card);
}
