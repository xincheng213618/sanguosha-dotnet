#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramBindingFacts(global::System.Func<global::CardGame.Core.ContentRegistry> getContentRegistry, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineActualPlayPhaseCardUsesEntries> getCoreGameEngineActualPlayPhaseCardUsesRuntime) : IProgramBindingFacts
{
    public ContentSkillDefinition Skill(string skillId) => getContentRegistry().GetSkill(skillId);
    public SkillProgramTrigger Trigger(ProgramTriggerCandidate candidate) => getCoreGameEngineProgramLifecycleRuntime().GetProgramTrigger2(candidate);
    public bool CanRun(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => getCoreGameEngineProgramLifecycleRuntime().CanRunProgramTrigger(candidate, context);
    public bool IsHuman(int seat) => matchParticipantsState.ReferenceToPlayers[seat].IsHuman;
    public string Name(int seat) => matchParticipantsState.ReferenceToPlayers[seat].Name;
    public int ActualPlayPhaseUseCount(int seat) => getCoreGameEngineActualPlayPhaseCardUsesRuntime().GetActualPlayPhaseUseCount(seat);
}
