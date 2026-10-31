#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryZhangLiaoCapability(Func<RuntimeEntries.CharacterZhangLiaoAnniversaryGodEntries> getCharacterZhangLiaoAnniversaryGodRuntime) : IAnniversaryZhangLiaoHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterZhangLiaoAnniversaryGodRuntime().BeginAnnZl(f, op);
}
