#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineShanheCampaignsRuntime
{
    private readonly ICoreShanheCampaignsCardZonesOperations _cardZonesOperations;
    private readonly ICoreShanheCampaignsCoreAdvancedWeaponsOperations _coreAdvancedWeaponsOperations;
    private readonly ICoreShanheCampaignsCoreEquipmentContentOperations _coreEquipmentContentOperations;
    private readonly ICoreShanheCampaignsCorePhysicalMovementFacadesOperations _corePhysicalMovementFacadesOperations;
    private readonly ICoreShanheCampaignsCoreTrustedCardZoneDiagnosticsOperations _coreTrustedCardZoneDiagnosticsOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionOptionsAccess _coreMatchDefinitionOptionsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineAdvancedWeaponsGeneratedPhysicalCardIdsAccess _coreGameEngineAdvancedWeaponsGeneratedPhysicalCardIdsAccess;
    internal CoreGameEngineShanheCampaignsRuntime(ICoreShanheCampaignsCardZonesOperations cardZonesOperations, ICoreShanheCampaignsCoreAdvancedWeaponsOperations coreAdvancedWeaponsOperations, ICoreShanheCampaignsCoreEquipmentContentOperations coreEquipmentContentOperations, ICoreShanheCampaignsCorePhysicalMovementFacadesOperations corePhysicalMovementFacadesOperations, ICoreShanheCampaignsCoreTrustedCardZoneDiagnosticsOperations coreTrustedCardZoneDiagnosticsOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionOptionsAccess coreMatchDefinitionOptionsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineAdvancedWeaponsGeneratedPhysicalCardIdsAccess coreGameEngineAdvancedWeaponsGeneratedPhysicalCardIdsAccess)
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
