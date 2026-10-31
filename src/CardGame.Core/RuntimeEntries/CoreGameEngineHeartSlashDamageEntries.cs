#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineHeartSlashDamageRuntime))]
internal sealed partial class CoreGameEngineHeartSlashDamageEntries
{
    private readonly Func<EngineOwners.CoreGameEngineHeartSlashDamageRuntime> _resolve;
    private CoreGameEngineHeartSlashDamageEntries(Func<EngineOwners.CoreGameEngineHeartSlashDamageRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineHeartSlashDamageEntries Create(Func<EngineOwners.CoreGameEngineHeartSlashDamageRuntime> resolve) => new(resolve);
}
