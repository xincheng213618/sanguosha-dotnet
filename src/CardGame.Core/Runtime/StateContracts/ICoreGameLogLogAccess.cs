#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameLogLogAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.GameLogEntry> _log { get; }
}
