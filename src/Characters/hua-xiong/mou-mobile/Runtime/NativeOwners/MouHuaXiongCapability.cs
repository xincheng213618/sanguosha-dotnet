#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MouHuaXiongCapability(Func<RuntimeEntries.CharacterHuaXiongMouMobileEntries> getCharacterHuaXiongMouMobileRuntime) : IMouHuaXiongHost
{
    public SkillProgramStepOutcome Yangwei(ProgramSkillFrame f) => getCharacterHuaXiongMouMobileRuntime().ResolveMouYangwei(f);
    public SkillProgramStepOutcome Yaowu(ProgramSkillFrame f) => getCharacterHuaXiongMouMobileRuntime().ResolveMouYaowu(f);
}
