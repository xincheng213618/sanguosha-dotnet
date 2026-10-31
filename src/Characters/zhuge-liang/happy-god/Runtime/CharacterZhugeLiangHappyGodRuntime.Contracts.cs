#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICharacterZhugeLiangHappyGodCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICharacterZhugeLiangHappyGodCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICharacterZhugeLiangHappyGodCoreProgramSkillChangesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetProgramBooleanState(global::CardGame.Core.ProgramSkillFrame frame, string stateId, bool value);
}

internal interface ICharacterZhugeLiangHappyGodCoreNativeCardClassificationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DamageNature GetDamageNature(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICharacterZhugeLiangHappyGodCoreProgramPersistentStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool GetProgramBooleanState(int ownerSeat, string skillId, string skillInstanceId, string stateId);
}

internal interface ICharacterZhugeLiangHappyGodCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
    global::CardGame.Core.ProgramSkillCardSetBinding GetProgramCardSet(global::CardGame.Core.ProgramSkillFrame frame, string bind);
}

internal interface ICharacterZhugeLiangHappyGodContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICharacterZhugeLiangHappyGodKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICharacterZhugeLiangHappyGodCoreProgramBooleanStateExpirationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ScheduleProgramBooleanStateExpiration(global::CardGame.Core.ProgramSkillFrame frame, global::System.Collections.Generic.IReadOnlyList<string> stateIds, global::CardGame.Core.SkillProgramDamageModifierExpiration expiration = (global::CardGame.Core.SkillProgramDamageModifierExpiration)0);
}
