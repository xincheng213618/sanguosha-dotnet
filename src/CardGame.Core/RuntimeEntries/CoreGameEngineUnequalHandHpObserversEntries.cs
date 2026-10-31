#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineUnequalHandHpObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnequalHandHpObserversRuntime> _resolve;
    private CoreGameEngineUnequalHandHpObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnequalHandHpObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineUnequalHandHpObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnequalHandHpObserversRuntime> resolve) => new(resolve);
}
