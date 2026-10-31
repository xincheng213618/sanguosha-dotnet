#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class SimultaneousPindianCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSimultaneousPindianEntries> getCoreGameEngineSimultaneousPindianRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePindianRevealWindowsEntries> getCoreGameEnginePindianRevealWindowsRuntime) : ISimultaneousPindianHost
{
    public SkillProgramStepOutcome StartSimultaneousPindian(ProgramSkillFrame f) => getCoreGameEngineSimultaneousPindianRuntime().StartSimultaneousPindian(f);
    public void AddRaoshePindianRankAndRestoreGushe(ProgramSkillFrame f, string skillId) => getCoreGameEnginePindianRevealWindowsRuntime().AddRaoshePindianRankAndRestoreGushe(f, skillId);
}
