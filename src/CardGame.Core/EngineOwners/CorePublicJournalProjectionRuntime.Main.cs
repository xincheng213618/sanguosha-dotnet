#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CorePublicJournalProjectionRuntime
{
    internal IReadOnlyList<GameLogEntry> Log => _characterSimaYiMouOperations.MouSmPublicLog();
    internal IReadOnlyList<EventEnvelope> Events => _characterSimaYiMouOperations.MouSmPublicEvents();
}
