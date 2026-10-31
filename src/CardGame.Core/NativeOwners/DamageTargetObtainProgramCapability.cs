#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class DamageTargetObtainProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageTargetObtainProgramsEntries> getCoreGameEngineDamageTargetObtainProgramsRuntime) : IDamageTargetObtainProgramHost
{
    public SkillProgramStepOutcome ObtainDamageTargetCardAndResolveCategory(ProgramSkillFrame f) => getCoreGameEngineDamageTargetObtainProgramsRuntime().BeginDamageTargetObtain(f);
}
