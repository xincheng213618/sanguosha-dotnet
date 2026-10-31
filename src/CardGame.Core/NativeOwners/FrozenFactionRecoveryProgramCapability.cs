#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class FrozenFactionRecoveryProgramCapability(Func<RuntimeEntries.CoreGameEngineFrozenFactionRecoveryEntries> getCoreGameEngineFrozenFactionRecoveryRuntime) : IFrozenFactionRecoveryProgramHost
{
    public void FreezeLivingFactionRecovery(ProgramSkillFrame f) => getCoreGameEngineFrozenFactionRecoveryRuntime().FreezeProgramLivingFactionRecovery(f);
    public void DrawToFrozenFactionCount(ProgramSkillFrame f) => getCoreGameEngineFrozenFactionRecoveryRuntime().DrawProgramToFrozenFactionCount(f);
}
