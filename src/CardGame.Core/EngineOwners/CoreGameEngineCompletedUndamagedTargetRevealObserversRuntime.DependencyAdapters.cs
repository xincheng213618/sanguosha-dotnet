#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreCompletedUndamagedTargetRevealObserversCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCompletedUndamagedTargetRevealObserversCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().GetProgramTrigger(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtDamageObserversRuntime))]
internal sealed class CoreCompletedUndamagedTargetRevealObserversCoreHalfHandPhaseDebtDamageObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtDamageObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCompletedUndamagedTargetRevealObserversCoreHalfHandPhaseDebtDamageObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtDamageObserversRuntime> _targetFactory = targetFactory;
    public bool HalfHandPaidDamageObserverEdge(int index) => this._targetFactory().HalfHandPaidDamageObserverEdge(index);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOffTurnUsedCardObserversRuntime))]
internal sealed class CoreCompletedUndamagedTargetRevealObserversCoreOffTurnUsedCardObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOffTurnUsedCardObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCompletedUndamagedTargetRevealObserversCoreOffTurnUsedCardObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOffTurnUsedCardObserversRuntime> _targetFactory = targetFactory;
    public bool OffTurnUsedCardGiftStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => this._targetFactory().OffTurnUsedCardGiftStructuralEdge(parent, child);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRecipientCategoryMarkObserversRuntime))]
internal sealed class CoreCompletedUndamagedTargetRevealObserversCoreRecipientCategoryMarkObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientCategoryMarkObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCompletedUndamagedTargetRevealObserversCoreRecipientCategoryMarkObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientCategoryMarkObserversRuntime> _targetFactory = targetFactory;
    public bool RecipientCategoryMarkStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => this._targetFactory().RecipientCategoryMarkStructuralEdge(parent, child);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreCompletedUndamagedTargetRevealObserversCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCompletedUndamagedTargetRevealObserversCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime))]
internal sealed class CoreCompletedUndamagedTargetRevealObserversCoreAlternatingBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCompletedUndamagedTargetRevealObserversCoreAlternatingBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory() => this._targetFactory().CompleteProgramEventHistory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSameNameHandDiscardOrDamageObserversRuntime))]
internal sealed class CoreCompletedUndamagedTargetRevealObserversCoreSameNameHandDiscardOrDamageObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSameNameHandDiscardOrDamageObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCompletedUndamagedTargetRevealObserversCoreSameNameHandDiscardOrDamageObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSameNameHandDiscardOrDamageObserversRuntime> _targetFactory = targetFactory;
    public bool SameNameHandStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => this._targetFactory().SameNameHandStructuralEdge(parent, child);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreCompletedUndamagedTargetRevealObserversCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCompletedUndamagedTargetRevealObserversCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOriginalHandEntityObserversRuntime))]
internal sealed class CoreCompletedUndamagedTargetRevealObserversCoreOriginalHandEntityObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOriginalHandEntityObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCompletedUndamagedTargetRevealObserversCoreOriginalHandEntityObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOriginalHandEntityObserversRuntime> _targetFactory = targetFactory;
    public bool OriginalHandEntityStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => this._targetFactory().OriginalHandEntityStructuralEdge(parent, child);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime))]
internal sealed class CoreCompletedUndamagedTargetRevealObserversCoreDyingCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCompletedUndamagedTargetRevealObserversCoreDyingCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DyingFrame? ActiveDying { get => this._targetFactory().ActiveDying; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCompletedUndamagedTargetRevealRuntime))]
internal sealed class CoreCompletedUndamagedTargetRevealObserversCoreCompletedUndamagedTargetRevealAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCompletedUndamagedTargetRevealRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCompletedUndamagedTargetRevealObserversCoreCompletedUndamagedTargetRevealOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCompletedUndamagedTargetRevealRuntime> _targetFactory = targetFactory;
    public bool HasExactAcceptedCompletedUndamagedUse(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardActionContext action) => this._targetFactory().HasExactAcceptedCompletedUndamagedUse(use, action);
    public global::CardGame.Core.CardUseFrame? CompletedUndamagedUse(global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().CompletedUndamagedUse(context);
    public bool CompletedUndamagedSourceUseQualifies(string skillId, global::CardGame.Core.CardUseFrame use) => this._targetFactory().CompletedUndamagedSourceUseQualifies(skillId, use);
    public long CompletedUndamagedMovementSequence { get => this._targetFactory().CompletedUndamagedMovementSequence; }

    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> CompletedUndamagedTargetRevealChoices(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().CompletedUndamagedTargetRevealChoices(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineObserverSubtreesRuntime))]
internal sealed class CoreCompletedUndamagedTargetRevealObserversCoreObserverSubtreesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineObserverSubtreesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCompletedUndamagedTargetRevealObserversCoreObserverSubtreesOperations2
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineObserverSubtreesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillFrame? FindProgramObserverRoot(global::System.Func<global::CardGame.Core.ProgramSkillFrame, global::CardGame.Core.ResolutionFrame, bool> firstChild, global::System.Func<int, bool> suffix, long? requiredRootId = default) => this._targetFactory().FindProgramObserverRoot(firstChild, suffix, requiredRootId);
    public bool TryAdvanceObserverSubtree(bool advanceRecoveryReplacement, string? incompleteAttackError = default, bool routeUnreturnedAttackThroughRuntime = false) => this._targetFactory().TryAdvanceObserverSubtree(advanceRecoveryReplacement, incompleteAttackError, routeUnreturnedAttackThroughRuntime);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDyingSuitsAndHistoricalUseObserversRuntime))]
internal sealed class CoreCompletedUndamagedTargetRevealObserversCoreDyingSuitsAndHistoricalUseObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingSuitsAndHistoricalUseObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCompletedUndamagedTargetRevealObserversCoreDyingSuitsAndHistoricalUseObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingSuitsAndHistoricalUseObserversRuntime> _targetFactory = targetFactory;
    public bool DyingSuitsStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => this._targetFactory().DyingSuitsStructuralEdge(parent, child);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXiaoQiaoMouOlRuntime))]
internal sealed class CoreCompletedUndamagedTargetRevealObserversCharacterXiaoQiaoMouOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiaoQiaoMouOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCompletedUndamagedTargetRevealObserversCharacterXiaoQiaoMouOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiaoQiaoMouOlRuntime> _targetFactory = targetFactory;
    public bool IsPublicHandRevealProducerSuspended(long parentId, string key) => this._targetFactory().IsPublicHandRevealProducerSuspended(parentId, key);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionRuntime))]
internal sealed class CoreCompletedUndamagedTargetRevealObserversCoreResponseCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCompletedUndamagedTargetRevealObserversCoreResponseCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionRuntime> _targetFactory = targetFactory;
    public bool ResponseCompletionStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => this._targetFactory().ResponseCompletionStructuralEdge(parent, child);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime))]
internal sealed class CoreCompletedUndamagedTargetRevealObserversCoreCardActionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCompletedUndamagedTargetRevealObserversCoreCardActionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillWindowContext CreateCardActionProgramContext(global::CardGame.Core.ProgramCardTriggerWindowFrame frame, global::CardGame.Core.ProgramCardTriggerCandidate candidate) => this._targetFactory().CreateCardActionProgramContext(frame, candidate);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramChildResolutionRuntime))]
internal sealed class CoreCompletedUndamagedTargetRevealObserversCoreProgramChildResolutionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramChildResolutionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCompletedUndamagedTargetRevealObserversCoreProgramChildResolutionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramChildResolutionRuntime> _targetFactory = targetFactory;
    public bool ProgramChildResolutionEdge(int index) => this._targetFactory().ProgramChildResolutionEdge(index);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOutsidePhaseDrawDiscardObserversRuntime))]
internal sealed class CoreCompletedUndamagedTargetRevealObserversCoreOutsidePhaseDrawDiscardObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOutsidePhaseDrawDiscardObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCompletedUndamagedTargetRevealObserversCoreOutsidePhaseDrawDiscardObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOutsidePhaseDrawDiscardObserversRuntime> _targetFactory = targetFactory;
    public bool OutsidePhaseDrawDiscardStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => this._targetFactory().OutsidePhaseDrawDiscardStructuralEdge(parent, child);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardSupplyCompletionRuntime))]
internal sealed class CoreCompletedUndamagedTargetRevealObserversCoreCardSupplyCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardSupplyCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCompletedUndamagedTargetRevealObserversCoreCardSupplyCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardSupplyCompletionRuntime> _targetFactory = targetFactory;
    public bool CardSupplyCompletionStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => this._targetFactory().CardSupplyCompletionStructuralEdge(parent, child);
}
