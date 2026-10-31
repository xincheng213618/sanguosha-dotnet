#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineGuanduEquipmentRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreForeignEquipmentDiscardPoliciesEntries _coreForeignEquipmentDiscardPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedWeaponsEntries _coreAdvancedWeaponsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries _coreCardUseLifecycleOperations;
    private readonly ICoreGuanduEquipmentCoreEquipmentContentOperations _coreEquipmentContentOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineFieldEquipmentEntries _coreFieldEquipmentOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineGuanduGeneralsEntries _coreGuanduGeneralsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNativeLogicalDiscardsEntries _coreNativeLogicalDiscardsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries _coreProgramEventHistoryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineAdvancedWeaponsGeneratedPhysicalCardIdsAccess _coreGameEngineAdvancedWeaponsGeneratedPhysicalCardIdsAccess;
    internal CoreGameEngineGuanduEquipmentRuntime(global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess cardZonesOperations, global::CardGame.Core.RuntimeEntries.CoreForeignEquipmentDiscardPoliciesEntries coreForeignEquipmentDiscardPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedWeaponsEntries coreAdvancedWeaponsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries coreCardUseLifecycleOperations, ICoreGuanduEquipmentCoreEquipmentContentOperations coreEquipmentContentOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineFieldEquipmentEntries coreFieldEquipmentOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineGuanduGeneralsEntries coreGuanduGeneralsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNativeLogicalDiscardsEntries coreNativeLogicalDiscardsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries coreProgramEventHistoryOperations, global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineAdvancedWeaponsGeneratedPhysicalCardIdsAccess coreGameEngineAdvancedWeaponsGeneratedPhysicalCardIdsAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._coreForeignEquipmentDiscardPoliciesOperations = coreForeignEquipmentDiscardPoliciesOperations;
        this._coreAdvancedWeaponsOperations = coreAdvancedWeaponsOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreEquipmentContentOperations = coreEquipmentContentOperations;
        this._coreFieldEquipmentOperations = coreFieldEquipmentOperations;
        this._coreGuanduGeneralsOperations = coreGuanduGeneralsOperations;
        this._coreNativeLogicalDiscardsOperations = coreNativeLogicalDiscardsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreGameEngineAdvancedWeaponsGeneratedPhysicalCardIdsAccess = coreGameEngineAdvancedWeaponsGeneratedPhysicalCardIdsAccess;
    }
}
