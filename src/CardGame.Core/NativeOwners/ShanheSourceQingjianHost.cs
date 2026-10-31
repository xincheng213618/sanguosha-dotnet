#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShanheSourceQingjianHost(Func<RuntimeEntries.CoreGameEngineShanheSourceQingjianEntries> getCoreGameEngineShanheSourceQingjianRuntime) : IShanheSourceQingjianHost
{
    public SkillProgramStepOutcome DistributeOriginalGains(ProgramSkillFrame frame) => getCoreGameEngineShanheSourceQingjianRuntime().DistributeSourceQingjianGains(frame);
}
