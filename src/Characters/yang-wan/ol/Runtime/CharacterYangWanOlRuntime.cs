#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterYangWanOlRuntime : global::CardGame.Core.IYangWanProgramHost
{
    private readonly ICharacterYangWanOlCardZonesOperations _cardZonesOperations;
    private readonly ICharacterYangWanOlContentRegistryOperations _contentRegistryOperations;
    private readonly ICharacterYangWanOlRandomOperations _randomOperations;
    private readonly ICharacterYangWanOlResolutionStackOperations _resolutionStackOperations;
    private readonly ICharacterYangWanOlSkillRuntimeStateOperations _skillRuntimeStateOperations;
    private readonly ICharacterYangWanOlCoreActualDiscardRecoveryPhasesOperations _coreActualDiscardRecoveryPhasesOperations;
    private readonly ICharacterYangWanOlCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICharacterYangWanOlCoreCardMovementProgramsOperations _coreCardMovementProgramsOperations;
    private readonly ICharacterYangWanOlCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICharacterYangWanOlCoreNativeLogicalDiscardsOperations _coreNativeLogicalDiscardsOperations;
    private readonly ICharacterYangWanOlCoreProgramDiscardOriginsOperations _coreProgramDiscardOriginsOperations;
    private readonly ICharacterYangWanOlCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICharacterYangWanOlCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICharacterYangWanOlCoreSkillProgramsOperations _coreSkillProgramsOperations;
    private readonly ICharacterYangWanOlCoreMatchLogOperations _coreMatchLogOperations;
    private readonly ICharacterYangWanOlCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICharacterYangWanOlCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICharacterYangWanOlCorePhysicalMovementFacadesOperations _corePhysicalMovementFacadesOperations;
    private readonly ICharacterYangWanOlCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICharacterYangWanOlCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICharacterYangWanOlKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess _coreMatchOutcomeStatusWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICharacterYangWanOlZhuihuanArmChoicesAccess _characterYangWanOlZhuihuanArmChoicesAccess;
    internal CharacterYangWanOlRuntime(ICharacterYangWanOlCardZonesOperations cardZonesOperations, ICharacterYangWanOlContentRegistryOperations contentRegistryOperations, ICharacterYangWanOlRandomOperations randomOperations, ICharacterYangWanOlResolutionStackOperations resolutionStackOperations, ICharacterYangWanOlSkillRuntimeStateOperations skillRuntimeStateOperations, ICharacterYangWanOlCoreActualDiscardRecoveryPhasesOperations coreActualDiscardRecoveryPhasesOperations, ICharacterYangWanOlCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICharacterYangWanOlCoreCardMovementProgramsOperations coreCardMovementProgramsOperations, ICharacterYangWanOlCoreNationalSkillsOperations coreNationalSkillsOperations, ICharacterYangWanOlCoreNativeLogicalDiscardsOperations coreNativeLogicalDiscardsOperations, ICharacterYangWanOlCoreProgramDiscardOriginsOperations coreProgramDiscardOriginsOperations, ICharacterYangWanOlCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICharacterYangWanOlCoreRuntimeOperations coreRuntimeOperations, ICharacterYangWanOlCoreSkillProgramsOperations coreSkillProgramsOperations, ICharacterYangWanOlCoreMatchLogOperations coreMatchLogOperations, ICharacterYangWanOlCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICharacterYangWanOlCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICharacterYangWanOlCorePhysicalMovementFacadesOperations corePhysicalMovementFacadesOperations, ICharacterYangWanOlCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICharacterYangWanOlCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICharacterYangWanOlKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess coreMatchOutcomeStatusWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICharacterYangWanOlZhuihuanArmChoicesAccess characterYangWanOlZhuihuanArmChoicesAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._randomOperations = randomOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._skillRuntimeStateOperations = skillRuntimeStateOperations;
        this._coreActualDiscardRecoveryPhasesOperations = coreActualDiscardRecoveryPhasesOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreNativeLogicalDiscardsOperations = coreNativeLogicalDiscardsOperations;
        this._coreProgramDiscardOriginsOperations = coreProgramDiscardOriginsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
        this._coreMatchOutcomeStatusWinnerAccess = coreMatchOutcomeStatusWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._characterYangWanOlZhuihuanArmChoicesAccess = characterYangWanOlZhuihuanArmChoicesAccess;
    }
}
