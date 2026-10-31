#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineSharedSlashOffersRuntime))]
internal sealed partial class CoreGameEngineSharedSlashOffersEntries
{
    private readonly Func<EngineOwners.CoreGameEngineSharedSlashOffersRuntime> _resolve;
    private CoreGameEngineSharedSlashOffersEntries(Func<EngineOwners.CoreGameEngineSharedSlashOffersRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSharedSlashOffersEntries Create(Func<EngineOwners.CoreGameEngineSharedSlashOffersRuntime> resolve) => new(resolve);
}
