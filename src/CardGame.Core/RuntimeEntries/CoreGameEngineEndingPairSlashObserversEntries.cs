#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineEndingPairSlashObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEndingPairSlashObserversRuntime> _resolve;
    private CoreGameEngineEndingPairSlashObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEndingPairSlashObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineEndingPairSlashObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEndingPairSlashObserversRuntime> resolve) => new(resolve);
}
