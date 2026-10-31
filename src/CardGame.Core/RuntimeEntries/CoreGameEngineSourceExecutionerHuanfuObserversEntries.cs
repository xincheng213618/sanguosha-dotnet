#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSourceExecutionerHuanfuObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceExecutionerHuanfuObserversRuntime> _resolve;
    private CoreGameEngineSourceExecutionerHuanfuObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceExecutionerHuanfuObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSourceExecutionerHuanfuObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceExecutionerHuanfuObserversRuntime> resolve) => new(resolve);
}
