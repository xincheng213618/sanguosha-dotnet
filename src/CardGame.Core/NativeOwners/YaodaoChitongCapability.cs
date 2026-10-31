#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class YaodaoChitongCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineYaodaoChitongRuntime>> getCoreGameEngineYaodaoChitongRuntime) : IYaodaoChitongHost
{
    public SkillProgramStepOutcome BeginYaodaoChitong(ProgramSkillFrame f) => getCoreGameEngineYaodaoChitongRuntime().Value.BeginYaodaoChitong(f);
    public SkillProgramStepOutcome ResolveYaodaoChitong(ProgramSkillFrame f, SkillProgramEffect e) => getCoreGameEngineYaodaoChitongRuntime().Value.ResolveYaodaoChitong(f, e);
}
