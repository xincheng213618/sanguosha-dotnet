namespace CardGame.Core;

public sealed partial class GameEngine
{
    private IReadOnlyList<ActiveSkillCardGroupTarget> GetZhangbaSlashGroupTargets(CharacterState actor)
    {
        var choices = new List<ActiveSkillCardGroupTarget>();
        foreach (var pair in GetZhangbaSlashPairs(actor))
        {
            var ids = Array.AsReadOnly(pair.Select(card => card.Id).ToArray());
            if (IsTurnPhysicalUseForbidden(actor.Seat, ids)) continue;
            foreach (var target in _players)
                if (CanUseVirtualSlashTarget(actor, target, CardKind.Slash, PhysicalGroupSuit(actor, pair),
                    PhysicalGroupColor(actor, pair), ZhangbaSpecificSlashRank(actor, pair), ids))
                    choices.Add(new(ids, target.Seat));
        }
        return choices.AsReadOnly();
    }

    private static bool MatchesPublishedCardGroupTarget(LegalAction action, IReadOnlyList<int> cardIds,
        IReadOnlyList<int> targets) => action.SelectableCardGroupTargets is not { } groups ||
        targets.Count >= action.MinTargetCount && targets.Count <= action.MaxTargetCount &&
        targets.All(target => groups.Any(group => group.TargetSeat == target && group.CardIds.Count == cardIds.Count &&
            group.CardIds.Order().SequenceEqual(cardIds.Order())));
}
