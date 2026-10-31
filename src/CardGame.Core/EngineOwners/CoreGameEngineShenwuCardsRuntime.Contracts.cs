#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreShenwuCardsCoreNativeNullificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginJizhiOrNullificationWindow(long resolutionId, global::CardGame.Core.Card effectCard, int sourceSeat, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, global::CardGame.Core.LegalActionKind actionKind, int? targetCardId = default, global::CardGame.Core.CardKind? requiredCardKind = default, global::CardGame.Core.CardKind? playedCardKind = default);
}

internal interface ICoreShenwuCardsCorePhysicalDrawFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> DrawCards(global::CardGame.Core.CharacterState player, int count, bool log, global::CardGame.Core.CardMoveReason? reason = default, global::CardGame.Core.CardConversionSource? directProducer = default);
}

internal interface ICoreShenwuCardsCharacterHuBanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsHuiyunOutsideGeneralWeaponMaterial(long useId, int cardId);
}

internal interface ICoreShenwuCardsCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreShenwuCardsCoreCardUseModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardUseForbidden(int actorSeat, global::CardGame.Core.CardKind effectiveKind, global::CardGame.Core.CardActionType actionType, bool ignoreIssuedPlayBan = false, long? existingUseFrameId = default);
}

internal interface ICoreShenwuCardsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
    void AdvanceRulesAndPublishState();
}

internal interface ICoreShenwuCardsCoreGuanduEquipmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsGuanduDestroyedChariotMaterial(long useId, int cardId);
}

internal interface ICoreShenwuCardsCoreMatchSetupDefinitionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    string? GetEffectiveFactionId(global::CardGame.Core.CharacterState player);
}

internal interface ICoreShenwuCardsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreShenwuCardsCharacterMiHengLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSiruiOutsideGeneralWeaponMaterial(long useId, int cardId);
}

internal interface ICoreShenwuCardsCoreDirectedTurnCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDirectedCardTargetProhibited(int actorSeat, int targetSeat, global::CardGame.Core.CardKind cardKind, long? cardUseFrameId = default);
}

internal interface ICoreShenwuCardsCoreNativeCardUseCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinishCardUse(long frameId, global::CardGame.Core.Card card, global::CardGame.Core.CardKind? playedCardKind = default);
}

internal interface ICoreShenwuCardsCoreNativeLegalActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> BuildLegalActions(global::CardGame.Core.CharacterState actor, bool includeProgramActions = true, int? selectedPhysicalCardId = default);
}

internal interface ICoreShenwuCardsCoreNativeNullificationFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetNullificationEffectCard(global::CardGame.Core.NullificationWindowFrame frame);
}

internal interface ICoreShenwuCardsCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player);
}

internal interface ICoreShenwuCardsCoreResolutionIdentityBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long _resolutionSequence { get; set; }
}

internal interface ICoreShenwuCardsCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
    void UpdateLifecycleCardUse(long frameId, global::System.Func<global::CardGame.Core.CardUseFrame, global::CardGame.Core.CardUseFrame> update);
}

internal interface ICoreShenwuCardsCoreNativeOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardLocation FindOwnedCardLocation(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card);
}

internal interface ICoreShenwuCardsCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame);
    void AdvanceRuntimeProgram(long frameId);
    void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind);
}

internal interface ICoreShenwuCardsCoreNativeDamagePoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardTargetProhibited(global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind cardKind, global::CardGame.Core.Suit? suit = default, bool? effectiveIsRed = default);
}

internal interface ICoreShenwuCardsCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreShenwuCardsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreShenwuCardsCoreNativeCardAppearanceOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreShenwuCardsCoreNativeCardUseFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetCardUseStep(long frameId, global::CardGame.Core.ResolutionFrameStep step);
}

internal interface ICoreShenwuCardsCoreNativeCardUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long BeginCardUse(global::CardGame.Core.Card card, int sourceSeat, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, global::CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default, global::System.Collections.Generic.IReadOnlyList<int>? designatedTargetSeats = default, bool isTrueZhangbaSlash = false, global::CardGame.Core.ProgramSelectedActorDuelOrigin? selectedActorDuelOrigin = default, global::CardGame.Core.ProgramDamageTargetDuelOrigin? damageTargetDuelOrigin = default, global::CardGame.Core.ProgramDualColorDuelOrigin? dualColorDuelOrigin = default, global::CardGame.Core.ConditionalDiscardDuelOrigin? conditionalDiscardDuelOrigin = default, global::CardGame.Core.ShiQuanDuelOrigin? shiQuanDuelOrigin = default, global::CardGame.Core.ProgramVirtualOrdinaryTrickOrigin? virtualOrdinaryTrickOrigin = default, global::CardGame.Core.DrawFundedDistinctBasicPayment? drawFundedPayment = default, global::CardGame.Core.ChainedStateBasicPayment? chainedStatePayment = default, global::CardGame.Core.FriendRecastDuelOrigin? friendRecastDuel = default, global::CardGame.Core.ChosenAssaultReturn? chosenAssault = default, global::CardGame.Core.ZhenyingDuelOrigin? zhenyingDuel = default, global::CardGame.Core.HeJinMouzhuUseOrigin? heJinMouzhu = default, global::CardGame.Core.WuAnGuoLiyongDuelOrigin? wuAnGuoLiyong = default, global::CardGame.Core.TianChouShandaoUseOrigin? tianChouShandao = default, global::CardGame.Core.QinLangXianyingUseOrigin? qinLangXianying = default, global::CardGame.Core.AHuiNanJuemanUseOrigin? aHuiNanJueman = default, global::CardGame.Core.LiuYanLimuUseOrigin? liuYanLimu = default, global::CardGame.Core.JueshengDuelOrigin? jueshengDuel = default, global::CardGame.Core.QuHuangNishouUseOrigin? quHuangNishou = default, global::CardGame.Core.WeijiePaidOrigin? weijieOrigin = default, global::CardGame.Core.ZonghuPaidOrigin? zonghuOrigin = default, global::CardGame.Core.HuanpeiPaidOrigin? huanpeiOrigin = default);
}

internal interface ICoreShenwuCardsCoreDeckEquipmentProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasBeneficiarySuitShield(int actor, int target, global::CardGame.Core.Suit? suit);
}

internal interface ICoreShenwuCardsCoreNativeCoreInvariantsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsActiveAttackCardConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
}

internal interface ICoreShenwuCardsCoreCurrentUsePhysicalClaimsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsClaimedUseCardEntity(long frameId, int cardId);
}

internal interface ICoreShenwuCardsCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreShenwuCardsCorePhysicalMovementFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
}

internal interface ICoreShenwuCardsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramInteractionRouter ProgramInteractions { get; }
}

internal interface ICoreShenwuCardsCoreDamageColorDuelProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetTrickRepresentation(long frameId, int cardId, bool requireProcessing = false);
}
