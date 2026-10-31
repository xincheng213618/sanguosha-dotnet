#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouLuMengCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLuMengMouMobileEntries> getCharacterLuMengMouMobileRuntime) : IMouLuMengHost
{
    public SkillProgramStepOutcome Keji(ProgramSkillFrame f) => getCharacterLuMengMouMobileRuntime().ResolveMouKeji(f);
    public SkillProgramStepOutcome Dujiang(ProgramSkillFrame f) => getCharacterLuMengMouMobileRuntime().ResolveMouDujiang(f);
    public SkillProgramStepOutcome Duojing(ProgramSkillFrame f) => getCharacterLuMengMouMobileRuntime().BeginMouDuojing(f);
}
