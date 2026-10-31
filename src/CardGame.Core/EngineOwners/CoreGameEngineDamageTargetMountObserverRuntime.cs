#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineDamageTargetMountObserverRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageTargetMountProgramsEntries _coreDamageTargetMountProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramChildResolutionEntries _coreProgramChildResolutionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    internal CoreGameEngineDamageTargetMountObserverRuntime(global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageTargetMountProgramsEntries coreDamageTargetMountProgramsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramChildResolutionEntries coreProgramChildResolutionOperations,global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDamageTargetMountProgramsOperations = coreDamageTargetMountProgramsOperations;
        this._coreProgramChildResolutionOperations = coreProgramChildResolutionOperations;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
    }
}
