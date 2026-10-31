#nullable enable
using CardGame.Core.Campaigns;

namespace CardGame.Core.NativeOwners;
internal sealed class ShanheSourceYanyuCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheSourceYanyuEntries> getCoreGameEngineShanheSourceYanyuRuntime) : IShanheSourceYanyuHost
{
    public SkillProgramStepOutcome ResolveSourceYanyu(ProgramSkillFrame frame, SkillProgramEffect effect) => getCoreGameEngineShanheSourceYanyuRuntime().ResolveShanheSourceYanyu(frame, effect);
}
