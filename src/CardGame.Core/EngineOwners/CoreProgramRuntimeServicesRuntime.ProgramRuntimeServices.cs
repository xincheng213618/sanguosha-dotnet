#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramRuntimeServicesRuntime
{
    void IProgramRuntimeEvents.AdvanceFact(IGameEvent fact) => _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(fact);
    PendingDecision? IProgramRuntimePrompts.Current => _corePendingDecisionPolicyPipelineOperations._pendingDecision;

    PromptId IProgramRuntimePrompts.NextId() => _coreNativePromptBoundaryOperations.CreatePromptId();
    void IProgramRuntimePrompts.Clear() => _coreNativePromptBoundaryOperations.ClearPendingDecision();
    void IProgramRuntimePrompts.Set(PendingDecision decision, bool isHuman)
    {
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = decision;
        _coreMatchOutcomeStatusAccess._status = isHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }
}
