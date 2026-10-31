#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ForcedPhaseAlcoholCapability(Func<RuntimeEntries.CoreGameEngineForcedPhaseAlcoholEntries> getCoreGameEngineForcedPhaseAlcoholRuntime) : IForcedPhaseAlcoholHost
{
    public SkillProgramStepOutcome UsePhaseOwnerVirtualAlcohol(ProgramSkillFrame f, int damageToOther) => getCoreGameEngineForcedPhaseAlcoholRuntime().BeginPhaseOwnerAlcohol(f, damageToOther);
}
