#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShortRangeSlashTargetsRuntime))]
internal sealed partial class CoreGameEngineShortRangeSlashTargetsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShortRangeSlashTargetsRuntime> _resolve;
    private CoreGameEngineShortRangeSlashTargetsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShortRangeSlashTargetsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShortRangeSlashTargetsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShortRangeSlashTargetsRuntime> resolve) => new(resolve);
}
