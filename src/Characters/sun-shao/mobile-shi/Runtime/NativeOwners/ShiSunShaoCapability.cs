#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class ShiSunShaoCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterSunShaoMobileShiEntries> getCharacterSunShaoMobileShiRuntime) : IShiSunShaoHost
{
    public SkillProgramStepOutcome ResolveGanjue(ProgramSkillFrame frame) => getCharacterSunShaoMobileShiRuntime().BeginShiSsGanjue(frame);
    public SkillProgramStepOutcome ResolveZhuji(ProgramSkillFrame frame) => getCharacterSunShaoMobileShiRuntime().BeginShiSsZhuji(frame);
}
