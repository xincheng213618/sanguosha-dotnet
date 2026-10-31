#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MobileZhouFangHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterZhouFangMobileRuntime>> getCharacterZhouFangMobileRuntime) : IMobileZhouFangHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f) => getCharacterZhouFangMobileRuntime().Value.BeginMobileDuanfa(f);
    public void PublishBait(ProgramSkillFrame f, string bind) => getCharacterZhouFangMobileRuntime().Value.PublishMobileYoudiBait(f, bind);
}
