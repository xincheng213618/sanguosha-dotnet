#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAttributedMarkerEventsRuntime))]
internal sealed partial class CoreGameEngineAttributedMarkerEventsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAttributedMarkerEventsRuntime> _resolve;
    private CoreGameEngineAttributedMarkerEventsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAttributedMarkerEventsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineAttributedMarkerEventsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAttributedMarkerEventsRuntime> resolve) => new(resolve);
}
