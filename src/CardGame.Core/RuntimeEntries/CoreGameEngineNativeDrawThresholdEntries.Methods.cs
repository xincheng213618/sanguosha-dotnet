#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNativeDrawThresholdEntries
{
    internal int[] MatchingNativeDrawThresholdIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate) => _resolve().MatchingNativeDrawThresholdIndexes(batch, candidate);
    internal NativeDrawThresholdProgress? MatchingNativeDrawThresholdProgress(CardMovementBatchContext batch, ProgramTriggerCandidate candidate) => _resolve().MatchingNativeDrawThresholdProgress(batch, candidate);
    internal bool TracksNativeDrawThresholds { get => _resolve().TracksNativeDrawThresholds; }
}
