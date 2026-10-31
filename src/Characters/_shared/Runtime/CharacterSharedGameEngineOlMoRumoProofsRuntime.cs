#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CharacterSharedGameEngineOlMoRumoProofsRuntime
{
    private readonly RuntimeEntries.CharacterDiaoChanOlMagicEntries _characterDiaoChanOlMagicOperations;
    private readonly RuntimeEntries.CharacterLuBuOlMagicEntries _characterLuBuOlMagicOperations;
    private readonly RuntimeEntries.CharacterSunQuanOlMagicEntries _characterSunQuanOlMagicOperations;
    private readonly IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly IKernelProgramPipelineHooksAccess _programPipelineHooksOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramEventHistoryEntries _coreProgramEventHistoryOperations;
    private readonly RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess _coreActualRoundProgressRoundNumberAccess;
    internal CharacterSharedGameEngineOlMoRumoProofsRuntime(RuntimeEntries.CharacterDiaoChanOlMagicEntries characterDiaoChanOlMagicOperations,RuntimeEntries.CharacterLuBuOlMagicEntries characterLuBuOlMagicOperations,RuntimeEntries.CharacterSunQuanOlMagicEntries characterSunQuanOlMagicOperations,IKernelContentRegistryAccess contentRegistryOperations,IKernelProgramPipelineHooksAccess programPipelineHooksOperations,RuntimeEntries.CoreGameEngineProgramEventHistoryEntries coreProgramEventHistoryOperations,RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations,EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess coreActualRoundProgressRoundNumberAccess)
    {
        this._characterDiaoChanOlMagicOperations = characterDiaoChanOlMagicOperations;
        this._characterLuBuOlMagicOperations = characterLuBuOlMagicOperations;
        this._characterSunQuanOlMagicOperations = characterSunQuanOlMagicOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._programPipelineHooksOperations = programPipelineHooksOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreActualRoundProgressRoundNumberAccess = coreActualRoundProgressRoundNumberAccess;
    }
}
