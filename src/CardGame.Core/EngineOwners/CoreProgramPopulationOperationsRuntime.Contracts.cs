#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramPopulationOperationsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ChangeProgramMaximumHp(global::CardGame.Core.ProgramSkillFrame frame, int amount, string? sourceSkillId = default);
}

internal interface ICoreProgramPopulationOperationsCoreRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetLivingFactionCount();
}

internal interface ICoreProgramPopulationOperationsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}
