#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeWeaponResponsesEntries
{
    internal void CompleteCixiongDoubleSwords(global::CardGame.Core.NativeOwners.CixiongDoubleSwordsHandle pending, bool activated, bool targetDiscarded, int? discardedCardId, int sourceDrawCount) => _resolve().CompleteCixiongDoubleSwords(pending, activated, targetDiscarded, discardedCardId, sourceDrawCount);
    internal void ResolveCixiongDoubleSwordsChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveCixiongDoubleSwordsChoice(selected);
    internal bool TryBeginCixiongDoubleSwordsChoice(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().TryBeginCixiongDoubleSwordsChoice(attack);
}
