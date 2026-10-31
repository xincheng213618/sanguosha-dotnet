#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEnginePaidHpLossProgramsRuntime
{
    private readonly ICorePaidHpLossProgramsContentRegistryOperations _contentRegistryOperations;
    private readonly ICorePaidHpLossProgramsResolutionStackOperations _resolutionStackOperations;
    private readonly ICorePaidHpLossProgramsSkillRuntimeStateOperations _skillRuntimeStateOperations;
    private readonly ICorePaidHpLossProgramsTurnCardUseEffectsOperations _turnCardUseEffectsOperations;
    private readonly ICorePaidHpLossProgramsCoreCardUseModulesOperations _coreCardUseModulesOperations;
    private readonly ICorePaidHpLossProgramsCoreHpChangeProgramsOperations _coreHpChangeProgramsOperations;
    private readonly ICorePaidHpLossProgramsCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICorePaidHpLossProgramsCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICorePaidHpLossProgramsCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICorePaidHpLossProgramsCoreSkillProgramsOperations _coreSkillProgramsOperations;
    private readonly ICorePaidHpLossProgramsCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICorePaidHpLossProgramsCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICorePaidHpLossProgramsKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess _coreMatchOutcomeStatusAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEnginePaidHpLossProgramsRuntime(ICorePaidHpLossProgramsContentRegistryOperations contentRegistryOperations, ICorePaidHpLossProgramsResolutionStackOperations resolutionStackOperations, ICorePaidHpLossProgramsSkillRuntimeStateOperations skillRuntimeStateOperations, ICorePaidHpLossProgramsTurnCardUseEffectsOperations turnCardUseEffectsOperations, ICorePaidHpLossProgramsCoreCardUseModulesOperations coreCardUseModulesOperations, ICorePaidHpLossProgramsCoreHpChangeProgramsOperations coreHpChangeProgramsOperations, ICorePaidHpLossProgramsCoreNationalSkillsOperations coreNationalSkillsOperations, ICorePaidHpLossProgramsCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICorePaidHpLossProgramsCoreRuntimeOperations coreRuntimeOperations, ICorePaidHpLossProgramsCoreSkillProgramsOperations coreSkillProgramsOperations, ICorePaidHpLossProgramsCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICorePaidHpLossProgramsCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICorePaidHpLossProgramsKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess coreMatchOutcomeStatusAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._skillRuntimeStateOperations = skillRuntimeStateOperations;
        this._turnCardUseEffectsOperations = turnCardUseEffectsOperations;
        this._coreCardUseModulesOperations = coreCardUseModulesOperations;
        this._coreHpChangeProgramsOperations = coreHpChangeProgramsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
        this._coreMatchOutcomeStatusAccess = coreMatchOutcomeStatusAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
