#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class PindianSelectionQueries(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNextActualUsePindianAiEntries> getCoreGameEngineNextActualUsePindianAiRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePindianPoliciesEntries> getCoreGameEnginePindianPoliciesRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime) : IPindianSelectionQueries
{
    public bool HasNextActualUseStructure(SkillProgramActivation activation) => getCoreGameEngineNextActualUsePindianAiRuntime().HasNextActualUsePindianStructure(activation);
    public int EffectiveRank(int ownerSeat, int cardId) => getCoreGameEnginePindianPoliciesRuntime().EffectivePindianRank(matchParticipantsState.ReferenceToPlayers[ownerSeat], getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[ownerSeat]).Single(card => card.Id == cardId));
    public int HandKeepValue(int ownerSeat, int cardId) => CardCatalog.Get(getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[ownerSeat]).Single(card => card.Id == cardId).Kind).HandKeepValue;
}
