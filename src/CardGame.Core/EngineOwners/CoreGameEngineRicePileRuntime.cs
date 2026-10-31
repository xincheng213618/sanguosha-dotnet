#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineRicePileRuntime : global::CardGame.Core.IRicePileProgramHost
{
    private readonly ICoreRicePileCardZonesOperations _cardZonesOperations;
    private readonly ICoreRicePileContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreRicePileResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreRicePileCoreAdvancedLifecycleOperations _coreAdvancedLifecycleOperations;
    private readonly ICoreRicePileCoreCardMovementProgramsOperations _coreCardMovementProgramsOperations;
    private readonly ICoreRicePileCoreCharacterStateProgramsOperations _coreCharacterStateProgramsOperations;
    private readonly ICoreRicePileCoreHpChangeProgramsOperations _coreHpChangeProgramsOperations;
    private readonly ICoreRicePileCoreProgramAtomicCardMovesOperations _coreProgramAtomicCardMovesOperations;
    private readonly ICoreRicePileCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreRicePileCorePublicPileCashOutOperations _corePublicPileCashOutOperations;
    private readonly ICoreRicePileCorePublicPileSourcesOperations _corePublicPileSourcesOperations;
    private readonly ICoreRicePileCoreRiceJudgmentOperations _coreRiceJudgmentOperations;
    private readonly ICoreRicePileCoreRicePileReceiptsOperations _coreRicePileReceiptsOperations;
    private readonly ICoreRicePileCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreRicePileCoreSkillOwnershipWindowsOperations _coreSkillOwnershipWindowsOperations;
    private readonly ICoreRicePileCoreSkillProgramsOperations _coreSkillProgramsOperations;
    private readonly ICoreRicePileCoreNativeDamageHandlesOperations _coreNativeDamageHandlesOperations;
    private readonly ICoreRicePileCoreNativeJudgmentOperations _coreNativeJudgmentOperations;
    private readonly ICoreRicePileCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICoreRicePileCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICoreRicePileCorePhysicalDrawFacadesOperations _corePhysicalDrawFacadesOperations;
    private readonly ICoreRicePileCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICoreRicePileCoreRecoveryReplacementOperations _coreRecoveryReplacementOperations;
    private readonly ICoreRicePileKernelCoreGameEngineAdvancedLifecycleHostOperations _kernelCoreGameEngineAdvancedLifecycleHostOperations;
    private readonly ICoreRicePileKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess _coreCardMovementJournalMovementSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess _coreMatchOutcomeStatusAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEnginePublicPileSourcesPublicPersistentPilesAccess _coreGameEnginePublicPileSourcesPublicPersistentPilesAccess;
    internal CoreGameEngineRicePileRuntime(ICoreRicePileCardZonesOperations cardZonesOperations, ICoreRicePileContentRegistryOperations contentRegistryOperations, ICoreRicePileResolutionStackOperations resolutionStackOperations, ICoreRicePileCoreAdvancedLifecycleOperations coreAdvancedLifecycleOperations, ICoreRicePileCoreCardMovementProgramsOperations coreCardMovementProgramsOperations, ICoreRicePileCoreCharacterStateProgramsOperations coreCharacterStateProgramsOperations, ICoreRicePileCoreHpChangeProgramsOperations coreHpChangeProgramsOperations, ICoreRicePileCoreProgramAtomicCardMovesOperations coreProgramAtomicCardMovesOperations, ICoreRicePileCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreRicePileCorePublicPileCashOutOperations corePublicPileCashOutOperations, ICoreRicePileCorePublicPileSourcesOperations corePublicPileSourcesOperations, ICoreRicePileCoreRiceJudgmentOperations coreRiceJudgmentOperations, ICoreRicePileCoreRicePileReceiptsOperations coreRicePileReceiptsOperations, ICoreRicePileCoreRuntimeOperations coreRuntimeOperations, ICoreRicePileCoreSkillOwnershipWindowsOperations coreSkillOwnershipWindowsOperations, ICoreRicePileCoreSkillProgramsOperations coreSkillProgramsOperations, ICoreRicePileCoreNativeDamageHandlesOperations coreNativeDamageHandlesOperations, ICoreRicePileCoreNativeJudgmentOperations coreNativeJudgmentOperations, ICoreRicePileCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICoreRicePileCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICoreRicePileCorePhysicalDrawFacadesOperations corePhysicalDrawFacadesOperations, ICoreRicePileCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICoreRicePileCoreRecoveryReplacementOperations coreRecoveryReplacementOperations, ICoreRicePileKernelCoreGameEngineAdvancedLifecycleHostOperations kernelCoreGameEngineAdvancedLifecycleHostOperations, ICoreRicePileKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess coreCardMovementJournalMovementSequenceAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess coreMatchOutcomeStatusAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEnginePublicPileSourcesPublicPersistentPilesAccess coreGameEnginePublicPileSourcesPublicPersistentPilesAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAdvancedLifecycleOperations = coreAdvancedLifecycleOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreCharacterStateProgramsOperations = coreCharacterStateProgramsOperations;
        this._coreHpChangeProgramsOperations = coreHpChangeProgramsOperations;
        this._coreProgramAtomicCardMovesOperations = coreProgramAtomicCardMovesOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._corePublicPileCashOutOperations = corePublicPileCashOutOperations;
        this._corePublicPileSourcesOperations = corePublicPileSourcesOperations;
        this._coreRiceJudgmentOperations = coreRiceJudgmentOperations;
        this._coreRicePileReceiptsOperations = coreRicePileReceiptsOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSkillOwnershipWindowsOperations = coreSkillOwnershipWindowsOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._coreNativeDamageHandlesOperations = coreNativeDamageHandlesOperations;
        this._coreNativeJudgmentOperations = coreNativeJudgmentOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalDrawFacadesOperations = corePhysicalDrawFacadesOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreRecoveryReplacementOperations = coreRecoveryReplacementOperations;
        this._kernelCoreGameEngineAdvancedLifecycleHostOperations = kernelCoreGameEngineAdvancedLifecycleHostOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
        this._coreCardMovementJournalMovementSequenceAccess = coreCardMovementJournalMovementSequenceAccess;
        this._coreMatchOutcomeStatusAccess = coreMatchOutcomeStatusAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreGameEnginePublicPileSourcesPublicPersistentPilesAccess = coreGameEnginePublicPileSourcesPublicPersistentPilesAccess;
    }
}
