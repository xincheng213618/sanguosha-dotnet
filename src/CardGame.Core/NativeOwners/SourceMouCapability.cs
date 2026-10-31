#nullable enable
using System.Globalization;
using CardGame.Core.Campaigns;

namespace CardGame.Core.NativeOwners;
internal sealed class SourceMouCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheSourceMouDongZhuoEntries> getCoreGameEngineShanheSourceMouDongZhuoRuntime) : IShanheSourceMouHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, SkillProgramEffectOp operation) => getCoreGameEngineShanheSourceMouDongZhuoRuntime().StartSourceMou(frame, operation);
}
