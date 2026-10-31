#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineInspectedHandSlashObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineInspectedHandSlashObserversRuntime> _resolve;
    private CoreGameEngineInspectedHandSlashObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineInspectedHandSlashObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineInspectedHandSlashObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineInspectedHandSlashObserversRuntime> resolve) => new(resolve);
}
