#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGuanduEventProgramsRuntime : global::CardGame.Core.IGuanduEventHost
{
    private readonly ICoreGuanduEventProgramsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreGuanduEventProgramsCoreDyingCursorRuntimeOperations _coreDyingCursorRuntimeOperations;
    private readonly ICoreGuanduEventProgramsCoreActivityBattleRulesOperations _coreActivityBattleRulesOperations;
    private readonly ICoreGuanduEventProgramsCoreCardUseLifecycleOperations _coreCardUseLifecycleOperations;
    private readonly ICoreGuanduEventProgramsCoreGuanduActivitiesOperations _coreGuanduActivitiesOperations;
    private readonly ICoreGuanduEventProgramsCoreProgramAttackAttemptOperations _coreProgramAttackAttemptOperations;
    private readonly ICoreGuanduEventProgramsCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICoreGuanduEventProgramsCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreGuanduEventProgramsCoreRuntimeOperations _coreRuntimeOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess _coreActualTurnProgressTurnNumberAccess;
    internal CoreGuanduEventProgramsRuntime(ICoreGuanduEventProgramsResolutionStackOperations resolutionStackOperations, ICoreGuanduEventProgramsCoreDyingCursorRuntimeOperations coreDyingCursorRuntimeOperations, ICoreGuanduEventProgramsCoreActivityBattleRulesOperations coreActivityBattleRulesOperations, ICoreGuanduEventProgramsCoreCardUseLifecycleOperations coreCardUseLifecycleOperations, ICoreGuanduEventProgramsCoreGuanduActivitiesOperations coreGuanduActivitiesOperations, ICoreGuanduEventProgramsCoreProgramAttackAttemptOperations coreProgramAttackAttemptOperations, ICoreGuanduEventProgramsCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICoreGuanduEventProgramsCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreGuanduEventProgramsCoreRuntimeOperations coreRuntimeOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess coreActualTurnProgressTurnNumberAccess)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreActivityBattleRulesOperations = coreActivityBattleRulesOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreGuanduActivitiesOperations = coreGuanduActivitiesOperations;
        this._coreProgramAttackAttemptOperations = coreProgramAttackAttemptOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreActualTurnProgressTurnNumberAccess = coreActualTurnProgressTurnNumberAccess;
    }
}
