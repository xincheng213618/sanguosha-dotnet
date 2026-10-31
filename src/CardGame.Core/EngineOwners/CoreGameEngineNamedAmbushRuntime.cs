#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineNamedAmbushRuntime : global::CardGame.Core.INamedAmbushHost
{
    private readonly ICoreNamedAmbushCardZonesOperations _cardZonesOperations;
    private readonly ICoreNamedAmbushContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreNamedAmbushModeRulesOperations _modeRulesOperations;
    private readonly ICoreNamedAmbushResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreNamedAmbushCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICoreNamedAmbushCoreCardAttackStateOperations _coreCardAttackStateOperations;
    private readonly ICoreNamedAmbushCoreCardMovementProgramsOperations _coreCardMovementProgramsOperations;
    private readonly ICoreNamedAmbushCoreCardUseLifecycleOperations _coreCardUseLifecycleOperations;
    private readonly ICoreNamedAmbushCoreCharacterStateProgramsOperations _coreCharacterStateProgramsOperations;
    private readonly ICoreNamedAmbushCoreHpChangeProgramsOperations _coreHpChangeProgramsOperations;
    private readonly ICoreNamedAmbushCoreNamedAmbushInvalidationOperations _coreNamedAmbushInvalidationOperations;
    private readonly ICoreNamedAmbushCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICoreNamedAmbushCorePrivateOfferReturnsOperations _corePrivateOfferReturnsOperations;
    private readonly ICoreNamedAmbushCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreNamedAmbushCoreResponseUseCompletionOperations _coreResponseUseCompletionOperations;
    private readonly ICoreNamedAmbushCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreNamedAmbushCoreSkillOwnershipWindowsOperations _coreSkillOwnershipWindowsOperations;
    private readonly ICoreNamedAmbushCoreSkillProgramsOperations _coreSkillProgramsOperations;
    private readonly ICoreNamedAmbushCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICoreNamedAmbushCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICoreNamedAmbushCorePhysicalMovementFacadesOperations _corePhysicalMovementFacadesOperations;
    private readonly ICoreNamedAmbushCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICoreNamedAmbushCoreRecoveryReplacementOperations _coreRecoveryReplacementOperations;
    private readonly ICoreNamedAmbushCoreSnapshotValueProjectionOperations _coreSnapshotValueProjectionOperations;
    private readonly ICoreNamedAmbushKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatAccess _coreActualTurnProgressCurrentSeatAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess _coreCardMovementJournalCardMovementsMovementSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess _coreMatchOutcomeStatusWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineNamedAmbushRuntime(ICoreNamedAmbushCardZonesOperations cardZonesOperations, ICoreNamedAmbushContentRegistryOperations contentRegistryOperations, ICoreNamedAmbushModeRulesOperations modeRulesOperations, ICoreNamedAmbushResolutionStackOperations resolutionStackOperations, ICoreNamedAmbushCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICoreNamedAmbushCoreCardAttackStateOperations coreCardAttackStateOperations, ICoreNamedAmbushCoreCardMovementProgramsOperations coreCardMovementProgramsOperations, ICoreNamedAmbushCoreCardUseLifecycleOperations coreCardUseLifecycleOperations, ICoreNamedAmbushCoreCharacterStateProgramsOperations coreCharacterStateProgramsOperations, ICoreNamedAmbushCoreHpChangeProgramsOperations coreHpChangeProgramsOperations, ICoreNamedAmbushCoreNamedAmbushInvalidationOperations coreNamedAmbushInvalidationOperations, ICoreNamedAmbushCoreNationalSkillsOperations coreNationalSkillsOperations, ICoreNamedAmbushCorePrivateOfferReturnsOperations corePrivateOfferReturnsOperations, ICoreNamedAmbushCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreNamedAmbushCoreResponseUseCompletionOperations coreResponseUseCompletionOperations, ICoreNamedAmbushCoreRuntimeOperations coreRuntimeOperations, ICoreNamedAmbushCoreSkillOwnershipWindowsOperations coreSkillOwnershipWindowsOperations, ICoreNamedAmbushCoreSkillProgramsOperations coreSkillProgramsOperations, ICoreNamedAmbushCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICoreNamedAmbushCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICoreNamedAmbushCorePhysicalMovementFacadesOperations corePhysicalMovementFacadesOperations, ICoreNamedAmbushCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICoreNamedAmbushCoreRecoveryReplacementOperations coreRecoveryReplacementOperations, ICoreNamedAmbushCoreSnapshotValueProjectionOperations coreSnapshotValueProjectionOperations, ICoreNamedAmbushKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatAccess coreActualTurnProgressCurrentSeatAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess coreCardMovementJournalCardMovementsMovementSequenceAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess coreMatchOutcomeStatusWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._modeRulesOperations = modeRulesOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreCardAttackStateOperations = coreCardAttackStateOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreCharacterStateProgramsOperations = coreCharacterStateProgramsOperations;
        this._coreHpChangeProgramsOperations = coreHpChangeProgramsOperations;
        this._coreNamedAmbushInvalidationOperations = coreNamedAmbushInvalidationOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._corePrivateOfferReturnsOperations = corePrivateOfferReturnsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreResponseUseCompletionOperations = coreResponseUseCompletionOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSkillOwnershipWindowsOperations = coreSkillOwnershipWindowsOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreRecoveryReplacementOperations = coreRecoveryReplacementOperations;
        this._coreSnapshotValueProjectionOperations = coreSnapshotValueProjectionOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatAccess = coreActualTurnProgressCurrentSeatAccess;
        this._coreCardMovementJournalCardMovementsMovementSequenceAccess = coreCardMovementJournalCardMovementsMovementSequenceAccess;
        this._coreMatchOutcomeStatusWinnerAccess = coreMatchOutcomeStatusWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
