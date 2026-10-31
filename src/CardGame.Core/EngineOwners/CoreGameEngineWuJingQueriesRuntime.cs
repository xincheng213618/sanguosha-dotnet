#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineWuJingQueriesRuntime
{
    private readonly RuntimeEntries.CharacterWuJingOlEntries _characterWuJingOlOperations;
    private readonly RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries _coreAdvancedLifecycleOperations;
    private readonly RuntimeEntries.CoreGameEngineCuiYanNativeQueriesEntries _coreCuiYanNativeQueriesOperations;
    private readonly RuntimeEntries.CoreGameEnginePlayCardHistoryEntries _corePlayCardHistoryOperations;
    private readonly RuntimeEntries.CoreGameEngineTurnHandCategoryRestrictionsEntries _coreTurnHandCategoryRestrictionsOperations;
    private readonly RuntimeEntries.CoreProgramParticipantPhaseOperationsEntries _coreProgramParticipantPhaseOperationsOperations;
    internal CoreGameEngineWuJingQueriesRuntime(RuntimeEntries.CharacterWuJingOlEntries characterWuJingOlOperations,RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries coreAdvancedLifecycleOperations,RuntimeEntries.CoreGameEngineCuiYanNativeQueriesEntries coreCuiYanNativeQueriesOperations,RuntimeEntries.CoreGameEnginePlayCardHistoryEntries corePlayCardHistoryOperations,RuntimeEntries.CoreGameEngineTurnHandCategoryRestrictionsEntries coreTurnHandCategoryRestrictionsOperations,RuntimeEntries.CoreProgramParticipantPhaseOperationsEntries coreProgramParticipantPhaseOperationsOperations)
    {
        this._characterWuJingOlOperations = characterWuJingOlOperations;
        this._coreAdvancedLifecycleOperations = coreAdvancedLifecycleOperations;
        this._coreCuiYanNativeQueriesOperations = coreCuiYanNativeQueriesOperations;
        this._corePlayCardHistoryOperations = corePlayCardHistoryOperations;
        this._coreTurnHandCategoryRestrictionsOperations = coreTurnHandCategoryRestrictionsOperations;
        this._coreProgramParticipantPhaseOperationsOperations = coreProgramParticipantPhaseOperationsOperations;
    }
}
