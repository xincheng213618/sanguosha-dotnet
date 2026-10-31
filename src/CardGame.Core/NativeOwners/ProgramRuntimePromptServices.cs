#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramRuntimePromptServices(Func<RuntimeEntries.CorePendingDecisionPolicyPipelineEntries> getCorePendingDecisionPolicyPipelineRuntime, Func<RuntimeEntries.CoreNativePromptBoundaryEntries> getCoreNativePromptBoundaryRuntime, EngineStates.CoreMatchOutcomeState matchOutcomeState) : IProgramRuntimePrompts
{
    public PendingDecision? Current => getCorePendingDecisionPolicyPipelineRuntime()._pendingDecision;

    public PromptId NextId() => getCoreNativePromptBoundaryRuntime().CreatePromptId();
    public void Clear() => getCoreNativePromptBoundaryRuntime().ClearPendingDecision();
    public void Set(PendingDecision decision, bool isHuman)
    {
        getCorePendingDecisionPolicyPipelineRuntime()._pendingDecision = decision;
        matchOutcomeState.ReferenceToStatus = isHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }
}
