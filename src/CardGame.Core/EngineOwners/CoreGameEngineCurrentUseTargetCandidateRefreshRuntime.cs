#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineCurrentUseTargetCandidateRefreshRuntime
{
    private readonly ICoreCurrentUseTargetCandidateRefreshContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreCurrentUseTargetCandidateRefreshResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreCurrentUseTargetCandidateRefreshCoreCardActionsOperations _coreCardActionsOperations;
    private readonly ICoreCurrentUseTargetCandidateRefreshCoreDesignatedExtraTargetsOperations _coreDesignatedExtraTargetsOperations;
    private readonly ICoreCurrentUseTargetCandidateRefreshCoreProgramCardUseRolesOperations _coreProgramCardUseRolesOperations;
    private readonly ICoreCurrentUseTargetCandidateRefreshCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreCurrentUseTargetCandidateRefreshCoreTableOrderOperations _coreTableOrderOperations;
    private readonly ICoreCurrentUseTargetCandidateRefreshCoreProgramTargetWindowProtocolsOperations _coreProgramTargetWindowProtocolsOperations;
    internal CoreGameEngineCurrentUseTargetCandidateRefreshRuntime(ICoreCurrentUseTargetCandidateRefreshContentRegistryOperations contentRegistryOperations, ICoreCurrentUseTargetCandidateRefreshResolutionStackOperations resolutionStackOperations, ICoreCurrentUseTargetCandidateRefreshCoreCardActionsOperations coreCardActionsOperations, ICoreCurrentUseTargetCandidateRefreshCoreDesignatedExtraTargetsOperations coreDesignatedExtraTargetsOperations, ICoreCurrentUseTargetCandidateRefreshCoreProgramCardUseRolesOperations coreProgramCardUseRolesOperations, ICoreCurrentUseTargetCandidateRefreshCoreRuntimeOperations coreRuntimeOperations, ICoreCurrentUseTargetCandidateRefreshCoreTableOrderOperations coreTableOrderOperations, ICoreCurrentUseTargetCandidateRefreshCoreProgramTargetWindowProtocolsOperations coreProgramTargetWindowProtocolsOperations)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardActionsOperations = coreCardActionsOperations;
        this._coreDesignatedExtraTargetsOperations = coreDesignatedExtraTargetsOperations;
        this._coreProgramCardUseRolesOperations = coreProgramCardUseRolesOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreTableOrderOperations = coreTableOrderOperations;
        this._coreProgramTargetWindowProtocolsOperations = coreProgramTargetWindowProtocolsOperations;
    }
}
