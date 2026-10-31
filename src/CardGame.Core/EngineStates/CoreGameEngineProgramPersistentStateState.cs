#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineProgramPersistentStateState : Contracts.ICoreGameEngineProgramPersistentStateProgramBooleanStatesAccess
{
    private Dictionary<GameEngine.ProgramBooleanStateKey, bool> _programBooleanStates = [];
    internal ref readonly Dictionary<GameEngine.ProgramBooleanStateKey, bool> ReferenceToProgramBooleanStates => ref _programBooleanStates;

    ref readonly Dictionary<GameEngine.ProgramBooleanStateKey, bool> Contracts.ICoreGameEngineProgramPersistentStateProgramBooleanStatesAccess._programBooleanStates => ref _programBooleanStates;
}
