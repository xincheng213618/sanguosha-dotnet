#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeStoneAxeEntries
{
    internal string DescribeStoneAxeCost(CharacterState source, Card card) => _resolve().DescribeStoneAxeCost(source, card);
    internal IReadOnlyList<Card> GetStoneAxeDiscardCards(CharacterState source) => _resolve().GetStoneAxeDiscardCards(source);
    internal void ResolveStoneAxeChoice(IReadOnlyList<int> discardedCardIds) => _resolve().ResolveStoneAxeChoice(discardedCardIds);
    internal bool TryBeginStoneAxeChoice(NativeOwners.CardAttackHandle attack) => _resolve().TryBeginStoneAxeChoice(attack);
}
