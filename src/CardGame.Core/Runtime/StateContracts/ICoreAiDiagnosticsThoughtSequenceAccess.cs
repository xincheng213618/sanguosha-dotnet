#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreAiDiagnosticsThoughtSequenceAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref int _thoughtSequence { get; }
}
