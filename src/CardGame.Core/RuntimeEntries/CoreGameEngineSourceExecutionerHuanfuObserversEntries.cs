#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineSourceExecutionerHuanfuObserversRuntime))]
internal sealed partial class CoreGameEngineSourceExecutionerHuanfuObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineSourceExecutionerHuanfuObserversRuntime> _resolve;
    private CoreGameEngineSourceExecutionerHuanfuObserversEntries(Func<EngineOwners.CoreGameEngineSourceExecutionerHuanfuObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSourceExecutionerHuanfuObserversEntries Create(Func<EngineOwners.CoreGameEngineSourceExecutionerHuanfuObserversRuntime> resolve) => new(resolve);
}
