#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramLifeRestorationCoreCharacterTurnedOverProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordCharacterTurnedOver(int targetSeat, bool wasFaceDown);
}

internal interface ICoreProgramLifeRestorationKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreProgramLifeRestorationCoreCharacterStateProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordCharacterStateChange(int targetSeat, global::CardGame.Core.SkillProgramTriggerWindow window, long? parentFrameId = default, global::CardGame.Core.CharacterTurnedOverState? turnedOver = default);
}
