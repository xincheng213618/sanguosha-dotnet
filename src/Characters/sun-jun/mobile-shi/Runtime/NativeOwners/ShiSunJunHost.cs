#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShiSunJunHost(Func<RuntimeEntries.CharacterSunJunMobileShiEntries> getCharacterSunJunMobileShiRuntime) : IShiSunJunHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, bool benefit) => getCharacterSunJunMobileShiRuntime().BeginShiSj(f, benefit);
}
