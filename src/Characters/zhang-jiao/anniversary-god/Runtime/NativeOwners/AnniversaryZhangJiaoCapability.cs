#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryZhangJiaoCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhangJiaoAnniversaryGodEntries> getCharacterZhangJiaoAnniversaryGodRuntime) : IAnniversaryZhangJiaoHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffect e) => getCharacterZhangJiaoAnniversaryGodRuntime().BeginAnniversaryZj(f, e);
}
