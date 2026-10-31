#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineShanheSchoolLateRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheS5QuyiEntries _coreShanheS5QuyiOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess _coreActualRoundProgressRoundNumberAccess;
    internal CoreGameEngineShanheSchoolLateRuntime(global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheS5QuyiEntries coreShanheS5QuyiOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess coreActualRoundProgressRoundNumberAccess)
    {
        this._coreShanheS5QuyiOperations = coreShanheS5QuyiOperations;
        this._coreActualRoundProgressRoundNumberAccess = coreActualRoundProgressRoundNumberAccess;
    }
}
