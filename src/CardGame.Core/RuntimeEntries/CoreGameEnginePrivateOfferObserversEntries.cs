#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePrivateOfferObserversRuntime))]
internal sealed partial class CoreGameEnginePrivateOfferObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePrivateOfferObserversRuntime> _resolve;
    private CoreGameEnginePrivateOfferObserversEntries(Func<EngineOwners.CoreGameEnginePrivateOfferObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePrivateOfferObserversEntries Create(Func<EngineOwners.CoreGameEnginePrivateOfferObserversRuntime> resolve) => new(resolve);
}
