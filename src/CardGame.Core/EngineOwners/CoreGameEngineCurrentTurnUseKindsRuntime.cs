#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineCurrentTurnUseKindsRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterCaiYongLeEntries _characterCaiYongLeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterCaoZhiLeEntries _characterCaoZhiLeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLuSuMobileShiEntries _characterLuSuMobileShiOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineActualTurnLegacyCompletionEntries _coreActualTurnLegacyCompletionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentRoundActualUsesEntries _coreCurrentRoundActualUsesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDelayedNullificationEntries _coreDelayedNullificationOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePlayCardHistoryEntries _corePlayCardHistoryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineWholeActualTurnUsesEntries _coreWholeActualTurnUsesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess;
    internal CoreGameEngineCurrentTurnUseKindsRuntime(global::CardGame.Core.RuntimeEntries.CharacterCaiYongLeEntries characterCaiYongLeOperations, global::CardGame.Core.RuntimeEntries.CharacterCaoZhiLeEntries characterCaoZhiLeOperations, global::CardGame.Core.RuntimeEntries.CharacterLuSuMobileShiEntries characterLuSuMobileShiOperations, global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineActualTurnLegacyCompletionEntries coreActualTurnLegacyCompletionOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentRoundActualUsesEntries coreCurrentRoundActualUsesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDelayedNullificationEntries coreDelayedNullificationOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePlayCardHistoryEntries corePlayCardHistoryOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineWholeActualTurnUsesEntries coreWholeActualTurnUsesOperations, global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess)
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
