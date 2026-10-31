#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTurnDrawDebtsRuntime))]
internal sealed class CoreProgramOwnedCardsCoreTurnDrawDebtsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnDrawDebtsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOwnedCardsCoreTurnDrawDebtsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnDrawDebtsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramEffect TurnDrawDebtOwnedSelectionEffect(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => this._targetFactory().TurnDrawDebtOwnedSelectionEffect(frame, effect);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtProgramsRuntime))]
internal sealed class CoreProgramOwnedCardsCoreHalfHandPhaseDebtProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOwnedCardsCoreHalfHandPhaseDebtProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtProgramsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramEffect PhaseHandDebtOwnedSelectionEffect(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => this._targetFactory().PhaseHandDebtOwnedSelectionEffect(frame, effect);
    public bool MatchesDeferredHandDebtCost(global::CardGame.Core.ProgramSkillFrame frame, string bind, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation location) => this._targetFactory().MatchesDeferredHandDebtCost(frame, bind, card, location);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime))]
internal sealed class CoreProgramOwnedCardsCoreCardAttackStateAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOwnedCardsCoreCardAttackStateOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card GetAttackCard(int id) => this._targetFactory().GetAttackCard(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileCashOutRuntime))]
internal sealed class CoreProgramOwnedCardsCorePublicPileCashOutAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileCashOutRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOwnedCardsCorePublicPileCashOutOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileCashOutRuntime> _targetFactory = targetFactory;
    public bool MatchesPublicPileStorageSelection(global::CardGame.Core.ProgramSkillFrame frame, string bind, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation location) => this._targetFactory().MatchesPublicPileStorageSelection(frame, bind, card, location);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime))]
internal sealed class CoreProgramOwnedCardsCoreNationalSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOwnedCardsCoreNationalSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> _targetFactory = targetFactory;
    public bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId) => this._targetFactory().HasRuntimeSkillInstance(player, skillId, skillInstanceId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOwnedExactCountAndNamedTargetsRuntime))]
internal sealed class CoreProgramOwnedCardsCoreOwnedExactCountAndNamedTargetsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnedExactCountAndNamedTargetsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOwnedCardsCoreOwnedExactCountAndNamedTargetsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnedExactCountAndNamedTargetsRuntime> _targetFactory = targetFactory;
    public void FreezeExactOwnedSelectionSuits(long frameId, global::CardGame.Core.SkillProgramEffect effect, global::System.Collections.Generic.IReadOnlyList<int> cardIds, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardLocation> locations) => this._targetFactory().FreezeExactOwnedSelectionSuits(frameId, effect, cardIds, locations);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreProgramOwnedCardsCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOwnedCardsCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeTop(next);
    public void AdvanceRuntimeProgram(long frameId) => this._targetFactory().AdvanceRuntimeProgram(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDeferredHandAlignmentsRuntime))]
internal sealed class CoreProgramOwnedCardsCoreDeferredHandAlignmentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeferredHandAlignmentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOwnedCardsCoreDeferredHandAlignmentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeferredHandAlignmentsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramEffect DeferredOwnedSelectionEffect(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect original) => this._targetFactory().DeferredOwnedSelectionEffect(frame, original);
    public void CompleteDeferredAlignmentDiscard(long frameId, global::System.Collections.Generic.IReadOnlyList<int> ids, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardLocation> locations) => this._targetFactory().CompleteDeferredAlignmentDiscard(frameId, ids, locations);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime))]
internal sealed class CoreProgramOwnedCardsCharacterDaQiaoXiaoQiaoHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOwnedCardsCharacterDaQiaoXiaoQiaoHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.HappyQiaoPool? CurrentHappyQiaoPool(int seat, long? parentFrameId = default, bool healthWrite = false) => this._targetFactory().CurrentHappyQiaoPool(seat, parentFrameId, healthWrite);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreProgramOwnedCardsCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOwnedCardsCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId) => this._targetFactory().GetActiveProgramFrame(frameId);
    public void CancelProgramBindingAndCleanup(global::CardGame.Core.ProgramSkillFrame frame, string reason) => this._targetFactory().CancelProgramBindingAndCleanup(frame, reason);
    public void SetProgramCardSet(long frameId, string bind, global::System.Collections.Generic.IReadOnlyList<int> cardIds, global::CardGame.Core.SkillProgramCardSetVisibility visibility, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardLocation>? sourceLocations = default, global::CardGame.Core.Suit? frozenRevealedSuit = default, int? selectionActorSeat = default) => this._targetFactory().SetProgramCardSet(frameId, bind, cardIds, visibility, sourceLocations, frozenRevealedSuit, selectionActorSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime))]
internal sealed class CoreProgramOwnedCardsCoreNativePromptBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOwnedCardsCoreNativePromptBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PromptId CreatePromptId() => this._targetFactory().CreatePromptId();
    public void ClearPendingDecision() => this._targetFactory().ClearPendingDecision();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime))]
internal sealed class CoreProgramOwnedCardsCoreRuleQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOwnedCardsCoreRuleQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime> _targetFactory = targetFactory;
    public int GetLivingPlayersMinHp() => this._targetFactory().GetLivingPlayersMinHp();
    public int GetLivingFactionCount() => this._targetFactory().GetLivingFactionCount();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTriggeredCardConversionsRuntime))]
internal sealed class CoreProgramOwnedCardsCoreTriggeredCardConversionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTriggeredCardConversionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOwnedCardsCoreTriggeredCardConversionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTriggeredCardConversionsRuntime> _targetFactory = targetFactory;
    public bool MatchesTriggeredSlashMaterial(global::CardGame.Core.ProgramSkillFrame frame, string bind, int ownerSeat, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation location) => this._targetFactory().MatchesTriggeredSlashMaterial(frame, bind, ownerSeat, card, location);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSharedSlashOffersRuntime))]
internal sealed class CoreProgramOwnedCardsCoreSharedSlashOffersAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSharedSlashOffersRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOwnedCardsCoreSharedSlashOffersOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSharedSlashOffersRuntime> _targetFactory = targetFactory;
    public bool MatchesProgramOwnedSelectionKind(global::CardGame.Core.ProgramSkillFrame f, string bind, global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardKind> kinds) => this._targetFactory().MatchesProgramOwnedSelectionKind(f, bind, owner, card, from, kinds);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime))]
internal sealed class CoreProgramOwnedCardsCoreProgramCardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOwnedCardsCoreProgramCardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Suit GetProgramEffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation? identityLocation = default) => this._targetFactory().GetProgramEffectiveSuit(owner, card, identityLocation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePindianHandReturnsRuntime))]
internal sealed class CoreProgramOwnedCardsCorePindianHandReturnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianHandReturnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOwnedCardsCorePindianHandReturnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianHandReturnsRuntime> _targetFactory = targetFactory;
    public void FreezePindianHalfReturn(global::CardGame.Core.ProgramSkillFrame f, int count) => this._targetFactory().FreezePindianHalfReturn(f, count);
    public bool HasPaidPindianHandReturn(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().HasPaidPindianHandReturn(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime))]
internal sealed class CoreProgramOwnedCardsCoreProgramCardPaymentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOwnedCardsCoreProgramCardPaymentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime> _targetFactory = targetFactory;
    public int ResolveProgramParticipant(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramParticipantReference reference) => this._targetFactory().ResolveProgramParticipant(frame, reference);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime))]
internal sealed class CoreProgramOwnedCardsCorePhysicalOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOwnedCardsCorePhysicalOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player) => this._targetFactory().GetHand(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTurnHandCategoryRestrictionsRuntime))]
internal sealed class CoreProgramOwnedCardsCoreTurnHandCategoryRestrictionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnHandCategoryRestrictionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOwnedCardsCoreTurnHandCategoryRestrictionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnHandCategoryRestrictionsRuntime> _targetFactory = targetFactory;
    public bool CanSelectSelfDiscardBinding(global::CardGame.Core.ProgramSkillFrame f, string bind, int selectionActorSeat, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation location) => this._targetFactory().CanSelectSelfDiscardBinding(f, bind, selectionActorSeat, card, location);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreForeignEquipmentDiscardPoliciesRuntime))]
internal sealed class CoreProgramOwnedCardsCoreForeignEquipmentDiscardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreForeignEquipmentDiscardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOwnedCardsCoreForeignEquipmentDiscardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreForeignEquipmentDiscardPoliciesRuntime> _targetFactory = targetFactory;
    public bool HasForeignDiscardCapability { get => this._targetFactory().HasForeignDiscardCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineForcedPhaseAlcoholRuntime))]
internal sealed class CoreProgramOwnedCardsCoreForcedPhaseAlcoholAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForcedPhaseAlcoholRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOwnedCardsCoreForcedPhaseAlcoholOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForcedPhaseAlcoholRuntime> _targetFactory = targetFactory;
    public bool HasPhaseAlcoholChain(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().HasPhaseAlcoholChain(frame);
    public bool CanUsePhaseOwnerAlcohol(global::CardGame.Core.ProgramSkillWindowContext? context) => this._targetFactory().CanUsePhaseOwnerAlcohol(context);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCategoryTargetLedgerRuntime))]
internal sealed class CoreProgramOwnedCardsCoreProgramCategoryTargetLedgerAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCategoryTargetLedgerRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOwnedCardsCoreProgramCategoryTargetLedgerOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCategoryTargetLedgerRuntime> _targetFactory = targetFactory;
    public int GetProgramCategoryTargetTurnUsage(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().GetProgramCategoryTargetTurnUsage(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime))]
internal sealed class CoreProgramOwnedCardsCoreNativeCardAppearanceAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOwnedCardsCoreNativeCardAppearanceOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().EffectiveSuit(owner, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentPairPaymentsRuntime))]
internal sealed class CoreProgramOwnedCardsCoreEquipmentPairPaymentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentPairPaymentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOwnedCardsCoreEquipmentPairPaymentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentPairPaymentsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramEffect EquipmentPairOwnedSelectionEffect(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => this._targetFactory().EquipmentPairOwnedSelectionEffect(frame, effect);
    public bool MatchesEquipmentPairOwnedCost(global::CardGame.Core.ProgramSkillFrame frame, string bind, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from) => this._targetFactory().MatchesEquipmentPairOwnedCost(frame, bind, card, from);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreProgramOwnedCardsCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOwnedCardsCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}
