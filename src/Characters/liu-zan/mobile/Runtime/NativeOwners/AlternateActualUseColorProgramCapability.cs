#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AlternateActualUseColorProgramCapability(Func<RuntimeEntries.CharacterLiuZanMobileEntries> getCharacterLiuZanMobileRuntime) : IAlternateActualUseColorProgramHost
{
    public void GrantAlternateActualUseColorDraw(ProgramSkillFrame frame) => getCharacterLiuZanMobileRuntime().GrantAlternateActualUseColorDraw(frame);
}
