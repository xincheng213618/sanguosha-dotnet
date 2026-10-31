#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreAiDiagnosticsThoughtSequenceAccess : IProgramRuntimeOperations
{
    ref int _thoughtSequence { get; }
}
