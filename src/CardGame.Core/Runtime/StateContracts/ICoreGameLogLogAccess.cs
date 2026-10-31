#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameLogLogAccess : IProgramRuntimeOperations
{
    ref readonly List<GameLogEntry> _log { get; }
}
