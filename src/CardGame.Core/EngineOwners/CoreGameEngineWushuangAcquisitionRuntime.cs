#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineWushuangAcquisitionRuntime
{
    private readonly ICoreWushuangAcquisitionContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreWushuangAcquisitionResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreWushuangAcquisitionCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICoreWushuangAcquisitionCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreWushuangAcquisitionCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreWushuangAcquisitionCoreWushuangOperations _coreWushuangOperations;
    private readonly ICoreWushuangAcquisitionCoreWushuangModeProjectionOperations _coreWushuangModeProjectionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess _coreAiBrainsAiBrainsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineWushuangAcquisitionRuntime(ICoreWushuangAcquisitionContentRegistryOperations contentRegistryOperations, ICoreWushuangAcquisitionResolutionStackOperations resolutionStackOperations, ICoreWushuangAcquisitionCoreNationalSkillsOperations coreNationalSkillsOperations, ICoreWushuangAcquisitionCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreWushuangAcquisitionCoreRuntimeOperations coreRuntimeOperations, ICoreWushuangAcquisitionCoreWushuangOperations coreWushuangOperations, ICoreWushuangAcquisitionCoreWushuangModeProjectionOperations coreWushuangModeProjectionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess coreAiBrainsAiBrainsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreWushuangOperations = coreWushuangOperations;
        this._coreWushuangModeProjectionOperations = coreWushuangModeProjectionOperations;
        this._coreAiBrainsAiBrainsAccess = coreAiBrainsAiBrainsAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
