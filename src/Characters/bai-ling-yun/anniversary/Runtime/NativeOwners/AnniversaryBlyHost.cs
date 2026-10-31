#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryBlyHost(Func<RuntimeEntries.CharacterBaiLingYunAnniversaryEntries> getCharacterBaiLingYunAnniversaryRuntime) : IAnniversaryBaiLingYunHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame frame) => getCharacterBaiLingYunAnniversaryRuntime().BeginAnniversaryBly(frame);
}
