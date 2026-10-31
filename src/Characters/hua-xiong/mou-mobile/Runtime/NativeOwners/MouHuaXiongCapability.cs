#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class MouHuaXiongCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHuaXiongMouMobileEntries> getCharacterHuaXiongMouMobileRuntime) : IMouHuaXiongHost
{
    public SkillProgramStepOutcome Yangwei(ProgramSkillFrame f) => getCharacterHuaXiongMouMobileRuntime().ResolveMouYangwei(f);
    public SkillProgramStepOutcome Yaowu(ProgramSkillFrame f) => getCharacterHuaXiongMouMobileRuntime().ResolveMouYaowu(f);
}
