#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AlternateActualUseColorProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLiuZanMobileEntries> getCharacterLiuZanMobileRuntime) : IAlternateActualUseColorProgramHost
{
    public void GrantAlternateActualUseColorDraw(ProgramSkillFrame frame) => getCharacterLiuZanMobileRuntime().GrantAlternateActualUseColorDraw(frame);
}
