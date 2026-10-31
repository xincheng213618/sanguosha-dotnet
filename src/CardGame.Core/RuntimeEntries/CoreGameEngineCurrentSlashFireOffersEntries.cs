#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCurrentSlashFireOffersRuntime))]
internal sealed partial class CoreGameEngineCurrentSlashFireOffersEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentSlashFireOffersRuntime> _resolve;
    private CoreGameEngineCurrentSlashFireOffersEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentSlashFireOffersRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCurrentSlashFireOffersEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentSlashFireOffersRuntime> resolve) => new(resolve);
}
