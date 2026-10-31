#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineDamageArmorPaymentsRuntime
{
    private readonly ICoreDamageArmorPaymentsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreDamageArmorPaymentsCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICoreDamageArmorPaymentsCoreNativeDamageArmorPoliciesOperations _coreNativeDamageArmorPoliciesOperations;
    private readonly ICoreDamageArmorPaymentsCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICoreDamageArmorPaymentsCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreDamageArmorPaymentsKernelCoreGameEngineProgramAttackAttemptOperations _kernelCoreGameEngineProgramAttackAttemptOperations;
    private readonly ICoreDamageArmorPaymentsKernelCoreMainOperations _kernelCoreMainOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineDamageArmorPaymentsRuntime(ICoreDamageArmorPaymentsResolutionStackOperations resolutionStackOperations, ICoreDamageArmorPaymentsCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICoreDamageArmorPaymentsCoreNativeDamageArmorPoliciesOperations coreNativeDamageArmorPoliciesOperations, ICoreDamageArmorPaymentsCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICoreDamageArmorPaymentsCoreRuntimeOperations coreRuntimeOperations, ICoreDamageArmorPaymentsKernelCoreGameEngineProgramAttackAttemptOperations kernelCoreGameEngineProgramAttackAttemptOperations, ICoreDamageArmorPaymentsKernelCoreMainOperations kernelCoreMainOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreNativeDamageArmorPoliciesOperations = coreNativeDamageArmorPoliciesOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._kernelCoreGameEngineProgramAttackAttemptOperations = kernelCoreGameEngineProgramAttackAttemptOperations;
        this._kernelCoreMainOperations = kernelCoreMainOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
