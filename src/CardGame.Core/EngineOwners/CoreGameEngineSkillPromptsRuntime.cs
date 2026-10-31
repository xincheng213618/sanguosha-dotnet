#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineSkillPromptsRuntime
{
    private readonly ICoreSkillPromptsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreSkillPromptsCorePindianOperations _corePindianOperations;
    private readonly ICoreSkillPromptsCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICoreSkillPromptsKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly ICoreSkillPromptsKernelCoreGameEngineRuntimeOperations _kernelCoreGameEngineRuntimeOperations;
    private readonly ICoreSkillPromptsKernelCoreMainOperations _kernelCoreMainOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess _coreActualTurnProgressTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCommandJournalStorageEventsPendingEventsAccess _coreCommandJournalStorageEventsPendingEventsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionOptionsAccess _coreMatchDefinitionOptionsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineSkillPromptsRuntime(ICoreSkillPromptsResolutionStackOperations resolutionStackOperations, ICoreSkillPromptsCorePindianOperations corePindianOperations, ICoreSkillPromptsCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICoreSkillPromptsKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, ICoreSkillPromptsKernelCoreGameEngineRuntimeOperations kernelCoreGameEngineRuntimeOperations, ICoreSkillPromptsKernelCoreMainOperations kernelCoreMainOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess coreActualTurnProgressTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCommandJournalStorageEventsPendingEventsAccess coreCommandJournalStorageEventsPendingEventsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionOptionsAccess coreMatchDefinitionOptionsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._corePindianOperations = corePindianOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._kernelCoreGameEngineRuntimeOperations = kernelCoreGameEngineRuntimeOperations;
        this._kernelCoreMainOperations = kernelCoreMainOperations;
        this._coreActualTurnProgressTurnNumberAccess = coreActualTurnProgressTurnNumberAccess;
        this._coreCommandJournalStorageEventsPendingEventsAccess = coreCommandJournalStorageEventsPendingEventsAccess;
        this._coreMatchDefinitionOptionsAccess = coreMatchDefinitionOptionsAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
