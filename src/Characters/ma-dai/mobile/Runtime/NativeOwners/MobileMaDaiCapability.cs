#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileMaDaiCapability(Func<RuntimeEntries.CharacterMaDaiMobileEntries> getCharacterMaDaiMobileRuntime) : IMobileMaDaiProgramHost
{
    public SkillProgramStepOutcome ReduceDamageTargetMaximum(ProgramSkillFrame frame) => getCharacterMaDaiMobileRuntime().ReduceMobileMaDaiTargetMaximum(frame);
}
