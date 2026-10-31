#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeStoneAxeEntries
{
    internal string DescribeStoneAxeCost(global::CardGame.Core.CharacterState source, global::CardGame.Core.Card card) => _resolve().DescribeStoneAxeCost(source, card);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetStoneAxeDiscardCards(global::CardGame.Core.CharacterState source) => _resolve().GetStoneAxeDiscardCards(source);
    internal void ResolveStoneAxeChoice(global::System.Collections.Generic.IReadOnlyList<int> discardedCardIds) => _resolve().ResolveStoneAxeChoice(discardedCardIds);
    internal bool TryBeginStoneAxeChoice(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().TryBeginStoneAxeChoice(attack);
}
