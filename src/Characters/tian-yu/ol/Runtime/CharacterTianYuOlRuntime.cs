#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterTianYuOlRuntime : global::CardGame.Core.IDistanceRuleRuntimeQueries, global::CardGame.Core.ITianYuProgramHost
{
    private readonly ICharacterTianYuOlCardZonesOperations _cardZonesOperations;
    private readonly ICharacterTianYuOlContentRegistryOperations _contentRegistryOperations;
    private readonly ICharacterTianYuOlResolutionStackOperations _resolutionStackOperations;
    private readonly ICharacterTianYuOlCoreDirectedTurnCardPoliciesOperations _coreDirectedTurnCardPoliciesOperations;
    private readonly ICharacterTianYuOlCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICharacterTianYuOlCoreCardUseLifecycleOperations _coreCardUseLifecycleOperations;
    private readonly ICharacterTianYuOlCoreCurrentCardEnhancementsOperations _coreCurrentCardEnhancementsOperations;
    private readonly ICharacterTianYuOlCoreDeckEquipmentProgramsOperations _coreDeckEquipmentProgramsOperations;
    private readonly ICharacterTianYuOlCoreEquipmentContentOperations _coreEquipmentContentOperations;
    private readonly ICharacterTianYuOlCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICharacterTianYuOlCoreProgramCardUseRolesOperations _coreProgramCardUseRolesOperations;
    private readonly ICharacterTianYuOlCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICharacterTianYuOlCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICharacterTianYuOlCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICharacterTianYuOlCoreTieredRoundActualTargetWindowsOperations _coreTieredRoundActualTargetWindowsOperations;
    private readonly ICharacterTianYuOlCoreMatchLogOperations _coreMatchLogOperations;
    private readonly ICharacterTianYuOlCoreNativeBorrowedSwordOperations _coreNativeBorrowedSwordOperations;
    private readonly ICharacterTianYuOlCoreNativeDamagePoliciesOperations _coreNativeDamagePoliciesOperations;
    private readonly ICharacterTianYuOlCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICharacterTianYuOlCoreNativeTargetLegalityOperations _coreNativeTargetLegalityOperations;
    private readonly ICharacterTianYuOlCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICharacterTianYuOlCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICharacterTianYuOlCorePlayerViewProjectionOperations _corePlayerViewProjectionOperations;
    private readonly ICharacterTianYuOlCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICharacterTianYuOlKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess _coreActualRoundProgressRoundNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess _coreAiBrainsAiBrainsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionPlayerCountAccess _coreMatchDefinitionPlayerCountAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess _coreMatchOutcomeStatusWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterTianYuOlRuntime(ICharacterTianYuOlCardZonesOperations cardZonesOperations, ICharacterTianYuOlContentRegistryOperations contentRegistryOperations, ICharacterTianYuOlResolutionStackOperations resolutionStackOperations, ICharacterTianYuOlCoreDirectedTurnCardPoliciesOperations coreDirectedTurnCardPoliciesOperations, ICharacterTianYuOlCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICharacterTianYuOlCoreCardUseLifecycleOperations coreCardUseLifecycleOperations, ICharacterTianYuOlCoreCurrentCardEnhancementsOperations coreCurrentCardEnhancementsOperations, ICharacterTianYuOlCoreDeckEquipmentProgramsOperations coreDeckEquipmentProgramsOperations, ICharacterTianYuOlCoreEquipmentContentOperations coreEquipmentContentOperations, ICharacterTianYuOlCoreNationalSkillsOperations coreNationalSkillsOperations, ICharacterTianYuOlCoreProgramCardUseRolesOperations coreProgramCardUseRolesOperations, ICharacterTianYuOlCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICharacterTianYuOlCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICharacterTianYuOlCoreRuntimeOperations coreRuntimeOperations, ICharacterTianYuOlCoreTieredRoundActualTargetWindowsOperations coreTieredRoundActualTargetWindowsOperations, ICharacterTianYuOlCoreMatchLogOperations coreMatchLogOperations, ICharacterTianYuOlCoreNativeBorrowedSwordOperations coreNativeBorrowedSwordOperations, ICharacterTianYuOlCoreNativeDamagePoliciesOperations coreNativeDamagePoliciesOperations, ICharacterTianYuOlCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICharacterTianYuOlCoreNativeTargetLegalityOperations coreNativeTargetLegalityOperations, ICharacterTianYuOlCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICharacterTianYuOlCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICharacterTianYuOlCorePlayerViewProjectionOperations corePlayerViewProjectionOperations, ICharacterTianYuOlCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICharacterTianYuOlKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess coreActualRoundProgressRoundNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess coreAiBrainsAiBrainsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionPlayerCountAccess coreMatchDefinitionPlayerCountAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess coreMatchOutcomeStatusWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.RuntimeAccess.IClosedSaodiSeatingRuntimeSource _closedSaodiSeatingRuntimeSource, global::CardGame.Core.RuntimeAccess.IClosedZhuitaoDistanceRuntimeSource _closedZhuitaoDistanceRuntimeSource)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDirectedTurnCardPoliciesOperations = coreDirectedTurnCardPoliciesOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreCurrentCardEnhancementsOperations = coreCurrentCardEnhancementsOperations;
        this._coreDeckEquipmentProgramsOperations = coreDeckEquipmentProgramsOperations;
        this._coreEquipmentContentOperations = coreEquipmentContentOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramCardUseRolesOperations = coreProgramCardUseRolesOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreTieredRoundActualTargetWindowsOperations = coreTieredRoundActualTargetWindowsOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreNativeBorrowedSwordOperations = coreNativeBorrowedSwordOperations;
        this._coreNativeDamagePoliciesOperations = coreNativeDamagePoliciesOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._coreNativeTargetLegalityOperations = coreNativeTargetLegalityOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._corePlayerViewProjectionOperations = corePlayerViewProjectionOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualRoundProgressRoundNumberAccess = coreActualRoundProgressRoundNumberAccess;
        this._coreAiBrainsAiBrainsAccess = coreAiBrainsAiBrainsAccess;
        this._coreMatchDefinitionPlayerCountAccess = coreMatchDefinitionPlayerCountAccess;
        this._coreMatchOutcomeStatusWinnerAccess = coreMatchOutcomeStatusWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._closedSaodiSeatingRuntimeSource = _closedSaodiSeatingRuntimeSource;
        this._closedZhuitaoDistanceRuntimeSource = _closedZhuitaoDistanceRuntimeSource;
    }

    private readonly global::CardGame.Core.RuntimeAccess.IClosedSaodiSeatingRuntimeSource _closedSaodiSeatingRuntimeSource;
    private readonly global::CardGame.Core.RuntimeAccess.IClosedZhuitaoDistanceRuntimeSource _closedZhuitaoDistanceRuntimeSource;
}
