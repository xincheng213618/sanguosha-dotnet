#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDeferredCardsEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginParticipantTopView(global::CardGame.Core.ProgramSkillFrame frame, int seat, int amount) => _resolve().BeginParticipantTopView(frame, seat, amount);
    internal void CleanupLostDeferredPileSources() => _resolve().CleanupLostDeferredPileSources();
    internal void ConsumeSkippedNextTurnDrawBenefits(global::CardGame.Core.CharacterState current) => _resolve().ConsumeSkippedNextTurnDrawBenefits(current);
    internal global::CardGame.Core.SkillProgramStepOutcome DepositDeferredPile(global::CardGame.Core.ProgramSkillFrame frame, string bind) => _resolve().DepositDeferredPile(frame, bind);
    internal void DiscardDeferredDeposit(global::CardGame.Core.DeferredPublicPileDeposit deposit) => _resolve().DiscardDeferredDeposit(deposit);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetDiscardEligibleHand(global::CardGame.Core.CharacterState owner) => _resolve().GetDiscardEligibleHand(owner);
    internal global::CardGame.Core.CardSnapshot[] GetPrivatelyViewedCards(int viewerSeat) => _resolve().GetPrivatelyViewedCards(viewerSeat);
    internal global::CardGame.Core.SkillProgramStepOutcome ObtainDeferredPile(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ObtainDeferredPile(frame);
    internal void ResolveParticipantTopView(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveParticipantTopView(selected);
    internal void RewardDeferredProviders(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().RewardDeferredProviders(frame);
}
