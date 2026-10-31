#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramPersistentStateRuntime
{
    internal bool GetProgramBooleanState(ProgramSkillFrame frame, string stateId)
    {
        return GetProgramBooleanState(frame.OwnerSeat, frame.SkillId, frame.SkillInstanceId, stateId);
    }

    internal bool GetProgramBooleanState(int ownerSeat, string skillId, string skillInstanceId, string stateId)
    {
        var definition = GetProgramBooleanStateDefinition(skillId, stateId);
        var key = new global::CardGame.Core.GameEngine.ProgramBooleanStateKey(ownerSeat, skillId, skillInstanceId, stateId);
        if (!_coreGameEngineProgramPersistentStateProgramBooleanStatesAccess._programBooleanStates.TryGetValue(key, out var value))
        {
            value = definition.InitialValue;
            _coreGameEngineProgramPersistentStateProgramBooleanStatesAccess._programBooleanStates.Add(key, value);
        }

        return value;
    }

    internal void SetProgramBooleanState(int ownerSeat, string skillId, string instanceId, string stateId, bool value)
    {
        var definition = GetProgramBooleanStateDefinition(skillId, stateId);
        var key = new global::CardGame.Core.GameEngine.ProgramBooleanStateKey(ownerSeat, skillId, instanceId, stateId);
        if (GetProgramBooleanState(ownerSeat, skillId, instanceId, stateId) == value)
            return;
        _coreGameEngineProgramPersistentStateProgramBooleanStatesAccess._programBooleanStates[key] = value;
        if (definition.Visibility == SkillProgramStateVisibility.Public)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramBooleanStateChangedEvent(ownerSeat, skillId, instanceId, stateId, value, definition.Visibility));
    }

    internal SkillProgramBooleanStateDefinition GetProgramBooleanStateDefinition(ProgramSkillFrame frame, string stateId) => GetProgramBooleanStateDefinition(frame.SkillId, stateId);
    internal SkillProgramBooleanStateDefinition GetProgramBooleanStateDefinition(string skillId, string stateId) => _contentRegistryOperations._contentRegistry!.GetSkill(skillId).Program!.BooleanStates.SingleOrDefault(item => item.Id == stateId) ?? throw new InvalidOperationException($"Program boolean state '{(stateId)}' is not declared.");
}
