#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineNativeDrawThresholdRuntime
{
    internal bool TracksNativeDrawThresholds => _nativeDrawThresholdInvoiceOperations.Get().Tracks;

    internal NativeDrawThresholdProgress? MatchingNativeDrawThresholdProgress(CardMovementBatchContext batch, ProgramTriggerCandidate candidate) => _nativeDrawThresholdOperations.Get().MatchingNativeDrawThresholdProgress(batch, candidate);
    internal int[] MatchingNativeDrawThresholdIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate) => _nativeDrawThresholdOperations.Get().MatchingNativeDrawThresholdIndexes(batch, candidate);
    internal bool HasExactNativeDrawThresholdSelection(ProgramSkillFrame frame) => _nativeDrawThresholdOperations.Get().HasExactNativeDrawThresholdSelection(frame);
}
