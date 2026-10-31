#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreNativeUsedDiscardOriginsCoreResponseCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsResponseCompletionEntry(global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation, long parentFrameId, global::CardGame.Core.CardActionCost cost, global::CardGame.Core.CardMovementRecord moved);
}

internal interface ICoreNativeUsedDiscardOriginsCoreCardSupplyCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardSupplyCompletionEntry(global::CardGame.Core.CardActionContext action, long useId, long requestOwnerId, global::CardGame.Core.CardActionCost cost, global::CardGame.Core.CardMovementRecord moved);
}

internal interface ICoreNativeUsedDiscardOriginsCoreDelayedNullificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasNullificationEffectOwner(global::CardGame.Core.NullificationWindowFrame window);
    long? NullificationParentActionId(global::CardGame.Core.NullificationWindowFrame window);
}

internal interface ICoreNativeUsedDiscardOriginsCoreRequestedDeckBasicOwnershipOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasValidRequestedDeckBasicMaterial(global::CardGame.Core.ResolutionFrame owner);
}

internal interface ICoreNativeUsedDiscardOriginsCoreResponseUseCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCompletedResponseUse(global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation);
}

internal interface ICoreNativeUsedDiscardOriginsCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.FactionDefenseHandle? ActiveFactionDefense { get; set; }
}

internal interface ICoreNativeUsedDiscardOriginsCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICoreNativeUsedDiscardOriginsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreNativeUsedDiscardOriginsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreNativeUsedDiscardOriginsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}
