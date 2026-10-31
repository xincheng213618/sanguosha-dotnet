#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterTianYuOlRuntime
{
    int IDistanceRuleRuntimeQueries.RoundNumber => _coreActualRoundProgressRoundNumberAccess._roundNumber;

    int IDistanceRuleRuntimeQueries.PlayerCount => _coreMatchDefinitionPlayerCountAccess._playerCount;

    int IDistanceRuleRuntimeQueries.AliveCount => _coreMatchParticipantsPlayersAccess._players.Count(player => player.IsAlive);

    bool IDistanceRuleRuntimeQueries.HasTriggerOperation(SkillProgramEffectOp op) => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(op);
    bool IDistanceRuleRuntimeQueries.HasSkill(int seat, string skillId) => _coreNationalSkillsOperations.HasRuntimeSkill(_coreMatchParticipantsPlayersAccess._players[seat], skillId);
    IReadOnlyList<T> IDistanceRuleRuntimeQueries.History<T>() => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<T>();
    IEnumerable<int> IDistanceRuleRuntimeQueries.OwnedCardIds(int seat) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[seat])).Select(card => card.Id);
    CardLocation IDistanceRuleRuntimeQueries.Location(int cardId) => _cardZonesOperations._cardZones.GetLocation(cardId);
    IEnumerable<ProgramDirectedDistanceLedgerEntry> IDistanceRuleRuntimeQueries.DirectedDistanceLedger()
    {
        foreach (var fact in _coreAlternatingBenefitsOperations.CompleteProgramEventHistory())
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
