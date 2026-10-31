#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOwnedHandRankSumGiftEntries
{
    internal ProgramOwnedHandRankSumGiftReceipt? CaptureOwnedHandRankSumGift(ProgramSkillFrame frame, IReadOnlyList<int> ids, CardMoveReason reason) => _resolve().CaptureOwnedHandRankSumGift(frame, ids, reason);
    internal void SealOwnedHandRankSumGift(long frameId, long batchId, IReadOnlyList<CardMovementRecord> movements) => _resolve().SealOwnedHandRankSumGift(frameId, batchId, movements);
    internal bool ValidOwnedHandRankSumGift(ProgramSkillFrame frame, ProgramSkillCardSetBinding source, int required) => _resolve().ValidOwnedHandRankSumGift(frame, source, required);
    internal void ValidateOwnedHandRankSumGiftActivation(ProgramSkillFrame frame) => _resolve().ValidateOwnedHandRankSumGiftActivation(frame);
}
