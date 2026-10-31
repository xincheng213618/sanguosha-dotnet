#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreCommandJournalStoragePendingNotificationsAccess : IProgramRuntimeOperations
{
    ref readonly Queue<EngineNotification> _pendingNotifications { get; }
}
