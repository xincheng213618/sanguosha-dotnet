#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineProgramPersistentStateState : global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineProgramPersistentStateProgramBooleanStatesAccess
{
    private global::System.Collections.Generic.Dictionary<global::CardGame.Core.GameEngine.ProgramBooleanStateKey, bool> _programBooleanStates = [];
    internal ref readonly global::System.Collections.Generic.Dictionary<global::CardGame.Core.GameEngine.ProgramBooleanStateKey, bool> ReferenceToProgramBooleanStates => ref _programBooleanStates;

    ref readonly global::System.Collections.Generic.Dictionary<global::CardGame.Core.GameEngine.ProgramBooleanStateKey, bool> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineProgramPersistentStateProgramBooleanStatesAccess._programBooleanStates => ref _programBooleanStates;
}
