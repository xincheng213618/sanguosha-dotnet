#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileYuJinHost(Func<RuntimeEntries.CharacterYuJinMobileEntries> getCharacterYuJinMobileRuntime) : IMobileYuJinHost
{
    public SkillProgramStepOutcome Transfer(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterYuJinMobileRuntime().BeginMobileYuJinTransfer(frame, effect);
}
