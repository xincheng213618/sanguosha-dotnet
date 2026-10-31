#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineSlashTargetPenaltyPaymentsRuntime))]
internal sealed partial class CoreGameEngineSlashTargetPenaltyPaymentsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineSlashTargetPenaltyPaymentsRuntime> _resolve;
    private CoreGameEngineSlashTargetPenaltyPaymentsEntries(Func<EngineOwners.CoreGameEngineSlashTargetPenaltyPaymentsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSlashTargetPenaltyPaymentsEntries Create(Func<EngineOwners.CoreGameEngineSlashTargetPenaltyPaymentsRuntime> resolve) => new(resolve);
}
