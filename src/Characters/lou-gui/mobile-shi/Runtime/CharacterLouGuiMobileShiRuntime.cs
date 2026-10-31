#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterLouGuiMobileShiRuntime : global::CardGame.Core.IShiLouGuiHost
{
    private readonly ICharacterLouGuiMobileShiCardZonesOperations _cardZonesOperations;
    private readonly ICharacterLouGuiMobileShiContentRegistryOperations _contentRegistryOperations;
    private readonly ICharacterLouGuiMobileShiRandomOperations _randomOperations;
    private readonly ICharacterLouGuiMobileShiResolutionStackOperations _resolutionStackOperations;
    private readonly ICharacterLouGuiMobileShiCoreProgramContinuationsOperations _coreProgramContinuationsOperations;
    private readonly ICharacterLouGuiMobileShiCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICharacterLouGuiMobileShiCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICharacterLouGuiMobileShiCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICharacterLouGuiMobileShiCoreSkillProgramsOperations _coreSkillProgramsOperations;
    private readonly ICharacterLouGuiMobileShiCoreMatchLogOperations _coreMatchLogOperations;
    private readonly ICharacterLouGuiMobileShiCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICharacterLouGuiMobileShiCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICharacterLouGuiMobileShiCorePhysicalMovementFacadesOperations _corePhysicalMovementFacadesOperations;
    private readonly ICharacterLouGuiMobileShiCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICharacterLouGuiMobileShiKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess _coreActualTurnProgressTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess _coreMatchOutcomeStatusWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess;
    internal CharacterLouGuiMobileShiRuntime(ICharacterLouGuiMobileShiCardZonesOperations cardZonesOperations, ICharacterLouGuiMobileShiContentRegistryOperations contentRegistryOperations, ICharacterLouGuiMobileShiRandomOperations randomOperations, ICharacterLouGuiMobileShiResolutionStackOperations resolutionStackOperations, ICharacterLouGuiMobileShiCoreProgramContinuationsOperations coreProgramContinuationsOperations, ICharacterLouGuiMobileShiCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICharacterLouGuiMobileShiCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICharacterLouGuiMobileShiCoreRuntimeOperations coreRuntimeOperations, ICharacterLouGuiMobileShiCoreSkillProgramsOperations coreSkillProgramsOperations, ICharacterLouGuiMobileShiCoreMatchLogOperations coreMatchLogOperations, ICharacterLouGuiMobileShiCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICharacterLouGuiMobileShiCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICharacterLouGuiMobileShiCorePhysicalMovementFacadesOperations corePhysicalMovementFacadesOperations, ICharacterLouGuiMobileShiCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICharacterLouGuiMobileShiKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess coreActualTurnProgressTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess coreMatchOutcomeStatusWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._randomOperations = randomOperations;
        this._resolutionStackOperations = resolutionStackOperations;
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
        this._coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess = coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess;
    }
}
