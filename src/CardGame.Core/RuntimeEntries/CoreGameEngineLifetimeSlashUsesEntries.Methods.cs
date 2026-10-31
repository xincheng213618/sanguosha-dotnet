#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineLifetimeSlashUsesEntries
{
    internal int LifetimeSlashUseCount(int seat) => _resolve().LifetimeSlashUseCount(seat);
}
