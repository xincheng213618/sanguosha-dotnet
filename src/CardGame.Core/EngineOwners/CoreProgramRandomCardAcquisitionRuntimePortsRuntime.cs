#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreProgramRandomCardAcquisitionRuntimePortsRuntime : global::CardGame.Core.IProgramRandomCardAcquisitionFacts, global::CardGame.Core.IProgramRandomCardAcquisitionNativeEntries
{
    private readonly ICoreProgramRandomCardAcquisitionRuntimePortsCharacterBianFuRenOlOperations _characterBianFuRenOlOperations;
    private readonly ICoreProgramRandomCardAcquisitionRuntimePortsCoreAdvancedWeaponsOperations _coreAdvancedWeaponsOperations;
    private readonly ICoreProgramRandomCardAcquisitionRuntimePortsCoreTableOrderOperations _coreTableOrderOperations;
    private readonly ICoreProgramRandomCardAcquisitionRuntimePortsCoreMatchLogOperations _coreMatchLogOperations;
    private readonly ICoreProgramRandomCardAcquisitionRuntimePortsCorePhysicalMovementFacadesOperations _corePhysicalMovementFacadesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreProgramRandomCardAcquisitionRuntimePortsRuntime(ICoreProgramRandomCardAcquisitionRuntimePortsCharacterBianFuRenOlOperations characterBianFuRenOlOperations, ICoreProgramRandomCardAcquisitionRuntimePortsCoreAdvancedWeaponsOperations coreAdvancedWeaponsOperations, ICoreProgramRandomCardAcquisitionRuntimePortsCoreTableOrderOperations coreTableOrderOperations, ICoreProgramRandomCardAcquisitionRuntimePortsCoreMatchLogOperations coreMatchLogOperations, ICoreProgramRandomCardAcquisitionRuntimePortsCorePhysicalMovementFacadesOperations corePhysicalMovementFacadesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterBianFuRenOlOperations = characterBianFuRenOlOperations;
        this._coreAdvancedWeaponsOperations = coreAdvancedWeaponsOperations;
        this._coreTableOrderOperations = coreTableOrderOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
