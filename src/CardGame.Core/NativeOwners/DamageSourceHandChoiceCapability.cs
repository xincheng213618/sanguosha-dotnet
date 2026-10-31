#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class DamageSourceHandChoiceCapability(Func<RuntimeEntries.CoreGameEngineDamageSourceHandChoiceEntries> getCoreGameEngineDamageSourceHandChoiceRuntime) : IDamageSourceHandChoiceHost
{
    public SkillProgramStepOutcome ResolveDamageSourceHandChoice(ProgramSkillFrame f) => getCoreGameEngineDamageSourceHandChoiceRuntime().BeginDamageSourceHandChoice(f);
}
