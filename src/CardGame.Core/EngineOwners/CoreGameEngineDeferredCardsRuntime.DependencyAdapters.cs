#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePopulationTopReorderProgramsRuntime))]
internal sealed class CoreDeferredCardsCorePopulationTopReorderProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePopulationTopReorderProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredCardsCorePopulationTopReorderProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePopulationTopReorderProgramsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardSnapshot[] GetPopulationTopPrivatelyViewedCards(int viewerSeat) => this._targetFactory().GetPopulationTopPrivatelyViewedCards(viewerSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWangCanOlRuntime))]
internal sealed class CoreDeferredCardsCharacterWangCanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangCanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredCardsCharacterWangCanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangCanOlRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardSnapshot[] DenglouPrivateCards(int viewer) => this._targetFactory().DenglouPrivateCards(viewer);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMaChengOlRuntime))]
internal sealed class CoreDeferredCardsCharacterMaChengOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaChengOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredCardsCharacterMaChengOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaChengOlRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardSnapshot> ChengliePrivateCards(int viewer) => this._targetFactory().ChengliePrivateCards(viewer);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlcoholIdentityPoliciesRuntime))]
internal sealed class CoreDeferredCardsCoreAlcoholIdentityPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlcoholIdentityPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredCardsCoreAlcoholIdentityPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlcoholIdentityPoliciesRuntime> _targetFactory = targetFactory;
    public bool IsEffectiveHandKindExempt(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::System.Collections.Generic.IReadOnlySet<global::CardGame.Core.CardKind> kinds) => this._targetFactory().IsEffectiveHandKindExempt(owner, card, kinds);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterFanChouOlRuntime))]
internal sealed class CoreDeferredCardsCharacterFanChouOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterFanChouOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredCardsCharacterFanChouOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterFanChouOlRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardSnapshot> FanChouPrivateDeckCards(int viewer) => this._targetFactory().FanChouPrivateDeckCards(viewer);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreDeferredCardsCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredCardsCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillCardSetBinding GetProgramCardSet(global::CardGame.Core.ProgramSkillFrame frame, string bind) => this._targetFactory().GetProgramCardSet(frame, bind);
    public void DrawProgramCards(long frameId, int targetSeat, int amount, global::CardGame.Core.SkillProgramNumberExpression? numberExpression, string? resultBind, global::CardGame.Core.SkillProgramCardSetVisibility visibility, global::CardGame.Core.CardMoveReason reason) => this._targetFactory().DrawProgramCards(frameId, targetSeat, amount, numberExpression, resultBind, visibility, reason);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMiHengLeRuntime))]
internal sealed class CoreDeferredCardsCharacterMiHengLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMiHengLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredCardsCharacterMiHengLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMiHengLeRuntime> _targetFactory = targetFactory;
    public bool IsJiguHandLimitExempt(global::CardGame.Core.CharacterState owner, int cardId) => this._targetFactory().IsJiguHandLimitExempt(owner, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDiaoChanLeRuntime))]
internal sealed class CoreDeferredCardsCharacterDiaoChanLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDiaoChanLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredCardsCharacterDiaoChanLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDiaoChanLeRuntime> _targetFactory = targetFactory;
    public bool IsTanbanHandLimitExempt(global::CardGame.Core.CharacterState owner, int cardId) => this._targetFactory().IsTanbanHandLimitExempt(owner, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineQuotaTopProgramsRuntime))]
internal sealed class CoreDeferredCardsCoreQuotaTopProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineQuotaTopProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredCardsCoreQuotaTopProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineQuotaTopProgramsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardSnapshot[] GetQuotaPrivatelyViewedCards(int viewerSeat) => this._targetFactory().GetQuotaPrivatelyViewedCards(viewerSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuangYueyingMouMobileRuntime))]
internal sealed class CoreDeferredCardsCharacterHuangYueyingMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangYueyingMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredCardsCharacterHuangYueyingMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangYueyingMouMobileRuntime> _targetFactory = targetFactory;
    public bool IsMouHyJizhiHandLimitExempt(global::CardGame.Core.CharacterState owner, int cardId) => this._targetFactory().IsMouHyJizhiHandLimitExempt(owner, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime))]
internal sealed class CoreDeferredCardsCorePhysicalMovementFacadesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredCardsCorePhysicalMovementFacadesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> _targetFactory = targetFactory;
    public void MoveCards(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0) => this._targetFactory().MoveCards(cards, from, to, reason, beforeFact, emitCardMovedEvents, disclosure);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterChenLinMouRuntime))]
internal sealed class CoreDeferredCardsCharacterChenLinMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChenLinMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredCardsCharacterChenLinMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterChenLinMouRuntime> _targetFactory = targetFactory;
    public bool IsMouChenLinHandLimitExempt(global::CardGame.Core.CharacterState owner, int cardId) => this._targetFactory().IsMouChenLinHandLimitExempt(owner, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterTaoQianOlRuntime))]
internal sealed class CoreDeferredCardsCharacterTaoQianOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterTaoQianOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredCardsCharacterTaoQianOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterTaoQianOlRuntime> _targetFactory = targetFactory;
    public bool IsZhaohuoHandLimitExempt(int ownerSeat, int cardId) => this._targetFactory().IsZhaohuoHandLimitExempt(ownerSeat, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineConvertingGiftsRuntime))]
internal sealed class CoreDeferredCardsCoreConvertingGiftsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineConvertingGiftsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredCardsCoreConvertingGiftsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineConvertingGiftsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardSnapshot[] GetConvertingGiftPrivatelyViewedCards(int viewerSeat) => this._targetFactory().GetConvertingGiftPrivatelyViewedCards(viewerSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSunQuanOlMagicRuntime))]
internal sealed class CoreDeferredCardsCharacterSunQuanOlMagicAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunQuanOlMagicRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredCardsCharacterSunQuanOlMagicOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunQuanOlMagicRuntime> _targetFactory = targetFactory;
    public bool IsMoTianenHandLimitExempt(global::CardGame.Core.CharacterState owner, int id) => this._targetFactory().IsMoTianenHandLimitExempt(owner, id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramPrivateDamageOfferOperationsRuntime))]
internal sealed class CoreDeferredCardsCoreProgramPrivateDamageOfferOperationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramPrivateDamageOfferOperationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredCardsCoreProgramPrivateDamageOfferOperationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramPrivateDamageOfferOperationsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardSnapshot[] GetOfferedPrivatelyViewedCards(int viewerSeat) => this._targetFactory().GetOfferedPrivatelyViewedCards(viewerSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterTangTangOlRuntime))]
internal sealed class CoreDeferredCardsCharacterTangTangOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterTangTangOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredCardsCharacterTangTangOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterTangTangOlRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.CardSnapshot> TangTangPrivatelyViewedCards(int viewerSeat) => this._targetFactory().TangTangPrivatelyViewedCards(viewerSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShenwuExpandedPoliciesRuntime))]
internal sealed class CoreDeferredCardsCoreShenwuExpandedPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShenwuExpandedPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredCardsCoreShenwuExpandedPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShenwuExpandedPoliciesRuntime> _targetFactory = targetFactory;
    public bool IsShenwuHonglianHandExempt(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().IsShenwuHonglianHandExempt(owner, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaiWenJiLeRuntime))]
internal sealed class CoreDeferredCardsCharacterCaiWenJiLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaiWenJiLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredCardsCharacterCaiWenJiLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaiWenJiLeRuntime> _targetFactory = targetFactory;
    public bool IsShuangjiaHandLimitExempt(global::CardGame.Core.CharacterState owner, int cardId) => this._targetFactory().IsShuangjiaHandLimitExempt(owner, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePrivateCardOffersRuntime))]
internal sealed class CoreDeferredCardsCorePrivateCardOffersAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrivateCardOffersRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredCardsCorePrivateCardOffersOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrivateCardOffersRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardSnapshot> DeferredPrivateOfferViewedCards(int viewer) => this._targetFactory().DeferredPrivateOfferViewedCards(viewer);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime))]
internal sealed class CoreDeferredCardsCoreNativePromptBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredCardsCoreNativePromptBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PromptId CreatePromptId() => this._targetFactory().CreatePromptId();
    public void ClearPendingDecision() => this._targetFactory().ClearPendingDecision();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSimaYiOlMagicRuntime))]
internal sealed class CoreDeferredCardsCharacterSimaYiOlMagicAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaYiOlMagicRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredCardsCharacterSimaYiOlMagicOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaYiOlMagicRuntime> _targetFactory = targetFactory;
    public bool IsMoGuifuHandLimitExempt(global::CardGame.Core.CharacterState owner, int id) => this._targetFactory().IsMoGuifuHandLimitExempt(owner, id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime))]
internal sealed class CoreDeferredCardsCorePhysicalOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredCardsCorePhysicalOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player) => this._targetFactory().GetHand(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreParticipantHandProgramsRuntime))]
internal sealed class CoreDeferredCardsCoreParticipantHandProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreParticipantHandProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredCardsCoreParticipantHandProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreParticipantHandProgramsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.CardSnapshot> GetParticipantPrivatelyViewedCards(int viewerSeat) => this._targetFactory().GetParticipantPrivatelyViewedCards(viewerSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreDeferredCardsCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredCardsCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhugeLiangWumiaoRuntime))]
internal sealed class CoreDeferredCardsCharacterZhugeLiangWumiaoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeLiangWumiaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredCardsCharacterZhugeLiangWumiaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeLiangWumiaoRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardSnapshot[] WuZglPrivateTopCards(int viewer) => this._targetFactory().WuZglPrivateTopCards(viewer);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreExactTopReorderProgramsRuntime))]
internal sealed class CoreDeferredCardsCoreExactTopReorderProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreExactTopReorderProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredCardsCoreExactTopReorderProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreExactTopReorderProgramsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardSnapshot[] GetExactTopPrivatelyViewedCards(int viewerSeat) => this._targetFactory().GetExactTopPrivatelyViewedCards(viewerSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSimaHuiOlRuntime))]
internal sealed class CoreDeferredCardsCharacterSimaHuiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaHuiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredCardsCharacterSimaHuiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaHuiOlRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.Card> SimaHuiPrivatelyViewedCards(int viewer) => this._targetFactory().SimaHuiPrivatelyViewedCards(viewer);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhouFeiLeRuntime))]
internal sealed class CoreDeferredCardsCharacterZhouFeiLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouFeiLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredCardsCharacterZhouFeiLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouFeiLeRuntime> _targetFactory = targetFactory;
    public bool IsLingkongHandLimitExempt(global::CardGame.Core.CharacterState owner, int cardId) => this._targetFactory().IsLingkongHandLimitExempt(owner, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreSnapshotValueProjectionRuntime))]
internal sealed class CoreDeferredCardsCoreSnapshotValueProjectionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreSnapshotValueProjectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredCardsCoreSnapshotValueProjectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreSnapshotValueProjectionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardSnapshot ToSnapshot(global::CardGame.Core.Card card) => this._targetFactory().ToSnapshot(card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaiYongLeRuntime))]
internal sealed class CoreDeferredCardsCharacterCaiYongLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaiYongLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredCardsCharacterCaiYongLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaiYongLeRuntime> _targetFactory = targetFactory;
    public bool IsJiaoweiHandLimitExempt(global::CardGame.Core.CharacterState owner, int cardId) => this._targetFactory().IsJiaoweiHandLimitExempt(owner, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWushuangTacticRulesRuntime))]
internal sealed class CoreDeferredCardsCoreWushuangTacticRulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangTacticRulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredCardsCoreWushuangTacticRulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangTacticRulesRuntime> _targetFactory = targetFactory;
    public bool IsWushuangHandLimitExempt(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().IsWushuangHandLimitExempt(owner, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime))]
internal sealed class CoreDeferredCardsCorePhysicalDrawFacadesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredCardsCorePhysicalDrawFacadesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card? DrawOneToProcessing(global::CardGame.Core.CardMoveReason reason) => this._targetFactory().DrawOneToProcessing(reason);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDongBaiOlRuntime))]
internal sealed class CoreDeferredCardsCharacterDongBaiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDongBaiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredCardsCharacterDongBaiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDongBaiOlRuntime> _targetFactory = targetFactory;
    public bool IsXiaHuiHandLimitExempt(int owner, global::CardGame.Core.Card card) => this._targetFactory().IsXiaHuiHandLimitExempt(owner, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime))]
internal sealed class CoreDeferredCardsCoreProgramCardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredCardsCoreProgramCardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> _targetFactory = targetFactory;
    public bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default) => this._targetFactory().HasCardPolicy(owner, kind, effectiveKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime))]
internal sealed class CoreDeferredCardsCoreCardMovementProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredCardsCoreCardMovementProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime> _targetFactory = targetFactory;
    public bool TryBeginCardsMovedProgramWindow(long? instructionFrameId = default, int? subjectSeat = default) => this._targetFactory().TryBeginCardsMovedProgramWindow(instructionFrameId, subjectSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramAtomicCardMovesRuntime))]
internal sealed class CoreDeferredCardsCoreProgramAtomicCardMovesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAtomicCardMovesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredCardsCoreProgramAtomicCardMovesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAtomicCardMovesRuntime> _targetFactory = targetFactory;
    public void MoveProgramCardsFromMultipleSources(global::System.Collections.Generic.IReadOnlyList<int> ids, global::CardGame.Core.CardLocation destination, global::CardGame.Core.CardMoveReason reason, global::System.Action<long, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord>>? afterPhysicalRecords = default, global::System.Func<global::CardGame.Core.Card, global::CardGame.Core.CardLocation, global::CardGame.Core.CardLocation>? destinationForCard = default, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0, global::System.Action<global::CardGame.Core.HappyWeiYanGainRequestSettlement>? afterRequestSettled = default, long? producerFrameId = default) => this._targetFactory().MoveProgramCardsFromMultipleSources(ids, destination, reason, afterPhysicalRecords, destinationForCard, disclosure, afterRequestSettled, producerFrameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRequestedDeckBasicsRuntime))]
internal sealed class CoreDeferredCardsCoreRequestedDeckBasicsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRequestedDeckBasicsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredCardsCoreRequestedDeckBasicsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRequestedDeckBasicsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardSnapshot[] RequestedDeckBasicPrivateCards(int viewer) => this._targetFactory().RequestedDeckBasicPrivateCards(viewer);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreDeferredCardsCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredCardsCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeTop(next);
    public void ReturnRuntimeProgramMovement(long frameId) => this._targetFactory().ReturnRuntimeProgramMovement(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDelegatedJudgmentChoicesRuntime))]
internal sealed class CoreDeferredCardsCoreDelegatedJudgmentChoicesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDelegatedJudgmentChoicesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredCardsCoreDelegatedJudgmentChoicesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDelegatedJudgmentChoicesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.CardSnapshot> DelegatedJudgmentPrivateCards(int viewer) => this._targetFactory().DelegatedJudgmentPrivateCards(viewer);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime))]
internal sealed class CoreDeferredCardsCorePlayCardHistoryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredCardsCorePlayCardHistoryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> EventsSinceLastBoundary(global::System.Func<global::CardGame.Core.IGameEvent, bool> boundary) => this._targetFactory().EventsSinceLastBoundary(boundary);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramHandLimitKindExemptionsRuntime))]
internal sealed class CoreDeferredCardsCoreProgramHandLimitKindExemptionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramHandLimitKindExemptionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredCardsCoreProgramHandLimitKindExemptionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramHandLimitKindExemptionsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlySet<global::CardGame.Core.CardKind> GetCurrentHandLimitExemptKinds(global::CardGame.Core.CharacterState owner) => this._targetFactory().GetCurrentHandLimitExemptKinds(owner);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiuLiAnniversaryRuntime))]
internal sealed class CoreDeferredCardsCharacterLiuLiAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuLiAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredCardsCharacterLiuLiAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuLiAnniversaryRuntime> _targetFactory = targetFactory;
    public bool IsAnniversaryLiuLiHandLimitExempt(int seat, global::CardGame.Core.Card card) => this._targetFactory().IsAnniversaryLiuLiHandLimitExempt(seat, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCheZhouAnniversaryRuntime))]
internal sealed class CoreDeferredCardsCharacterCheZhouAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCheZhouAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredCardsCharacterCheZhouAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCheZhouAnniversaryRuntime> _targetFactory = targetFactory;
    public bool IsAnniversaryCheZhouHandExempt(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().IsAnniversaryCheZhouHandExempt(owner, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterJuShouMouRuntime))]
internal sealed class CoreDeferredCardsCharacterJuShouMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterJuShouMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredCardsCharacterJuShouMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterJuShouMouRuntime> _targetFactory = targetFactory;
    public bool IsMouJsHandLimitExempt(int actor, int card) => this._targetFactory().IsMouJsHandLimitExempt(actor, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime))]
internal sealed class CoreDeferredCardsCharacterZhugeGuoLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredCardsCharacterZhugeGuoLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime> _targetFactory = targetFactory;
    public bool IsLeXidiHandLimitExempt(global::CardGame.Core.CharacterState owner, int cardId) => this._targetFactory().IsLeXidiHandLimitExempt(owner, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXieLingYuOlRuntime))]
internal sealed class CoreDeferredCardsCharacterXieLingYuOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXieLingYuOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredCardsCharacterXieLingYuOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXieLingYuOlRuntime> _targetFactory = targetFactory;
    public bool IsXinyouHandLimitExempt(int ownerSeat, int cardId) => this._targetFactory().IsXinyouHandLimitExempt(ownerSeat, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZuoCiHappyGodRuntime))]
internal sealed class CoreDeferredCardsCharacterZuoCiHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZuoCiHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredCardsCharacterZuoCiHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZuoCiHappyGodRuntime> _targetFactory = targetFactory;
    public bool IsHappyZuoCiHandLimitExempt(global::CardGame.Core.CharacterState owner, int cardId) => this._targetFactory().IsHappyZuoCiHandLimitExempt(owner, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuXunWumiaoRuntime))]
internal sealed class CoreDeferredCardsCharacterLuXunWumiaoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuXunWumiaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredCardsCharacterLuXunWumiaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuXunWumiaoRuntime> _targetFactory = targetFactory;
    public bool IsWuXiongmuHandLimitExempt(global::CardGame.Core.CharacterState owner, int cardId) => this._targetFactory().IsWuXiongmuHandLimitExempt(owner, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineInspectedHandSlashesRuntime))]
internal sealed class CoreDeferredCardsCoreInspectedHandSlashesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineInspectedHandSlashesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredCardsCoreInspectedHandSlashesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineInspectedHandSlashesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.CardSnapshot> InspectedHandPrivateCards(int viewer) => this._targetFactory().InspectedHandPrivateCards(viewer);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXiaoQiaoLeRuntime))]
internal sealed class CoreDeferredCardsCharacterXiaoQiaoLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiaoQiaoLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredCardsCharacterXiaoQiaoLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiaoQiaoLeRuntime> _targetFactory = targetFactory;
    public bool IsQiqinHandLimitExempt(global::CardGame.Core.CharacterState owner, int id) => this._targetFactory().IsQiqinHandLimitExempt(owner, id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaoZhiLeRuntime))]
internal sealed class CoreDeferredCardsCharacterCaoZhiLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoZhiLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredCardsCharacterCaoZhiLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoZhiLeRuntime> _targetFactory = targetFactory;
    public bool IsFuleHandLimitExempt(global::CardGame.Core.CharacterState owner, int cardId) => this._targetFactory().IsFuleHandLimitExempt(owner, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhugeLiangMouRuntime))]
internal sealed class CoreDeferredCardsCharacterZhugeLiangMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeLiangMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredCardsCharacterZhugeLiangMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeLiangMouRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.CardSnapshot> MouZhitianPrivateCards(int viewerSeat) => this._targetFactory().MouZhitianPrivateCards(viewerSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZouShiLeRuntime))]
internal sealed class CoreDeferredCardsCharacterZouShiLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZouShiLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredCardsCharacterZouShiLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZouShiLeRuntime> _targetFactory = targetFactory;
    public bool IsYunzhengHandLimitExempt(global::CardGame.Core.CharacterState owner, int cardId) => this._targetFactory().IsYunzhengHandLimitExempt(owner, cardId);
}
