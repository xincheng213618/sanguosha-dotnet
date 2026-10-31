#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileYuJinHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterYuJinMobileEntries> getCharacterYuJinMobileRuntime) : IMobileYuJinHost
{
    public SkillProgramStepOutcome Transfer(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterYuJinMobileRuntime().BeginMobileYuJinTransfer(frame, effect);
    public bool MatchesChoiceResult(ProgramSkillFrame frame, ProgramChoiceResultBinding binding) => getCharacterYuJinMobileRuntime().MatchesMobileYuJinChoiceResult(frame, binding);
}
