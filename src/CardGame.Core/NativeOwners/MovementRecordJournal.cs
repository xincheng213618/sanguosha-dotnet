#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MovementRecordJournal(EngineStates.CoreCardMovementJournalState cardMovementJournalState) : IMovementRecordJournal
{
    public int NextSequence() => ++cardMovementJournalState.ReferenceToMovementSequence;
    public void Append(CardMovementRecord movement) => cardMovementJournalState.ReferenceToCardMovements.Add(movement);
}
