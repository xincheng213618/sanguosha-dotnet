#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineOwnedHandRankSumProgramsRuntime : global::CardGame.Core.IOwnedHandRankSumHost
{
    private readonly ICoreOwnedHandRankSumProgramsCharacterGuanYuWumiaoOperations _characterGuanYuWumiaoOperations;
    private readonly ICoreOwnedHandRankSumProgramsCardZonesOperations _cardZonesOperations;
    private readonly ICoreOwnedHandRankSumProgramsContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreOwnedHandRankSumProgramsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreOwnedHandRankSumProgramsCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICoreOwnedHandRankSumProgramsCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreOwnedHandRankSumProgramsCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreOwnedHandRankSumProgramsCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICoreOwnedHandRankSumProgramsCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICoreOwnedHandRankSumProgramsCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess _coreMatchOutcomeStatusAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineOwnedHandRankSumProgramsRuntime(ICoreOwnedHandRankSumProgramsCharacterGuanYuWumiaoOperations characterGuanYuWumiaoOperations, ICoreOwnedHandRankSumProgramsCardZonesOperations cardZonesOperations, ICoreOwnedHandRankSumProgramsContentRegistryOperations contentRegistryOperations, ICoreOwnedHandRankSumProgramsResolutionStackOperations resolutionStackOperations, ICoreOwnedHandRankSumProgramsCoreNationalSkillsOperations coreNationalSkillsOperations, ICoreOwnedHandRankSumProgramsCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreOwnedHandRankSumProgramsCoreRuntimeOperations coreRuntimeOperations, ICoreOwnedHandRankSumProgramsCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICoreOwnedHandRankSumProgramsCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICoreOwnedHandRankSumProgramsCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess coreMatchOutcomeStatusAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterGuanYuWumiaoOperations = characterGuanYuWumiaoOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreMatchOutcomeStatusAccess = coreMatchOutcomeStatusAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
