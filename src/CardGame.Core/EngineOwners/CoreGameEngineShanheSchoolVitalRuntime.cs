#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineShanheSchoolVitalRuntime : global::CardGame.Core.IShanheSchoolVitalHost
{
    private readonly ICoreShanheSchoolVitalResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreShanheSchoolVitalCoreHpChangeProgramsOperations _coreHpChangeProgramsOperations;
    private readonly ICoreShanheSchoolVitalCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICoreShanheSchoolVitalCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreShanheSchoolVitalCoreShanheSchoolsOperations _coreShanheSchoolsOperations;
    private readonly ICoreShanheSchoolVitalKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatAccess _coreActualTurnProgressCurrentSeatAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineShanheSchoolVitalRuntime(ICoreShanheSchoolVitalResolutionStackOperations resolutionStackOperations, ICoreShanheSchoolVitalCoreHpChangeProgramsOperations coreHpChangeProgramsOperations, ICoreShanheSchoolVitalCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICoreShanheSchoolVitalCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreShanheSchoolVitalCoreShanheSchoolsOperations coreShanheSchoolsOperations, ICoreShanheSchoolVitalKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatAccess coreActualTurnProgressCurrentSeatAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreHpChangeProgramsOperations = coreHpChangeProgramsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreShanheSchoolsOperations = coreShanheSchoolsOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatAccess = coreActualTurnProgressCurrentSeatAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
