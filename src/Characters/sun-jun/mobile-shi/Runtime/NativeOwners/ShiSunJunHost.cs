#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShiSunJunHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterSunJunMobileShiRuntime>> getCharacterSunJunMobileShiRuntime) : IShiSunJunHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, bool benefit) => getCharacterSunJunMobileShiRuntime().Value.BeginShiSj(f, benefit);
}
