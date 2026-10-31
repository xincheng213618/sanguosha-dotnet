#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShenPangTongCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterPangTongAnniversaryGodEntries> getCharacterPangTongAnniversaryGodRuntime) : IShenPangTongHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterPangTongAnniversaryGodRuntime().BeginShenPt(f, op);
}
