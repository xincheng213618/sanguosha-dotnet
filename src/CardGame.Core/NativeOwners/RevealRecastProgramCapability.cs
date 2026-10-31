#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class RevealRecastProgramCapability(Func<RuntimeEntries.CoreGameEngineRevealRecastProgramsEntries> getCoreGameEngineRevealRecastProgramsRuntime) : IRevealRecastProgramHost
{
    public SkillProgramStepOutcome RecastSelectedCards(ProgramSkillFrame frame, string stateId, int threshold) => getCoreGameEngineRevealRecastProgramsRuntime().RecastProgramSelectedCards(frame, stateId, threshold);
    public void DrawCompletedCardParticipants(ProgramSkillFrame frame, string stateId, int threshold) => getCoreGameEngineRevealRecastProgramsRuntime().DrawProgramCompletedCardParticipants(frame, stateId, threshold);
    public SkillProgramStepOutcome RevealSelectedHandAgainstTarget(ProgramSkillFrame frame, string resultBind) => getCoreGameEngineRevealRecastProgramsRuntime().RevealProgramSelectedHandAgainstTarget(frame, resultBind);
}
