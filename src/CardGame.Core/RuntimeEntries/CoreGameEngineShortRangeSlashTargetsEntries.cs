#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShortRangeSlashTargetsRuntime))]
internal sealed partial class CoreGameEngineShortRangeSlashTargetsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShortRangeSlashTargetsRuntime> _resolve;
    private CoreGameEngineShortRangeSlashTargetsEntries(Func<EngineOwners.CoreGameEngineShortRangeSlashTargetsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShortRangeSlashTargetsEntries Create(Func<EngineOwners.CoreGameEngineShortRangeSlashTargetsRuntime> resolve) => new(resolve);
}
