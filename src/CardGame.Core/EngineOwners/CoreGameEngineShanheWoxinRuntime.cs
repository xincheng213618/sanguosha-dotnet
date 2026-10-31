#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineShanheWoxinRuntime
{
    private readonly ICoreShanheWoxinContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreShanheWoxinResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreShanheWoxinCoreCardAttackStateOperations _coreCardAttackStateOperations;
    private readonly ICoreShanheWoxinCoreDamageArmorPaymentsOperations _coreDamageArmorPaymentsOperations;
    private readonly ICoreShanheWoxinCoreProgramAttackAttemptOperations _coreProgramAttackAttemptOperations;
    private readonly ICoreShanheWoxinCoreProgramCardPoliciesOperations _coreProgramCardPoliciesOperations;
    private readonly ICoreShanheWoxinCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICoreShanheWoxinKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineShanheWoxinRuntime(ICoreShanheWoxinContentRegistryOperations contentRegistryOperations, ICoreShanheWoxinResolutionStackOperations resolutionStackOperations, ICoreShanheWoxinCoreCardAttackStateOperations coreCardAttackStateOperations, ICoreShanheWoxinCoreDamageArmorPaymentsOperations coreDamageArmorPaymentsOperations, ICoreShanheWoxinCoreProgramAttackAttemptOperations coreProgramAttackAttemptOperations, ICoreShanheWoxinCoreProgramCardPoliciesOperations coreProgramCardPoliciesOperations, ICoreShanheWoxinCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICoreShanheWoxinKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardAttackStateOperations = coreCardAttackStateOperations;
        this._coreDamageArmorPaymentsOperations = coreDamageArmorPaymentsOperations;
        this._coreProgramAttackAttemptOperations = coreProgramAttackAttemptOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
