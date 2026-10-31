#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreOrderedPrintedSkillLossObserversCoreNativeCardClassificationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DamageNature GetDamageNature(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreOrderedPrintedSkillLossObserversCoreObserverSubtreesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame? FindProgramObserverRoot(global::System.Func<global::CardGame.Core.ProgramSkillFrame, global::CardGame.Core.ResolutionFrame, bool> firstChild, global::System.Func<int, bool> suffix, long? requiredRootId = default);
    bool TryAdvanceObserverSubtree(bool advanceRecoveryReplacement, string? incompleteAttackError = default, bool routeUnreturnedAttackThroughRuntime = false);
}

internal interface ICoreOrderedPrintedSkillLossObserversCoreOrderedPrintedSkillLossObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool OrderedPrintedSkillLossFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child);
    bool OrderedPrintedSkillLossObserverEdge(int child);
}

internal interface ICoreOrderedPrintedSkillLossObserversResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreOrderedPrintedSkillLossObserversCoreOrderedPrintedSkillLossOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long OrderedPrintedLossMovementSequence { get; }
}

internal interface ICoreOrderedPrintedSkillLossObserversCoreAlcoholSlashSkillReplacementsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidAlcoholSlashSkillReplacementReceipt(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreOrderedPrintedSkillLossObserversCoreObserverSubtreesOperations2 : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasObserverSuffix(int rootIndex, global::System.Func<int, bool> edge, global::System.Func<int, global::CardGame.Core.DyingFrame, bool>? completedDyingSuffix);
    bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreOrderedPrintedSkillLossObserversCoreLastDamageSourceReciprocityObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameEngine.IDamageAttempt? LastDamageSourceOriginalAttack(long id);
}

internal interface ICoreOrderedPrintedSkillLossObserversCoreDyingSuitsAndHistoricalUseObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool DyingSuitsStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreOrderedPrintedSkillLossObserversCoreProgramChildResolutionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ProgramChildResolutionEdge(int index);
}

internal interface ICoreOrderedPrintedSkillLossObserversCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreOrderedPrintedSkillLossObserversCoreMatchingRecastObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MatchingRecastStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreOrderedPrintedSkillLossObserversCoreOverflowTargetCancellationObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool OverflowTargetCancellationStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreOrderedPrintedSkillLossObserversContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreOrderedPrintedSkillLossObserversCoreDamageTargetSkillRoundDrawOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RequireDamageTargetSkillRoundDraw(global::CardGame.Core.ProgramSkillFrame frame);
    bool ValidDamageTargetSkillRoundDraw(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreOrderedPrintedSkillLossObserversCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreOrderedPrintedSkillLossObserversCoreHalfHandPhaseDebtDamageObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HalfHandPaidDamageObserverEdge(int index);
}

internal interface ICoreOrderedPrintedSkillLossObserversCoreRandomDamageTargetSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidRandomDamageTargetSkillReceipt(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreOrderedPrintedSkillLossObserversCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreOrderedPrintedSkillLossObserversCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate);
}

internal interface ICoreOrderedPrintedSkillLossObserversCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}
