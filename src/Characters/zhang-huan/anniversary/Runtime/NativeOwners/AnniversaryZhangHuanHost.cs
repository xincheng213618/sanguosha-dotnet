#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryZhangHuanHost(Func<RuntimeEntries.CharacterZhangHuanAnniversaryEntries> getCharacterZhangHuanAnniversaryRuntime) : IAnniversaryZhangHuanHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f) => getCharacterZhangHuanAnniversaryRuntime().BeginZhangHuan(f);
}
