#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineProgramSelectionTransformsRuntime : global::CardGame.Core.IDuelSelectionQueries, global::CardGame.Core.IEquipmentPlacementSelectionQueries, global::CardGame.Core.IForeignSlashSelectionQueries, global::CardGame.Core.IMatchingRecastSelectionQueries, global::CardGame.Core.IPairObtainSelectionQueries, global::CardGame.Core.IPindianSelectionQueries, global::CardGame.Core.ISelectionParticipantQueries, global::CardGame.Core.ISelectionUsageQueries, global::CardGame.Core.ISourceCurseSelectionQueries
{
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAppliedDamageBenefitsEntries _coreAppliedDamageBenefitsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCapturedEquipmentDrawsEntries _coreCapturedEquipmentDrawsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageColorDuelProgramsEntries _coreDamageColorDuelProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineEquipmentPlacementAndHpPairEntries _coreEquipmentPlacementAndHpPairOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineForeignSelectedCardSlashEntries _coreForeignSelectedCardSlashOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineMatchingRecastsEntries _coreMatchingRecastsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNextActualUsePindianAiEntries _coreNextActualUsePindianAiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePairObtainShownGiftsEntries _corePairObtainShownGiftsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePindianPoliciesEntries _corePindianPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCategoryTargetLedgerEntries _coreProgramCategoryTargetLedgerOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSourceCursesEntries _coreSourceCursesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreProgramParticipantPhaseOperationsEntries _coreProgramParticipantPhaseOperationsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineProgramSelectionTransformsRuntime(global::CardGame.Core.RuntimeEntries.CoreGameEngineAppliedDamageBenefitsEntries coreAppliedDamageBenefitsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCapturedEquipmentDrawsEntries coreCapturedEquipmentDrawsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageColorDuelProgramsEntries coreDamageColorDuelProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineEquipmentPlacementAndHpPairEntries coreEquipmentPlacementAndHpPairOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineForeignSelectedCardSlashEntries coreForeignSelectedCardSlashOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineMatchingRecastsEntries coreMatchingRecastsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNextActualUsePindianAiEntries coreNextActualUsePindianAiOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePairObtainShownGiftsEntries corePairObtainShownGiftsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePindianPoliciesEntries corePindianPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCategoryTargetLedgerEntries coreProgramCategoryTargetLedgerOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineSourceCursesEntries coreSourceCursesOperations, global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreProgramParticipantPhaseOperationsEntries coreProgramParticipantPhaseOperationsOperations, global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._coreAppliedDamageBenefitsOperations = coreAppliedDamageBenefitsOperations;
        this._coreCapturedEquipmentDrawsOperations = coreCapturedEquipmentDrawsOperations;
        this._coreDamageColorDuelProgramsOperations = coreDamageColorDuelProgramsOperations;
        this._coreEquipmentPlacementAndHpPairOperations = coreEquipmentPlacementAndHpPairOperations;
        this._coreForeignSelectedCardSlashOperations = coreForeignSelectedCardSlashOperations;
        this._coreMatchingRecastsOperations = coreMatchingRecastsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreNextActualUsePindianAiOperations = coreNextActualUsePindianAiOperations;
        this._corePairObtainShownGiftsOperations = corePairObtainShownGiftsOperations;
        this._corePindianPoliciesOperations = corePindianPoliciesOperations;
        this._coreProgramCategoryTargetLedgerOperations = coreProgramCategoryTargetLedgerOperations;
        this._coreSourceCursesOperations = coreSourceCursesOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreProgramParticipantPhaseOperationsOperations = coreProgramParticipantPhaseOperationsOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
