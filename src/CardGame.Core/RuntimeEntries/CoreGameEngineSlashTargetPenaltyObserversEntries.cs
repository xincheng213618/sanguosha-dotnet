#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineSlashTargetPenaltyObserversRuntime))]
internal sealed partial class CoreGameEngineSlashTargetPenaltyObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineSlashTargetPenaltyObserversRuntime> _resolve;
    private CoreGameEngineSlashTargetPenaltyObserversEntries(Func<EngineOwners.CoreGameEngineSlashTargetPenaltyObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSlashTargetPenaltyObserversEntries Create(Func<EngineOwners.CoreGameEngineSlashTargetPenaltyObserversRuntime> resolve) => new(resolve);
}
