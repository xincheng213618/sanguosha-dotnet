#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreSnapshotValueProjectionEntries
{
    internal global::CardGame.Core.CardSnapshot ToJudgmentSnapshot(global::CardGame.Core.Card card) => _resolve().ToJudgmentSnapshot(card);
    internal global::CardGame.Core.CardSnapshot ToSnapshot(global::CardGame.Core.Card card) => _resolve().ToSnapshot(card);
}
