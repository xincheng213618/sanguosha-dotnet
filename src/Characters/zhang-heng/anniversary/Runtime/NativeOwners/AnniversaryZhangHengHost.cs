#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryZhangHengHost(Func<RuntimeEntries.CharacterZhangHengAnniversaryEntries> getCharacterZhangHengAnniversaryRuntime) : IAnniversaryZhangHengHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f) => getCharacterZhangHengAnniversaryRuntime().BeginZhangHengDangzai(f);
}
