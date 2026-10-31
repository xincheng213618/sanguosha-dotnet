#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class ShiZangHongCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZangHongMobileShiEntries> getCharacterZangHongMobileShiRuntime) : IShiZangHongHost
{
    public SkillProgramStepOutcome ResolveLiezhi(ProgramSkillFrame frame) => getCharacterZangHongMobileShiRuntime().BeginShiZhLiezhi(frame);
    public SkillProgramStepOutcome ResolveJuegu(ProgramSkillFrame frame) => getCharacterZangHongMobileShiRuntime().BeginShiZhJuegu(frame);
}
