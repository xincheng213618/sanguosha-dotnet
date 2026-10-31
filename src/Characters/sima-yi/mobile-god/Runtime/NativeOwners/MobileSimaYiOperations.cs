#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileSimaYiOperations(Func<RuntimeEntries.CoreGameEngineProgramSkillChangesEntries> getCoreGameEngineProgramSkillChangesRuntime, Func<RuntimeEntries.CoreGameEngineProgramMarkerPaymentsEntries> getCoreGameEngineProgramMarkerPaymentsRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime, Func<RuntimeEntries.CoreGameEngineProgramContinuationsEntries> getCoreGameEngineProgramContinuationsRuntime, KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState) : IMobileSimaYiOperations
{
    public void GrantSkills(ProgramSkillFrame frame, IReadOnlyList<string> skillIds) => getCoreGameEngineProgramSkillChangesRuntime().GrantProgramSkills(frame, skillIds);
    public void PayMarker(int owner, SkillProgramMarkerCost cost, string skillId, string bindingId) => getCoreGameEngineProgramMarkerPaymentsRuntime().PayProgramMarkerCost(matchParticipantsState.ReferenceToPlayers[owner], cost, skillId, bindingId);
    public void ExtraTurn(ProgramSkillFrame frame, int target) => getCoreGameEngineSkillProgramsRuntime().PendProgramExtraTurn(frame, target);
    public bool DrainChildWindows(long frameId) => getCoreGameEngineProgramContinuationsRuntime().TryBeginProgramChildWindows(frameId, PostEventContinuation.Program);
    public void PublishState() => advanceRulesAndPublishState();
}
