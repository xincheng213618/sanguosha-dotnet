#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CharacterSharedGameEngineOlMoRumoProofsRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDiaoChanOlMagicEntries _characterDiaoChanOlMagicOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLuBuOlMagicEntries _characterLuBuOlMagicOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterSunQuanOlMagicEntries _characterSunQuanOlMagicOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelProgramPipelineHooksAccess _programPipelineHooksOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries _coreProgramEventHistoryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess _coreActualRoundProgressRoundNumberAccess;
    internal CharacterSharedGameEngineOlMoRumoProofsRuntime(global::CardGame.Core.RuntimeEntries.CharacterDiaoChanOlMagicEntries characterDiaoChanOlMagicOperations,global::CardGame.Core.RuntimeEntries.CharacterLuBuOlMagicEntries characterLuBuOlMagicOperations,global::CardGame.Core.RuntimeEntries.CharacterSunQuanOlMagicEntries characterSunQuanOlMagicOperations,global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations,global::CardGame.Core.EngineOwners.IKernelProgramPipelineHooksAccess programPipelineHooksOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries coreProgramEventHistoryOperations,global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations,global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess coreActualRoundProgressRoundNumberAccess)
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
