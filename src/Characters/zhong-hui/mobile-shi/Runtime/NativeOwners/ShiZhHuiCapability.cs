#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class ShiZhHuiCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhongHuiMobileShiEntries> getCharacterZhongHuiMobileShiRuntime) : IShiZhHuiHost
{
    public SkillProgramStepOutcome ResolveSizi(ProgramSkillFrame frame) => getCharacterZhongHuiMobileShiRuntime().BeginShiZhHui(frame);
    public SkillProgramStepOutcome ResolveSiziEnding(ProgramSkillFrame frame) => getCharacterZhongHuiMobileShiRuntime().BeginShiZhHui(frame);
    public SkillProgramStepOutcome ResolveXiezhi(ProgramSkillFrame frame) => getCharacterZhongHuiMobileShiRuntime().BeginShiZhHui(frame);
    public SkillProgramStepOutcome ResolveYunan(ProgramSkillFrame frame) => getCharacterZhongHuiMobileShiRuntime().BeginShiZhHui(frame);
}
