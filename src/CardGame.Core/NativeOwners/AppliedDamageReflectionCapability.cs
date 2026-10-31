#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AppliedDamageReflectionCapability(Func<RuntimeEntries.CoreGameEngineAppliedDamageReflectionEntries> getCoreGameEngineAppliedDamageReflectionRuntime) : IAppliedDamageReflectionHost
{
    public SkillProgramStepOutcome ReflectAppliedDamage(ProgramSkillFrame frame) => getCoreGameEngineAppliedDamageReflectionRuntime().BeginAppliedDamageReflection(frame);
}
