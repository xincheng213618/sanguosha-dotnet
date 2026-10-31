#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class PhysicalCardDrawLogs(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries> getCoreMatchLogRuntime) : IPhysicalCardDrawLogs
{
    public void Add(string type, string message, int? actor = null) => getCoreMatchLogRuntime().AddLog(type, message, actor);
}
