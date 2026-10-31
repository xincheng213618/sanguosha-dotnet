#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFinalTargetSlashRuntime))]
internal sealed partial class CoreGameEngineFinalTargetSlashEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFinalTargetSlashRuntime> _resolve;
    private CoreGameEngineFinalTargetSlashEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFinalTargetSlashRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineFinalTargetSlashEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFinalTargetSlashRuntime> resolve) => new(resolve);
}
