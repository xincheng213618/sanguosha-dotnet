#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ManualDiscardFacts(global::CardGame.Core.EngineStates.CoreMatchOutcomeState matchOutcomeState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineTurnHandCategoryRestrictionsEntries> getCoreGameEngineTurnHandCategoryRestrictionsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePlannedPhaseQuantitiesEntries> getCoreGameEnginePlannedPhaseQuantitiesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreNativeRuleQueriesEntries> getCoreNativeRuleQueriesRuntime) : IManualDiscardFacts
{
    public Winner Winner => matchOutcomeState.ReferenceToWinner;
    public EngineStatus Status => matchOutcomeState.ReferenceToStatus;

    public int RequiredDiscard(CharacterState owner) => getCoreGameEngineTurnHandCategoryRestrictionsRuntime().RequiredSelfHandLimitDiscard(owner);
    public int QuotaMultiplier(CharacterState owner) => getCoreGameEnginePlannedPhaseQuantitiesRuntime().GetDiscardQuotaMultiplier(owner);
    public IReadOnlyList<Card> DiscardableHand(CharacterState owner) => getCoreGameEngineTurnHandCategoryRestrictionsRuntime().GetSelfDiscardableHand(owner);
    public int HandLimit(CharacterState owner) => getCoreNativeRuleQueriesRuntime().GetHandLimit(owner);
}
