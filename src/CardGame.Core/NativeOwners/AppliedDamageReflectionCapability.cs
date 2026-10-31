#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AppliedDamageReflectionCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineAppliedDamageReflectionRuntime>> getCoreGameEngineAppliedDamageReflectionRuntime) : IAppliedDamageReflectionHost
{
    public SkillProgramStepOutcome ReflectAppliedDamage(ProgramSkillFrame frame) => getCoreGameEngineAppliedDamageReflectionRuntime().Value.BeginAppliedDamageReflection(frame);
}
