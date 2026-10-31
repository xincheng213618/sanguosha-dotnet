#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouYuJinCapability(Func<RuntimeEntries.CharacterYuJinMouMobileEntries> getCharacterYuJinMouMobileRuntime) : IMouYuJinHost
{
    public SkillProgramStepOutcome Xiayuan(ProgramSkillFrame f) => getCharacterYuJinMouMobileRuntime().ResolveMouXiayuan(f);
    public SkillProgramStepOutcome Jieyue(ProgramSkillFrame f) => getCharacterYuJinMouMobileRuntime().ResolveMouJieyue(f);
}
