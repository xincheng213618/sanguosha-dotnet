#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramPhaseSchedulingFacts(global::CardGame.Core.EngineStates.CoreMatchOutcomeState matchOutcomeState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineAfterTurnEndedProgramsEntries> getCoreGameEngineAfterTurnEndedProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineGrantedEntityPhasesEntries> getCoreGameEngineGrantedEntityPhasesRuntime) : IProgramPhaseSchedulingFacts
{
    public Winner Winner => matchOutcomeState.ReferenceToWinner;

    public bool MatchesAfterTurnEndedChild(DeferredTurnEndFrame parent, ProgramSkillFrame child, ProgramSkillWindowContext context) => getCoreGameEngineAfterTurnEndedProgramsRuntime().MatchesAfterTurnEndedChild(parent, child, context);
    public bool HasRuntimeSkillInstance(int seat, string skillId, string instanceId) => getCoreGameEngineNationalSkillsRuntime().HasRuntimeSkillInstance(matchParticipantsState.ReferenceToPlayers[seat], skillId, instanceId);
    public bool IsReturningGrantedEntityPhase(ProgramSkillFrame frame) => getCoreGameEngineGrantedEntityPhasesRuntime().IsReturningGrantedEntityPhase(frame);
}
