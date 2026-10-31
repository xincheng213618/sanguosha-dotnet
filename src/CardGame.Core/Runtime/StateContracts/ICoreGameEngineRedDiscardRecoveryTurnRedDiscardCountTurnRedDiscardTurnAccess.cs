#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineRedDiscardRecoveryTurnRedDiscardCountTurnRedDiscardTurnAccess : IProgramRuntimeOperations
{
    ref int _turnRedDiscardTurn { get; }

    ref int _turnRedDiscardCount { get; }
}
