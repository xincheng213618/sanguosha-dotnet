#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileMaDaiCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterMaDaiMobileRuntime>> getCharacterMaDaiMobileRuntime) : IMobileMaDaiProgramHost
{
    public SkillProgramStepOutcome ReduceDamageTargetMaximum(ProgramSkillFrame frame) => getCharacterMaDaiMobileRuntime().Value.ReduceMobileMaDaiTargetMaximum(frame);
}
