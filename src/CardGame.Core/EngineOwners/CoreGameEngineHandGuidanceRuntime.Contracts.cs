#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

internal interface ICoreHandGuidanceCoreNativeLegalActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> GetHumanLegalActions();
}

internal interface ICoreHandGuidanceCoreCommandSessionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long Revision { get; }
}
