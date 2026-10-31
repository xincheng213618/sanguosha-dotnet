#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNativeDrawOrdinalsEntries
{
    internal int[] MatchingNativeDrawOrdinalIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate) => _resolve().MatchingNativeDrawOrdinalIndexes(batch, candidate);
}
