#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryZhuZhiHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterZhuZhiAnniversaryRuntime>> getCharacterZhuZhiAnniversaryRuntime) : IAnniversaryZhuZhiHost
{
    public SkillProgramStepOutcome Qualify(ProgramSkillFrame frame, string resultBind) => getCharacterZhuZhiAnniversaryRuntime().Value.QualifyAnniversaryAnguo(frame, resultBind);
}
