#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class DamageSourceHandChoiceCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineDamageSourceHandChoiceRuntime>> getCoreGameEngineDamageSourceHandChoiceRuntime) : IDamageSourceHandChoiceHost
{
    public SkillProgramStepOutcome ResolveDamageSourceHandChoice(ProgramSkillFrame f) => getCoreGameEngineDamageSourceHandChoiceRuntime().Value.BeginDamageSourceHandChoice(f);
}
