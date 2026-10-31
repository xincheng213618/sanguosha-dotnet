#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineShanheSchoolLateRuntime
{
    private readonly RuntimeEntries.CoreGameEngineShanheS5QuyiEntries _coreShanheS5QuyiOperations;
    private readonly EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess _coreActualRoundProgressRoundNumberAccess;
    internal CoreGameEngineShanheSchoolLateRuntime(RuntimeEntries.CoreGameEngineShanheS5QuyiEntries coreShanheS5QuyiOperations, EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess coreActualRoundProgressRoundNumberAccess)
    {
        this._coreShanheS5QuyiOperations = coreShanheS5QuyiOperations;
        this._coreActualRoundProgressRoundNumberAccess = coreActualRoundProgressRoundNumberAccess;
    }
}
