#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShiSunShaoCapability(Func<RuntimeEntries.CharacterSunShaoMobileShiEntries> getCharacterSunShaoMobileShiRuntime) : IShiSunShaoHost
{
    public SkillProgramStepOutcome ResolveGanjue(ProgramSkillFrame frame) => getCharacterSunShaoMobileShiRuntime().BeginShiSsGanjue(frame);
    public SkillProgramStepOutcome ResolveZhuji(ProgramSkillFrame frame) => getCharacterSunShaoMobileShiRuntime().BeginShiSsZhuji(frame);
}
