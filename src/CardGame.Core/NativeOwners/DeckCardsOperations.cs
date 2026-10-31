#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class DeckCardsOperations(global::System.Func<global::CardGame.Core.RuntimeEntries.CorePhysicalDrawFacadesEntries> getCorePhysicalDrawFacadesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAtomicCardMovesEntries> getCoreGameEngineProgramAtomicCardMovesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineBoundCardMovementContinuationEntries> getCoreGameEngineBoundCardMovementContinuationRuntime) : IProgramDeckCardsOperations
{
    public bool EnsureDrawPile() => getCorePhysicalDrawFacadesRuntime().EnsureDrawPile();
    public void MoveCards(IReadOnlyList<int> ids, CardLocation destination, CardMoveReason reason, Action<long, IReadOnlyList<CardMovementRecord>> afterPhysicalRecords) => getCoreGameEngineProgramAtomicCardMovesRuntime().MoveProgramCardsFromMultipleSources(ids, destination, reason, afterPhysicalRecords);
    public SkillProgramStepOutcome AwaitMovements(long frameId, int ownerSeat) => getCoreGameEngineBoundCardMovementContinuationRuntime().AwaitProgramBoundCardMovements(frameId, ownerSeat);
}
