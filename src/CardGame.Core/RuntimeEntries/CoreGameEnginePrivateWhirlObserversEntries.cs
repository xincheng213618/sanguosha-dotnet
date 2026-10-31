#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePrivateWhirlObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrivateWhirlObserversRuntime> _resolve;
    private CoreGameEnginePrivateWhirlObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrivateWhirlObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePrivateWhirlObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrivateWhirlObserversRuntime> resolve) => new(resolve);
}
