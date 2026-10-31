#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class JiqiaoProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineJiqiaoProgramsEntries> getCoreGameEngineJiqiaoProgramsRuntime) : IJiqiaoProgramHost
{
    public SkillProgramStepOutcome ExecutePaidJiqiao(ProgramSkillFrame f, SkillProgramEffect e) => getCoreGameEngineJiqiaoProgramsRuntime().ExecutePaidJiqiao(f, e);
}
