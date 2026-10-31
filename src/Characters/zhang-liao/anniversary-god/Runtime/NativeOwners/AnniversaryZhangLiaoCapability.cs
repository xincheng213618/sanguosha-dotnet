#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryZhangLiaoCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhangLiaoAnniversaryGodEntries> getCharacterZhangLiaoAnniversaryGodRuntime) : IAnniversaryZhangLiaoHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterZhangLiaoAnniversaryGodRuntime().BeginAnnZl(f, op);
}
