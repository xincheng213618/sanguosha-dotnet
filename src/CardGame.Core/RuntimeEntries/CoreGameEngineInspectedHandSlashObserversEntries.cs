#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineInspectedHandSlashObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineInspectedHandSlashObserversRuntime> _resolve;
    private CoreGameEngineInspectedHandSlashObserversEntries(Func<EngineOwners.CoreGameEngineInspectedHandSlashObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineInspectedHandSlashObserversEntries Create(Func<EngineOwners.CoreGameEngineInspectedHandSlashObserversRuntime> resolve) => new(resolve);
}
