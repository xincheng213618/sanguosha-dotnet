#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class PindianSelectionQueries(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineNextActualUsePindianAiRuntime>> getCoreGameEngineNextActualUsePindianAiRuntime, Func<RuntimeEntries.CoreGameEnginePindianPoliciesEntries> getCoreGameEnginePindianPoliciesRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime) : IPindianSelectionQueries
{
    public bool HasNextActualUseStructure(SkillProgramActivation activation) => getCoreGameEngineNextActualUsePindianAiRuntime().Value.HasNextActualUsePindianStructure(activation);
    public int EffectiveRank(int ownerSeat, int cardId) => getCoreGameEnginePindianPoliciesRuntime().EffectivePindianRank(matchParticipantsState.ReferenceToPlayers[ownerSeat], getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[ownerSeat]).Single(card => card.Id == cardId));
    public int HandKeepValue(int ownerSeat, int cardId) => CardCatalog.Get(getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[ownerSeat]).Single(card => card.Id == cardId).Kind).HandKeepValue;
}
