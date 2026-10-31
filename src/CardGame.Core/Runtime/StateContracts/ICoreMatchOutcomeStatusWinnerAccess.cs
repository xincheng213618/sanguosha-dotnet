#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreMatchOutcomeStatusWinnerAccess : IProgramRuntimeOperations
{
    ref EngineStatus _status { get; }

    ref Winner _winner { get; }
}
