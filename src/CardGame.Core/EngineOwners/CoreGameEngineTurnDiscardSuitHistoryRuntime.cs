#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineTurnDiscardSuitHistoryRuntime
{
    private readonly ICoreTurnDiscardSuitHistoryCardZonesOperations _cardZonesOperations;
    private readonly ICoreTurnDiscardSuitHistoryContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreTurnDiscardSuitHistoryCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICoreTurnDiscardSuitHistoryKernelCoreProgramTriggerRuntimeQueriesOperations _kernelCoreProgramTriggerRuntimeQueriesOperations;
    private readonly ICoreTurnDiscardSuitHistoryKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly ICoreTurnDiscardSuitHistoryProgramTriggerOperations _programTriggerOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess _coreActualTurnProgressTurnNumberAccess;
    internal CoreGameEngineTurnDiscardSuitHistoryRuntime(ICoreTurnDiscardSuitHistoryCardZonesOperations cardZonesOperations, ICoreTurnDiscardSuitHistoryContentRegistryOperations contentRegistryOperations, ICoreTurnDiscardSuitHistoryCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICoreTurnDiscardSuitHistoryKernelCoreProgramTriggerRuntimeQueriesOperations kernelCoreProgramTriggerRuntimeQueriesOperations, ICoreTurnDiscardSuitHistoryKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, ICoreTurnDiscardSuitHistoryProgramTriggerOperations programTriggerOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess coreActualTurnProgressTurnNumberAccess)
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
