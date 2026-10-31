#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCurrentEnhancementSlashRedirectsRuntime))]
internal sealed partial class CoreGameEngineCurrentEnhancementSlashRedirectsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentEnhancementSlashRedirectsRuntime> _resolve;
    private CoreGameEngineCurrentEnhancementSlashRedirectsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentEnhancementSlashRedirectsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCurrentEnhancementSlashRedirectsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentEnhancementSlashRedirectsRuntime> resolve) => new(resolve);
}
