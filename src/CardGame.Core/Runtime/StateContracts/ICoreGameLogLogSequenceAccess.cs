#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameLogLogSequenceAccess : IProgramRuntimeOperations
{
    ref int _logSequence { get; }
}
