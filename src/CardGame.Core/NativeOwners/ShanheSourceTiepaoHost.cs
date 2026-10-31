#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class ShanheSourceTiepaoHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheSourceTiepaoEntries> getCoreGameEngineShanheSourceTiepaoRuntime) : IShanheSourceTiepaoHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f) => getCoreGameEngineShanheSourceTiepaoRuntime().BeginShanheSourceTiepao(f);
}
