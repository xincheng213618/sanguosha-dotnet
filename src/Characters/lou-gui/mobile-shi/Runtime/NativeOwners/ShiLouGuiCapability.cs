#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShiLouGuiCapability(Func<RuntimeEntries.CharacterLouGuiMobileShiEntries> getCharacterLouGuiMobileShiRuntime) : IShiLouGuiHost
{
    public SkillProgramStepOutcome ResolveGuanSha(ProgramSkillFrame frame) => getCharacterLouGuiMobileShiRuntime().BeginShiLgGuanSha(frame);
    public SkillProgramStepOutcome ResolveJiYu(ProgramSkillFrame frame) => getCharacterLouGuiMobileShiRuntime().BeginShiLgJiYu(frame);
}
