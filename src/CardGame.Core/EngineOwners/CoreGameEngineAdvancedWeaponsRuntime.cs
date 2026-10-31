#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineAdvancedWeaponsRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDiaoChanLeEntries _characterDiaoChanLeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuCheErMouEntries _characterHuCheErMouOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLiYiOlEntries _characterLiYiOlOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelMatchRandomAccess _randomOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries _coreAdvancedLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCharacterHostEntries _coreCharacterHostOperations;
    private readonly ICoreAdvancedWeaponsCoreEquipmentContentOperations _coreEquipmentContentOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineFieldEquipmentEntries _coreFieldEquipmentOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePlayCardHistoryEntries _corePlayCardHistoryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries _coreProgramEventHistoryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheStartingResourcesEntries _coreShanheStartingResourcesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTableOrderEntries _coreTableOrderOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangShopCardsEntries _coreWushuangShopCardsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalDrawFacadesEntries _corePhysicalDrawFacadesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalMovementFacadesEntries _corePhysicalMovementFacadesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreProgramPipelineHooksEntries _coreProgramPipelineHooksOperations;
    private readonly ICoreAdvancedWeaponsCoreTrustedCardZoneDiagnosticsOperations _coreTrustedCardZoneDiagnosticsOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineAdvancedLifecycleCopiedWeaponAbilitiesInheritedWeaponAbilitiesWeaponCopyConsumedAccess _coreGameEngineAdvancedLifecycleCopiedWeaponAbilitiesInheritedWeaponAbilitiesWeaponCopyConsumedAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineAdvancedWeaponsCopiedWeaponSkillsGeneratedPhysicalCardIdsPermanentlyRemovedWeaponsUsedGeneralWeaponSourcesAccess _coreGameEngineAdvancedWeaponsCopiedWeaponSkillsGeneratedPhysicalCardIdsPermanentlyRemovedWeaponsUsedGeneralWeaponSourcesAccess;
    internal CoreGameEngineAdvancedWeaponsRuntime(global::CardGame.Core.RuntimeEntries.CharacterDiaoChanLeEntries characterDiaoChanLeOperations, global::CardGame.Core.RuntimeEntries.CharacterHuCheErMouEntries characterHuCheErMouOperations, global::CardGame.Core.RuntimeEntries.CharacterLiYiOlEntries characterLiYiOlOperations, global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess cardZonesOperations, global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.EngineOwners.IKernelMatchRandomAccess randomOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries coreAdvancedLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCharacterHostEntries coreCharacterHostOperations, ICoreAdvancedWeaponsCoreEquipmentContentOperations coreEquipmentContentOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineFieldEquipmentEntries coreFieldEquipmentOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePlayCardHistoryEntries corePlayCardHistoryOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries coreProgramEventHistoryOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheStartingResourcesEntries coreShanheStartingResourcesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineTableOrderEntries coreTableOrderOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangShopCardsEntries coreWushuangShopCardsOperations, global::CardGame.Core.RuntimeEntries.CorePhysicalDrawFacadesEntries corePhysicalDrawFacadesOperations, global::CardGame.Core.RuntimeEntries.CorePhysicalMovementFacadesEntries corePhysicalMovementFacadesOperations, global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreProgramPipelineHooksEntries coreProgramPipelineHooksOperations, ICoreAdvancedWeaponsCoreTrustedCardZoneDiagnosticsOperations coreTrustedCardZoneDiagnosticsOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineAdvancedLifecycleCopiedWeaponAbilitiesInheritedWeaponAbilitiesWeaponCopyConsumedAccess coreGameEngineAdvancedLifecycleCopiedWeaponAbilitiesInheritedWeaponAbilitiesWeaponCopyConsumedAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineAdvancedWeaponsCopiedWeaponSkillsGeneratedPhysicalCardIdsPermanentlyRemovedWeaponsUsedGeneralWeaponSourcesAccess coreGameEngineAdvancedWeaponsCopiedWeaponSkillsGeneratedPhysicalCardIdsPermanentlyRemovedWeaponsUsedGeneralWeaponSourcesAccess)
    {
        this._characterDiaoChanLeOperations = characterDiaoChanLeOperations;
        this._characterHuCheErMouOperations = characterHuCheErMouOperations;
        this._characterLiYiOlOperations = characterLiYiOlOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._randomOperations = randomOperations;
        this._coreAdvancedLifecycleOperations = coreAdvancedLifecycleOperations;
        this._coreCharacterHostOperations = coreCharacterHostOperations;
        this._coreEquipmentContentOperations = coreEquipmentContentOperations;
        this._coreFieldEquipmentOperations = coreFieldEquipmentOperations;
        this._corePlayCardHistoryOperations = corePlayCardHistoryOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreShanheStartingResourcesOperations = coreShanheStartingResourcesOperations;
        this._coreTableOrderOperations = coreTableOrderOperations;
        this._coreWushuangShopCardsOperations = coreWushuangShopCardsOperations;
        this._corePhysicalDrawFacadesOperations = corePhysicalDrawFacadesOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreProgramPipelineHooksOperations = coreProgramPipelineHooksOperations;
        this._coreTrustedCardZoneDiagnosticsOperations = coreTrustedCardZoneDiagnosticsOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreGameEngineAdvancedLifecycleCopiedWeaponAbilitiesInheritedWeaponAbilitiesWeaponCopyConsumedAccess = coreGameEngineAdvancedLifecycleCopiedWeaponAbilitiesInheritedWeaponAbilitiesWeaponCopyConsumedAccess;
        this._coreGameEngineAdvancedWeaponsCopiedWeaponSkillsGeneratedPhysicalCardIdsPermanentlyRemovedWeaponsUsedGeneralWeaponSourcesAccess = coreGameEngineAdvancedWeaponsCopiedWeaponSkillsGeneratedPhysicalCardIdsPermanentlyRemovedWeaponsUsedGeneralWeaponSourcesAccess;
    }
}
