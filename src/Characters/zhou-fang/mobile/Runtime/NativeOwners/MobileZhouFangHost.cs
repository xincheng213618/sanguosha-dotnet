#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MobileZhouFangHost(Func<RuntimeEntries.CharacterZhouFangMobileEntries> getCharacterZhouFangMobileRuntime) : IMobileZhouFangHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f) => getCharacterZhouFangMobileRuntime().BeginMobileDuanfa(f);
    public void PublishBait(ProgramSkillFrame f, string bind) => getCharacterZhouFangMobileRuntime().PublishMobileYoudiBait(f, bind);
}
