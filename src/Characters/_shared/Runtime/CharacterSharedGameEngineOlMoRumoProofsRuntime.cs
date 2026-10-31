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
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries _coreProgramEventHistoryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRoundEndingEntries _coreRoundEndingOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess _coreActualRoundProgressRoundNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess _coreActualTurnProgressTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCommandJournalStorageEventsPendingEventsAccess _coreCommandJournalStorageEventsPendingEventsAccess;
    internal CharacterSharedGameEngineOlMoRumoProofsRuntime(global::CardGame.Core.RuntimeEntries.CharacterDiaoChanOlMagicEntries characterDiaoChanOlMagicOperations, global::CardGame.Core.RuntimeEntries.CharacterLuBuOlMagicEntries characterLuBuOlMagicOperations, global::CardGame.Core.RuntimeEntries.CharacterSunQuanOlMagicEntries characterSunQuanOlMagicOperations, global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.EngineOwners.IKernelProgramPipelineHooksAccess programPipelineHooksOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries coreProgramEventHistoryOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRoundEndingEntries coreRoundEndingOperations, global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess coreActualRoundProgressRoundNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess coreActualTurnProgressTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCommandJournalStorageEventsPendingEventsAccess coreCommandJournalStorageEventsPendingEventsAccess)
    {
        this._characterDiaoChanOlMagicOperations = characterDiaoChanOlMagicOperations;
        this._characterLuBuOlMagicOperations = characterLuBuOlMagicOperations;
        this._characterSunQuanOlMagicOperations = characterSunQuanOlMagicOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._programPipelineHooksOperations = programPipelineHooksOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreRoundEndingOperations = coreRoundEndingOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreActualRoundProgressRoundNumberAccess = coreActualRoundProgressRoundNumberAccess;
        this._coreActualTurnProgressTurnNumberAccess = coreActualTurnProgressTurnNumberAccess;
        this._coreCommandJournalStorageEventsPendingEventsAccess = coreCommandJournalStorageEventsPendingEventsAccess;
    }
}
