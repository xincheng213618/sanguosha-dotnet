#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterLvYiMobileShiRuntime : global::CardGame.Core.IShiLvYiHost
{
    private readonly ICharacterLvYiMobileShiCardZonesOperations _cardZonesOperations;
    private readonly ICharacterLvYiMobileShiContentRegistryOperations _contentRegistryOperations;
    private readonly ICharacterLvYiMobileShiRandomOperations _randomOperations;
    private readonly ICharacterLvYiMobileShiResolutionStackOperations _resolutionStackOperations;
    private readonly ICharacterLvYiMobileShiCoreAdvancedLifecycleOperations _coreAdvancedLifecycleOperations;
    private readonly ICharacterLvYiMobileShiCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICharacterLvYiMobileShiCoreCardUseLifecycleOperations _coreCardUseLifecycleOperations;
    private readonly ICharacterLvYiMobileShiCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICharacterLvYiMobileShiCoreProgramContinuationsOperations _coreProgramContinuationsOperations;
    private readonly ICharacterLvYiMobileShiCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICharacterLvYiMobileShiCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICharacterLvYiMobileShiCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICharacterLvYiMobileShiCoreSkillProgramsOperations _coreSkillProgramsOperations;
    private readonly ICharacterLvYiMobileShiCoreMatchLogOperations _coreMatchLogOperations;
    private readonly ICharacterLvYiMobileShiCoreNativeCardAppearanceOperations _coreNativeCardAppearanceOperations;
    private readonly ICharacterLvYiMobileShiCoreNativeCardUseOperations _coreNativeCardUseOperations;
    private readonly ICharacterLvYiMobileShiCoreNativeNullificationOperations _coreNativeNullificationOperations;
    private readonly ICharacterLvYiMobileShiCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICharacterLvYiMobileShiCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICharacterLvYiMobileShiCorePhysicalDrawFacadesOperations _corePhysicalDrawFacadesOperations;
    private readonly ICharacterLvYiMobileShiCorePhysicalMovementFacadesOperations _corePhysicalMovementFacadesOperations;
    private readonly ICharacterLvYiMobileShiCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICharacterLvYiMobileShiKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess _coreActualRoundProgressRoundNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess _coreMatchOutcomeStatusWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterLvYiMobileShiRuntime(ICharacterLvYiMobileShiCardZonesOperations cardZonesOperations, ICharacterLvYiMobileShiContentRegistryOperations contentRegistryOperations, ICharacterLvYiMobileShiRandomOperations randomOperations, ICharacterLvYiMobileShiResolutionStackOperations resolutionStackOperations, ICharacterLvYiMobileShiCoreAdvancedLifecycleOperations coreAdvancedLifecycleOperations, ICharacterLvYiMobileShiCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICharacterLvYiMobileShiCoreCardUseLifecycleOperations coreCardUseLifecycleOperations, ICharacterLvYiMobileShiCoreNationalSkillsOperations coreNationalSkillsOperations, ICharacterLvYiMobileShiCoreProgramContinuationsOperations coreProgramContinuationsOperations, ICharacterLvYiMobileShiCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICharacterLvYiMobileShiCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICharacterLvYiMobileShiCoreRuntimeOperations coreRuntimeOperations, ICharacterLvYiMobileShiCoreSkillProgramsOperations coreSkillProgramsOperations, ICharacterLvYiMobileShiCoreMatchLogOperations coreMatchLogOperations, ICharacterLvYiMobileShiCoreNativeCardAppearanceOperations coreNativeCardAppearanceOperations, ICharacterLvYiMobileShiCoreNativeCardUseOperations coreNativeCardUseOperations, ICharacterLvYiMobileShiCoreNativeNullificationOperations coreNativeNullificationOperations, ICharacterLvYiMobileShiCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICharacterLvYiMobileShiCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICharacterLvYiMobileShiCorePhysicalDrawFacadesOperations corePhysicalDrawFacadesOperations, ICharacterLvYiMobileShiCorePhysicalMovementFacadesOperations corePhysicalMovementFacadesOperations, ICharacterLvYiMobileShiCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICharacterLvYiMobileShiKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess coreActualRoundProgressRoundNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess coreMatchOutcomeStatusWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._randomOperations = randomOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAdvancedLifecycleOperations = coreAdvancedLifecycleOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramContinuationsOperations = coreProgramContinuationsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreNativeCardAppearanceOperations = coreNativeCardAppearanceOperations;
        this._coreNativeCardUseOperations = coreNativeCardUseOperations;
        this._coreNativeNullificationOperations = coreNativeNullificationOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalDrawFacadesOperations = corePhysicalDrawFacadesOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualRoundProgressRoundNumberAccess = coreActualRoundProgressRoundNumberAccess;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
        this._coreMatchOutcomeStatusWinnerAccess = coreMatchOutcomeStatusWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
