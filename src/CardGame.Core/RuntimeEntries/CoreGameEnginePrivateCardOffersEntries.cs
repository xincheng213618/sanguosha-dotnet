#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePrivateCardOffersRuntime))]
internal sealed partial class CoreGameEnginePrivateCardOffersEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrivateCardOffersRuntime> _resolve;
    private CoreGameEnginePrivateCardOffersEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrivateCardOffersRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePrivateCardOffersEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrivateCardOffersRuntime> resolve) => new(resolve);
}
