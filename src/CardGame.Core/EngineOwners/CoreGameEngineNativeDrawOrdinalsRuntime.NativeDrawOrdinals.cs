#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineNativeDrawOrdinalsRuntime
{
    internal int[] MatchingNativeDrawOrdinalIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate) => _nativeDrawOrdinalTriggerOperations.Get().MatchingIndexes(batch, candidate);
}
