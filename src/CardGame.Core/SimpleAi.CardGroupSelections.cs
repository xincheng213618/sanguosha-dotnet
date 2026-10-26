namespace CardGame.Core;

public sealed partial class SimpleAiBrain
{
    private static IReadOnlyList<int> ChoosePublishedCardGroup(GameSnapshot view, LegalAction action,
        IReadOnlyList<CardSnapshot> cards, Func<CardSnapshot, double> keepValue)
    {
        var byId = cards.ToDictionary(card => card.Id);
        var targets = view.Players.Where(player => player.IsAlive && action.SelectableTargetSeats.Contains(player.Seat))
            .Select(player => player.Seat).ToHashSet();
        return action.SelectableCardGroupTargets!
            .Where(group => targets.Contains(group.TargetSeat) &&
                group.CardIds.Count >= action.MinCardCount && group.CardIds.Count <= action.MaxCardCount &&
                group.CardIds.Distinct().Count() == group.CardIds.Count && group.CardIds.All(byId.ContainsKey))
            .OrderBy(group => group.CardIds.Sum(id => keepValue(byId[id])))
            .ThenBy(group => group.CardIds.Min())
            .ThenBy(group => string.Join(',', group.CardIds.Order()), StringComparer.Ordinal)
            .Select(group => (IReadOnlyList<int>)group.CardIds.ToArray()).FirstOrDefault()
            ?? throw new InvalidOperationException("The active skill has no complete published material group.");
    }
}
