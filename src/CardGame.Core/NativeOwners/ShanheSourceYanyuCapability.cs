#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShanheSourceYanyuCapability(Func<RuntimeEntries.CoreGameEngineShanheSourceYanyuEntries> getCoreGameEngineShanheSourceYanyuRuntime) : IShanheSourceYanyuHost
{
    public SkillProgramStepOutcome ResolveSourceYanyu(ProgramSkillFrame frame, SkillProgramEffect effect) => getCoreGameEngineShanheSourceYanyuRuntime().ResolveShanheSourceYanyu(frame, effect);
}
