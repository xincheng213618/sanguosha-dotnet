#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineOriginalHandEntitiesOriginalHandEntityStatesAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.Dictionary<global::CardGame.Core.GameEngine.OriginalHandStateKey, global::CardGame.Core.GameEngine.OriginalHandEntityState> _originalHandEntityStates { get; }
}
