#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileCampClaimHost(Func<RuntimeEntries.CharacterGanNingMobileGodEntries> getCharacterGanNingMobileGodRuntime) : IMobileCampClaimHost
{
    public SkillProgramStepOutcome Claim(ProgramSkillFrame frame) => getCharacterGanNingMobileGodRuntime().ClaimMobileCampHand(frame);
}
