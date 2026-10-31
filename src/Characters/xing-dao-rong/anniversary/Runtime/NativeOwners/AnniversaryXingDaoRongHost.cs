#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryXingDaoRongHost(Func<RuntimeEntries.CharacterXingDaoRongAnniversaryEntries> getCharacterXingDaoRongAnniversaryRuntime) : IAnniversaryXingDaoRongHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f) => getCharacterXingDaoRongAnniversaryRuntime().BeginXingDaoRong(f);
}
