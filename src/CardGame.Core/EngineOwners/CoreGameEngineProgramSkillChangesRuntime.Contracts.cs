#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramSkillChangesCoreDyingOwnerSkillReplacementOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDyingOwnerReplacementBinding(global::CardGame.Core.ProgramSkillFrame frame);
    global::System.Collections.Generic.IReadOnlyList<string> AcquireDyingOwnerReplacementSkills(global::CardGame.Core.ProgramSkillFrame frame, global::System.Collections.Generic.IReadOnlyList<string> skillIds);
    global::CardGame.Core.ProgramReplacedPaidHpContinuation? CaptureDyingOwnerReplacementContinuation(global::CardGame.Core.ProgramSkillFrame replacement, global::CardGame.Core.SkillProgramEffect effect);
    void IssueReplacedPaidHpContinuation(global::CardGame.Core.ProgramReplacedPaidHpContinuation continuation);
}

internal interface ICoreProgramSkillChangesCoreSkillProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinishProgramSkill(global::CardGame.Core.ProgramSkillFrame frame, bool completed);
}

internal interface ICoreProgramSkillChangesCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICoreProgramSkillChangesCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<string> EnabledContentSkillIds(global::CardGame.Core.CharacterState player);
    global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player);
}

internal interface ICoreProgramSkillChangesProgramSkillChangeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillChangeRuntime Get();
}

internal interface ICoreProgramSkillChangesCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreProgramSkillChangesCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
}

internal interface ICoreProgramSkillChangesCoreProgramPersistentStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetProgramBooleanState(int ownerSeat, string skillId, string instanceId, string stateId, bool value);
}

internal interface ICoreProgramSkillChangesContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreProgramSkillChangesKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreProgramSkillChangesSkillBindingIndexOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.MatchSkillBindingIndex _skillBindingIndex { get; }
}

internal interface ICoreProgramSkillChangesCoreConversionStatesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RegisterTaggedConversionSkill(global::CardGame.Core.CharacterState player, string skillId);
}
