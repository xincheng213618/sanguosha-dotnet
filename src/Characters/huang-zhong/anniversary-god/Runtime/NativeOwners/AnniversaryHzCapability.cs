#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryHzCapability(Func<RuntimeEntries.CharacterHuangZhongAnniversaryGodEntries> getCharacterHuangZhongAnniversaryGodRuntime) : IAnniversaryHzHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterHuangZhongAnniversaryGodRuntime().BeginAnniversaryHz(f, op);
}
