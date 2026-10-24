namespace CardGame.Core;

public interface IModeOpeningRule
{
    int GetStartingSeat(IReadOnlyList<ModePlayerLife> players, Func<int, int> nextRandom,
        int? initialActorSeat = null);
}
