#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICharacterNiuJinOlCoreProgramPersistentStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetProgramBooleanState(int ownerSeat, string skillId, string instanceId, string stateId, bool value);
}

internal interface ICharacterNiuJinOlKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}
