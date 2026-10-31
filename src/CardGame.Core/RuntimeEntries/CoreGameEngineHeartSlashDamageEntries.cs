#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHeartSlashDamageRuntime))]
internal sealed partial class CoreGameEngineHeartSlashDamageEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHeartSlashDamageRuntime> _resolve;
    private CoreGameEngineHeartSlashDamageEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHeartSlashDamageRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineHeartSlashDamageEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHeartSlashDamageRuntime> resolve) => new(resolve);
}
