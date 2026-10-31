#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineDeathGrowthSkillsRuntime : global::CardGame.Core.IDeathGrowthSkillHost
{
    private readonly ICoreDeathGrowthSkillsContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreDeathGrowthSkillsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreDeathGrowthSkillsCoreCardMovementProgramsOperations _coreCardMovementProgramsOperations;
    private readonly ICoreDeathGrowthSkillsCoreDeathGrowthSkillObserversOperations _coreDeathGrowthSkillObserversOperations;
    private readonly ICoreDeathGrowthSkillsCoreHpChangeProgramsOperations _coreHpChangeProgramsOperations;
    private readonly ICoreDeathGrowthSkillsCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreDeathGrowthSkillsCoreProgramSkillChangesOperations _coreProgramSkillChangesOperations;
    private readonly ICoreDeathGrowthSkillsCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreDeathGrowthSkillsCoreSkillProgramsOperations _coreSkillProgramsOperations;
    private readonly ICoreDeathGrowthSkillsCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICoreDeathGrowthSkillsCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICoreDeathGrowthSkillsCorePhysicalDrawFacadesOperations _corePhysicalDrawFacadesOperations;
    private readonly ICoreDeathGrowthSkillsCoreRecoveryReplacementOperations _coreRecoveryReplacementOperations;
    private readonly ICoreDeathGrowthSkillsCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICoreDeathGrowthSkillsKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess _coreCardMovementJournalMovementSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess _coreMatchOutcomeStatusAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineDeathGrowthSkillsRuntime(ICoreDeathGrowthSkillsContentRegistryOperations contentRegistryOperations, ICoreDeathGrowthSkillsResolutionStackOperations resolutionStackOperations, ICoreDeathGrowthSkillsCoreCardMovementProgramsOperations coreCardMovementProgramsOperations, ICoreDeathGrowthSkillsCoreDeathGrowthSkillObserversOperations coreDeathGrowthSkillObserversOperations, ICoreDeathGrowthSkillsCoreHpChangeProgramsOperations coreHpChangeProgramsOperations, ICoreDeathGrowthSkillsCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreDeathGrowthSkillsCoreProgramSkillChangesOperations coreProgramSkillChangesOperations, ICoreDeathGrowthSkillsCoreRuntimeOperations coreRuntimeOperations, ICoreDeathGrowthSkillsCoreSkillProgramsOperations coreSkillProgramsOperations, ICoreDeathGrowthSkillsCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICoreDeathGrowthSkillsCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICoreDeathGrowthSkillsCorePhysicalDrawFacadesOperations corePhysicalDrawFacadesOperations, ICoreDeathGrowthSkillsCoreRecoveryReplacementOperations coreRecoveryReplacementOperations, ICoreDeathGrowthSkillsCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICoreDeathGrowthSkillsKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess coreCardMovementJournalMovementSequenceAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess coreMatchOutcomeStatusAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreDeathGrowthSkillObserversOperations = coreDeathGrowthSkillObserversOperations;
        this._coreHpChangeProgramsOperations = coreHpChangeProgramsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreProgramSkillChangesOperations = coreProgramSkillChangesOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalDrawFacadesOperations = corePhysicalDrawFacadesOperations;
        this._coreRecoveryReplacementOperations = coreRecoveryReplacementOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreCardMovementJournalMovementSequenceAccess = coreCardMovementJournalMovementSequenceAccess;
        this._coreMatchOutcomeStatusAccess = coreMatchOutcomeStatusAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
