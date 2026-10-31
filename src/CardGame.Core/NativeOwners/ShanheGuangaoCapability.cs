#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShanheGuangaoCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineShanheGuangaoRuntime>> getCoreGameEngineShanheGuangaoRuntime) : IShanheGuangaoHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame) => getCoreGameEngineShanheGuangaoRuntime().Value.BeginShanheGuangao(frame);
}
