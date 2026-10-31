#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreMatchSetupGameStartingProgramsResolvedSetupCompleteAccess : IProgramRuntimeOperations
{
    ref bool _setupComplete { get; }

    ref bool _gameStartingProgramsResolved { get; }
}
