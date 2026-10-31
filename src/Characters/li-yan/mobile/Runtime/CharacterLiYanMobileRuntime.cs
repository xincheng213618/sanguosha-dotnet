#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterLiYanMobileRuntime : global::CardGame.Core.IMobileLiYanNextDrawHost
{
    private readonly ICharacterLiYanMobileContentRegistryOperations _contentRegistryOperations;
    private readonly ICharacterLiYanMobileCoreActualDiscardRecoveryPhasesOperations _coreActualDiscardRecoveryPhasesOperations;
    private readonly ICharacterLiYanMobileCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICharacterLiYanMobileCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICharacterLiYanMobileCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICharacterLiYanMobileKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterLiYanMobileRuntime(ICharacterLiYanMobileContentRegistryOperations contentRegistryOperations, ICharacterLiYanMobileCoreActualDiscardRecoveryPhasesOperations coreActualDiscardRecoveryPhasesOperations, ICharacterLiYanMobileCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICharacterLiYanMobileCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICharacterLiYanMobileCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICharacterLiYanMobileKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreActualDiscardRecoveryPhasesOperations = coreActualDiscardRecoveryPhasesOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
