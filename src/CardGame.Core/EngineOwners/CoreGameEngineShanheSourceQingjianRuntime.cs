#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineShanheSourceQingjianRuntime : global::CardGame.Core.IShanheSourceQingjianHost
{
    private readonly ICoreShanheSourceQingjianCardZonesOperations _cardZonesOperations;
    private readonly ICoreShanheSourceQingjianContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreShanheSourceQingjianResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreShanheSourceQingjianCoreAdvancedLifecycleOperations _coreAdvancedLifecycleOperations;
    private readonly ICoreShanheSourceQingjianCoreFireTargetBenefitsOperations _coreFireTargetBenefitsOperations;
    private readonly ICoreShanheSourceQingjianCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICoreShanheSourceQingjianCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreShanheSourceQingjianCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreShanheSourceQingjianCoreSkillProgramsOperations _coreSkillProgramsOperations;
    private readonly ICoreShanheSourceQingjianCoreHappyWeiYanGainDestinationsOperations _coreHappyWeiYanGainDestinationsOperations;
    private readonly ICoreShanheSourceQingjianCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICoreShanheSourceQingjianCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICoreShanheSourceQingjianCoreTeamActivityProgramsOperations _coreTeamActivityProgramsOperations;
    private readonly ICoreShanheSourceQingjianKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess _coreCardMovementJournalMovementSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess _coreMatchOutcomeStatusAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineShanheSourceQingjianRuntime(ICoreShanheSourceQingjianCardZonesOperations cardZonesOperations, ICoreShanheSourceQingjianContentRegistryOperations contentRegistryOperations, ICoreShanheSourceQingjianResolutionStackOperations resolutionStackOperations, ICoreShanheSourceQingjianCoreAdvancedLifecycleOperations coreAdvancedLifecycleOperations, ICoreShanheSourceQingjianCoreFireTargetBenefitsOperations coreFireTargetBenefitsOperations, ICoreShanheSourceQingjianCoreNationalSkillsOperations coreNationalSkillsOperations, ICoreShanheSourceQingjianCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreShanheSourceQingjianCoreRuntimeOperations coreRuntimeOperations, ICoreShanheSourceQingjianCoreSkillProgramsOperations coreSkillProgramsOperations, ICoreShanheSourceQingjianCoreHappyWeiYanGainDestinationsOperations coreHappyWeiYanGainDestinationsOperations, ICoreShanheSourceQingjianCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICoreShanheSourceQingjianCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICoreShanheSourceQingjianCoreTeamActivityProgramsOperations coreTeamActivityProgramsOperations, ICoreShanheSourceQingjianKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess coreCardMovementJournalMovementSequenceAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess coreMatchOutcomeStatusAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAdvancedLifecycleOperations = coreAdvancedLifecycleOperations;
        this._coreFireTargetBenefitsOperations = coreFireTargetBenefitsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._coreHappyWeiYanGainDestinationsOperations = coreHappyWeiYanGainDestinationsOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._coreTeamActivityProgramsOperations = coreTeamActivityProgramsOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreCardMovementJournalMovementSequenceAccess = coreCardMovementJournalMovementSequenceAccess;
        this._coreMatchOutcomeStatusAccess = coreMatchOutcomeStatusAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
