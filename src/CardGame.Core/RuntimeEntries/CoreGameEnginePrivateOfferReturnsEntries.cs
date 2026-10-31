#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePrivateOfferReturnsRuntime))]
internal sealed partial class CoreGameEnginePrivateOfferReturnsEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePrivateOfferReturnsRuntime> _resolve;
    private CoreGameEnginePrivateOfferReturnsEntries(Func<EngineOwners.CoreGameEnginePrivateOfferReturnsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePrivateOfferReturnsEntries Create(Func<EngineOwners.CoreGameEnginePrivateOfferReturnsRuntime> resolve) => new(resolve);
}
