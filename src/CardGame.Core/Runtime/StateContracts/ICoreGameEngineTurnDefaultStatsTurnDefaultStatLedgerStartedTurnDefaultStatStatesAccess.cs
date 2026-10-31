#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineTurnDefaultStatsTurnDefaultStatLedgerStartedTurnDefaultStatStatesAccess : IProgramRuntimeOperations
{
    ref bool _turnDefaultStatLedgerStarted { get; }

    ref readonly Dictionary<GameEngine.TurnDefaultStatKey, GameEngine.TurnDefaultStatState> _turnDefaultStatStates { get; }
}
