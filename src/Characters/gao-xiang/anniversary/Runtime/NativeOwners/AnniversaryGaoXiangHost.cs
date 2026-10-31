#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryGaoXiangHost(Func<RuntimeEntries.CharacterGaoXiangAnniversaryEntries> getCharacterGaoXiangAnniversaryRuntime) : IAnniversaryGaoXiangHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f) => getCharacterGaoXiangAnniversaryRuntime().BeginGaoXiang(f);
}
