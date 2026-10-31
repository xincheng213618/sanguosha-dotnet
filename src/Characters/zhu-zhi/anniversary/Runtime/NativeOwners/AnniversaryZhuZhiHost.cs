#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryZhuZhiHost(Func<RuntimeEntries.CharacterZhuZhiAnniversaryEntries> getCharacterZhuZhiAnniversaryRuntime) : IAnniversaryZhuZhiHost
{
    public SkillProgramStepOutcome Qualify(ProgramSkillFrame frame, string resultBind) => getCharacterZhuZhiAnniversaryRuntime().QualifyAnniversaryAnguo(frame, resultBind);
}
