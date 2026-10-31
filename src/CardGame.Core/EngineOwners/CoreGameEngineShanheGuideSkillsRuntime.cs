#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineShanheGuideSkillsRuntime : global::CardGame.Core.IShanheGuideSkillsHost
{
    private readonly ICoreShanheGuideSkillsContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreShanheGuideSkillsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreShanheGuideSkillsSkillRuntimeStateOperations _skillRuntimeStateOperations;
    private readonly ICoreShanheGuideSkillsCoreBoundCardMovementContinuationOperations _coreBoundCardMovementContinuationOperations;
    private readonly ICoreShanheGuideSkillsCoreDamagePreventionProgramsOperations _coreDamagePreventionProgramsOperations;
    private readonly ICoreShanheGuideSkillsCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICoreShanheGuideSkillsCoreProgramAttackAttemptOperations _coreProgramAttackAttemptOperations;
    private readonly ICoreShanheGuideSkillsCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICoreShanheGuideSkillsCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreShanheGuideSkillsCoreProgramNativeDrawOperations _coreProgramNativeDrawOperations;
    private readonly ICoreShanheGuideSkillsCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreShanheGuideSkillsCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICoreShanheGuideSkillsCoreTeamActivityProgramsOperations _coreTeamActivityProgramsOperations;
    private readonly ICoreShanheGuideSkillsKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineShanheGuideSkillsRuntime(ICoreShanheGuideSkillsContentRegistryOperations contentRegistryOperations, ICoreShanheGuideSkillsResolutionStackOperations resolutionStackOperations, ICoreShanheGuideSkillsSkillRuntimeStateOperations skillRuntimeStateOperations, ICoreShanheGuideSkillsCoreBoundCardMovementContinuationOperations coreBoundCardMovementContinuationOperations, ICoreShanheGuideSkillsCoreDamagePreventionProgramsOperations coreDamagePreventionProgramsOperations, ICoreShanheGuideSkillsCoreNationalSkillsOperations coreNationalSkillsOperations, ICoreShanheGuideSkillsCoreProgramAttackAttemptOperations coreProgramAttackAttemptOperations, ICoreShanheGuideSkillsCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICoreShanheGuideSkillsCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreShanheGuideSkillsCoreProgramNativeDrawOperations coreProgramNativeDrawOperations, ICoreShanheGuideSkillsCoreRuntimeOperations coreRuntimeOperations, ICoreShanheGuideSkillsCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICoreShanheGuideSkillsCoreTeamActivityProgramsOperations coreTeamActivityProgramsOperations, ICoreShanheGuideSkillsKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._skillRuntimeStateOperations = skillRuntimeStateOperations;
        this._coreBoundCardMovementContinuationOperations = coreBoundCardMovementContinuationOperations;
        this._coreDamagePreventionProgramsOperations = coreDamagePreventionProgramsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramAttackAttemptOperations = coreProgramAttackAttemptOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreProgramNativeDrawOperations = coreProgramNativeDrawOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreTeamActivityProgramsOperations = coreTeamActivityProgramsOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
