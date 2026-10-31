#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineActivityOrdinaryTricksRuntime
{
    private readonly IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly IKernelMatchModeRulesAccess _modeRulesOperations;
    private readonly EngineStates.Contracts.ICoreGameEngineActivityOrdinaryTricksActivityOrdinaryProgramsAccess _coreGameEngineActivityOrdinaryTricksActivityOrdinaryProgramsAccess;
    internal CoreGameEngineActivityOrdinaryTricksRuntime(IKernelContentRegistryAccess contentRegistryOperations, IKernelMatchModeRulesAccess modeRulesOperations, EngineStates.Contracts.ICoreGameEngineActivityOrdinaryTricksActivityOrdinaryProgramsAccess coreGameEngineActivityOrdinaryTricksActivityOrdinaryProgramsAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._modeRulesOperations = modeRulesOperations;
        this._coreGameEngineActivityOrdinaryTricksActivityOrdinaryProgramsAccess = coreGameEngineActivityOrdinaryTricksActivityOrdinaryProgramsAccess;
    }
}
