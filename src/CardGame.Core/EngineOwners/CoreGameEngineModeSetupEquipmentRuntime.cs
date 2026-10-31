#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineModeSetupEquipmentRuntime
{
    private readonly ICoreModeSetupEquipmentCardZonesOperations _cardZonesOperations;
    private readonly ICoreModeSetupEquipmentCoreAdvancedWeaponsOperations _coreAdvancedWeaponsOperations;
    private readonly ICoreModeSetupEquipmentCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICoreModeSetupEquipmentCoreModeSetupOperations _coreModeSetupOperations;
    private readonly ICoreModeSetupEquipmentCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICoreModeSetupEquipmentKernelCoreGameEngineModeSetupEquipmentOperations _kernelCoreGameEngineModeSetupEquipmentOperations;
    private readonly ICoreModeSetupEquipmentKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess _coreActualTurnProgressTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupSetupCompleteAccess _coreMatchSetupSetupCompleteAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICorePhysicalCardPopulationInitialCardCountAccess _corePhysicalCardPopulationInitialCardCountAccess;
    internal CoreGameEngineModeSetupEquipmentRuntime(ICoreModeSetupEquipmentCardZonesOperations cardZonesOperations, ICoreModeSetupEquipmentCoreAdvancedWeaponsOperations coreAdvancedWeaponsOperations, ICoreModeSetupEquipmentCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICoreModeSetupEquipmentCoreModeSetupOperations coreModeSetupOperations, ICoreModeSetupEquipmentCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICoreModeSetupEquipmentKernelCoreGameEngineModeSetupEquipmentOperations kernelCoreGameEngineModeSetupEquipmentOperations, ICoreModeSetupEquipmentKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess coreActualTurnProgressTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupSetupCompleteAccess coreMatchSetupSetupCompleteAccess, global::CardGame.Core.EngineStates.Contracts.ICorePhysicalCardPopulationInitialCardCountAccess corePhysicalCardPopulationInitialCardCountAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._coreAdvancedWeaponsOperations = coreAdvancedWeaponsOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreModeSetupOperations = coreModeSetupOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineModeSetupEquipmentOperations = kernelCoreGameEngineModeSetupEquipmentOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressTurnNumberAccess = coreActualTurnProgressTurnNumberAccess;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreMatchSetupSetupCompleteAccess = coreMatchSetupSetupCompleteAccess;
        this._corePhysicalCardPopulationInitialCardCountAccess = corePhysicalCardPopulationInitialCardCountAccess;
    }
}
