#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineRedDiscardRecoveryTurnRedDiscardCountTurnRedDiscardTurnAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref int _turnRedDiscardTurn { get; }

    ref int _turnRedDiscardCount { get; }
}
