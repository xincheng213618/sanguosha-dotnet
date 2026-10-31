#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineWushuangAiRuntime
{
    private readonly IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly RuntimeEntries.CoreGameEngineWushuangEntries _coreWushuangOperations;
    private readonly RuntimeEntries.CoreGameEngineWushuangAcquisitionEntries _coreWushuangAcquisitionOperations;
    private readonly RuntimeEntries.CoreGameEngineWushuangTacticRulesEntries _coreWushuangTacticRulesOperations;
    private readonly ICoreWushuangAiCorePlayerViewProjectionOperations _corePlayerViewProjectionOperations;
    private readonly ICoreWushuangAiCoreWushuangModeProjectionOperations _coreWushuangModeProjectionOperations;
    private readonly EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess _coreAiBrainsAiBrainsAccess;
    internal CoreGameEngineWushuangAiRuntime(IKernelContentRegistryAccess contentRegistryOperations, IKernelFrameStoreAccess resolutionStackOperations, RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations, RuntimeEntries.CoreGameEngineWushuangEntries coreWushuangOperations, RuntimeEntries.CoreGameEngineWushuangAcquisitionEntries coreWushuangAcquisitionOperations, RuntimeEntries.CoreGameEngineWushuangTacticRulesEntries coreWushuangTacticRulesOperations, ICoreWushuangAiCorePlayerViewProjectionOperations corePlayerViewProjectionOperations, ICoreWushuangAiCoreWushuangModeProjectionOperations coreWushuangModeProjectionOperations, EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess coreAiBrainsAiBrainsAccess)
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
