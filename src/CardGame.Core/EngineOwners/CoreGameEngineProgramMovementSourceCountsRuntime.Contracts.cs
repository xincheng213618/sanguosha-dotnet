#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramMovementSourceCountsCoreProgramDiscardOriginsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardLocation? GetProgramDiscardSource(global::CardGame.Core.CardMovementRecord movement);
    global::CardGame.Core.CardMovementSourceCount GetProgramDiscardOriginSourceCount(global::CardGame.Core.CardLocation source, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord> completions);
}
