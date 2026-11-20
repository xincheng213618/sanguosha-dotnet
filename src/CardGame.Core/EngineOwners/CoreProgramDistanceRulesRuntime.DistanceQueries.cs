#nullable enable

namespace CardGame.Core.EngineOwners;

internal sealed partial class CoreProgramDistanceRulesRuntime
{
    int IDistanceRuleRuntimeQueries.RoundNumber => _round._roundNumber;
    int IDistanceRuleRuntimeQueries.PlayerCount => _matchDefinition._playerCount;
    int IDistanceRuleRuntimeQueries.AliveCount => _participants._players.Count(player => player.IsAlive);
    bool IDistanceRuleRuntimeQueries.HasTriggerOperation(SkillProgramEffectOp operation) => _content.HasTriggerOperation(operation);
    bool IDistanceRuleRuntimeQueries.HasSkill(int seat, string skillId) => _nationalSkills.HasRuntimeSkill(_participants._players[seat], skillId);
    IReadOnlyList<T> IDistanceRuleRuntimeQueries.History<T>() => _history.Read<T>();
    CardLocation IDistanceRuleRuntimeQueries.Location(int cardId) => _zones.GetLocation(cardId);

    IEnumerable<int> IDistanceRuleRuntimeQueries.OwnedCardIds(int seat)
    {
        var ownerSeat = _participants._players[seat].Seat;
        return _zones.CardsAt(CardLocation.Hand(ownerSeat))
            .Concat(_zones.CardsAt(CardLocation.Equipment(ownerSeat)))
            .Select(card => card.Id);
    }
}
