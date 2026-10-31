#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeTrickEffectsRuntime))]
internal sealed class CoreExtraEffectForeignSelectionCoreNativeTrickEffectsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTrickEffectsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtraEffectForeignSelectionCoreNativeTrickEffectsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTrickEffectsRuntime> _targetFactory = targetFactory;
    public void SkipUnavailableTargetCardEffect(global::CardGame.Core.NullificationWindowFrame pending, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardEffectSkipReason reason) => this._targetFactory().SkipUnavailableTargetCardEffect(pending, target, reason);
    public void ApplyTargetCardEffect(long resolutionId, global::CardGame.Core.Card effectCard, global::CardGame.Core.CardKind effectiveCardKind, int sourceSeat, int targetSeat, global::CardGame.Core.LegalActionKind actionKind, global::CardGame.Core.Card targetCard, global::CardGame.Core.CardLocation targetCardFromZone) => this._targetFactory().ApplyTargetCardEffect(resolutionId, effectCard, effectiveCardKind, sourceSeat, targetSeat, actionKind, targetCard, targetCardFromZone);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime))]
internal sealed class CoreExtraEffectForeignSelectionCoreCardUseLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtraEffectForeignSelectionCoreCardUseLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId) => this._targetFactory().LifecycleCardUse(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime))]
internal sealed class CoreExtraEffectForeignSelectionCorePhysicalOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtraEffectForeignSelectionCorePhysicalOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player) => this._targetFactory().GetHand(player);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player) => this._targetFactory().GetEquipment(player);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetJudgment(global::CardGame.Core.CharacterState player) => this._targetFactory().GetJudgment(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDamageColorDuelProgramsRuntime))]
internal sealed class CoreExtraEffectForeignSelectionCoreDamageColorDuelProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageColorDuelProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtraEffectForeignSelectionCoreDamageColorDuelProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageColorDuelProgramsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card GetTrickRepresentation(long frameId, int cardId, bool requireProcessing = false) => this._targetFactory().GetTrickRepresentation(frameId, cardId, requireProcessing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardUseFramesRuntime))]
internal sealed class CoreExtraEffectForeignSelectionCoreNativeCardUseFramesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseFramesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtraEffectForeignSelectionCoreNativeCardUseFramesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseFramesRuntime> _targetFactory = targetFactory;
    public void SetCardUseStep(long frameId, global::CardGame.Core.ResolutionFrameStep step) => this._targetFactory().SetCardUseStep(frameId, step);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime))]
internal sealed class CoreExtraEffectForeignSelectionCoreResolutionIdentityBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtraEffectForeignSelectionCoreResolutionIdentityBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime> _targetFactory = targetFactory;
    public long _resolutionSequence { get => this._targetFactory()._resolutionSequence; set => this._targetFactory()._resolutionSequence = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime))]
internal sealed class CoreExtraEffectForeignSelectionCoreCardAttackStateAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtraEffectForeignSelectionCoreCardAttackStateOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card GetAttackCard(int id) => this._targetFactory().GetAttackCard(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreForeignEquipmentDiscardPoliciesRuntime))]
internal sealed class CoreExtraEffectForeignSelectionCoreForeignEquipmentDiscardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreForeignEquipmentDiscardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtraEffectForeignSelectionCoreForeignEquipmentDiscardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreForeignEquipmentDiscardPoliciesRuntime> _targetFactory = targetFactory;
    public bool IsForeignEquipmentDiscardPrevented(int actorSeat, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.OwnedCardMoveIntent intent) => this._targetFactory().IsForeignEquipmentDiscardPrevented(actorSeat, card, from, intent);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreExtraEffectForeignSelectionCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtraEffectForeignSelectionCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreExtraEffectForeignSelectionCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtraEffectForeignSelectionCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame) => this._targetFactory().PushRuntimeFrame(frame);
    public void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind) => this._targetFactory().PopResolutionFrame(frameId, expectedKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime))]
internal sealed class CoreExtraEffectForeignSelectionCharacterZhugeGuoLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtraEffectForeignSelectionCharacterZhugeGuoLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime> _targetFactory = targetFactory;
    public bool IsLeChengyanEffectUse(long id) => this._targetFactory().IsLeChengyanEffectUse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime))]
internal sealed class CoreExtraEffectForeignSelectionCoreNativePromptBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtraEffectForeignSelectionCoreNativePromptBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PromptId CreatePromptId() => this._targetFactory().CreatePromptId();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCurrentUsePhysicalClaimsRuntime))]
internal sealed class CoreExtraEffectForeignSelectionCoreCurrentUsePhysicalClaimsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentUsePhysicalClaimsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtraEffectForeignSelectionCoreCurrentUsePhysicalClaimsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentUsePhysicalClaimsRuntime> _targetFactory = targetFactory;
    public bool IsClaimedUseCardEntity(long frameId, int cardId) => this._targetFactory().IsClaimedUseCardEntity(frameId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWushuangCombatTacticsRuntime))]
internal sealed class CoreExtraEffectForeignSelectionCoreWushuangCombatTacticsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangCombatTacticsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtraEffectForeignSelectionCoreWushuangCombatTacticsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangCombatTacticsRuntime> _targetFactory = targetFactory;
    public bool WushuangHasVisibleTargetHand(int source, global::CardGame.Core.CardKind kind) => this._targetFactory().WushuangHasVisibleTargetHand(source, kind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineExtraCardEffectRoundsRuntime))]
internal sealed class CoreExtraEffectForeignSelectionCoreExtraCardEffectRoundsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtraCardEffectRoundsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtraEffectForeignSelectionCoreExtraCardEffectRoundsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtraCardEffectRoundsRuntime> _targetFactory = targetFactory;
    public bool IsExtraCardEffectRound(long id) => this._targetFactory().IsExtraCardEffectRound(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineMantianGuohaiRuntime))]
internal sealed class CoreExtraEffectForeignSelectionCoreMantianGuohaiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMantianGuohaiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtraEffectForeignSelectionCoreMantianGuohaiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMantianGuohaiRuntime> _targetFactory = targetFactory;
    public bool IsMantianGuohaiUse(long id) => this._targetFactory().IsMantianGuohaiUse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWushuangTargetCardsRuntime))]
internal sealed class CoreExtraEffectForeignSelectionCoreWushuangTargetCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangTargetCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtraEffectForeignSelectionCoreWushuangTargetCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangTargetCardsRuntime> _targetFactory = targetFactory;
    public bool IsWushuangExtraForeignSelection(long useId) => this._targetFactory().IsWushuangExtraForeignSelection(useId);
    public void FinishWushuangTargetCards(global::CardGame.Core.CardUseFrame use) => this._targetFactory().FinishWushuangTargetCards(use);
}
