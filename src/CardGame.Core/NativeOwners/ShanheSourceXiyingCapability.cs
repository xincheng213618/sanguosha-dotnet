#nullable enable
using System.Globalization;
using CardGame.Core.Campaigns;

namespace CardGame.Core.NativeOwners;
internal sealed class ShanheSourceXiyingCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheSourceXiyingEntries> getCoreGameEngineShanheSourceXiyingRuntime) : IShanheSourceXiyingHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCoreGameEngineShanheSourceXiyingRuntime().BeginSourceXiying(f);
}
