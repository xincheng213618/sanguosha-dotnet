#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramChildResolutionRuntime))]
internal sealed class CoreSameNameHandDiscardOrDamageObserversCoreProgramChildResolutionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramChildResolutionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSameNameHandDiscardOrDamageObserversCoreProgramChildResolutionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramChildResolutionRuntime> _targetFactory = targetFactory;
    public bool ProgramChildResolutionEdge(int index) => this._targetFactory().ProgramChildResolutionEdge(index);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSameNameHandDiscardOrDamageRuntime))]
internal sealed class CoreSameNameHandDiscardOrDamageObserversCoreSameNameHandDiscardOrDamageAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSameNameHandDiscardOrDamageRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSameNameHandDiscardOrDamageObserversCoreSameNameHandDiscardOrDamageOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSameNameHandDiscardOrDamageRuntime> _targetFactory = targetFactory;
    public bool SameNameHandActionParent(int owner, global::CardGame.Core.ProgramSkillWindowContext context, out global::CardGame.Core.ProgramCardTriggerWindowFrame window) => this._targetFactory().SameNameHandActionParent(owner, context, out window);
    public global::CardGame.Core.SameNameHandMaterial[] SameNameHandMaterials(int seat, global::CardGame.Core.CardKind name) => this._targetFactory().SameNameHandMaterials(seat, name);
    public string SameNameHandDiscardReason(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().SameNameHandDiscardReason(f);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> SameNameHandChoices(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().SameNameHandChoices(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionRuntime))]
internal sealed class CoreSameNameHandDiscardOrDamageObserversCoreResponseCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSameNameHandDiscardOrDamageObserversCoreResponseCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionRuntime> _targetFactory = targetFactory;
    public bool ResponseCompletionStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => this._targetFactory().ResponseCompletionStructuralEdge(parent, child);
    public bool ValidResponseCompletionWindow(global::CardGame.Core.ProgramCardTriggerWindowFrame window) => this._targetFactory().ValidResponseCompletionWindow(window);
    public bool ResponseCompletionFirstChild(global::CardGame.Core.ProgramCardTriggerWindowFrame window, global::CardGame.Core.ResolutionFrame child) => this._targetFactory().ResponseCompletionFirstChild(window, child);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineObserverSubtreesRuntime))]
internal sealed class CoreSameNameHandDiscardOrDamageObserversCoreObserverSubtreesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineObserverSubtreesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSameNameHandDiscardOrDamageObserversCoreObserverSubtreesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineObserverSubtreesRuntime> _targetFactory = targetFactory;
    public bool HasObserverSuffix(int rootIndex, global::System.Func<int, bool> edge, global::System.Func<int, global::CardGame.Core.DyingFrame, bool>? completedDyingSuffix) => this._targetFactory().HasObserverSuffix(rootIndex, edge, completedDyingSuffix);
    public global::CardGame.Core.ProgramSkillFrame? FindProgramObserverRoot(global::System.Func<global::CardGame.Core.ProgramSkillFrame, global::CardGame.Core.ResolutionFrame, bool> firstChild, global::System.Func<int, bool> suffix, long? requiredRootId = default) => this._targetFactory().FindProgramObserverRoot(firstChild, suffix, requiredRootId);
    public bool TryAdvanceObserverSubtree(bool advanceRecoveryReplacement, string? incompleteAttackError = default, bool routeUnreturnedAttackThroughRuntime = false) => this._targetFactory().TryAdvanceObserverSubtree(advanceRecoveryReplacement, incompleteAttackError, routeUnreturnedAttackThroughRuntime);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOutsidePhaseDrawDiscardObserversRuntime))]
internal sealed class CoreSameNameHandDiscardOrDamageObserversCoreOutsidePhaseDrawDiscardObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOutsidePhaseDrawDiscardObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSameNameHandDiscardOrDamageObserversCoreOutsidePhaseDrawDiscardObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOutsidePhaseDrawDiscardObserversRuntime> _targetFactory = targetFactory;
    public bool OutsidePhaseDrawDiscardStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => this._targetFactory().OutsidePhaseDrawDiscardStructuralEdge(parent, child);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime))]
internal sealed class CoreSameNameHandDiscardOrDamageObserversCoreNationalSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSameNameHandDiscardOrDamageObserversCoreNationalSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> _targetFactory = targetFactory;
    public bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId) => this._targetFactory().HasRuntimeSkillInstance(player, skillId, skillInstanceId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineJoinedTrickDamageObserversRuntime))]
internal sealed class CoreSameNameHandDiscardOrDamageObserversCoreJoinedTrickDamageObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJoinedTrickDamageObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSameNameHandDiscardOrDamageObserversCoreJoinedTrickDamageObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJoinedTrickDamageObserversRuntime> _targetFactory = targetFactory;
    public bool JoinedTrickDamageRewardStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => this._targetFactory().JoinedTrickDamageRewardStructuralEdge(parent, child);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime))]
internal sealed class CoreSameNameHandDiscardOrDamageObserversCoreAlternatingBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSameNameHandDiscardOrDamageObserversCoreAlternatingBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory() => this._targetFactory().CompleteProgramEventHistory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDyingSuitsAndHistoricalUseObserversRuntime))]
internal sealed class CoreSameNameHandDiscardOrDamageObserversCoreDyingSuitsAndHistoricalUseObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingSuitsAndHistoricalUseObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSameNameHandDiscardOrDamageObserversCoreDyingSuitsAndHistoricalUseObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingSuitsAndHistoricalUseObserversRuntime> _targetFactory = targetFactory;
    public bool DyingSuitsStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => this._targetFactory().DyingSuitsStructuralEdge(parent, child);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime))]
internal sealed class CoreSameNameHandDiscardOrDamageObserversCoreCardActionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSameNameHandDiscardOrDamageObserversCoreCardActionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillWindowContext CreateCardActionProgramContext(global::CardGame.Core.ProgramCardTriggerWindowFrame frame, global::CardGame.Core.ProgramCardTriggerCandidate candidate) => this._targetFactory().CreateCardActionProgramContext(frame, candidate);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCompletedUndamagedTargetRevealObserversRuntime))]
internal sealed class CoreSameNameHandDiscardOrDamageObserversCoreCompletedUndamagedTargetRevealObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCompletedUndamagedTargetRevealObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSameNameHandDiscardOrDamageObserversCoreCompletedUndamagedTargetRevealObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCompletedUndamagedTargetRevealObserversRuntime> _targetFactory = targetFactory;
    public bool CompletedUndamagedTargetRevealStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => this._targetFactory().CompletedUndamagedTargetRevealStructuralEdge(parent, child);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreSameNameHandDiscardOrDamageObserversCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSameNameHandDiscardOrDamageObserversCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRoundGainedEquipmentObserversRuntime))]
internal sealed class CoreSameNameHandDiscardOrDamageObserversCoreRoundGainedEquipmentObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundGainedEquipmentObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSameNameHandDiscardOrDamageObserversCoreRoundGainedEquipmentObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundGainedEquipmentObserversRuntime> _targetFactory = targetFactory;
    public bool RoundGainedEquipmentDrawStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => this._targetFactory().RoundGainedEquipmentDrawStructuralEdge(parent, child);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtDamageObserversRuntime))]
internal sealed class CoreSameNameHandDiscardOrDamageObserversCoreHalfHandPhaseDebtDamageObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtDamageObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSameNameHandDiscardOrDamageObserversCoreHalfHandPhaseDebtDamageObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtDamageObserversRuntime> _targetFactory = targetFactory;
    public bool HalfHandPaidDamageObserverEdge(int index) => this._targetFactory().HalfHandPaidDamageObserverEdge(index);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOffTurnUsedCardObserversRuntime))]
internal sealed class CoreSameNameHandDiscardOrDamageObserversCoreOffTurnUsedCardObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOffTurnUsedCardObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSameNameHandDiscardOrDamageObserversCoreOffTurnUsedCardObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOffTurnUsedCardObserversRuntime> _targetFactory = targetFactory;
    public bool OffTurnUsedCardGiftStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => this._targetFactory().OffTurnUsedCardGiftStructuralEdge(parent, child);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTypedRecoveryDamageCursorRuntime))]
internal sealed class CoreSameNameHandDiscardOrDamageObserversCoreTypedRecoveryDamageCursorAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTypedRecoveryDamageCursorRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSameNameHandDiscardOrDamageObserversCoreTypedRecoveryDamageCursorOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTypedRecoveryDamageCursorRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.TypedResponseCompletionHealthCursorProjection ProjectTypedResponseCompletionHealthCursor() => this._targetFactory().ProjectTypedResponseCompletionHealthCursor();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRecipientCategoryMarkObserversRuntime))]
internal sealed class CoreSameNameHandDiscardOrDamageObserversCoreRecipientCategoryMarkObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientCategoryMarkObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSameNameHandDiscardOrDamageObserversCoreRecipientCategoryMarkObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientCategoryMarkObserversRuntime> _targetFactory = targetFactory;
    public bool RecipientCategoryMarkStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => this._targetFactory().RecipientCategoryMarkStructuralEdge(parent, child);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardSupplyCompletionRuntime))]
internal sealed class CoreSameNameHandDiscardOrDamageObserversCoreCardSupplyCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardSupplyCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSameNameHandDiscardOrDamageObserversCoreCardSupplyCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardSupplyCompletionRuntime> _targetFactory = targetFactory;
    public bool CardSupplyCompletionStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => this._targetFactory().CardSupplyCompletionStructuralEdge(parent, child);
    public global::CardGame.Core.ProgramCardTriggerWindowFrame? CardSupplyCompletionObserverRoot() => this._targetFactory().CardSupplyCompletionObserverRoot();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramAtomicCardMovesRuntime))]
internal sealed class CoreSameNameHandDiscardOrDamageObserversCoreProgramAtomicCardMovesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAtomicCardMovesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSameNameHandDiscardOrDamageObserversCoreProgramAtomicCardMovesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAtomicCardMovesRuntime> _targetFactory = targetFactory;
    public bool IsRedirectedPaidDiscardMovement(global::CardGame.Core.CardMovementRecord move, global::CardGame.Core.CardLocation expectedSource, global::CardGame.Core.CardMoveReason expectedReason) => this._targetFactory().IsRedirectedPaidDiscardMovement(move, expectedSource, expectedReason);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePairedColorResponseObserversRuntime))]
internal sealed class CoreSameNameHandDiscardOrDamageObserversCorePairedColorResponseObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePairedColorResponseObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSameNameHandDiscardOrDamageObserversCorePairedColorResponseObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePairedColorResponseObserversRuntime> _targetFactory = targetFactory;
    public bool PairedColorDispositionStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => this._targetFactory().PairedColorDispositionStructuralEdge(parent, child);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOriginalHandEntityObserversRuntime))]
internal sealed class CoreSameNameHandDiscardOrDamageObserversCoreOriginalHandEntityObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOriginalHandEntityObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSameNameHandDiscardOrDamageObserversCoreOriginalHandEntityObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOriginalHandEntityObserversRuntime> _targetFactory = targetFactory;
    public bool OriginalHandEntityStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => this._targetFactory().OriginalHandEntityStructuralEdge(parent, child);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime))]
internal sealed class CoreSameNameHandDiscardOrDamageObserversCoreDyingCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSameNameHandDiscardOrDamageObserversCoreDyingCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DyingFrame? ActiveDying { get => this._targetFactory().ActiveDying; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreSameNameHandDiscardOrDamageObserversCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSameNameHandDiscardOrDamageObserversCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}
