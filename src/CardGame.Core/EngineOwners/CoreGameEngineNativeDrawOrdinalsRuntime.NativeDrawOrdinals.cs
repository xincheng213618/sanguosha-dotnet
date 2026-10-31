#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineNativeDrawOrdinalsRuntime
{
    internal int[] MatchingNativeDrawOrdinalIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate) => _nativeDrawOrdinalTriggerOperations.Get().MatchingIndexes(batch, candidate);
}
