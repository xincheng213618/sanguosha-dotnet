#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryBlyHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterBaiLingYunAnniversaryEntries> getCharacterBaiLingYunAnniversaryRuntime) : IAnniversaryBaiLingYunHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame frame) => getCharacterBaiLingYunAnniversaryRuntime().BeginAnniversaryBly(frame);
}
