#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterPangXiMobileShiRuntime : global::CardGame.Core.IShiPangXiHost
{
    private readonly ICharacterPangXiMobileShiCardZonesOperations _cardZonesOperations;
    private readonly ICharacterPangXiMobileShiContentRegistryOperations _contentRegistryOperations;
    private readonly ICharacterPangXiMobileShiResolutionStackOperations _resolutionStackOperations;
    private readonly ICharacterPangXiMobileShiCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICharacterPangXiMobileShiCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICharacterPangXiMobileShiCoreProgramContinuationsOperations _coreProgramContinuationsOperations;
    private readonly ICharacterPangXiMobileShiCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICharacterPangXiMobileShiCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICharacterPangXiMobileShiCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICharacterPangXiMobileShiCoreSkillProgramsOperations _coreSkillProgramsOperations;
    private readonly ICharacterPangXiMobileShiCoreMatchLogOperations _coreMatchLogOperations;
    private readonly ICharacterPangXiMobileShiCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICharacterPangXiMobileShiCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICharacterPangXiMobileShiCorePhysicalMovementFacadesOperations _corePhysicalMovementFacadesOperations;
    private readonly ICharacterPangXiMobileShiCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICharacterPangXiMobileShiKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess _coreActualTurnProgressTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess _coreMatchOutcomeStatusWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterPangXiMobileShiRuntime(ICharacterPangXiMobileShiCardZonesOperations cardZonesOperations, ICharacterPangXiMobileShiContentRegistryOperations contentRegistryOperations, ICharacterPangXiMobileShiResolutionStackOperations resolutionStackOperations, ICharacterPangXiMobileShiCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICharacterPangXiMobileShiCoreNationalSkillsOperations coreNationalSkillsOperations, ICharacterPangXiMobileShiCoreProgramContinuationsOperations coreProgramContinuationsOperations, ICharacterPangXiMobileShiCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICharacterPangXiMobileShiCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICharacterPangXiMobileShiCoreRuntimeOperations coreRuntimeOperations, ICharacterPangXiMobileShiCoreSkillProgramsOperations coreSkillProgramsOperations, ICharacterPangXiMobileShiCoreMatchLogOperations coreMatchLogOperations, ICharacterPangXiMobileShiCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICharacterPangXiMobileShiCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICharacterPangXiMobileShiCorePhysicalMovementFacadesOperations corePhysicalMovementFacadesOperations, ICharacterPangXiMobileShiCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICharacterPangXiMobileShiKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess coreActualTurnProgressTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess coreMatchOutcomeStatusWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramContinuationsOperations = coreProgramContinuationsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressTurnNumberAccess = coreActualTurnProgressTurnNumberAccess;
        this._coreMatchOutcomeStatusWinnerAccess = coreMatchOutcomeStatusWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
