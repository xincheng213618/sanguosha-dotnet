#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineWushuangAiRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangEntries _coreWushuangOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangAcquisitionEntries _coreWushuangAcquisitionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangTacticRulesEntries _coreWushuangTacticRulesOperations;
    private readonly ICoreWushuangAiCorePlayerViewProjectionOperations _corePlayerViewProjectionOperations;
    private readonly ICoreWushuangAiCoreWushuangModeProjectionOperations _coreWushuangModeProjectionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess _coreAiBrainsAiBrainsAccess;
    internal CoreGameEngineWushuangAiRuntime(global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangEntries coreWushuangOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangAcquisitionEntries coreWushuangAcquisitionOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangTacticRulesEntries coreWushuangTacticRulesOperations, ICoreWushuangAiCorePlayerViewProjectionOperations corePlayerViewProjectionOperations, ICoreWushuangAiCoreWushuangModeProjectionOperations coreWushuangModeProjectionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess coreAiBrainsAiBrainsAccess)
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
