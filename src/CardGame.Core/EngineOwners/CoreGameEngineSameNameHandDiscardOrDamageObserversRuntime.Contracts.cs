#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreSameNameHandDiscardOrDamageObserversCoreProgramChildResolutionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ProgramChildResolutionEdge(int index);
}

internal interface ICoreSameNameHandDiscardOrDamageObserversCoreSameNameHandDiscardOrDamageOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool SameNameHandActionParent(int owner, global::CardGame.Core.ProgramSkillWindowContext context, out global::CardGame.Core.ProgramCardTriggerWindowFrame window);
    global::CardGame.Core.SameNameHandMaterial[] SameNameHandMaterials(int seat, global::CardGame.Core.CardKind name);
    string SameNameHandDiscardReason(global::CardGame.Core.ProgramSkillFrame f);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> SameNameHandChoices(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreSameNameHandDiscardOrDamageObserversCoreResponseCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ResponseCompletionStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
    bool ValidResponseCompletionWindow(global::CardGame.Core.ProgramCardTriggerWindowFrame window);
    bool ResponseCompletionFirstChild(global::CardGame.Core.ProgramCardTriggerWindowFrame window, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreSameNameHandDiscardOrDamageObserversCoreObserverSubtreesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasObserverSuffix(int rootIndex, global::System.Func<int, bool> edge, global::System.Func<int, global::CardGame.Core.DyingFrame, bool>? completedDyingSuffix);
    global::CardGame.Core.ProgramSkillFrame? FindProgramObserverRoot(global::System.Func<global::CardGame.Core.ProgramSkillFrame, global::CardGame.Core.ResolutionFrame, bool> firstChild, global::System.Func<int, bool> suffix, long? requiredRootId = default);
    bool TryAdvanceObserverSubtree(bool advanceRecoveryReplacement, string? incompleteAttackError = default, bool routeUnreturnedAttackThroughRuntime = false);
}

internal interface ICoreSameNameHandDiscardOrDamageObserversCoreOutsidePhaseDrawDiscardObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool OutsidePhaseDrawDiscardStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreSameNameHandDiscardOrDamageObserversCoreObserverSubtreesOperations2 : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreSameNameHandDiscardOrDamageObserversCoreSameNameHandDiscardOrDamageObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSameNameHandObserverEdge(int child);
    bool SameNameHandFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child);
    bool SameNameHandObserverSuffix(int index);
}

internal interface ICoreSameNameHandDiscardOrDamageObserversCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId);
}

internal interface ICoreSameNameHandDiscardOrDamageObserversCoreJoinedTrickDamageObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool JoinedTrickDamageRewardStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreSameNameHandDiscardOrDamageObserversCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreSameNameHandDiscardOrDamageObserversCoreDyingSuitsAndHistoricalUseObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool DyingSuitsStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreSameNameHandDiscardOrDamageObserversCharacterHuanShuJunOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HuanpeiDyingRescueRide(int index, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreSameNameHandDiscardOrDamageObserversContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreSameNameHandDiscardOrDamageObserversCoreCardActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillWindowContext CreateCardActionProgramContext(global::CardGame.Core.ProgramCardTriggerWindowFrame frame, global::CardGame.Core.ProgramCardTriggerCandidate candidate);
}

internal interface ICoreSameNameHandDiscardOrDamageObserversCoreCompletedUndamagedTargetRevealObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CompletedUndamagedTargetRevealStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreSameNameHandDiscardOrDamageObserversCoreTieredRoundZeroReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TieredRoundZeroDyingRescueRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreSameNameHandDiscardOrDamageObserversCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreSameNameHandDiscardOrDamageObserversCoreRoundGainedEquipmentObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RoundGainedEquipmentDrawStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreSameNameHandDiscardOrDamageObserversCoreHalfHandPhaseDebtDamageObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HalfHandPaidDamageObserverEdge(int index);
}

internal interface ICoreSameNameHandDiscardOrDamageObserversCoreOffTurnUsedCardObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool OffTurnUsedCardGiftStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreSameNameHandDiscardOrDamageObserversCoreTypedRecoveryDamageCursorOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.TypedResponseCompletionHealthCursorProjection ProjectTypedResponseCompletionHealthCursor();
}

internal interface ICoreSameNameHandDiscardOrDamageObserversResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreSameNameHandDiscardOrDamageObserversCoreRecipientCategoryMarkObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RecipientCategoryMarkStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreSameNameHandDiscardOrDamageObserversCoreCardSupplyCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CardSupplyCompletionStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
    global::CardGame.Core.ProgramCardTriggerWindowFrame? CardSupplyCompletionObserverRoot();
}

internal interface ICoreSameNameHandDiscardOrDamageObserversCoreProgramAtomicCardMovesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsRedirectedPaidDiscardMovement(global::CardGame.Core.CardMovementRecord move, global::CardGame.Core.CardLocation expectedSource, global::CardGame.Core.CardMoveReason expectedReason);
}

internal interface ICoreSameNameHandDiscardOrDamageObserversCoreDrawFundedDistinctBasicReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool DrawFundedDistinctBasicDyingRescueRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreSameNameHandDiscardOrDamageObserversCorePairedColorResponseObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool PairedColorDispositionStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreSameNameHandDiscardOrDamageObserversCoreOriginalHandEntityObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool OriginalHandEntityStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreSameNameHandDiscardOrDamageObserversCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreSameNameHandDiscardOrDamageObserversCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}
