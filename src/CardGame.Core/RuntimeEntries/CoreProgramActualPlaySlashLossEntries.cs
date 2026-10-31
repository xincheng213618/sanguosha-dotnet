#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreProgramActualPlaySlashLossRuntime))]
internal sealed partial class CoreProgramActualPlaySlashLossEntries
{
    private readonly Func<EngineOwners.CoreProgramActualPlaySlashLossRuntime> _resolve;
    private CoreProgramActualPlaySlashLossEntries(Func<EngineOwners.CoreProgramActualPlaySlashLossRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramActualPlaySlashLossEntries Create(Func<EngineOwners.CoreProgramActualPlaySlashLossRuntime> resolve) => new(resolve);
}
