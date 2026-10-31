#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreFirstSlashDuelBenefitObserversCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreFirstSlashDuelBenefitObserversCoreSameNameHandDiscardOrDamageObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool SameNameHandObserverSuffix(int index);
}

internal interface ICoreFirstSlashDuelBenefitObserversCorePrivateOfferReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramEffect? GetPausedPrivateOfferEffect(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreFirstSlashDuelBenefitObserversCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreFirstSlashDuelBenefitObserversCoreFirstSlashDuelBenefitObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool FirstSlashDuelFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreFirstSlashDuelBenefitObserversCoreFirstSlashDuelLegacyWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsFirstLegacySlashUse(global::CardGame.Core.CardUseFrame use, long producer, bool requireSingle = true);
}

internal interface ICoreFirstSlashDuelBenefitObserversCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
}

internal interface ICoreFirstSlashDuelBenefitObserversResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreFirstSlashDuelBenefitObserversCoreObserverSubtreesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame? FindProgramObserverRoot(global::System.Func<global::CardGame.Core.ProgramSkillFrame, global::CardGame.Core.ResolutionFrame, bool> firstChild, global::System.Func<int, bool> suffix, long? requiredRootId = default);
    bool TryAdvanceObserverSubtree(bool advanceRecoveryReplacement, string? incompleteAttackError = default, bool routeUnreturnedAttackThroughRuntime = false);
}

internal interface ICoreFirstSlashDuelBenefitObserversCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreFirstSlashDuelBenefitObserversCoreFirstSlashDuelBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardConversionSource FirstSlashDuelSource(global::CardGame.Core.ProgramSkillFrame f);
    bool CanContinueFirstSlashDuelBenefit(global::CardGame.Core.ProgramSkillFrame f);
}
