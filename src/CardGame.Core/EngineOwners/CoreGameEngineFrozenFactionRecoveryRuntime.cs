#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineFrozenFactionRecoveryRuntime : global::CardGame.Core.IFrozenFactionRecoveryProgramHost
{
    private readonly ICoreFrozenFactionRecoveryCoreDyingCursorRuntimeOperations _coreDyingCursorRuntimeOperations;
    private readonly ICoreFrozenFactionRecoveryCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICoreFrozenFactionRecoveryCoreProgramExpressionsOperations _coreProgramExpressionsOperations;
    private readonly ICoreFrozenFactionRecoveryCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreFrozenFactionRecoveryCoreRuleQueriesOperations _coreRuleQueriesOperations;
    private readonly ICoreFrozenFactionRecoveryCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreFrozenFactionRecoveryCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICoreFrozenFactionRecoveryKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineFrozenFactionRecoveryRuntime(ICoreFrozenFactionRecoveryCoreDyingCursorRuntimeOperations coreDyingCursorRuntimeOperations, ICoreFrozenFactionRecoveryCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICoreFrozenFactionRecoveryCoreProgramExpressionsOperations coreProgramExpressionsOperations, ICoreFrozenFactionRecoveryCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreFrozenFactionRecoveryCoreRuleQueriesOperations coreRuleQueriesOperations, ICoreFrozenFactionRecoveryCoreRuntimeOperations coreRuntimeOperations, ICoreFrozenFactionRecoveryCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICoreFrozenFactionRecoveryKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreProgramExpressionsOperations = coreProgramExpressionsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuleQueriesOperations = coreRuleQueriesOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
