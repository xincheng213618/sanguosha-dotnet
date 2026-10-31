#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineShanheCampaignsRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedWeaponsEntries _coreAdvancedWeaponsOperations;
    private readonly ICoreShanheCampaignsCoreEquipmentContentOperations _coreEquipmentContentOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalMovementFacadesEntries _corePhysicalMovementFacadesOperations;
    private readonly ICoreShanheCampaignsCoreTrustedCardZoneDiagnosticsOperations _coreTrustedCardZoneDiagnosticsOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionOptionsAccess _coreMatchDefinitionOptionsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineAdvancedWeaponsGeneratedPhysicalCardIdsAccess _coreGameEngineAdvancedWeaponsGeneratedPhysicalCardIdsAccess;
    internal CoreGameEngineShanheCampaignsRuntime(global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess cardZonesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedWeaponsEntries coreAdvancedWeaponsOperations, ICoreShanheCampaignsCoreEquipmentContentOperations coreEquipmentContentOperations, global::CardGame.Core.RuntimeEntries.CorePhysicalMovementFacadesEntries corePhysicalMovementFacadesOperations, ICoreShanheCampaignsCoreTrustedCardZoneDiagnosticsOperations coreTrustedCardZoneDiagnosticsOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionOptionsAccess coreMatchDefinitionOptionsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineAdvancedWeaponsGeneratedPhysicalCardIdsAccess coreGameEngineAdvancedWeaponsGeneratedPhysicalCardIdsAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._coreAdvancedWeaponsOperations = coreAdvancedWeaponsOperations;
        this._coreEquipmentContentOperations = coreEquipmentContentOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._coreTrustedCardZoneDiagnosticsOperations = coreTrustedCardZoneDiagnosticsOperations;
        this._coreMatchDefinitionOptionsAccess = coreMatchDefinitionOptionsAccess;
        this._coreGameEngineAdvancedWeaponsGeneratedPhysicalCardIdsAccess = coreGameEngineAdvancedWeaponsGeneratedPhysicalCardIdsAccess;
    }
}
