#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShiHuangQuanCapability(Func<RuntimeEntries.CharacterHuangQuanMobileShiEntries> getCharacterHuangQuanMobileShiRuntime) : IShiHuangQuanHost
{
    public SkillProgramStepOutcome ResolveCuixian(ProgramSkillFrame frame) => getCharacterHuangQuanMobileShiRuntime().BeginShiQuanCuixian(frame);
    public SkillProgramStepOutcome ResolveGushi(ProgramSkillFrame frame) => getCharacterHuangQuanMobileShiRuntime().BeginShiQuanGushi(frame);
}
