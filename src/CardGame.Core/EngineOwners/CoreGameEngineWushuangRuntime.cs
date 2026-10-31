#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineWushuangRuntime
{
    private readonly IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly IKernelMatchModeRuntimeAccess _modeRuntimeOperations;
    private readonly ICoreWushuangCoreWushuangModeProjectionOperations _coreWushuangModeProjectionOperations;
    private readonly IKernelRejectOperations _kernelCoreMainOperations;
    internal CoreGameEngineWushuangRuntime(IKernelContentRegistryAccess contentRegistryOperations, IKernelMatchModeRuntimeAccess modeRuntimeOperations, ICoreWushuangCoreWushuangModeProjectionOperations coreWushuangModeProjectionOperations, IKernelRejectOperations kernelCoreMainOperations)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._modeRuntimeOperations = modeRuntimeOperations;
        this._coreWushuangModeProjectionOperations = coreWushuangModeProjectionOperations;
        this._kernelCoreMainOperations = kernelCoreMainOperations;
    }
}
