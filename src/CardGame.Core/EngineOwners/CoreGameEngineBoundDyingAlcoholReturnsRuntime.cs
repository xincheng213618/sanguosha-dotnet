#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineBoundDyingAlcoholReturnsRuntime
{
    private readonly ICoreBoundDyingAlcoholReturnsContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreBoundDyingAlcoholReturnsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreBoundDyingAlcoholReturnsCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICoreBoundDyingAlcoholReturnsCoreCardUseLifecycleOperations _coreCardUseLifecycleOperations;
    private readonly ICoreBoundDyingAlcoholReturnsCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICoreBoundDyingAlcoholReturnsCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreBoundDyingAlcoholReturnsCoreMatchLogOperations _coreMatchLogOperations;
    private readonly ICoreBoundDyingAlcoholReturnsKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess _coreCardMovementJournalCardMovementsMovementSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineBoundDyingAlcoholReturnsRuntime(ICoreBoundDyingAlcoholReturnsContentRegistryOperations contentRegistryOperations, ICoreBoundDyingAlcoholReturnsResolutionStackOperations resolutionStackOperations, ICoreBoundDyingAlcoholReturnsCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICoreBoundDyingAlcoholReturnsCoreCardUseLifecycleOperations coreCardUseLifecycleOperations, ICoreBoundDyingAlcoholReturnsCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICoreBoundDyingAlcoholReturnsCoreRuntimeOperations coreRuntimeOperations, ICoreBoundDyingAlcoholReturnsCoreMatchLogOperations coreMatchLogOperations, ICoreBoundDyingAlcoholReturnsKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess coreCardMovementJournalCardMovementsMovementSequenceAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreCardMovementJournalCardMovementsMovementSequenceAccess = coreCardMovementJournalCardMovementsMovementSequenceAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
