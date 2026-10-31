#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class DamageTargetMountProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageTargetMountProgramsEntries> getCoreGameEngineDamageTargetMountProgramsRuntime) : IDamageTargetMountProgramHost
{
    public SkillProgramStepOutcome DiscardDamageTargetAndClaimMount(ProgramSkillFrame f) => getCoreGameEngineDamageTargetMountProgramsRuntime().BeginDamageTargetMount(f);
}
