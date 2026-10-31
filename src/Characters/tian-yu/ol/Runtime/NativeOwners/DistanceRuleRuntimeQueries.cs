#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class DistanceRuleRuntimeQueries(global::CardGame.Core.EngineStates.CoreActualRoundProgressState actualRoundProgressState, global::CardGame.Core.EngineStates.CoreMatchDefinitionState matchDefinitionState, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.ContentRegistry> getContentRegistry, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries> getCoreGameEngineProgramEventHistoryRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime, global::System.Func<global::CardGame.Core.CardZoneStore> getCardZoneStore, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries> getCoreGameEngineAlternatingBenefitsRuntime) : IDistanceRuleRuntimeQueries
{
    public int RoundNumber => actualRoundProgressState.ReferenceToRoundNumber;
    public int PlayerCount => matchDefinitionState.ReferenceToPlayerCount;
    public int AliveCount => matchParticipantsState.ReferenceToPlayers.Count(player => player.IsAlive);

    public bool HasTriggerOperation(SkillProgramEffectOp op) => getContentRegistry().ProgramDependencies.HasTriggerOperation(op);
    public bool HasSkill(int seat, string skillId) => getCoreGameEngineNationalSkillsRuntime().HasRuntimeSkill(matchParticipantsState.ReferenceToPlayers[seat], skillId);
    public IReadOnlyList<T> History<T>()
        where T : IGameEvent => getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<T>();
    public IEnumerable<int> OwnedCardIds(int seat) => getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[seat]).Concat(getCorePhysicalOwnedCardQueriesRuntime().GetEquipment(matchParticipantsState.ReferenceToPlayers[seat])).Select(card => card.Id);
    public CardLocation Location(int cardId) => getCardZoneStore().GetLocation(cardId);
    public IEnumerable<ProgramDirectedDistanceLedgerEntry> DirectedDistanceLedger()
    {
        foreach (var fact in getCoreGameEngineAlternatingBenefitsRuntime().CompleteProgramEventHistory())
            switch (fact)
            {
                case ProgramDirectedDistanceGrantedEvent grant:
                    yield return new(grant.OwnerSeat, grant.TargetSeat, grant);
                    break;
                case ProgramDirectedDistanceRevokedEvent revoke:
                    yield return new(revoke.OwnerSeat, revoke.TargetSeat, null);
                    break;
            }
    }
}
