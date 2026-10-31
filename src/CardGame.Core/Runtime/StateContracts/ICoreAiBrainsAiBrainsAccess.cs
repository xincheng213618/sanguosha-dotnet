#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreAiBrainsAiBrainsAccess : IProgramRuntimeOperations
{
    ref readonly Dictionary<int, SimpleAiBrain> _aiBrains { get; }
}
