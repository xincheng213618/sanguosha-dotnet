#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class JiqiaoProgramCapability(Func<RuntimeEntries.CoreGameEngineJiqiaoProgramsEntries> getCoreGameEngineJiqiaoProgramsRuntime) : IJiqiaoProgramHost
{
    public SkillProgramStepOutcome ExecutePaidJiqiao(ProgramSkillFrame f, SkillProgramEffect e) => getCoreGameEngineJiqiaoProgramsRuntime().ExecutePaidJiqiao(f, e);
}
