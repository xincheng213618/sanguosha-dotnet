#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSharedSlashOffersRuntime))]
internal sealed partial class CoreGameEngineSharedSlashOffersEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSharedSlashOffersRuntime> _resolve;
    private CoreGameEngineSharedSlashOffersEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSharedSlashOffersRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSharedSlashOffersEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSharedSlashOffersRuntime> resolve) => new(resolve);
}
