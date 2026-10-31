#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineNativeDrawThresholdRuntime
{
    internal bool TracksNativeDrawThresholds => _nativeDrawThresholdInvoiceOperations.Get().Tracks;

    internal NativeDrawThresholdProgress? MatchingNativeDrawThresholdProgress(CardMovementBatchContext batch, ProgramTriggerCandidate candidate) => _nativeDrawThresholdOperations.Get().MatchingNativeDrawThresholdProgress(batch, candidate);
    internal int[] MatchingNativeDrawThresholdIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate) => _nativeDrawThresholdOperations.Get().MatchingNativeDrawThresholdIndexes(batch, candidate);
}
