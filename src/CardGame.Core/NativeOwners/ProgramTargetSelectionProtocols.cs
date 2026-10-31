#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramTargetSelectionProtocols(global::System.Func<global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries> getCorePendingDecisionPolicyPipelineRuntime, global::CardGame.Core.EngineStates.CoreMatchOutcomeState matchOutcomeState, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineOwnedExactCountAndNamedTargetsEntries> getCoreGameEngineOwnedExactCountAndNamedTargetsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhugeJinMobileEntries> getCharacterZhugeJinMobileRuntime) : IProgramTargetSelectionProtocols
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
    public int[] OrderSupportTargets(ProgramSkillFrame frame, int[] targets) => getCharacterZhugeJinMobileRuntime().OrderMobileZhugeJinHongyuanTargets(frame, targets);
}
