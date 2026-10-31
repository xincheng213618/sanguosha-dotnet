#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineTurnDefaultStatsState : Contracts.ICoreGameEngineTurnDefaultStatsTurnDefaultStatLedgerStartedTurnDefaultStatStatesAccess
{
    private bool _turnDefaultStatLedgerStarted = default !;
    internal ref bool ReferenceToTurnDefaultStatLedgerStarted => ref _turnDefaultStatLedgerStarted;

    // This is durable gameplay state. Choices and their frozen pool remain only
    // on ProgramSkillFrame; no pending action is retained in this dictionary.
    private Dictionary<GameEngine.TurnDefaultStatKey, GameEngine.TurnDefaultStatState> _turnDefaultStatStates = new();
    internal ref readonly Dictionary<GameEngine.TurnDefaultStatKey, GameEngine.TurnDefaultStatState> ReferenceToTurnDefaultStatStates => ref _turnDefaultStatStates;

    ref bool Contracts.ICoreGameEngineTurnDefaultStatsTurnDefaultStatLedgerStartedTurnDefaultStatStatesAccess._turnDefaultStatLedgerStarted => ref _turnDefaultStatLedgerStarted;

    ref readonly Dictionary<GameEngine.TurnDefaultStatKey, GameEngine.TurnDefaultStatState> Contracts.ICoreGameEngineTurnDefaultStatsTurnDefaultStatLedgerStartedTurnDefaultStatStatesAccess._turnDefaultStatStates => ref _turnDefaultStatStates;
}
