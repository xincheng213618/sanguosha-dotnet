#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameLogLogLogSequenceAccess : IProgramRuntimeOperations
{
    ref int _logSequence { get; }

    ref readonly List<GameLogEntry> _log { get; }
}
