#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class SpSunShangxiangOlCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterSunShangxiangSpOlEntries> getCharacterSunShangxiangSpOlRuntime) : ISpSunShangxiangOlHost
{
    public SkillProgramStepOutcome Liangzhu(ProgramSkillFrame f) => getCharacterSunShangxiangSpOlRuntime().BeginSpSunShangxiangLiangzhu(f);
    public SkillProgramStepOutcome CompleteFanxiang(ProgramSkillFrame f) => getCharacterSunShangxiangSpOlRuntime().CompleteSpSunShangxiangFanxiang(f);
}
