#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class SourceExecutionerCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineSourceExecutionerYishiRuntime>> getCoreGameEngineSourceExecutionerYishiRuntime, Func<RuntimeEntries.CoreGameEngineSourceExecutionerHuanfuEntries> getCoreGameEngineSourceExecutionerHuanfuRuntime) : ISourceExecutionerHost
{
    public SkillProgramStepOutcome Yishi(ProgramSkillFrame f) => getCoreGameEngineSourceExecutionerYishiRuntime().Value.ResolveSourceExecutionerYishi(f);
    public SkillProgramStepOutcome Huanfu(ProgramSkillFrame f) => getCoreGameEngineSourceExecutionerHuanfuRuntime().BeginSourceExecutionerHuanfu(f);
}
