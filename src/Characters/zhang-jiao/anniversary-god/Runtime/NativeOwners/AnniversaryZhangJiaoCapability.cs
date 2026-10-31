#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryZhangJiaoCapability(Func<RuntimeEntries.CharacterZhangJiaoAnniversaryGodEntries> getCharacterZhangJiaoAnniversaryGodRuntime) : IAnniversaryZhangJiaoHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffect e) => getCharacterZhangJiaoAnniversaryGodRuntime().BeginAnniversaryZj(f, e);
}
