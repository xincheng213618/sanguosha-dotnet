#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MouZhouYuMobileCapability(Func<RuntimeEntries.CharacterZhouYuMouMobileEntries> getCharacterZhouYuMouMobileRuntime) : IMouZhouYuMobileHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, bool yingzi) => getCharacterZhouYuMouMobileRuntime().BeginMouZyMobile(f, yingzi);
}
