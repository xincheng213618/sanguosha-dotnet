#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineProgramTurnQueriesRuntime : global::CardGame.Core.IProgramTurnQueries
{
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess;
    internal CoreGameEngineProgramTurnQueriesRuntime(global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess)
    {
        this._coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess = coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess;
    }
}
