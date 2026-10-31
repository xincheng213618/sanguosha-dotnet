#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRoundGainedSourceUsesRuntime))]
internal sealed class CharacterKeBiNengOlCoreRoundGainedSourceUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundGainedSourceUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCoreRoundGainedSourceUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundGainedSourceUsesRuntime> _targetFactory = targetFactory;
    public bool HasIssuedRoundGainedBasicBonus(long useId, int actor) => this._targetFactory().HasIssuedRoundGainedBasicBonus(useId, actor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMaXiuMaTieOlRuntime))]
internal sealed class CharacterKeBiNengOlCharacterMaXiuMaTieOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaXiuMaTieOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCharacterMaXiuMaTieOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaXiuMaTieOlRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> KenshangProgramSlashChoices(global::CardGame.Core.ProgramSkillFrame f, int actorSeat, global::System.Collections.Generic.IReadOnlyList<int> targets, string? resultBind = default) => this._targetFactory().KenshangProgramSlashChoices(f, actorSeat, targets, resultBind);
    public bool HasAcceptedKenshangKeAction(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.LegalAction action) => this._targetFactory().HasAcceptedKenshangKeAction(f, action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHandComparisonRuntime))]
internal sealed class CharacterKeBiNengOlCoreHandComparisonAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHandComparisonRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCoreHandComparisonOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHandComparisonRuntime> _targetFactory = targetFactory;
    public bool HasSelfCardTargetProhibition(int seat) => this._targetFactory().HasSelfCardTargetProhibition(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEphemeralCardMaterialsRuntime))]
internal sealed class CharacterKeBiNengOlCoreEphemeralCardMaterialsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEphemeralCardMaterialsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCoreEphemeralCardMaterialsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEphemeralCardMaterialsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardLocation NormalizeEphemeralCardDestination(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason? reason = default) => this._targetFactory().NormalizeEphemeralCardDestination(card, from, to, reason);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiuFengBoundaryRuntime))]
internal sealed class CharacterKeBiNengOlCharacterLiuFengBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuFengBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCharacterLiuFengBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuFengBoundaryRuntime> _targetFactory = targetFactory;
    public bool HasBoundaryLiuFengGenericBasic(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().HasBoundaryLiuFengGenericBasic(f);
    public bool HasAcceptedBoundaryLiuFengKeAction(global::CardGame.Core.ProgramSkillFrame parent, global::CardGame.Core.LegalAction selected) => this._targetFactory().HasAcceptedBoundaryLiuFengKeAction(parent, selected);
    public bool BoundaryLiuFengRequestChild(global::CardGame.Core.ProgramSkillFrame parent, global::CardGame.Core.ResolutionFrame child) => this._targetFactory().BoundaryLiuFengRequestChild(parent, child);
    public global::CardGame.Core.ProgramSkillFrame? BoundaryLiuFengNativeParent(global::CardGame.Core.CardUseFrame use) => this._targetFactory().BoundaryLiuFengNativeParent(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicPaymentRuntime))]
internal sealed class CharacterKeBiNengOlCoreDrawFundedDistinctBasicPaymentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicPaymentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCoreDrawFundedDistinctBasicPaymentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicPaymentRuntime> _targetFactory = targetFactory;
    public void BeginDrawFundedDistinctBasic(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardConversionSource source, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> targets, global::CardGame.Core.PendingDecision original, global::CardGame.Core.GameEngine.DrawFundedBasicNeed need) => this._targetFactory().BeginDrawFundedDistinctBasic(actor, source, kind, targets, original, need);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicConversionsRuntime))]
internal sealed class CharacterKeBiNengOlCoreChainedStateBasicConversionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicConversionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCoreChainedStateBasicConversionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicConversionsRuntime> _targetFactory = targetFactory;
    public void BeginChainedStateBasic(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardConversionSource source, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> targets, global::CardGame.Core.PendingDecision original, global::CardGame.Core.GameEngine.ChainedStateBasicNeed need) => this._targetFactory().BeginChainedStateBasic(actor, source, kind, targets, original, need);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime))]
internal sealed class CharacterKeBiNengOlCoreCardConversionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCoreCardConversionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramMultiCardViewAsSelection> GetProgramMultiCardViewAsSelections(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind outputKind, bool forResponse, bool ignoreSuitUseProhibition = false, int? requestedUseTarget = default) => this._targetFactory().GetProgramMultiCardViewAsSelections(owner, outputKind, forResponse, ignoreSuitUseProhibition, requestedUseTarget);
    public global::CardGame.Core.ProgramMultiCardViewAsSelection? FindProgramMultiCardViewAsSelection(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int> cardIds, global::CardGame.Core.CardKind outputKind, bool forResponse, global::CardGame.Core.CardConversionSource source) => this._targetFactory().FindProgramMultiCardViewAsSelection(owner, cardIds, outputKind, forResponse, source);
    public bool IgnoresProgramSlashLimit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardConversionSource? source) => this._targetFactory().IgnoresProgramSlashLimit(owner, source);
    public string DescribeConversion(global::CardGame.Core.CardConversionSource? source, string description) => this._targetFactory().DescribeConversion(source, description);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuangGaiMouMobileRuntime))]
internal sealed class CharacterKeBiNengOlCharacterHuangGaiMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangGaiMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCharacterHuangGaiMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangGaiMouMobileRuntime> _targetFactory = targetFactory;
    public void RecordMouZhaJiangAwareAlcoholDebit(long useId, global::CardGame.Core.CharacterState actor) => this._targetFactory().RecordMouZhaJiangAwareAlcoholDebit(useId, actor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CharacterKeBiNengOlCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePrivateOfferReturnsRuntime))]
internal sealed class CharacterKeBiNengOlCorePrivateOfferReturnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrivateOfferReturnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCorePrivateOfferReturnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrivateOfferReturnsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramEffect? GetPausedPrivateOfferEffect(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().GetPausedPrivateOfferEffect(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuoTuOlRuntime))]
internal sealed class CharacterKeBiNengOlCharacterGuoTuOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoTuOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCharacterGuoTuOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoTuOlRuntime> _targetFactory = targetFactory;
    public bool HasWeijieGenericBasic(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().HasWeijieGenericBasic(f);
    public bool HasAcceptedWeijieKeAction(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.LegalAction a) => this._targetFactory().HasAcceptedWeijieKeAction(f, a);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedLifecycleRuntime))]
internal sealed class CharacterKeBiNengOlCoreAdvancedLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCoreAdvancedLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardKind AdvancedEffectiveHandKind(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().AdvancedEffectiveHandKind(owner, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFinalizedCardTargetsRuntime))]
internal sealed class CharacterKeBiNengOlCoreFinalizedCardTargetsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFinalizedCardTargetsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCoreFinalizedCardTargetsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFinalizedCardTargetsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<int> GetImplicitSelfCardUseTargets(global::CardGame.Core.CardKind kind, int sourceSeat, global::System.Collections.Generic.IReadOnlyList<int> targets) => this._targetFactory().GetImplicitSelfCardUseTargets(kind, sourceSeat, targets);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime))]
internal sealed class CharacterKeBiNengOlCorePhysicalMovementFacadesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCorePhysicalMovementFacadesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> _targetFactory = targetFactory;
    public void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0) => this._targetFactory().MoveCard(card, from, to, reason, beforeFact, emitCardMovedEvents, disclosure);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime))]
internal sealed class CharacterKeBiNengOlCoreCardUseLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCoreCardUseLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> _targetFactory = targetFactory;
    public void BeginSimpleCardUse(long frameId, global::CardGame.Core.ProgramSimpleCardContinuation continuation) => this._targetFactory().BeginSimpleCardUse(frameId, continuation);
    public void UpdateLifecycleCardUse(long frameId, global::System.Func<global::CardGame.Core.CardUseFrame, global::CardGame.Core.CardUseFrame> update) => this._targetFactory().UpdateLifecycleCardUse(frameId, update);
    public global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId) => this._targetFactory().LifecycleCardUse(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CharacterKeBiNengOlCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime))]
internal sealed class CharacterKeBiNengOlCorePlayerViewProjectionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCorePlayerViewProjectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuanShuJunOlRuntime))]
internal sealed class CharacterKeBiNengOlCharacterHuanShuJunOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuanShuJunOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCharacterHuanShuJunOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuanShuJunOlRuntime> _targetFactory = targetFactory;
    public bool HasHuanpeiGenericBasic(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().HasHuanpeiGenericBasic(f);
    public bool HasAcceptedHuanpeiKeAction(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.LegalAction action) => this._targetFactory().HasAcceptedHuanpeiKeAction(f, action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime))]
internal sealed class CharacterKeBiNengOlCorePhysicalDrawFacadesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCorePhysicalDrawFacadesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<int> DrawCards(global::CardGame.Core.CharacterState player, int count, bool log, global::CardGame.Core.CardMoveReason? reason = default, global::CardGame.Core.CardConversionSource? directProducer = default) => this._targetFactory().DrawCards(player, count, log, reason, directProducer);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWoLongFengChuOlRuntime))]
internal sealed class CharacterKeBiNengOlCharacterWoLongFengChuOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWoLongFengChuOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCharacterWoLongFengChuOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWoLongFengChuOlRuntime> _targetFactory = targetFactory;
    public bool IsEquipmentSlotNamedSkill(string skill) => this._targetFactory().IsEquipmentSlotNamedSkill(skill);
    public bool TryBeginEquipmentSlotNamedPlay(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action) => this._targetFactory().TryBeginEquipmentSlotNamedPlay(actor, action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime))]
internal sealed class CharacterKeBiNengOlCoreSkillProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCoreSkillProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime> _targetFactory = targetFactory;
    public void FinishProgramSkill(global::CardGame.Core.ProgramSkillFrame frame, bool completed) => this._targetFactory().FinishProgramSkill(frame, completed);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeOwnedCardQueriesRuntime))]
internal sealed class CharacterKeBiNengOlCoreNativeOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCoreNativeOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardLocation FindOwnedCardLocation(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card) => this._targetFactory().FindOwnedCardLocation(actor, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime))]
internal sealed class CharacterKeBiNengOlCorePlayCardHistoryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCorePlayCardHistoryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramViewAs? ViewAsRule(global::CardGame.Core.CardConversionSource source) => this._targetFactory().ViewAsRule(source);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineObserverSubtreesRuntime))]
internal sealed class CharacterKeBiNengOlCoreObserverSubtreesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineObserverSubtreesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCoreObserverSubtreesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineObserverSubtreesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillFrame? FindProgramObserverRoot(global::System.Func<global::CardGame.Core.ProgramSkillFrame, global::CardGame.Core.ResolutionFrame, bool> firstChild, global::System.Func<int, bool> suffix, long? requiredRootId = default) => this._targetFactory().FindProgramObserverRoot(firstChild, suffix, requiredRootId);
    public bool TryAdvanceObserverSubtree(bool advanceRecoveryReplacement, string? incompleteAttackError = default, bool routeUnreturnedAttackThroughRuntime = false) => this._targetFactory().TryAdvanceObserverSubtree(advanceRecoveryReplacement, incompleteAttackError, routeUnreturnedAttackThroughRuntime);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime))]
internal sealed class CharacterKeBiNengOlCoreDyingCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCoreDyingCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DyingFrame? ActiveDying { get => this._targetFactory().ActiveDying; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeTargetLegalityRuntime))]
internal sealed class CharacterKeBiNengOlCoreNativeTargetLegalityAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTargetLegalityRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCoreNativeTargetLegalityOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTargetLegalityRuntime> _targetFactory = targetFactory;
    public bool CanUseVirtualSlashTarget(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind effectiveKind = (global::CardGame.Core.CardKind)0, global::CardGame.Core.Suit? physicalSuit = default, bool? effectiveColor = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, bool ignoreDistance = false, global::CardGame.Core.CardConversionSource? conversionSource = default, bool actualUnconvertedPhysical = false, bool countsTowardLimit = true) => this._targetFactory().CanUseVirtualSlashTarget(source, target, effectiveKind, physicalSuit, effectiveColor, effectiveRank, physicalCardIds, ignoreDistance, conversionSource, actualUnconvertedPhysical, countsTowardLimit);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTurnRedSlashBenefitsRuntime))]
internal sealed class CharacterKeBiNengOlCoreTurnRedSlashBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnRedSlashBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCoreTurnRedSlashBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnRedSlashBenefitsRuntime> _targetFactory = targetFactory;
    public bool? PhysicalGroupColor(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards) => this._targetFactory().PhysicalGroupColor(owner, cards);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNativeRecastInvocationsRuntime))]
internal sealed class CharacterKeBiNengOlCoreNativeRecastInvocationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeRecastInvocationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCoreNativeRecastInvocationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeRecastInvocationsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillFrame BeginFrameNativeRecastInvocation(global::CardGame.Core.ProgramSkillFrame frame, int executorSeat, global::System.Collections.Generic.IReadOnlyList<int> originalPrimaryIds, int? participantCursor = default) => this._targetFactory().BeginFrameNativeRecastInvocation(frame, executorSeat, originalPrimaryIds, participantCursor);
    public global::CardGame.Core.ProgramSkillFrame CompleteFrameNativeRecastInvocation(global::CardGame.Core.ProgramSkillFrame frame, int actualDrawCount) => this._targetFactory().CompleteFrameNativeRecastInvocation(frame, actualDrawCount);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime))]
internal sealed class CharacterKeBiNengOlCoreCardAttackStateAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCoreCardAttackStateOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card GetAttackCard(int id) => this._targetFactory().GetAttackCard(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRoundDistinctBasicPlayRuntime))]
internal sealed class CharacterKeBiNengOlCoreRoundDistinctBasicPlayAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundDistinctBasicPlayRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCoreRoundDistinctBasicPlayOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundDistinctBasicPlayRuntime> _targetFactory = targetFactory;
    public bool CanUseRoundDistinctBasicAlcohol(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards) => this._targetFactory().CanUseRoundDistinctBasicAlcohol(owner, cards);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeActionDispatchRuntime))]
internal sealed class CharacterKeBiNengOlCoreNativeActionDispatchAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeActionDispatchRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCoreNativeActionDispatchOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeActionDispatchRuntime> _targetFactory = targetFactory;
    public void ExecuteAction(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action, global::System.Collections.Generic.IReadOnlyList<int>? activeSkillCardIds = default, global::System.Collections.Generic.IReadOnlyList<int>? activeSkillTargetSeats = default) => this._targetFactory().ExecuteAction(actor, action, activeSkillCardIds, activeSkillTargetSeats);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShownEntityTurnPoliciesRuntime))]
internal sealed class CharacterKeBiNengOlCoreShownEntityTurnPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShownEntityTurnPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCoreShownEntityTurnPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShownEntityTurnPoliciesRuntime> _targetFactory = targetFactory;
    public bool ShownEntityUseActorMatches(global::CardGame.Core.CardUseFrame use, int originalActor, int provider) => this._targetFactory().ShownEntityUseActorMatches(use, originalActor, provider);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime))]
internal sealed class CharacterKeBiNengOlCorePhysicalOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCorePhysicalOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player) => this._targetFactory().GetHand(player);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(int seat) => this._targetFactory().GetEquipment(seat);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetWoodenOxGrain(global::CardGame.Core.CharacterState player) => this._targetFactory().GetWoodenOxGrain(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeSlashUseRuntime))]
internal sealed class CharacterKeBiNengOlCoreNativeSlashUseAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSlashUseRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCoreNativeSlashUseOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSlashUseRuntime> _targetFactory = targetFactory;
    public void ResolveSlashCore(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card slash, global::CardGame.Core.CardKind playedCardKind, int physicalOwnerSeat, global::CardGame.Core.NativeOwners.FactionCardRequestHandle? factionRequest = default, global::CardGame.Core.NativeOwners.BorrowedSwordHandle? borrowedSword = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? physicalCards = default, bool countsTowardSlashLimit = true, bool usesZhuqueFan = false, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default, long? programSkillCardUseFrameId = default, global::CardGame.Core.CardLocation? physicalSourceLocation = default) => this._targetFactory().ResolveSlashCore(source, target, slash, playedCardKind, physicalOwnerSeat, factionRequest, borrowedSword, physicalCards, countsTowardSlashLimit, usesZhuqueFan, conversionSource, additionalConversionSources, programSkillCardUseFrameId, physicalSourceLocation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTreatedWoundedRuntime))]
internal sealed class CharacterKeBiNengOlCoreTreatedWoundedAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTreatedWoundedRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCoreTreatedWoundedOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTreatedWoundedRuntime> _targetFactory = targetFactory;
    public bool IsRuleWounded(global::CardGame.Core.CharacterState player) => this._targetFactory().IsRuleWounded(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeLegalActionsRuntime))]
internal sealed class CharacterKeBiNengOlCoreNativeLegalActionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeLegalActionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCoreNativeLegalActionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeLegalActionsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> BuildLegalActions(global::CardGame.Core.CharacterState actor, bool includeProgramActions = true, int? selectedPhysicalCardId = default) => this._targetFactory().BuildLegalActions(actor, includeProgramActions, selectedPhysicalCardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime))]
internal sealed class CharacterKeBiNengOlCoreNativePromptBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCoreNativePromptBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PromptId CreatePromptId() => this._targetFactory().CreatePromptId();
    public void ClearPendingDecision() => this._targetFactory().ClearPendingDecision();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDeckEquipmentProgramsRuntime))]
internal sealed class CharacterKeBiNengOlCoreDeckEquipmentProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeckEquipmentProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCoreDeckEquipmentProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeckEquipmentProgramsRuntime> _targetFactory = targetFactory;
    public bool HasBeneficiarySuitShield(int actor, int target, global::CardGame.Core.Suit? suit) => this._targetFactory().HasBeneficiarySuitShield(actor, target, suit);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeResponseMaterialQueriesRuntime))]
internal sealed class CharacterKeBiNengOlCoreNativeResponseMaterialQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeResponseMaterialQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCoreNativeResponseMaterialQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeResponseMaterialQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>> GetZhangbaSlashPairs(global::CardGame.Core.CharacterState responder) => this._targetFactory().GetZhangbaSlashPairs(responder);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardUseRuntime))]
internal sealed class CharacterKeBiNengOlCoreNativeCardUseAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCoreNativeCardUseOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseRuntime> _targetFactory = targetFactory;
    public long BeginCardUse(global::CardGame.Core.Card card, int sourceSeat, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, global::CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default, global::System.Collections.Generic.IReadOnlyList<int>? designatedTargetSeats = default, bool isTrueZhangbaSlash = false, global::CardGame.Core.ProgramSelectedActorDuelOrigin? selectedActorDuelOrigin = default, global::CardGame.Core.ProgramDamageTargetDuelOrigin? damageTargetDuelOrigin = default, global::CardGame.Core.ProgramDualColorDuelOrigin? dualColorDuelOrigin = default, global::CardGame.Core.ConditionalDiscardDuelOrigin? conditionalDiscardDuelOrigin = default, global::CardGame.Core.ShiQuanDuelOrigin? shiQuanDuelOrigin = default, global::CardGame.Core.ProgramVirtualOrdinaryTrickOrigin? virtualOrdinaryTrickOrigin = default, global::CardGame.Core.DrawFundedDistinctBasicPayment? drawFundedPayment = default, global::CardGame.Core.ChainedStateBasicPayment? chainedStatePayment = default, global::CardGame.Core.FriendRecastDuelOrigin? friendRecastDuel = default, global::CardGame.Core.ChosenAssaultReturn? chosenAssault = default, global::CardGame.Core.ZhenyingDuelOrigin? zhenyingDuel = default, global::CardGame.Core.HeJinMouzhuUseOrigin? heJinMouzhu = default, global::CardGame.Core.WuAnGuoLiyongDuelOrigin? wuAnGuoLiyong = default, global::CardGame.Core.TianChouShandaoUseOrigin? tianChouShandao = default, global::CardGame.Core.QinLangXianyingUseOrigin? qinLangXianying = default, global::CardGame.Core.AHuiNanJuemanUseOrigin? aHuiNanJueman = default, global::CardGame.Core.LiuYanLimuUseOrigin? liuYanLimu = default, global::CardGame.Core.JueshengDuelOrigin? jueshengDuel = default, global::CardGame.Core.QuHuangNishouUseOrigin? quHuangNishou = default, global::CardGame.Core.WeijiePaidOrigin? weijieOrigin = default, global::CardGame.Core.ZonghuPaidOrigin? zonghuOrigin = default, global::CardGame.Core.HuanpeiPaidOrigin? huanpeiOrigin = default) => this._targetFactory().BeginCardUse(card, sourceSeat, targetSeats, playedCardKind, ignoresArmor, physicalCardIds, conversionSource, additionalConversionSources, designatedTargetSeats, isTrueZhangbaSlash, selectedActorDuelOrigin, damageTargetDuelOrigin, dualColorDuelOrigin, conditionalDiscardDuelOrigin, shiQuanDuelOrigin, virtualOrdinaryTrickOrigin, drawFundedPayment, chainedStatePayment, friendRecastDuel, chosenAssault, zhenyingDuel, heJinMouzhu, wuAnGuoLiyong, tianChouShandao, qinLangXianying, aHuiNanJueman, liuYanLimu, jueshengDuel, quHuangNishou, weijieOrigin, zonghuOrigin, huanpeiOrigin);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterChengJiChengCuiOlRuntime))]
internal sealed class CharacterKeBiNengOlCharacterChengJiChengCuiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengJiChengCuiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCharacterChengJiChengCuiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengJiChengCuiOlRuntime> _targetFactory = targetFactory;
    public bool HasTousuiProgramSlash(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IEnumerable<int> targets) => this._targetFactory().HasTousuiProgramSlash(actor, targets);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> TousuiProgramSlashChoices(global::CardGame.Core.ProgramSkillFrame f, int actorSeat, global::System.Collections.Generic.IReadOnlyList<int> targets, string? resultBind = default) => this._targetFactory().TousuiProgramSlashChoices(f, actorSeat, targets, resultBind);
    public bool HasAcceptedTousuiKeAction(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.LegalAction action) => this._targetFactory().HasAcceptedTousuiKeAction(f, action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime))]
internal sealed class CharacterKeBiNengOlCoreNationalSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCoreNationalSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> _targetFactory = targetFactory;
    public bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId) => this._targetFactory().HasRuntimeSkillInstance(player, skillId, skillInstanceId);
    public global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player) => this._targetFactory().GetSkillBindingShard(player);
    public bool HasRuntimeSkill(global::CardGame.Core.CharacterState player, string skillId) => this._targetFactory().HasRuntimeSkill(player, skillId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentSkillsRuntime))]
internal sealed class CharacterKeBiNengOlCoreEquipmentSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCoreEquipmentSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentSkillsRuntime> _targetFactory = targetFactory;
    public bool IsActiveProgramSourceEquipmentCard(int ownerSeat, string skillId, string skillInstanceId, global::CardGame.Core.Card card) => this._targetFactory().IsActiveProgramSourceEquipmentCard(ownerSeat, skillId, skillInstanceId, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamagePoliciesRuntime))]
internal sealed class CharacterKeBiNengOlCoreNativeDamagePoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamagePoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCoreNativeDamagePoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamagePoliciesRuntime> _targetFactory = targetFactory;
    public bool IsCardTargetProhibited(global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind cardKind, global::CardGame.Core.Suit? suit = default, bool? effectiveIsRed = default) => this._targetFactory().IsCardTargetProhibited(target, cardKind, suit, effectiveIsRed);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFinalTargetSlashRuntime))]
internal sealed class CharacterKeBiNengOlCoreFinalTargetSlashAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFinalTargetSlashRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCoreFinalTargetSlashOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFinalTargetSlashRuntime> _targetFactory = targetFactory;
    public int? ZhangbaSpecificSlashRank(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CharacterState? provider = default) => this._targetFactory().ZhangbaSpecificSlashRank(actor, cards, provider);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramContinuationsRuntime))]
internal sealed class CharacterKeBiNengOlCoreProgramContinuationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramContinuationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCoreProgramContinuationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramContinuationsRuntime> _targetFactory = targetFactory;
    public bool TryBeginProgramChildWindows(long frameId, global::CardGame.Core.PostEventContinuation continuation) => this._targetFactory().TryBeginProgramChildWindows(frameId, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CharacterKeBiNengOlCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeTop(next);
    public void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeFrame(expectedFrameId, next);
    public void AdvanceRuntimeProgram(long frameId) => this._targetFactory().AdvanceRuntimeProgram(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFriendTrioRuntime))]
internal sealed class CharacterKeBiNengOlCoreFriendTrioAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFriendTrioRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCoreFriendTrioOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFriendTrioRuntime> _targetFactory = targetFactory;
    public bool HasFriendTrioAlly(global::CardGame.Core.CharacterState owner, string id) => this._targetFactory().HasFriendTrioAlly(owner, id);
    public bool FriendXuanjianIgnoresDistance(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardConversionSource? source) => this._targetFactory().FriendXuanjianIgnoresDistance(owner, source);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime))]
internal sealed class CharacterKeBiNengOlCoreRuleQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCoreRuleQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime> _targetFactory = targetFactory;
    public bool CanSpendSlashUse(global::CardGame.Core.CharacterState player, global::CardGame.Core.CharacterState target, bool ignoresCount, global::CardGame.Core.CardKind effectiveKind = (global::CardGame.Core.CardKind)0, global::CardGame.Core.Card? physicalCard = default, int? effectiveRank = default, int? excludedEquipmentId = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default, bool ignoreMoQuanyu = false, global::CardGame.Core.Suit? effectiveSuit = default) => this._targetFactory().CanSpendSlashUse(player, target, ignoresCount, effectiveKind, physicalCard, effectiveRank, excludedEquipmentId, physicalCardIds, excludedEquipmentSlot, ignoreMoQuanyu, effectiveSuit);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiuZhangOlRuntime))]
internal sealed class CharacterKeBiNengOlCharacterLiuZhangOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuZhangOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCharacterLiuZhangOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuZhangOlRuntime> _targetFactory = targetFactory;
    public bool HasZonghuGenericBasic(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().HasZonghuGenericBasic(f);
    public bool HasAcceptedZonghuKeAction(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.LegalAction a) => this._targetFactory().HasAcceptedZonghuKeAction(f, a);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime))]
internal sealed class CharacterKeBiNengOlCoreNativeCardAppearanceAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCoreNativeCardAppearanceOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().EffectiveSuit(owner, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeRecoveryCardsRuntime))]
internal sealed class CharacterKeBiNengOlCoreNativeRecoveryCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRecoveryCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCoreNativeRecoveryCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRecoveryCardsRuntime> _targetFactory = targetFactory;
    public void ResolveRecoveryCard(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card card, string cardName, global::CardGame.Core.CardKind? playedCardKind = default, int recoveryAmount = 1, global::System.Collections.Generic.IReadOnlyList<(string SkillId, string PolicyId)>? recoveryPolicySources = default, global::CardGame.Core.CardConversionSource? conversionSource = default, global::CardGame.Core.DyingResponseEvent? dyingResponse = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? physicalCards = default, global::CardGame.Core.DrawFundedDistinctBasicPayment? drawFundedPayment = default, global::CardGame.Core.HuanpeiPaidOrigin? huanpeiOrigin = default) => this._targetFactory().ResolveRecoveryCard(source, target, card, cardName, playedCardKind, recoveryAmount, recoveryPolicySources, conversionSource, dyingResponse, physicalCards, drawFundedPayment, huanpeiOrigin);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime))]
internal sealed class CharacterKeBiNengOlCoreAlternatingBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCoreAlternatingBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory() => this._targetFactory().CompleteProgramEventHistory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingSuitAndCompletedTopRuntime))]
internal sealed class CharacterKeBiNengOlCoreAlternatingSuitAndCompletedTopAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingSuitAndCompletedTopRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCoreAlternatingSuitAndCompletedTopOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingSuitAndCompletedTopRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Suit? PhysicalGroupSuit(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards) => this._targetFactory().PhysicalGroupSuit(owner, cards);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CharacterKeBiNengOlCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterKeBiNengOlCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId) => this._targetFactory().GetActiveProgramFrame(frameId);
}
