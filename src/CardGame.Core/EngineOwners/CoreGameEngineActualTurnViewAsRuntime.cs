#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineActualTurnViewAsRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelSkillRuntimeStateAccess _skillRuntimeStateOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePlayCardHistoryEntries _corePlayCardHistoryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    internal CoreGameEngineActualTurnViewAsRuntime(global::CardGame.Core.EngineOwners.IKernelSkillRuntimeStateAccess skillRuntimeStateOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePlayCardHistoryEntries corePlayCardHistoryOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations)
    {
        this._skillRuntimeStateOperations = skillRuntimeStateOperations;
        this._corePlayCardHistoryOperations = corePlayCardHistoryOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
    }
}
