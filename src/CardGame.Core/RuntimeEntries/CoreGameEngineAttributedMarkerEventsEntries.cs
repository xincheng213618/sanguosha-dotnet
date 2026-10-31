#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineAttributedMarkerEventsRuntime))]
internal sealed partial class CoreGameEngineAttributedMarkerEventsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineAttributedMarkerEventsRuntime> _resolve;
    private CoreGameEngineAttributedMarkerEventsEntries(Func<EngineOwners.CoreGameEngineAttributedMarkerEventsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineAttributedMarkerEventsEntries Create(Func<EngineOwners.CoreGameEngineAttributedMarkerEventsRuntime> resolve) => new(resolve);
}
