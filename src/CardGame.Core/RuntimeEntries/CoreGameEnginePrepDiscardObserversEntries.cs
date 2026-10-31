#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePrepDiscardObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePrepDiscardObserversRuntime> _resolve;
    private CoreGameEnginePrepDiscardObserversEntries(Func<EngineOwners.CoreGameEnginePrepDiscardObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePrepDiscardObserversEntries Create(Func<EngineOwners.CoreGameEnginePrepDiscardObserversRuntime> resolve) => new(resolve);
}
