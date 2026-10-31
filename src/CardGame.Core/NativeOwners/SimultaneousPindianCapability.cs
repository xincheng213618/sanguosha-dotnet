#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class SimultaneousPindianCapability(Func<RuntimeEntries.CoreGameEngineSimultaneousPindianEntries> getCoreGameEngineSimultaneousPindianRuntime, Func<RuntimeEntries.CoreGameEnginePindianRevealWindowsEntries> getCoreGameEnginePindianRevealWindowsRuntime) : ISimultaneousPindianHost
{
    public SkillProgramStepOutcome StartSimultaneousPindian(ProgramSkillFrame f) => getCoreGameEngineSimultaneousPindianRuntime().StartSimultaneousPindian(f);
    public void AddRaoshePindianRankAndRestoreGushe(ProgramSkillFrame f, string skillId) => getCoreGameEnginePindianRevealWindowsRuntime().AddRaoshePindianRankAndRestoreGushe(f, skillId);
}
