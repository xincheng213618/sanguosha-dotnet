#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class DamageTargetObtainProgramCapability(Func<RuntimeEntries.CoreGameEngineDamageTargetObtainProgramsEntries> getCoreGameEngineDamageTargetObtainProgramsRuntime) : IDamageTargetObtainProgramHost
{
    public SkillProgramStepOutcome ObtainDamageTargetCardAndResolveCategory(ProgramSkillFrame f) => getCoreGameEngineDamageTargetObtainProgramsRuntime().BeginDamageTargetObtain(f);
}
