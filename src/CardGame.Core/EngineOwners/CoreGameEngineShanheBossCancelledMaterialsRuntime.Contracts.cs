#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreShanheBossCancelledMaterialsCoreShanheStartingResourcesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsShanheStartingResource(int id);
    bool ShanheStartingResourceWasAcquired(int id, int before = 2147483647);
}

internal interface ICoreShanheBossCancelledMaterialsCoreDelayedNullificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long? NullificationParentActionId(global::CardGame.Core.NullificationWindowFrame window);
}

internal interface ICoreShanheBossCancelledMaterialsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreShanheBossCancelledMaterialsCoreResponseCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsResponseCompletionProgramDuelParent(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramCardContinuation continuation);
}

internal interface ICoreShanheBossCancelledMaterialsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreShanheBossCancelledMaterialsCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}
