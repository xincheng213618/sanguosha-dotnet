#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineModeSetupRuntime
{
    private readonly IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly IKernelMatchModeRuntimeAccess _modeRuntimeOperations;
    private readonly IKernelParticipantSetupAccess _participantSetupOperations;
    private readonly IKernelCreateParticipantSetupContextOperations _kernelCoreGameEngineModeSetupOperations;
    private readonly EngineStates.Contracts.ICoreMatchDefinitionGeneralPoolAccess _coreMatchDefinitionGeneralPoolAccess;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly EngineStates.Contracts.ICoreMatchSetupAvailableGeneralsSelectionOrderAccess _coreMatchSetupAvailableGeneralsSelectionOrderAccess;
    internal CoreGameEngineModeSetupRuntime(IKernelContentRegistryAccess contentRegistryOperations, IKernelMatchModeRuntimeAccess modeRuntimeOperations, IKernelParticipantSetupAccess participantSetupOperations, IKernelCreateParticipantSetupContextOperations kernelCoreGameEngineModeSetupOperations, EngineStates.Contracts.ICoreMatchDefinitionGeneralPoolAccess coreMatchDefinitionGeneralPoolAccess, EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, EngineStates.Contracts.ICoreMatchSetupAvailableGeneralsSelectionOrderAccess coreMatchSetupAvailableGeneralsSelectionOrderAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._modeRuntimeOperations = modeRuntimeOperations;
        this._participantSetupOperations = participantSetupOperations;
        this._kernelCoreGameEngineModeSetupOperations = kernelCoreGameEngineModeSetupOperations;
        this._coreMatchDefinitionGeneralPoolAccess = coreMatchDefinitionGeneralPoolAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreMatchSetupAvailableGeneralsSelectionOrderAccess = coreMatchSetupAvailableGeneralsSelectionOrderAccess;
    }
}
