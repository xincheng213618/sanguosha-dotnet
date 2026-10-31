#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineWushuangRuntime
{
    private readonly ICoreWushuangContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreWushuangModeRuntimeOperations _modeRuntimeOperations;
    private readonly ICoreWushuangCoreWushuangModeProjectionOperations _coreWushuangModeProjectionOperations;
    private readonly ICoreWushuangKernelCoreMainOperations _kernelCoreMainOperations;
    internal CoreGameEngineWushuangRuntime(ICoreWushuangContentRegistryOperations contentRegistryOperations, ICoreWushuangModeRuntimeOperations modeRuntimeOperations, ICoreWushuangCoreWushuangModeProjectionOperations coreWushuangModeProjectionOperations, ICoreWushuangKernelCoreMainOperations kernelCoreMainOperations)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._modeRuntimeOperations = modeRuntimeOperations;
        this._coreWushuangModeProjectionOperations = coreWushuangModeProjectionOperations;
        this._kernelCoreMainOperations = kernelCoreMainOperations;
    }
}
