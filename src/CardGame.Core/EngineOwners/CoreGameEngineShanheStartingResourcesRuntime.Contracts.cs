#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreShanheStartingResourcesCoreDelayedNullificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long? NullificationParentActionId(global::CardGame.Core.NullificationWindowFrame window);
    bool HasNullificationEffectOwner(global::CardGame.Core.NullificationWindowFrame window);
}

internal interface ICoreShanheStartingResourcesCoreAdvancedLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetAdvancedCard(int id);
}

internal interface ICoreShanheStartingResourcesResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreShanheStartingResourcesCoreShanheBossCancelledMaterialsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryDescribeShanheBossCancelledMaterial(int cardId, global::CardGame.Core.CardMovementRecord entry, global::CardGame.Core.CardMoveReason reason, out global::CardGame.Core.ShanheBossMaterialCancelledEvent cancelled);
}

internal interface ICoreShanheStartingResourcesCoreResponseCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsResponseCompletionProgramDuelParent(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramCardContinuation continuation);
}

internal interface ICoreShanheStartingResourcesCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICoreShanheStartingResourcesCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
}

internal interface ICoreShanheStartingResourcesCoreShanheMarketApplicationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsShanheMarketResource(int id);
}

internal interface ICoreShanheStartingResourcesKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}
