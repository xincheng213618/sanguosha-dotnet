#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramActualPlaySlashLossEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramActualPlaySlashLossRuntime> _resolve;
    private CoreProgramActualPlaySlashLossEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramActualPlaySlashLossRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramActualPlaySlashLossEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramActualPlaySlashLossRuntime> resolve) => new(resolve);
}
