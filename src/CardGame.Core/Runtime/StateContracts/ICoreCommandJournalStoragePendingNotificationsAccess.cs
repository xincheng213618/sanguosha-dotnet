#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreCommandJournalStoragePendingNotificationsAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.Queue<global::CardGame.Core.EngineNotification> _pendingNotifications { get; }
}
