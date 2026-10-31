#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreMatchSetupGameStartingProgramsResolvedSetupCompleteAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref bool _setupComplete { get; }

    ref bool _gameStartingProgramsResolved { get; }
}
