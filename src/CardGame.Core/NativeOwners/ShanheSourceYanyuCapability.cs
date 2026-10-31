#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShanheSourceYanyuCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineShanheSourceYanyuRuntime>> getCoreGameEngineShanheSourceYanyuRuntime) : IShanheSourceYanyuHost
{
    public SkillProgramStepOutcome ResolveSourceYanyu(ProgramSkillFrame frame, SkillProgramEffect effect) => getCoreGameEngineShanheSourceYanyuRuntime().Value.ResolveShanheSourceYanyu(frame, effect);
}
