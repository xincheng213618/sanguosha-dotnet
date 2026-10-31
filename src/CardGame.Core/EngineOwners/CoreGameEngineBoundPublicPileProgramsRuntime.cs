#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineBoundPublicPileProgramsRuntime : global::CardGame.Core.IBoundPublicPileProgramHost
{
    private readonly ICoreBoundPublicPileProgramsCardZonesOperations _cardZonesOperations;
    private readonly ICoreBoundPublicPileProgramsContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreBoundPublicPileProgramsCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICoreBoundPublicPileProgramsCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICoreBoundPublicPileProgramsCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreBoundPublicPileProgramsCorePublicPersistentPilesOperations _corePublicPersistentPilesOperations;
    private readonly ICoreBoundPublicPileProgramsCorePublicPileSourcesOperations _corePublicPileSourcesOperations;
    private readonly ICoreBoundPublicPileProgramsCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreBoundPublicPileProgramsCoreSkillProgramsOperations _coreSkillProgramsOperations;
    private readonly ICoreBoundPublicPileProgramsCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICoreBoundPublicPileProgramsKernelCoreGameEngineAdvancedLifecycleHostOperations _kernelCoreGameEngineAdvancedLifecycleHostOperations;
    private readonly ICoreBoundPublicPileProgramsKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineBoundPublicPileProgramsRuntime(ICoreBoundPublicPileProgramsCardZonesOperations cardZonesOperations, ICoreBoundPublicPileProgramsContentRegistryOperations contentRegistryOperations, ICoreBoundPublicPileProgramsCoreNationalSkillsOperations coreNationalSkillsOperations, ICoreBoundPublicPileProgramsCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICoreBoundPublicPileProgramsCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreBoundPublicPileProgramsCorePublicPersistentPilesOperations corePublicPersistentPilesOperations, ICoreBoundPublicPileProgramsCorePublicPileSourcesOperations corePublicPileSourcesOperations, ICoreBoundPublicPileProgramsCoreRuntimeOperations coreRuntimeOperations, ICoreBoundPublicPileProgramsCoreSkillProgramsOperations coreSkillProgramsOperations, ICoreBoundPublicPileProgramsCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICoreBoundPublicPileProgramsKernelCoreGameEngineAdvancedLifecycleHostOperations kernelCoreGameEngineAdvancedLifecycleHostOperations, ICoreBoundPublicPileProgramsKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._corePublicPersistentPilesOperations = corePublicPersistentPilesOperations;
        this._corePublicPileSourcesOperations = corePublicPileSourcesOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._kernelCoreGameEngineAdvancedLifecycleHostOperations = kernelCoreGameEngineAdvancedLifecycleHostOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
