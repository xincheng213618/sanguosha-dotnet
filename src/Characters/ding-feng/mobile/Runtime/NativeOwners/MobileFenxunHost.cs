#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileFenxunHost(Func<RuntimeEntries.CharacterDingFengMobileEntries> getCharacterDingFengMobileRuntime) : IMobileFenxunHost
{
    public SkillProgramStepOutcome Grant(ProgramSkillFrame frame) => getCharacterDingFengMobileRuntime().GrantMobileFenxun(frame);
}
