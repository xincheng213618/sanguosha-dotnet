#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class ShiLouGuiCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLouGuiMobileShiEntries> getCharacterLouGuiMobileShiRuntime) : IShiLouGuiHost
{
    public SkillProgramStepOutcome ResolveGuanSha(ProgramSkillFrame frame) => getCharacterLouGuiMobileShiRuntime().BeginShiLgGuanSha(frame);
    public SkillProgramStepOutcome ResolveJiYu(ProgramSkillFrame frame) => getCharacterLouGuiMobileShiRuntime().BeginShiLgJiYu(frame);
}
