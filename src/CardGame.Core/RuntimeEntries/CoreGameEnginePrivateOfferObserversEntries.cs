#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePrivateOfferObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrivateOfferObserversRuntime> _resolve;
    private CoreGameEnginePrivateOfferObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrivateOfferObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePrivateOfferObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrivateOfferObserversRuntime> resolve) => new(resolve);
}
