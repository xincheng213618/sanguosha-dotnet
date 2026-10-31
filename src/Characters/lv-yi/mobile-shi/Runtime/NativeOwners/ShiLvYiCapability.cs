#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShiLvYiCapability(Func<RuntimeEntries.CharacterLvYiMobileShiEntries> getCharacterLvYiMobileShiRuntime) : IShiLvYiHost
{
    public SkillProgramStepOutcome ResolveHuilv(ProgramSkillFrame frame) => getCharacterLvYiMobileShiRuntime().BeginShiLyHuilv(frame);
    public SkillProgramStepOutcome ResolveHuilvSettle(ProgramSkillFrame frame) => getCharacterLvYiMobileShiRuntime().BeginShiLyHuilvSettle(frame);
    public SkillProgramStepOutcome ResolveSongyan(ProgramSkillFrame frame) => getCharacterLvYiMobileShiRuntime().BeginShiLySongyan(frame);
}
