#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineCurrentEnhancementSlashRedirectsRuntime))]
internal sealed partial class CoreGameEngineCurrentEnhancementSlashRedirectsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineCurrentEnhancementSlashRedirectsRuntime> _resolve;
    private CoreGameEngineCurrentEnhancementSlashRedirectsEntries(Func<EngineOwners.CoreGameEngineCurrentEnhancementSlashRedirectsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCurrentEnhancementSlashRedirectsEntries Create(Func<EngineOwners.CoreGameEngineCurrentEnhancementSlashRedirectsRuntime> resolve) => new(resolve);
}
