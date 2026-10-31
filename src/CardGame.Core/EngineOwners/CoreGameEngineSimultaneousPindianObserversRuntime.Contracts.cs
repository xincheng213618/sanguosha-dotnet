#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreSimultaneousPindianObserversCoreSimultaneousPindianObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool SimultaneousFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreSimultaneousPindianObserversCorePrivateOfferReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramEffect? GetPausedPrivateOfferEffect(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreSimultaneousPindianObserversCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreSimultaneousPindianObserversCharacterPangJiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PindianRevealedCard> GetDouyuOriginalSimultaneousCards(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreSimultaneousPindianObserversResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreSimultaneousPindianObserversCoreObserverSubtreesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame? FindProgramObserverRoot(global::System.Func<global::CardGame.Core.ProgramSkillFrame, global::CardGame.Core.ResolutionFrame, bool> firstChild, global::System.Func<int, bool> suffix, long? requiredRootId = default);
    bool TryAdvanceObserverSubtree(bool advanceRecoveryReplacement, string? incompleteAttackError = default, bool routeUnreturnedAttackThroughRuntime = false);
}

internal interface ICoreSimultaneousPindianObserversCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreSimultaneousPindianObserversCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreSimultaneousPindianObserversCoreSimultaneousPindianSelectionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardLocation SimultaneousOriginalLocation(global::CardGame.Core.SimultaneousPindianReceipt r, global::CardGame.Core.PindianRevealedCard card);
}

internal interface ICoreSimultaneousPindianObserversCoreObserverSubtreesOperations2 : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMovementObserverSuffix(int rootIndex);
}

internal interface ICoreSimultaneousPindianObserversCoreSimultaneousPindianOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertSimultaneousPindian(global::CardGame.Core.ProgramSkillFrame f);
    bool IsSimultaneousReceiptSource(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreSimultaneousPindianObserversCharacterLuKangWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardLocation WuLuKangAssignedLocation(global::CardGame.Core.ResolutionFrame parent, int seat);
    global::System.Collections.Generic.IReadOnlyList<int> WuLuKangHeldProcessing(global::CardGame.Core.ResolutionFrame parent);
}

internal interface ICoreSimultaneousPindianObserversCoreRecoveryReplacementDamageRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RecoveryReplacementFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath);
}

internal interface ICoreSimultaneousPindianObserversCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}
