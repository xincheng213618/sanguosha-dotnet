#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineOriginalHandEntitiesOriginalHandEntityStatesAccess : IProgramRuntimeOperations
{
    ref readonly Dictionary<GameEngine.OriginalHandStateKey, GameEngine.OriginalHandEntityState> _originalHandEntityStates { get; }
}
