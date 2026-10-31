#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

internal interface ICoreForeignTurnContestsForeignActualTurnWindowOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ForeignActualTurnWindowRuntime Get();
}

internal interface ICoreForeignTurnContestsForeignTurnContestOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ForeignTurnContestRuntime Get();
}
