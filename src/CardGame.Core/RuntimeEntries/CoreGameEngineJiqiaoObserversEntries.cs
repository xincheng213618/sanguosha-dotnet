#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineJiqiaoObserversRuntime))]
internal sealed partial class CoreGameEngineJiqiaoObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineJiqiaoObserversRuntime> _resolve;
    private CoreGameEngineJiqiaoObserversEntries(Func<EngineOwners.CoreGameEngineJiqiaoObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineJiqiaoObserversEntries Create(Func<EngineOwners.CoreGameEngineJiqiaoObserversRuntime> resolve) => new(resolve);
}
