#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class YaodaoChitongCapability(Func<RuntimeEntries.CoreGameEngineYaodaoChitongEntries> getCoreGameEngineYaodaoChitongRuntime) : IYaodaoChitongHost
{
    public SkillProgramStepOutcome BeginYaodaoChitong(ProgramSkillFrame f) => getCoreGameEngineYaodaoChitongRuntime().BeginYaodaoChitong(f);
    public SkillProgramStepOutcome ResolveYaodaoChitong(ProgramSkillFrame f, SkillProgramEffect e) => getCoreGameEngineYaodaoChitongRuntime().ResolveYaodaoChitong(f, e);
}
