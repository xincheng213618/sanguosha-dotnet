#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class PhaseGiftBasicProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePhaseGiftBasicEntries> getCoreGameEnginePhaseGiftBasicRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineVirtualBasicOffersEntries> getCoreGameEngineVirtualBasicOffersRuntime) : IPhaseGiftBasicProgramHost
{
    public void AccumulatePaidPhaseGift(ProgramSkillFrame frame, string resultBind, int threshold) => getCoreGameEnginePhaseGiftBasicRuntime().AccumulatePaidPhaseGift(frame, resultBind, threshold);
    public SkillProgramStepOutcome OfferVirtualBasicCard(ProgramSkillFrame frame) => getCoreGameEngineVirtualBasicOffersRuntime().OfferVirtualBasicCard(frame);
}
