#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class DingShangWanNativeOperations(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAtomicCardMovesEntries> getCoreGameEngineProgramAtomicCardMovesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHuangQuanMouEntries> getCharacterHuangQuanMouRuntime, global::CardGame.Core.EngineStates.CoreCardMovementJournalState cardMovementJournalState) : IDingShangWanNativeOperations
{
    public void Give(long frameId, int giver, int recipient, IReadOnlyList<int> cards, CardMoveReason reason) => getCoreGameEngineProgramAtomicCardMovesRuntime().MoveProgramCardsFromMultipleSources(cards, CardLocation.Hand(recipient), reason, getCharacterHuangQuanMouRuntime().NativeGiveCompletionRecords(frameId, giver, recipient, cards, cardMovementJournalState.ReferenceToMovementSequence, null));
}
