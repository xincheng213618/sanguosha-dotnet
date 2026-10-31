#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class SpSunShangxiangOlCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterSunShangxiangSpOlRuntime>> getCharacterSunShangxiangSpOlRuntime) : ISpSunShangxiangOlHost
{
    public SkillProgramStepOutcome Liangzhu(ProgramSkillFrame f) => getCharacterSunShangxiangSpOlRuntime().Value.BeginSpSunShangxiangLiangzhu(f);
    public SkillProgramStepOutcome CompleteFanxiang(ProgramSkillFrame f) => getCharacterSunShangxiangSpOlRuntime().Value.CompleteSpSunShangxiangFanxiang(f);
}
