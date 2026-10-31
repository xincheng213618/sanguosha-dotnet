#nullable enable
using CardGame.Core.Campaigns;

namespace CardGame.Core.NativeOwners;
internal sealed class ShanheSourceBiriCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheSourceBiriEntries> getCoreGameEngineShanheSourceBiriRuntime) : IShanheSourceBiriHost
{
    public SkillProgramStepOutcome PreventFireRecoverDraw(ProgramSkillFrame frame) => getCoreGameEngineShanheSourceBiriRuntime().BeginShanheSourceBiri(frame);
}
