#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShiPangXiCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterPangXiMobileShiRuntime>> getCharacterPangXiMobileShiRuntime) : IShiPangXiHost
{
    public SkillProgramStepOutcome ResolveXuye(ProgramSkillFrame frame) => getCharacterPangXiMobileShiRuntime().Value.BeginShiPxXuye(frame);
    public SkillProgramStepOutcome ResolveKuangxiang(ProgramSkillFrame frame) => getCharacterPangXiMobileShiRuntime().Value.BeginShiPxKuangxiang(frame);
    public SkillProgramStepOutcome ResolveRider(ProgramSkillFrame frame) => getCharacterPangXiMobileShiRuntime().Value.BeginShiPxRider(frame);
    public SkillProgramStepOutcome ResolveSweep(ProgramSkillFrame frame) => getCharacterPangXiMobileShiRuntime().Value.BeginShiPxSweep(frame);
}
