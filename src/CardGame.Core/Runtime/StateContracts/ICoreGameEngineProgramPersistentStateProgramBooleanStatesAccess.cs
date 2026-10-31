#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineProgramPersistentStateProgramBooleanStatesAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.Dictionary<global::CardGame.Core.GameEngine.ProgramBooleanStateKey, bool> _programBooleanStates { get; }
}
