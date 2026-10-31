#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileHeQiHost(Func<RuntimeEntries.CharacterHeQiMobileEntries> getCharacterHeQiMobileRuntime) : IMobileHeQiHost
{
    public SkillProgramStepOutcome View(ProgramSkillFrame f) => getCharacterHeQiMobileRuntime().BeginMobileHeQiView(f);
}
