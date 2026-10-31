#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class LiushiEffectCapability(Func<RuntimeEntries.CoreGameEngineLiushiEffectsEntries> getCoreGameEngineLiushiEffectsRuntime) : ILiushiEffectHost
{
    public void ApplyLiushiEffect(ProgramSkillFrame f) => getCoreGameEngineLiushiEffectsRuntime().ApplyProgramLiushiEffect(f);
    public void ClearLiushiEffect(ProgramSkillFrame f) => getCoreGameEngineLiushiEffectsRuntime().ClearProgramLiushiEffect(f);
}
