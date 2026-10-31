#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShiZangHongCapability(Func<RuntimeEntries.CharacterZangHongMobileShiEntries> getCharacterZangHongMobileShiRuntime) : IShiZangHongHost
{
    public SkillProgramStepOutcome ResolveLiezhi(ProgramSkillFrame frame) => getCharacterZangHongMobileShiRuntime().BeginShiZhLiezhi(frame);
    public SkillProgramStepOutcome ResolveJuegu(ProgramSkillFrame frame) => getCharacterZangHongMobileShiRuntime().BeginShiZhJuegu(frame);
}
