#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryZhangHengHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterZhangHengAnniversaryRuntime>> getCharacterZhangHengAnniversaryRuntime) : IAnniversaryZhangHengHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f) => getCharacterZhangHengAnniversaryRuntime().Value.BeginZhangHengDangzai(f);
}
