namespace CardGame.Core;
public sealed record ProgramBooleanStateChangedEvent(int OwnerSeat, string SkillId, string SkillInstanceId, string StateId, bool Value, SkillProgramStateVisibility Visibility) : IGameEvent;
public sealed partial class GameEngine
{
    private ref readonly global::System.Collections.Generic.Dictionary<global::CardGame.Core.GameEngine.ProgramBooleanStateKey, bool> _programBooleanStates => ref _programPersistentState.ReferenceToProgramBooleanStates;

    internal readonly record struct ProgramBooleanStateKey(int OwnerSeat, string SkillId, string SkillInstanceId, string StateId);
}
