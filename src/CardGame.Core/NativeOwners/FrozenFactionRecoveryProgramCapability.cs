#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class FrozenFactionRecoveryProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineFrozenFactionRecoveryRuntime>> getCoreGameEngineFrozenFactionRecoveryRuntime) : IFrozenFactionRecoveryProgramHost
{
    public void FreezeLivingFactionRecovery(ProgramSkillFrame f) => getCoreGameEngineFrozenFactionRecoveryRuntime().Value.FreezeProgramLivingFactionRecovery(f);
    public void DrawToFrozenFactionCount(ProgramSkillFrame f) => getCoreGameEngineFrozenFactionRecoveryRuntime().Value.DrawProgramToFrozenFactionCount(f);
}
