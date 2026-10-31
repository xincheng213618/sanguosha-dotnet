#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramAllHandTrickPaymentsRuntime
{
    private readonly ICoreProgramAllHandTrickPaymentsContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreProgramAllHandTrickPaymentsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreProgramAllHandTrickPaymentsCoreCardUsePaymentsOperations _coreCardUsePaymentsOperations;
    private readonly ICoreProgramAllHandTrickPaymentsCoreRuntimeOperations _coreRuntimeOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    internal CoreGameEngineProgramAllHandTrickPaymentsRuntime(ICoreProgramAllHandTrickPaymentsContentRegistryOperations contentRegistryOperations, ICoreProgramAllHandTrickPaymentsResolutionStackOperations resolutionStackOperations, ICoreProgramAllHandTrickPaymentsCoreCardUsePaymentsOperations coreCardUsePaymentsOperations, ICoreProgramAllHandTrickPaymentsCoreRuntimeOperations coreRuntimeOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardUsePaymentsOperations = coreCardUsePaymentsOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
    }
}
