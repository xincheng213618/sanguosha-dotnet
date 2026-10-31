#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileCampClaimHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterGanNingMobileGodRuntime>> getCharacterGanNingMobileGodRuntime) : IMobileCampClaimHost
{
    public SkillProgramStepOutcome Claim(ProgramSkillFrame frame) => getCharacterGanNingMobileGodRuntime().Value.ClaimMobileCampHand(frame);
}
