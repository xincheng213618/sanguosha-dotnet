#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreSnapshotValueProjectionEntries
{
    internal CardSnapshot ToJudgmentSnapshot(Card card) => _resolve().ToJudgmentSnapshot(card);
    internal CardSnapshot ToSnapshot(Card card) => _resolve().ToSnapshot(card);
}
