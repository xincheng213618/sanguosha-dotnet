#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineProgramSelectionValidationRuntime : global::CardGame.Core.IBlackGiftSelectionQuery, global::CardGame.Core.ICapturedEquipmentPlacementSelectionQuery, global::CardGame.Core.ICategoryRefundSelectionQuery, global::CardGame.Core.IDeferredHandPairSelectionQuery, global::CardGame.Core.IDistinctFactionSelectionQuery, global::CardGame.Core.IDynamicDiscardDamageSelectionQuery, global::CardGame.Core.IEquipmentDonationSelectionQuery, global::CardGame.Core.IEquipmentPlacementSelectionQuery, global::CardGame.Core.IExactOwnedCountSelectionQuery, global::CardGame.Core.IForeignSlashSelectionQuery, global::CardGame.Core.IGameHandHpSelectionQuery, global::CardGame.Core.IMatchingRecastSelectionQuery, global::CardGame.Core.INextActualUseSelectionQuery, global::CardGame.Core.IOwnedRankSumSelectionQuery, global::CardGame.Core.IPairObtainSelectionQuery, global::CardGame.Core.IProvenanceSelectionQuery, global::CardGame.Core.ISelectedActorDuelSelectionQuery, global::CardGame.Core.IWholeHandSuitSelectionQuery
{
    private readonly global::CardGame.Core.EngineOwners.IKernelProgramSelectionValidatorsAccess _programSelectionValidatorsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCapturedEquipmentDrawsEntries _coreCapturedEquipmentDrawsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries _coreCardAttackStateOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardConversionsEntries _coreCardConversionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageColorDuelProgramsEntries _coreDamageColorDuelProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDiscardedProvenanceEntries _coreDiscardedProvenanceOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDistinctFactionDiscardsEntries _coreDistinctFactionDiscardsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDrawDiscardCategoryRefundEntries _coreDrawDiscardCategoryRefundOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDynamicDiscardDamagePaymentsEntries _coreDynamicDiscardDamagePaymentsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineEquipmentDonationProgramsEntries _coreEquipmentDonationProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineEquipmentPlacementAndHpPairEntries _coreEquipmentPlacementAndHpPairOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineForeignSelectedCardSlashEntries _coreForeignSelectedCardSlashOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineFriendTrioEntries _coreFriendTrioOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineGameTargetHandHpChoicesEntries _coreGameTargetHandHpChoicesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineGuanHaiBridgesEntries _coreGuanHaiBridgesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineHalfHandPhaseDebtProgramsEntries _coreHalfHandPhaseDebtProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineHpLossSlashMaterialsEntries _coreHpLossSlashMaterialsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineMatchingRecastsEntries _coreMatchingRecastsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNextActualUseTargetAdjustmentsEntries _coreNextActualUseTargetAdjustmentsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineOwnedExactCountAndNamedTargetsEntries _coreOwnedExactCountAndNamedTargetsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineOwnedHandRankSumProgramsEntries _coreOwnedHandRankSumProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePairObtainShownGiftsEntries _corePairObtainShownGiftsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries _coreProgramCardPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRecipientConsequencesReceiptsEntries _coreRecipientConsequencesReceiptsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeTargetLegalityEntries _coreNativeTargetLegalityOperations;
    private readonly ICoreProgramSelectionValidationCoreProgramSelectionModulesOperations _coreProgramSelectionModulesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineProgramSelectionValidationRuntime(global::CardGame.Core.EngineOwners.IKernelProgramSelectionValidatorsAccess programSelectionValidatorsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCapturedEquipmentDrawsEntries coreCapturedEquipmentDrawsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries coreCardAttackStateOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardConversionsEntries coreCardConversionsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageColorDuelProgramsEntries coreDamageColorDuelProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDiscardedProvenanceEntries coreDiscardedProvenanceOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDistinctFactionDiscardsEntries coreDistinctFactionDiscardsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDrawDiscardCategoryRefundEntries coreDrawDiscardCategoryRefundOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDynamicDiscardDamagePaymentsEntries coreDynamicDiscardDamagePaymentsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineEquipmentDonationProgramsEntries coreEquipmentDonationProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineEquipmentPlacementAndHpPairEntries coreEquipmentPlacementAndHpPairOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineForeignSelectedCardSlashEntries coreForeignSelectedCardSlashOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineFriendTrioEntries coreFriendTrioOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineGameTargetHandHpChoicesEntries coreGameTargetHandHpChoicesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineGuanHaiBridgesEntries coreGuanHaiBridgesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineHalfHandPhaseDebtProgramsEntries coreHalfHandPhaseDebtProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineHpLossSlashMaterialsEntries coreHpLossSlashMaterialsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineMatchingRecastsEntries coreMatchingRecastsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNextActualUseTargetAdjustmentsEntries coreNextActualUseTargetAdjustmentsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineOwnedExactCountAndNamedTargetsEntries coreOwnedExactCountAndNamedTargetsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineOwnedHandRankSumProgramsEntries coreOwnedHandRankSumProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePairObtainShownGiftsEntries corePairObtainShownGiftsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries coreProgramCardPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRecipientConsequencesReceiptsEntries coreRecipientConsequencesReceiptsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeTargetLegalityEntries coreNativeTargetLegalityOperations, ICoreProgramSelectionValidationCoreProgramSelectionModulesOperations coreProgramSelectionModulesOperations, global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._programSelectionValidatorsOperations = programSelectionValidatorsOperations;
        this._coreCapturedEquipmentDrawsOperations = coreCapturedEquipmentDrawsOperations;
        this._coreCardAttackStateOperations = coreCardAttackStateOperations;
        this._coreCardConversionsOperations = coreCardConversionsOperations;
        this._coreDamageColorDuelProgramsOperations = coreDamageColorDuelProgramsOperations;
        this._coreDiscardedProvenanceOperations = coreDiscardedProvenanceOperations;
        this._coreDistinctFactionDiscardsOperations = coreDistinctFactionDiscardsOperations;
        this._coreDrawDiscardCategoryRefundOperations = coreDrawDiscardCategoryRefundOperations;
        this._coreDynamicDiscardDamagePaymentsOperations = coreDynamicDiscardDamagePaymentsOperations;
        this._coreEquipmentDonationProgramsOperations = coreEquipmentDonationProgramsOperations;
        this._coreEquipmentPlacementAndHpPairOperations = coreEquipmentPlacementAndHpPairOperations;
        this._coreForeignSelectedCardSlashOperations = coreForeignSelectedCardSlashOperations;
        this._coreFriendTrioOperations = coreFriendTrioOperations;
        this._coreGameTargetHandHpChoicesOperations = coreGameTargetHandHpChoicesOperations;
        this._coreGuanHaiBridgesOperations = coreGuanHaiBridgesOperations;
        this._coreHalfHandPhaseDebtProgramsOperations = coreHalfHandPhaseDebtProgramsOperations;
        this._coreHpLossSlashMaterialsOperations = coreHpLossSlashMaterialsOperations;
        this._coreMatchingRecastsOperations = coreMatchingRecastsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreNextActualUseTargetAdjustmentsOperations = coreNextActualUseTargetAdjustmentsOperations;
        this._coreOwnedExactCountAndNamedTargetsOperations = coreOwnedExactCountAndNamedTargetsOperations;
        this._coreOwnedHandRankSumProgramsOperations = coreOwnedHandRankSumProgramsOperations;
        this._corePairObtainShownGiftsOperations = corePairObtainShownGiftsOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreRecipientConsequencesReceiptsOperations = coreRecipientConsequencesReceiptsOperations;
        this._coreNativeTargetLegalityOperations = coreNativeTargetLegalityOperations;
        this._coreProgramSelectionModulesOperations = coreProgramSelectionModulesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
