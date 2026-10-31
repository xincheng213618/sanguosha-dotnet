#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShiLouGuiCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterLouGuiMobileShiRuntime>> getCharacterLouGuiMobileShiRuntime) : IShiLouGuiHost
{
    public SkillProgramStepOutcome ResolveGuanSha(ProgramSkillFrame frame) => getCharacterLouGuiMobileShiRuntime().Value.BeginShiLgGuanSha(frame);
    public SkillProgramStepOutcome ResolveJiYu(ProgramSkillFrame frame) => getCharacterLouGuiMobileShiRuntime().Value.BeginShiLgJiYu(frame);
}
