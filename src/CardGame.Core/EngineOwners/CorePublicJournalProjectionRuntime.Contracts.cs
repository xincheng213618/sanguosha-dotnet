#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICorePublicJournalProjectionCharacterSimaYiMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GameLogEntry> MouSmPublicLog();
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.EventEnvelope> MouSmPublicEvents();
}
