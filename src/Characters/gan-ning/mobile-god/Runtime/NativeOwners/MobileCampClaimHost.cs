#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileCampClaimHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterGanNingMobileGodEntries> getCharacterGanNingMobileGodRuntime) : IMobileCampClaimHost
{
    public SkillProgramStepOutcome Claim(ProgramSkillFrame frame) => getCharacterGanNingMobileGodRuntime().ClaimMobileCampHand(frame);
}
