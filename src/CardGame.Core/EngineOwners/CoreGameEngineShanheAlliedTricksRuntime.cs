#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineShanheAlliedTricksRuntime
{
    private readonly ICoreShanheAlliedTricksResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreShanheAlliedTricksCoreGroupTargetNullificationOperations _coreGroupTargetNullificationOperations;
    private readonly ICoreShanheAlliedTricksCoreProgramCardPoliciesOperations _coreProgramCardPoliciesOperations;
    private readonly ICoreShanheAlliedTricksCoreShanheGuideSkillsOperations _coreShanheGuideSkillsOperations;
    private readonly ICoreShanheAlliedTricksCoreNativeCardContinuationCursorsOperations _coreNativeCardContinuationCursorsOperations;
    private readonly ICoreShanheAlliedTricksCoreNativeHarvestOperations _coreNativeHarvestOperations;
    private readonly ICoreShanheAlliedTricksCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICoreShanheAlliedTricksCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess _coreMatchOutcomeStatusAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineShanheAlliedTricksRuntime(ICoreShanheAlliedTricksResolutionStackOperations resolutionStackOperations, ICoreShanheAlliedTricksCoreGroupTargetNullificationOperations coreGroupTargetNullificationOperations, ICoreShanheAlliedTricksCoreProgramCardPoliciesOperations coreProgramCardPoliciesOperations, ICoreShanheAlliedTricksCoreShanheGuideSkillsOperations coreShanheGuideSkillsOperations, ICoreShanheAlliedTricksCoreNativeCardContinuationCursorsOperations coreNativeCardContinuationCursorsOperations, ICoreShanheAlliedTricksCoreNativeHarvestOperations coreNativeHarvestOperations, ICoreShanheAlliedTricksCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICoreShanheAlliedTricksCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess coreMatchOutcomeStatusAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreGroupTargetNullificationOperations = coreGroupTargetNullificationOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreShanheGuideSkillsOperations = coreShanheGuideSkillsOperations;
        this._coreNativeCardContinuationCursorsOperations = coreNativeCardContinuationCursorsOperations;
        this._coreNativeHarvestOperations = coreNativeHarvestOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._coreMatchOutcomeStatusAccess = coreMatchOutcomeStatusAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
