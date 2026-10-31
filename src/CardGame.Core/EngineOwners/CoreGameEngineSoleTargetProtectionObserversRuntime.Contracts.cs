#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreSoleTargetProtectionObserversCoreAlternativeViewAsCostsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsProgramAlternativeCost(global::CardGame.Core.CardActionContext? action, int cardId);
}

internal interface ICoreSoleTargetProtectionObserversCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreSoleTargetProtectionObserversCoreSoleTargetProtectionObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool SoleTargetProtectionFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreSoleTargetProtectionObserversCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreSoleTargetProtectionObserversCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
}

internal interface ICoreSoleTargetProtectionObserversCoreCurrentUsePhysicalClaimsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCurrentUsePhysicalCardClaim(long frameId, int cardId);
}

internal interface ICoreSoleTargetProtectionObserversCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreSoleTargetProtectionObserversCoreSameNameHandDiscardOrDamageObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool SameNameHandObserverSuffix(int index);
}

internal interface ICoreSoleTargetProtectionObserversCoreResponseEntityExchangeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsExchangedCardClaim(long actionId, int cardId);
}

internal interface ICoreSoleTargetProtectionObserversCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreSoleTargetProtectionObserversCoreSoleTargetQuarantineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasExactSoleTargetQuarantine(global::CardGame.Core.CardUseFrame use);
    bool IsSoleTargetQuarantinedMaterial(long id, int cardId);
}

internal interface ICoreSoleTargetProtectionObserversCoreSoleTargetProtectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasExactSoleProtectionRoot(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreSoleTargetProtectionObserversResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreSoleTargetProtectionObserversCoreObserverSubtreesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame? FindProgramObserverRoot(global::System.Func<global::CardGame.Core.ProgramSkillFrame, global::CardGame.Core.ResolutionFrame, bool> firstChild, global::System.Func<int, bool> suffix, long? requiredRootId = default);
    bool TryAdvanceObserverSubtree(bool advanceRecoveryReplacement, string? incompleteAttackError = default, bool routeUnreturnedAttackThroughRuntime = false);
}
