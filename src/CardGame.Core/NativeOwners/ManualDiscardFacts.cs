#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ManualDiscardFacts(EngineStates.CoreMatchOutcomeState matchOutcomeState, Func<RuntimeEntries.CoreGameEngineTurnHandCategoryRestrictionsEntries> getCoreGameEngineTurnHandCategoryRestrictionsRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEnginePlannedPhaseQuantitiesRuntime>> getCoreGameEnginePlannedPhaseQuantitiesRuntime, Func<RuntimeEntries.CoreNativeRuleQueriesEntries> getCoreNativeRuleQueriesRuntime) : IManualDiscardFacts
{
    public Winner Winner => matchOutcomeState.ReferenceToWinner;
    public EngineStatus Status => matchOutcomeState.ReferenceToStatus;

    public int RequiredDiscard(CharacterState owner) => getCoreGameEngineTurnHandCategoryRestrictionsRuntime().RequiredSelfHandLimitDiscard(owner);
    public int QuotaMultiplier(CharacterState owner) => getCoreGameEnginePlannedPhaseQuantitiesRuntime().Value.GetDiscardQuotaMultiplier(owner);
    public IReadOnlyList<Card> DiscardableHand(CharacterState owner) => getCoreGameEngineTurnHandCategoryRestrictionsRuntime().GetSelfDiscardableHand(owner);
    public int HandLimit(CharacterState owner) => getCoreNativeRuleQueriesRuntime().GetHandLimit(owner);
}
