#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineShanheS5ShensuRuntime : global::CardGame.Core.IShanheS5ShensuHost
{
    private readonly ICoreShanheS5ShensuContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreShanheS5ShensuResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreShanheS5ShensuCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICoreShanheS5ShensuCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICoreShanheS5ShensuCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreShanheS5ShensuCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreShanheS5ShensuCoreNativeTargetLegalityOperations _coreNativeTargetLegalityOperations;
    private readonly ICoreShanheS5ShensuCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICoreShanheS5ShensuKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineShanheS5ShensuRuntime(ICoreShanheS5ShensuContentRegistryOperations contentRegistryOperations, ICoreShanheS5ShensuResolutionStackOperations resolutionStackOperations, ICoreShanheS5ShensuCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICoreShanheS5ShensuCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICoreShanheS5ShensuCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreShanheS5ShensuCoreRuntimeOperations coreRuntimeOperations, ICoreShanheS5ShensuCoreNativeTargetLegalityOperations coreNativeTargetLegalityOperations, ICoreShanheS5ShensuCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICoreShanheS5ShensuKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreNativeTargetLegalityOperations = coreNativeTargetLegalityOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
