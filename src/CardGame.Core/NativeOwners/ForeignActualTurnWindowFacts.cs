#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ForeignActualTurnWindowFacts(EngineStates.CoreMatchOutcomeState matchOutcomeState, Func<RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CoreGameEngineTableOrderEntries> getCoreGameEngineTableOrderRuntime) : IForeignActualTurnWindowFacts
{
    public Winner Winner => matchOutcomeState.ReferenceToWinner;
    public EngineStatus Status => matchOutcomeState.ReferenceToStatus;

    public SkillProgramTriggerFacts CaptureTriggerFacts(int ownerSeat) => getCoreGameEngineProgramLifecycleRuntime().CaptureProgramTriggerFacts(matchParticipantsState.ReferenceToPlayers[ownerSeat]);
    public IReadOnlyList<ProgramTriggerCandidate> CollectCandidates(int ownerSeat, SkillProgramTriggerFacts facts) => getCoreGameEngineProgramLifecycleRuntime().CollectEligibleProgramTriggerCandidates(matchParticipantsState.ReferenceToPlayers[ownerSeat], SkillProgramTriggerWindow.OtherActualTurnStarted, facts);
    public int RelativeTableOrder(int from, int target) => getCoreGameEngineTableOrderRuntime().RelativeTableOrder(from, target);
}
