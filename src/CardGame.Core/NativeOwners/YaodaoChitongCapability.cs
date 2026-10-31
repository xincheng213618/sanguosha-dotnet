#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class YaodaoChitongCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineYaodaoChitongEntries> getCoreGameEngineYaodaoChitongRuntime) : IYaodaoChitongHost
{
    public SkillProgramStepOutcome BeginYaodaoChitong(ProgramSkillFrame f) => getCoreGameEngineYaodaoChitongRuntime().BeginYaodaoChitong(f);
    public SkillProgramStepOutcome ResolveYaodaoChitong(ProgramSkillFrame f, SkillProgramEffect e) => getCoreGameEngineYaodaoChitongRuntime().ResolveYaodaoChitong(f, e);
}
