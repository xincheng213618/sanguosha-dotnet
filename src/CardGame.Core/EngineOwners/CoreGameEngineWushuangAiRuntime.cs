#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineWushuangAiRuntime
{
    private readonly ICoreWushuangAiContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreWushuangAiResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreWushuangAiCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreWushuangAiCoreWushuangOperations _coreWushuangOperations;
    private readonly ICoreWushuangAiCoreWushuangAcquisitionOperations _coreWushuangAcquisitionOperations;
    private readonly ICoreWushuangAiCoreWushuangTacticRulesOperations _coreWushuangTacticRulesOperations;
    private readonly ICoreWushuangAiCorePlayerViewProjectionOperations _corePlayerViewProjectionOperations;
    private readonly ICoreWushuangAiCoreWushuangModeProjectionOperations _coreWushuangModeProjectionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess _coreAiBrainsAiBrainsAccess;
    internal CoreGameEngineWushuangAiRuntime(ICoreWushuangAiContentRegistryOperations contentRegistryOperations, ICoreWushuangAiResolutionStackOperations resolutionStackOperations, ICoreWushuangAiCoreRuntimeOperations coreRuntimeOperations, ICoreWushuangAiCoreWushuangOperations coreWushuangOperations, ICoreWushuangAiCoreWushuangAcquisitionOperations coreWushuangAcquisitionOperations, ICoreWushuangAiCoreWushuangTacticRulesOperations coreWushuangTacticRulesOperations, ICoreWushuangAiCorePlayerViewProjectionOperations corePlayerViewProjectionOperations, ICoreWushuangAiCoreWushuangModeProjectionOperations coreWushuangModeProjectionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess coreAiBrainsAiBrainsAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreWushuangOperations = coreWushuangOperations;
        this._coreWushuangAcquisitionOperations = coreWushuangAcquisitionOperations;
        this._coreWushuangTacticRulesOperations = coreWushuangTacticRulesOperations;
        this._corePlayerViewProjectionOperations = corePlayerViewProjectionOperations;
        this._coreWushuangModeProjectionOperations = coreWushuangModeProjectionOperations;
        this._coreAiBrainsAiBrainsAccess = coreAiBrainsAiBrainsAccess;
    }
}
