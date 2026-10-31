#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineProgramPersistentStateProgramBooleanStatesAccess : IProgramRuntimeOperations
{
    ref readonly Dictionary<GameEngine.ProgramBooleanStateKey, bool> _programBooleanStates { get; }
}
