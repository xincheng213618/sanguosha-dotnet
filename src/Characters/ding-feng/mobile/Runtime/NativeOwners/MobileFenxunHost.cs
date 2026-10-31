#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileFenxunHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterDingFengMobileEntries> getCharacterDingFengMobileRuntime) : IMobileFenxunHost
{
    public SkillProgramStepOutcome Grant(ProgramSkillFrame frame) => getCharacterDingFengMobileRuntime().GrantMobileFenxun(frame);
}
