#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineTurnDiscardSuitHistoryRuntime
{
    private readonly IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries _coreAlternatingBenefitsOperations;
    private readonly IKernelCreateProgramTriggerQueriesOperations _kernelCoreProgramTriggerRuntimeQueriesOperations;
    private readonly IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly ICoreTurnDiscardSuitHistoryProgramTriggerOperations _programTriggerOperations;
    private readonly EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess _coreActualTurnProgressTurnNumberAccess;
    internal CoreGameEngineTurnDiscardSuitHistoryRuntime(IKernelCardZoneStoreAccess cardZonesOperations, IKernelContentRegistryAccess contentRegistryOperations, RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries coreAlternatingBenefitsOperations, IKernelCreateProgramTriggerQueriesOperations kernelCoreProgramTriggerRuntimeQueriesOperations, IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, ICoreTurnDiscardSuitHistoryProgramTriggerOperations programTriggerOperations, EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess coreActualTurnProgressTurnNumberAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._kernelCoreProgramTriggerRuntimeQueriesOperations = kernelCoreProgramTriggerRuntimeQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._programTriggerOperations = programTriggerOperations;
        this._coreActualTurnProgressTurnNumberAccess = coreActualTurnProgressTurnNumberAccess;
    }
}
