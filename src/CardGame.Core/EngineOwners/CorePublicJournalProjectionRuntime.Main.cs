#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CorePublicJournalProjectionRuntime
{
    internal IReadOnlyList<GameLogEntry> Log => _characterSimaYiMouOperations.MouSmPublicLog();
    internal IReadOnlyList<EventEnvelope> Events => _characterSimaYiMouOperations.MouSmPublicEvents();
}
