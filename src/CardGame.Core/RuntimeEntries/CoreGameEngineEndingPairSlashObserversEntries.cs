#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineEndingPairSlashObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineEndingPairSlashObserversRuntime> _resolve;
    private CoreGameEngineEndingPairSlashObserversEntries(Func<EngineOwners.CoreGameEngineEndingPairSlashObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineEndingPairSlashObserversEntries Create(Func<EngineOwners.CoreGameEngineEndingPairSlashObserversRuntime> resolve) => new(resolve);
}
