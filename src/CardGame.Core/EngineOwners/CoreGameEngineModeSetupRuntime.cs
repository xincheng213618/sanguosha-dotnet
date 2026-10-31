#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineModeSetupRuntime
{
    private readonly ICoreModeSetupContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreModeSetupModeRuntimeOperations _modeRuntimeOperations;
    private readonly ICoreModeSetupParticipantSetupOperations _participantSetupOperations;
    private readonly ICoreModeSetupKernelCoreGameEngineModeSetupOperations _kernelCoreGameEngineModeSetupOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionGeneralPoolAccess _coreMatchDefinitionGeneralPoolAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupAvailableGeneralsSelectionOrderAccess _coreMatchSetupAvailableGeneralsSelectionOrderAccess;
    internal CoreGameEngineModeSetupRuntime(ICoreModeSetupContentRegistryOperations contentRegistryOperations, ICoreModeSetupModeRuntimeOperations modeRuntimeOperations, ICoreModeSetupParticipantSetupOperations participantSetupOperations, ICoreModeSetupKernelCoreGameEngineModeSetupOperations kernelCoreGameEngineModeSetupOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionGeneralPoolAccess coreMatchDefinitionGeneralPoolAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupAvailableGeneralsSelectionOrderAccess coreMatchSetupAvailableGeneralsSelectionOrderAccess)
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
