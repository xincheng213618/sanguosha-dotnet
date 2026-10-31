#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProvenanceClaimObserversRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDiscardedProvenanceEntries _coreDiscardedProvenanceOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramChildResolutionEntries _coreProgramChildResolutionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreParticipantHandProgramRescueRidesEntries _coreParticipantHandProgramRescueRidesOperations;
    internal CoreGameEngineProvenanceClaimObserversRuntime(global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineDiscardedProvenanceEntries coreDiscardedProvenanceOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramChildResolutionEntries coreProgramChildResolutionOperations,global::CardGame.Core.RuntimeEntries.CoreParticipantHandProgramRescueRidesEntries coreParticipantHandProgramRescueRidesOperations)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDiscardedProvenanceOperations = coreDiscardedProvenanceOperations;
        this._coreProgramChildResolutionOperations = coreProgramChildResolutionOperations;
        this._coreParticipantHandProgramRescueRidesOperations = coreParticipantHandProgramRescueRidesOperations;
    }
}
