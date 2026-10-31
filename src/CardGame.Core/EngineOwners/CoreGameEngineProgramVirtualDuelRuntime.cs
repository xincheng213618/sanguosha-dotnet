#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramVirtualDuelRuntime
{
    private readonly ICoreProgramVirtualDuelCharacterLuZhiMouOlOperations _characterLuZhiMouOlOperations;
    private readonly ICoreProgramVirtualDuelCharacterYangYiOlOperations _characterYangYiOlOperations;
    private readonly ICoreProgramVirtualDuelContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreProgramVirtualDuelProgramPipelineHooksOperations _programPipelineHooksOperations;
    private readonly ICoreProgramVirtualDuelResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreProgramVirtualDuelCoreActiveNativeAttackQueriesOperations _coreActiveNativeAttackQueriesOperations;
    private readonly ICoreProgramVirtualDuelCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICoreProgramVirtualDuelCoreCardAttackStateOperations _coreCardAttackStateOperations;
    private readonly ICoreProgramVirtualDuelCoreCurrentRoundActualUsesOperations _coreCurrentRoundActualUsesOperations;
    private readonly ICoreProgramVirtualDuelCoreCurrentTurnUseKindsOperations _coreCurrentTurnUseKindsOperations;
    private readonly ICoreProgramVirtualDuelCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreProgramVirtualDuelCoreWholeActualTurnUsesOperations _coreWholeActualTurnUsesOperations;
    private readonly ICoreProgramVirtualDuelCoreNativeCardClassificationsOperations _coreNativeCardClassificationsOperations;
    private readonly ICoreProgramVirtualDuelCoreNativeCardContinuationCursorsOperations _coreNativeCardContinuationCursorsOperations;
    private readonly ICoreProgramVirtualDuelCoreNativeDuelOperations _coreNativeDuelOperations;
    private readonly ICoreProgramVirtualDuelCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICoreProgramVirtualDuelKernelCoreMainOperations _kernelCoreMainOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineProgramVirtualDuelRuntime(ICoreProgramVirtualDuelCharacterLuZhiMouOlOperations characterLuZhiMouOlOperations, ICoreProgramVirtualDuelCharacterYangYiOlOperations characterYangYiOlOperations, ICoreProgramVirtualDuelContentRegistryOperations contentRegistryOperations, ICoreProgramVirtualDuelProgramPipelineHooksOperations programPipelineHooksOperations, ICoreProgramVirtualDuelResolutionStackOperations resolutionStackOperations, ICoreProgramVirtualDuelCoreActiveNativeAttackQueriesOperations coreActiveNativeAttackQueriesOperations, ICoreProgramVirtualDuelCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICoreProgramVirtualDuelCoreCardAttackStateOperations coreCardAttackStateOperations, ICoreProgramVirtualDuelCoreCurrentRoundActualUsesOperations coreCurrentRoundActualUsesOperations, ICoreProgramVirtualDuelCoreCurrentTurnUseKindsOperations coreCurrentTurnUseKindsOperations, ICoreProgramVirtualDuelCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreProgramVirtualDuelCoreWholeActualTurnUsesOperations coreWholeActualTurnUsesOperations, ICoreProgramVirtualDuelCoreNativeCardClassificationsOperations coreNativeCardClassificationsOperations, ICoreProgramVirtualDuelCoreNativeCardContinuationCursorsOperations coreNativeCardContinuationCursorsOperations, ICoreProgramVirtualDuelCoreNativeDuelOperations coreNativeDuelOperations, ICoreProgramVirtualDuelCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICoreProgramVirtualDuelKernelCoreMainOperations kernelCoreMainOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
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
