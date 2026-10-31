#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineWushuangRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelMatchModeRuntimeAccess _modeRuntimeOperations;
    private readonly ICoreWushuangCoreWushuangModeProjectionOperations _coreWushuangModeProjectionOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelRejectOperations _kernelCoreMainOperations;
    internal CoreGameEngineWushuangRuntime(global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.EngineOwners.IKernelMatchModeRuntimeAccess modeRuntimeOperations, ICoreWushuangCoreWushuangModeProjectionOperations coreWushuangModeProjectionOperations, global::CardGame.Core.EngineOwners.IKernelRejectOperations kernelCoreMainOperations)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._modeRuntimeOperations = modeRuntimeOperations;
        this._coreWushuangModeProjectionOperations = coreWushuangModeProjectionOperations;
        this._kernelCoreMainOperations = kernelCoreMainOperations;
    }
}
