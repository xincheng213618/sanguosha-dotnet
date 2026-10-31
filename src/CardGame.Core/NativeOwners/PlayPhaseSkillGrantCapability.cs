#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class PlayPhaseSkillGrantCapability(Func<RuntimeEntries.CoreGameEnginePlayPhaseSkillGrantsEntries> getCoreGameEnginePlayPhaseSkillGrantsRuntime) : IPlayPhaseSkillGrantHost
{
    public void GrantPlayPhaseSkills(ProgramSkillFrame frame, IReadOnlyList<string> skillIds) => getCoreGameEnginePlayPhaseSkillGrantsRuntime().GrantProgramPlayPhaseSkills(frame, skillIds);
}
