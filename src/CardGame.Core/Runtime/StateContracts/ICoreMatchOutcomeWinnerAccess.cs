#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreMatchOutcomeWinnerAccess : IProgramRuntimeOperations
{
    ref Winner _winner { get; }
}
