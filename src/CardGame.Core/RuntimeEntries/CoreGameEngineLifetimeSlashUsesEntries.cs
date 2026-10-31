#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineLifetimeSlashUsesRuntime))]
internal sealed partial class CoreGameEngineLifetimeSlashUsesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineLifetimeSlashUsesRuntime> _resolve;
    private CoreGameEngineLifetimeSlashUsesEntries(Func<EngineOwners.CoreGameEngineLifetimeSlashUsesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineLifetimeSlashUsesEntries Create(Func<EngineOwners.CoreGameEngineLifetimeSlashUsesRuntime> resolve) => new(resolve);
}
