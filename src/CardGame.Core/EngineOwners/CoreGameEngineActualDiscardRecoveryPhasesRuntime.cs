#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineActualDiscardRecoveryPhasesRuntime
{
    private readonly ICoreActualDiscardRecoveryPhasesActualDiscardRecoveryPhaseOperations _actualDiscardRecoveryPhaseOperations;
    internal CoreGameEngineActualDiscardRecoveryPhasesRuntime(ICoreActualDiscardRecoveryPhasesActualDiscardRecoveryPhaseOperations actualDiscardRecoveryPhaseOperations)
    {
        this._actualDiscardRecoveryPhaseOperations = actualDiscardRecoveryPhaseOperations;
    }
}
