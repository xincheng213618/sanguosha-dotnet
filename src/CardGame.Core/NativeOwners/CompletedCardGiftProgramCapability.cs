#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class CompletedCardGiftProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineCompletedCardGiftsRuntime>> getCoreGameEngineCompletedCardGiftsRuntime) : ICompletedCardGiftProgramHost
{
    public SkillProgramStepOutcome OfferCompletedCardGift(ProgramSkillFrame frame) => getCoreGameEngineCompletedCardGiftsRuntime().Value.OfferProgramCompletedCardGift(frame);
}
