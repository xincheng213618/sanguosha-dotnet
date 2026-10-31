#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineShanheSourceZirunRuntime : global::CardGame.Core.IShanheSourceZirunHost
{
    private readonly ICoreShanheSourceZirunResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreShanheSourceZirunCoreCardMovementProgramsOperations _coreCardMovementProgramsOperations;
    private readonly ICoreShanheSourceZirunCoreCharacterStateProgramsOperations _coreCharacterStateProgramsOperations;
    private readonly ICoreShanheSourceZirunCoreHalfHandPhaseDebtProgramsOperations _coreHalfHandPhaseDebtProgramsOperations;
    private readonly ICoreShanheSourceZirunCoreHpChangeProgramsOperations _coreHpChangeProgramsOperations;
    private readonly ICoreShanheSourceZirunCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICoreShanheSourceZirunCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreShanheSourceZirunCoreProgramNativeDrawOperations _coreProgramNativeDrawOperations;
    private readonly ICoreShanheSourceZirunCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreShanheSourceZirunCoreSkillOwnershipWindowsOperations _coreSkillOwnershipWindowsOperations;
    private readonly ICoreShanheSourceZirunCoreTableOrderOperations _coreTableOrderOperations;
    private readonly ICoreShanheSourceZirunCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICoreShanheSourceZirunCoreRecoveryReplacementOperations _coreRecoveryReplacementOperations;
    private readonly ICoreShanheSourceZirunCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICoreShanheSourceZirunKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess _coreActualTurnProgressTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineShanheSourceZirunRuntime(ICoreShanheSourceZirunResolutionStackOperations resolutionStackOperations, ICoreShanheSourceZirunCoreCardMovementProgramsOperations coreCardMovementProgramsOperations, ICoreShanheSourceZirunCoreCharacterStateProgramsOperations coreCharacterStateProgramsOperations, ICoreShanheSourceZirunCoreHalfHandPhaseDebtProgramsOperations coreHalfHandPhaseDebtProgramsOperations, ICoreShanheSourceZirunCoreHpChangeProgramsOperations coreHpChangeProgramsOperations, ICoreShanheSourceZirunCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICoreShanheSourceZirunCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreShanheSourceZirunCoreProgramNativeDrawOperations coreProgramNativeDrawOperations, ICoreShanheSourceZirunCoreRuntimeOperations coreRuntimeOperations, ICoreShanheSourceZirunCoreSkillOwnershipWindowsOperations coreSkillOwnershipWindowsOperations, ICoreShanheSourceZirunCoreTableOrderOperations coreTableOrderOperations, ICoreShanheSourceZirunCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICoreShanheSourceZirunCoreRecoveryReplacementOperations coreRecoveryReplacementOperations, ICoreShanheSourceZirunCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICoreShanheSourceZirunKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess coreActualTurnProgressTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreCharacterStateProgramsOperations = coreCharacterStateProgramsOperations;
        this._coreHalfHandPhaseDebtProgramsOperations = coreHalfHandPhaseDebtProgramsOperations;
        this._coreHpChangeProgramsOperations = coreHpChangeProgramsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreProgramNativeDrawOperations = coreProgramNativeDrawOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSkillOwnershipWindowsOperations = coreSkillOwnershipWindowsOperations;
        this._coreTableOrderOperations = coreTableOrderOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreRecoveryReplacementOperations = coreRecoveryReplacementOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressTurnNumberAccess = coreActualTurnProgressTurnNumberAccess;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
