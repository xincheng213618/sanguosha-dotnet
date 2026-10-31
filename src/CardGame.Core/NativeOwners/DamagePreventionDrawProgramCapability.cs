#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class DamagePreventionDrawProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineDamagePreventionDrawRuntime>> getCoreGameEngineDamagePreventionDrawRuntime) : IDamagePreventionDrawProgramHost
{
    public SkillProgramStepOutcome PreventDamageAndDrawMultiple(ProgramSkillFrame frame, int multiplier) => getCoreGameEngineDamagePreventionDrawRuntime().Value.PreventDamageAndDrawMultiple(frame, multiplier);
}
