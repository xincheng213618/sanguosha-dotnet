#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class HappyZhangLiaoHost(Func<RuntimeEntries.CharacterZhangLiaoHappyGodEntries> getCharacterZhangLiaoHappyGodRuntime) : IHappyZhangLiaoHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterZhangLiaoHappyGodRuntime().BeginHappyZl(f, op);
}
