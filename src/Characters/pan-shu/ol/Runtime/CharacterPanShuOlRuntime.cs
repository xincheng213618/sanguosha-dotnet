#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterPanShuOlRuntime : global::CardGame.Core.IPanShuProgramHost
{
    private readonly ICharacterPanShuOlCharacterBianFuRenOlOperations _characterBianFuRenOlOperations;
    private readonly ICharacterPanShuOlCardZonesOperations _cardZonesOperations;
    private readonly ICharacterPanShuOlContentRegistryOperations _contentRegistryOperations;
    private readonly ICharacterPanShuOlResolutionStackOperations _resolutionStackOperations;
    private readonly ICharacterPanShuOlCoreCardMovementProgramsOperations _coreCardMovementProgramsOperations;
    private readonly ICharacterPanShuOlCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICharacterPanShuOlCoreNativeLogicalDiscardsOperations _coreNativeLogicalDiscardsOperations;
    private readonly ICharacterPanShuOlCorePlayCardHistoryOperations _corePlayCardHistoryOperations;
    private readonly ICharacterPanShuOlCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICharacterPanShuOlCoreProgramMovementSourceCountsOperations _coreProgramMovementSourceCountsOperations;
    private readonly ICharacterPanShuOlCoreProgramPersistentStateOperations _coreProgramPersistentStateOperations;
    private readonly ICharacterPanShuOlCoreProgramTopReorderOperations _coreProgramTopReorderOperations;
    private readonly ICharacterPanShuOlCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICharacterPanShuOlCoreMatchLogOperations _coreMatchLogOperations;
    private readonly ICharacterPanShuOlCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICharacterPanShuOlCoreNativeRecoveryFramesOperations _coreNativeRecoveryFramesOperations;
    private readonly ICharacterPanShuOlCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICharacterPanShuOlCorePhysicalMovementFacadesOperations _corePhysicalMovementFacadesOperations;
    private readonly ICharacterPanShuOlCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICharacterPanShuOlCoreRecoveryReplacementOperations _coreRecoveryReplacementOperations;
    private readonly ICharacterPanShuOlCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICharacterPanShuOlKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseAccess _coreActualTurnProgressCurrentSeatPhaseAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess _coreMatchOutcomeStatusAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineProgramPersistentStateProgramBooleanStatesAccess _coreGameEngineProgramPersistentStateProgramBooleanStatesAccess;
    internal CharacterPanShuOlRuntime(ICharacterPanShuOlCharacterBianFuRenOlOperations characterBianFuRenOlOperations, ICharacterPanShuOlCardZonesOperations cardZonesOperations, ICharacterPanShuOlContentRegistryOperations contentRegistryOperations, ICharacterPanShuOlResolutionStackOperations resolutionStackOperations, ICharacterPanShuOlCoreCardMovementProgramsOperations coreCardMovementProgramsOperations, ICharacterPanShuOlCoreNationalSkillsOperations coreNationalSkillsOperations, ICharacterPanShuOlCoreNativeLogicalDiscardsOperations coreNativeLogicalDiscardsOperations, ICharacterPanShuOlCorePlayCardHistoryOperations corePlayCardHistoryOperations, ICharacterPanShuOlCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICharacterPanShuOlCoreProgramMovementSourceCountsOperations coreProgramMovementSourceCountsOperations, ICharacterPanShuOlCoreProgramPersistentStateOperations coreProgramPersistentStateOperations, ICharacterPanShuOlCoreProgramTopReorderOperations coreProgramTopReorderOperations, ICharacterPanShuOlCoreRuntimeOperations coreRuntimeOperations, ICharacterPanShuOlCoreMatchLogOperations coreMatchLogOperations, ICharacterPanShuOlCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICharacterPanShuOlCoreNativeRecoveryFramesOperations coreNativeRecoveryFramesOperations, ICharacterPanShuOlCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICharacterPanShuOlCorePhysicalMovementFacadesOperations corePhysicalMovementFacadesOperations, ICharacterPanShuOlCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICharacterPanShuOlCoreRecoveryReplacementOperations coreRecoveryReplacementOperations, ICharacterPanShuOlCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICharacterPanShuOlKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseAccess coreActualTurnProgressCurrentSeatPhaseAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess coreMatchOutcomeStatusAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineProgramPersistentStateProgramBooleanStatesAccess coreGameEngineProgramPersistentStateProgramBooleanStatesAccess)
    {
        this._characterBianFuRenOlOperations = characterBianFuRenOlOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreNativeLogicalDiscardsOperations = coreNativeLogicalDiscardsOperations;
        this._corePlayCardHistoryOperations = corePlayCardHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreProgramMovementSourceCountsOperations = coreProgramMovementSourceCountsOperations;
        this._coreProgramPersistentStateOperations = coreProgramPersistentStateOperations;
        this._coreProgramTopReorderOperations = coreProgramTopReorderOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._coreNativeRecoveryFramesOperations = coreNativeRecoveryFramesOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreRecoveryReplacementOperations = coreRecoveryReplacementOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatPhaseAccess = coreActualTurnProgressCurrentSeatPhaseAccess;
        this._coreMatchOutcomeStatusAccess = coreMatchOutcomeStatusAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreGameEngineProgramPersistentStateProgramBooleanStatesAccess = coreGameEngineProgramPersistentStateProgramBooleanStatesAccess;
    }
}
