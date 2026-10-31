#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreNativeNullificationFramesRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageColorDuelProgramsEntries _coreDamageColorDuelProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineOriginalUseLightningEntries _coreOriginalUseLightningOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeSlashTargetDuelEntries _coreNativeSlashTargetDuelOperations;
    internal CoreNativeNullificationFramesRuntime(global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageColorDuelProgramsEntries coreDamageColorDuelProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineOriginalUseLightningEntries coreOriginalUseLightningOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations, global::CardGame.Core.RuntimeEntries.CoreNativeSlashTargetDuelEntries coreNativeSlashTargetDuelOperations)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDamageColorDuelProgramsOperations = coreDamageColorDuelProgramsOperations;
        this._coreOriginalUseLightningOperations = coreOriginalUseLightningOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreNativeSlashTargetDuelOperations = coreNativeSlashTargetDuelOperations;
    }
}
