#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreMatchOutcomeWinnerAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref global::CardGame.Core.Winner _winner { get; }
}
