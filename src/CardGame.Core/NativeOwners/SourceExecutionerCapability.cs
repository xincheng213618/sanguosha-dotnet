#nullable enable
using CardGame.Core.Campaigns;

namespace CardGame.Core.NativeOwners;
internal sealed class SourceExecutionerCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSourceExecutionerYishiEntries> getCoreGameEngineSourceExecutionerYishiRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSourceExecutionerHuanfuEntries> getCoreGameEngineSourceExecutionerHuanfuRuntime) : ISourceExecutionerHost
{
    public SkillProgramStepOutcome Yishi(ProgramSkillFrame f) => getCoreGameEngineSourceExecutionerYishiRuntime().ResolveSourceExecutionerYishi(f);
    public SkillProgramStepOutcome Huanfu(ProgramSkillFrame f) => getCoreGameEngineSourceExecutionerHuanfuRuntime().BeginSourceExecutionerHuanfu(f);
}
