#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreActualHandGainObserversCoreActualHandGainObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ActualHandGainFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreActualHandGainObserversCoreActualHandGainProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramEffect? ActualHandGainPausedEffect(global::CardGame.Core.ProgramSkillFrame f);
    bool HasRecordedActualOwnHandGain(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.CardMovementBatchContext batch);
    bool IsActualHandGainOwnDrawBatch(global::CardGame.Core.CardMovementBatchContext batch, int owner, string skill, string hash);
    global::CardGame.Core.ActualHandGainEntity[] ActualHandGainEntities(global::CardGame.Core.CardMovementBatchContext batch, int owner);
    global::CardGame.Core.ActualHandGainEntity[] ForeignHandGainRoster(int owner, string skill, string hash, int turn, int actualOwner);
    global::CardGame.Core.CardLocation ActualHandGainOriginalSource(global::CardGame.Core.CardMovementRecord first, global::CardGame.Core.CardMovementBatchContext? batch = default);
    bool ExactActualHandGainUse(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context, out global::CardGame.Core.ProgramCardTriggerWindowFrame parent, out global::CardGame.Core.CardUseFrame use);
    bool SameCategoryGiftUsed(int owner, string skill, int turn, global::CardGame.Core.SkillProgramCardCategory category);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ActualHandGainChoices(global::CardGame.Core.ProgramSkillFrame f);
    bool IsActualHandGainMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p);
}

internal interface ICoreActualHandGainObserversResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreActualHandGainObserversCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreActualHandGainObserversCoreObserverSubtreesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasHandGainDebtObserverSuffix(int rootIndex);
}

internal interface ICoreActualHandGainObserversCoreObserverSubtreesOperations2 : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame? FindProgramObserverRoot(global::System.Func<global::CardGame.Core.ProgramSkillFrame, global::CardGame.Core.ResolutionFrame, bool> firstChild, global::System.Func<int, bool> suffix, long? requiredRootId = default);
    bool TryAdvanceObserverSubtree(bool advanceRecoveryReplacement, string? incompleteAttackError = default, bool routeUnreturnedAttackThroughRuntime = false);
}

internal interface ICoreActualHandGainObserversContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreActualHandGainObserversCoreAfterTurnEndedProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsActualAfterTurnEndedParent(global::CardGame.Core.DeferredTurnEndFrame parent);
    global::CardGame.Core.ProgramTriggerCandidate AfterTurnEndedCandidate(global::CardGame.Core.DeferredTurnEndFrame parent);
}

internal interface ICoreActualHandGainObserversCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreActualHandGainObserversCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreActualHandGainObserversCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}
