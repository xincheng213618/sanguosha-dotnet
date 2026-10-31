#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreDeferredCardsCorePopulationTopReorderProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardSnapshot[] GetPopulationTopPrivatelyViewedCards(int viewerSeat);
}

internal interface ICoreDeferredCardsCharacterWangCanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardSnapshot[] DenglouPrivateCards(int viewer);
}

internal interface ICoreDeferredCardsCharacterMaChengOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardSnapshot> ChengliePrivateCards(int viewer);
}

internal interface ICoreDeferredCardsCoreAlcoholIdentityPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsEffectiveHandKindExempt(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::System.Collections.Generic.IReadOnlySet<global::CardGame.Core.CardKind> kinds);
}

internal interface ICoreDeferredCardsCharacterFanChouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardSnapshot> FanChouPrivateDeckCards(int viewer);
}

internal interface ICoreDeferredCardsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreDeferredCardsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillCardSetBinding GetProgramCardSet(global::CardGame.Core.ProgramSkillFrame frame, string bind);
    void DrawProgramCards(long frameId, int targetSeat, int amount, global::CardGame.Core.SkillProgramNumberExpression? numberExpression, string? resultBind, global::CardGame.Core.SkillProgramCardSetVisibility visibility, global::CardGame.Core.CardMoveReason reason);
}

internal interface ICoreDeferredCardsCharacterMiHengLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsJiguHandLimitExempt(global::CardGame.Core.CharacterState owner, int cardId);
}

internal interface ICoreDeferredCardsCharacterDiaoChanLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsTanbanHandLimitExempt(global::CardGame.Core.CharacterState owner, int cardId);
}

internal interface ICoreDeferredCardsCoreQuotaTopProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardSnapshot[] GetQuotaPrivatelyViewedCards(int viewerSeat);
}

internal interface ICoreDeferredCardsCharacterHuangYueyingMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouHyJizhiHandLimitExempt(global::CardGame.Core.CharacterState owner, int cardId);
}

internal interface ICoreDeferredCardsCorePhysicalMovementFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveCards(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
}

internal interface ICoreDeferredCardsCharacterChenLinMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouChenLinHandLimitExempt(global::CardGame.Core.CharacterState owner, int cardId);
}

internal interface ICoreDeferredCardsCharacterTaoQianOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsZhaohuoHandLimitExempt(int ownerSeat, int cardId);
}

internal interface ICoreDeferredCardsCoreConvertingGiftsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardSnapshot[] GetConvertingGiftPrivatelyViewedCards(int viewerSeat);
}

internal interface ICoreDeferredCardsCharacterSunQuanOlMagicOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMoTianenHandLimitExempt(global::CardGame.Core.CharacterState owner, int id);
}

internal interface ICoreDeferredCardsCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreDeferredCardsCoreProgramPrivateDamageOfferOperationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardSnapshot[] GetOfferedPrivatelyViewedCards(int viewerSeat);
}

internal interface ICoreDeferredCardsCharacterTangTangOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.CardSnapshot> TangTangPrivatelyViewedCards(int viewerSeat);
}

internal interface ICoreDeferredCardsCoreShenwuExpandedPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsShenwuHonglianHandExempt(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreDeferredCardsCharacterCaiWenJiLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsShuangjiaHandLimitExempt(global::CardGame.Core.CharacterState owner, int cardId);
}

internal interface ICoreDeferredCardsCorePrivateCardOffersOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardSnapshot> DeferredPrivateOfferViewedCards(int viewer);
}

internal interface ICoreDeferredCardsCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
    void ClearPendingDecision();
}

internal interface ICoreDeferredCardsCharacterSimaYiOlMagicOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMoGuifuHandLimitExempt(global::CardGame.Core.CharacterState owner, int id);
}

internal interface ICoreDeferredCardsCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
}

internal interface ICoreDeferredCardsCoreParticipantHandProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.CardSnapshot> GetParticipantPrivatelyViewedCards(int viewerSeat);
}

internal interface ICoreDeferredCardsCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreDeferredCardsCharacterZhugeLiangWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardSnapshot[] WuZglPrivateTopCards(int viewer);
}

internal interface ICoreDeferredCardsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreDeferredCardsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreDeferredCardsCoreExactTopReorderProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardSnapshot[] GetExactTopPrivatelyViewedCards(int viewerSeat);
}

internal interface ICoreDeferredCardsCharacterSimaHuiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.Card> SimaHuiPrivatelyViewedCards(int viewer);
}

internal interface ICoreDeferredCardsCharacterZhouFeiLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsLingkongHandLimitExempt(global::CardGame.Core.CharacterState owner, int cardId);
}

internal interface ICoreDeferredCardsCoreSnapshotValueProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardSnapshot ToSnapshot(global::CardGame.Core.Card card);
}

internal interface ICoreDeferredCardsCharacterCaiYongLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsJiaoweiHandLimitExempt(global::CardGame.Core.CharacterState owner, int cardId);
}

internal interface ICoreDeferredCardsTurnCardUseEffectsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.TurnCardUseEffectStore _turnCardUseEffects { get; }
}

internal interface ICoreDeferredCardsCoreWushuangTacticRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWushuangHandLimitExempt(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreDeferredCardsCorePhysicalDrawFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card? DrawOneToProcessing(global::CardGame.Core.CardMoveReason reason);
}

internal interface ICoreDeferredCardsCharacterDongBaiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsXiaHuiHandLimitExempt(int owner, global::CardGame.Core.Card card);
}

internal interface ICoreDeferredCardsCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default);
}

internal interface ICoreDeferredCardsCoreCardMovementProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCardsMovedProgramWindow(long? instructionFrameId = default, int? subjectSeat = default);
}

internal interface ICoreDeferredCardsCoreProgramAtomicCardMovesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveProgramCardsFromMultipleSources(global::System.Collections.Generic.IReadOnlyList<int> ids, global::CardGame.Core.CardLocation destination, global::CardGame.Core.CardMoveReason reason, global::System.Action<long, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord>>? afterPhysicalRecords = default, global::System.Func<global::CardGame.Core.Card, global::CardGame.Core.CardLocation, global::CardGame.Core.CardLocation>? destinationForCard = default, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0, global::System.Action<global::CardGame.Core.HappyWeiYanGainRequestSettlement>? afterRequestSettled = default, long? producerFrameId = default);
}

internal interface ICoreDeferredCardsCoreRequestedDeckBasicsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardSnapshot[] RequestedDeckBasicPrivateCards(int viewer);
}

internal interface ICoreDeferredCardsCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void ReturnRuntimeProgramMovement(long frameId);
}

internal interface ICoreDeferredCardsCoreDelegatedJudgmentChoicesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.CardSnapshot> DelegatedJudgmentPrivateCards(int viewer);
}

internal interface ICoreDeferredCardsCorePlayCardHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> EventsSinceLastBoundary(global::System.Func<global::CardGame.Core.IGameEvent, bool> boundary);
}

internal interface ICoreDeferredCardsCoreProgramHandLimitKindExemptionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlySet<global::CardGame.Core.CardKind> GetCurrentHandLimitExemptKinds(global::CardGame.Core.CharacterState owner);
}

internal interface ICoreDeferredCardsCharacterLiuLiAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsAnniversaryLiuLiHandLimitExempt(int seat, global::CardGame.Core.Card card);
}

internal interface ICoreDeferredCardsCharacterCheZhouAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsAnniversaryCheZhouHandExempt(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreDeferredCardsCharacterJuShouMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouJsHandLimitExempt(int actor, int card);
}

internal interface ICoreDeferredCardsCharacterZhugeGuoLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsLeXidiHandLimitExempt(global::CardGame.Core.CharacterState owner, int cardId);
}

internal interface ICoreDeferredCardsCharacterXieLingYuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsXinyouHandLimitExempt(int ownerSeat, int cardId);
}

internal interface ICoreDeferredCardsCharacterZuoCiHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsHappyZuoCiHandLimitExempt(global::CardGame.Core.CharacterState owner, int cardId);
}

internal interface ICoreDeferredCardsCharacterLuXunWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWuXiongmuHandLimitExempt(global::CardGame.Core.CharacterState owner, int cardId);
}

internal interface ICoreDeferredCardsCoreInspectedHandSlashesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.CardSnapshot> InspectedHandPrivateCards(int viewer);
}

internal interface ICoreDeferredCardsCharacterXiaoQiaoLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsQiqinHandLimitExempt(global::CardGame.Core.CharacterState owner, int id);
}

internal interface ICoreDeferredCardsCharacterCaoZhiLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsFuleHandLimitExempt(global::CardGame.Core.CharacterState owner, int cardId);
}

internal interface ICoreDeferredCardsCharacterZhugeLiangMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.CardSnapshot> MouZhitianPrivateCards(int viewerSeat);
}

internal interface ICoreDeferredCardsCharacterZouShiLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsYunzhengHandLimitExempt(global::CardGame.Core.CharacterState owner, int cardId);
}
