#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSharedSlashOffersEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSharedSlashOffersRuntime> _resolve;
    private CoreGameEngineSharedSlashOffersEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSharedSlashOffersRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSharedSlashOffersEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSharedSlashOffersRuntime> resolve) => new(resolve);
}
