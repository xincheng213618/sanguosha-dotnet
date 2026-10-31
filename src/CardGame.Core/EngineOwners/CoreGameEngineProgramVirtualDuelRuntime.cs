#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramVirtualDuelRuntime
{
    private readonly RuntimeEntries.CharacterLuZhiMouOlEntries _characterLuZhiMouOlOperations;
    private readonly RuntimeEntries.CharacterYangYiOlEntries _characterYangYiOlOperations;
    private readonly IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly IKernelProgramPipelineHooksAccess _programPipelineHooksOperations;
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly RuntimeEntries.CoreActiveNativeAttackQueriesEntries _coreActiveNativeAttackQueriesOperations;
    private readonly RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries _coreAlternatingBenefitsOperations;
    private readonly RuntimeEntries.CoreGameEngineCardAttackStateEntries _coreCardAttackStateOperations;
    private readonly RuntimeEntries.CoreGameEngineCurrentRoundActualUsesEntries _coreCurrentRoundActualUsesOperations;
    private readonly RuntimeEntries.CoreGameEngineCurrentTurnUseKindsEntries _coreCurrentTurnUseKindsOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly RuntimeEntries.CoreGameEngineWholeActualTurnUsesEntries _coreWholeActualTurnUsesOperations;
    private readonly RuntimeEntries.CoreNativeCardClassificationsEntries _coreNativeCardClassificationsOperations;
    private readonly RuntimeEntries.CoreNativeCardContinuationCursorsEntries _coreNativeCardContinuationCursorsOperations;
    private readonly RuntimeEntries.CoreNativeDuelEntries _coreNativeDuelOperations;
    private readonly RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly IKernelCreateCardAttackHandleAndCreateDuelHandleOperations _kernelCoreMainOperations;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineProgramVirtualDuelRuntime(RuntimeEntries.CharacterLuZhiMouOlEntries characterLuZhiMouOlOperations, RuntimeEntries.CharacterYangYiOlEntries characterYangYiOlOperations, IKernelContentRegistryAccess contentRegistryOperations, IKernelProgramPipelineHooksAccess programPipelineHooksOperations, IKernelFrameStoreAccess resolutionStackOperations, RuntimeEntries.CoreActiveNativeAttackQueriesEntries coreActiveNativeAttackQueriesOperations, RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries coreAlternatingBenefitsOperations, RuntimeEntries.CoreGameEngineCardAttackStateEntries coreCardAttackStateOperations, RuntimeEntries.CoreGameEngineCurrentRoundActualUsesEntries coreCurrentRoundActualUsesOperations, RuntimeEntries.CoreGameEngineCurrentTurnUseKindsEntries coreCurrentTurnUseKindsOperations, RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, RuntimeEntries.CoreGameEngineWholeActualTurnUsesEntries coreWholeActualTurnUsesOperations, RuntimeEntries.CoreNativeCardClassificationsEntries coreNativeCardClassificationsOperations, RuntimeEntries.CoreNativeCardContinuationCursorsEntries coreNativeCardContinuationCursorsOperations, RuntimeEntries.CoreNativeDuelEntries coreNativeDuelOperations, RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations, IKernelCreateCardAttackHandleAndCreateDuelHandleOperations kernelCoreMainOperations, EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterLuZhiMouOlOperations = characterLuZhiMouOlOperations;
        this._characterYangYiOlOperations = characterYangYiOlOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._programPipelineHooksOperations = programPipelineHooksOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreActiveNativeAttackQueriesOperations = coreActiveNativeAttackQueriesOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreCardAttackStateOperations = coreCardAttackStateOperations;
        this._coreCurrentRoundActualUsesOperations = coreCurrentRoundActualUsesOperations;
        this._coreCurrentTurnUseKindsOperations = coreCurrentTurnUseKindsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreWholeActualTurnUsesOperations = coreWholeActualTurnUsesOperations;
        this._coreNativeCardClassificationsOperations = coreNativeCardClassificationsOperations;
        this._coreNativeCardContinuationCursorsOperations = coreNativeCardContinuationCursorsOperations;
        this._coreNativeDuelOperations = coreNativeDuelOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreMainOperations = kernelCoreMainOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
