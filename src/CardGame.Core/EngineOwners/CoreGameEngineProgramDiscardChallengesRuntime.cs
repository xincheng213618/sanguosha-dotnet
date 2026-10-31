#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineProgramDiscardChallengesRuntime : global::CardGame.Core.IDiscardChallengeProgramHost
{
    private readonly ICoreProgramDiscardChallengesCardZonesOperations _cardZonesOperations;
    private readonly ICoreProgramDiscardChallengesContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreProgramDiscardChallengesResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreProgramDiscardChallengesCoreCardMovementProgramsOperations _coreCardMovementProgramsOperations;
    private readonly ICoreProgramDiscardChallengesCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICoreProgramDiscardChallengesCoreNativeLogicalDiscardsOperations _coreNativeLogicalDiscardsOperations;
    private readonly ICoreProgramDiscardChallengesCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreProgramDiscardChallengesCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreProgramDiscardChallengesCoreSkillProgramsOperations _coreSkillProgramsOperations;
    private readonly ICoreProgramDiscardChallengesCoreTableOrderOperations _coreTableOrderOperations;
    private readonly ICoreProgramDiscardChallengesCoreTurnHandCategoryRestrictionsOperations _coreTurnHandCategoryRestrictionsOperations;
    private readonly ICoreProgramDiscardChallengesCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICoreProgramDiscardChallengesCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICoreProgramDiscardChallengesCorePhysicalMovementFacadesOperations _corePhysicalMovementFacadesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionPlayerCountAccess _coreMatchDefinitionPlayerCountAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess _coreMatchOutcomeStatusAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineProgramDiscardChallengesRuntime(ICoreProgramDiscardChallengesCardZonesOperations cardZonesOperations, ICoreProgramDiscardChallengesContentRegistryOperations contentRegistryOperations, ICoreProgramDiscardChallengesResolutionStackOperations resolutionStackOperations, ICoreProgramDiscardChallengesCoreCardMovementProgramsOperations coreCardMovementProgramsOperations, ICoreProgramDiscardChallengesCoreNationalSkillsOperations coreNationalSkillsOperations, ICoreProgramDiscardChallengesCoreNativeLogicalDiscardsOperations coreNativeLogicalDiscardsOperations, ICoreProgramDiscardChallengesCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreProgramDiscardChallengesCoreRuntimeOperations coreRuntimeOperations, ICoreProgramDiscardChallengesCoreSkillProgramsOperations coreSkillProgramsOperations, ICoreProgramDiscardChallengesCoreTableOrderOperations coreTableOrderOperations, ICoreProgramDiscardChallengesCoreTurnHandCategoryRestrictionsOperations coreTurnHandCategoryRestrictionsOperations, ICoreProgramDiscardChallengesCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICoreProgramDiscardChallengesCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICoreProgramDiscardChallengesCorePhysicalMovementFacadesOperations corePhysicalMovementFacadesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionPlayerCountAccess coreMatchDefinitionPlayerCountAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess coreMatchOutcomeStatusAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreNativeLogicalDiscardsOperations = coreNativeLogicalDiscardsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._coreTableOrderOperations = coreTableOrderOperations;
        this._coreTurnHandCategoryRestrictionsOperations = coreTurnHandCategoryRestrictionsOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._coreMatchDefinitionPlayerCountAccess = coreMatchDefinitionPlayerCountAccess;
        this._coreMatchOutcomeStatusAccess = coreMatchOutcomeStatusAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
