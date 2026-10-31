#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class CompletedCardGiftProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCompletedCardGiftsEntries> getCoreGameEngineCompletedCardGiftsRuntime) : ICompletedCardGiftProgramHost
{
    public SkillProgramStepOutcome OfferCompletedCardGift(ProgramSkillFrame frame) => getCoreGameEngineCompletedCardGiftsRuntime().OfferProgramCompletedCardGift(frame);
}
