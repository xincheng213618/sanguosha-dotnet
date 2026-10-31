#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineAdvancedLifecycleAiRuntime
{
    private readonly ICoreAdvancedLifecycleAiCardZonesOperations _cardZonesOperations;
    private readonly ICoreAdvancedLifecycleAiContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreAdvancedLifecycleAiCoreAdvancedLifecycleOperations _coreAdvancedLifecycleOperations;
    private readonly ICoreAdvancedLifecycleAiCoreAdvancedWeaponsOperations _coreAdvancedWeaponsOperations;
    private readonly ICoreAdvancedLifecycleAiCoreEquipmentContentOperations _coreEquipmentContentOperations;
    private readonly ICoreAdvancedLifecycleAiCoreFieldEquipmentOperations _coreFieldEquipmentOperations;
    private readonly ICoreAdvancedLifecycleAiCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICoreAdvancedLifecycleAiCorePlayerViewProjectionOperations _corePlayerViewProjectionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess _coreAiBrainsAiBrainsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineAdvancedLifecycleAiRuntime(ICoreAdvancedLifecycleAiCardZonesOperations cardZonesOperations, ICoreAdvancedLifecycleAiContentRegistryOperations contentRegistryOperations, ICoreAdvancedLifecycleAiCoreAdvancedLifecycleOperations coreAdvancedLifecycleOperations, ICoreAdvancedLifecycleAiCoreAdvancedWeaponsOperations coreAdvancedWeaponsOperations, ICoreAdvancedLifecycleAiCoreEquipmentContentOperations coreEquipmentContentOperations, ICoreAdvancedLifecycleAiCoreFieldEquipmentOperations coreFieldEquipmentOperations, ICoreAdvancedLifecycleAiCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICoreAdvancedLifecycleAiCorePlayerViewProjectionOperations corePlayerViewProjectionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess coreAiBrainsAiBrainsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreAdvancedLifecycleOperations = coreAdvancedLifecycleOperations;
        this._coreAdvancedWeaponsOperations = coreAdvancedWeaponsOperations;
        this._coreEquipmentContentOperations = coreEquipmentContentOperations;
        this._coreFieldEquipmentOperations = coreFieldEquipmentOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._corePlayerViewProjectionOperations = corePlayerViewProjectionOperations;
        this._coreAiBrainsAiBrainsAccess = coreAiBrainsAiBrainsAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
