#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineActualTurnViewAsRuntime
{
    private readonly ICoreActualTurnViewAsSkillRuntimeStateOperations _skillRuntimeStateOperations;
    private readonly ICoreActualTurnViewAsCorePlayCardHistoryOperations _corePlayCardHistoryOperations;
    private readonly ICoreActualTurnViewAsKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    internal CoreGameEngineActualTurnViewAsRuntime(ICoreActualTurnViewAsSkillRuntimeStateOperations skillRuntimeStateOperations, ICoreActualTurnViewAsCorePlayCardHistoryOperations corePlayCardHistoryOperations, ICoreActualTurnViewAsKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations)
    {
        this._skillRuntimeStateOperations = skillRuntimeStateOperations;
        this._corePlayCardHistoryOperations = corePlayCardHistoryOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
    }
}
