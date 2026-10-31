#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MovementRecordNotifications(EngineStates.CoreCommandJournalStorageState commandJournalStorageState) : IMovementRecordNotifications
{
    public void Enqueue(CardMovementRecord movement) => commandJournalStorageState.ReferenceToPendingNotifications.Enqueue(new CardMovedNotification(movement));
}
