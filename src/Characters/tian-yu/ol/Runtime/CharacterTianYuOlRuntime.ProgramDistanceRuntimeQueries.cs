#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterTianYuOlRuntime
{
    int global::CardGame.Core.IDistanceRuleRuntimeQueries.RoundNumber => _coreActualRoundProgressRoundNumberAccess._roundNumber;

    int global::CardGame.Core.IDistanceRuleRuntimeQueries.PlayerCount => _coreMatchDefinitionPlayerCountAccess._playerCount;

    int global::CardGame.Core.IDistanceRuleRuntimeQueries.AliveCount => _coreMatchParticipantsPlayersAccess._players.Count(player => player.IsAlive);

    bool global::CardGame.Core.IDistanceRuleRuntimeQueries.HasTriggerOperation(SkillProgramEffectOp op) => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(op);
    bool global::CardGame.Core.IDistanceRuleRuntimeQueries.HasSkill(int seat, string skillId) => _coreNationalSkillsOperations.HasRuntimeSkill(_coreMatchParticipantsPlayersAccess._players[seat], skillId);
    IReadOnlyList<T> global::CardGame.Core.IDistanceRuleRuntimeQueries.History<T>() => _coreProgramEventHistoryOperations.ProgramEventHistory<T>();
    IEnumerable<int> global::CardGame.Core.IDistanceRuleRuntimeQueries.OwnedCardIds(int seat) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[seat])).Select(card => card.Id);
    CardLocation global::CardGame.Core.IDistanceRuleRuntimeQueries.Location(int cardId) => _cardZonesOperations._cardZones.GetLocation(cardId);
    IEnumerable<ProgramDirectedDistanceLedgerEntry> global::CardGame.Core.IDistanceRuleRuntimeQueries.DirectedDistanceLedger()
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
