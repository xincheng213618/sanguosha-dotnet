#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineDamageTargetObtainObserverRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageTargetObtainProgramsEntries _coreDamageTargetObtainProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageTargetRegionObtainEntries _coreDamageTargetRegionObtainOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramChildResolutionEntries _coreProgramChildResolutionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    internal CoreGameEngineDamageTargetObtainObserverRuntime(global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageTargetObtainProgramsEntries coreDamageTargetObtainProgramsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageTargetRegionObtainEntries coreDamageTargetRegionObtainOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramChildResolutionEntries coreProgramChildResolutionOperations,global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDamageTargetObtainProgramsOperations = coreDamageTargetObtainProgramsOperations;
        this._coreDamageTargetRegionObtainOperations = coreDamageTargetRegionObtainOperations;
        this._coreProgramChildResolutionOperations = coreProgramChildResolutionOperations;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
    }
}
