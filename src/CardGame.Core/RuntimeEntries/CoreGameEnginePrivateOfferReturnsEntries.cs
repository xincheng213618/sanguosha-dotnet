#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePrivateOfferReturnsRuntime))]
internal sealed partial class CoreGameEnginePrivateOfferReturnsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrivateOfferReturnsRuntime> _resolve;
    private CoreGameEnginePrivateOfferReturnsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrivateOfferReturnsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePrivateOfferReturnsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrivateOfferReturnsRuntime> resolve) => new(resolve);
}
