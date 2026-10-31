#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreAiBrainsAiBrainsAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.Dictionary<int, global::CardGame.Core.SimpleAiBrain> _aiBrains { get; }
}
