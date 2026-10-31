namespace CardGame.Core;

public sealed partial class GameEngine
{
internal sealed record YanjiaoCandidate(int Id, int Rank, CardLocation Location,
        string DisplayName, string RankText);
}
