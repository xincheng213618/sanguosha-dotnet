#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineCurrentSlashFireOffersRuntime))]
internal sealed partial class CoreGameEngineCurrentSlashFireOffersEntries
{
    private readonly Func<EngineOwners.CoreGameEngineCurrentSlashFireOffersRuntime> _resolve;
    private CoreGameEngineCurrentSlashFireOffersEntries(Func<EngineOwners.CoreGameEngineCurrentSlashFireOffersRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCurrentSlashFireOffersEntries Create(Func<EngineOwners.CoreGameEngineCurrentSlashFireOffersRuntime> resolve) => new(resolve);
}
