#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramAbolishAreasOperationCharacterLiuHongOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveTuxingAfterSlotAbolish(global::CardGame.Core.ProgramSkillFrame frame, int recipientSeat, int abolishedSlotCount);
}

internal interface ICoreProgramAbolishAreasOperationKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}
