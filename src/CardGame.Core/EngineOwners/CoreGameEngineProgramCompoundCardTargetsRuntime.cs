#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramCompoundCardTargetsRuntime
{
    private readonly ICoreProgramCompoundCardTargetsContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreProgramCompoundCardTargetsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreProgramCompoundCardTargetsCoreCardUseLifecycleOperations _coreCardUseLifecycleOperations;
    private readonly ICoreProgramCompoundCardTargetsCoreProgramCardUseRolesOperations _coreProgramCardUseRolesOperations;
    private readonly ICoreProgramCompoundCardTargetsCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreProgramCompoundCardTargetsCoreNativeBorrowedSwordOperations _coreNativeBorrowedSwordOperations;
    private readonly ICoreProgramCompoundCardTargetsCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICoreProgramCompoundCardTargetsCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess _coreMatchOutcomeStatusAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineProgramCompoundCardTargetsRuntime(ICoreProgramCompoundCardTargetsContentRegistryOperations contentRegistryOperations, ICoreProgramCompoundCardTargetsResolutionStackOperations resolutionStackOperations, ICoreProgramCompoundCardTargetsCoreCardUseLifecycleOperations coreCardUseLifecycleOperations, ICoreProgramCompoundCardTargetsCoreProgramCardUseRolesOperations coreProgramCardUseRolesOperations, ICoreProgramCompoundCardTargetsCoreRuntimeOperations coreRuntimeOperations, ICoreProgramCompoundCardTargetsCoreNativeBorrowedSwordOperations coreNativeBorrowedSwordOperations, ICoreProgramCompoundCardTargetsCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICoreProgramCompoundCardTargetsCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess coreMatchOutcomeStatusAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreProgramCardUseRolesOperations = coreProgramCardUseRolesOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreNativeBorrowedSwordOperations = coreNativeBorrowedSwordOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._coreMatchOutcomeStatusAccess = coreMatchOutcomeStatusAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
