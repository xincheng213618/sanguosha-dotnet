#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineDrawPhaseAttemptsRuntime
{
    private readonly ICoreDrawPhaseAttemptsOwnedDrawPhaseOperations _ownedDrawPhaseOperations;
    internal CoreGameEngineDrawPhaseAttemptsRuntime(ICoreDrawPhaseAttemptsOwnedDrawPhaseOperations ownedDrawPhaseOperations)
    {
        this._ownedDrawPhaseOperations = ownedDrawPhaseOperations;
    }
}
