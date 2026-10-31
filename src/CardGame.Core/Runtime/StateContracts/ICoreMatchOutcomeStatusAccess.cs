#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreMatchOutcomeStatusAccess : IProgramRuntimeOperations
{
    ref EngineStatus _status { get; }
}
