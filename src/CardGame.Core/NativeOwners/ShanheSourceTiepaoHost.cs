#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShanheSourceTiepaoHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineShanheSourceTiepaoRuntime>> getCoreGameEngineShanheSourceTiepaoRuntime) : IShanheSourceTiepaoHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f) => getCoreGameEngineShanheSourceTiepaoRuntime().Value.BeginShanheSourceTiepao(f);
}
