#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class ShanheSourceQingjianHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheSourceQingjianEntries> getCoreGameEngineShanheSourceQingjianRuntime) : IShanheSourceQingjianHost
{
    public SkillProgramStepOutcome DistributeOriginalGains(ProgramSkillFrame frame) => getCoreGameEngineShanheSourceQingjianRuntime().DistributeSourceQingjianGains(frame);
}
