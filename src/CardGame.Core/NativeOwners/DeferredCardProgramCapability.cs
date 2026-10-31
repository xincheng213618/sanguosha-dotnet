#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class DeferredCardProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineDeferredCardsEntries> getCoreGameEngineDeferredCardsRuntime) : IDeferredCardProgramHost
{
    public SkillProgramStepOutcome ViewTopCardsAndObtainMatchingCards(ProgramSkillFrame frame, int seat, int amount, IReadOnlyList<SkillProgramCardCategory> categories) => getCoreGameEngineDeferredCardsRuntime().BeginParticipantTopView(frame, seat, amount);
    public SkillProgramStepOutcome DepositBoundCardsUntilNextTurn(ProgramSkillFrame frame, string bind) => getCoreGameEngineDeferredCardsRuntime().DepositDeferredPile(frame, bind);
    public SkillProgramStepOutcome ObtainDeferredPile(ProgramSkillFrame frame) => getCoreGameEngineDeferredCardsRuntime().ObtainDeferredPile(frame);
    public void RewardDeferredProviders(ProgramSkillFrame frame) => getCoreGameEngineDeferredCardsRuntime().RewardDeferredProviders(frame);
}
