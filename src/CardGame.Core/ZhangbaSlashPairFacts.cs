namespace CardGame.Core;

internal sealed class ZhangbaSlashPairFacts(Suit? suit, bool? color, int? rank, IEnumerable<int> legalTargets)
{
    internal Suit? Suit { get; } = suit;
    internal bool? Color { get; } = color;
    internal int? Rank { get; } = rank;
    internal IReadOnlyList<int> LegalTargets { get; } = Array.AsReadOnly(legalTargets.ToArray());
}
