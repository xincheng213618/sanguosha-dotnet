#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCompletedUndamagedTargetRevealObserversRuntime))]
internal sealed class CoreOriginalHandEntityObserversCoreCompletedUndamagedTargetRevealObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCompletedUndamagedTargetRevealObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalHandEntityObserversCoreCompletedUndamagedTargetRevealObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCompletedUndamagedTargetRevealObserversRuntime> _targetFactory = targetFactory;
    public bool CompletedUndamagedTargetRevealStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => this._targetFactory().CompletedUndamagedTargetRevealStructuralEdge(parent, child);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramChildResolutionRuntime))]
internal sealed class CoreOriginalHandEntityObserversCoreProgramChildResolutionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramChildResolutionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalHandEntityObserversCoreProgramChildResolutionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramChildResolutionRuntime> _targetFactory = targetFactory;
    public bool ProgramChildResolutionEdge(int index) => this._targetFactory().ProgramChildResolutionEdge(index);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOriginalHandEntitiesRuntime))]
internal sealed class CoreOriginalHandEntityObserversCoreOriginalHandEntitiesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOriginalHandEntitiesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalHandEntityObserversCoreOriginalHandEntitiesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOriginalHandEntitiesRuntime> _targetFactory = targetFactory;
    public bool TracksOriginalHandEntities { get => this._targetFactory().TracksOriginalHandEntities; }

    public global::CardGame.Core.OriginalHandLossEligibleEvent? OriginalHandLossEligibility(int owner, string skill, string state, string binding, long sequence) => this._targetFactory().OriginalHandLossEligibility(owner, skill, state, binding, sequence);
    public long OriginalHandMovementSequence { get => this._targetFactory().OriginalHandMovementSequence; }

    public global::CardGame.Core.OriginalHandPermanentBonus[] OriginalHandInheritableBonuses(int owner, string skill, string stateId) => this._targetFactory().OriginalHandInheritableBonuses(owner, skill, stateId);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> OriginalHandEntityChoices(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().OriginalHandEntityChoices(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSameNameHandDiscardOrDamageObserversRuntime))]
internal sealed class CoreOriginalHandEntityObserversCoreSameNameHandDiscardOrDamageObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSameNameHandDiscardOrDamageObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalHandEntityObserversCoreSameNameHandDiscardOrDamageObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSameNameHandDiscardOrDamageObserversRuntime> _targetFactory = targetFactory;
    public bool SameNameHandStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => this._targetFactory().SameNameHandStructuralEdge(parent, child);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreOriginalHandEntityObserversCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalHandEntityObserversCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().GetProgramTrigger(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardSupplyCompletionRuntime))]
internal sealed class CoreOriginalHandEntityObserversCoreCardSupplyCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardSupplyCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalHandEntityObserversCoreCardSupplyCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardSupplyCompletionRuntime> _targetFactory = targetFactory;
    public bool CardSupplyCompletionStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => this._targetFactory().CardSupplyCompletionStructuralEdge(parent, child);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOutsidePhaseDrawDiscardObserversRuntime))]
internal sealed class CoreOriginalHandEntityObserversCoreOutsidePhaseDrawDiscardObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOutsidePhaseDrawDiscardObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalHandEntityObserversCoreOutsidePhaseDrawDiscardObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOutsidePhaseDrawDiscardObserversRuntime> _targetFactory = targetFactory;
    public bool OutsidePhaseDrawDiscardStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => this._targetFactory().OutsidePhaseDrawDiscardStructuralEdge(parent, child);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreOriginalHandEntityObserversCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalHandEntityObserversCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRecipientCategoryMarkObserversRuntime))]
internal sealed class CoreOriginalHandEntityObserversCoreRecipientCategoryMarkObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientCategoryMarkObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalHandEntityObserversCoreRecipientCategoryMarkObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientCategoryMarkObserversRuntime> _targetFactory = targetFactory;
    public bool RecipientCategoryMarkStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => this._targetFactory().RecipientCategoryMarkStructuralEdge(parent, child);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime))]
internal sealed class CoreOriginalHandEntityObserversCoreDyingCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalHandEntityObserversCoreDyingCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DyingFrame? ActiveDying { get => this._targetFactory().ActiveDying; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOffTurnUsedCardObserversRuntime))]
internal sealed class CoreOriginalHandEntityObserversCoreOffTurnUsedCardObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOffTurnUsedCardObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalHandEntityObserversCoreOffTurnUsedCardObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOffTurnUsedCardObserversRuntime> _targetFactory = targetFactory;
    public bool OffTurnUsedCardGiftStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => this._targetFactory().OffTurnUsedCardGiftStructuralEdge(parent, child);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreOriginalHandEntityObserversCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalHandEntityObserversCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime))]
internal sealed class CoreOriginalHandEntityObserversCoreProgramEventHistoryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalHandEntityObserversCoreProgramEventHistoryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent => this._targetFactory().ProgramEventHistory<T>();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtDamageObserversRuntime))]
internal sealed class CoreOriginalHandEntityObserversCoreHalfHandPhaseDebtDamageObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtDamageObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalHandEntityObserversCoreHalfHandPhaseDebtDamageObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtDamageObserversRuntime> _targetFactory = targetFactory;
    public bool HalfHandPaidDamageObserverEdge(int index) => this._targetFactory().HalfHandPaidDamageObserverEdge(index);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime))]
internal sealed class CoreOriginalHandEntityObserversCoreAlternatingBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalHandEntityObserversCoreAlternatingBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory() => this._targetFactory().CompleteProgramEventHistory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDyingSuitsAndHistoricalUseObserversRuntime))]
internal sealed class CoreOriginalHandEntityObserversCoreDyingSuitsAndHistoricalUseObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingSuitsAndHistoricalUseObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalHandEntityObserversCoreDyingSuitsAndHistoricalUseObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingSuitsAndHistoricalUseObserversRuntime> _targetFactory = targetFactory;
    public bool DyingSuitsStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => this._targetFactory().DyingSuitsStructuralEdge(parent, child);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineObserverSubtreesRuntime))]
internal sealed class CoreOriginalHandEntityObserversCoreObserverSubtreesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineObserverSubtreesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalHandEntityObserversCoreObserverSubtreesOperations2
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineObserverSubtreesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillFrame? FindProgramObserverRoot(global::System.Func<global::CardGame.Core.ProgramSkillFrame, global::CardGame.Core.ResolutionFrame, bool> firstChild, global::System.Func<int, bool> suffix, long? requiredRootId = default) => this._targetFactory().FindProgramObserverRoot(firstChild, suffix, requiredRootId);
    public bool TryAdvanceObserverSubtree(bool advanceRecoveryReplacement, string? incompleteAttackError = default, bool routeUnreturnedAttackThroughRuntime = false) => this._targetFactory().TryAdvanceObserverSubtree(advanceRecoveryReplacement, incompleteAttackError, routeUnreturnedAttackThroughRuntime);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOriginalHandAwakeningRuntime))]
internal sealed class CoreOriginalHandEntityObserversCoreOriginalHandAwakeningAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOriginalHandAwakeningRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalHandEntityObserversCoreOriginalHandAwakeningOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOriginalHandAwakeningRuntime> _targetFactory = targetFactory;
    public bool OriginalHandAwakeningFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => this._targetFactory().OriginalHandAwakeningFirstChild(f, child);
    public bool ValidOriginalHandAwakeningReceipt(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().ValidOriginalHandAwakeningReceipt(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionRuntime))]
internal sealed class CoreOriginalHandEntityObserversCoreResponseCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOriginalHandEntityObserversCoreResponseCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionRuntime> _targetFactory = targetFactory;
    public bool ResponseCompletionStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => this._targetFactory().ResponseCompletionStructuralEdge(parent, child);
}
