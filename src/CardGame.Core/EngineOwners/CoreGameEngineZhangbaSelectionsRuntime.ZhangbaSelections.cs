#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineZhangbaSelectionsRuntime
{
    internal ZhangbaSlashPairFacts GetZhangbaSlashPairFacts(CharacterState actor, Card first, Card second)
    {
        if (_coreGameEngineActionQueriesActionQueryAccess._actionQuery is not { } query)
            return CalculateZhangbaSlashPairFacts(actor, first, second);
        var key = (actor.Seat, first, second);
        if (!query.ZhangbaSlashPairs.TryGetValue(key, out var result))
            query.ZhangbaSlashPairs[key] = result = CalculateZhangbaSlashPairFacts(actor, first, second);
        return result;
    }

    private ZhangbaSlashPairFacts CalculateZhangbaSlashPairFacts(CharacterState actor, Card first, Card second)
    {
        Card[] cards = [first, second];
        int[] ids = [first.Id, second.Id];
        var suit = _coreAlternatingSuitAndCompletedTopOperations.PhysicalGroupSuit(actor, cards);
        var color = _coreTurnRedSlashBenefitsOperations.PhysicalGroupColor(actor, cards);
        var rank = _coreFinalTargetSlashOperations.ZhangbaSpecificSlashRank(actor, cards);
        return new(suit, color, rank, _coreNativeTargetLegalityOperations.GetVirtualSlashTargetSeats(actor,
            _coreMatchParticipantsPlayersAccess._players, CardKind.Slash, suit, color, rank, ids));
    }

    internal IReadOnlyList<ActiveSkillCardGroupTarget> GetZhangbaSlashGroupTargets(CharacterState actor)
    {
        var choices = new List<ActiveSkillCardGroupTarget>();
        foreach (var pair in _coreNativeResponseMaterialQueriesOperations.EnumerateZhangbaSlashPairs(actor))
        {
            var ids = Array.AsReadOnly(pair.Select(card => card.Id).ToArray());
            if (_coreProgramParticipantPhaseOperationsOperations.IsTurnPhysicalUseForbidden(actor.Seat, ids))
                continue;
            var facts = GetZhangbaSlashPairFacts(actor, pair[0], pair[1]);
            foreach (var target in facts.LegalTargets)
                choices.Add(new(ids, target));
        }

        return choices.AsReadOnly();
    }

    internal static bool MatchesPublishedCardGroupTarget(LegalAction action, IReadOnlyList<int> cardIds, IReadOnlyList<int> targets) => action.SelectableCardGroupTargets is not { } groups || targets.Count >= action.MinTargetCount && targets.Count <= action.MaxTargetCount && targets.All(target => groups.Any(group => group.TargetSeat == target && group.CardIds.Count == cardIds.Count && group.CardIds.Order().SequenceEqual(cardIds.Order())));
}
