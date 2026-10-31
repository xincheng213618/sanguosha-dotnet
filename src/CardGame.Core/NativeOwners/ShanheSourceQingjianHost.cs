#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShanheSourceQingjianHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineShanheSourceQingjianRuntime>> getCoreGameEngineShanheSourceQingjianRuntime) : IShanheSourceQingjianHost
{
    public SkillProgramStepOutcome DistributeOriginalGains(ProgramSkillFrame frame) => getCoreGameEngineShanheSourceQingjianRuntime().Value.DistributeSourceQingjianGains(frame);
}
