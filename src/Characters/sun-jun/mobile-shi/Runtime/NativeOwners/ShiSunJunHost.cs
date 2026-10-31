#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class ShiSunJunHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterSunJunMobileShiEntries> getCharacterSunJunMobileShiRuntime) : IShiSunJunHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, bool benefit) => getCharacterSunJunMobileShiRuntime().BeginShiSj(f, benefit);
}
