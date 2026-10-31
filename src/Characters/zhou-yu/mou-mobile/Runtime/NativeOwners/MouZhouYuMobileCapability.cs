#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class MouZhouYuMobileCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhouYuMouMobileEntries> getCharacterZhouYuMouMobileRuntime) : IMouZhouYuMobileHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, bool yingzi) => getCharacterZhouYuMouMobileRuntime().BeginMouZyMobile(f, yingzi);
}
