#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineFinalTargetSlashRuntime))]
internal sealed partial class CoreGameEngineFinalTargetSlashEntries
{
    private readonly Func<EngineOwners.CoreGameEngineFinalTargetSlashRuntime> _resolve;
    private CoreGameEngineFinalTargetSlashEntries(Func<EngineOwners.CoreGameEngineFinalTargetSlashRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineFinalTargetSlashEntries Create(Func<EngineOwners.CoreGameEngineFinalTargetSlashRuntime> resolve) => new(resolve);
}
