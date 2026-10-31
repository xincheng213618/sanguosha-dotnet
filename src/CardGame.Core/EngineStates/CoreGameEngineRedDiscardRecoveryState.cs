#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineRedDiscardRecoveryState : Contracts.ICoreGameEngineRedDiscardRecoveryTurnRedDiscardCountTurnRedDiscardTurnAccess
{
    private int _turnRedDiscardCount = default !;
    internal ref int ReferenceToTurnRedDiscardCount => ref _turnRedDiscardCount;

    private int _turnRedDiscardTurn = -1;
    internal ref int ReferenceToTurnRedDiscardTurn => ref _turnRedDiscardTurn;

    ref int Contracts.ICoreGameEngineRedDiscardRecoveryTurnRedDiscardCountTurnRedDiscardTurnAccess._turnRedDiscardTurn => ref _turnRedDiscardTurn;

    ref int Contracts.ICoreGameEngineRedDiscardRecoveryTurnRedDiscardCountTurnRedDiscardTurnAccess._turnRedDiscardCount => ref _turnRedDiscardCount;
}
