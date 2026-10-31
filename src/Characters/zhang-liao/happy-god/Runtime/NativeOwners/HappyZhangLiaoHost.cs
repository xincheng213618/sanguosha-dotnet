#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class HappyZhangLiaoHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhangLiaoHappyGodEntries> getCharacterZhangLiaoHappyGodRuntime) : IHappyZhangLiaoHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterZhangLiaoHappyGodRuntime().BeginHappyZl(f, op);
}
