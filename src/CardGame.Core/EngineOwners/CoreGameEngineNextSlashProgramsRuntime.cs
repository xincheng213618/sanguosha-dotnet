#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineNextSlashProgramsRuntime : global::CardGame.Core.INextSlashProgramHost
{
    private readonly ICoreNextSlashProgramsContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreNextSlashProgramsCoreActiveNativeAttackQueriesOperations _coreActiveNativeAttackQueriesOperations;
    private readonly ICoreNextSlashProgramsCoreCardUseLifecycleOperations _coreCardUseLifecycleOperations;
    private readonly ICoreNextSlashProgramsCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreNextSlashProgramsCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreNextSlashProgramsCoreSlashSuitDiscardOperations _coreSlashSuitDiscardOperations;
    private readonly ICoreNextSlashProgramsCoreNativeRecoveryFramesOperations _coreNativeRecoveryFramesOperations;
    private readonly ICoreNextSlashProgramsCoreRecoveryReplacementOperations _coreRecoveryReplacementOperations;
    private readonly ICoreNextSlashProgramsKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineNextSlashProgramsNamedNextSlashReservesAccess _coreGameEngineNextSlashProgramsNamedNextSlashReservesAccess;
    internal CoreGameEngineNextSlashProgramsRuntime(ICoreNextSlashProgramsContentRegistryOperations contentRegistryOperations, ICoreNextSlashProgramsCoreActiveNativeAttackQueriesOperations coreActiveNativeAttackQueriesOperations, ICoreNextSlashProgramsCoreCardUseLifecycleOperations coreCardUseLifecycleOperations, ICoreNextSlashProgramsCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreNextSlashProgramsCoreRuntimeOperations coreRuntimeOperations, ICoreNextSlashProgramsCoreSlashSuitDiscardOperations coreSlashSuitDiscardOperations, ICoreNextSlashProgramsCoreNativeRecoveryFramesOperations coreNativeRecoveryFramesOperations, ICoreNextSlashProgramsCoreRecoveryReplacementOperations coreRecoveryReplacementOperations, ICoreNextSlashProgramsKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineNextSlashProgramsNamedNextSlashReservesAccess coreGameEngineNextSlashProgramsNamedNextSlashReservesAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreActiveNativeAttackQueriesOperations = coreActiveNativeAttackQueriesOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSlashSuitDiscardOperations = coreSlashSuitDiscardOperations;
        this._coreNativeRecoveryFramesOperations = coreNativeRecoveryFramesOperations;
        this._coreRecoveryReplacementOperations = coreRecoveryReplacementOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreGameEngineNextSlashProgramsNamedNextSlashReservesAccess = coreGameEngineNextSlashProgramsNamedNextSlashReservesAccess;
    }
}
