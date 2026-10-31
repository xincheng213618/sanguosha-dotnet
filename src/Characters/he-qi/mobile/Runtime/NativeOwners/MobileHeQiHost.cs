#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileHeQiHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHeQiMobileEntries> getCharacterHeQiMobileRuntime) : IMobileHeQiHost
{
    public SkillProgramStepOutcome View(ProgramSkillFrame f) => getCharacterHeQiMobileRuntime().BeginMobileHeQiView(f);
}
