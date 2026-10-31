#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameLogLogSequenceAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref int _logSequence { get; }
}
