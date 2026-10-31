#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileSimaYiOperations(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramSkillChangesEntries> getCoreGameEngineProgramSkillChangesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramMarkerPaymentsEntries> getCoreGameEngineProgramMarkerPaymentsRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramContinuationsEntries> getCoreGameEngineProgramContinuationsRuntime, global::CardGame.Core.NativeOwners.KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState) : IMobileSimaYiOperations
{
    public void GrantSkills(ProgramSkillFrame frame, IReadOnlyList<string> skillIds) => getCoreGameEngineProgramSkillChangesRuntime().GrantProgramSkills(frame, skillIds);
    public void PayMarker(int owner, SkillProgramMarkerCost cost, string skillId, string bindingId) => getCoreGameEngineProgramMarkerPaymentsRuntime().PayProgramMarkerCost(matchParticipantsState.ReferenceToPlayers[owner], cost, skillId, bindingId);
    public void ExtraTurn(ProgramSkillFrame frame, int target) => getCoreGameEngineSkillProgramsRuntime().PendProgramExtraTurn(frame, target);
    public bool DrainChildWindows(long frameId) => getCoreGameEngineProgramContinuationsRuntime().TryBeginProgramChildWindows(frameId, PostEventContinuation.Program);
    public void PublishState() => advanceRulesAndPublishState();
}
