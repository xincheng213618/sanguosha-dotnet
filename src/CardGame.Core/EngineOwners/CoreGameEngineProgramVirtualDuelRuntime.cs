#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramVirtualDuelRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLuZhiMouOlEntries _characterLuZhiMouOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterYangYiOlEntries _characterYangYiOlOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelProgramPipelineHooksAccess _programPipelineHooksOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreActiveNativeAttackQueriesEntries _coreActiveNativeAttackQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries _coreAlternatingBenefitsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries _coreCardAttackStateOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentRoundActualUsesEntries _coreCurrentRoundActualUsesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentTurnUseKindsEntries _coreCurrentTurnUseKindsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineWholeActualTurnUsesEntries _coreWholeActualTurnUsesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardClassificationsEntries _coreNativeCardClassificationsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries _coreNativeCardContinuationCursorsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDuelEntries _coreNativeDuelOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCreateCardAttackHandleAndCreateDuelHandleOperations _kernelCoreMainOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineProgramVirtualDuelRuntime(global::CardGame.Core.RuntimeEntries.CharacterLuZhiMouOlEntries characterLuZhiMouOlOperations, global::CardGame.Core.RuntimeEntries.CharacterYangYiOlEntries characterYangYiOlOperations, global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.EngineOwners.IKernelProgramPipelineHooksAccess programPipelineHooksOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreActiveNativeAttackQueriesEntries coreActiveNativeAttackQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries coreAlternatingBenefitsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries coreCardAttackStateOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentRoundActualUsesEntries coreCurrentRoundActualUsesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentTurnUseKindsEntries coreCurrentTurnUseKindsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineWholeActualTurnUsesEntries coreWholeActualTurnUsesOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardClassificationsEntries coreNativeCardClassificationsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries coreNativeCardContinuationCursorsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeDuelEntries coreNativeDuelOperations, global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations, global::CardGame.Core.EngineOwners.IKernelCreateCardAttackHandleAndCreateDuelHandleOperations kernelCoreMainOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
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
