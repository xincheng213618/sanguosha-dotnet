#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShiLvYiCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterLvYiMobileShiRuntime>> getCharacterLvYiMobileShiRuntime) : IShiLvYiHost
{
    public SkillProgramStepOutcome ResolveHuilv(ProgramSkillFrame frame) => getCharacterLvYiMobileShiRuntime().Value.BeginShiLyHuilv(frame);
    public SkillProgramStepOutcome ResolveHuilvSettle(ProgramSkillFrame frame) => getCharacterLvYiMobileShiRuntime().Value.BeginShiLyHuilvSettle(frame);
    public SkillProgramStepOutcome ResolveSongyan(ProgramSkillFrame frame) => getCharacterLvYiMobileShiRuntime().Value.BeginShiLySongyan(frame);
}
