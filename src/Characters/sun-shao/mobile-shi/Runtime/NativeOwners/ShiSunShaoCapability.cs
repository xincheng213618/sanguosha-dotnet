#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShiSunShaoCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterSunShaoMobileShiRuntime>> getCharacterSunShaoMobileShiRuntime) : IShiSunShaoHost
{
    public SkillProgramStepOutcome ResolveGanjue(ProgramSkillFrame frame) => getCharacterSunShaoMobileShiRuntime().Value.BeginShiSsGanjue(frame);
    public SkillProgramStepOutcome ResolveZhuji(ProgramSkillFrame frame) => getCharacterSunShaoMobileShiRuntime().Value.BeginShiSsZhuji(frame);
}
