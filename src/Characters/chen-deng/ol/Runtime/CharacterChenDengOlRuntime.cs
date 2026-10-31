#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterChenDengOlRuntime : global::CardGame.Core.IChenDengProgramHost
{
    private readonly ICharacterChenDengOlContentRegistryOperations _contentRegistryOperations;
    private readonly ICharacterChenDengOlResolutionStackOperations _resolutionStackOperations;
    private readonly ICharacterChenDengOlCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICharacterChenDengOlCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICharacterChenDengOlCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICharacterChenDengOlCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICharacterChenDengOlCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICharacterChenDengOlCoreMatchLogOperations _coreMatchLogOperations;
    private readonly ICharacterChenDengOlCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICharacterChenDengOlCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICharacterChenDengOlCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICharacterChenDengOlKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess _coreActualRoundProgressRoundNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnProgressionAccess _coreActualTurnProgressCurrentSeatTurnProgressionAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess _coreMatchOutcomeStatusWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterChenDengOlRuntime(ICharacterChenDengOlContentRegistryOperations contentRegistryOperations, ICharacterChenDengOlResolutionStackOperations resolutionStackOperations, ICharacterChenDengOlCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICharacterChenDengOlCoreNationalSkillsOperations coreNationalSkillsOperations, ICharacterChenDengOlCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICharacterChenDengOlCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICharacterChenDengOlCoreRuntimeOperations coreRuntimeOperations, ICharacterChenDengOlCoreMatchLogOperations coreMatchLogOperations, ICharacterChenDengOlCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICharacterChenDengOlCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICharacterChenDengOlCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICharacterChenDengOlKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess coreActualRoundProgressRoundNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnProgressionAccess coreActualTurnProgressCurrentSeatTurnProgressionAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess coreMatchOutcomeStatusWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualRoundProgressRoundNumberAccess = coreActualRoundProgressRoundNumberAccess;
        this._coreActualTurnProgressCurrentSeatTurnProgressionAccess = coreActualTurnProgressCurrentSeatTurnProgressionAccess;
        this._coreMatchOutcomeStatusWinnerAccess = coreMatchOutcomeStatusWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
