#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramDamageTransferRuntime
{
    private readonly ICoreProgramDamageTransferResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreProgramDamageTransferCoreProgramAttackAttemptOperations _coreProgramAttackAttemptOperations;
    private readonly ICoreProgramDamageTransferCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreProgramDamageTransferCoreRecipientScopedDamageOperations _coreRecipientScopedDamageOperations;
    private readonly ICoreProgramDamageTransferCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreProgramDamageTransferCoreMatchLogOperations _coreMatchLogOperations;
    private readonly ICoreProgramDamageTransferCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICoreProgramDamageTransferKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineProgramDamageTransferRuntime(ICoreProgramDamageTransferResolutionStackOperations resolutionStackOperations, ICoreProgramDamageTransferCoreProgramAttackAttemptOperations coreProgramAttackAttemptOperations, ICoreProgramDamageTransferCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreProgramDamageTransferCoreRecipientScopedDamageOperations coreRecipientScopedDamageOperations, ICoreProgramDamageTransferCoreRuntimeOperations coreRuntimeOperations, ICoreProgramDamageTransferCoreMatchLogOperations coreMatchLogOperations, ICoreProgramDamageTransferCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICoreProgramDamageTransferKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreProgramAttackAttemptOperations = coreProgramAttackAttemptOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRecipientScopedDamageOperations = coreRecipientScopedDamageOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
