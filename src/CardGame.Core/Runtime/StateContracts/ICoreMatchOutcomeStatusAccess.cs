#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreMatchOutcomeStatusAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref global::CardGame.Core.EngineStatus _status { get; }
}
