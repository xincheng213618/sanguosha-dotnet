#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineGuanduGeneralAiRuntime
{
    private readonly ICoreGuanduGeneralAiCharacterHuangQuanOlOperations _characterHuangQuanOlOperations;
    private readonly ICoreGuanduGeneralAiCardZonesOperations _cardZonesOperations;
    private readonly ICoreGuanduGeneralAiCoreAdvancedLifecycleOperations _coreAdvancedLifecycleOperations;
    private readonly ICoreGuanduGeneralAiCoreGuanduActivitiesOperations _coreGuanduActivitiesOperations;
    private readonly ICoreGuanduGeneralAiCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICoreGuanduGeneralAiCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineGuanduGeneralAiRuntime(ICoreGuanduGeneralAiCharacterHuangQuanOlOperations characterHuangQuanOlOperations, ICoreGuanduGeneralAiCardZonesOperations cardZonesOperations, ICoreGuanduGeneralAiCoreAdvancedLifecycleOperations coreAdvancedLifecycleOperations, ICoreGuanduGeneralAiCoreGuanduActivitiesOperations coreGuanduActivitiesOperations, ICoreGuanduGeneralAiCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICoreGuanduGeneralAiCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterHuangQuanOlOperations = characterHuangQuanOlOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._coreAdvancedLifecycleOperations = coreAdvancedLifecycleOperations;
        this._coreGuanduActivitiesOperations = coreGuanduActivitiesOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
