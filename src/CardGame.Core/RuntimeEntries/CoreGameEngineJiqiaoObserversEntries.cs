#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineJiqiaoObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJiqiaoObserversRuntime> _resolve;
    private CoreGameEngineJiqiaoObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJiqiaoObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineJiqiaoObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJiqiaoObserversRuntime> resolve) => new(resolve);
}
