#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSlashTargetPenaltyPaymentsRuntime))]
internal sealed partial class CoreGameEngineSlashTargetPenaltyPaymentsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashTargetPenaltyPaymentsRuntime> _resolve;
    private CoreGameEngineSlashTargetPenaltyPaymentsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashTargetPenaltyPaymentsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSlashTargetPenaltyPaymentsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashTargetPenaltyPaymentsRuntime> resolve) => new(resolve);
}
