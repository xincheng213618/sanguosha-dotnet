#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineLifetimeSlashUsesRuntime))]
internal sealed partial class CoreGameEngineLifetimeSlashUsesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLifetimeSlashUsesRuntime> _resolve;
    private CoreGameEngineLifetimeSlashUsesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLifetimeSlashUsesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineLifetimeSlashUsesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLifetimeSlashUsesRuntime> resolve) => new(resolve);
}
