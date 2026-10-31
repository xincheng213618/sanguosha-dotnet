#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineAdvancedLifecycleAiRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries _coreAdvancedLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedWeaponsEntries _coreAdvancedWeaponsOperations;
    private readonly ICoreAdvancedLifecycleAiCoreEquipmentContentOperations _coreEquipmentContentOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineFieldEquipmentEntries _coreFieldEquipmentOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    private readonly ICoreAdvancedLifecycleAiCorePlayerViewProjectionOperations _corePlayerViewProjectionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess _coreAiBrainsAiBrainsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineAdvancedLifecycleAiRuntime(global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess cardZonesOperations, global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries coreAdvancedLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedWeaponsEntries coreAdvancedWeaponsOperations, ICoreAdvancedLifecycleAiCoreEquipmentContentOperations coreEquipmentContentOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineFieldEquipmentEntries coreFieldEquipmentOperations, global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations, ICoreAdvancedLifecycleAiCorePlayerViewProjectionOperations corePlayerViewProjectionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess coreAiBrainsAiBrainsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
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
