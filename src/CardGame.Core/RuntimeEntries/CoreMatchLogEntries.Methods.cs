#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreMatchLogEntries
{
    internal void AddLog(string type, string message, int? actor = default, int? target = default) => _resolve().AddLog(type, message, actor, target);
    internal void FlushNotifications(global::CardGame.Core.GameSnapshot preparedSnapshot) => _resolve().FlushNotifications(preparedSnapshot);
}
