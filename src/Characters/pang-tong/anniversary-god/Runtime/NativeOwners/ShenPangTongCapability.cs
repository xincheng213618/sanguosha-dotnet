#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShenPangTongCapability(Func<RuntimeEntries.CharacterPangTongAnniversaryGodEntries> getCharacterPangTongAnniversaryGodRuntime) : IShenPangTongHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterPangTongAnniversaryGodRuntime().BeginShenPt(f, op);
}
