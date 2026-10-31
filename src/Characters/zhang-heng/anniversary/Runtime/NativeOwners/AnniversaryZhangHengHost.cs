#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryZhangHengHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhangHengAnniversaryEntries> getCharacterZhangHengAnniversaryRuntime) : IAnniversaryZhangHengHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f) => getCharacterZhangHengAnniversaryRuntime().BeginZhangHengDangzai(f);
}
