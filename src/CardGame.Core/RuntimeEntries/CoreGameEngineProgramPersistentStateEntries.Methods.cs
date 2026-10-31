#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramPersistentStateEntries
{
    internal bool GetProgramBooleanState(ProgramSkillFrame frame, string stateId) => _resolve().GetProgramBooleanState(frame, stateId);
    internal bool GetProgramBooleanState2(int ownerSeat, string skillId, string skillInstanceId, string stateId) => _resolve().GetProgramBooleanState(ownerSeat, skillId, skillInstanceId, stateId);
    internal SkillProgramBooleanStateDefinition GetProgramBooleanStateDefinition(ProgramSkillFrame frame, string stateId) => _resolve().GetProgramBooleanStateDefinition(frame, stateId);
    internal SkillProgramBooleanStateDefinition GetProgramBooleanStateDefinition2(string skillId, string stateId) => _resolve().GetProgramBooleanStateDefinition(skillId, stateId);
    internal void SetProgramBooleanState(int ownerSeat, string skillId, string instanceId, string stateId, bool value) => _resolve().SetProgramBooleanState(ownerSeat, skillId, instanceId, stateId, value);
}
