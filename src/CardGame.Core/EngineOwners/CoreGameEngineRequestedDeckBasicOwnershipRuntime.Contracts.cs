#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreRequestedDeckBasicOwnershipResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreRequestedDeckBasicOwnershipCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.FactionCardRequestHandle? ActiveFactionCardRequest { get; set; }
}

internal interface ICoreRequestedDeckBasicOwnershipContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreRequestedDeckBasicOwnershipCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreRequestedDeckBasicOwnershipCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreRequestedDeckBasicOwnershipCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreRequestedDeckBasicOwnershipCoreRequestedDeckBasicsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.CardConversionSource Source, global::CardGame.Core.SkillProgramCardPolicy Policy)> RequestedDeckBasicSources(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind required);
    global::CardGame.Core.CardKind RequestedDeckBasicKind(global::CardGame.Core.PendingDecision p, global::CardGame.Core.RequestedDeckBasicIntent intent);
    (global::CardGame.Core.RequestedDeckBasicIntent Intent, long OwnerId, long RequestId, int Cursor)? RequestedDeckBasicContext(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ResolutionFrame? suppliedTop = default);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> RequestedDeckBasicChoices(global::CardGame.Core.RequestedDeckBasicFrame view);
}
