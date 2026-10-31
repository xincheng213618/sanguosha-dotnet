#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreMatchOutcomeStatusWinnerAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref global::CardGame.Core.EngineStatus _status { get; }

    ref global::CardGame.Core.Winner _winner { get; }
}
