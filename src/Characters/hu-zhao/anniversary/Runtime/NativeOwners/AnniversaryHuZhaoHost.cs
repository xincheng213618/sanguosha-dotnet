#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryHuZhaoHost(Func<RuntimeEntries.CharacterHuZhaoAnniversaryEntries> getCharacterHuZhaoAnniversaryRuntime) : IAnniversaryHuZhaoHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame) => getCharacterHuZhaoAnniversaryRuntime().BeginHuZhaoMidu(frame);
}
