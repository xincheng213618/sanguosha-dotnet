#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class DamageTargetMountProgramCapability(Func<RuntimeEntries.CoreGameEngineDamageTargetMountProgramsEntries> getCoreGameEngineDamageTargetMountProgramsRuntime) : IDamageTargetMountProgramHost
{
    public SkillProgramStepOutcome DiscardDamageTargetAndClaimMount(ProgramSkillFrame f) => getCoreGameEngineDamageTargetMountProgramsRuntime().BeginDamageTargetMount(f);
}
