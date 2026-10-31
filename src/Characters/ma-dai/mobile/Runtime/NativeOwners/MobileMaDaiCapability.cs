#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileMaDaiCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterMaDaiMobileEntries> getCharacterMaDaiMobileRuntime) : IMobileMaDaiProgramHost
{
    public SkillProgramStepOutcome ReduceDamageTargetMaximum(ProgramSkillFrame frame) => getCharacterMaDaiMobileRuntime().ReduceMobileMaDaiTargetMaximum(frame);
}
