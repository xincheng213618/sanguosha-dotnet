#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramRuntimeServicesRuntime
{
    void global::CardGame.Core.IProgramRuntimeEvents.AdvanceFact(IGameEvent fact) => _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(fact);
    PendingDecision? global::CardGame.Core.IProgramRuntimePrompts.Current => _corePendingDecisionPolicyPipelineOperations._pendingDecision;

    PromptId global::CardGame.Core.IProgramRuntimePrompts.NextId() => _coreNativePromptBoundaryOperations.CreatePromptId();
    void global::CardGame.Core.IProgramRuntimePrompts.Clear() => _coreNativePromptBoundaryOperations.ClearPendingDecision();
    void global::CardGame.Core.IProgramRuntimePrompts.Set(PendingDecision decision, bool isHuman)
    {
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = decision;
        _coreMatchOutcomeStatusAccess._status = isHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }
}
