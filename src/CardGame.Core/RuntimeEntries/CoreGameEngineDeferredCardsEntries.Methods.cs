#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDeferredCardsEntries
{
    internal SkillProgramStepOutcome BeginParticipantTopView(ProgramSkillFrame frame, int seat, int amount) => _resolve().BeginParticipantTopView(frame, seat, amount);
    internal void CleanupLostDeferredPileSources() => _resolve().CleanupLostDeferredPileSources();
    internal void ConsumeSkippedNextTurnDrawBenefits(CharacterState current) => _resolve().ConsumeSkippedNextTurnDrawBenefits(current);
    internal SkillProgramStepOutcome DepositDeferredPile(ProgramSkillFrame frame, string bind) => _resolve().DepositDeferredPile(frame, bind);
    internal void DiscardDeferredDeposit(DeferredPublicPileDeposit deposit) => _resolve().DiscardDeferredDeposit(deposit);
    internal IReadOnlyList<Card> GetDiscardEligibleHand(CharacterState owner) => _resolve().GetDiscardEligibleHand(owner);
    internal CardSnapshot[] GetPrivatelyViewedCards(int viewerSeat) => _resolve().GetPrivatelyViewedCards(viewerSeat);
    internal SkillProgramStepOutcome ObtainDeferredPile(ProgramSkillFrame frame) => _resolve().ObtainDeferredPile(frame);
    internal void ResolveParticipantTopView(PromptChoice selected) => _resolve().ResolveParticipantTopView(selected);
    internal void RewardDeferredProviders(ProgramSkillFrame frame) => _resolve().RewardDeferredProviders(frame);
}
