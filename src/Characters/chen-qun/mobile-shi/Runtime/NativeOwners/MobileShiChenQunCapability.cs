#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileShiChenQunCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterChenQunMobileShiRuntime>> getCharacterChenQunMobileShiRuntime) : IMobileShiChenQunProgramHost, IMobileShiChenQunSelectionQueries
{
    public SkillProgramStepOutcome Claim(ProgramSkillFrame frame) => getCharacterChenQunMobileShiRuntime().Value.ClaimMobileShiChenQunFaenDebt(frame);
    public void Issue(ProgramSkillFrame frame) => getCharacterChenQunMobileShiRuntime().Value.IssueMobileShiChenQunFaenDebt(frame);
    public IReadOnlyList<int> MaximumLossSeats => getCharacterChenQunMobileShiRuntime().Value.MobileShiChenQunMaximumLossSeats();
}
