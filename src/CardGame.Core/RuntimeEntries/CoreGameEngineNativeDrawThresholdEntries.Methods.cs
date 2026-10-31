#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNativeDrawThresholdEntries
{
    internal bool HasExactNativeDrawThresholdSelection(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().HasExactNativeDrawThresholdSelection(frame);
    internal int[] MatchingNativeDrawThresholdIndexes(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().MatchingNativeDrawThresholdIndexes(batch, candidate);
    internal global::CardGame.Core.NativeDrawThresholdProgress? MatchingNativeDrawThresholdProgress(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().MatchingNativeDrawThresholdProgress(batch, candidate);
    internal bool TracksNativeDrawThresholds { get => _resolve().TracksNativeDrawThresholds; }
}
