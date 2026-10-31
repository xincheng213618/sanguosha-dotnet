#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShanheSourceTiepaoHost(Func<RuntimeEntries.CoreGameEngineShanheSourceTiepaoEntries> getCoreGameEngineShanheSourceTiepaoRuntime) : IShanheSourceTiepaoHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f) => getCoreGameEngineShanheSourceTiepaoRuntime().BeginShanheSourceTiepao(f);
}
