#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreHandCardGuidanceProjectionCoreHandGuidanceOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.HandCardGuidance> BuildHumanHandGuidance(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> legalActions);
}

internal interface ICoreHandCardGuidanceProjectionCoreNativeLegalActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> GetHumanLegalActions();
}
