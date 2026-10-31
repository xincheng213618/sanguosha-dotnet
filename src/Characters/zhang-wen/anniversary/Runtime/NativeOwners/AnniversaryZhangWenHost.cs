#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryZhangWenHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterZhangWenAnniversaryRuntime>> getCharacterZhangWenAnniversaryRuntime) : IAnniversaryZhangWenHost
{
    public void Refund(ProgramSkillFrame frame) => getCharacterZhangWenAnniversaryRuntime().Value.RefundZhangWenSongshu(frame);
    public SkillProgramStepOutcome Sibian(ProgramSkillFrame frame) => getCharacterZhangWenAnniversaryRuntime().Value.BeginZhangWenSibian(frame);
}
