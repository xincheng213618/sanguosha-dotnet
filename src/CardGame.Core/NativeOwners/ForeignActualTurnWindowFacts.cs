#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ForeignActualTurnWindowFacts(global::CardGame.Core.EngineStates.CoreMatchOutcomeState matchOutcomeState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineTableOrderEntries> getCoreGameEngineTableOrderRuntime) : IForeignActualTurnWindowFacts
{
    public Winner Winner => matchOutcomeState.ReferenceToWinner;
    public EngineStatus Status => matchOutcomeState.ReferenceToStatus;

    public SkillProgramTriggerFacts CaptureTriggerFacts(int ownerSeat) => getCoreGameEngineProgramLifecycleRuntime().CaptureProgramTriggerFacts(matchParticipantsState.ReferenceToPlayers[ownerSeat]);
    public IReadOnlyList<ProgramTriggerCandidate> CollectCandidates(int ownerSeat, SkillProgramTriggerFacts facts) => getCoreGameEngineProgramLifecycleRuntime().CollectEligibleProgramTriggerCandidates(matchParticipantsState.ReferenceToPlayers[ownerSeat], SkillProgramTriggerWindow.OtherActualTurnStarted, facts);
    public int RelativeTableOrder(int from, int target) => getCoreGameEngineTableOrderRuntime().RelativeTableOrder(from, target);
}
