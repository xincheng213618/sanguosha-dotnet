#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryZhangWenHost(Func<RuntimeEntries.CharacterZhangWenAnniversaryEntries> getCharacterZhangWenAnniversaryRuntime) : IAnniversaryZhangWenHost
{
    public void Refund(ProgramSkillFrame frame) => getCharacterZhangWenAnniversaryRuntime().RefundZhangWenSongshu(frame);
    public SkillProgramStepOutcome Sibian(ProgramSkillFrame frame) => getCharacterZhangWenAnniversaryRuntime().BeginZhangWenSibian(frame);
}
