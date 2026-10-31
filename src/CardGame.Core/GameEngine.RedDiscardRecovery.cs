namespace CardGame.Core;
/// <summary>
/// Current OL 界孟获 再起: at the owner's own turn ending, up to X other characters
/// (X = red cards put into the discard pile this turn) each choose between drawing
/// one card and recovering the owner by one. The owner-owned draft and the
/// turn-scoped red-discard counter follow the discard-budget / discard-suit-mask
/// precedents: typed state on the owning frame, a public committed fact per
/// selection, and deterministic reconstruction through command replay.
/// </summary>
public sealed partial class GameEngine
{
    private ref int _turnRedDiscardCount => ref _redDiscardRecovery.ReferenceToTurnRedDiscardCount;
    private ref int _turnRedDiscardTurn => ref _redDiscardRecovery.ReferenceToTurnRedDiscardTurn;
}
