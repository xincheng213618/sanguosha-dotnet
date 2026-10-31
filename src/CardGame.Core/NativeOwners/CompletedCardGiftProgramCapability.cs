#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class CompletedCardGiftProgramCapability(Func<RuntimeEntries.CoreGameEngineCompletedCardGiftsEntries> getCoreGameEngineCompletedCardGiftsRuntime) : ICompletedCardGiftProgramHost
{
    public SkillProgramStepOutcome OfferCompletedCardGift(ProgramSkillFrame frame) => getCoreGameEngineCompletedCardGiftsRuntime().OfferProgramCompletedCardGift(frame);
}
