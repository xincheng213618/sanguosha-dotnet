#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSlashTargetPenaltyWindowsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashTargetPenaltyWindowsRuntime> _resolve;
    private CoreGameEngineSlashTargetPenaltyWindowsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashTargetPenaltyWindowsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSlashTargetPenaltyWindowsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashTargetPenaltyWindowsRuntime> resolve) => new(resolve);
}
