#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouPangTongCapability(Func<RuntimeEntries.CharacterPangTongMouMobileEntries> getCharacterPangTongMouMobileRuntime) : IMouPangTongHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterPangTongMouMobileRuntime().ResolveMouPt(f, op);
}
