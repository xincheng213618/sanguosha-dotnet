#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOwnedHandRankSumGiftEntries
{
    internal global::CardGame.Core.ProgramOwnedHandRankSumGiftReceipt? CaptureOwnedHandRankSumGift(global::CardGame.Core.ProgramSkillFrame frame, global::System.Collections.Generic.IReadOnlyList<int> ids, global::CardGame.Core.CardMoveReason reason) => _resolve().CaptureOwnedHandRankSumGift(frame, ids, reason);
    internal void SealOwnedHandRankSumGift(long frameId, long batchId, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord> movements) => _resolve().SealOwnedHandRankSumGift(frameId, batchId, movements);
    internal bool ValidOwnedHandRankSumGift(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramSkillCardSetBinding source, int required) => _resolve().ValidOwnedHandRankSumGift(frame, source, required);
    internal void ValidateOwnedHandRankSumGiftActivation(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ValidateOwnedHandRankSumGiftActivation(frame);
}
