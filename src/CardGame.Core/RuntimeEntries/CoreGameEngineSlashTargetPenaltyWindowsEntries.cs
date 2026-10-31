#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineSlashTargetPenaltyWindowsRuntime))]
internal sealed partial class CoreGameEngineSlashTargetPenaltyWindowsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineSlashTargetPenaltyWindowsRuntime> _resolve;
    private CoreGameEngineSlashTargetPenaltyWindowsEntries(Func<EngineOwners.CoreGameEngineSlashTargetPenaltyWindowsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSlashTargetPenaltyWindowsEntries Create(Func<EngineOwners.CoreGameEngineSlashTargetPenaltyWindowsRuntime> resolve) => new(resolve);
}
