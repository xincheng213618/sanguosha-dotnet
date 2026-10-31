#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AppliedDamageReflectionCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineAppliedDamageReflectionEntries> getCoreGameEngineAppliedDamageReflectionRuntime) : IAppliedDamageReflectionHost
{
    public SkillProgramStepOutcome ReflectAppliedDamage(ProgramSkillFrame frame) => getCoreGameEngineAppliedDamageReflectionRuntime().BeginAppliedDamageReflection(frame);
}
