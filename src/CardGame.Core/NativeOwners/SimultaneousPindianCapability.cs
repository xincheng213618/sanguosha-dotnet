#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class SimultaneousPindianCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineSimultaneousPindianRuntime>> getCoreGameEngineSimultaneousPindianRuntime, Func<RuntimeEntries.CoreGameEnginePindianRevealWindowsEntries> getCoreGameEnginePindianRevealWindowsRuntime) : ISimultaneousPindianHost
{
    public SkillProgramStepOutcome StartSimultaneousPindian(ProgramSkillFrame f) => getCoreGameEngineSimultaneousPindianRuntime().Value.StartSimultaneousPindian(f);
    public void AddRaoshePindianRankAndRestoreGushe(ProgramSkillFrame f, string skillId) => getCoreGameEnginePindianRevealWindowsRuntime().AddRaoshePindianRankAndRestoreGushe(f, skillId);
}
