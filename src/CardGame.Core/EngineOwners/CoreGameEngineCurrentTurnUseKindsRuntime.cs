#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineCurrentTurnUseKindsRuntime
{
    private readonly RuntimeEntries.CharacterCaiYongLeEntries _characterCaiYongLeOperations;
    private readonly RuntimeEntries.CharacterCaoZhiLeEntries _characterCaoZhiLeOperations;
    private readonly RuntimeEntries.CharacterLuSuMobileShiEntries _characterLuSuMobileShiOperations;
    private readonly IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly RuntimeEntries.CoreGameEngineActualTurnLegacyCompletionEntries _coreActualTurnLegacyCompletionOperations;
    private readonly RuntimeEntries.CoreGameEngineCurrentRoundActualUsesEntries _coreCurrentRoundActualUsesOperations;
    private readonly RuntimeEntries.CoreGameEngineDelayedNullificationEntries _coreDelayedNullificationOperations;
    private readonly RuntimeEntries.CoreGameEnginePlayCardHistoryEntries _corePlayCardHistoryOperations;
    private readonly RuntimeEntries.CoreGameEngineWholeActualTurnUsesEntries _coreWholeActualTurnUsesOperations;
    private readonly RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess;
    internal CoreGameEngineCurrentTurnUseKindsRuntime(RuntimeEntries.CharacterCaiYongLeEntries characterCaiYongLeOperations, RuntimeEntries.CharacterCaoZhiLeEntries characterCaoZhiLeOperations, RuntimeEntries.CharacterLuSuMobileShiEntries characterLuSuMobileShiOperations, IKernelContentRegistryAccess contentRegistryOperations, IKernelFrameStoreAccess resolutionStackOperations, RuntimeEntries.CoreGameEngineActualTurnLegacyCompletionEntries coreActualTurnLegacyCompletionOperations, RuntimeEntries.CoreGameEngineCurrentRoundActualUsesEntries coreCurrentRoundActualUsesOperations, RuntimeEntries.CoreGameEngineDelayedNullificationEntries coreDelayedNullificationOperations, RuntimeEntries.CoreGameEnginePlayCardHistoryEntries corePlayCardHistoryOperations, RuntimeEntries.CoreGameEngineWholeActualTurnUsesEntries coreWholeActualTurnUsesOperations, RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations, IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess)
    {
        this._characterCaiYongLeOperations = characterCaiYongLeOperations;
        this._characterCaoZhiLeOperations = characterCaoZhiLeOperations;
        this._characterLuSuMobileShiOperations = characterLuSuMobileShiOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreActualTurnLegacyCompletionOperations = coreActualTurnLegacyCompletionOperations;
        this._coreCurrentRoundActualUsesOperations = coreCurrentRoundActualUsesOperations;
        this._coreDelayedNullificationOperations = coreDelayedNullificationOperations;
        this._corePlayCardHistoryOperations = corePlayCardHistoryOperations;
        this._coreWholeActualTurnUsesOperations = coreWholeActualTurnUsesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess = coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess;
    }
}
