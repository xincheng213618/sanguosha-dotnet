#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreAwaitedBlindHandTakeProgramsRuntime : global::CardGame.Core.IAwaitedBlindHandTakeProgramHost
{
    private readonly ICoreAwaitedBlindHandTakeProgramsContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreAwaitedBlindHandTakeProgramsRandomOperations _randomOperations;
    private readonly ICoreAwaitedBlindHandTakeProgramsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreAwaitedBlindHandTakeProgramsCoreCardMovementProgramsOperations _coreCardMovementProgramsOperations;
    private readonly ICoreAwaitedBlindHandTakeProgramsCoreHpChangeProgramsOperations _coreHpChangeProgramsOperations;
    private readonly ICoreAwaitedBlindHandTakeProgramsCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICoreAwaitedBlindHandTakeProgramsCoreProgramAtomicCardMovesOperations _coreProgramAtomicCardMovesOperations;
    private readonly ICoreAwaitedBlindHandTakeProgramsCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreAwaitedBlindHandTakeProgramsCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreAwaitedBlindHandTakeProgramsCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICoreAwaitedBlindHandTakeProgramsCoreRecoveryReplacementOperations _coreRecoveryReplacementOperations;
    private readonly ICoreAwaitedBlindHandTakeProgramsCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICoreAwaitedBlindHandTakeProgramsKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreAwaitedBlindHandTakeProgramsRuntime(ICoreAwaitedBlindHandTakeProgramsContentRegistryOperations contentRegistryOperations, ICoreAwaitedBlindHandTakeProgramsRandomOperations randomOperations, ICoreAwaitedBlindHandTakeProgramsResolutionStackOperations resolutionStackOperations, ICoreAwaitedBlindHandTakeProgramsCoreCardMovementProgramsOperations coreCardMovementProgramsOperations, ICoreAwaitedBlindHandTakeProgramsCoreHpChangeProgramsOperations coreHpChangeProgramsOperations, ICoreAwaitedBlindHandTakeProgramsCoreNationalSkillsOperations coreNationalSkillsOperations, ICoreAwaitedBlindHandTakeProgramsCoreProgramAtomicCardMovesOperations coreProgramAtomicCardMovesOperations, ICoreAwaitedBlindHandTakeProgramsCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreAwaitedBlindHandTakeProgramsCoreRuntimeOperations coreRuntimeOperations, ICoreAwaitedBlindHandTakeProgramsCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICoreAwaitedBlindHandTakeProgramsCoreRecoveryReplacementOperations coreRecoveryReplacementOperations, ICoreAwaitedBlindHandTakeProgramsCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICoreAwaitedBlindHandTakeProgramsKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._randomOperations = randomOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreHpChangeProgramsOperations = coreHpChangeProgramsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramAtomicCardMovesOperations = coreProgramAtomicCardMovesOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreRecoveryReplacementOperations = coreRecoveryReplacementOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
