#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class DamagePreventionDrawProgramCapability(Func<RuntimeEntries.CoreGameEngineDamagePreventionDrawEntries> getCoreGameEngineDamagePreventionDrawRuntime) : IDamagePreventionDrawProgramHost
{
    public SkillProgramStepOutcome PreventDamageAndDrawMultiple(ProgramSkillFrame frame, int multiplier) => getCoreGameEngineDamagePreventionDrawRuntime().PreventDamageAndDrawMultiple(frame, multiplier);
}
