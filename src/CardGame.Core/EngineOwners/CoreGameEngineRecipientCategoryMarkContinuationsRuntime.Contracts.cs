#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreRecipientCategoryMarkContinuationsCoreRecipientCategoryMarkObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RecipientCategoryMarkStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
    bool ValidRecipientCategoryMarkReceipt(global::CardGame.Core.ProgramSkillFrame f);
    bool RecipientCategoryMarkFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreRecipientCategoryMarkContinuationsCoreRoundGainedEquipmentObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RoundGainedEquipmentDrawStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreRecipientCategoryMarkContinuationsCorePairedColorResponseObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool PairedColorDispositionStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreRecipientCategoryMarkContinuationsCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreRecipientCategoryMarkContinuationsCoreOriginalHandEntityObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool OriginalHandEntityStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreRecipientCategoryMarkContinuationsCoreRecipientCategoryMarkContinuationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsRecipientCategoryObserverEdge(int child);
    bool RecipientCategoryObserverSuffix(int rootIndex);
}

internal interface ICoreRecipientCategoryMarkContinuationsCoreHalfHandPhaseDebtDamageObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HalfHandPaidDamageObserverEdge(int index);
}

internal interface ICoreRecipientCategoryMarkContinuationsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreRecipientCategoryMarkContinuationsCoreProgramChildResolutionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ProgramChildResolutionEdge(int index);
}

internal interface ICoreRecipientCategoryMarkContinuationsCoreJoinedTrickDamageObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool JoinedTrickDamageRewardStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreRecipientCategoryMarkContinuationsCoreObserverSubtreesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasObserverSuffix(int rootIndex, global::System.Func<int, bool> edge, global::System.Func<int, global::CardGame.Core.DyingFrame, bool>? completedDyingSuffix);
    global::CardGame.Core.ProgramSkillFrame? FindProgramObserverRoot(global::System.Func<global::CardGame.Core.ProgramSkillFrame, global::CardGame.Core.ResolutionFrame, bool> firstChild, global::System.Func<int, bool> suffix, long? requiredRootId = default);
    bool TryAdvanceObserverSubtree(bool advanceRecoveryReplacement, string? incompleteAttackError = default, bool routeUnreturnedAttackThroughRuntime = false);
}

internal interface ICoreRecipientCategoryMarkContinuationsCoreWenhePoisonOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWenhePoisonDyingChild(global::CardGame.Core.ResolutionFrame? parent, global::CardGame.Core.DyingFrame child);
}

internal interface ICoreRecipientCategoryMarkContinuationsCoreDyingSuitsAndHistoricalUseObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool DyingSuitsStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreRecipientCategoryMarkContinuationsCoreRecipientCategoryMarkObserversOperations2 : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RecipientCategoryMarkFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreRecipientCategoryMarkContinuationsCoreResponseCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ResponseCompletionStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreRecipientCategoryMarkContinuationsCoreSameNameHandDiscardOrDamageObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool SameNameHandStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreRecipientCategoryMarkContinuationsCoreOrderedPrintedSkillLossObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool OrderedPrintedSkillLossStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
    bool OrderedPrintedSkillLossSkillsChangedEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreRecipientCategoryMarkContinuationsCoreCompletedUndamagedTargetRevealObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CompletedUndamagedTargetRevealStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreRecipientCategoryMarkContinuationsCoreOverflowTargetCancellationObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool OverflowTargetCancellationStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreRecipientCategoryMarkContinuationsCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreRecipientCategoryMarkContinuationsCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreRecipientCategoryMarkContinuationsCoreCardSupplyCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CardSupplyCompletionStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreRecipientCategoryMarkContinuationsCoreOutsidePhaseDrawDiscardObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool OutsidePhaseDrawDiscardStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}
