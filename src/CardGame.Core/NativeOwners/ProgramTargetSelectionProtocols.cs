#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramTargetSelectionProtocols(Func<RuntimeEntries.CorePendingDecisionPolicyPipelineEntries> getCorePendingDecisionPolicyPipelineRuntime, EngineStates.CoreMatchOutcomeState matchOutcomeState, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, Func<RuntimeEntries.CoreGameEngineOwnedExactCountAndNamedTargetsEntries> getCoreGameEngineOwnedExactCountAndNamedTargetsRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterZhugeJinMobileRuntime>> getCharacterZhugeJinMobileRuntime) : IProgramTargetSelectionProtocols
{
    public void Present(PendingDecision decision, int ownerSeat)
    {
        // Keep publication before the owner-control read, as in the original prompt boundary.
        getCorePendingDecisionPolicyPipelineRuntime()._pendingDecision = decision;
        matchOutcomeState.ReferenceToStatus = matchParticipantsState.ReferenceToPlayers[ownerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    public void Cancel(ProgramSkillFrame frame, string reason) => getCoreGameEngineProgramLifecycleRuntime().CancelProgramBindingAndCleanup(frame, reason);
    public SkillProgramStepOutcome SelectNamedBoundTargets(ProgramSkillFrame frame, SkillProgramEffect effect) => getCoreGameEngineOwnedExactCountAndNamedTargetsRuntime().SelectProgramNamedBoundTargets(frame, effect);
    public void ResolveNamedBoundTargets(ProgramSkillFrame frame, SkillProgramEffect effect, PromptChoice selected) => getCoreGameEngineOwnedExactCountAndNamedTargetsRuntime().ResolveProgramNamedBoundTargets(frame, effect, selected);
    public int[] OrderSupportTargets(ProgramSkillFrame frame, int[] targets) => getCharacterZhugeJinMobileRuntime().Value.OrderMobileZhugeJinHongyuanTargets(frame, targets);
}
