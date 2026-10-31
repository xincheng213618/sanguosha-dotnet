#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEnginePindianHandReturnsRuntime
{
    private readonly ICorePindianHandReturnsContentRegistryOperations _contentRegistryOperations;
    private readonly ICorePindianHandReturnsResolutionStackOperations _resolutionStackOperations;
    private readonly ICorePindianHandReturnsCoreCardMovementProgramsOperations _coreCardMovementProgramsOperations;
    private readonly ICorePindianHandReturnsCoreHpChangeProgramsOperations _coreHpChangeProgramsOperations;
    private readonly ICorePindianHandReturnsCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICorePindianHandReturnsCoreOutsidePhaseDrawDiscardOperations _coreOutsidePhaseDrawDiscardOperations;
    private readonly ICorePindianHandReturnsCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICorePindianHandReturnsCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICorePindianHandReturnsCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICorePindianHandReturnsCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICorePindianHandReturnsCoreRecoveryReplacementOperations _coreRecoveryReplacementOperations;
    private readonly ICorePindianHandReturnsKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEnginePindianHandReturnsRuntime(ICorePindianHandReturnsContentRegistryOperations contentRegistryOperations, ICorePindianHandReturnsResolutionStackOperations resolutionStackOperations, ICorePindianHandReturnsCoreCardMovementProgramsOperations coreCardMovementProgramsOperations, ICorePindianHandReturnsCoreHpChangeProgramsOperations coreHpChangeProgramsOperations, ICorePindianHandReturnsCoreNationalSkillsOperations coreNationalSkillsOperations, ICorePindianHandReturnsCoreOutsidePhaseDrawDiscardOperations coreOutsidePhaseDrawDiscardOperations, ICorePindianHandReturnsCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICorePindianHandReturnsCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICorePindianHandReturnsCoreRuntimeOperations coreRuntimeOperations, ICorePindianHandReturnsCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICorePindianHandReturnsCoreRecoveryReplacementOperations coreRecoveryReplacementOperations, ICorePindianHandReturnsKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreHpChangeProgramsOperations = coreHpChangeProgramsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreOutsidePhaseDrawDiscardOperations = coreOutsidePhaseDrawDiscardOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreRecoveryReplacementOperations = coreRecoveryReplacementOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
