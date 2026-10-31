#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreProgramActualPlaySlashLossRuntime))]
internal sealed partial class CoreProgramActualPlaySlashLossEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramActualPlaySlashLossRuntime> _resolve;
    private CoreProgramActualPlaySlashLossEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramActualPlaySlashLossRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramActualPlaySlashLossEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramActualPlaySlashLossRuntime> resolve) => new(resolve);
}
