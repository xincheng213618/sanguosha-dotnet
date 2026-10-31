#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSlashTargetPenaltyObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashTargetPenaltyObserversRuntime> _resolve;
    private CoreGameEngineSlashTargetPenaltyObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashTargetPenaltyObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSlashTargetPenaltyObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashTargetPenaltyObserversRuntime> resolve) => new(resolve);
}
