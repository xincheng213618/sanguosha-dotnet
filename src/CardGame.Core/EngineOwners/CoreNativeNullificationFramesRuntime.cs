#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreNativeNullificationFramesRuntime
{
    private readonly ICoreNativeNullificationFramesResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreNativeNullificationFramesCoreDamageColorDuelProgramsOperations _coreDamageColorDuelProgramsOperations;
    private readonly ICoreNativeNullificationFramesCoreOriginalUseLightningOperations _coreOriginalUseLightningOperations;
    private readonly ICoreNativeNullificationFramesCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreNativeNullificationFramesCoreNativeSlashTargetDuelOperations _coreNativeSlashTargetDuelOperations;
    internal CoreNativeNullificationFramesRuntime(ICoreNativeNullificationFramesResolutionStackOperations resolutionStackOperations, ICoreNativeNullificationFramesCoreDamageColorDuelProgramsOperations coreDamageColorDuelProgramsOperations, ICoreNativeNullificationFramesCoreOriginalUseLightningOperations coreOriginalUseLightningOperations, ICoreNativeNullificationFramesCoreRuntimeOperations coreRuntimeOperations, ICoreNativeNullificationFramesCoreNativeSlashTargetDuelOperations coreNativeSlashTargetDuelOperations)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDamageColorDuelProgramsOperations = coreDamageColorDuelProgramsOperations;
        this._coreOriginalUseLightningOperations = coreOriginalUseLightningOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreNativeSlashTargetDuelOperations = coreNativeSlashTargetDuelOperations;
    }
}
