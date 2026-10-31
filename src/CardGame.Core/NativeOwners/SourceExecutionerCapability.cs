#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class SourceExecutionerCapability(Func<RuntimeEntries.CoreGameEngineSourceExecutionerYishiEntries> getCoreGameEngineSourceExecutionerYishiRuntime, Func<RuntimeEntries.CoreGameEngineSourceExecutionerHuanfuEntries> getCoreGameEngineSourceExecutionerHuanfuRuntime) : ISourceExecutionerHost
{
    public SkillProgramStepOutcome Yishi(ProgramSkillFrame f) => getCoreGameEngineSourceExecutionerYishiRuntime().ResolveSourceExecutionerYishi(f);
    public SkillProgramStepOutcome Huanfu(ProgramSkillFrame f) => getCoreGameEngineSourceExecutionerHuanfuRuntime().BeginSourceExecutionerHuanfu(f);
}
