#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineTurnDefaultStatsTurnDefaultStatLedgerStartedTurnDefaultStatStatesAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref bool _turnDefaultStatLedgerStarted { get; }

    ref readonly global::System.Collections.Generic.Dictionary<global::CardGame.Core.GameEngine.TurnDefaultStatKey, global::CardGame.Core.GameEngine.TurnDefaultStatState> _turnDefaultStatStates { get; }
}
