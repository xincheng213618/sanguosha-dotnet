#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouPangTongCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterPangTongMouMobileEntries> getCharacterPangTongMouMobileRuntime) : IMouPangTongHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterPangTongMouMobileRuntime().ResolveMouPt(f, op);
}
