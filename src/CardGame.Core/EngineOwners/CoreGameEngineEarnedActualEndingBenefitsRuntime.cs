#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineEarnedActualEndingBenefitsRuntime
{
    private readonly ICoreEarnedActualEndingBenefitsContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreEarnedActualEndingBenefitsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreEarnedActualEndingBenefitsCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICoreEarnedActualEndingBenefitsCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreEarnedActualEndingBenefitsCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICoreEarnedActualEndingBenefitsKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineEarnedActualEndingBenefitsRuntime(ICoreEarnedActualEndingBenefitsContentRegistryOperations contentRegistryOperations, ICoreEarnedActualEndingBenefitsResolutionStackOperations resolutionStackOperations, ICoreEarnedActualEndingBenefitsCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICoreEarnedActualEndingBenefitsCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreEarnedActualEndingBenefitsCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICoreEarnedActualEndingBenefitsKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
