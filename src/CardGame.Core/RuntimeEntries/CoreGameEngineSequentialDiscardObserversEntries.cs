#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSequentialDiscardObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineSequentialDiscardObserversRuntime> _resolve;
    private CoreGameEngineSequentialDiscardObserversEntries(Func<EngineOwners.CoreGameEngineSequentialDiscardObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSequentialDiscardObserversEntries Create(Func<EngineOwners.CoreGameEngineSequentialDiscardObserversRuntime> resolve) => new(resolve);
}
