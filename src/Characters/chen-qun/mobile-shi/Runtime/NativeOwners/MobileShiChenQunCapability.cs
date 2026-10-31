#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileShiChenQunCapability(Func<RuntimeEntries.CharacterChenQunMobileShiEntries> getCharacterChenQunMobileShiRuntime) : IMobileShiChenQunProgramHost, IMobileShiChenQunSelectionQueries
{
    public SkillProgramStepOutcome Claim(ProgramSkillFrame frame) => getCharacterChenQunMobileShiRuntime().ClaimMobileShiChenQunFaenDebt(frame);
    public void Issue(ProgramSkillFrame frame) => getCharacterChenQunMobileShiRuntime().IssueMobileShiChenQunFaenDebt(frame);
    public IReadOnlyList<int> MaximumLossSeats => getCharacterChenQunMobileShiRuntime().MobileShiChenQunMaximumLossSeats();
}
