#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class PhaseGiftBasicProgramCapability(Func<RuntimeEntries.CoreGameEnginePhaseGiftBasicEntries> getCoreGameEnginePhaseGiftBasicRuntime, Func<RuntimeEntries.CoreGameEngineVirtualBasicOffersEntries> getCoreGameEngineVirtualBasicOffersRuntime) : IPhaseGiftBasicProgramHost
{
    public void AccumulatePaidPhaseGift(ProgramSkillFrame frame, string resultBind, int threshold) => getCoreGameEnginePhaseGiftBasicRuntime().AccumulatePaidPhaseGift(frame, resultBind, threshold);
    public SkillProgramStepOutcome OfferVirtualBasicCard(ProgramSkillFrame frame) => getCoreGameEngineVirtualBasicOffersRuntime().OfferVirtualBasicCard(frame);
}
