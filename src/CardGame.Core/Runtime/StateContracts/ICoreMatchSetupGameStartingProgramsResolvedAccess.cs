#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreMatchSetupGameStartingProgramsResolvedAccess : IProgramRuntimeOperations
{
    ref bool _gameStartingProgramsResolved { get; }
}
