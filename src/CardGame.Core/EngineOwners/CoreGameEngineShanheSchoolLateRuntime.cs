#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineShanheSchoolLateRuntime
{
    private readonly ICoreShanheSchoolLateCoreShanheS5QuyiOperations _coreShanheS5QuyiOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess _coreActualRoundProgressRoundNumberAccess;
    internal CoreGameEngineShanheSchoolLateRuntime(ICoreShanheSchoolLateCoreShanheS5QuyiOperations coreShanheS5QuyiOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess coreActualRoundProgressRoundNumberAccess)
    {
        this._coreShanheS5QuyiOperations = coreShanheS5QuyiOperations;
        this._coreActualRoundProgressRoundNumberAccess = coreActualRoundProgressRoundNumberAccess;
    }
}
