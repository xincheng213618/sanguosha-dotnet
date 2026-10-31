#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameLogLogLogSequenceAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref int _logSequence { get; }

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.GameLogEntry> _log { get; }
}
