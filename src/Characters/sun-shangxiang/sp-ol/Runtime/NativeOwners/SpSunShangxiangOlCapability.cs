#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class SpSunShangxiangOlCapability(Func<RuntimeEntries.CharacterSunShangxiangSpOlEntries> getCharacterSunShangxiangSpOlRuntime) : ISpSunShangxiangOlHost
{
    public SkillProgramStepOutcome Liangzhu(ProgramSkillFrame f) => getCharacterSunShangxiangSpOlRuntime().BeginSpSunShangxiangLiangzhu(f);
    public SkillProgramStepOutcome CompleteFanxiang(ProgramSkillFrame f) => getCharacterSunShangxiangSpOlRuntime().CompleteSpSunShangxiangFanxiang(f);
}
