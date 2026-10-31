#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineTurnDefaultStatsState : global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineTurnDefaultStatsTurnDefaultStatLedgerStartedTurnDefaultStatStatesAccess
{
    private bool _turnDefaultStatLedgerStarted = default !;
    internal ref bool ReferenceToTurnDefaultStatLedgerStarted => ref _turnDefaultStatLedgerStarted;

    // This is durable gameplay state. Choices and their frozen pool remain only
    // on ProgramSkillFrame; no pending action is retained in this dictionary.
    private global::System.Collections.Generic.Dictionary<global::CardGame.Core.GameEngine.TurnDefaultStatKey, global::CardGame.Core.GameEngine.TurnDefaultStatState> _turnDefaultStatStates = new();
    internal ref readonly global::System.Collections.Generic.Dictionary<global::CardGame.Core.GameEngine.TurnDefaultStatKey, global::CardGame.Core.GameEngine.TurnDefaultStatState> ReferenceToTurnDefaultStatStates => ref _turnDefaultStatStates;

    ref bool global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineTurnDefaultStatsTurnDefaultStatLedgerStartedTurnDefaultStatStatesAccess._turnDefaultStatLedgerStarted => ref _turnDefaultStatLedgerStarted;

    ref readonly global::System.Collections.Generic.Dictionary<global::CardGame.Core.GameEngine.TurnDefaultStatKey, global::CardGame.Core.GameEngine.TurnDefaultStatState> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineTurnDefaultStatsTurnDefaultStatLedgerStartedTurnDefaultStatStatesAccess._turnDefaultStatStates => ref _turnDefaultStatStates;
}
