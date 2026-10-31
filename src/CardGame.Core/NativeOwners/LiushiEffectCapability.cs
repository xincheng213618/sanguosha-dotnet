#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class LiushiEffectCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineLiushiEffectsRuntime>> getCoreGameEngineLiushiEffectsRuntime) : ILiushiEffectHost
{
    public void ApplyLiushiEffect(ProgramSkillFrame f) => getCoreGameEngineLiushiEffectsRuntime().Value.ApplyProgramLiushiEffect(f);
    public void ClearLiushiEffect(ProgramSkillFrame f) => getCoreGameEngineLiushiEffectsRuntime().Value.ClearProgramLiushiEffect(f);
}
